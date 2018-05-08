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
            try
            {
                Console.WriteLine("开始启动监听服务");
                IMQ mq = new LYMQ();
                mq.StartServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("启动监听服务失败：" + ex.Message);
            }
        }
    }
}
