using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Input
{
    public class WorkflowTypeAddOrUpdateInput
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public IList<WorkflowTypeNodeAddOrUpdateInput> Nodes { get; set; }
    }
}
