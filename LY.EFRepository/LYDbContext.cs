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
            //modelBuilder.Entity<RoleUserMapping>().HasOne(a => a.User).WithMany(a => a.RoleUserMappingList).HasForeignKey(a => a.UserId);
            //modelBuilder.Entity<RoleUserMapping>().HasOne(a => a.Role).WithMany(a => a.RoleUserMappingList).HasForeignKey(a => a.RoleId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
