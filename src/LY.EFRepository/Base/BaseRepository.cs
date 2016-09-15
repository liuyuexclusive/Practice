using LY.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using LY.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class BaseRepository<Tkey, TEntity> : IBaseRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        private readonly IBaseUnitOfWork<Tkey> _unitOfWork;
        private readonly DbContext _dbContext;

        [DebuggerStepThrough]
        public BaseRepository(IBaseUnitOfWork<Tkey> unitOfWork, DbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        protected IBaseUnitOfWork<Tkey> UnitOfWork => _unitOfWork;

        protected DbContext DbContext => _dbContext;

        public DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public virtual bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.AsExpandable().Any(expression);
        }

        public virtual TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return Entities.AsExpandable().Max(selector);
        }

        public virtual TResult Max<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector)
        {
            return Entities.AsExpandable().Where(expression).Max(selector);
        }

        public virtual TEntity Get(Tkey id)
        {
            return Entities.FirstOrDefault(a => a.ID.Equals(id));
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.AsExpandable().FirstOrDefault(expression);
        }

        public virtual TEntity Get<TProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TProperty>> path)
        {
            return Entities.Include(path).AsExpandable().FirstOrDefault(expression);
        }

        public virtual IList<TEntity> Query()
        {
            return Entities.ToList();
        }

        public virtual IList<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.AsExpandable().Where(expression).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> @orderby, bool isAscending)
        {
            var entities = Entities.AsExpandable().Where(expression);
            if (isAscending)
            {
                return entities.OrderBy(@orderby).ToList();
            }
            return entities.OrderByDescending(@orderby).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, int pageIndex, int pageSize, out int totalRecordCount)
        {
            totalRecordCount = 0;
            var query = Entities.AsExpandable().Where(expression);

            if (isAscending)
            {
                query = query.OrderBy(keySelector);
            }
            else
            {
                query = query.OrderByDescending(keySelector);
            }

            if (pageSize > 0) //分页查询
            {
                pageIndex--;
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                totalRecordCount = query.Count();
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return query.ToList();
        }

        public virtual void Add(TEntity entity)
        {
            _unitOfWork.RegisterAdded(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _unitOfWork.RegisterChangeded(entity);
        }

        public virtual void Delete(Tkey id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                _unitOfWork.RegisterRemoved(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _unitOfWork.RegisterRemoved(entity);
        }

        public virtual void AddOnDemand(TEntity entity)
        {
            Entities.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void UpdateOnDemand(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public virtual void DeleteOnDemand(Tkey id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
        }

        public virtual void DeleteOnDemand(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }
    }
}
