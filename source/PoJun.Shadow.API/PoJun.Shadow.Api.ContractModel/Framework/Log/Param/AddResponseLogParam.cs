using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.Framework.Log
{
    /// <summary>
    /// 【新增响应日志】参数
    /// </summary>
    public class AddResponseLogParam
    {
        /// <summary>
        /// 父追踪ID
        /// </summary>
        public string ParentTraceID { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime ResponseTime { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string ErrorBody { get; set; }

        /// <summary>
        /// 请求耗时（毫秒）
        /// </summary>
        public int TimeCost { get; set; }

        /// <summary>
        /// 是否为报错日志
        /// </summary>
        public bool IsError { get; set; } = false;
    }
}
