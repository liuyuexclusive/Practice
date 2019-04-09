using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LY.Common
{
    /// <summary>
    ///  代表一个资源库。
    /// </summary>
    /// <typeparam name="TEventEntity">实体类型。</typeparam>
    public interface IEventRepository<TEventEntity> : IQueryRepository<TEventEntity>  where TEventEntity : IEvent
    {
        void Add(IDictionary<string,object> dic);
        void Update(int aggregatedID, IDictionary<string,object> dic);
        void Delete(int[] aggregatedIDs);
        Task<T> Play<T>(int aggregatedID) where T : Entity, new();
        Task<IEnumerable<T>> Play<T>(Expression<Func<TEventEntity, bool>> expression) where T : Entity, new();
    }
}
