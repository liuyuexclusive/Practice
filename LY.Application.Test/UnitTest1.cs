using LY.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using System;

namespace LY.Application.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new ElasticClient(new Uri("http://192.168.123.6:9200"));
            //client.Index(new ElasticTest() { Name = "我的宝马发动机多少", Age = 333 },a=>a.Index("ly"));
            var xx = client.Search<ElasticTest>(a =>
            a.From(0)
            .Size(10)
            .Index("ly")
            .Query(x =>
            x.Match(t => t.Field(p => p.Name).Query("宝马"))
            || x.Term(t => t.Age, 111)
            )
            );
        }
    }
}
