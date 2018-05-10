using LY.DTO;
using System;

namespace LY.Daemon
{
    public class TestMQHandler : IMQHandler
    {
        public TestMQResultDTO Test(TestMQParameter param)
        {
            string result = $"成功了，我叫{param.Name},我今年{param.Age}岁";
            Console.WriteLine(result);
            return new TestMQResultDTO() { ReturnMsg = result };
        }
    }
}
