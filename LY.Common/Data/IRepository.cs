﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LY.Common
{
    /// <summary>
    ///  代表一个资源库。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public interface IRepository<TEntity> : IQueryRepository<TEntity>  where TEntity : Entity
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="tkey"></param>
        /// <returns></returns>
        TEntity Get(int tkey);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// 新增实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Add(TEntity entity);

        /// <summary>
        /// 更新实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Update(TEntity entity);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="id">实体主键。</param>
        void Delete(int id);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="expression">条件表达式</param>
        void Delete(Expression<Func<TEntity, bool>> expression);
    }
}
