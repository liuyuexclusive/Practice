using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using LY.Initializer;
using LY.Common;

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
            //globle route prefix
            services.AddMvc(options =>
            {    
                options.UseCentralRoutePrefix(new RouteAttribute("api"));
                options.Filters.Add(typeof(ExceptionFilterAttribute));
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
              options.AddPolicy("cors", p => p.WithOrigins("http://localhost:60651").AllowAnyMethod().AllowAnyHeader())
            );

            //services.AddSession();
            LYStartup startup = new LYStartup();
            return startup.StartWebAPI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            //must put in the front
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            app.UseVisitLogger();
            app.UseStaticFiles();
            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            appLifetime.ApplicationStopped.Register(() => IOCManager.Container.Dispose());

            // swagger ui
            app.UseSwagger();
            app.UseSwaggerUi("swagger");
            app.UseMvc();
            app.UseCors("cors");
        }
    }
}
