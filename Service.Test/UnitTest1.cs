using LY.Common;
using LY.Domain;
using LY.Domain.Sys;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Service.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //using (var lymc = new LYMasterContext())
            //{
            //    var uw = new UnitOfWork(lymc);
            //    IRepository<Sys_User> userRepo = new Repository<Sys_User>(uw,lymc);
            //    IRepository<Sys_Role> roleRepo = new Repository<Sys_Role>(uw,lymc);
            //    var user = userRepo.Queryable
            //        .Include(x=>x.RoleUserMappingList)
            //        .ThenInclude(x=>x.Role)
            //        .First(x => x.Name== "admin");
            //    var list = user.RoleUserMappingList;
            //    list.FirstOrDefault().Role.Description = "hehehaha";

            //    user.Mobile = "13100000001";
            //    userRepo.Update(user);
            //    roleRepo.Update(list.FirstOrDefault().Role);
            //    uw.Commit();               
            //}
        }

        [Fact]
        public void TestRedis()
        {
            ExportByClassName(typeof(string).Name);
        }

        public void Export<T1>(T1 xx)
        {
            Console.WriteLine(xx);
        }


        public void ExportByClassName(string typename1)
        {
            Type t1 = Type.GetType(typename1);
            MethodInfo mi = this.GetType().GetMethod("Export").MakeGenericMethod(new Type[] { t1 });
            mi.Invoke(this, new object[] { "test111" });
        }
    }
}
