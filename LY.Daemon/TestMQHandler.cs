using LY.Common.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Daemon
{
    public class TestMQHandler : IMQHandler
    {
        public void Test(TestMQParameter param)
        {
            Console.WriteLine($"成功了，我叫{param.Name},我今年{param.Age}岁");
        }
    }
}
