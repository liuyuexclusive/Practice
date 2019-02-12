using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Input
{
    public class WorkflowTypeNodeAddOrUpdateInput
    {
        public string Name { get; set; }

        public IEnumerable<int> Auditors { get; set; }
    }
}
