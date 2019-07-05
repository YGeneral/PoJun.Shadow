using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.External.Test.v2
{
    /// <summary>
    /// 默认测试接口[参数]
    /// </summary>
    public class IndexParam
    {
        /// <summary>
        /// Token
        /// </summary>
        [Required(ErrorMessage = "Token不能为空")]
        [StringLength(maximumLength: 32, MinimumLength = 32, ErrorMessage = "Token无效")]
        public string Token { get; set; }
    }
}
