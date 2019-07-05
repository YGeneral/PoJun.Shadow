using PoJun.Shadow.Code;
using PoJun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.ContractModel
{
    #region  基础返回结果(返回数据)

    /// <summary>
    /// 基础返回结果(返回数据)
    /// </summary>
    public class BaseResponse<T>
    {
        /// <summary>
        /// 网关状态
        /// </summary>
        public GatewayStatusCode GatewayStatus { get; set; }

        /// <summary>
        /// 网关状态描述
        /// </summary>
        public string GatewayMessage { get; set; }

        /// <summary>
        /// 详细交易状态
        /// </summary>
        public DetailedStatusCode DetailedStatus { get; set; }

        /// <summary>
        /// 详细交易状态
        /// </summary>
        public string DetailedMessage { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseResponse()
        {
            GatewayStatus = GatewayStatusCode.Success;
            GatewayMessage = GatewayStatus.Description();
            DetailedStatus = DetailedStatusCode.Success;
            DetailedMessage = DetailedStatus.Description();
        }
    }

    #endregion

    #region  基础返回结果(不返回数据)

    /// <summary>
    /// 基础返回结果(不返回数据)
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 网关状态
        /// </summary>
        public GatewayStatusCode GatewayStatus { get; set; }

        /// <summary>
        /// 网关状态描述
        /// </summary>
        public string GatewayMessage { get; set; }

        /// <summary>
        /// 详细状态
        /// </summary>
        public DetailedStatusCode DetailedStatus { get; set; }

        /// <summary>
        /// 详细状态描述
        /// </summary>
        public string DetailedMessage { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseResponse()
        {
            GatewayStatus = GatewayStatusCode.Success;
            GatewayMessage = GatewayStatus.Description();
            DetailedStatus = DetailedStatusCode.Success;
            DetailedMessage = DetailedStatus.Description();
        }
    }

    #endregion
}
