using System;
using System.Collections;
using System.Collections.Generic;

namespace LY.Domain.Sys
{
    public class Role : EntityBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { set; get; }


        public virtual IList<RoleUserMapping> RoleUserMappingList { get; set; }
    }
}
