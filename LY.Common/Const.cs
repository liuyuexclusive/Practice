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
    }
}
