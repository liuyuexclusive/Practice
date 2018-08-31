using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LY.Service.Sys
{
    public class UserService
    {
        public ILogger<UserService> Logger { get; set; }
        public IRepository<User> UserRepo { get; set; }
        public UserService()
        {

        }
        
        public void Register(User user)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                throw new BusinessException("邮箱不能为空");
            }
        }
        public void Test()
        {
            Logger.LogDebug("测试service记日志成功");
        }

        public object GetUser()
        {
            return UserRepo.Queryable.Select(x => new
            {
                Name = x.Name,
                Age = 11,
                Gender = "不男不女"
            }).ToList();
        }
    }
}