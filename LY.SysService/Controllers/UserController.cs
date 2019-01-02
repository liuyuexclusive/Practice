using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.Domain.Sys;
using LY.DTO;
using LY.DTO.Input;
using LY.DTO.Output;
using LY.Service.Sys;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class UserController : ApiControllerBase
    {
        public ILogger<UserController> _logger;
        public UserService _userService;
        public IEntityCache<Sys_User> UserCache { get; set; }
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("GetList")]
        public async Task<OutputList<UserOutput>> GetList(BaseQueryInput value)
        {
            var data = UserCache.List();
            return await OKList(
                data.Select(x => new { x.ID, x.Name, x.Email, x.Mobile, x.LastOn }.Adapt<UserOutput>()).ToList(),
                data.Count()
               );
        }

        [UnAuthorize]
        [HttpPost]
        [Route("Register")]
        public async Task<Output> Register(RegisterInput value)
        {
            _userService.Register(value);
            return await OK("注册成功");
        }

        [UnAuthorize]
        [HttpPut]
        [Route("Login")]
        public async Task<Output<LoginOutput>> Login(LoginInput value)
        {
            var user = _userService.Login(value, out string token);
            return await OK<LoginOutput>(new LoginOutput { Token = token, UserName = user.Name }, "登录成功");
        }

        [UnAuthorize]
        [HttpGet]
        [Route("GetValidateCode")]
        public async Task<Output> GetValidateCode(string email)
        {
            await _userService.GetValidateCode(email);
            return await OK();
        }

        [UnAuthorize]
        [HttpDelete]
        [Route("Delete")]
        public async Task<Output> Delete(BaseDeleteInput value)
        {
            _userService.Delete(value);
            return await OK("删除成功");
        }
    }
}
