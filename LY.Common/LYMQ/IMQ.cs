using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common.LYMQ
{
    public interface IMQ
    {
        void StartServer();
        T Send<T>(string handlerTypeName, string handlerMethodName, object parameterObj = null) where T : MQResult;
        MQResult Send(string handlerTypeName, string handlerMethodName, object parameterObj = null);
    }
}
