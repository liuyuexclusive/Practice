using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common.Utils
{
    public static class ElasticUtil
    {
        public static Uri Uri
        {
            get
            {
                return new Uri("http://localhost:9200");
            }
        }

        public static void Indexing()
        {
            var client = new ElasticClient(Uri);
            client.Create(new CreateRequest<ElasticTest>("ly", "test", "1"));
            client.Index(new ElasticTest() { Name = "aa", Age = 11 }, x=>x.Index("ly").Type("test").Id(1));
        }

        public static ElasticTest Get()
        {
            var client = new ElasticClient(Uri);
            return client.Get<ElasticTest>(new GetRequest("ly","test","1")).Source;
        }
    }

    public class ElasticTest
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
