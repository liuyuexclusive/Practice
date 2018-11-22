using LY.Common;
using LY.Common.API;
using LY.Common.Middlewares;
using LY.Initializer;
using LY.SysService.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LY.SysService
{
    public class Startup
    {
        public static IList<WebSocket> ListWebSocket { get; set; } = new List<WebSocket>();
        public Startup()
        {

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //swagger ui
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SysService", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "LY.SysService.xml"));
            });

            //cors
            services.AddCors(options =>
              options.AddPolicy("cors", p => p.WithOrigins(ConfigUtil.ConfigurationRoot["Cors:Origins"].Split(","))
                   //ungerlying policy
                   .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                   .AllowAnyMethod().AllowAnyHeader()
              )
            );

            //cache
            services.AddDistributedMemoryCache();//启用session之前必须先添加内存

            ////session
            //services.AddSession();

            //globle route prefix
            services.AddMvc(options =>
            {
                //options.UseCentralRoutePrefix(new RouteAttribute("api"));
                options.Filters.Add(typeof(ExceptionFilterAttribute));
            })
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            LYStartup startup = new LYStartup();
            return startup.StartWebAPI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            //must put in the front
            //app.UseSession();
            app.UseStaticFiles();
            appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());

            // swagger uis
            app.UseCors("cors");
            app.UseMvc().UseSwagger().UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "sys");
            });

            app.UseWebSockets();
            app.UseMiddleware<WebsocketHandleMiddleware>();        
            //app.UseVisitLogger();
        }
    }
}
