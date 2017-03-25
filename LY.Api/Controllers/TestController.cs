﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Domain.Sys;
using LY.Common;
using Microsoft.Extensions.Logging;
using LY.Domain;

namespace LY.Api.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IRepository<Role> _roleRepo;
        private readonly ILogger<TestController> _logger;
        public TestController(IRepository<Role> roleRepo, ILoggerFactory logger)
        {
            _roleRepo = roleRepo;
            _logger = logger.CreateLogger<TestController>();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "hello" };
        }

        [HttpGet]
        [Route("diy")]
        public IEnumerable<string> DIY()
        {
            _logger.LogInformation("测试成功了哈哈哈");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 蹦跶你个蹦达
        /// </summary>
        /// <param name="dd">小弟弟</param>
        /// <returns>大数组哦</returns>
        [HttpPut]
        [Route("diy2")]
        public IEnumerable<string> DIY2(string dd)
        {
            return new string[] { "value1", "value2", "value3" };
        }

        // POST api/values
        [HttpPost]
        [Route("diy3")]
        public void DIY([FromBody]string value)
        {
        }
    }
}