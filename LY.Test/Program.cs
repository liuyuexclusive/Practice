using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass mc = new MyClass();
            mc.Do2();
            Console.Read();
        }
    }
    public class MyClass
    {
        public string Name => "123";
        public string DO() => "hehe";
        public dynamic GetSth()
        {
            return new
            {
                Num1 = 1,
                Num2 = 3,

            };
        }
        public void Do2()
        {
            Func<int, int> handler = null;
            handler = (n) =>
            {
                if (n == 0) return 1;
                return n * handler(n - 1);
            };
            Console.WriteLine(handler(4));
        }
    }
}
