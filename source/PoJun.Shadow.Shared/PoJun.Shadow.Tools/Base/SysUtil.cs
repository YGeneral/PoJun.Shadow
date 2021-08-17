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

        #region Bytes到KB,MB,GB,TB单位智能转换

        /// <summary>
        /// Bytes到KB,MB,GB,TB单位智能转换
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ConvertBytes(long len)
        {
            //string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            //int order = 0;
            //while (len >= 1024 && order + 1 < sizes.Length)
            //{
            //    order++;
            //    len = len / 1024;
            //}
            //return String.Format("{0:0.##} {1}", len, sizes[order]);
            double leng = System.Convert.ToDouble(len);
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (leng >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                leng = leng / 1024;
            }
            return string.Format("{0:0.##} {1}", leng, sizes[order]);
        }

        #endregion
    }
}
