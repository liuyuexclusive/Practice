using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LY.Common.Utils
{
    public static class MQUtil
    {
        public static object _lockPublishObj = new object();

        /// <summary>
        /// MQ服务端地址
        /// </summary>
        private static string Address
        {
            get
            {
                return ConfigUtil.ConfigurationRoot["LYMQ:Address"];
            }
        }

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
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="topic">主题</param>
        public static async void Publish<T>(T data, string topic) where T : class
        {
            await Task.Run(() => {
                try
                {
                    string content = JsonConvert.SerializeObject(data);
                    lock (_lockPublishObj)//防止并发导致发送失败
                    {
                        if (_publisher == null)
                        {
                            _publisher = new PublisherSocket();
                            _publisher.Bind(Address);
                        }
                        _publisher.SendMoreFrame(topic).SendFrame(content);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger("Publish").LogError(ex.ToString());
                }
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">收到数据后的行为</param>
        /// <param name="topic">主题</param>
        public static async void Subscrib<T>(Action<T> action, string topic) where T : class
        {
            await Task.Run(() =>
            {
                try
                {
                    using (SubscriberSocket subscriber = new SubscriberSocket())
                    {
                        subscriber.Connect(Address);
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
                                if (typeof(T).IsAssignableFrom(typeof(string)))
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
                                LogUtil.Logger("ReceiveFrameString").LogError(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger("Subscrib").LogError(ex.ToString());
                }
            });
        }
    }
}
