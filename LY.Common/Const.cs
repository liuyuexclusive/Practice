using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common
{
    public static class Const
    {
        public static class Regex
        {
            public static string _wsRegex = @"^wss?$";
            public static string _httpRegex = @"^https?://(.+):(\d+)/?$";
            public static string _httpMethodRegex = @"^Http(.+)Attribute$";
        }


        public static class JWT
        {
            public static string SecurityKey = "dd%65*377f6d&f£$$£$FdddFF33fssDG^!3";
            public static string Issuer = "test";
            public static string Audience = "test";
        }
    }
}
