using Microsoft.EntityFrameworkCore;
using LY.Domain.Sys;
using LY.Common;
using Microsoft.Extensions.Configuration;

namespace LY.EFRepository
{
    public static class ModelBuilderExtension
    {
        public static void EntityToTable(this ModelBuilder modelBuilder)
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

            modelBuilder.Entity<Sys_WorkflowType>().ToTable("sys_workflowtype");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().ToTable("sys_workflowtype_node");
            modelBuilder.Entity<Sys_WorkflowTypeNodeAuditor>().ToTable("sys_workflowtype_node_auditor");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.Type).WithMany(x => x.NodeList).HasForeignKey(x => x.TypeID);
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.PreNode).WithOne(x => x.NextNode).HasForeignKey<Sys_WorkflowTypeNode>(x => x.PreNodeID);
        }
    }

    public class LYMasterContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(ConfigUtil.MasterConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityToTable(modelBuilder);
        }

        private void EntityToTable(ModelBuilder modelBuilder)
        {
            modelBuilder.EntityToTable();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class LYSlaveContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(ConfigUtil.SlaveConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityToTable(modelBuilder);
        }

        private void EntityToTable(ModelBuilder modelBuilder)
        {
            modelBuilder.EntityToTable();
            base.OnModelCreating(modelBuilder);
        }
    }
}
