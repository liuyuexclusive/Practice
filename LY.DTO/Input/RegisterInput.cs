using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO
{
    public class RegisterInput
    {
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
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
    }
}
