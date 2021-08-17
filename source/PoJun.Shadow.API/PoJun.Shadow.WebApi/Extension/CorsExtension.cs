using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 跨域配置
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:13:11
    /// </summary>
    public static class CorsExtension
    {
        /// <summary>
        /// 跨域配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomCors(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //解决跨域（如果不用可以注释掉）
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCrossDomain", builder =>
                {
                    builder.AllowAnyOrigin()//允许任何来源的主机访问
                    //builder.WithOrigins(APIConfig.GetInstance().RequestSource)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//指定处理cookie
                });
            });

        }
    }
}
