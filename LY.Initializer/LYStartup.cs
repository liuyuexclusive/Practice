using Autofac;
using Autofac.Extensions.DependencyInjection;
using LY.Common;
using LY.Common.LYMQ;
using LY.Domain;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace LY.Initializer
{
    public class LYStartup
    {
        private readonly ContainerBuilder _containerBuilder;

        public LYStartup()
        {
            _containerBuilder = IOCManager.ContainerBuilder;
        }

        #region private
        private void RegisterRepository(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.EFRepository")); 
            var types = assembly.ExportedTypes;

            builder
                .RegisterType(types.FirstOrDefault(t => t.Name.Equals("LYDbContext")))
                .As<DbContext>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder
                .RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("Repository`1")))
                .As(typeof(IRepository<>))
                .PropertiesAutowired();

            builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("UnitOfWork") || t.Name.EndsWith("Repository") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }

        private void RegisterService(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.Application"));

            builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsSelf()
                .PropertiesAutowired();
        }

        private void RegisterCommon(ContainerBuilder builder)
        { 
            builder
                .RegisterType<LYMQ>()
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }

        private void StartCommon(IServiceCollection services, Action action = null)
        {
            //dbContext
            services.AddDbContext<LYDbContext>();

            //autofac
            RegisterCommon(_containerBuilder);
            RegisterRepository(_containerBuilder);
            RegisterService(_containerBuilder);
            
            if (action != null)
            {
                action.Invoke();
            }

            //redis
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                    new RedisCache(new RedisCacheOptions
                    {
                        Configuration = ConfigUtil.ConfigurationRoot["Redis:Configuration"],
                        InstanceName = "LY:"
                    })
            );
            _containerBuilder.Populate(services);
        }
        #endregion


        public IServiceProvider StartWebAPI(IServiceCollection services)
        {
            StartCommon(services);
            return new AutofacServiceProvider(IOCManager.Container);
        }

        public IServiceProvider StartWeb(IServiceCollection services)
        {
            StartCommon(services);
            return new AutofacServiceProvider(IOCManager.Container);
        }

        public void StartDaemon(IServiceCollection services)
        {
            StartCommon(services, () => {

                var assembly = Assembly.Load(new AssemblyName("LY.Daemon"));

                _containerBuilder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.Equals("Test"))
                    .AsSelf()
                    .PropertiesAutowired();
            });
        }
    }
}
