﻿using PoJun.Shadow.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 没有权限
    /// </summary>
    public class NoPermissionException : BaseException
    {
        /// <summary>
        ///  【无权限】异常(自定义异常类型)
        /// </summary>
        /// <param name="detailedStatus">接口详细描述代码</param>
        /// <param name="message">详细状态描述</param>
        /// <param name="gatewayStatus">网关状态</param>
        public NoPermissionException(string message = null, DetailedStatusCode detailedStatus = DetailedStatusCode.NoPermission, GatewayStatusCode gatewayStatus = GatewayStatusCode.Fail) : base(message, detailedStatus, gatewayStatus)
        {
        }
    }
}
