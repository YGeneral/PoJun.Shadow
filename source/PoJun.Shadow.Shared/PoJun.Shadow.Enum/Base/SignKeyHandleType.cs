using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoJun.Shadow.Enum
{
    /// <summary>
    /// 签名Key处理类型
    /// </summary>
    [Description("签名Key处理类型")]
    public enum SignKeyHandleType : int
    {
        /// <summary>
        /// 无意义的，防止某些序列化工具在序列化时报错
        /// </summary>
        [Description("无意义的，防止某些序列化工具在序列化时报错")]
        None = 0,

        /// <summary>
        /// 不处理
        /// </summary>
        [Description("不处理")]
        Normal = 1,

        /// <summary>
        /// 首字母小写
        /// </summary>
        [Description("首字母小写")]
        InitialLower = 2,

        /// <summary>
        /// 全小写
        /// </summary>
        [Description("全小写")]
        AllLower = 3,

        /// <summary>
        /// 全大写
        /// </summary>
        [Description("全大写")]
        AllUpper = 4,

        /// <summary>
        /// 首字母大写
        /// </summary>
        [Description("首字母大写")]
        InitialUpper = 5,
    }
}
