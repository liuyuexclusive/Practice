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

            string connStr = ConfigUtil.ConfigurationRoot.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("sys_role");
            modelBuilder.Entity<User>().ToTable("sys_user");
            modelBuilder.Entity<RoleUserMapping>().ToTable("sys_roleusermapping");
            base.OnModelCreating(modelBuilder);
        }
    }
}
