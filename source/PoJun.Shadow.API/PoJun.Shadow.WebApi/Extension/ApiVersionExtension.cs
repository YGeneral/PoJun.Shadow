using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// Api版本控制注入
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:21:54
    /// </summary>
    public static class ApiVersionExtension
    {
        /// <summary>
        /// 版本控制
        /// </summary>
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApiVersioning(options =>
            {
                //当设置为 true 时, API 将返回响应标头中支持的版本信息
                options.ReportApiVersions = true;
                //此选项将用于不提供版本的请求。默认情况下, 假定的 API 版本为1.0。
                options.AssumeDefaultVersionWhenUnspecified = true;
                //此选项用于指定在请求中未指定版本时要使用的默认 API 版本。这将默认版本为1.0。
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            }).AddVersionedApiExplorer(options =>
            {
                // 版本名的格式：v+版本号
                options.GroupNameFormat = "'v'V";
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}
