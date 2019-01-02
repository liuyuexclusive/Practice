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
        public RoleService RoleService { get; set; }
        public IEntityCache<Sys_Role> RoleCache { get; set; }
        public RoleController()
        {
        }

        [HttpPost]
        [Route("GetList")]
        public async Task<OutputList<RoleOutput>> GetList(BaseQueryInput value)
        {
            var data = RoleCache.List();
            return await OKList(
                data.Select(x => new { x.ID, x.Name, x.Description }.Adapt<RoleOutput>()).ToList(),
                data.Count()
            );
        }

        [HttpPost]
        [Route("AddOrUpdate")]
        public async Task<Output> AddOrUpdate(RoleAddOrUpdateInput value)
        {
            RoleService.AddOrUpdate(value);
            return await OK("操作成功");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<Output> Delete(BaseDeleteInput value)
        {
            RoleService.Delete(value);
            return await OK("删除成功");
        }
    }
}
