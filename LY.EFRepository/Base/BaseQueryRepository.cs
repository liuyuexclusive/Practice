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
    public class BaseQueryRepository<Tkey, TEntity> : IBaseQueryRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        private readonly LYSlaveContext _dbContext;

        public BaseQueryRepository(LYSlaveContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public virtual IQueryable<TEntity> Queryable => Entities.AsQueryable<TEntity>();

        public virtual TEntity Get(Tkey id)
        {
            return Queryable.FirstOrDefault(x => x.ID.Equals(id));
        }
    }
}
