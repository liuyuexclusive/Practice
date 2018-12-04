using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LY.Common
{
    public static class LogUtil
    {
        private static ILoggerFactory _loggerFactory;

        private static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_loggerFactory == null)
                {
                    _loggerFactory = new LoggerFactory();
                    _loggerFactory.AddNLog();
                }
                return _loggerFactory;
            }
        }

        public static ILogger Logger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }

        public static ILogger Logger(string categoryName)
        {
            return LoggerFactory.CreateLogger(categoryName);
        }
    }
}
