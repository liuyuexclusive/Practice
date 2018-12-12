using LY.Common;
using LY.Common.Extensions;
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
            token = GetJWTToken(user);
            user.LastOn = DateTime.Now;
            UserRepo.Update(user);
            UnitOfWork.Commit();
            return user;
        }

        private string GetJWTToken(Sys_User user)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                   new Claim(ClaimTypes.Name, user.Name),
                   new Claim(ClaimTypes.Email, user.Email)
            };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.JWT._securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                iss: The issuer of the token，token 是给谁的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            var token = new JwtSecurityToken(
                audience: Const.JWT._audience,
                issuer: Const.JWT._issuer,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task GetValidateCode(string email)
        {
            string validateCode = new Random().Next(1000, 9999).ToString();
            DistributedCache.SetString(email, validateCode, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) });
            await MailUtil.SendMailAsync(email, validateCode);           
        }
    }
}