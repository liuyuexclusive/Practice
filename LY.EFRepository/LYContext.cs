using LY.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace LY.EFRepository
{
    public class LYContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
= new LoggerFactory(new[] { new EFLoggerProvider() });
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IOCManager.Resolve<IEntityToTable>(x => x.Excute(modelBuilder));
            base.OnModelCreating(modelBuilder);
        }
    }

    public sealed class LYMasterContext : LYContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySql(ConfigUtil.MasterConnectionString);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseMySql(ConfigUtil.MasterConnectionString);
        }
    }

    public sealed class LYSlaveContext : LYContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(ConfigUtil.SlaveConnectionString);
        }
    }

    class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }

    class EFLogger : ILogger
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
                //打印到控制台
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
