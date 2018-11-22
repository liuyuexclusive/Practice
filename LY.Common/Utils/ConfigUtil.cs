using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common
{
    public class ConfigUtil
    {
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build();
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnStr
        {
            get {
                return ConfigUtil.ConfigurationRoot.GetConnectionString("DefaultConnection");
            }
        }
    }
}
