using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LY.Common.NetMQ;
using LY.Daemon;

namespace TestMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            while (true)
            {
                Console.WriteLine("现在发送，请点击");
                Console.ReadKey();
                var mq = new LYMQ("tcp://127.0.0.1:5555");
                mq.Send("TestMQHandler", "Test", new TestMQParameter()
                {
                    Name = "liuyu",
                    Age = 18
                });
            }
        }
    }
}
