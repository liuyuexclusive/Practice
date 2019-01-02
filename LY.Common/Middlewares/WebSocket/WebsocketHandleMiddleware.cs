using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LY.Common.Middlewares
{
    public class WebsocketHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public WebsocketHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            string regex = "^/ws/(.+)/(.+)/?$";
            var type = Regex.Match(path, regex).Groups[1].Value;
            var id = Regex.Match(path, regex).Groups[2].Value;
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(id))
            {
                await _next(context);
            }

            else if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
                await new WebSocketHandler().Handle(new LYWebSocket() { ID = id, Type = type, WebSocket = socket });
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
    }
}
