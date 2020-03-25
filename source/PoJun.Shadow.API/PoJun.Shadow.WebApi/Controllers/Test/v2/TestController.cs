using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PoJun.Shadow.WebApi.Controllers.Test.v2
{
    /// <summary>
    /// v2版测试Controller
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class TestController : BaseController
    {
        /// <summary>
        /// v2版测试Index接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            return $"[破军测试接口v2.0]-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
    }
}