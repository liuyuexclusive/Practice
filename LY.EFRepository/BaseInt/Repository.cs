using LY.Domain;
using Microsoft.EntityFrameworkCore;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class Repository<TEntity> : BaseRepository<int, TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public Repository(IUnitOfWork unitOfWork, DbContext dbContext) : base(unitOfWork, dbContext)
        {
        }
    }
}
