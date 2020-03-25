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
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //注册MongoDB仓储（如果不用可以注释掉）
            RepositoryContainer.RegisterAll(AutofacModuleRegister.GetAllAssembliesName());
            services.AddControllers();
            services.AddMvc(option =>
            {
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
                //不使用驼峰样式的key
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //增加参数自动去除前后空格转换器
                option.SerializerSettings.Converters.Add(new TrimmingConverter());
            });

            #region 解决跨域

            //解决跨域（如果不用可以注释掉）
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCrossDomain", builder =>
                {
                    //builder.AllowAnyOrigin()//允许任何来源的主机访问
                    builder.WithOrigins(APIConfig.GetInstance().RequestSource)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            }); 

            #endregion

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region 接口版本控制

            services.AddApiVersioning(options =>
                {
                //当设置为 true 时, API 将返回响应标头中支持的版本信息
                options.ReportApiVersions = true;
                //此选项将用于不提供版本的请求。默认情况下, 假定的 API 版本为1.0。
                options.AssumeDefaultVersionWhenUnspecified = true;
                //此选项用于指定在请求中未指定版本时要使用的默认 API 版本。这将默认版本为1.0。
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                // 版本名的格式：v+版本号
                options.GroupNameFormat = "'v'V";
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            #endregion

            #region 注册Swagger服务

            this.Provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(c =>
            {
                //多版本控制
                foreach (var item in Provider.ApiVersionDescriptions)
                {
                    //添加文档信息
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = "绝影框架",
                        Version = item.ApiVersion.ToString(),
                        Description = "ASP.NET CORE 3.1 WebApi",
                        Contact = new OpenApiContact
                        {
                            Name = "PoJun",
                            Email = "general_y@126.com",
                            Url = new Uri("https://github.com/YGeneral/PoJun.Shadow")
                        }
                    });

                    #region 读取xml信息

                    //启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.ContractModel.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.ContractModel.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Enum.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Code.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.IService.xml"), true);
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.Service.xml"), true);

                    #endregion
                }
            }); 

            #endregion

            services.AddMvc();
            //注册权限验证
            services.AddScoped<AuthenticationAttribute>();

            #region Quartz调度框架注册

            //注入Quartz调度类（如果不用可以注释掉）
            services.AddSingleton<QuartzStartup>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, IOCJobFactory>(); 

            #endregion

            //注入 HttpClientHelp（如果不用可以注释掉）
            services.AddTransient<HttpClientHelp>();

            #region 自定义job注册

            //注入自定义job
            //services.AddSingleton<TestJob>(); 

            #endregion

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //禁用.net core webapi 项目本身的模型参数绑定验证体系
                options.SuppressModelStateInvalidFilter = true;
            });
            return RegisterAutofac(services);//注册Autofac
        }

        #region 注册Autofac

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

        #endregion

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            MyHttpContext.HttpContextAccessor = httpContextAccessor;

            #region 解决跨域

            //解决跨域（如果不用可以注释掉）
            app.UseCors("EnableCrossDomain"); 

            #endregion

            app.UseCors();
            app.UseStaticFiles();//注册wwwroot静态文件（如果不用可以注释掉）
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region 启动Job

            //执行数据导入定时同步Job
            //var quartz = app.ApplicationServices.GetRequiredService<QuartzStartup>();
            //【每分钟执行一次】
            //await quartz.Start<TestJob>("SyncTask", nameof(TestJob), "0 0/1 * * * ? "); 

            #endregion

            #region 启用Swagger

            //启用Swagger中间件
            app.UseSwagger();
            //配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                foreach (var item in Provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", SysUtil.GetSystemId() + item.ApiVersion);
                }
            }); 

            #endregion
        }
    }
}
