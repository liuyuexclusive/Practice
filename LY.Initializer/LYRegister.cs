using Autofac;
using Autofac.Extensions.DependencyInjection;
using LY.Common;
using LY.Common.API;
using LY.Domain;
using LY.EFRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using LY.Common.Utils;
using Microsoft.Extensions.Caching.Redis;
using DotNetCore.CAP;

namespace LY.Initializer
{
    public class LYRegister
    {
        public static IList<TypeInfo> ControllerTypes{ get; set; }

        #region private

        private void RegisterRepository()
        {
            var assembly = Assembly.Load(new AssemblyName("LY.EFRepository"));
            var types = assembly.ExportedTypes;

            IOCManager.ContainerBuilder
                .RegisterType<LYMasterContext>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterType<LYSlaveContext>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("EntityCache`1")))
                .As(typeof(IEntityCache<>))
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("Repository`1")))
                .As(typeof(IRepository<>))
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("QueryRepository`1")))
                .As(typeof(IQueryRepository<>))
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("UnitOfWork")  || t.Name.EndsWith("Repository") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .PropertiesAutowired();

        }

        private void RegisterService()
        {
            var assembly = Assembly.Load(new AssemblyName("LY.Application"));
            IOCManager.ContainerBuilder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsSelf()
                .PropertiesAutowired();
        }

        private void RegisterController()
        {
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(Assembly.GetEntryAssembly()));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);
            ControllerTypes = feature.Controllers;

            IOCManager.ContainerBuilder.RegisterTypes(ControllerTypes.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
        }
        #endregion


        public IServiceProvider Register(IServiceCollection services)
        {
            //registCAP
            services.AddCap(x =>
            {
                // If you are using EF, you need to add the configuration：
                //x.UseEntityFramework<CAPContext>(); //Options, Notice: You don't need to config x.UseSqlServer(""") again! CAP can autodiscovery.
                x.UseMySql(ConfigUtil.CAPConnectionString);
                x.UseRabbitMQ(ConfigUtil.RabbitMQAddress);
                x.UseDashboard();
            });

            //swagger ui
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = ConfigUtil.AppName, Version = "v1" });
                c.IncludeXmlComments(Path.Combine(ConfigUtil.CurrentDirectory, $"{ConfigUtil.AppName}.xml"));
            });

            //cache
            services.AddDistributedMemoryCache();//

            ////session
            //services.AddSession();

            //redis
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                    new RedisCache(new RedisCacheOptions
                    {
                        Configuration = ConfigUtil.RedisAddress,
                        InstanceName = "LY:"
                    })
            );

            //cors
            services.AddCors(options =>
              options.AddPolicy("cors",
                    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                   .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                   .AllowAnyMethod().AllowAnyHeader()
              )
            );

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>()); // for PropertiesAutowired
            services.AddMvc(options =>
            {
                //options.UseCentralRoutePrefix(new RouteAttribute("api"));
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            })
.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //autofac
            RegisterRepository();

            RegisterService();

            RegisterController(); // for PropertiesAutowired

            IOCManager.ContainerBuilder.Populate(services);

            return new AutofacServiceProvider(IOCManager.Container);
        }

    }
}
