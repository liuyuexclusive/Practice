using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LY.Service.Sys
{
    public class UserService
    {
        private readonly IRepository<User> _userRepo;
        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }
        
        public void Register(User user)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                throw new BusinessException("” œ‰≤ªƒ‹Œ™ø’");
            }
        }
    }
}