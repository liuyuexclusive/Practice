using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using LY.Initializer;

namespace LY.Api
{
    public class Startup : LYStartup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //全局路由设置
            services.AddMvc(options =>
            {   // 路由参数在此处仍然是有效的，比如添加一个版本号
                options.UseCentralRoutePrefix(new RouteAttribute("api"));
            });

            // swagger ui
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
                options.IncludeXmlComments(Path.Combine(Env.ContentRootPath, "bin", "Debug", "netcoreapp1.0", Configuration["Swagger:Path"]));
                options.DescribeAllEnumsAsStrings();
            });

            return base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            base.Configure(app, loggerFactory, appLifetime);//must put in the front
            // swagger ui
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseMvc();
        }
    }
}
