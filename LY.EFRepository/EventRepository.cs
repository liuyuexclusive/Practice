using LY.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEventEntity">实体类型。</typeparam>
    public class EventRepository<TEventEntity> : IEventRepository<TEventEntity> where TEventEntity : EventEntity, new()
    {
        IUnitOfWork _unitOfWork;
        LYMasterContext _context;
        public IEventHandler<TEventEntity> _evnetHandler;

        public EventRepository(IUnitOfWork unitOfWork, LYMasterContext context, IEventHandler<TEventEntity> evnetHandler)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _evnetHandler = evnetHandler;
        }

        public virtual IQueryable<TEventEntity> Queryable => _context.Set<TEventEntity>();

        public virtual void Add(IDictionary<string,object> dic)
        {
            if(dic.IsNullOrEmpty())
            {
                return;
            }

            int? max = Queryable.OrderByDescending(x => x.AggregatedID).FirstOrDefault()?.AggregatedID;
            TEventEntity entity = new TEventEntity
            {
                AggregatedID = (max??0) + 1,
                Type = EventType.Create,
                Data = new JsonObject<IDictionary<string,object>>(dic)
            };
            _unitOfWork.RegisterAdded(entity);
        }

        public virtual void Update(int aggregationID, IDictionary<string,object> dic)
        {
            if (dic.IsNullOrEmpty())
            {
                return;
            }

            Type type = AssemblyUtil.DomainTypes
    .Where(x => typeof(IEntity).IsAssignableFrom(x))
    .FirstOrDefault(x => x.Name == typeof(TEventEntity).Name.Replace("Event", ""));

            var obj = Play(type,aggregationID).Result;

            if (obj == null)
            {
                throw new BusinessException("缺少已存在的数据");
            }

            dic = dic.Where(x => x.Value.Equals(type.GetProperty(x.Key).GetValue(obj)) == false).ToDictionary(x => x.Key, x => x.Value);

            TEventEntity entity = new TEventEntity
            {
                AggregatedID = aggregationID,  
                Type = EventType.Update,
                Data = new JsonObject<IDictionary<string,object>>(dic)
            };
            _unitOfWork.RegisterAdded(entity);
        }

        public virtual void Delete(params int[] aggregationIDs)
        {
            if(!aggregationIDs.IsNullOrEmpty())
            {
                foreach (var aggregationID in aggregationIDs)
                {
                    TEventEntity entity = new TEventEntity
                    {
                        AggregatedID = aggregationID,
                        Type = EventType.Delete
                    };
                    _unitOfWork.RegisterAdded(entity);
                }
            }

        }


        private async Task<object> Play(Type type,int aggregatedID)
        {
            object result = default(Object);

            var eventList = Queryable.Where(x => x.AggregatedID == aggregatedID).ToList();

            foreach(var item in eventList)
            {
                result = await _evnetHandler.HandleObj(type, result, item);
            }
            return result;

        }

        public virtual async Task<T> Play<T>(int aggregatedID) where T : Entity, new()
        {
            T result = default(T);

            var eventList = Queryable.Where(x => x.AggregatedID == aggregatedID).ToList();

            foreach (var item in eventList)
            {
                result = await _evnetHandler.Handle(result, item);
            }
            return result;

        }

        public virtual async Task<IEnumerable<T>> Play<T>(Expression<Func<TEventEntity, bool>> expression) where T : Entity,new()
        {
            IList<T> result = new List<T>();
            var aggregatedList = Queryable.Where(expression).GroupBy(x => x.AggregatedID).Select(x => new
            {
                AggregatedID = x.Key,
                List = x.OrderBy(y => y.ID).ToList()
            }).ToList();
            foreach (var aggregatedRoot in aggregatedList)
            {
                T t = default(T);
                if(!aggregatedRoot.List.IsNullOrEmpty())
                { 
                    foreach(var item in aggregatedRoot.List)
                    {
                       t = await _evnetHandler.Handle(t, item);
                    }
                }
                if (t != default(T))
                {
                    result.Add(t);
                }
            }
            return result;
        }
    }
}
