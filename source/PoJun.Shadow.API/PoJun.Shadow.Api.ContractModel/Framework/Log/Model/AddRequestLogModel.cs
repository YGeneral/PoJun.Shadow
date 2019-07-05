using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.Framework.Log
{
    /// <summary>
    /// 【新增请求日志】返回结果
    /// </summary>
    public class AddRequestLogModel
    {
        /// <summary>
        /// 追踪ID【请求进入时生成的日志ID】
        /// </summary>
        public string TraceID { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }
    }
}
