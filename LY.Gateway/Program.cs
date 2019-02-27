using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LY.Common;
using LY.Common.Utils;
using LY.Initializer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LY.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //MQUtil.Start();
            //MQUtil.Subscrib<IList<GatewayReRoute>>(x => GatewayConfigUtil.Update("configuration.json", x), "GatewayConfigUtilGen");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(ConfigUtil.StartUrl);
    }
}
