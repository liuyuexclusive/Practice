using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LY.Common.Utils
{
    public static class ConsulUtil
    {
        static string ServiceID
        {
            get
            {
                return (ConfigUtil.AppName + "_" + ConfigUtil.ApplicationUrl).GetHashCode().ToString();
            }
        }
        public async static Task ServiceRegister()
        {
            using (var client = new ConsulClient(x => x.Address = new Uri(ConfigUtil.ConsulUrl)))
            {
                var registration = new AgentServiceRegistration()
                {
                    ID = ServiceID,
                    Name = ConfigUtil.AppName,
                    Address = ConfigUtil.Host,
                    Port = ConfigUtil.Port,
                    Check = new AgentServiceCheck()
                    {
                        HTTP = $"{ConfigUtil.ApplicationUrl}health",
                        Interval = TimeSpan.FromSeconds(5),
                        Timeout = TimeSpan.FromSeconds(1),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10)
                    }
                };
                await client.Agent.ServiceRegister(registration);
            }
        }

        public async static Task ServiceDeRegister()
        {
            using (var client = new ConsulClient(x => x.Address = new Uri(ConfigUtil.ConsulUrl)))
            {
                await client.Agent.ServiceDeregister(ServiceID);
            }
        }
    }
}
