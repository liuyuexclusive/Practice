using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using LY.Common;
using LY.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LY.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        [UnAuthorize]
        [CapSubscribe("GatewayConfigUtilGen",Group = "GatewayConfigUtilGenGroup1")]
        public Task GatewayConfigUtilGen(IList<GatewayReRoute> list)
        {
            GatewayConfigUtil.Update("configuration.json", list);
            return Task.CompletedTask;
        }
        [UnAuthorize]
        [CapSubscribe("GatewayConfigUtilGen", Group = "GatewayConfigUtilGenGroup1")]
        public Task GatewayConfigUtilGen2(IList<GatewayReRoute> list)
        {
            GatewayConfigUtil.Update("configuration.json", list);
            return Task.CompletedTask;
        }
        [UnAuthorize]
        [CapSubscribe("GatewayConfigUtilGen", Group = "GatewayConfigUtilGenGroup2")]
        public Task GatewayConfigUtilGen3(IList<GatewayReRoute> list)
        {
            GatewayConfigUtil.Update("configuration.json", list);
            return Task.CompletedTask;
        }
    }
}
