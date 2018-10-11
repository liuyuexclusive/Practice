using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.Domain.Sys;
using LY.DTO.Input;
using LY.DTO.Output;
using LY.Service.Sys;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LY.SysService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class RoleController : ApiControllerBase
    {
        public ILogger<RoleController> _logger;
        public RoleService _roleService;
        public IRepository<Sys_Role> _roleRepo;
        public RoleController(ILogger<RoleController> logger, RoleService roleService, IRepository<Sys_Role> roleRepo)
        {
            _logger = logger;
            _roleService = roleService;
            _roleRepo = roleRepo;
        }

        [Route("GetList")]
        public async Task<OutputList<RoleOutput>> GetList(BaseQueryInput value)
        {
            return await OKList(
                _roleRepo.Queryable.Paging(value).Select(x => new { x.ID, x.Name, x.Description }.Adapt<RoleOutput>()).ToList(),
                _roleRepo.Queryable.Count()
            );
        }

        [HttpPost]
        [Route("AddOrUpdate")]
        public async Task<Output> AddOrUpdate(RoleAddOrUpdateInput value)
        {
            _roleService.AddOrUpdate(value);
            return await OK("操作成功");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<Output> Delete(BaseDeleteInput value)
        {
            _roleService.Delete(value);
            return await OK("删除成功");
        }
    }
}
