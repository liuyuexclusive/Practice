using LY.Common.LYMQ;
using LY.DTO;
using System;

namespace LY.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            LYMQ mq = new LYMQ();
            while (true)
            {
                var key = Console.ReadKey();
                for (int i = 0; i < 100000; i++)
                {
                    mq.Send("TestMQHandler", "Test", new TestMQParameter() { Name = $"张大庆{i}", Age = 30 });
                }
            }
        }
    }
}