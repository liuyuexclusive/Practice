using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common.NetMQ
{
    public class MQResultDTO
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public MQResultStatus Status { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 参数JSON字符串
        /// </summary>
        public string ResultContent { get; set; }
    }
    public enum MQResultStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Sucess = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0
    }
}
