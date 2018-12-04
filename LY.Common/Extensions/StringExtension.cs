using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LY.Common.Extensions
{
    public static class StringExtension
    {
        public static string Encrypt(this string pwd, string salt = "")
        {
            if (pwd.IsNullOrEmpty())
            {
                throw new BusinessException("密码不能为空");
            }
            using (var md5 = MD5.Create())
            {
                string str = pwd + salt;
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(bytes);
                return strResult.Replace("-", "");
            }
        }

        public static void ValidateEmail(this string email)
        {
            if (!new Regex(@"^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$").IsMatch(email))
            {
                throw new BusinessException("请输入有效邮箱");
            }
        }

        public static void ValidateMobile(this string mobile)
        {
            if (!new Regex(@"(^$)|(^1(3[0-9]|4[579]|5[0-35-9]|7[0-9]|8[0-9])\d{8}$)").IsMatch(mobile))
            {
                throw new BusinessException("请输入有效电话");
            }
        }

        public static string GetHttpMethod(this string typeName)
        {
            return new Regex(Const.Regex._httpMethodRegex).Match(typeName).Groups[1].Value;
        }
    }
}
