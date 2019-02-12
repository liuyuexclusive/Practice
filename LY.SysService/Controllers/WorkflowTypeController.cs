using LY.Application.Sys;
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
    public class WorkflowTypeController : ApiControllerBase
    {
        public WorkflowTypeService WorkflowTypeService { get; set; }
        public IEntityCache<Sys_WorkflowType> WorkflowTypeCache { get; set; }

        public WorkflowTypeController()
        {

        }

        [HttpPost]
        [Route("AddOrUpdate")]
        public async Task<Output> AddOrUpdate(WorkflowTypeAddOrUpdateInput value)
        {
            WorkflowTypeService.AddOrUpdate(value);
            return await OK("操作成功");
        }

        [HttpPost]
        [Route("GetList")]
        public async Task<OutputList<WorkflowTypeOutput>> GetList()
        {
            var list = WorkflowTypeCache.List();
            return await OKList<WorkflowTypeOutput>(list.Select(x=> {
                return new WorkflowTypeOutput()
                {
                   ID = x.ID, Name = x.Name, Nodes = x.NodeList?.Select(a=> {
                       return new WorkflowTypeNodeOutput()
                       {
                          ID = a.ID,
                          Name = a.Name,
                          Auditors = a.AuditorList?.Select(b=>b.UserID)
                       };
                   })
                };
            }), list.Count);
        }
    }
}
