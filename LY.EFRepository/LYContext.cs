﻿using Microsoft.EntityFrameworkCore;
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