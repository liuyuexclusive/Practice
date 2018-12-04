using Consul;
using LY.Common;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var client = new ConsulClient())
            //{
            //    client.Agent.ServiceRegister(new AgentServiceRegistration() {
            //        ID="LY.SysService",
            //        Name="LY.Service",
            //        Address = "localhost",
            //        Port=9001,                  
            //        Check = new AgentServiceCheck() {
            //           HTTP = "https://localhost:9001/health",
            //           Interval = TimeSpan.FromSeconds(5),
            //           Timeout = TimeSpan.FromSeconds(1)
            //        }
            //    });

            //    client.Agent.ServiceDeregister("LY.SysService");
            //}
            Console.WriteLine("hello");
            LogUtil.Logger<Program>().LogError("123");
            Console.Read();
        }
    }
}
