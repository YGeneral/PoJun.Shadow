using Autofac;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Jobs
{
    /// <summary>
    /// 测试job
    /// </summary>
    public class TestJob : IJob
    {
        #region 初始化

        //private IComponentContext componentContext;//Autofac上下文

        ///// <summary>
        ///// 初始化
        ///// </summary>
        ///// <param name="_componentContext"></param>
        //public TestJob(IComponentContext _componentContext)
        //{
        //    //默认
            
        //}

        #endregion

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            //调用Server层的代码实现业务逻辑
            await Task.FromResult(0);           
        }
    }
}
