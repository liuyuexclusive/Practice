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
.AddJsonFile(path, optional: true, reloadOnChange: false);
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
        public static string MasterConnectionString
        {
            get
            {
#if DEBUG
                return "server=127.0.0.1;port=3306;uid=root;pwd=123456;DataBase=ly;charset=utf8;max pool size=1000;AllowUserVariables=True;";
#endif
                return ReadJsonFile("connectionString.json").GetConnectionString("MasterConnection");
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string SlaveConnectionString
        {
            get
            {
#if DEBUG
                return "server=127.0.0.1;port=3307;uid=root;pwd=123456;DataBase=ly;charset=utf8;max pool size=1000;AllowUserVariables=True;";
#endif
                return ReadJsonFile("connectionString.json").GetConnectionString("SlaveConnection");
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string HangfireConnectionString
        {
            get
            {
#if DEBUG
                return "server=127.0.0.1;port=3306;uid=root;pwd=123456;DataBase=ly_hangfire;charset=utf8;max pool size=1000;AllowUserVariables=True;";
#endif
                return ReadJsonFile("connectionString.json").GetConnectionString("HangfireConnection");
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string CAPConnectionString
        {
            get
            {
#if DEBUG
                return "server=127.0.0.1;port=3306;uid=root;pwd=123456;DataBase=ly_cap;charset=utf8;max pool size=1000;AllowUserVariables=True;";
#endif
                return ReadJsonFile("connectionString.json").GetConnectionString("CAPConnection");
            }
        }
        
        public static string RedisAddress
        {
            get
            {
#if DEBUG
                return $"127.0.0.1:{Const.Port._redis}";
#endif
                return $"{Const.IP._redis}:{Const.Port._redis}";
            }
        }

        public static string RabbitMQAddress
        {
            get
            {
#if DEBUG
                return $"127.0.0.1";
#endif
                return $"{Const.IP._rabbitmq}";
            }
        }

        public static string ConsulUrl
        {
            get
            {
#if DEBUG
                return $"http://127.0.0.1:{Const.Port._consul}";
#endif
                return $"http://{Const.IP._consul}:{Const.Port._consul}";
            }
        }
    }
}
