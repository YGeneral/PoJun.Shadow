using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Filtering;
using App.Metrics.AspNetCore;
using PoJun.Shadow.Tools;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())          
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder
               .UseStartup<Startup>()
               .UseUrls($"http://0.0.0.0:{APIConfig.GetInstance().Port}")//部署在linux上才生效，接口的端口配置
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
               })
               .UseNLog();
           })
           //.ConfigureMetricsWithDefaults(builder =>
           //{
           //    builder.Filter.With(new MetricsFilter());
           //    builder.Report.ToInfluxDb(options =>
           //    {
           //        options.InfluxDb.BaseUri = new Uri(AppMetricsConfig.GetInstance().InfluxDB.DBUrl);
           //        options.InfluxDb.Database = AppMetricsConfig.GetInstance().InfluxDB.DBName;
           //        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
           //        options.HttpPolicy.FailuresBeforeBackoff = 5;
           //        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
           //        options.FlushInterval = TimeSpan.FromSeconds(5);
           //    });
           //    //builder.Report.ToConsole(TimeSpan.FromSeconds(5));
           //    builder.Configuration.Configure(p =>
           //    {
           //        p.Enabled = true;
           //        p.ReportingEnabled = true;
           //        p.AddAppTag(SysUtil.GetSystemId());
           //        p.AddEnvTag(APIConfig.GetInstance().Environment);
           //    });
           //})
           //.UseMetrics()
           //.UseMetricsWebTracking()
           .Build()
           .Run();
        }
    }
}
