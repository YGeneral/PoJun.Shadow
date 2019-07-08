using Autofac;
using PoJun.Shadow.Api.IService.Test;
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

        private IUserInfoService userInfoService;
        private IComponentContext componentContext;//Autofac上下文

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_componentContext"></param>
        /// <param name="_userInfoService"></param>
        public TestJob(IComponentContext _componentContext, IUserInfoService _userInfoService)
        {
            //默认
            userInfoService = _userInfoService;
        }

        #endregion

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            //调用Server层的代码实现业务逻辑
            await userInfoService.Add(new Api.ContractModel.External.Test.v1.AddUserParam() { Age=new PoJun.Util.Helpers.Random().Next(int.MaxValue), Name=$"PoJun-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}" });
        }
    }
}
