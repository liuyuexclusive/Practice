using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LY.EFRepository;
using Autofac;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LY.Domain;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Text;
using NLog.Extensions.Logging;

namespace LY.Initializer
{
    public class LYStartup
    {
        public LYStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Env = env;
            Configuration = builder.Build();
        }

        /// <summary>
        /// appsettings.json配置
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        public IHostingEnvironment Env { get; }

        /// <summary>
        /// autofac容器
        /// </summary>
        private IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LYDbContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            });

            //redis
            //services.AddSingleton<IDistributedCache>(
            //    serviceProvider =>
            //        new RedisCache(new RedisCacheOptions
            //        {
            //            Configuration = Configuration["Redis:Configuration"],
            //            InstanceName = "LY:"
            //        })
            //);
            //services.AddSession();

            //autofac
            var builder = new ContainerBuilder();

            RegisterRepository(builder);
            RegisterService(builder);

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        private void RegisterRepository(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.EFRepository"));
            var types = assembly.ExportedTypes;

            builder.RegisterType(types.FirstOrDefault(t => t.Name.Equals("LYDbContext"))).As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("Repository`1"))).As(typeof(IRepository<>));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("UnitOfWork") || t.Name.EndsWith("Repository") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces();
        }

        private void RegisterService(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.Application"));
            var types = assembly.ExportedTypes;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("Service"))
                .AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();
            app.UseVisitLogger();
            app.UseStaticFiles();

            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
