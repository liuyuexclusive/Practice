using LY.Common;
using LY.Common.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LY.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MQUtil.Start();
            MQUtil.Subscrib<IList<GatewayReRoute>>(x => GatewayConfigUtil.Update("configuration.json", x), "GatewayConfigUtilGen");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}
