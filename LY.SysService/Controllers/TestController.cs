using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.Domain.Sys;
using LY.Service.Sys;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class TestController : ApiControllerBase
    {
        public IEntityCache<Sys_User> UserCache { get; set; }
        public IRepository<Sys_User> UserRepo { get; set; }
        public IUnitOfWork UW { get; set; }
        public TestController()
        {

        }

        [UnAuthorize]
        [HttpGet]
        [Route("Test")]
        public string Test()
        {
            var xx = UserCache.List();
            return JsonConvert.SerializeObject(xx);
        }

        [UnAuthorize]
        [HttpGet]
        [Route("Test1")]
        public string Test1()
        {
            var user = UserRepo.Queryable.FirstOrDefault(x => x.Name == "admin");
            user.Mobile = new Random().Next(1310000, 1319999).ToString()+"0000";
            UserRepo.Update(user);
            UW.Commit();
            return "";
            //_cache.SetString("aa", num);
        }
    }
}
