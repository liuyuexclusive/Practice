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
        public string DownstreamPathTemplate { get; set; }
        public IList<GatewayReRouteDownstreamHostAndPort> DownstreamHostAndPorts { get; set; } = new List<GatewayReRouteDownstreamHostAndPort>();
        public string DownstreamScheme { get; set; } = "https";
        public GatewayRouteAuthenticationOption AuthenticationOptions { get; set; }
        public string AppName { get; set; }
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
        public ServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }
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
