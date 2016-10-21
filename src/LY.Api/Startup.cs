using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using LY.Domain.Sys;
using LY.Domain;
using Microsoft.EntityFrameworkCore;
using LY.EFRepository;
using LY.EFRepository.Sys;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Text;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using LY.Common;

namespace LY.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            BasePath = env.ContentRootPath;
            var builder = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfigurationRoot Configuration { get; }

        public string BasePath { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
<<<<<<< HEAD
=======
            //services.AddMvc();
>>>>>>> 96ce407dca7eb7f8eba7e35427d494e093cc7692
            //数据库
            services.AddDbContext<LYDbContext>();
            //全局路由设置
            services.AddMvc(options =>
            {   // 路由参数在此处仍然是有效的，比如添加一个版本号
                options.UseCentralRoutePrefix(new RouteAttribute("api"));
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
            #region swagger ui
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "Geo Search API",
                    Description = "A simple api to search using geo location in Elasticsearch",
                    TermsOfService = "None"
                });
                options.IncludeXmlComments(Path.Combine(BasePath, "bin", "Debug", "netcoreapp1.0", Configuration["Swagger:Path"]));
                options.DescribeAllEnumsAsStrings();
            });
            #endregion

            //autofac
            var builder = new ContainerBuilder();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<LYDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Role>>().As<IRepository<Role>>().InstancePerLifetimeScope();
            builder.RegisterType<RoleRepo>().As<IRoleRepo>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<User>>().As<IRepository<User>>().InstancePerLifetimeScope();

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddNLog();
            app.UseVisitLogger();
            app.UseSwagger();
            app.UseSwaggerUi();
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());

            app.UseMvc();
        }
    }
}
