using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LY.Common
{
    public static class ConfigUtil
    {
        public static IConfiguration Configuration { get; set; }

        public static IConfigurationRoot ReadJsonFile(string path, string basePath = null, bool reloadOnChange = false)
        {
            if (basePath.IsNullOrEmpty())
            {
                basePath = ApplicationBasePath;
            }
            var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(path, optional: true, reloadOnChange: reloadOnChange);
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
                return Const._scheme + "://" + Host + ":" + Port + "/";
            }
        }

        public static string StartUrl
        {
            get
            {
                return Const._scheme + "://*:" + Port ;
            }
        }

        public static string Host
        {
            get
            {
                ///获取服务端
                string AddressIP = string.Empty;
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        AddressIP = _IPAddress.ToString();
                    }
                }
                return AddressIP;
            }
        }

        public static int Port
        {
            get
            {
                return int.Parse(ConfigUtil.ReadJsonFile("appsettings.json", ConfigUtil.CurrentDirectory)["Port"]);
            }
        }


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string MasterConnectionString
        {
            get
            {
                return Configuration.GetConnectionString("Master"); 
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string SlaveConnectionString
        {
            get
            {
                return Configuration.GetConnectionString("Slave");
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string HangfireConnectionString
        {
            get
            {
                return Configuration.GetConnectionString("Hangfire");
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string CAPConnectionString
        {
            get
            {
                return Configuration.GetConnectionString("CAP");
            }
        }

        public static string RedisAddress
        {
            get
            {
                return Configuration.GetConnectionString("Redis");
            }
        }

        public static string RabbitMQAddress
        {
            get
            {
                return Configuration.GetConnectionString("RabbitMQ");
            }
        }

        public static string ConsulUrl
        {
            get
            {
                return Configuration.GetConnectionString("Consul");
            }
        }
    }
}
