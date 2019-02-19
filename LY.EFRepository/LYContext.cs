using Microsoft.EntityFrameworkCore;
using LY.Domain.Sys;
using LY.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;

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

            modelBuilder.Entity<Sys_WorkflowType>().ToTable("sys_workflow_type");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().ToTable("sys_workflow_type_node");
            modelBuilder.Entity<Sys_WorkflowTypeNodeAuditor>().ToTable("sys_workflow_type_node_auditor");
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.Type).WithMany(x => x.NodeList).HasForeignKey(x => x.TypeID);
            modelBuilder.Entity<Sys_WorkflowTypeNode>().HasOne(x => x.PreNode).WithOne(x => x.NextNode).HasForeignKey<Sys_WorkflowTypeNode>(x => x.PreNodeID);
        }
    }

    public class LYMasterContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new EFLoggerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySql(ConfigUtil.MasterConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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
            modelBuilder.EntityToTable();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }
    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                    && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                //TODO: 拿到日志内容想怎么玩就怎么玩吧
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
