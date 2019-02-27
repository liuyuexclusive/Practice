using Autofac;
using DotNetCore.CAP;
using LY.Common;
using LY.Domain.Sys;
using LY.EFRepository;
using LY.Initializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.SysService
{
    public class Startup: LYAPIStartup
    {
        public Startup(IConfiguration configuration):base(configuration)
        {

        }
    }
}
