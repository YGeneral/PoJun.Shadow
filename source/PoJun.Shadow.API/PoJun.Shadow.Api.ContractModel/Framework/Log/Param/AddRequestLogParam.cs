using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.Framework.Log
{
    /// <summary>
    /// 【新增请求日志】参数
    /// </summary>
    public class AddRequestLogParam
    {
        /// <summary>
        /// 追踪ID【请求进入时生成的日志ID】
        /// </summary>
        public string TraceID { get; set; }

        /// <summary>
        /// 父追踪ID
        /// </summary>
        public string ParentTraceID { get; set; }

        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string APIName { get; set; }

        /// <summary>
        /// 客户端的域名或IP
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// 服务器的域名或IP
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 等级ID（从1开始）
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }
    }
}
