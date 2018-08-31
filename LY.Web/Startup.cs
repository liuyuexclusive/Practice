using System;
using System.Collections.Generic;
using System.Globalization;
using LY.Common;
using LY.Initializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LY.Web
{
    public class Startup
    {
        /// <summary>
        /// HostingEnvironment
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; }


        public Startup(IHostingEnvironment env)  
        {
            HostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //locallization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            //services.AddSession();
            LYStartup startup = new LYStartup();
            return startup.StartWeb(services);
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            //must put in the front
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            app.UseVisitLogger();
            app.UseStaticFiles();
            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());

            //locallization
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

            //error page
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //route
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
