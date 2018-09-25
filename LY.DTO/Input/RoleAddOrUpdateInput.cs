using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Input
{
    public class RoleAddOrUpdateInput:BaseAddOrUpdateInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
