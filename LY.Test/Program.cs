using LY.Common.LYMQ;
using LY.DTO;
using System;

namespace LY.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IMQ mq = new LYMQ();
            while (true)
            {
                Console.ReadKey();
                mq.Send("TestMQHandler", "Test", new TestMQParameter() { Name = "hh", Age = 89 });
            }
        }
    }
}
