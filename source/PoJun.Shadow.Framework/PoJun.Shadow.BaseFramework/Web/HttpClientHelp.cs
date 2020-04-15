using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.IFramework.Log;
using PoJun.Shadow.IFramework.Web;
using PoJun.Shadow.Tools;
using PoJun.Util.Webs.Clients;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.BaseFramework
{
    /// <summary>
    /// HttpClientHelp
    /// </summary>
    public class HttpClientHelp : IHttpClientHelp
    {
        #region 初始化

        /// <summary>
        /// 日志服务
        /// </summary>
        private IAPILogService apiLogService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_apiLogService"></param>
        public HttpClientHelp(IAPILogService _apiLogService)
        {
            apiLogService = _apiLogService;
        }

        #endregion

        #region GET请求[同步]

        /// <summary>
        /// GET请求[同步]
        /// </summary>
        /// <typeparam name="T">只能传入DTO对象</typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        public T HttpGet<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100)
        {
            using (HttpClient client = new HttpClient())
            {
                #region Http基础设置

                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = null;
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = apiLogService.AddRequestLogAsync(log_request_param).Result;

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                Byte[] resultBytes = client.GetByteArrayAsync(url).Result;
                var result = Encoding.UTF8.GetString(resultBytes);

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = result;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                apiLogService.AddResponseLogAsync(log_response_param).Wait();

                #endregion

                #region 返回结果

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);

                #endregion
            }
        }

        /// <summary>
        /// GET请求[同步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        public string HttpGet(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0)
        {
            using (HttpClient client = new HttpClient())
            {
                #region Http基础设置

                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = null;
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = apiLogService.AddRequestLogAsync(log_request_param).Result;

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                Byte[] resultBytes = client.GetByteArrayAsync(url).Result;
                var result = Encoding.UTF8.GetString(resultBytes);

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = result;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                apiLogService.AddResponseLogAsync(log_response_param).Wait();

                #endregion

                #region 返回结果

                return result;

                #endregion
            }
        }

        #endregion

        #region GET请求[异步]

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <typeparam name="T">只能传入DTO对象</typeparam>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        public async Task<T> HttpGetAsync<T>(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 100)
        {
            using (HttpClient client = new HttpClient())
            {
                #region Http基础设置

                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = null;
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = await apiLogService.AddRequestLogAsync(log_request_param);

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                Byte[] resultBytes = await client.GetByteArrayAsync(url);
                var result = Encoding.UTF8.GetString(resultBytes);

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = result;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                await apiLogService.AddResponseLogAsync(log_response_param);

                #endregion

                #region 返回结果

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);

                #endregion
            }
        }

        /// <summary>
        /// GET请求[异步]
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="apiName">接口名称(接口地址)</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <returns></returns>
        public async Task<string> HttpGetAsync(string host, string apiName, Dictionary<string, string> headers = null, int timeout = 0)
        {
            using (HttpClient client = new HttpClient())
            {
                #region Http基础设置

                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = null;
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = await apiLogService.AddRequestLogAsync(log_request_param);

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                Byte[] resultBytes = await client.GetByteArrayAsync(url);
                var result = Encoding.UTF8.GetString(resultBytes);

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = result;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                await apiLogService.AddResponseLogAsync(log_response_param);

                #endregion

                #region 返回结果

                return result;

                #endregion
            }
        }

        #endregion

        #region Post请求[同步]

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
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public T HttpPost<T>(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            using (HttpClient client = ((httpHandler == null) ? new HttpClient() : new HttpClient(httpHandler)))
            {
                #region Http基础设置

                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                HttpContent content = new FormUrlEncodedContent(dicParameters);
                var _contentType = (contentType == HttpContentType.Json ? "application/json" : "application/x-www-form-urlencoded");
                if (contentType == HttpContentType.Json)
                {
                    if (isMultilevelNestingJson)
                        content = new StringContent(dicParameters["json"]);
                    else
                        content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters));
                }
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters);
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = apiLogService.AddRequestLogAsync(log_request_param).Result;

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                //异步发送请求
                var events = client.PostAsync(url, content).Result;
                //异步获取请求的结果
                var strResult = events.Content.ReadAsStringAsync().Result;

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = strResult;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                apiLogService.AddResponseLogAsync(log_response_param).Wait();

                #endregion

                #region 返回结果

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strResult);

                #endregion
            }
        }

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
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public string HttpPost(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            using (HttpClient client = ((httpHandler == null) ? new HttpClient() : new HttpClient(httpHandler)))
            {
                #region Http基础设置

                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                HttpContent content = new FormUrlEncodedContent(dicParameters);
                var _contentType = (contentType == HttpContentType.Json ? "application/json" : "application/x-www-form-urlencoded");
                if (contentType == HttpContentType.Json)
                {
                    if (isMultilevelNestingJson)
                        content = new StringContent(dicParameters["json"]);
                    else
                        content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters));
                }
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters);
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = apiLogService.AddRequestLogAsync(log_request_param).Result;

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                //异步发送请求
                var events = client.PostAsync(url, content).Result;
                //异步获取请求的结果
                var strResult = events.Content.ReadAsStringAsync().Result;

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = strResult;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                apiLogService.AddResponseLogAsync(log_response_param).Wait();

                #endregion

                #region 返回结果

                return strResult;

                #endregion
            }
        }

        #endregion

        #region Post请求[异步]


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
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public async Task<T> HttpPostAsync<T>(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            using (HttpClient client = ((httpHandler == null) ? new HttpClient() : new HttpClient(httpHandler)))
            {
                #region Http基础设置

                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                HttpContent content = new FormUrlEncodedContent(dicParameters);
                var _contentType = (contentType == HttpContentType.Json ? "application/json" : "application/x-www-form-urlencoded");
                if (contentType == HttpContentType.Json)
                {
                    if (isMultilevelNestingJson)
                        content = new StringContent(dicParameters["json"]);
                    else
                        content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters));
                }
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters);
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = await apiLogService.AddRequestLogAsync(log_request_param);

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                //异步发送请求
                var events = await client.PostAsync(url, content);
                //异步获取请求的结果
                var strResult = await events.Content.ReadAsStringAsync();

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = strResult;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                await apiLogService.AddResponseLogAsync(log_response_param);

                #endregion

                #region 返回结果

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strResult);

                #endregion
            }
        }

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
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public async Task<string> HttpPostAsync(string host, string apiName, IDictionary<string, string> dicParameters, DiscoveryHttpClientHandler httpHandler = null, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            using (HttpClient client = ((httpHandler == null) ? new HttpClient() : new HttpClient(httpHandler)))
            {
                #region Http基础设置

                //设置接口超时时间
                if (timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                }

                HttpContent content = new FormUrlEncodedContent(dicParameters);
                var _contentType = (contentType == HttpContentType.Json ? "application/json" : "application/x-www-form-urlencoded");
                if (contentType == HttpContentType.Json)
                {
                    if (isMultilevelNestingJson)
                        content = new StringContent(dicParameters["json"]);
                    else
                        content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters));
                }
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
                //设置HTTP请求头
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }
                }

                #endregion

                #region 新增请求日志

                var log_request_param = new AddRequestLogParam();
                log_request_param.APIName = apiName;
                log_request_param.ClientHost = SysUtil.Ip;
                log_request_param.RequestTime = DateTime.Now;
                log_request_param.ServerHost = host;
                log_request_param.SystemID = SysUtil.GetSystemId();
                log_request_param.TraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
                log_request_param.Level = 2;
                log_request_param.ParentTraceID = SysUtil.GetTraceId();
                log_request_param.RequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(dicParameters);
                if (SysUtil.GetTraceId() != null)
                {
                    log_request_param.Level = 2;
                    log_request_param.ParentTraceID = SysUtil.GetTraceId();
                }
                else
                {
                    log_request_param.Level = 1;
                }
                var log_request_result = await apiLogService.AddRequestLogAsync(log_request_param);

                #endregion

                #region 发起Http请求

                var url = $"{host}{apiName}";
                //异步发送请求
                var events = await client.PostAsync(url, content);
                //异步获取请求的结果
                var strResult = await events.Content.ReadAsStringAsync();

                #endregion

                #region 新增响应日志

                var log_response_param = new AddResponseLogParam();
                log_response_param.IsError = false;
                log_response_param.ParentTraceID = log_request_param.TraceID;
                log_response_param.ResponseBody = strResult;
                log_response_param.ResponseTime = DateTime.Now;
                log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);

                await apiLogService.AddResponseLogAsync(log_response_param);

                #endregion

                #region 返回结果

                return strResult;

                #endregion
            }
        }

        #endregion
    }
}
