using System;
using LY.Initializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LY.Web
{
    public class Startup : LYWebStartup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {

        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            base.Configure(app, appLifetime);//must put in the front

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
