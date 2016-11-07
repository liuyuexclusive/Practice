using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LY.Initializer
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExceptionFilterAttribute>();
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
            //context.ExceptionHandled = true;
        }
    }
}
