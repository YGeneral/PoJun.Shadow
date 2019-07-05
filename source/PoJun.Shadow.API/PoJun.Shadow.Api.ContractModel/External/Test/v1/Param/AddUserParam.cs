using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.External.Test.v1
{
    public class AddUserParam
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "【姓名】不能为空")]
        [RegularExpression("[A-Za-z0-9_\\-\u4e00-\u9fa5]+", ErrorMessage = "【姓名】中不能存在特殊字符")]
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Required(ErrorMessage = "【年龄】不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "【年龄】必须是正整数")]
        public int Age { get; set; }
    }
}
