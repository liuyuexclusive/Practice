using Microsoft.EntityFrameworkCore;
using LY.Domain.Sys;
using LY.Common;
using Microsoft.Extensions.Configuration;

namespace LY.EFRepository
{
    public class LYDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(ConfigUtil.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sys_Role>().ToTable("sys_role");
            modelBuilder.Entity<Sys_User>().ToTable("sys_user");
            modelBuilder.Entity<Sys_RoleUserMapping>().ToTable("sys_roleusermapping");
            base.OnModelCreating(modelBuilder);
        }
    }
}
