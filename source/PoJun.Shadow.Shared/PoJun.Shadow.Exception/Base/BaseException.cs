using PoJun.Shadow.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoJun.Util;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 异常基类
    /// </summary>
    public class BaseException : System.Exception
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public DetailedStatusCode DetailedStatus;

        /// <summary>
        /// 网关状态
        /// </summary>
        public GatewayStatusCode GatewayStatus;

        /// <summary>
        ///  失败异常(自定义异常类型)
        /// </summary>
        /// <param name="detailedStatus">接口详细描述代码</param>
        /// <param name="message">详细状态描述</param>
        /// <param name="gatewayStatus">网关状态</param>
        public BaseException(string message = null, DetailedStatusCode detailedStatus = DetailedStatusCode.Fail, GatewayStatusCode gatewayStatus = GatewayStatusCode.Success) : base(string.IsNullOrWhiteSpace(message) ? detailedStatus.Description() : message)
        {
            DetailedStatus = detailedStatus;
            GatewayStatus = gatewayStatus;
        }
    }
}
