namespace LY.Common.LYMQ
{
    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class MQResult
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public MQResultStatus Status { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
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
