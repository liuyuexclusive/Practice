using LY.Domain.Sys;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            modelBuilder.Entity<Sys_Role>().ToTable("sys_role");
            modelBuilder.Entity<Sys_User>().ToTable("sys_user");
            modelBuilder.Entity<Sys_RoleUserMapping>().ToTable("sys_role_user_mapping");
            modelBuilder.Entity<Sys_RoleUserMapping>().HasOne(x => x.Role)
                .WithMany(x => x.RoleUserMappingList)
                .HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<Sys_RoleUserMapping>().HasOne(x => x.User)
                .WithMany(x => x.RoleUserMappingList)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Sys_WorkflowType>().ToTable("sys_workflow_type");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().ToTable("sys_workflow_type_node");
            modelBuilder.Entity<Sys_WorkflowTypeNodeAuditor>().ToTable("sys_workflow_type_node_auditor");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.Type).WithMany(x => x.NodeList).HasForeignKey(x => x.TypeID);
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.PreNode).WithOne(x => x.NextNode).HasForeignKey<Sys_WorkflowTypeNode>(x => x.PreNodeID);
        }
    }
}
