using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LY.DTO;
using Mapster;
using LY.Common.Utils;
using Microsoft.Extensions.Caching.Distributed;
using LY.DTO.Input;

namespace LY.Service.Sys
{
    public class UserService
    {
        public ILogger<UserService> Logger { get; set; }
        public IRepository<Sys_User> UserRepo { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IDistributedCache DistributedCache { get; set; }

        public UserService()
        {

        }

        public void Delete(BaseDeleteInput value)
        {
            var userList = UserRepo.Queryable.Where(x => value.IDs.Contains(x.ID));
            if (userList.IsNullOrEmpty())
            {
                throw new BusinessException("指定的数据不存在");
            }
            foreach (var item in userList)
            {
                UserRepo.Delete(item);
            }
            UnitOfWork.Commit();
        }

        public void Register(RegisterInput value)
        {
            if (value == null)
            {
                throw new BusinessException("参数不能为空");
            }
            string validateCode = DistributedCache.GetString(value.Email);
            if (validateCode.IsNullOrEmpty())
            {
                throw new BusinessException("验证码已过期，请重新获取");
            }
            if (value.ValidateCode != validateCode)
            {
                throw new BusinessException("验证码错误");
            }

            Sys_User newUser = value.Adapt<Sys_User>();
            var user = newUser.Register(newUser);
            if (UserRepo.Queryable.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                throw new BusinessException("用户已存在");
            }
            
            UserRepo.Add(user);
            UnitOfWork.Commit();
        }

        public Sys_User Login(LoginInput value,out string token)
        {
            if (value == null)
            {
                throw new BusinessException("参数不能为空");
            }
            value.Email.ValidateEmail();
            var user = UserRepo.Queryable.FirstOrDefault(x => x.Email == value.Email);
            if (user == null)
            {
                throw new BusinessException("该用户不存在");
            }
            if (value.Password.Encrypt(user.Salt) != user.Password)
            {
                throw new BusinessException("密码错误");
            }
            token = JwtUtil.GenJwtToken(user.Name, user.Email);
            user.LastOn = DateTime.Now;
            UserRepo.Update(user);
            UnitOfWork.Commit();
            return user;
        }

        public async Task GetValidateCode(string email)
        {
            string validateCode = new Random().Next(1000, 9999).ToString();
            DistributedCache.SetString(email, validateCode, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) });
            await MailUtil.SendMailAsync(email, validateCode);           
        }
    }
}