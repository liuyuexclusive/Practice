using DotNetCore.CAP;
using LY.Common;
using LY.Common.Middlewares;
using LY.Common.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LY.Initializer
{
    public class LYAPIStartup: LYStartup
    {
        public LYAPIStartup(IConfiguration configuration):base(configuration)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, ICapPublisher publisher, ILoggerFactory factory)
        {
            //must put in the front
            //app.UseSession();
            //appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());            
            factory.AddNLog();
            appLifetime.ApplicationStarted.Register(() =>
            {
                publisher.Publish<IList<GatewayReRoute>>("GatewayConfigUtilGen", GatewayConfigUtil.Gen(LYRegister.ControllerTypes.ToArray()));
                ConsulUtil.ServiceRegister().Wait();
            });
            appLifetime.ApplicationStopping.Register(() =>
            {
                ConsulUtil.ServiceDeRegister().Wait();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
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
