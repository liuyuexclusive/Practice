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

            builder.RegisterType(types.FirstOrDefault(t => t.Name.Equals("LYDbContext"))).As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(types.FirstOrDefault(t => t.Name.Equals("Repository`1"))).As(typeof(IRepository<>));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("UnitOfWork") || t.Name.EndsWith("Repository") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces();
        }

        private void RegisterService(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.Application"));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsSelf();
        }

        private void RegisterCommon(ContainerBuilder builder)
        {
            builder.RegisterType<LYMQ>().AsImplementedInterfaces();
        }

        private void RegisterDaemon(ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("LY.Daemon"));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.Equals("Test"))
                .AsSelf();
        }
        #endregion

        private void StartCommon(IServiceCollection services, Action action = null)
        {
            //dbContext
            services.AddDbContext<LYDbContext>();

            //autofac
            RegisterRepository(_containerBuilder);
            RegisterService(_containerBuilder);
            RegisterCommon(_containerBuilder);
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider Start(IServiceCollection services)
        {
            StartCommon(services);
            return new AutofacServiceProvider(IOCManager.Container);
        }

        public void StartDaemon(IServiceCollection services)
        {
            StartCommon(services, () => { RegisterDaemon(_containerBuilder); });
        }
    }
}
