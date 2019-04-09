using LY.Common;
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

        public virtual void RegisterAdded<T>(T entity) where T : Entity
        {
            Context.Set<Entity>().Add(entity);
        }

        public virtual void RegisterUpdated(IEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void RegisterDeleted(IEntity entity)
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
                    var repoType = typeof(Repository<>).MakeGenericType(grnericType);
                    var value = repoType.GetMethod("GetAll").Invoke(Activator.CreateInstance(repoType, this, Context), null);

                    Cache.SetStringAsync(typeName, JsonConvert.SerializeObject(value)).Wait();
                }
            }
        }
    }
}
