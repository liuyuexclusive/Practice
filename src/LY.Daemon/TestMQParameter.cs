using LY.Common.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Daemon
{
    public class TestMQParameter : IMQParameter
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
