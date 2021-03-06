﻿using LY.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Domain.Sys
{
    public class Sys_WorkflowTypeNodeAuditor : Entity, IEntityCacheable
    {
        public int NodeID { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public Sys_WorkflowTypeNode Node { get; set; }
    }
}
