using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PoJun.Shadow.WebApi.Controllers.Test.v2
{
    //[Route("api/[controller]")]
    [Route("api/Test/v{version:apiVersion}")]
    [ApiVersion("2.0")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            return $"[破军测试接口v2.0]-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
    }
}