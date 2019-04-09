using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.Common
{
    /// <summary>
    /// 事件
    /// </summary>
    public interface IEvent:IEntity
    {
        int AggregatedID { get; set; }
        EventType Type { get; set; }
        JsonObject<IDictionary<string, object>> Data { get; set; }
    }

    public enum EventType
    { 
        Create=1,
        Update,
        Delete
    }
}
