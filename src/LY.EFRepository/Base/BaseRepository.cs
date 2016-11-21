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

        private IQueryable<TEntity> GetPathQuery(params NavigationPropertyPath<TEntity>[] paths)
        {
            var result = Entities.AsExpandable();
            if (!paths.IsNullOrEmpty())
            {
                foreach (var path in paths)
                {
                    var includData = (IIncludableQueryable<TEntity, object>)result.Include(path.Include);
                    result = includData;
                    foreach (var thenIncludes in path.ThenIncludes)
                    {
                        result = includData.ThenInclude(thenIncludes);
                    }
                }
            }
            return result;
        }

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

        public virtual TEntity Get(Tkey id, params NavigationPropertyPath<TEntity>[] paths)
        {
            
            return GetPathQuery(paths).FirstOrDefault(a => a.ID.Equals(id));
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression, params NavigationPropertyPath<TEntity>[] paths)
        {
            return GetPathQuery(paths).FirstOrDefault(expression);
        }

        public virtual IList<TEntity> Query(params NavigationPropertyPath<TEntity>[] paths)
        {
            return GetPathQuery(paths).ToList();
        }

        public virtual IList<TEntity> Query(Expression<Func<TEntity, bool>> expression, params NavigationPropertyPath<TEntity>[] paths)
        {
            return GetPathQuery(paths).Where(expression).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> @orderby, bool isAscending, params NavigationPropertyPath<TEntity>[] paths)
        {
            var query = GetPathQuery(paths).Where(expression);
            if (isAscending)
            {
                return query.OrderBy(@orderby).ToList();
            }
            return query.OrderByDescending(@orderby).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, int pageIndex, int pageSize, out int totalRecordCount, params NavigationPropertyPath<TEntity>[] paths)
        {
            totalRecordCount = 0;
            var query = GetPathQuery(paths).Where(expression);

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

        public virtual void AddOnDemand(TEntity entity)
        {
            _unitOfWork.RegisterAdded(entity);
            _unitOfWork.Commit();
        }

        public virtual void UpdateOnDemand(TEntity entity)
        {
            _unitOfWork.RegisterUpdated(entity);
            _unitOfWork.Commit();
        }

        public virtual void DeleteOnDemand(Tkey id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                _unitOfWork.RegisterDeleted(entity);
                _unitOfWork.Commit();
            }
        }

        public virtual void DeleteOnDemand(TEntity entity)
        {
            _unitOfWork.RegisterDeleted(entity);
            _unitOfWork.Commit();
        }
    }
}
