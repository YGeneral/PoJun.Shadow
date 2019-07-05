using Microsoft.AspNetCore.Mvc.Filters;
using PoJun.Shadow.Exception;
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
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var workerUserId = string.Empty;

            //if (APIConfig.GetInstance().Environment != "localhost")
            //{
                //if (MyHttpContext.Current.Request == null || MyHttpContext.Current.Request.Headers == null)
                //    throw new ParamErrorException("Request不存在");

                //if (!MyHttpContext.Current.Request.Headers.ContainsKey("Token"))
                //    throw new ParamErrorException("Token获取失败！");     

                //var token = MyHttpContext.Current.Request.Headers["Token"].First();
                //if (string.IsNullOrEmpty(token))
                //    throw new ParamErrorException("Token获取失败！");

                //workerUserId = result.resultData;
            //}
            //else
            //{
            //    //TODO:测试专用代码
            //    workerUserId = "jiangjun.yang";
            //}

            //if (string.IsNullOrEmpty(workerUserId))
            //{
            //    throw new NoPermissionException("Token获取失败！");
            //}
            
            //把用户信息存入本次请求中
            //context.RouteData.Values[nameof(ConstantConfig.PeopleInfo)] = user.UserInfo;

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
