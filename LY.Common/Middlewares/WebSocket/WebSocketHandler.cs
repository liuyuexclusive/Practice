using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LY.Common.Middlewares
{
    public class WebSocketHandler
    {
        public async Task Handle(LYWebSocket socket)
        {
            WebSocketPool.Add(socket);
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = null;
            while (true)
            {
                result = await socket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                {
                    break;
                }
                Receive(Encoding.Default.GetString(buffer));
            }            
            WebSocketPool.Remove(socket);
            await socket.WebSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private void Receive(string content)
        {
            var handlerTypes = Assembly.GetEntryAssembly().ExportedTypes.Where(x => 
                (typeof(IWebSocketHandleFactory)).IsAssignableFrom(x)                
            );

            var type = handlerTypes.FirstOrDefault();

            if (type != null)
            {
                ((IWebSocketHandleFactory)Activator.CreateInstance(type)).Receive(content);
            }
        }
    }
}
