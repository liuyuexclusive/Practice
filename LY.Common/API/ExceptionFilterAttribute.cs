using System;
using LY.Common;
using LY.Common.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LY.Common.API
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;
        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new Output()
            {
                Success = false,
                Message = context.Exception.Message
            }));
            context.ExceptionHandled = true;
        }
    }
}
