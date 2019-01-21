using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common
{
    public static class Const
    {
        public static string _scheme = "http";
        public static class Regex
        {
            public static string _wsRegex = @"^wss?$";
            public static string _httpAddressRegex = @"^.+:\d+$";
            public static string _httpUrlRegex = @"^https?://(.+):(\d+)/?$";
            public static string _httpMethodAttributeRegex = @"^Http(.+)Attribute$";
        }


        public static class JWT
        {
            public static string _securityKey = "dd%65*377f6d&f£$$£$FdddFF33fssDG^!3";
            public static string _issuer = "ly-user";
            public static string _audience = "ly";
        }

        public static class IP
        {
            public static string _network = "172.18.0.0/16";
            static string _prefix = _network.Substring(0, 6);
            public static string _mysqlMaster = $"{_prefix}.200.1";
            public static string _mysqlSlave = $"{_prefix}.200.2";
            public static string _redis = $"{_prefix}.201.1";
            public static string _consul = $"{_prefix}.202.1";
            public static string _rabbitmq = $"{_prefix}.203.1";
            public static string _elasticsearch= $"{_prefix}.204.1";
            public static string _gateway = $"{_prefix}.211.1";
            public static string _daemon = $"{_prefix}.212.1";
        }
    }
}
