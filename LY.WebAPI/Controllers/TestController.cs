using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Domain.Sys;
using LY.Common;
using Microsoft.Extensions.Logging;
using LY.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace LY.WebAPI.Controllers
{
    [Route("test")]
    [EnableCors("cors")]
    public class TestController : Controller
    {
        private readonly IRepository<Role> _roleRepo;
        private readonly ILogger<TestController> _logger;
        private readonly IRepository<User> _userRepo;
        public TestController(IRepository<Role> roleRepo, ILoggerFactory logger, IRepository<User> userRepo)
        {
            _roleRepo = roleRepo;
            _logger = logger.CreateLogger<TestController>();
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("GetTestData")]
        public async Task<object> GetTestData()
        {
            return await Task.Run<object>(() =>
            {
                return _userRepo.Queryable.Select(x => new
                {
                    Name = x.Name,
                    Age = 11,
                    Gender = "不男不女"
                }).ToList();
            });
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
