using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common.API
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
        /// <summary>q
        /// 结果数据
        /// </summary>
        public T Data { get; set; }

        public int? Total { get; set; }
    }

    /// <summary>
    /// 返回列表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OutputList<T> : Output
    {
        /// <summary>q
        /// 结果数据
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 总数
        /// </summary>
        public int? Total { get; set; } 
    }
}
