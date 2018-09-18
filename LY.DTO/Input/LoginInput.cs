using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO
{
    public class LoginInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
    }
}
