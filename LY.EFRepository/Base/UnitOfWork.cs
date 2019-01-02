using LY.Common;
using LY.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Linq;
using System.Reflection;

namespace LY.EFRepository
{
    [DebuggerStepThrough]
    public class UnitOfWork : IUnitOfWork
    {
        public LYMasterContext Context { get; set; }

        public IDistributedCache Cache { get; set; }

        public UnitOfWork()
        {
        }

        public virtual void RegisterAdded(Entity entity)
        {
            Context.Set<Entity>().Add(entity);
        }

        public virtual void RegisterUpdated(Entity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void RegisterDeleted(Entity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            var changeTypeNames = Context.ChangeTracker.Entries().Where(x =>
    x.State == EntityState.Added
    || x.State == EntityState.Modified
    || x.State == EntityState.Deleted
).Select(x => x.Metadata.Name).Distinct().ToArray();
            Context.SaveChanges();
            if (!changeTypeNames.IsNullOrEmpty())
            {
                var assembly = Assembly.Load(new AssemblyName("LY.Domain"));
                var types = assembly.ExportedTypes;
                foreach (var typeName in changeTypeNames)
                {
                    var grnericType = types.FirstOrDefault(x => x.FullName == typeName);
                    if (!typeof(IEntityCacheable).IsAssignableFrom(grnericType))
                    {
                        continue;
                    }
                    var type = typeof(Repository<>).MakeGenericType(grnericType);
                    var value = type.GetMethod("GetAll").Invoke(Activator.CreateInstance(type, this, Context),null);

                    Cache.SetStringAsync(typeName, JsonConvert.SerializeObject(value)).Wait();
                }
            }
        }
    }
}
