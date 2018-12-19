using LY.Domain;
using Microsoft.EntityFrameworkCore;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class QueryRepository<TEntity> : BaseQueryRepository<int, TEntity>, IQueryRepository<TEntity> where TEntity : Entity
    {
        public QueryRepository(LYSlaveContext dbContext) : base(dbContext)
        {
        }
    }
}
