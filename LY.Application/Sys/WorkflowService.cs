using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using LY.DTO.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Application.Sys
{
    public class WorkflowService
    {
        public IRepository<Sys_WorkflowType> WorkflowTypeRepo { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

        public void AddOrUpdate(WorkflowTypeAddOrUpdateInput value)
        {
            if (value == null)
            {
                throw new BusinessException("参数不能为空");
            }

            if (string.IsNullOrEmpty(value.Name))
            {
                throw new BusinessException("工作流类型名称不能为空");
            }

            var listNodes = new List<Sys_WorkflowTypeNode>();
            if (!value.Nodes.IsNullOrEmpty())
            {
                for (int i = 0; i < value.Nodes.Count(); i++)
                {
                    var item = value.Nodes[i];
                    listNodes.Add(new Sys_WorkflowTypeNode() {
                        AuditorList = item.Auditors?.Select(x => new Sys_WorkflowTypeNodeAuditor() { UserID = x }).ToList(),
                        PreNode = i==0 ? null: listNodes[i-1],
                        SortIndex = i,
                        Name = item.Name,
                    });
                }
            }

            Sys_WorkflowType type = null;
            if (value.ID.HasValue)
            {
                type = WorkflowTypeRepo.Queryable.Include(x => x.NodeList).ThenInclude(x => x.AuditorList).FirstOrDefault(x => x.ID == value.ID.Value);
                if (type == null)
                {
                    throw new BusinessException($"无法找到ID为{value.ID.Value}的工作流类型");
                }
                type.Name = value.Name;
                type.NodeList = listNodes;
                WorkflowTypeRepo.Update(type);
            }
            else
            {
                type = new Sys_WorkflowType() { Name = value.Name };
                type.NodeList = listNodes;
                WorkflowTypeRepo.Add(type);
            }
            UnitOfWork.Commit();
            var xxx = WorkflowTypeRepo.GetAll();
        }
    }
}
