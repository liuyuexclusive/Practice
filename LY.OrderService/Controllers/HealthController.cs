using LY.Common;
using LY.Common.API;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;

namespace LY.OrderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class HealthController : ApiControllerBase
    {
        [UnAuthorize]
        [HttpGet]
        public string Health()
        {
            return "OK";
        }
    }
}
