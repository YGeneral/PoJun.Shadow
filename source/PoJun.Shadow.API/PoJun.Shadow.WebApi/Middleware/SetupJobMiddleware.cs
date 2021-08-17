using PoJun.Shadow.WebApi.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoJun.Shadow.Tools;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 启动Job中间件
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:42:31
    /// </summary>
    public static class SetupJobMiddleware
    {
        /// <summary>
        /// 任务配置
        /// </summary>
        /// <param name="app"></param>
        public static async Task UseJobMilddleware(this IApplicationBuilder app)
        {
            //执行数据导入定时同步Job
            var quartz = app.ApplicationServices.GetRequiredService<QuartzStartup>();
            //【每分钟执行一次】
            await quartz.Start<TestJob>("SyncTask", nameof(TestJob), "0 0/1 * * * ? ");
        }
    }
}
