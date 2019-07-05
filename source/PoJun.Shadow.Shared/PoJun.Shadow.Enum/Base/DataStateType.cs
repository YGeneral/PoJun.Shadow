using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoJun.Shadow.Enum
{
    /// <summary>
    /// 数据状态类型
    /// </summary>
    [Description("数据状态类型")]
    public enum DataStateType : int
    {
        /// <summary>
        /// 无意义的，防止某些序列化工具在序列化时报错
        /// </summary>
        [Description("无意义的，防止某些序列化工具在序列化时报错")]
        None = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Frozen = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 3,
    }
}
