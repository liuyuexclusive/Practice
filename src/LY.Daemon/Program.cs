using System;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using LY.Initializer;
using Microsoft.Extensions.DependencyInjection;
using LY.Common;

namespace LY.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogUtil.Logger<Program>().LogDebug("hehehaha");


            Console.Read();
            //LYMQ lyMQ = new LYMQ("tcp://127.0.0.1:5555");
            //lyMQ.StartServer();
        }
    }
}
