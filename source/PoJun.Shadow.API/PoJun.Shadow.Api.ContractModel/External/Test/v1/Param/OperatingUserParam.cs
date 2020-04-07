using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.External.Test
{
    /// <summary>
    /// 操作用户信息[参数]
    /// </summary>
    public class OperatingUserParam
    {
        /// <summary>
        /// 操作类型 1:新增 2：修改 3：删除 4：查询
        /// </summary>
        [Required(ErrorMessage = "【操作类型】不能为空")]
        [Range(1, 4, ErrorMessage = "【操作类型】错误")]
        public int Type { get; set; }
    }
}
