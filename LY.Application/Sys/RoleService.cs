using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using LY.DTO.Input;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Service.Sys
{
    public class RoleService
    {
        public IRepository<Sys_Role> RoleRepo { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public RoleService()
        {

        }
        public void Add(RoleAddInput value)
        {
            if (value == null)
            {
                throw new BusinessException("参数不能为空");
            }
            var role = value.Adapt<Sys_Role>();
            RoleRepo.Add(role);
            UnitOfWork.Commit();
        }
    }
}
