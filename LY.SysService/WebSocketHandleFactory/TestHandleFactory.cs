using LY.Common.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LY.SysService.WebSocketHandleFactory
{
    public class TestHandleFactory : IWebSocketHandleFactory
    {
        public void Receive(string content)
        {
            Send("Test", "its a test");
        }

        public void Send(string type, string content, string id = null)
        {
            Func<LYWebSocket, bool> func = x => x.Type == type && (string.IsNullOrEmpty(id) || x.ID == id);

            foreach (var item in WebSocketPool.List.Where(func))
            {
                item.WebSocket.SendAsync(Encoding.Default.GetBytes(content), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
