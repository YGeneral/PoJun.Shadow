using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class QuartzStartup
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _iocJobfactory;
        private IScheduler _scheduler;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocJobfactory"></param>
        /// <param name="schedulerFactory"></param>
        public QuartzStartup(IJobFactory iocJobfactory, ISchedulerFactory schedulerFactory)
        {
            //1、声明一个调度工厂
            this._schedulerFactory = schedulerFactory;
            this._iocJobfactory = iocJobfactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <param name="cron"></param>
        /// <returns></returns>
        public async Task Start<T>(string jobGroup, string jobName, string cron) where T : IJob
        {
            //2、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            _scheduler.JobFactory = this._iocJobfactory;//  替换默认工厂
            //3、开启调度器
            await _scheduler.Start();
            //4、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithIdentity(jobName, jobGroup)
                            .WithCronSchedule(cron)
                            .Build();
            //5、创建任务
            var job = JobBuilder.Create<T>()
                            .WithIdentity(jobName, jobGroup)
                            .Build();
            //6、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
        }
    }
}
