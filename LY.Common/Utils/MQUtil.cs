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
        public async static void Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    using (ResponseSocket socket = new ResponseSocket(ConfigUtil.ResponseAddress))
                    {
                        while (true)
                        {
                            MQResponseResult result = new MQResponseResult();
                            try
                            {
                                string content = socket.ReceiveFrameString();
                                var transfer = JsonConvert.DeserializeObject<MQTransfer>(content);
                                if (_publisher == null)
                                {
                                    _publisher = new PublisherSocket();
                                    _publisher.Bind(ConfigUtil.PublishAddress);
                                }
                                _publisher.SendMoreFrame(transfer.Topic).SendFrame(transfer.Data);
                            }
                            catch (Exception ex)
                            {
                                result.IsSuccessed = false;
                                LogUtil.Logger("MQ Receive Request").LogError(ex.ToString());
                            }
                            finally
                            {
                                socket.SendFrame(JsonConvert.SerializeObject(result));
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

        public static MQResponseResult Publish<T>(T data, string topic) where T : class
        {
            try
            {
                using (RequestSocket socket = new RequestSocket(ConfigUtil.ResponseAddress))
                {
                    var transfer = new MQTransfer()
                    {
                        Topic = topic
                    };
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        transfer.Data = data as string;
                    }
                    else
                    {
                        transfer.Data = JsonConvert.SerializeObject(data);
                    }
                    string content = JsonConvert.SerializeObject(transfer);
                    socket.SendFrame(content);
                    var result = JsonConvert.DeserializeObject<MQResponseResult>(socket.ReceiveFrameString());
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger("MQ Publish").LogError(ex.ToString());
                throw ex;
            }

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
