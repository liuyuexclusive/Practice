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

        public Sys_WorkflowType Type { get; set; }
    }
}
