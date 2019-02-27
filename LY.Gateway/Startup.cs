using LY.Initializer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Gateway
{
    public class Startup: LYGatewayStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
