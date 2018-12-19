using LY.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace LY.EFRepository
{
    public class BaseUnitOfWork<Tkey> : IBaseUnitOfWork<Tkey>
    {
        private readonly LYMasterContext _dbContext;

        public BaseUnitOfWork(LYMasterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void RegisterAdded(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Set<BaseEntity<Tkey>>().Add(entityBase);
        }

        public virtual void RegisterUpdated(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Modified;
        }

        public virtual void RegisterDeleted(BaseEntity<Tkey> entityBase)
        {
            _dbContext.Entry(entityBase).State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
