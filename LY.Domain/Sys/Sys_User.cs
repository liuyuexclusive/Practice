using LY.Common;
using LY.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.Domain.Sys
{
    public class Sys_User : Entity, IEntityCacheable
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
        /// 盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastOn { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public ICollection<Sys_RoleUserMapping> RoleUserMappingList { get; set; }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        public Sys_User Register(Sys_User user)
        {
            if (user == null)
            {
                throw new BusinessException("参数为空");
            }
            ;
            user.Email.ValidateEmail();
            user.Mobile.ValidateMobile();
            if (user.Password.IsNullOrEmpty())
            {
                throw new BusinessException("密码不能为空");
            }

            user.Salt = Guid.NewGuid().ToString().ToLower();
            user.Password = user.Password.Encrypt(user.Salt);
            if (user.Name.IsNullOrEmpty())
            {
                user.Name = user.Email;
            }

            return user;
        }
    }
}
