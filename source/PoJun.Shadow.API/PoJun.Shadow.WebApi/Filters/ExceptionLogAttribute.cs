using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Code;
using PoJun.Shadow.ContractModel;
using PoJun.Shadow.Exception;
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
		/// 文本日志
		/// </summary>
		private readonly ILogger<ExceptionLogAttribute> logger;

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="_apiLogService"></param>
		/// <param name="_logger"></param>
		public ExceptionLogAttribute(IAPILogService _apiLogService, ILogger<ExceptionLogAttribute> _logger)
		{
			apiLogService = _apiLogService;
			logger = _logger;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task OnExceptionAsync(ExceptionContext context)
        {
			var dto = new ContractModel.BaseResponse();
			bool isError = false;

			switch (context.Exception)
			{
				case BaseException baseException:
					dto.DetailedStatus = baseException.DetailedStatus;
					dto.GatewayStatus = baseException.GatewayStatus;
					dto.DetailedMessage = $"{context.Exception.Message}";
					dto.GatewayMessage = dto.GatewayStatus.Description();
					break;
				default:
					dto.DetailedStatus = DetailedStatusCode.Error;
					dto.DetailedMessage = $"{dto.DetailedStatus.Description()}";
					dto.GatewayStatus = GatewayStatusCode.Fail;
					dto.GatewayMessage = dto.GatewayStatus.Description();
					isError = true;
					break;
			}

			var serializerSettings = new JsonSerializerSettings
			{
				//设置为不是驼峰命名
				ContractResolver = new DefaultContractResolver()
			};

			if (APIConfig.GetInstance().IsLittleCamelFormat)
				//设置为驼峰命名
				serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var result = JsonConvert.SerializeObject(dto, serializerSettings);
			context.Result = new ContentResult() { Content = result };

			#region 新增接口返回日志

			context.HttpContext.Items.TryGetValue(nameof(APILogConfig.PoJun_LogTraceID), out object logTraceID);
			if (logTraceID != null)
			{
				context.HttpContext.Items.TryGetValue(nameof(APILogConfig.RequestTime), out object _requestTime);
				var logParam = new AddResponseLogParam
				{
					ResponseBody = result,
					ErrorBody = $"StackTrace:{context.Exception.StackTrace} | Message:{context.Exception.Message} | Source:{context.Exception.Source}",
					IsError = isError,
					ParentTraceID = logTraceID.SafeString(),
					ResponseTime = DateTime.Now
				};
				logParam.TimeCost = Convert.ToInt32((logParam.ResponseTime - Convert.ToDateTime(_requestTime)).TotalMilliseconds);
				try
				{
					await apiLogService.AddResponseLogAsync(logParam);
				}
				catch (System.Exception)
				{
					logger.LogWarning($"{Newtonsoft.Json.JsonConvert.SerializeObject(logParam)},");
				}
			}

			#endregion

			context.Exception = null;
		}
    }
}
