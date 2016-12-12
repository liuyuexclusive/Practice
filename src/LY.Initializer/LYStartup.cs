using Autofac;
using Autofac.Extensions.DependencyInjection;
using LY.Domain;
using LY.EFRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LY.Initializer
{
    public class LYStartup
    {
        public LYStartup()
        {

        }

        #region private
        /// <summary>
        /// autofac容器
        /// </summary>
        private IContainer ApplicationContainer { get; set; }

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
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LYDbContext>();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();

            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
