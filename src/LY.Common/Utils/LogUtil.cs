using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common
{
    public static class LogUtil
    {
        private static ILoggerFactory _loggerFactory;

        public static ILogger Logger<T>()
        {

            if (_loggerFactory == null)
            {
                _loggerFactory = new LoggerFactory();
                _loggerFactory.AddNLog();
            }
            return _loggerFactory.CreateLogger<T>();
        }
    }
}
