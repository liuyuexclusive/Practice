using LY.Domain;
using Microsoft.EntityFrameworkCore;
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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork _unitOfWork;
        LYMasterContext _context;

        public Repository(IUnitOfWork unitOfWork, LYMasterContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public virtual IQueryable<TEntity> Queryable => _context.Set<TEntity>();


        public virtual TEntity Get(int id)
        {
            return Queryable.FirstOrDefault(x => x.ID.Equals(id));
        }

        public virtual IList<TEntity> GetAll()
        {
            return Queryable.ToList();
        }

        public virtual void Add(TEntity entity)
        {
            _unitOfWork.RegisterAdded(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _unitOfWork.RegisterUpdated(entity);
        }

        public virtual void Delete(int id)
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

        public virtual void Delete(Expression<Func<TEntity, bool>> expression)
        {
            var list = Queryable.Where(expression);
            foreach (var item in list)
            {
                _unitOfWork.RegisterDeleted(item);
            }
        }
    }
}
