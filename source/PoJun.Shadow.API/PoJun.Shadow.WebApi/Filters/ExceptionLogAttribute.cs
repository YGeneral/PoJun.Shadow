using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NIO.SpecialCar.LogFramework;
using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Code;
using PoJun.Shadow.ContractModel;
using PoJun.Shadow.IFramework.Log;
using PoJun.Shadow.Tools;
using PoJun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Filters
{
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class ExceptionLogAttribute : IAsyncExceptionFilter
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
        public ExceptionLogAttribute(IAPILogService _apiLogService)
        {
            apiLogService = _apiLogService;
        }

        #endregion

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var dto = new BaseResponse();
            bool isError = false;

            if (context.Exception.GetType().ToString() == typeof(PoJun.Shadow.Exception.RepeatSubmitException).ToString())
            {
                dto.DetailedStatus = DetailedStatusCode.RepeatSubmit;
                dto.DetailedMessage = $"{dto.DetailedStatus.Description()}:{context.Exception.Message}";
                dto.GatewayStatus = GatewayStatusCode.Fail;
                dto.GatewayMessage = dto.GatewayStatus.Description();
            }
            else if (context.Exception.GetType().ToString() == typeof(PoJun.Shadow.Exception.ParamErrorException).ToString())
            {
                dto.DetailedStatus = DetailedStatusCode.ParamsError;
                dto.DetailedMessage = $"{dto.DetailedStatus.Description()}:{context.Exception.Message}";
                dto.GatewayStatus = GatewayStatusCode.Fail;
                dto.GatewayMessage = dto.GatewayStatus.Description();
            }
            else if(context.Exception.GetType().ToString() == typeof(PoJun.Shadow.Exception.FailException).ToString())
            {
                dto.DetailedStatus = DetailedStatusCode.Fail;
                dto.DetailedMessage = $"{dto.DetailedStatus.Description()}:{context.Exception.Message}";
                dto.GatewayStatus = GatewayStatusCode.Fail;
                dto.GatewayMessage = dto.GatewayStatus.Description();
            }
            else
            {
                dto.DetailedStatus = DetailedStatusCode.Error;
                dto.DetailedMessage = $"{dto.DetailedStatus.Description()}";
                dto.GatewayStatus = GatewayStatusCode.Fail;
                dto.GatewayMessage = dto.GatewayStatus.Description();
                isError = true;
            }
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            context.Result = new ContentResult() { Content = result };


            #region 新增接口返回日志

            object logTraceID = null;
            context.HttpContext.Items.TryGetValue(nameof(APILogConfig.NIO_LogTraceID), out logTraceID);
            if (logTraceID != null)
            {
                object _requestTime = null;
                context.HttpContext.Items.TryGetValue(nameof(APILogConfig.RequestTime), out _requestTime);
                var logParam = new AddResponseLogParam();
                logParam.ResponseBody = result;
                logParam.ErrorBody = $"StackTrace:{context.Exception.StackTrace} | Message:{context.Exception.Message} | Source:{context.Exception.Source}";
                logParam.IsError = isError;
                logParam.ParentTraceID = logTraceID.SafeString();
                logParam.ResponseTime = DateTime.Now;
                logParam.TimeCost = Convert.ToInt32((logParam.ResponseTime - Convert.ToDateTime(_requestTime)).TotalMilliseconds);
                await apiLogService.AddResponseLogAsync(logParam);
            }

            #endregion

            context.Exception = null;
        }
    }
}
