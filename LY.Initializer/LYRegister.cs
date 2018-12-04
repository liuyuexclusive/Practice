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

namespace LY.Initializer
{
    public class LYRegister
    {
        #region private
             

        private void RegisterRepository()
        {
            var assembly = Assembly.Load(new AssemblyName("LY.EFRepository"));
            var types = assembly.ExportedTypes;

            IOCManager.ContainerBuilder
                .RegisterType(types.FirstOrDefault(t => t.Name.Equals("LYDbContext")))
                .As<DbContext>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("Repository`1")))
                .As(typeof(IRepository<>))
                .PropertiesAutowired();

            IOCManager.ContainerBuilder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("UnitOfWork") || t.Name.EndsWith("Repository") || t.Name.EndsWith("Repo"))
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

        private void RegisterController(IServiceCollection services)
        {
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(Assembly.GetEntryAssembly()));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);
            var controllers = feature.Controllers;
            GatewayConfigUtil.Gen(controllers.ToArray());
            IOCManager.ContainerBuilder.RegisterTypes(controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
        }

        private void RegisterCommon(IServiceCollection services)
        {
            //dbContext
            services.AddDbContext<LYDbContext>();

            //autofac
            RegisterRepository();
            RegisterService();

            //cache
            services.AddDistributedMemoryCache();//

            ////session
            //services.AddSession();

            //redis
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                    new RedisCache(new RedisCacheOptions
                    {
                        Configuration = ConfigUtil.AppSettings["Redis:Configuration"],
                        InstanceName = "LY:"
                    })
            );

            //cors
            services.AddCors(options =>
              options.AddPolicy("cors",
                    p=>p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                   .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                   .AllowAnyMethod().AllowAnyHeader()
              )
            );
            IOCManager.ContainerBuilder.Populate(services);
        }
        #endregion


        public IServiceProvider ConfigureServicesWebAPI(IServiceCollection services)
        {
            //swagger ui
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = ConfigUtil.AppName, Version = "v1" });
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{ConfigUtil.AppName}.xml"));
            });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>()); // for PropertiesAutowired
            services.AddMvc(options =>
            {
                //options.UseCentralRoutePrefix(new RouteAttribute("api"));
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            })
.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            RegisterController(services); // for PropertiesAutowired

            RegisterCommon(services);

            return new AutofacServiceProvider(IOCManager.Container);
        }

    }
}
