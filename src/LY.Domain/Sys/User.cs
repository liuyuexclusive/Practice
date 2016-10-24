using LY.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LY.Domain.Sys
{
    public class User : Entity
    {
        [Required(ErrorMessage = "邮箱不能为空")]
        public string Email { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastOn { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleUserMapping> RoleUserMappingList { get; set; }
    }
}
