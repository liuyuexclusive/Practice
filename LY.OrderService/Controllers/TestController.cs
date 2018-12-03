using LY.Common;
using LY.Common.API;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LY.OrderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class TestController : ApiControllerBase
    {
        public TestController()
        {
        }
        
        [HttpGet]
        public string Test()
        {
            return "你好,接口已经启动"+ ConfigUtil.AppSettings["AppName"];
        }
    }
}
