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
            Test();
            Console.Read();
        }

        static async void Test()
        {
            using (var client = new ConsulClient())
            {
                await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = "LY.SysService",
                    Name = "LY.Service",
                    Address = "localhost",
                    Port = 9001,
                    Check = new AgentServiceCheck()
                    {
                        HTTP = "https://localhost:9001/health",
                        Interval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(1)
                    }
                });

                await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = "LY.SysService",
                    Name = "LY.Service",
                    Address = "localhost",
                    Port = 9001,
                    Check = new AgentServiceCheck()
                    {
                        HTTP = "https://localhost:9001/health",
                        Interval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(1)
                    }
                });

                var xx = client.PreparedQuery.Create(new PreparedQueryDefinition() { Name= "LY.Service" });

                Console.WriteLine("hello");
            }

        }
    }
}
