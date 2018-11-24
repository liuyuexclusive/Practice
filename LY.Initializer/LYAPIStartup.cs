using LY.Common;
using LY.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Initializer
{
    public class LYAPIStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return new LYRegister().StartWebAPI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            //must put in the front
            //app.UseSession();
            //appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());

            // swagger uis
            app.UseCors("cors");
            app.UseMvc().UseSwagger().UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", PlatformServices.Default.Application.ApplicationName);
            });

            //websocket
            app.UseWebSockets();
            app.UseMiddleware<WebsocketHandleMiddleware>();
        }
    }
}
