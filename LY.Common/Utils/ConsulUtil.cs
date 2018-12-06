using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common.Utils
{
    public static class ConsulUtil
    {
        public async static void ServiceRegister()
        {
            using (var client = new ConsulClient())
            {
                var exist = await client.Agent.Services();
                foreach (var item in exist.Response.Values)
                {
                    await client.Agent.ServiceDeregister(item.ID);
                }
                var xx = new AgentServiceRegistration()
                {
                    //ID = ConfigUtil.AppName + ConfigUtil.ApplicationUrl,
                    ID = Guid.NewGuid().ToString(),
                    Name = ConfigUtil.AppName,
                    Address = ConfigUtil.Host,
                    Port = ConfigUtil.Port,
                    //Check = new AgentServiceCheck()
                    //{
                    //    HTTP = $"{ConfigUtil.ApplicationUrl}health",
                    //    Interval = TimeSpan.FromSeconds(5),
                    //    Timeout = TimeSpan.FromSeconds(1),
                    //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)
                    //}
                };
                var xxx = await client.Agent.ServiceRegister(xx);
            }
        }

        public static void ServiceDeRegister()
        {
            using (var client = new ConsulClient())
            {
                client.Agent.ServiceDeregister(ConfigUtil.AppName + ConfigUtil.ApplicationUrl);
            }
        }
    }
}
