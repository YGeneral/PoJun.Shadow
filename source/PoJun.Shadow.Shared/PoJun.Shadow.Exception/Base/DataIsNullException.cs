using PoJun.Shadow.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 暂无数据异常
    /// </summary>
    public class DataIsNullException : BaseException
    {
        /// <summary>
        /// 暂无数据异常(自定义异常类型)
        /// </summary>
        /// <param name="detailedStatus">接口详细描述代码</param>
        /// <param name="message">详细状态描述</param>
        /// <param name="gatewayStatus">网关状态</param>
        public DataIsNullException(string message = null, DetailedStatusCode detailedStatus = DetailedStatusCode.DataIsNull, GatewayStatusCode gatewayStatus = GatewayStatusCode.Success) : base(message, detailedStatus, gatewayStatus)
        {
        }
    }
}
