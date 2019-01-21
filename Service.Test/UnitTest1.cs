using LY.Common;
using LY.Common.Utils;
using LY.Domain;
using LY.Domain.Sys;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Service.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ElasticUtil.Indexing();
        }

        [Fact]
        public void Test2()
        {
            var xx = ElasticUtil.Get();
        }
    }
}
