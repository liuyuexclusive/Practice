﻿using DotNetCore.CAP;
using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class TestController : ApiControllerBase
    {
        public IEntityCache<Sys_User> UserCache { get; set; }
        public IRepository<Sys_User> UserRepo { get; set; }
        public IUnitOfWork UW { get; set; }
        public ICapPublisher Publisher { get; set; }
        public ILogger<TestController> Logger { get; set; }
        public TestController()
        {

        }

        [UnAuthorize]
        [HttpGet]
        public string Test()
        {
            throw new Exception("测试异常");
            Logger.LogInformation("测试日志");
            return "OK";
        }
    }
}
