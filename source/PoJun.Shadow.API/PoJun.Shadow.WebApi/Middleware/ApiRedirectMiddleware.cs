using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 接口重定向中间件
    /// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// 创建人：杨江军
    /// 创建时间：2021/2/23/星期二 21:12:02
    /// </summary>
    public class ApiRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ApiRedirectMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Request.Headers.ContainsKey("IsApiRedirect"))
            {
                var isApiRedirect = httpContext.Request.Headers.TryGetValue("IsApiRedirect",out Microsoft.Extensions.Primitives.StringValues msg);
                if(isApiRedirect)
                {
                    //var path = httpContext.Request.Path.ToUriComponent();
                    httpContext.Request.Path = "/api/v1/test/index";
                }
            }
            

            return _next(httpContext);
        }
    }

    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class ApiRedirectMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiRedirectMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRedirectMiddleware>();
        }
    }
}
