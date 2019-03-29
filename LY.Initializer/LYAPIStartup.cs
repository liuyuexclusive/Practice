using System.Collections.Generic;
using System.Linq;
using DotNetCore.CAP;
using LY.Common;
using LY.Common.Middlewares;
using LY.Common.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;

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
#if DEBUG //debug调试时不注册consul,网关指向自己
                publisher.Publish<IList<GatewayReRoute>>("GatewayConfigUtilGen", GatewayConfigUtil.Gen(LYRegister.ControllerTypes.ToArray(),true));
#else
                publisher.Publish<IList<GatewayReRoute>>("GatewayConfigUtilGen", GatewayConfigUtil.Gen(LYRegister.ControllerTypes.ToArray()));
                ConsulUtil.ServiceRegister().Wait();
#endif
            });
            appLifetime.ApplicationStopping.Register(() =>
            {
#if DEBUG //服务停止时网关重新指向consul
                publisher.Publish<IList<GatewayReRoute>>("GatewayConfigUtilGen", GatewayConfigUtil.Gen(LYRegister.ControllerTypes.ToArray()));
#else
                ConsulUtil.ServiceDeRegister().Wait();
#endif
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
