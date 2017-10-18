using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using LY.Common;

namespace LY.Initializer
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public ExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
        }

        public void OnException(ExceptionContext context)
        {
            LogUtil.Logger<ExceptionFilterAttribute>().LogError(context.Exception.ToString());
            //context.ExceptionHandled = true;
        }
    }
}
