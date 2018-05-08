using LY.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common.LYMQ
{
    public interface IMQ
    {
        void StartServer();
        MQResultDTO Send(string handlerTypeName, string handlerMethodName, object parameterObj = null);
    }
}
