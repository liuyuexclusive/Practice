using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.EFRepository;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.Extensions.Logging;
using LY.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using LY.Service.Sys;

namespace LY.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Role> _roleRepo;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IRepository<Role> roleRepo,
            UserService userService
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roleRepo = roleRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            var xxx = await Task.Run<string>(
                () =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int count;

                    var paths = new NavigationPropertyPath<Role>[] {
                        new NavigationPropertyPath<Role>(x => x.RoleUserMappingList, x => ((RoleUserMapping)x).User)
                    };

                    var test = _roleRepo.Query(x => true, x => x.ID, true, 1, 100, out count, paths);
                    sw.Stop();
                    ViewBag.Times = sw.Elapsed.TotalSeconds;
                    return "ok";
                });
            //throw new Exception("手动抛异常");
            //User xxx;
            ////IocManager.Resolve<IRepository<User>>(a => xxx = a.Get(1));

            //var user = _userRepo.Get(1);
            //user.Mobile = "456";
            //_userRepo.UpdateOnDemand(user);
            //_logger.LogDebug("done");
            //_logger.LogCritical("LogCritical");
            //_logger.LogError("LogError");
            //_logger.LogInformation("LogInformation");
            //_logger.LogTrace("LogTrace");
            //_logger.LogWarning("LogWarning");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
