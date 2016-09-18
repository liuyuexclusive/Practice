using LY.Domain;
using LY.Domain.Sys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LY.Test
{
    public class Program
    {

        public static void Main(string[] args)
        {

            //Task<string> task = Task.Run(() =>
            //{
            //    return GetName();
            //});

            //var name = task.GetAwaiter().GetResult();
            //Console.WriteLine($"my name is {name}");
            //Test();

            //Assembly.Load(new AssemblyName(@"‪D:\MyFiles\Code\practice\src\LY.Test\bin\Debug\netcoreapp1.0\LY.Common.dll"));

            Console.Read();
        }

        public static async Task Test()
        {
            var name = await GetName();
            Console.WriteLine($"my name is {name}");
        }

        public static async Task<string> GetName()
        {
            await Task.Delay(2000);
            return await Task.Run(() =>
            {    
                return "tom";
            });
        }
    }
    public class Test
    {
        public List<Test2> TestList { get; set; }
    }

    public class Test2
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
