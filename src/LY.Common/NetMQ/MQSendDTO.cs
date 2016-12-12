using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common.NetMQ
{
    public class MQSendDTO
    {
        /// <summary>
        /// 执行类名
        /// </summary>
        public string HandlerTypeName { get; set; }
        /// <summary>
        /// 执行方法名
        /// </summary>
        public string HandlerMethodName { get; set; }
        /// <summary>
        /// 参数类名
        /// </summary>
        public string ParameterTypeName { get; set; }
        /// <summary>
        /// 参数JSON字符串
        /// </summary>
        public string ParameterContent { get; set; }
    }
}
