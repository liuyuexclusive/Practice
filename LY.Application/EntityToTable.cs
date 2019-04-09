using LY.Common;
using LY.Domain.Sys;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LY.Daemon
{
    /// <summary>
    /// 映射关系
    /// </summary>
    public class EntityToTable: IEntityToTable
    {
        /// <summary>
        /// Excute
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void Excute(ModelBuilder modelBuilder)
        {

            foreach(var type in AssemblyUtil.DomainTypes)
            {
                modelBuilder.Entity(type).ToTable(type.Name);
            }

            modelBuilder.Entity<Sys_RoleUserMapping>().HasOne(x => x.Role)
                .WithMany(x => x.RoleUserMappingList)
                .HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<Sys_RoleUserMapping>().HasOne(x => x.User)
                .WithMany(x => x.RoleUserMappingList)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.Type).WithMany(x => x.NodeList).HasForeignKey(x => x.TypeID);
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.PreNode).WithOne(x => x.NextNode).HasForeignKey<Sys_WorkflowTypeNode>(x => x.PreNodeID);
        }
    }
}
