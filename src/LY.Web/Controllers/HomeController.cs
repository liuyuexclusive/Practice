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

namespace LY.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepo;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IRoleRepo roleRepo,
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IRepository<User> userRepo
            )
        {
            _roleRepo = roleRepo;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
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
                    var test = _roleRepo.QueryInclude();
                    return Newtonsoft.Json.JsonConvert.SerializeObject(test.Take(3).Select(x => new { x.ID, x.Name, x.Description }));
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
