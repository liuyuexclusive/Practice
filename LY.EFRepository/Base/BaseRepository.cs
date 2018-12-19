using LY.Common;
using LY.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class BaseRepository<Tkey, TEntity> : IBaseRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        private readonly IBaseUnitOfWork<Tkey> _unitOfWork;

        private readonly LYMasterContext _dbContext;

        public BaseRepository(IBaseUnitOfWork<Tkey> unitOfWork, LYMasterContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        protected IBaseUnitOfWork<Tkey> UnitOfWork => _unitOfWork;

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public virtual IQueryable<TEntity> Queryable => Entities.AsQueryable<TEntity>();

        public virtual TEntity Get(Tkey id)
        {
            return Queryable.FirstOrDefault(x => x.ID.Equals(id));
        }

        public virtual void Add(TEntity entity)
        {
            _unitOfWork.RegisterAdded(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _unitOfWork.RegisterUpdated(entity);
        }

        public virtual void Delete(Tkey id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                _unitOfWork.RegisterDeleted(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _unitOfWork.RegisterDeleted(entity);
        }

    }
}
