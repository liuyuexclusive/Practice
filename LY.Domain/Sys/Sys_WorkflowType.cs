using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Domain.Sys
{
    public class Sys_WorkflowType : Entity, IEntityCacheable
    {
        public string Name { get; set; }
        
        [JsonIgnore]
        public IList<Sys_WorkflowTypeNode> NodeList { get; set; }
    }
}
