using LY.Common.LYMQ;
using System;
using System.Text;

namespace LY.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                Console.WriteLine("开始启动监听服务");
                LYMQ lyMQ = new LYMQ();
                lyMQ.StartServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("启动监听服务失败：" + ex.Message);
            }
        }
    }
}
