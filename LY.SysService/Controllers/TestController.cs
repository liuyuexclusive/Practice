using LY.Common;
using LY.Common.API;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class TestController : ApiControllerBase
    {
        IDistributedCache _cache;
        public TestController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [UnAuthorize]
        [HttpGet]
        public string Test()
        {
            string num = new Random().Next(1000, 9999).ToString();
            //_cache.SetString("aa", num);
            return "你好,接口已经启动" + ConfigUtil.ApplicationUrl;
        }
    }
}
