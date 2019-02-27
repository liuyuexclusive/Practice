using LY.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class QueryRepository<TEntity> :  IQueryRepository<TEntity> where TEntity : Entity
    {
        public LYSlaveContext Context { get; set; }

        public QueryRepository()
        {

        }

        public virtual IQueryable<TEntity> Queryable => Context.Set<TEntity>().AsNoTracking();

        public virtual TEntity Get(int id)
        {
            return Queryable.FirstOrDefault(x => x.ID.Equals(id));
        }
    }
}
