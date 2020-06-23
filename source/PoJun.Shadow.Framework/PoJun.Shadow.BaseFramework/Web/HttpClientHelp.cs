using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.IFramework.Log;
using PoJun.Shadow.IFramework.Web;
using PoJun.Shadow.Tools;
using PoJun.Util.Webs.Clients;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAPILogService apiLogService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_apiLogService"></param>
        /// <param name="_clientFactory"></param>
        public HttpClientHelp(IAPILogService _apiLogService, IHttpClientFactory _clientFactory)
        {
            apiLogService = _apiLogService;
            clientFactory = _clientFactory;
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
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var result = string.Empty;

                Exception exs = null;

                try
                {
                    //异步发送请求
                    Byte[] resultBytes = client.GetByteArrayAsync(url).Result;
                    if (resultBytes == null || !resultBytes.Any())
                        return default(T);
                    result = Encoding.UTF8.GetString(resultBytes);

                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = result;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (string.IsNullOrEmpty(result) && exs != null)
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    apiLogService.AddResponseLogAsync(log_response_param).Wait();

                    #endregion
                }

                #endregion

                #region 返回结果

                if (string.IsNullOrEmpty(result))
                    return default(T);
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
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var result = string.Empty;

                Exception exs = null;

                try
                {
                    //异步发送请求
                    Byte[] resultBytes = client.GetByteArrayAsync(url).Result;
                    if (resultBytes == null || !resultBytes.Any())
                        return null;
                    result = Encoding.UTF8.GetString(resultBytes);

                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = result;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (string.IsNullOrEmpty(result) && exs != null)
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    apiLogService.AddResponseLogAsync(log_response_param).Wait();

                    #endregion
                }

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
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var result = string.Empty;

                Exception exs = null;

                try
                {
                    //异步发送请求
                    Byte[] resultBytes = await client.GetByteArrayAsync(url);
                    if (resultBytes == null || !resultBytes.Any())
                        return default(T);
                    result = Encoding.UTF8.GetString(resultBytes);

                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = result;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (string.IsNullOrEmpty(result) && exs != null)
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    await apiLogService.AddResponseLogAsync(log_response_param);

                    #endregion
                }

                #endregion

                #region 返回结果

                if (string.IsNullOrEmpty(result))
                    return default(T);
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
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var result = string.Empty;

                Exception exs = null;

                try
                {
                    //异步发送请求
                    Byte[] resultBytes = await client.GetByteArrayAsync(url);
                    if (resultBytes == null || !resultBytes.Any())
                        return null;
                    result = Encoding.UTF8.GetString(resultBytes);

                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = result;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (string.IsNullOrEmpty(result) && exs != null)
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    await apiLogService.AddResponseLogAsync(log_response_param);

                    #endregion
                }

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
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public T HttpPost<T>(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var strResult = string.Empty;
                HttpResponseMessage events = null;
                Exception exs = null;

                try
                {
                    //异步发送请求
                    events = client.PostAsync(url, content).Result;
                    if (events != null && events.IsSuccessStatusCode)
                        //异步获取请求的结果
                        strResult = events.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = strResult;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (events != null)
                        log_response_param.ErrorBody = $"StatusCode：{events.StatusCode.ToString()} | events: {Newtonsoft.Json.JsonConvert.SerializeObject(events)}";
                    else
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    apiLogService.AddResponseLogAsync(log_response_param).Wait();

                    #endregion
                }

                #endregion

                #region 返回结果

                if (string.IsNullOrEmpty(strResult))
                    return default(T);
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
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public string HttpPost(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var strResult = string.Empty;
                HttpResponseMessage events = null;
                Exception exs = null;

                try
                {
                    //异步发送请求
                    events = client.PostAsync(url, content).Result;
                    if (events != null && events.IsSuccessStatusCode)
                        //异步获取请求的结果
                        strResult = events.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = strResult;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (events != null)
                        log_response_param.ErrorBody = $"StatusCode：{events.StatusCode.ToString()} | events: {Newtonsoft.Json.JsonConvert.SerializeObject(events)}";
                    else
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    apiLogService.AddResponseLogAsync(log_response_param).Wait();

                    #endregion
                }

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
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public async Task<T> HttpPostAsync<T>(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var strResult = string.Empty;
                HttpResponseMessage events = null;
                Exception exs = null;

                try
                {
                    //异步发送请求
                    events = await client.PostAsync(url, content);
                    if (events != null && events.IsSuccessStatusCode)
                        //异步获取请求的结果
                        strResult = await events.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = strResult;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (events != null)
                        log_response_param.ErrorBody = $"StatusCode：{events.StatusCode.ToString()} | events: {Newtonsoft.Json.JsonConvert.SerializeObject(events)}";
                    else
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    await apiLogService.AddResponseLogAsync(log_response_param);

                    #endregion
                }

                #endregion

                #region 返回结果

                if (string.IsNullOrEmpty(strResult))
                    return default(T);
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
        /// <param name="headers">请求头</param>
        /// <param name="timeout">请求响应超时时间，单位/s(默认100秒)</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="isMultilevelNestingJson">是否为多层嵌套json</param>
        /// <returns></returns>
        public async Task<string> HttpPostAsync(string host, string apiName, IDictionary<string, string> dicParameters, Dictionary<string, string> headers = null, int timeout = 100, HttpContentType contentType = HttpContentType.Json, bool isMultilevelNestingJson = false)
        {
            var client = clientFactory.CreateClient("base");
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
                log_request_param.APIName = apiName.ToString().ToLower();
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
                var strResult = string.Empty;
                HttpResponseMessage events = null;
                Exception exs = null;

                try
                {
                    //异步发送请求
                    events = await client.PostAsync(url, content);
                    if (events != null && events.IsSuccessStatusCode)
                        //异步获取请求的结果
                        strResult = await events.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    exs = ex;
                }
                finally
                {
                    #region 新增响应日志

                    var log_response_param = new AddResponseLogParam();
                    log_response_param.IsError = false;
                    log_response_param.ResponseBody = strResult;
                    log_response_param.ResponseTime = DateTime.Now;
                    log_response_param.ParentTraceID = log_request_param.TraceID;
                    log_response_param.TimeCost = Convert.ToInt32((log_response_param.ResponseTime - log_request_param.RequestTime).TotalMilliseconds);
                    if (events != null)
                        log_response_param.ErrorBody = $"StatusCode：{events.StatusCode.ToString()} | events: {Newtonsoft.Json.JsonConvert.SerializeObject(events)}";
                    else
                        log_response_param.ErrorBody = $"Message：{exs.Message} | StackTrace: {exs.StackTrace} | Source: {exs.Source} | InnerException： {exs.InnerException?.ToString()}";
                    await apiLogService.AddResponseLogAsync(log_response_param);

                    #endregion
                }

                #endregion

                #region 返回结果

                return strResult;

                #endregion
            }
        }

        #endregion
    }
}
