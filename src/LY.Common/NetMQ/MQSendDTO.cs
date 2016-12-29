using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common.NetMQ
{
    internal class MQSendDTO
    {
        /// <summary>
        /// 执行类名
        /// </summary>
        internal string HandlerTypeName { get; set; }
        /// <summary>
        /// 执行方法名
        /// </summary>
        internal string HandlerMethodName { get; set; }
        /// <summary>
        /// 参数类名
        /// </summary>
        internal string ParameterTypeName { get; set; }
        /// <summary>
        /// 参数JSON字符串
        /// </summary>
        internal string ParameterContent { get; set; }
    }
}
