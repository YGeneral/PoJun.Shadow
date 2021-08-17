using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
        /// 文本日志
        /// </summary>
        private readonly ILogger<ResponseLogAttribute> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_apiLogService"></param>
        /// <param name="_logger"></param>
        public ResponseLogAttribute(IAPILogService _apiLogService, ILogger<ResponseLogAttribute> _logger)
        {
            apiLogService = _apiLogService;
            logger = _logger;
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

            context.HttpContext.Items.TryGetValue(nameof(APILogConfig.PoJun_LogTraceID), out object logTraceID);
            if (logTraceID != null)
            {
                context.HttpContext.Items.TryGetValue(nameof(APILogConfig.RequestTime), out object _requestTime);
                var logParam = new AddResponseLogParam();
                if (context.Result != null)
                {
                    dynamic json = Newtonsoft.Json.Linq.JToken.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(context.Result)) as dynamic;
                    logParam.ResponseBody = System.Convert.ToString(json.Value);
                }
                logParam.IsError = false;
                logParam.ParentTraceID = logTraceID.ToString();
                logParam.ResponseTime = DateTime.Now;
                logParam.TimeCost = System.Convert.ToInt32((logParam.ResponseTime - System.Convert.ToDateTime(_requestTime)).TotalMilliseconds);
                try
                {
                    await apiLogService.AddResponseLogAsync(logParam);
                }
                catch (System.Exception)
                {
                    logger.LogWarning($"{Newtonsoft.Json.JsonConvert.SerializeObject(logParam)},");
                }
            }

            await base.OnResultExecutionAsync(context, next);

            #endregion

            await base.OnResultExecutionAsync(context, next);
        }
    }
}
