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
        public static IConfigurationRoot ReadJsonFile(string path, string basePath = null)
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
                return Const._scheme + "://" + Host + ":" + Port + "/";
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
                //return "172.16.210.140";
            }
        }

        public static int Port
        {
            get
            {
                return 80;
                //return int.Parse(ReadJsonFile("appSettings.json",CurrentDirectory)["Port"]);
            }
        }


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ReadJsonFile("connectionString.json").GetConnectionString("DefaultConnection");
            }
        }

        /// <summary>
        /// Response地址
        /// </summary>
        public static string ResponseAddress
        {
            get
            {
                return ReadJsonFile("mqSettings.json")["ResponseAddress"];
            }
        }

        /// <summary>
        /// Publish地址
        /// </summary>
        public static string PublishAddress
        {
            get
            {
                return ReadJsonFile("mqSettings.json")["PublishAddress"];
            }
        }

        public static string RedisAddress
        {
            get
            {
                return ReadJsonFile("redisSettings.json")["Address"];
            }
        }

        public static string ConsulUrl
        {
            get
            {
                return ReadJsonFile("consulSettings.json")["Address"];
            }
        }
    }
}
