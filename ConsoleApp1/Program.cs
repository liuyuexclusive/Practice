using LY.Common.Utils;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("输入日期，格式:yyyy-MM-dd");
                string str =  Console.ReadLine();
                if (DateTime.TryParse(str, out DateTime date))
                {
                    var roomtime = (date.ToUniversalTime().Ticks - 621355968000000000) / 10000 / 1000;
                    Console.WriteLine($"结果：{roomtime}");
                }
                else
                {
                    Console.WriteLine("格式错误");
                }
            }

            Console.Read();
        }
    }
}