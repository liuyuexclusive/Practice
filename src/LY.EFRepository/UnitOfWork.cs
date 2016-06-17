using LY.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace LY.EFRepository
{
    [DebuggerStepThrough]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(LYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void RegisterAdded(EntityBase entityBase)
        {
            _dbContext.Set<EntityBase>().Add(entityBase);
        }

        public virtual void RegisterChangeded(EntityBase entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Modified;
        }

        public virtual void RegisterRemoved(EntityBase entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
