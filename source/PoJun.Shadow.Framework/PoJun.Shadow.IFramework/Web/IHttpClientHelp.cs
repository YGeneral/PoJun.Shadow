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
        /// <returns></returns>
        T HttpGet<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100);

        /// <summary>
        /// GET请求[同步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        string HttpGet(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0);

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <typeparam name="T">只能传入DTO对象</typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        Task<T> HttpGetAsync<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100);

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        Task<string> HttpGetAsync(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0);

        /// <summary>
        /// Post请求[同步]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="httpHandler">httpHandler</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <returns></returns>
        T HttpPost<T>(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json);

        /// <summary>
        /// Post请求[同步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="httpHandler">httpHandler</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <returns></returns>
        string HttpPost(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json);

        /// <summary>
        /// Post请求[异步]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="httpHandler">httpHandler</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <returns></returns>
        Task<T> HttpPostAsync<T>(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json);

        /// <summary>
        /// Post请求[异步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="dicParameters">接口参数</param>
        /// <param name="httpHandler">httpHandler</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <returns></returns>
        Task<string> HttpPostAsync(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json);


    }
}
