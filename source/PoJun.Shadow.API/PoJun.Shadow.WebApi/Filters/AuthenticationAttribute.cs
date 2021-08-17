using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
    /// 权限验证
    /// </summary>
    public class AuthenticationAttribute : ActionFilterAttribute, IActionFilter
    {
        #region 初始化

        /// <summary>
        /// 接口日志服务
        /// </summary>
        private readonly IAPILogService apiLogService;

        /// <summary>
        /// 文本日志
        /// </summary>
        private readonly ILogger<AuthenticationAttribute> logger;

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="_apiLogService"></param>
        /// <param name="_logger"></param>
        public AuthenticationAttribute(IAPILogService _apiLogService, ILogger<AuthenticationAttribute> _logger)
        {
            apiLogService = _apiLogService;
            logger = _logger;
        } 

        #endregion

        /// <summary>
        /// 
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

            var logParam = new AddRequestLogParam
            {
                APIName = context.HttpContext.Request.Path.ToString().ToLower(),
                //这里存储的是客户端的IP+端口
                ClientHost = $"{context.HttpContext.Connection.RemoteIpAddress}:{context.HttpContext.Connection.RemotePort}",
                Headers = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers.ToList()),
                Level = 1,
                RequestBody = jsonData,
                //这里存储的当前服务器的IP+端口+接口地址+url参数
                ServerHost = $"{PoJun.Util.Helpers.Web.Ip}:{context.HttpContext.Request.Host.Port}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}",
                SystemID = SysUtil.GetSystemId(),
                TraceID = logTraceID,
                RequestTime = requestTime
            };

            try
            {
                //新增日志
                var log = await apiLogService.AddRequestLogAsync(logParam);
            }
            catch (System.Exception)
            {
                //记录文本日志
                logger.LogCritical($"{Newtonsoft.Json.JsonConvert.SerializeObject(logParam)},");
            }

            #endregion

            #region 向后传输数据

            //把日志ID写入
            context.HttpContext.Items.Add(nameof(APILogConfig.PoJun_LogTraceID), logTraceID);
            context.HttpContext.Request.Headers.Add(nameof(APILogConfig.PoJun_LogTraceID), logTraceID);
            //把接口请求时间写入
            context.HttpContext.Items.Add(nameof(APILogConfig.RequestTime), requestTime);
            context.HttpContext.Request.Headers.Add(nameof(APILogConfig.RequestTime), requestTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            #endregion

            #region 根据控制器名称判断是否进行登录验证

            //var controller = MyHttpContext.Current.Request.RouteValues["controller"].ToString().ToLower();
            //if (SingleSignOnConfig.GetInstance().ExcludeControllerNames.Contains(controller))
            //{
            //    await base.OnActionExecutionAsync(context, next);
            //    return;
            //}

            #endregion

            #region 判断是否启用单点登录验证

            //if (!SingleSignOnConfig.GetInstance().IsEnable && controller != "apilogs")
            //{
            //    await base.OnActionExecutionAsync(context, next);
            //    return;
            //}

            #endregion

            #region Token校验

            //RefreshTokenModel userInfo;

            #region 验证传入的Token

            //if (string.IsNullOrEmpty(SysUtil.GetToken()))
            //    throw new ParamErrorException("Token获取失败！");

            #endregion

            #region 验证传入的客户端ID

            //if (string.IsNullOrEmpty(SysUtil.GetClientId()))
            //    throw new ParamErrorException("请传入正确的客户端ID！");

            #endregion

            #region 验证传入的客户端类型

            //if (SysUtil.GetClientType() == ClientType.None)
            //    throw new ParamErrorException("请传入正确的客户端类型！");

            #endregion

            #region 调用单点登录的刷新Token接口验证Token的合法性

            //var result = await loginService.RefreshToken(new Api.ContractModel.Framework.Login.RefreshTokenParam() { ClientId = SysUtil.GetClientId(), ClientType = SysUtil.GetClientType(), Token = SysUtil.GetToken() });

            //if (result.DetailedStatus != Code.DetailedStatusCode.Success)
            //    throw new BaseException(result.DetailedMessage, result.DetailedStatus);

            #endregion

            //userInfo = result.Data;

            ////把用户信息存入本次请求中
            //context.HttpContext.Items.Add(nameof(APILogConfig.LoginUserInfo), userInfo);
            //context.HttpContext.Request.Headers.Add(nameof(APILogConfig.LoginUserInfo), Newtonsoft.Json.JsonConvert.SerializeObject(userInfo));

            #endregion

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
