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

        private const int capacity10 = 10;
        private const int capacity36 = 36;
        private const int capacity62 = 62;

        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="type">1：纯数字【默认】；2：字母(大写)+数字；2：字母(大写+小写)+数字</param>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string RandomString(int type, int length)
        {
            char[] constant;
            switch (type)
            {
                default:
                case 1:
                    constant = new char[capacity10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    break;
                case 2:
                    constant = new char[capacity36] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                    break;
                case 3:
                    constant = new char[capacity62] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                    break;
            }

            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length)]);
            }
            return newRandom.ToString();

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

        #region 获取百分比

        /// <summary>
        /// 获取百分比
        /// </summary>
        /// <param name="total">总数量</param>
        /// <param name="part">部分数量</param>
        /// <returns>部分数量站总数量的百分比</returns>
        public static decimal GetPercentage(decimal total, decimal part)
        {
            decimal t = Math.Round((part / total), 2); //四舍五入,精确2位
            return t * 100;
        }

        #endregion

        #region 获取远程服务器内容，并转换成二进制数组

        /// <summary>
        /// 获取远程服务器内容，并转换成二进制数组
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] GetUrlByte(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    List<byte> btlist = new List<byte>();
                    int b = responseStream.ReadByte();
                    while (b > -1)
                    {
                        btlist.Add((byte)b);
                        b = responseStream.ReadByte();
                    }
                    return btlist.ToArray();
                }
            }
        }

        #endregion

        #region 判断字符串中是否不包含字符

        /// <summary>
        /// 判断字符串中是否不包含字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strList">不包含字符集合</param>
        /// <returns>如果不包含则返回true，如果包含则返回false</returns>
        public static bool NotContains(this string str, List<string> strList)
        {
            foreach (var item in strList)
            {
                if (str.Contains(item))
                    return false;
            }
            return true;
        }

        #endregion

        #region 判断字符串中是否全部包含该集合中的字符

        /// <summary>
        /// 判断字符串中是否全部包含该集合中的字符
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="strList">字符集合</param>
        /// <returns></returns>
        public static bool Contains(this string str, List<string> strList)
        {
            foreach (var item in strList)
            {
                if (!str.Contains(item))
                    return false;
            }
            return true;
        }

        #endregion

        #region 判断字符串中是否全部安装顺序包含该集合中的字符

        /// <summary>
        /// 判断字符串中是否全部安装顺序包含该集合中的字符
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="strList">字符集合</param>
        /// <returns></returns> 
        public static bool OrderContains(this string str, List<string> strList)
        {
            var order = new List<int>();
            foreach (var item in strList)
            {
                if (!str.Contains(item))
                    return false;
                order.Add(str.IndexOf(item));
            }
            var newOrder = order.OrderBy(x => x).ToList();
            if (string.Join("", order) == string.Join("", newOrder))
                return true;
            else
                return false;
        }

        #endregion

        #region 在字符串中删除传入集合中的字符串

        /// <summary>
        /// 在字符串中删除传入集合中的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strList">字符集合</param>
        /// <returns></returns>
        public static string Remove(this string str, List<string> strList)
        {
            foreach (var item in strList)
            {
                str = str.Replace(item, null);
            }
            return str;
        }

        #endregion
    }
}
