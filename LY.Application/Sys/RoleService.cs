using System;
using System.Linq;
using LY.Common;
using LY.Domain.Sys;
using LY.DTO.Input;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LY.Service.Sys
{
    public class RoleService
    {
        public IRepository<Sys_Role> RoleRepo { get; set; }
        public IEventRepository<Sys_RoleEvent> RoleEventRepo { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public RoleService()
        {

        }
        public void AddOrUpdate(RoleAddOrUpdateInput value)
        {

            if (value == null)
            {
                throw new BusinessException("参数不能为空");
            }

            if (!value.ID.HasValue)
            {
                RoleEventRepo.Add(value.ToDic());
                //var role = value.Adapt<Sys_Role>();
                //RoleRepo.Add(role);
            }
            else
            {
                RoleEventRepo.Update(value.ID.Value, value.ToDic());
                //var role = RoleRepo.Queryable.FirstOrDefault(x=>x.ID==value.ID.Value);
                //if (role == null)
                //{
                //    throw new BusinessException("当前数据不存在");
                //}
                //value.Adapt(role);
                //RoleRepo.Update(role);
            }
            UnitOfWork.Commit();
        }

        public void Delete(BaseDeleteInput value)
        {
            RoleEventRepo.Delete(value.IDs.ToArray());

            //var roleList = RoleRepo.Queryable.Where(x => value.IDs.Contains(x.ID));
            //if (roleList.IsNullOrEmpty())
            //{
            //    throw new BusinessException("指定的数据不存在");
            //}
            //foreach (var item in roleList)
            //{
            //    RoleRepo.Delete(item);
            //}
            UnitOfWork.Commit();
        }
    }
}
