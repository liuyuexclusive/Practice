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
        private static IConfigurationRoot AppSettings
        {
            get
            {
                return ReadJsonFile("appsettings.json", CurrentDirectory);
            }
        }


        private static IConfigurationRoot ConnectionStringSettings
        {
            get
            {
                return ReadJsonFile("connectionString.json");
            }
        }

        private static IConfigurationRoot RedisSettings
        {
            get
            {
                return ReadJsonFile("redisSettings.json");
            }
        }

        private static IConfigurationRoot MQSettings
        {
            get
            {
                return ReadJsonFile("mqSettings.json");
            }
        }

        private static IConfigurationRoot ReadJsonFile(string path, string basePath = null)
        {
            if (basePath.IsNullOrEmpty())
            {
                basePath = ApplicationBasePath;
            }
            var builder = new ConfigurationBuilder()
.SetBasePath(basePath)
.AddJsonFile(path, optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public static string ApplicationBasePath
        {
            get
            {
                return PlatformServices.Default.Application.ApplicationBasePath;
            }
        }

        public static string CurrentDirectory
        {
            get
            {
                return Directory.GetCurrentDirectory();
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
                var address = AppSettings["Address"];
                if (string.IsNullOrEmpty(address))
                {
                    throw new Exception("无法获取启动地址,请检查配置文件appsettings.json");
                }
                if (!new Regex(Const.Regex._httpAddressRegex).IsMatch(address))
                {
                    throw new Exception("启动地址格式错误，请检查配置文件appsettings.json");
                }
                var url = Const._scheme + "://" + address + "/";
                return url;
            }
        }

        public static string Host
        {
            get
            {
                return new Regex(Const.Regex._httpUrlRegex).Match(ApplicationUrl).Groups[1].Value;
            }
        }

        public static int Port
        {
            get
            {
                if (int.TryParse(new Regex(Const.Regex._httpUrlRegex).Match(ApplicationUrl).Groups[2].Value, out int result))
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
            get
            {
                return ConfigUtil.ConnectionStringSettings.GetConnectionString("DefaultConnection");
            }
        }

        /// <summary>
        /// Response地址
        /// </summary>
        public static string ResponseAddress
        {
            get
            {
                return ConfigUtil.MQSettings["ResponseAddress"];
            }
        }

        /// <summary>
        /// Publish地址
        /// </summary>
        public static string PublishAddress
        {
            get
            {
                return ConfigUtil.MQSettings["PublishAddress"];
            }
        }

        public static string RedisAddress
        {
            get { return ConfigUtil.RedisSettings["Address"]; }
        }
    }
}
