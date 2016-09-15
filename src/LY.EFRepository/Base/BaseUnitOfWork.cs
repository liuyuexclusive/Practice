using LY.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace LY.EFRepository
{
    [DebuggerStepThrough]
    public class BaseUnitOfWork<Tkey> : IBaseUnitOfWork<Tkey>
    {
        private readonly DbContext _dbContext;

        public BaseUnitOfWork(LYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void RegisterAdded(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Set<BaseEntity<Tkey>>().Add(entityBase);
        }

        public virtual void RegisterChangeded(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Modified;
        }

        public virtual void RegisterRemoved(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
