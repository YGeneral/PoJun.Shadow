using PoJun.Shadow.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PoJun.Shadow.Tools;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// Swagger注入服务
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:15:12
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Swagger 服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Env"></param>
        public static void AddCustomSwagger(this IServiceCollection services, List<string> Env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (!Env.Contains(APIConfig.GetInstance().Environment.ToLower()))
            {
                services.AddSwaggerGen(c =>
                {
                    //多版本控制
                    foreach (var item in services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
                    {
                        //添加文档信息
                        c.SwaggerDoc(item.GroupName, new OpenApiInfo
                        {
                            Title = ".Net 5 WebApi",
                            Version = item.ApiVersion.ToString(),
                            Description = ".Net 5 WebApi",
                            Contact = new OpenApiContact
                            {
                                Name = "破军",
                                Email = "general_y@126.com"
                            }
                        });
                    }

                    c.DocumentFilter<SwaggerEnumFilter>();

                    #region 读取xml信息

                    //启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.ContractModel.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.ContractModel.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Enum.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Entity.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Code.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.IService.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.Service.xml"), true);

                    #endregion
                });
            }
        }
    }
}
