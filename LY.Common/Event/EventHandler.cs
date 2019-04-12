using System;
using System.Linq;
using System.Threading.Tasks;
using LY.Common;

namespace LY.Common
{
    public class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent:IEvent
    {
        public async Task<T> Handle<T>(T entity,TEvent eventObj) where T : Entity, new()
        {
            T result = entity;

            switch(eventObj.Type)
            { 
               case EventType.Create:
                    result = new T();
                    result.ID = eventObj.AggregatedID;
                    result.CreatedBy = eventObj.CreatedBy;
                    result.CreatedOn = eventObj.CreatedOn;
                    result.Series = eventObj.Series;
                    SetValueFromEvent(typeof(T), result, eventObj);
                    break;
                case EventType.Update:
                    if (!result.Equals(default(T)))
                    {
                        SetValueFromEvent(typeof(T), result, eventObj);
                    }
                    break;
                case EventType.Delete:
                    result = default(T);
                    break;
            }

            return await Task.FromResult(result);
        }

        public async Task<object> HandleObj(Type type, object obj, TEvent eventObj)
        {
            if (type == null)
            {
                throw new BusinessException("缺少实体类");
            }

            object result = obj;

            switch (eventObj.Type)
            {
                case EventType.Create:
                    result = Activator.CreateInstance(type);
                    type.GetProperty("ID").SetValue(result, eventObj.AggregatedID);
                    type.GetProperty("Series").SetValue(result, eventObj.Series);
                    type.GetProperty("CreatedBy").SetValue(result, eventObj.CreatedBy);
                    type.GetProperty("CreatedOn").SetValue(result, eventObj.CreatedOn);
                    SetValueFromEvent(type, result, eventObj);
                    break;
                case EventType.Update:
                    if (!result.Equals(default(object)))
                    {
                        SetValueFromEvent(type, result, eventObj);
                    }
                    break;
                case EventType.Delete:
                    result = default(object);
                    break;
                default:
                    break;
            }

            return await Task.FromResult(result);

        }

        private void SetValueFromEvent<T>(Type type, object obj, T item) where T : IEvent
        {
            if (item.Data.Object == null)
            {
                return;
            }
            foreach (var dicItem in item.Data.Object)
            {
                var propertyInfo = type.GetProperty(dicItem.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, dicItem.Value);
                }
            }
        }
    }
}
