using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Domain.Sys
{
    public class Sys_WorkflowTypeNodeRelation : Entity, IEntityCacheable
    {
        public int NodeID { get; set; }

        public int PreNodeID { get; set; }

        public Sys_WorkflowTypeNode Node { get; set; }
        public Sys_WorkflowTypeNode PreNode { get; set; }
    }
}
