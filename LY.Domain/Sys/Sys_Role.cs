using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LY.Domain.Sys
{
    public class Sys_Role : Entity, IEntityCacheable
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { set; get; }


        public IList<Sys_RoleUserMapping> RoleUserMappingList { get; set; }
    }
}
