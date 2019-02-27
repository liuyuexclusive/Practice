using LY.Initializer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Daemon
{
    public class Startup: LYDaemonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
