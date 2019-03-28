using LY.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LY.Common.API
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogFilterAttribute> _logger;

        private Stopwatch _stopwatch;

        public LogFilterAttribute(ILogger<LogFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            string token = context.HttpContext.Request.Headers["Authorization"].ToString().TrimStart(JwtBearerDefaults.AuthenticationScheme.ToArray()).Trim();
            if (!string.IsNullOrEmpty(token) && token!= "undefined" && string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
            {
                try
                {
                    context.HttpContext.User = JwtUtil.GetClaimsPrincipal(token);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var metadata = context.ActionDescriptor.EndpointMetadata;
            //skip Get or UnAuthorize
            if (metadata.Any(x => x is HttpGetAttribute || x is UnAuthorizeAttribute))
            {
                return;
            }
            var log = new VisitLog();
            var request = context.HttpContext.Request;
            log.Url = $"{ConfigUtil.ApplicationUrl}{request.Path.ToString()}";
            log.Method = request.Method;
            log.Headers = request.Headers.ToDictionary(k => k.Key, v => string.Join(";", v.Value.ToList()));
            request.EnableBuffering();
            request.Body.Position = 0;
            using (StreamReader sr = new StreamReader(request.Body))
            {
                log.RequestBody = sr.ReadToEnd();
            }

            log.Elapsed = _stopwatch.Elapsed.TotalMilliseconds;
            var result = context.Result as ObjectResult;
            if (result != null)
            {
                log.Result = JsonConvert.SerializeObject(result.Value);
            }

            _logger.LogInformation(log.ToString());
        }
    }

    public class VisitLog
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public double Elapsed { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public string RequestBody { get; set; }

        public string Result { get; set; }

        public override string ToString()
        {
            string headers = "[" + string.Join(",", this.Headers.Select(i => "{" + $"\"{i.Key}\":\"{i.Value}\"" + "}")) + "]";
            return $"Url: {this.Url},\r\nMethod: {this.Method},\r\nElapsed: {this.Elapsed},\r\nHeaders: {headers},\r\nRequestBody: {this.RequestBody},\r\nResult: {this.Result}";
        }
    }
}
