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
        #region ��ʼ��

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
        /// ����
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
                //����ѭ������
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //�շ��ʽ
                option.SerializerSettings.ContractResolver = APIConfig.GetInstance().IsLittleCamelFormat ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver();
                //���Ӳ����Զ�ȥ��ǰ��ո�ת����
                option.SerializerSettings.Converters.Add(new TrimmingConverter());
            });
            //�������������ÿ���ע�͵���
            services.AddCustomCors();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCustomApiVersioning();//�ӿڰ汾����            
            services.AddCustomSwagger(Env);//ע��Swagger����
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //����.net core webapi ��Ŀ�����ģ�Ͳ�������֤��ϵ
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddHttpClient();
            services.AddHttpClient("base").AddTransientHttpErrorPolicy(x => x.RetryAsync(3));//������Բ��ԡ� ������ʧ�ܣ�������������;
            services.AddCustomUploadFileSizeLimit();//�ϴ��ļ���С��������
            services.AddHealth().AddHealthEndpoints().AddHealthChecks();//ע��AddMetrics�Ľ������
            services.AddHealthEndpoints();
        }

        /// <summary>
		/// ���Autofac DI���񹤳�
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
            app.UseCors("EnableCrossDomain");//�������������ÿ���ע�͵���
            //app.UseStaticFiles();//ע��wwwroot��̬�ļ���������ÿ���ע�͵���
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseCustomAppMetrics();//����AppMetrics
            //����Job��������ÿ���ע�͵���
            //Task.Run(async () => { await app.UseJobMilddleware(); });
        }
    }
}
