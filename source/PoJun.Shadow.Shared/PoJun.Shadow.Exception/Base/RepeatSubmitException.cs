using PoJun.Shadow.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 【重复提交】异常
    /// </summary>
    public class RepeatSubmitException : BaseException
    {
        /// <summary>
        ///  【重复提交】异常(自定义异常类型)
        /// </summary>
        /// <param name="detailedStatus">接口详细描述代码</param>
        /// <param name="message">详细状态描述</param>
        /// <param name="gatewayStatus">网关状态</param>
        public RepeatSubmitException(string message = null, DetailedStatusCode detailedStatus = DetailedStatusCode.RepeatSubmit, GatewayStatusCode gatewayStatus = GatewayStatusCode.Success) : base(message, detailedStatus, gatewayStatus)
        {
        }
    }
}
