using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LY.Common
{
    public static class ConfigUtil
    {
        public static IConfigurationRoot AppSettings
        {
            get
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build();
            }
        }

        public static IConfigurationRoot LanchSettings
        {
            get
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("Properties/launchSettings.json", optional: true, reloadOnChange: true);
                return builder.Build();
            }
        }


        public static string AppName
        {
            get
            {
                return PlatformServices.Default.Application.ApplicationName;
            }
        }

        public static string ApplicationUrl
        {
            get
            {
                return LanchSettings[$"profiles:{AppName}:applicationUrl"];
            }
        }

        public static string Host
        {
            get
            {
                return new Regex(Const._httpRegex).Match(ApplicationUrl).Groups[1].Value;
            }
        }

        public static int Port
        {
            get
            {
                if (int.TryParse(new Regex(Const._httpRegex).Match(ApplicationUrl).Groups[2].Value, out int result))
                {
                    return result;
                }
                else
                {
                    throw new BusinessException("获取端口号失败");
                }
            }
        }


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnStr
        {
            get {
                return ConfigUtil.AppSettings.GetConnectionString("DefaultConnection");
            }
        }

        /// <summary>
        /// Response地址
        /// </summary>
        public static string ResponseAddress
        {
            get
            {
                return ConfigUtil.AppSettings["LYMQ:ResponseAddress"];
            }
        }

        /// <summary>
        /// Publish地址
        /// </summary>
        public static string PublishAddress
        {
            get
            {
                return ConfigUtil.AppSettings["LYMQ:PublishAddress"];
            }
        }
    }
}
