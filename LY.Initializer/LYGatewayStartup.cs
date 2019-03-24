using LY.Common;
using LY.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Text;

namespace LY.Initializer
{
    public class LYGatewayStartup : LYStartup
    {
        public LYGatewayStartup(IConfiguration configuration) : base(configuration)
        {
            OcelotConfiguration = ConfigUtil.ReadJsonFile("configuration.json", ConfigUtil.CurrentDirectory, true);
        }


        public IConfiguration OcelotConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(Const.JWT._providerKey, option => option.TokenValidationParameters = JwtUtil.BuildTokenValidationParameters()
            );
            services.AddOcelot(OcelotConfiguration).AddConsul();
            return base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            //factory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc().UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/LY.SysService/swagger.json", "LY.SysService");
                c.SwaggerEndpoint("/LY.OrderService/swagger.json", "LY.OrderService");
            });
            app.UseWebSockets();
            app.UseOcelot().Wait();
        }
    }
}
