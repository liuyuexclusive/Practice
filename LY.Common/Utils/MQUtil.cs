using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LY.Common.Utils
{
    public class MQTransfer
    {
        public string Topic { get; set; }
        public string Data { get; set; }
    }

    public class MQResponseResult
    {
        public bool IsSuccessed { get; set; } = true;
        public string Message { get; set; }
    }

    public static class MQUtil
    {

        public static object _lockPublish = new object();

        private static PublisherSocket _publisher;

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            _publisher.Close();
            _publisher.Dispose();
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async static Task Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_publisher == null)
                    {
                        _publisher = new PublisherSocket(ConfigUtil.PublishAddress);
                    }
                    using (ResponseSocket socket = new ResponseSocket())
                    {
                        socket.Bind(ConfigUtil.ResponseAddress);
                        while (true)
                        {
                            MQResponseResult result = new MQResponseResult();
                            try
                            {
                                string content = socket.ReceiveFrameString();
                                var transfer = JsonConvert.DeserializeObject<MQTransfer>(content);
                                _publisher.SendMoreFrame(transfer.Topic).SendFrame(transfer.Data);
                                socket.SendFrame(JsonConvert.SerializeObject(result));
                            }
                            catch (Exception ex)
                            {
                                result.IsSuccessed = false;
                                socket.SendFrame(JsonConvert.SerializeObject(result));
                                LogUtil.Logger("MQ Receive Request").LogError(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger("MQ Start").LogError(ex.ToString());
                }
            });
        }

        public static Task<MQResponseResult> Publish<T>(T data, string topic) where T : class
        {
            return Task<MQResponseResult>.Run(() =>
            {
                try
                {
                    using (RequestSocket socket = new RequestSocket())
                    {
                        socket.Connect(ConfigUtil.ResponseAddress);
                        var transfer = new MQTransfer()
                        {
                            Topic = topic
                        };
                        if (typeof(string).IsAssignableFrom(typeof(T)))
                        {
                            transfer.Data = data as string;
                        }
                        else
                        {
                            transfer.Data = JsonConvert.SerializeObject(data);
                        }
                        string content = JsonConvert.SerializeObject(transfer);

                        if (socket.TrySendFrame(content))
                        {
                            var result = JsonConvert.DeserializeObject<MQResponseResult>(socket.ReceiveFrameString());
                            return result;
                        }
                        return new MQResponseResult() { IsSuccessed = false, Message = "发送失败" };
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger("MQ Publish").LogError(ex.ToString());
                    throw ex;
                }
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">收到数据后的行为</param>
        /// <param name="topic">主题</param>
        public static async Task Subscrib<T>(Action<T> action, string topic) where T : class
        {
            await Task.Run(() =>
            {
                try
                {
                    using (SubscriberSocket subscriber = new SubscriberSocket())
                    {
                        subscriber.Connect(ConfigUtil.PublishAddress);
                        subscriber.Subscribe(topic);
                        while (true)
                        {
                            try
                            {
                                string receiveStr = subscriber.ReceiveFrameString();
                                if (receiveStr == topic)
                                {
                                    continue;
                                }
                                T result = default(T);
                                if (typeof(string).IsAssignableFrom(typeof(T)))
                                {
                                    result = receiveStr as T;
                                }
                                else
                                {
                                    result = JsonConvert.DeserializeObject<T>(receiveStr);
                                }
                                action(result);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Logger("MQ Receive Subscrib").LogError(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger("MQ Subscrib").LogError(ex.ToString());
                }
            });
        }
    }
}
