using LY.Common;
using LY.Common.LYMQ;
using LY.Initializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace LY.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new LYStartup().StartDaemon(new ServiceCollection());//注册IOC
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //try
            //{
            //    Console.WriteLine("开始启动监听服务");
            //    LYMQ lyMQ = new LYMQ();
            //    lyMQ.StartServer();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("启动监听服务失败：" + ex.Message);
            //}

            var xxx = Encoding.UTF8.GetBytes("123454321");
            IOCManager.Resolve<IDistributedCache>(x => x.Set("aa", xxx));
            byte[] yyy = null;
            IOCManager.Resolve<IDistributedCache>(x => yyy = x.Get("aa"));
            Console.WriteLine(Encoding.UTF8.GetString(yyy));
            Console.Read();
        }
    }
}
