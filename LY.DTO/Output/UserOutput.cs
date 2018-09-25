using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Output
{
    public class UserOutput
    {
        public int ID { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }

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
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
    }
}
