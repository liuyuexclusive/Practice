using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace LY.Common.Middlewares
{
    public class LYWebSocket
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public WebSocket WebSocket { get; set; }
    }
}
