using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoJun.Shadow.Code
{
    /// <summary>
    /// 网关状态代码
    /// </summary>
    [Description("网关状态代码")]
    public enum GatewayStatusCode : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 2,
    }
}
