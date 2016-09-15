using LY.Domain;
using LY.Domain.Sys;
using System;
using System.Collections.Generic;
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

            //Console.WriteLine("complish");

            var test2List = new List<Test2>() { new Test2() { Name = "aa", Age = 1 }, new Test2() { Name = "bb", Age = 2 } };

            var test = new Test() { TestList = test2List };

            var xxx = test.GetType().GetProperty("TestList").GetValue(test);

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
