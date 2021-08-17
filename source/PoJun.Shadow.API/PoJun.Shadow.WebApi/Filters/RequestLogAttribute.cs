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
        /// <summary>
        /// 进入处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
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
