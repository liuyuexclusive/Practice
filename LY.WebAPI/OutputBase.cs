using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.WebAPI
{
    /// <summary>
    /// 输出参数
    /// </summary>
    public class Output
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

    }

    /// <summary>
    /// 返回单个数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Output<T> : Output
    {
        public Output() {
            base.Success = true;
            Message = "操作成功";
        }

        /// <summary>q
        /// 结果数据
        /// </summary>
        public T Data { get; set; }
    }
}
