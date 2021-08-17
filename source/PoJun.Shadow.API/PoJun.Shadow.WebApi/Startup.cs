using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PoJun.MongoDB.Repository;
using PoJun.Shadow.BaseFramework;
using PoJun.Shadow.Tools;
using PoJun.Shadow.WebApi.Filters;
using PoJun.Shadow.WebApi.Jobs;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Api版本信息
        /// </summary>
        private IApiVersionDescriptionProvider Provider;

        /// <summary>
        /// 环境
        /// </summary>
        private readonly static List<string> Env = new() { "demo", "prod" }; 

        #endregion

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(AuthenticationAttribute));
                option.Filters.Add(typeof(ExceptionLogAttribute));
                option.Filters.Add(typeof(RequestLogAttribute));
                option.Filters.Add(typeof(ResponseLogAttribute));
                option.MaxModelValidationErrors = 100;
            })
            .AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //忽略循环引用
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //驼峰格式
                option.SerializerSettings.ContractResolver = APIConfig.GetInstance().IsLittleCamelFormat ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver();
                //增加参数自动去除前后空格转换器
                option.SerializerSettings.Converters.Add(new TrimmingConverter());
            });
            //解决跨域（如果不用可以注释掉）
            services.AddCustomCors();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCustomApiVersioning();//接口版本控制            
            services.AddCustomSwagger(Env);//注册Swagger服务
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //禁用.net core webapi 项目本身的模型参数绑定验证体系
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddHttpClient();
            services.AddHttpClient("base").AddTransientHttpErrorPolicy(x => x.RetryAsync(3));//添加重试策略。 若请求失败，最多可重试三次;
            services.AddCustomUploadFileSizeLimit();//上传文件大小限制设置
            services.AddHealth().AddHealthEndpoints().AddHealthChecks();//注入AddMetrics的健康检测
            services.AddHealthEndpoints();
        }

        /// <summary>
		/// 添加Autofac DI服务工厂
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            this.Provider = provider.GetRequiredService<IApiVersionDescriptionProvider>();
            PoJun.Util.Helpers.MyHttpContext.HttpContextAccessor = httpContextAccessor;
            MyHttpContext.HttpContextAccessor = httpContextAccessor;

            app.UseRequestLocalizationMilddleware();
            app.UseSwaggerMilddleware(Env, Provider);
            app.UseCors("EnableCrossDomain");//解决跨域（如果不用可以注释掉）
            //app.UseStaticFiles();//注册wwwroot静态文件（如果不用可以注释掉）
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseCustomAppMetrics();//启动AppMetrics
            //启动Job（如果不用可以注释掉）
            //Task.Run(async () => { await app.UseJobMilddleware(); });
        }
    }
}
