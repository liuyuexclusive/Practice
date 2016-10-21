using System;
using System.Collections;
using System.Collections.Generic;

namespace LY.Domain.Sys
{
    public interface IRoleRepo : IRepository<Role>
    {
        IList<Role> QueryInclude();
    }

    public class Role : Entity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { set; get; }


        public IList<RoleUserMapping> RoleUserMappingList { get; set; }
    }
}
