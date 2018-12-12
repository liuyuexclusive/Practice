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
        /// <param name="serviceName"></param>
        /// <param name="controllers"></param>
        public static async Task Gen(params TypeInfo[] controllers)
        {
            if (controllers.IsNullOrEmpty())
            {
                return;
            }
            IList<GatewayReRoute> listResult = new List<GatewayReRoute>();

            //socket
            listResult.Add(new GatewayReRoute()
            {
                UpstreamPathTemplate = "/ws/" + ConfigUtil.AppName + "/{type}/{id}",
                DownstreamPathTemplate = "/ws/{type}/{id}",
                DownstreamHostAndPorts = new List<GatewayReRouteDownstreamHostAndPort>() {
                                new GatewayReRouteDownstreamHostAndPort(){
                                    Host = ConfigUtil.Host,
                                    Port = ConfigUtil.Port
                                }
                            },
                AppName = ConfigUtil.AppName,
                DownstreamScheme = "ws" + Const._scheme.TrimStart("http".ToArray()) 
            });

            //swagger
            listResult.Add(new GatewayReRoute()
            {
                UpstreamPathTemplate = $"/{ConfigUtil.AppName}/swagger.json",
                DownstreamPathTemplate = "/swagger/v1/swagger.json",
                AppName = ConfigUtil.AppName,
                ServiceName = ConfigUtil.AppName,
                LoadBalancerOptions = new LoadBalancerOptions()
            });

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
                        string template = $"/{controller.Name.Replace("Controller",string.Empty)}/{route}";
                        listResult.Add(new GatewayReRoute()
                        {
                            AuthenticationOptions = isUnAuthorize ? null : new GatewayRouteAuthenticationOption(),
                            //DownstreamHostAndPorts = new List<GatewayReRouteDownstreamHostAndPort>() {
                            //    new GatewayReRouteDownstreamHostAndPort(){
                            //        Host = ConfigUtil.Host,
                            //        Port = ConfigUtil.Port
                            //    }
                            //},
                            DownstreamPathTemplate = template,
                            UpstreamPathTemplate = template,
                            UpstreamHttpMethod = httpMethods.Select(x => x.AttributeType.Name.GetHttpMethod()).ToList(),
                            AppName = ConfigUtil.AppName,
                            ServiceName = ConfigUtil.AppName,
                            LoadBalancerOptions = new LoadBalancerOptions()
                        });
                    }
                }
            }

            await MQUtil.Publish(listResult, "GatewayConfigUtilGen");
        }


        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="reRoutes"></param>
        public static void Update(string configPath,IList<GatewayReRoute> reRoutes)
        {
            var config = JsonConvert.DeserializeObject<GatewayConfig>(File.ReadAllText(configPath));
            if (config != null)
            {
                var apps = reRoutes.Select(x => x.AppName).Distinct();
                config.ReRoutes = config.ReRoutes.Except(config.ReRoutes.Where(x=> apps.Contains(x.AppName) && !new Regex(Const.Regex._wsRegex).IsMatch(x.DownstreamScheme))).ToList();
                var needAddList = reRoutes.Except(reRoutes.Where(x => new Regex(Const.Regex._wsRegex).IsMatch(x.DownstreamScheme)).Join(config.ReRoutes,
                    x => new { x.AppName, x.DownstreamScheme }, x => new { x.AppName, x.DownstreamScheme }, (x, y) => x));
                foreach (var item in needAddList)
                {
                    config.ReRoutes.Add(item);
                }
            }
            File.WriteAllText(configPath, JsonConvert.SerializeObject(config,new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
