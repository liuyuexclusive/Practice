using System;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using LY.Initializer;
using Microsoft.Extensions.DependencyInjection;
using LY.Common;
using LY.Common.NetMQ;
using System.Text;

namespace LY.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LYMQ lyMQ = new LYMQ();
            lyMQ.StartServer();
            Console.Read();
        }
    }
}
