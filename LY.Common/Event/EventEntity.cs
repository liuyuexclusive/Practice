using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.Common
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class EventEntity : Entity, IEvent
    {
        public int AggregatedID { get; set; }
        public EventType Type { get; set; }
        public JsonObject<IDictionary<string,object>> Data { get; set; } 
    }
}
