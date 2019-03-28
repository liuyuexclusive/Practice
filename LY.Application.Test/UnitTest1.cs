using LY.Common;
using LY.Common.Utils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace LY.Application.Test
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var client = new ElasticClient(new Uri("http://172.16.30.195:9200"));
        //    //client.Index(new ElasticTest() { Name = "我的宝马发动机多少", Age = 333 },a=>a.Index("ly"));
        //    var xx = client.Search<ElasticTest>(a =>
        //    a.From(0)
        //    .Size(10)
        //    .Index("ly")
        //    .Query(x =>
        //    x.Match(t => t.Field(p => p.Name).Query("宝马"))
        //    || x.Term(t => t.Age, 111)
        //    )
        //    );
        //}
        [TestMethod]
        public void TestMethod2()
        {
            var account = new Account();
            var taskList = new List<Task>();
            Parallel.For(0, 100, i =>
            {
                taskList.Add(account.Transfer(i));
            });
            Task.WhenAll(taskList).Wait();
            var result = account.GetMoney().Result;
            Console.WriteLine(result);
        }

        [TestMethod]
        public void ReadJWT()
        {

        }
    }
    public class Account
    {
        int Money { get; set; }
        public async Task Transfer(int money)
        {
            await Task.Delay(1);
            Money += money;
        }

        public async Task<int> GetMoney()
        {
            return await Task.FromResult(Money);
        }
    }
}
