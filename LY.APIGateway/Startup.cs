using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using LY.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Ocelot.Provider.Consul;

namespace LY.APIGateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                   //.AddJsonFile("appsettings.json")
                   //add configuration.json
                   .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //swagger ui
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "APIGateway", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "LY.APIGateway.xml"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("TestKey", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = ConfigUtil.ConfigurationRoot["JWT:Audience"],//Audience
                        ValidIssuer = ConfigUtil.ConfigurationRoot["JWT:Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigUtil.ConfigurationRoot["JWT:SecurityKey"])),//拿到SecurityKey
                    };
                });
            services.AddOcelot(Configuration).AddConsul();
            //services.AddOcelot(Configuration).AddConsul();
            services.AddMvc();
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc().UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/sys/swagger.json", "SysService");
                c.SwaggerEndpoint("/order/swagger.json", "OrderService");
            });
            app.UseWebSockets();
            await app.UseOcelot();
        }
    }
}
