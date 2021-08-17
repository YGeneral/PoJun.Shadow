using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 上传文件大小限制
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 15:48:29
    /// </summary>
    public static class UploadFileSizeLimitExtension
    {
        /// <summary>
        /// 注入上传文件大小限制
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomUploadFileSizeLimit(this IServiceCollection services)
        {
            //上传文件大小限制Kestrel设置
            services.Configure<FormOptions>(options => 
            { 
                options.KeyLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            //上传文件大小限制Kestrel设置
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = long.MaxValue;
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
            });
            ////上传文件大小限制IIS设置
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = long.MaxValue;
            //});
        }
    }
}
