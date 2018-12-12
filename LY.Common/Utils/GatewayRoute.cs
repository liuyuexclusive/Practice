using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common
{
    public class GatewayConfig
    {
        public IList<GatewayReRoute> ReRoutes { get; set; } = new List<GatewayReRoute>();
        public GatewayGlobalConfiguration GlobalConfiguration { get; set; } = new GatewayGlobalConfiguration();
    }

    public class GatewayReRoute
    {
        public string UpstreamPathTemplate { get; set; }
        [JsonProperty(PropertyName = " UpstreamHttpMethod ")]
        public IList<string> UpstreamHttpMethod { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public IList<GatewayReRouteDownstreamHostAndPort> DownstreamHostAndPorts { get; set; }
        public string DownstreamScheme { get; set; } = Const._scheme;
        public GatewayRouteAuthenticationOption AuthenticationOptions { get; set; }
        public string AppName { get; set; }
        public string ServiceName { get; set; }
        public LoadBalancerOptions LoadBalancerOptions { get; set; }
    }

    public class LoadBalancerOptions
    {
        public string Type { get; set; } = "LeastConnection";
    }


    public class GatewayReRouteDownstreamHostAndPort
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class GatewayRouteAuthenticationOption
    {
        public string AuthenticationProviderKey { get; set; } = "TestKey";
    }

    public class GatewayGlobalConfiguration
    {
        public string RequestIdKey { get; set; } = "OcRequestId";
        public string AdministrationPath { get; set; } = "/administration";
        public ServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; } = new ServiceDiscoveryProvider();
    }

    public class ServiceDiscoveryProvider
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 8500;
        public string Type { get; set; } = "Consul";
    }

    public class UnAuthorizeAttribute : Attribute
    {

    }
}
