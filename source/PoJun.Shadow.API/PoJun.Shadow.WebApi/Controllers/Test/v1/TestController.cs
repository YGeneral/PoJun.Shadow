using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoJun.Shadow.Api.ContractModel.External.Test.v1;
using PoJun.Shadow.ContractModel;

namespace PoJun.Shadow.WebApi.Controllers.Test.v1
{
    /// <summary>
    /// v1版测试Controller
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TestController : BaseController
    {
        #region 初始化

        ///// <summary>
        ///// 
        ///// </summary>
        //private IComponentContext componentContext;//Autofac上下文

        ///// <summary>
        ///// 初始化
        ///// </summary>
        ///// <param name="_componentContext"></param>
        //public TestController(IComponentContext _componentContext)
        //{
        //    ////指定V1
        //    //_v1userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v1.UserInfoService).Name);
        //    ////指定V2
        //    //_v2userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v2.UserInfoService).Name);
        //    ////指定V0
        //    //_v0userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.V0UserInfoService).Name);
        //    ////指定V3
        //    //_v3userInfoService = _componentContext.ResolveNamed<IUserInfoService>(typeof(MyAutofac.Api.Service.v3.UserInfoService).Name);
        //}

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


    }
}