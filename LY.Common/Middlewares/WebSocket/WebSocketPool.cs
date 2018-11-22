using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

namespace LY.Common.Middlewares
{
    public static class WebSocketPool
    {
        public static IList<LYWebSocket> List { get; set; } = new List<LYWebSocket>();

        public static void Remove(LYWebSocket webSocket)
        {
            var exist = List.FirstOrDefault(x => x.ID == webSocket.ID);
            if (exist != null)
            {
                List.Remove(exist);
            }
        }

        public static void Add(LYWebSocket socket)
        {
            if (socket != null && !string.IsNullOrEmpty(socket.ID) && !string.IsNullOrEmpty(socket.Type) && socket.WebSocket != null)
            {
                var exist = List.FirstOrDefault(x => x.ID == socket.ID);
                if (exist == null)
                {
                    List.Add(socket);
                }
            }
        }
    }
}
