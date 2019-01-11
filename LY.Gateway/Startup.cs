using LY.Initializer;
using Microsoft.Extensions.Configuration;

namespace LY.Gateway
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup : LYGatewayStartup
    {
        public Startup(IConfiguration configuration):base(configuration)
        {
            
        }
    }
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
    //        builder.SetBasePath(ConfigUtil.CurrentDirectory)
    //               //add configuration.json
    //               .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
    //               .AddEnvironmentVariables();

    //        Configuration = builder.Build();
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        //swagger ui
    //        services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new Info { Title = "LY.Gateway", Version = "v1" });
    //            c.IncludeXmlComments(Path.Combine(ConfigUtil.CurrentDirectory, "LY.Gateway.xml"));
    //        });

    //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //            .AddJwtBearer("TestKey", options =>
    //            {
    //                options.TokenValidationParameters = new TokenValidationParameters
    //                {
    //                    ValidateIssuer = true,//是否验证Issuer
    //                    ValidateAudience = true,//是否验证Audience
    //                    ValidateLifetime = true,//是否验证失效时间
    //                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
    //                    ValidAudience = Const.JWT._audience,//Audience
    //                    ValidIssuer = Const.JWT._issuer,//Issuer，这两项和前面签发jwt的设置一致
    //                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.JWT._securityKey)),//拿到SecurityKey
    //                };
    //            });
    //        services.AddOcelot(Configuration).AddConsul();
    //        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        else
    //        {
    //            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //            app.UseHsts();
    //        }

    //        app.UseHttpsRedirection();
    //        app.UseMvc().UseSwagger().UseSwaggerUI(c =>
    //        {
    //            c.SwaggerEndpoint("/LY.SysService/swagger.json", "LY.SysService");
    //            c.SwaggerEndpoint("/LY.OrderService/swagger.json", "LY.OrderService");
    //        });
    //        app.UseWebSockets();
    //        app.UseOcelot().Wait();
    //    }
    //}
}
