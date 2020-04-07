using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoJun.Shadow.Api.ContractModel.External.Test;
using PoJun.Shadow.Api.ContractModel.External.Test.v1;
using PoJun.Shadow.Api.IService.Test;
using PoJun.Shadow.ContractModel;

namespace PoJun.Shadow.WebApi.Controllers.Test.v1
{
    /// <summary>
    /// v1版测试Controller
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TestController : BaseController
    {
        #region 初始化

        ///// <summary>
        ///// Autofac上下文
        ///// </summary>
        //private IComponentContext componentContext;

        /// <summary>
        /// 用户表服务
        /// </summary>
        private IUserService userService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_userService"></param>
        public TestController(IUserService _userService/*IComponentContext _componentContext*/)
        {
            userService = _userService;
            ////指定V1
            //_v1userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v1.UserInfoService).Name);
            ////指定V2
            //_v2userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v2.UserInfoService).Name);
            ////指定V0
            //_v0userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.V0UserInfoService).Name);
            ////指定V3
            //_v3userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v3.UserInfoService).Name);
        }

        #endregion

        /// <summary>
        /// v1版测试Index接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            return $"[破军测试接口v1.0]-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }

        /// <summary>
        /// MySQL框架功能测试（本功能为测试使用）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Test")]
        public Task Test(OperatingUserParam param)
        {
            return userService.Test(param);
        }
    }
}