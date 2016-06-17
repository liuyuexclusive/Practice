using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LY.Domain.Sys;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.EFRepository
{
    public class LYDbContext : DbContext
    {
        private static readonly IServiceProvider _serviceProvider = new ServiceCollection().AddEntityFrameworkMySql().BuildServiceProvider();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connStr = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");

            optionsBuilder.UseInternalServiceProvider(_serviceProvider).UseMySql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("sys_role");
            modelBuilder.Entity<User>().ToTable("sys_user");
            modelBuilder.Entity<RoleUserMapping>().ToTable("sys_roleusermapping");
            modelBuilder.Entity<RoleUserMapping>().HasOne(a => a.User).WithMany(a => a.RoleUserMappingList).HasForeignKey(a => a.UserId);
            modelBuilder.Entity<RoleUserMapping>().HasOne(a => a.Role).WithMany(a => a.RoleUserMappingList).HasForeignKey(a => a.RoleId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
