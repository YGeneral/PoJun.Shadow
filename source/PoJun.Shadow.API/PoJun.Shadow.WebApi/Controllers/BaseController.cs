using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Controllers
{
    [Produces("application/json")]
    [EnableCors("EnableCrossDomain")]//设置跨域处理的 代理
    [ServiceFilter(typeof(PoJun.Shadow.WebApi.Filters.AuthenticationAttribute))]
    public class BaseController : Controller
    {

    }
}
