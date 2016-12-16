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
        private readonly IRepository<User> _userRepo;
        private readonly ILogger<UserService> _logger;
        public UserService(IRepository<User> userRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
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
            _logger.LogDebug("测试service记日志成功");
        }
    }
}