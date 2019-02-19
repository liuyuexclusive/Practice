using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Domain.Sys
{
    public class Sys_Workflow : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public int TypeID { get; set; }
        public int CurrentNodeID { get; set; }
    }
}
