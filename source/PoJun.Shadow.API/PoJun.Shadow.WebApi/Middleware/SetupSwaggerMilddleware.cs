using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 启动Swagger中间件
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:46:56
    /// </summary>
    public static class SetupSwaggerMilddleware
    {
        /// <summary>
        /// Swagger中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="Env">环境</param>
        /// <param name="Provider">API版本</param>
        public static void UseSwaggerMilddleware(this IApplicationBuilder app, List<string> Env, IApiVersionDescriptionProvider Provider)
        {
            if (!Env.Contains(APIConfig.GetInstance().Environment.ToLower()))
            {
                //启用Swagger中间件
                app.UseSwagger();
                //配置SwaggerUI
                app.UseSwaggerUI(c =>
                {
                    foreach (var item in Provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", $"{SysUtil.GetSystemId()}{item.ApiVersion}");
                    }
                });
            }
        }
    }
}
