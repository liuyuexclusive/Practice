using System;

namespace LY.Domain.Sys
{
    public class RoleUserMapping : Entity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { set; get; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
