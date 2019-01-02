using System;

namespace LY.Domain.Sys
{
    public class Sys_RoleUserMapping : Entity, IEntityCacheable
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { set; get; }

        public Sys_Role Role { get; set; }
        public Sys_User User { get; set; }
    }
}
