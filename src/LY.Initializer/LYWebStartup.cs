﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using LY.Common;
using System.Text;

namespace LY.Initializer
{
    public class LYWebStartup
    {
        public LYWebStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            HostingEnvironment = env;
            ConfigurationRoot = builder.Build();
        }

        /// <summary>
        /// appsettings.json配置
        /// </summary>
        public IConfigurationRoot ConfigurationRoot { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
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
            LYStartup startup = new LYStartup();
            return startup.Start(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            app.UseVisitLogger();
            app.UseStaticFiles();
            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            appLifetime.ApplicationStopped.Register(() => IocManager.Container.Dispose());
        }
    }
}
