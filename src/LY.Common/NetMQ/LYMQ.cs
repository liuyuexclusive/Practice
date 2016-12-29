using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;

namespace LY.Common.NetMQ
{
    public class LYMQ
    {
        public LYMQ()
        {
        }

        /// <summary>
        /// 开始服务
        /// </summary>
        public void StartServer()
        {
            var parameterTypes = Assembly.GetEntryAssembly().ExportedTypes.Where(x => (typeof(IMQParameter)).IsAssignableFrom(x));
            var handlerTypes = Assembly.GetEntryAssembly().ExportedTypes.Where(x => (typeof(IMQHandler)).IsAssignableFrom(x));

            using (NetMQSocket serverSocket = new ResponseSocket())
            {
                serverSocket.Bind(Address);
                while (true)
                {
                    try
                    {
                        string message = serverSocket.ReceiveFrameString();
                        var transfer = JsonConvert.DeserializeObject<MQSendDTO>(message);

                        var parameters = new List<object> { };
                        if (!string.IsNullOrEmpty(transfer.ParameterContent) && !string.IsNullOrEmpty(transfer.ParameterTypeName))
                        {
                            var parameterType = parameterTypes.FirstOrDefault(x => x.Name == transfer.ParameterTypeName);
                            if (parameterType != null)
                            {
                                var parameterObj = JsonConvert.DeserializeObject(transfer.ParameterContent, parameterType);
                                if (parameterObj != null)
                                {
                                    parameters.Add(parameterObj);
                                }
                            }
                        }

                        var handlerType = handlerTypes.FirstOrDefault(x => x.Name == transfer.HandlerTypeName);
                        if (handlerType != null && !string.IsNullOrEmpty(transfer.HandlerMethodName))
                        {
                            handlerType.GetMethod(transfer.HandlerMethodName).Invoke(Activator.CreateInstance(handlerType), parameters.ToArray());
                        }
                        serverSocket.SendFrame(JsonConvert.SerializeObject(new MQResultDTO() { Status = MQResultStatus.Sucess, Msg = "OK" }));
                    }
                    catch (Exception ex)
                    {
                        serverSocket.SendFrame(JsonConvert.SerializeObject(new MQResultDTO() { Status = MQResultStatus.Fail, Msg = ex.Message }));
                        LogUtil.Logger<LYMQ>().LogError(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="handlerTypeName">类名</param>
        /// <param name="handlerMethodName">方法名</param>
        /// <param name="parameterObj">参数对象</param>
        public MQResultDTO Send(string handlerTypeName, string handlerMethodName, object parameterObj = null)
        {
            using (NetMQSocket clientSocket = new RequestSocket())
            {
                clientSocket.Connect(Address);

                string parameterTypeName = string.Empty;
                string parameterContent = string.Empty;
                if (parameterObj != null)
                {
                    parameterTypeName = parameterObj.GetType().Name;
                    parameterContent = JsonConvert.SerializeObject(parameterObj);
                }
                MQSendDTO transfer = new MQSendDTO()
                {
                    HandlerTypeName = handlerTypeName,
                    HandlerMethodName = handlerMethodName,
                    ParameterTypeName = parameterTypeName,
                    ParameterContent = parameterContent
                };
                clientSocket.SendFrame(JsonConvert.SerializeObject(transfer));

                MQResultDTO result = JsonConvert.DeserializeObject<MQResultDTO>(clientSocket.ReceiveFrameString());
                return result;
            }
        }

        /// <summary>
        /// MQ服务端地址
        /// </summary>
        private string Address
        {
            get
            {
                return ConfigUtil.ConfigurationRoot["LYMQ:Address"];
            }
        }
    }
}
