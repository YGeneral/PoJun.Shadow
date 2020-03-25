using PoJun.Shadow.Enum;
using PoJun.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PoJun.Shadow.Tools
{
    /// <summary>
    /// 系统帮助类
    /// </summary>
    public static class SysUtil
    {
        #region 判断字符串是否为json

        /// <summary>
        /// 判断字符串是否为json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsJsonStart(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                json = json.Trim('\r', '\n', ' ');
                if (json.Length > 1)
                {
                    char s = json[0];
                    char e = json[json.Length - 1];
                    return (s == '{' && e == '}') || (s == '[' && e == ']');
                }
            }
            return false;
        } 

        #endregion

        #region 获取TraceId

        /// <summary>
        /// 获取TraceId
        /// </summary>
        /// <returns></returns>
        public static string GetTraceId()
        {
            if (MyHttpContext.Current == null || MyHttpContext.Current.Request == null || MyHttpContext.Current.Request.Headers == null)
                return null;
            if (MyHttpContext.Current.Request.Headers.ContainsKey(nameof(APILogConfig.PoJun_LogTraceID)))
                return MyHttpContext.Current.Request.Headers[nameof(APILogConfig.PoJun_LogTraceID)].ToString();
            return null;
        } 

        #endregion

        #region 获取系统ID

        /// <summary>
        /// 获取系统ID
        /// </summary>
        /// <returns></returns>
        public static string GetSystemId()
        {
            return Tools.ConfigurationManager.AppSettings["SystemID"].ToString();
        } 

        #endregion

        #region Class To Dictionary

        /// <summary>
        /// Class To Dictionary
        /// 不能转换类里面有字段类型为Class的类型的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ClassToDictionary<T>(T data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        } 

        #endregion

        #region CBC模式的Des加密

        /// <summary>
        /// CBC模式的Des加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesEncode_CBC(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        } 

        #endregion

        #region CBC模式的Des解密

        /// <summary>
        /// CBC模式的Des解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecode_CBC(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < (str.Length / 2); i++)
            {
                int num2 = System.Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Encoding.GetEncoding("UTF-8").GetString(stream.ToArray());
        } 

        #endregion

        #region 签名排序

        /// <summary>
        /// 签名排序
        /// </summary>
        /// <param name="param">需要签名的DTO对象</param>
        /// <param name="delimiter">参数链接符号</param>
        /// <param name="isFieldNotNull">字段如果为空则不加入签名</param>
        /// <param name="isEncode">是否需要进行参数编码</param>
        /// <param name="signKeyHandle">签名Key处理类型</param>
        /// <returns></returns>
        public static string SignSort(Dictionary<string, string> param, string delimiter = "&", bool isFieldNotNull = false, bool isEncode = true, SignKeyHandleType signKeyHandle = SignKeyHandleType.Normal)
        {
            var signStr = string.Empty;
            foreach (var item in param.OrderBy(x => x.Key))
            {
                #region 字段如果为空则不加入签名处理

                if (isFieldNotNull)
                {
                    if (string.IsNullOrWhiteSpace(item.Value) || string.IsNullOrEmpty(item.Value))
                        continue;
                }

                #endregion

                #region 签名Key处理

                var key = item.Key;
                switch (signKeyHandle)
                {
                    case SignKeyHandleType.AllLower:
                        key = key.ToLower();
                        break;
                    case SignKeyHandleType.AllUpper:
                        key = key.ToUpper();
                        break;
                    case SignKeyHandleType.InitialLower:
                        key = $"{key.Substring(0, 1).ToLower()}{key.Substring(1, key.Length - 1)}";
                        break;
                    case SignKeyHandleType.InitialUpper:
                        key = $"{key.Substring(0, 1).ToUpper()}{key.Substring(1, key.Length - 1)}";
                        break;
                }

                #endregion

                #region 参数编码处理

                if (isEncode)
                    signStr += $"{key}={System.Net.WebUtility.UrlEncode(item.Value)}{delimiter}";
                else
                    signStr += $"{key}={item.Value}{delimiter}";

                #endregion
            }
            return signStr.Substring(0, signStr.Length - 1);
        }

        #endregion

        #region 取随机数

        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string RandomString(int size, bool lowerCase)
        {
            Random randomSeed = new Random();
            StringBuilder randStr = new StringBuilder(size);
            int start = (lowerCase) ? 97 : 65;
            for (int i = 0; i < size; i++)
                randStr.Append((char)(26 * randomSeed.NextDouble() + start));

            return randStr.ToString();
        } 

        #endregion

        #region  DateTime转Unix

        /// <summary>
        /// DateTime转Unix
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToUnix(DateTime time)
        {
            return ((time.ToUniversalTime().Ticks - 621355968000000000) / 10000);
        }

        #endregion

        #region  DateTime转Unix

        /// <summary>
        /// DateTime转Unix
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToUnixTo10(DateTime time)
        {
            return ((time.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        #endregion

        #region Unix转DateTime

        /// <summary>
        /// Unix转DateTime
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixToDateTime(long unix)
        {
            DateTime startUnixTime = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = long.Parse(unix + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return startUnixTime.Add(toNow);
        }

        #endregion

        #region Unix转DateTime(10位) 

        /// <summary>
        /// Unix转DateTime(10位) 
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixToDateTimeTo10(long unix)
        {
            DateTime startUnixTime = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            return startUnixTime.AddSeconds(unix);
        }

        #endregion

        #region Ip(客户端Ip地址)

        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        public static string Ip
        {
            get
            {
                var list = new[] { "127.0.0.1", "::1" };
                var result = MyHttpContext.Current?.Connection?.RemoteIpAddress.SafeString();
                if (string.IsNullOrWhiteSpace(result) || list.Contains(result))
                    result = GetLanIp();
                return result;
            }
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        #endregion
    }
}
