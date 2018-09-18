using LY.Domain;
using LY.Domain.Sys;
using LY.DTO.Input;
using LY.Service.Sys;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LY.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class RoleController : ApiControllerBase
    {
        public ILogger<UserController> _logger;
        public RoleService _roleService;
        public IRepository<Sys_Role> _roleRepo;
        public RoleController(ILogger<UserController> logger, RoleService roleService, IRepository<Sys_Role> roleRepo)
        {
            _logger = logger;
            _roleService = roleService;
            _roleRepo = roleRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<Output<object>> Get()
        {
            return await Task<Output<object>>.Run(() => {
                return new Output<object>()
                {
                    Data = _roleRepo.Queryable.Take(30).Select(x=> new { x.ID,x.Name,x.Description }).ToList()
                };
            });
        }

        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<Output> Add(RoleAddInput value)
        {
            _roleService.Add(value);
            return await OK();
        }
    }
}
