using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LY.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>() { };
            var xx = list.AsQueryable();
            Console.WriteLine(xx.ElementType);
            Console.WriteLine(xx.Expression);
            Console.WriteLine(xx.Provider);
            Console.WriteLine("Hello World!");
            Console.Read();
        }
    }

    public class MyQueryAble : IQueryable
    {
        public Type ElementType => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}