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
    public interface IBaseRepository<Tkey, TEntity> : IBaseQueryRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="tkey"></param>
        /// <returns></returns>
        TEntity Get(Tkey tkey);

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

    }
}
