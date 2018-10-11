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
        public IRepository<Sys_User> _userRepo;
        public UserController(ILogger<UserController> logger, UserService userService, IRepository<Sys_User> userRepo)
        {
            _logger = logger;
            _userService = userService;
            _userRepo = userRepo;
        }

        [Route("GetList")]
        public async Task<OutputList<UserOutput>> GetList(BaseQueryInput value)
        {
            return await OKList(
                _userRepo.Queryable.Paging(value).Select(x => new { x.ID, x.Name, x.Email, x.Mobile, x.LastOn }.Adapt<UserOutput>()).ToList(),
                _userRepo.Queryable.Count()
               );
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Output> Register(RegisterInput value)
        {
            _userService.Register(value);
            return await OK("注册成功");
        }

        [HttpPut]
        [Route("Login")]
        public async Task<Output<LoginOutput>> Login(LoginInput value)
        {
            var user = _userService.Login(value, out string token);
            return await OK<LoginOutput>(new LoginOutput { Token = token, UserName = user.Name }, "登录成功");
        }

        [HttpGet]
        [Route("GetValidateCode")]
        public async Task<Output> GetValidateCode(string email)
        {
            await _userService.GetValidateCode(email);
            return await OK();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<Output> Delete(BaseDeleteInput value)
        {
            _userService.Delete(value);
            return await OK("删除成功");
        }
    }
}
