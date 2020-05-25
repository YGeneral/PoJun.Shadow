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
        /// <summary>
        /// ��ʼ��
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
        /// Api�汾��Ϣ
        /// </summary>
        private IApiVersionDescriptionProvider Provider;

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //ע��MongoDB�ִ���������ÿ���ע�͵���
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
                //����ѭ������
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //���Ӳ����Զ�ȥ��ǰ��ո�ת����
                option.SerializerSettings.Converters.Add(new TrimmingConverter());
            });

            #region �������

            //�������������ÿ���ע�͵���
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCrossDomain", builder =>
                {
                    builder.AllowAnyOrigin()//�����κ���Դ����������
                    //builder.WithOrigins(APIConfig.GetInstance().RequestSource)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//ָ������cookie
                });
            });

            #endregion

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region �ӿڰ汾����

            services.AddApiVersioning(options =>
                {
                    //������Ϊ true ʱ, API ��������Ӧ��ͷ��֧�ֵİ汾��Ϣ
                    options.ReportApiVersions = true;
                    //��ѡ����ڲ��ṩ�汾������Ĭ�������, �ٶ��� API �汾Ϊ1.0��
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    //��ѡ������ָ����������δָ���汾ʱҪʹ�õ�Ĭ�� API �汾���⽫Ĭ�ϰ汾Ϊ1.0��
                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddVersionedApiExplorer(options =>
                {
                    //�汾���ĸ�ʽ��v+�汾��
                    options.GroupNameFormat = "'v'V";
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            #endregion

            #region ע��Swagger����

            services.AddSwaggerGen(c =>
            {
                //��汾����
                foreach (var item in Provider.ApiVersionDescriptions)
                {
                    //����ĵ���Ϣ
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = "��Ӱ���",
                        Version = item.ApiVersion.ToString(),
                        Description = "ASP.NET CORE 3.1 WebApi",
                        Contact = new OpenApiContact
                        {
                            Name = "PoJun",
                            Email = "general_y@126.com",
                            Url = new Uri("https://github.com/YGeneral/PoJun.Shadow")
                        }
                    });
                }

                c.DocumentFilter<SwaggerEnumFilter>();

                #region ��ȡxml��Ϣ

                //����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.ContractModel.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.ContractModel.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Enum.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Code.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.IService.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"PoJun.Shadow.Api.Service.xml"), true);

                #endregion
            });

            #endregion

            services.AddMvc();
            //ע��Ȩ����֤
            services.AddScoped<AuthenticationAttribute>();

            #region Quartz���ȿ��ע��

            //ע��Quartz�����ࣨ������ÿ���ע�͵���
            services.AddSingleton<QuartzStartup>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, IOCJobFactory>();

            #endregion

            //ע�� HttpClientHelp��������ÿ���ע�͵���
            services.AddTransient<HttpClientHelp>();

            #region �Զ���jobע��

            //ע���Զ���job
            services.AddSingleton<TestJob>();

            #endregion

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //����.net core webapi ��Ŀ�����ģ�Ͳ�������֤��ϵ
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddHttpClient();
            services.AddHttpClient("base").AddTransientHttpErrorPolicy(x => x.RetryAsync(3));//������Բ��ԡ� ������ʧ�ܣ�������������;
            return RegisterAutofac(services);//ע��Autofac
        }

        #region ע��Autofac

        /// <summary>
        /// ע��Autofac
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //ʵ����Autofac����
            var builder = new ContainerBuilder();
            //��Services�еķ�����䵽Autofac��
            builder.Populate(services);
            //��ȡ�ӿڰ汾��Ϣ
            this.Provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            //��ģ�����ע��    
            builder.RegisterModule<AutofacModuleRegister>();
            //��������
            var Container = builder.Build();
            //������IOC�ӹ� core����DI���� 
            return new AutofacServiceProvider(Container);
        }

        #endregion

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            MyHttpContext.HttpContextAccessor = httpContextAccessor;

            #region �������

            //�������������ÿ���ע�͵���
            app.UseCors("EnableCrossDomain");

            #endregion

            app.UseCors();
            app.UseStaticFiles();//ע��wwwroot��̬�ļ���������ÿ���ע�͵���
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region ����Job

            //ִ�����ݵ��붨ʱͬ��Job
            var quartz = app.ApplicationServices.GetRequiredService<QuartzStartup>();
            //��ÿ����ִ��һ�Ρ�
            await quartz.Start<TestJob>("SyncTask", nameof(TestJob), "0 0/1 * * * ? ");

            #endregion

            #region ����Swagger

            //����Swagger�м��
            app.UseSwagger();
            //����SwaggerUI
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
