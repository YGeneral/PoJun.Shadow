using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 启动AppMetrics
    /// </summary>
    public static class SetupAppMetricsMilddleware
    {
        /// <summary>
        /// 启动AppMetrics
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomAppMetrics(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var metricsRoot = services.GetService<IMetricsRoot>();
            SetupHealth(metricsRoot);
            app.UseMetricsAllMiddleware();
            app.UseHealthAllEndpoints();
        }

        /// <summary>
        /// 设置运行状态检测
        /// </summary>
        /// <param name="metricsRoot"></param>
        public static void SetupHealth(IMetricsRoot metricsRoot)
        {
            var healthBuilder = new HealthBuilder();
            var health = healthBuilder.Configuration.Configure(p =>
            {
                p.Enabled = true;
                p.ReportingEnabled = true;
            })
            .Report
            .ToMetrics(metricsRoot)
            .HealthChecks.AddProcessPrivateMemorySizeCheck($"占用的[私有内存]是否超过阀值({SysUtil.ConvertBytes(AppMetricsConfig.GetInstance().PrivateMemoryThreshold)})", AppMetricsConfig.GetInstance().PrivateMemoryThreshold)
            //.HealthChecks.AddProcessVirtualMemorySizeCheck($"占用的[虚拟内存]是否超过阀值({SysUtil.ConvertBytes(AppMetricsConfig.GetInstance().VirtualMemoryThreshold)})", AppMetricsConfig.GetInstance().VirtualMemoryThreshold)
            .HealthChecks.AddProcessPhysicalMemoryCheck($"占用的[物理内存]是否超过阀值({SysUtil.ConvertBytes(AppMetricsConfig.GetInstance().PhysicalMemoryThreshold)})", AppMetricsConfig.GetInstance().PhysicalMemoryThreshold)
            //.HealthChecks.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10))
            //.HealthChecks.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10))
            .Build();

            var scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(5), async () =>
            {
                var healthStatus = await health.HealthCheckRunner.ReadAsync();

                foreach (var reporter in health.Reporters)
                {
                    await reporter.ReportAsync(health.Options, healthStatus);
                }
            });
            scheduler.Start();
        }
    }
}
