using System;
using System.Threading.Tasks;

namespace LY.Common
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task<T> Handle<T>(T entity,TEvent eventObj) where T : Entity, new();

        Task<object>HandleObj(Type type, object obj, TEvent eventObj);
    }
}
