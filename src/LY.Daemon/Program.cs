using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using LY.Common.NetMQ;
using Microsoft.Extensions.DependencyInjection;
using LY.Initializer;
using Microsoft.AspNetCore.Hosting;

namespace LY.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LYStartup starp = new LYStartup();
            starp.ConfigureServices(new ServiceCollection());

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LYMQ lyMQ = new LYMQ("tcp://127.0.0.1:5555");
            lyMQ.StartServer();
        }
    }
}
