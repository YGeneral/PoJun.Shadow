using Microsoft.AspNetCore.Mvc.Filters;
using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Exception;
using PoJun.Shadow.IFramework.Log;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Filters
{
    /// <summary>
    /// 进入数据日志处理-过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequestLogAttribute : ActionFilterAttribute, IActionFilter
    {
        #region 初始化

        /// <summary>
        /// 接口日志服务
        /// </summary>
        private static IAPILogService apiLogService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_apiLogService"></param>
        public RequestLogAttribute(IAPILogService _apiLogService)
        {
            apiLogService = _apiLogService;
        }

        #endregion

        /// <summary>
        /// 进入处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            #region 获取传入的数据

            var jsonData = string.Empty;

            if (context.HttpContext.Request.ContentType != null)
            {
                var contentType = context.HttpContext.Request.ContentType.ToLower();
                if ((contentType.Contains("application/x-www-form-urlencoded") 
                    || contentType.Contains("multipart/form-data")) 
                    && context.HttpContext.Request.ContentLength != null 
                    && context.HttpContext.Request.ContentLength > 0)
                {
                    jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Form);
                }
                else if (contentType.Contains("application/json"))
                {
                    foreach (var item in context.ActionArguments)
                    {
                        jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(item.Value);
                        continue;
                    }
                }
            }

            #endregion

            #region 新增接口日志

            //获取日志ID
            var logTraceID = PoJun.Util.Helpers.Id.GetGuidBy32();
            var requestTime = DateTime.Now;

            var logParam = new AddRequestLogParam();
            logParam.APIName = context.HttpContext.Request.Path.ToString().ToLower();
            //这里存储的是客户端的IP+端口
            logParam.ClientHost = $"{context.HttpContext.Connection.RemoteIpAddress.ToString()}:{context.HttpContext.Connection.RemotePort}";
            logParam.Headers = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers.ToList());
            logParam.Level = 1;
            logParam.RequestBody = jsonData;
            //这里存储的当前服务器的IP+端口+接口地址+url参数
            logParam.ServerHost = $"{SysUtil.Ip}:{context.HttpContext.Request.Host.Port}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            logParam.SystemID = SysUtil.GetSystemId();
            logParam.TraceID = logTraceID;
            logParam.RequestTime = requestTime;

            //新增日志
            var log = await apiLogService.AddRequestLogAsync(logParam);
            #endregion

            #region 向后传输数据

            //把日志ID写入
            context.HttpContext.Items.Add(nameof(APILogConfig.PoJun_LogTraceID), logTraceID);
            context.HttpContext.Request.Headers.Add(nameof(APILogConfig.PoJun_LogTraceID), logTraceID);
            //把接口请求时间写入
            context.HttpContext.Items.Add(nameof(APILogConfig.RequestTime), requestTime);
            context.HttpContext.Request.Headers.Add(nameof(APILogConfig.RequestTime), requestTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            #endregion

            #region 全局DTO对象参数验证

            if (!context.ModelState.IsValid)
            {
                //使用自定义参数绑定验证体系
                var modelState = context.ModelState.FirstOrDefault(f => f.Value.Errors.Any());
                string errorMsg = modelState.Value.Errors.First().ErrorMessage;
                throw new ParamErrorException(errorMsg);
            }

            #endregion

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
