using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Api
{
    public class VisitLogMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        private VisitLog _visitLog;

        public VisitLogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<VisitLogMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _visitLog = new VisitLog();
            HttpRequest request = context.Request;
            _visitLog.Url = request.Path.ToString();
            _visitLog.Headers = request.Headers.ToDictionary(k => k.Key, v => string.Join(";", v.Value.ToList()));
            _visitLog.Method = request.Method;
            _visitLog.ExcuteStartTime = DateTime.Now;

            using (StreamReader reader = new StreamReader(request.Body))
            {
                _visitLog.RequestBody = reader.ReadToEnd();
            }

            context.Response.OnCompleted(ResponseCompletedCallback, context);
            await _next(context);
        }

        private Task ResponseCompletedCallback(object obj)
        {
            _visitLog.ExcuteEndTime = DateTime.Now;
            _logger.LogTrace($"VisitLog: {_visitLog.ToString()}");
            return Task.FromResult(0);
        }
    }
    public static class VisitLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseVisitLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitLogMiddleware>();
        }
    }
}
