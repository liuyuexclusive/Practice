using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LY.Domain
{
    /// <summary>
    ///  代表一个资源库。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public interface IBaseRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        /// <summary>
        /// 检查是否存在满足条件的实体。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>如果存在满足条件的实体，返回true；否则返回false。</returns>
        bool Any(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定属性的最大值。
        /// </summary>
        /// <typeparam name="TResult">属性类型。</typeparam>
        /// <param name="selector">属性选择表达式。</param>
        /// <returns>属性的最大值。</returns>
        TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 获取指定属性的最大值。
        /// </summary>
        /// <typeparam name="TResult">属性类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <param name="selector">属性选择表达式。</param>
        /// <returns>属性的最大值。</returns>
        TResult Max<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <param name="id">实体主键。</param>
        /// <returns>实体对象。</returns>
        TEntity Get(Tkey id, params NavigationPropertyPath<TEntity>[] paths);

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>实体对象。</returns>
        TEntity Get(Expression<Func<TEntity, bool>> expression, params NavigationPropertyPath<TEntity>[] paths);


        /// <summary>
        /// 获取所有实体。
        /// </summary>
        /// <returns>实体列表。</returns>
        IList<TEntity> Query(params NavigationPropertyPath<TEntity>[] paths);


        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query(Expression<Func<TEntity, bool>> expression, params NavigationPropertyPath<TEntity>[] paths);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <typeparam name="TOrderProperty">排序属性。</typeparam>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="orderby">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TOrderProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TOrderProperty>> @orderby, bool isAscending, params NavigationPropertyPath<TEntity>[] paths);


        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TOrderProperty">排序属性。</typeparam>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <param name="pageIndex">当前页索引（从1开始）。</param>
        /// <param name="pageSize">每页显示的记录数（小于等于0时表示不分页）。</param>
        /// <param name="totalRecordCount">符合条件的总记录数。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TOrderProperty>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TOrderProperty>> keySelector, bool isAscending, int pageIndex, int pageSize, out int totalRecordCount, params NavigationPropertyPath<TEntity>[] paths);


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
        void Delete(Tkey id);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 按需新增实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void AddOnDemand(TEntity entity);

        /// <summary>
        /// 按需更新实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void UpdateOnDemand(TEntity entity);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="id">实体主键。</param>
        void DeleteOnDemand(Tkey id);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void DeleteOnDemand(TEntity entity);
    }
}
