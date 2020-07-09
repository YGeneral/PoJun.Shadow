using PoJun.Util.Webs.Clients;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.IFramework.Web
{
    /// <summary>
    /// HttpClientHelp
    /// </summary>
    public interface IHttpClientHelp
    {
        /// <summary>
        /// GET请求[同步]
        /// </summary>
        /// <typeparam name="T">只能传入DTO对象</typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        (T Data, string LogId) HttpGet<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100, Dictionary<string, string> authorization = null);

        /// <summary>
        /// GET请求[同步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        (string Data, string LogId) HttpGet(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0, Dictionary<string, string> authorization = null);

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <typeparam name="T">只能传入DTO对象</typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        Task<(T Data, string LogId)> HttpGetAsync<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100, Dictionary<string, string> authorization = null);

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        Task<(string Data, string LogId)> HttpGetAsync(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0, Dictionary<string, string> authorization = null);

        /// <summary>
        /// Post请求[同步]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        (T Data, string LogId) HttpPost<T>(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false, Dictionary<string, string> authorization = null);

        /// <summary>
        /// Post请求[同步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        (string Data, string LogId) HttpPost(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false, Dictionary<string, string> authorization = null);

        /// <summary>
        /// Post请求[异步]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        Task<(T Data, string LogId)> HttpPostAsync<T>(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false, Dictionary<string, string> authorization = null);

        /// <summary>
        /// Post请求[异步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <param name="authorization">身份认证</param>
        /// <returns></returns>
        Task<(string Data, string LogId)> HttpPostAsync(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false, Dictionary<string, string> authorization = null);


    }
}
