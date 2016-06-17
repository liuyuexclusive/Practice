using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.EFRepository;
using LY.Domain;
using LY.Domain.Sys;
using Microsoft.Extensions.Logging;

namespace LY.Web.Controllers
{

    public class HomeController : Controller
    {
        IRepository<Role> _roleRepo;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IRepository<Role> roleRepo,
            ILogger<HomeController> logger
            )
        {
            _roleRepo = roleRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            //var test = _roleRepo.Get(x => true, new System.Linq.Expressions.Expression<Func<Role, object>>[] { x => x.RoleUserMappingList }, new System.Linq.Expressions.Expression<Func<object, object>>[] { x => ((RoleUserMapping)x).User });

            var test = _roleRepo.Get(x => true, new System.Linq.Expressions.Expression<Func<Role, object>>[] { x => x.RoleUserMappingList }, null );

            var xxx = test.RoleUserMappingList;


            _logger.LogDebug("LogDebug");
            _logger.LogCritical("LogCritical");
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogTrace("LogTrace");
            _logger.LogWarning("LogWarning");

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
