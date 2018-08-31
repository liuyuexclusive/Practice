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
using LY.Service.Sys;

namespace LY.WebAPI.Controllers
{
    [Route("test")]
    [EnableCors("cors")]
    public class TestController : ControllerBase
    {
        public ILogger<TestController> Logger { get; set; }
        public UserService _userService { get; set; }
        public TestController()
        {
        }

        [HttpGet]
        [Route("GetTestData")]
        public async Task<object> GetTestData()
        {
            return await Task.Run<object>(() =>
            {
                return _userService.GetUser();
            });
        }

        [HttpGet]
        [Route("diy")]
        public IEnumerable<string> DIY()
        {
            Logger.LogInformation("测试成功了哈哈哈");
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
