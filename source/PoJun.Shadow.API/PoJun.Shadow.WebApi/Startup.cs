using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PoJun.MongoDB.Repository;
using PoJun.Shadow.BaseFramework;
using PoJun.Shadow.Tools;
using PoJun.Shadow.WebApi.Filters;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace PoJun.Shadow.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //注册MongoDB仓储（如果不用可以注释掉）
            RepositoryContainer.RegisterAll(AutofacModuleRegister.GetAllAssembliesName());
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(ExceptionLogAttribute));
                option.Filters.Add(typeof(RequestLogAttribute));
                option.Filters.Add(typeof(ResponseLogAttribute));
                option.MaxModelValidationErrors = 100;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(option =>
             {
                 option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                 //忽略循环引用
                 option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //不使用驼峰样式的key
                 option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 //增加参数自动去除前后空格转换器
                 option.SerializerSettings.Converters.Add(new TrimmingConverter());
             });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApiVersioning(options =>
            {
                //当设置为 true 时, API 将返回响应标头中支持的版本信息
                options.ReportApiVersions = true;
                //此选项将用于不提供版本的请求。默认情况下, 假定的 API 版本为1.0。
                options.AssumeDefaultVersionWhenUnspecified = true;
                //此选项用于指定在请求中未指定版本时要使用的默认 API 版本。这将默认版本为1.0。
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });
            //解决跨域（如果不用可以注释掉）
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCrossDomain", builder =>
                {
                    builder.AllowAnyOrigin()//允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            //注册权限验证
            services.AddScoped<AuthenticationAttribute>();

            //注入 Quartz调度类（如果不用可以注释掉）
            services.AddSingleton<QuartzStartup>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, IOCJobFactory>();
            //注入 HttpClientHelp（如果不用可以注释掉）
            services.AddTransient<HttpClientHelp>();            
            services.Configure<ApiBehaviorOptions>(options => {
                //禁用.net core webapi 项目本身的模型参数绑定验证体系
                options.SuppressModelStateInvalidFilter = true;
            });
            return RegisterAutofac(services);//注册Autofac
        }

        /// <summary>
        /// 注册Autofac
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            MyHttpContext.HttpContextAccessor = httpContextAccessor;
            //解决跨域（如果不用可以注释掉）
            app.UseCors("EnableCrossDomain");
            app.UseStaticFiles(); //注册wwwroot静态文件（如果不用可以注释掉）
        }
    }
}
