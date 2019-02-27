using LY.Common;
using LY.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Initializer
{
    public class LYStartup
    {
        public LYStartup(IConfiguration configuration)
        {
            ConfigUtil.Configuration = configuration;
        }

        public virtual IServiceProvider ConfigureServices (IServiceCollection services)
        {
            var result =  new LYRegister().Register(services);
            return result;
        }

        public Action<ModelBuilder> EntityToTable { get; set; }
    }
}
