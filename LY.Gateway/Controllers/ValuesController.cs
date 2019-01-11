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
    public class ValuesController : ControllerBase
    {
        [UnAuthorize]
        [CapSubscribe("GatewayConfigUtilGen")]
        public Task CheckReceivedMessage(IList<GatewayReRoute> list)
        {
            GatewayConfigUtil.Update("configuration.json", list);
            return Task.CompletedTask;
        }
    }
}
