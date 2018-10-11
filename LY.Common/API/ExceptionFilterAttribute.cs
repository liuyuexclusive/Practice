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
        public ExceptionFilterAttribute()
        {
        }

        public void OnException(ExceptionContext context)
        {
            LogUtil.Logger<ExceptionFilterAttribute>().LogError(context.Exception.ToString());
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new Output()
            {
                Success = false,
                Message = context.Exception.Message
            }));
            context.ExceptionHandled = true;
        }
    }
}
