using LY.Common;
using LY.Initializer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace LY.WebAPI
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = ConfigUtil.ConfigurationRoot["JWT:Audience"],//Audience
                        ValidIssuer = ConfigUtil.ConfigurationRoot["JWT:Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigUtil.ConfigurationRoot["JWT:SecurityKey"]))//拿到SecurityKey
                    };
                });

            //swagger ui
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "Geo Search API",
                    Description = "A simple api to search using geo location in Elasticsearch",
                    TermsOfService = "None"
                });
                options.IncludeXmlComments(Path.Combine(HostingEnvironment.ContentRootPath, "bin", "Debug", "netcoreapp2.0", ConfigUtil.ConfigurationRoot["Swagger:Path"]));
                options.DescribeAllEnumsAsStrings();
            });

            //cors
            services.AddCors(options =>
              options.AddPolicy("cors", p => p.WithOrigins(ConfigUtil.ConfigurationRoot["Cors:Origins"].Split(",")).SetPreflightMaxAge(TimeSpan.FromSeconds(3600)).AllowAnyMethod().AllowAnyHeader())
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
            app.UseAuthentication();
            app.UseStaticFiles();
            appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());

            // swagger ui
            app.UseSwagger();
            app.UseSwaggerUi("swagger");
            app.UseHttpsRedirection();
            app.UseCors("cors");
            app.UseMvc();
            app.UseVisitLogger();
        }
    }
}
