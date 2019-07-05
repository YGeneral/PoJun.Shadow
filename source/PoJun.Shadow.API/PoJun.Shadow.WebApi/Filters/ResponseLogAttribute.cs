using Microsoft.AspNetCore.Mvc.Filters;
using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.IFramework.Log;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Filters
{
    /// <summary>
    /// 返回数据日志处理
    /// </summary>
    public class ResponseLogAttribute : ResultFilterAttribute, IAsyncResultFilter
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
        public ResponseLogAttribute(IAPILogService _apiLogService)
        {
            apiLogService = _apiLogService;
        }

        #endregion

        /// <summary>
        /// 新增返回日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            #region 新增接口返回日志

            object logTraceID = null;
            context.HttpContext.Items.TryGetValue(nameof(APILogConfig.PoJun_LogTraceID), out logTraceID);
            if (logTraceID != null)
            {
                object _requestTime = null;
                context.HttpContext.Items.TryGetValue(nameof(APILogConfig.RequestTime), out _requestTime);
                var logParam = new AddResponseLogParam();
                if (context.Result != null)
                {
                    dynamic json = Newtonsoft.Json.Linq.JToken.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(context.Result)) as dynamic;
                    logParam.ResponseBody = Convert.ToString(json.Value);
                }
                logParam.IsError = false;
                logParam.ParentTraceID = logTraceID.ToString();
                logParam.ResponseTime = DateTime.Now;
                logParam.TimeCost = Convert.ToInt32((logParam.ResponseTime - Convert.ToDateTime(_requestTime)).TotalMilliseconds);
                await apiLogService.AddResponseLogAsync(logParam);
            }

            #endregion

            await base.OnResultExecutionAsync(context, next);
        }
    }
}
