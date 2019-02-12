using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Domain.Sys
{
    public class Sys_WorkflowTypeNode : Entity, IEntityCacheable
    {
        public int TypeID { get; set; }

        public string Name { get; set; }

        public int SortIndex { get; set; }

        public int? PreNodeID { get; set; }

        
        public Sys_WorkflowTypeNode PreNode { get; set; }

        [JsonIgnore]
        public Sys_WorkflowTypeNode NextNode { get; set; }

        [JsonIgnore]
        public Sys_WorkflowType Type { get; set; }

        
        public IList<Sys_WorkflowTypeNodeAuditor> AuditorList { get; set; }
    }
}
