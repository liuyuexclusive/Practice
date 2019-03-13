using DotNetCore.CAP;
using LY.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LY.Common.Utils
{
    public static class GatewayConfigUtil
    {
        /// <summary>
        /// 生成配置
        /// </summary>
        /// <param name="controllers">控制器信息</param>
        /// <param name="doNotUseConsul">是否不使用consul</param>
        /// <returns></returns>
        public static IList<GatewayReRoute> Gen(TypeInfo[] controllers, bool doNotUseConsul = false)
        {
            if (controllers.IsNullOrEmpty())
            {
                return new List<GatewayReRoute>();
            }
            IList<GatewayReRoute> listResult = new List<GatewayReRoute>();

            //socket
            listResult.Add(new GatewayReRoute()
            {
                AppName = ConfigUtil.AppName,
                UpstreamPathTemplate = "/ws/" + ConfigUtil.AppName + "/{type}/{id}",
                DownstreamPathTemplate = "/ws/{type}/{id}",
                DownstreamHostAndPorts = new List<GatewayReRouteDownstreamHostAndPort>() {
                                new GatewayReRouteDownstreamHostAndPort(){
                                    Host = ConfigUtil.Host,
                                    Port = ConfigUtil.Port
                                }
                            },
                DownstreamScheme = "ws" + Const._scheme.TrimStart("http".ToArray())
            });

            //swagger

            var swaggerGatewayReRoute = new GatewayReRoute()
            {
                AppName = ConfigUtil.AppName,
                UpstreamPathTemplate = $"/{ConfigUtil.AppName}/swagger.json",
                DownstreamPathTemplate = "/swagger/v1/swagger.json",
            };

            if (!doNotUseConsul)
            {
                swaggerGatewayReRoute.ServiceName = ConfigUtil.AppName;
                swaggerGatewayReRoute.LoadBalancerOptions = new LoadBalancerOptions();
            }
            else
            {
                swaggerGatewayReRoute.DownstreamHostAndPorts = new List<GatewayReRouteDownstreamHostAndPort>() {
                                new GatewayReRouteDownstreamHostAndPort(){
                                    Host = ConfigUtil.Host,
                                    Port = ConfigUtil.Port
                                }
                            };
            }

            listResult.Add(swaggerGatewayReRoute);

            foreach (var controller in controllers)
            {
                foreach (var method in controller.DeclaredMethods)
                {
                    var httpMethods = method.CustomAttributes.Where(x => typeof(HttpMethodAttribute).IsAssignableFrom(x.AttributeType));
                    if (httpMethods.Count() > 0)
                    {
                        var isUnAuthorize = method.CustomAttributes.Where(x => typeof(UnAuthorizeAttribute).IsAssignableFrom(x.AttributeType)).Count() > 0;
                        var routeAttribute = method.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
                        string route = routeAttribute != null ? routeAttribute.Template : string.Empty;
                        string template = $"/{controller.Name.Replace("Controller", string.Empty)}/{route}";

                        var actionGatewayReRoute = new GatewayReRoute()
                        {
                            AppName = ConfigUtil.AppName,
                            AuthenticationOptions = isUnAuthorize ? null : new GatewayRouteAuthenticationOption(),
                            UpstreamHttpMethod = httpMethods.Select(x => x.AttributeType.Name.GetHttpMethod()).ToList(),
                            DownstreamPathTemplate = template,
                            UpstreamPathTemplate = template,
                        };

                        if (!doNotUseConsul)
                        {
                            actionGatewayReRoute.ServiceName = ConfigUtil.AppName;
                            actionGatewayReRoute.LoadBalancerOptions = new LoadBalancerOptions();
                        }
                        else
                        {
                            actionGatewayReRoute.DownstreamHostAndPorts = new List<GatewayReRouteDownstreamHostAndPort>() {
                                new GatewayReRouteDownstreamHostAndPort(){
                                    Host = ConfigUtil.Host,
                                    Port = ConfigUtil.Port
                                }
                            };
                        }

                        listResult.Add(actionGatewayReRoute);
                    }
                }
            }
            return listResult;
        }


        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="reRoutes"></param>
        public static void Update(string configPath, IList<GatewayReRoute> reRoutes)
        {
            var config = JsonConvert.DeserializeObject<GatewayConfig>(File.ReadAllText(configPath));
            if (config != null)
            {
                var apps = reRoutes.Select(x => x.AppName).Distinct();
                config.ReRoutes = config.ReRoutes.Except(config.ReRoutes.Where(x => apps.Contains(x.AppName))).ToList();
                var existUpstreamPathTemplates = config.ReRoutes.Select(x => x.UpstreamPathTemplate);
                var needAddList = reRoutes
                    .Except(reRoutes.Where(x => existUpstreamPathTemplates.Contains(x.UpstreamPathTemplate)));
                foreach (var item in needAddList)
                {
                    config.ReRoutes.Add(item);
                }
            }
            File.WriteAllText(configPath, JsonConvert.SerializeObject(config, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
