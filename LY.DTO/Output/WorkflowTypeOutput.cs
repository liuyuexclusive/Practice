using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Output
{
    public class WorkflowTypeOutput
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class WorkflowTypeNodeOutput
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Auditors { get; set; }
    }
}
