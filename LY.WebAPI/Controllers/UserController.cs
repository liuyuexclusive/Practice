using LY.Common;
using LY.Common.Utils;
using LY.Domain;
using LY.Domain.Sys;
using LY.DTO;
using LY.Service.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LY.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class UserController : ApiControllerBase
    {
        public ILogger<UserController> _logger;
        public UserService _userService;
        public IRepository<Sys_User> _userRepo;
        public UserController(ILogger<UserController> logger, UserService userService, IRepository<Sys_User> userRepo)
        {
            _logger = logger;
            _userService = userService;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Authorize]
        //Authorization: Bearer 
        public async Task<Output> Index(string name)
        {
            var isLogin = HttpContext.Session.Keys.Contains(name);
            if (!isLogin)
            {
                return new Output()
                {
                    Message = "还没登录就想进去？",
                    Success = true
                };
            }
            else
            {
                return new Output()
                {
                    Message = "已经登录了",
                    Success = true
                };
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Output> Register(RegisterInput value)
        {
            return await Task.Run<Output>(() =>
            {
                _userService.Register(value);
                return new Output() { Success = true, Message="注册成功" };
            });
        }

        [HttpPut]
        [Route("Login")]
        public async Task<Output<object>> Login(LoginInput value)
        {
            return await Task.Run<Output<object>>(() =>
            {
                var user = _userService.Login(value, out string token);
                return new Output<object>() { Data = new { Token = token,UserName = user.Name } };
            });
        }

        [HttpGet]
        [Route("GetValidateCode")]
        public async Task<Output> GetValidateCode(string email)
        {
            await _userService.GetValidateCode(email);
            return await OK();
        }

        [HttpGet]
        [Authorize]
        [Route("GetTest")]
        public async Task<Output<object>> GetTest() 
        {
            await MailUtil.SendMailAsync("yu-liu@qulv.com", "测试");
            return await Task<object>.Run(() =>
            {
                var data = new List<(string date, string name, string address)>() {
                    ("2016-05-02","王小虎","上海市普陀区金沙江路 1518 弄"),
                    ("2016-05-02","王小虎","上海市普陀区金沙江路 1518 弄"),
                    ("2016-05-02","王小虎","上海市普陀区金沙江路 1518 弄"),
                    ("2016-05-02","王小虎","上海市普陀区金沙江路 1518 弄"),
                    ("2016-05-02","王小虎","上海市普陀区金沙江路 1518 弄")
                };
                return new Output<object>() { Data =data };
            });
        }
    }



}
