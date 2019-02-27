using DotNetCore.CAP;
using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class ScheduleController : ApiControllerBase
    {
        public IEntityCache<Sys_User> UserCache { get; set; }
        public IRepository<Sys_User> UserRepo { get; set; }
        public IUnitOfWork UW { get; set; }
        public ICapPublisher Publisher { get; set; }
        public ILogger<TestController> Logger { get; set; }
        public ScheduleController()
        {

        }

        [HttpGet]
        [UnAuthorize]
        [Route("GetList")]
        public async Task<OutputList<object>> GetList()
        {
            var beginDate = new DateTime(2019, 2, 23);
            var dates = new List<DateTime> { };
            var skipDates = new List<DateTime> {
                new DateTime(2019, 4, 5),
                new DateTime(2019, 4, 6),
                new DateTime(2019, 4, 7),
                new DateTime(2019, 5, 1),
                new DateTime(2019, 6, 7),
                new DateTime(2019, 6, 8),
                new DateTime(2019, 6, 9),
                new DateTime(2019, 9, 13),
                new DateTime(2019, 9, 14),
                new DateTime(2019, 9, 15),
                new DateTime(2019, 10, 1),
                new DateTime(2019, 10, 2),
                new DateTime(2019, 10, 3),
                new DateTime(2019, 10, 4),
                new DateTime(2019, 10, 5),
                new DateTime(2019, 10, 6),
                new DateTime(2019, 10, 7)
            };
            var date = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

            bool isWork = true;
            while (true)
            {
                if (date >= new DateTime(2020, 1, 1))
                {
                    break;
                }

                if (!skipDates.Contains(date) && isWork)
                {
                    dates.Add(date);
                    isWork = false;
                }
                else
                {
                    isWork = true;
                }

                date = date.AddDays(7);
            }
            
            string[,] teams =  {
                //{ "刘钰", "马荣健", "李林玉"},
                //{ "詹张", "焦娇", "黄燕" },
                //{ "李佩逸", "雷星", "李庆强" },
                //{ "苏莹", "毛景杰", "施礼斌" },
                //{ "刘威", "李明", "钟文康" },

                { "马荣健", "刘钰", "李林玉" },
                { "詹张", "焦娇", "黄燕" },
                { "雷星", "李庆强", "李佩逸" },
                { "苏莹", "施礼斌","毛景杰" },
                { "李明", "钟文康","刘威" },
            };

            return await OKList<object>(
                dates.Select(x => new
                {
                    Date = x.ToString("yyyy-MM-dd"),
                    Team1 = teams[0, dates.IndexOf(x) % 3],
                    Team2 = teams[1, dates.IndexOf(x) % 3],
                    Team3 = teams[2, dates.IndexOf(x) % 3],
                    Team4 = teams[3, dates.IndexOf(x) % 3],
                    Team5 = teams[4, dates.IndexOf(x) % 3]
                }).ToList(),
                dates.Count()
            );
        }
    }
}
