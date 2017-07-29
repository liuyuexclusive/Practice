using LY.Common.LYMQ;
using LY.DTO;
using System;

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
