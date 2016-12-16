using LY.Initializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace LY.Web
{
    public class Startup : LYWebStartup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {

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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
