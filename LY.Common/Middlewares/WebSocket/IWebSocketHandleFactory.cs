using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace LY.Common.Middlewares
{
    public interface IWebSocketHandleFactory
    {
        void Receive(string content);
        void Send(string type, string content, string id = null);
    }
}
