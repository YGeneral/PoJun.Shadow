using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoJun.Shadow.Code
{
    /// <summary>
    /// 接口详细描述代码
    /// </summary>
    [Description("接口详细描述代码")]
    public enum DetailedStatusCode : int
    {
        #region 公用

        /// <summary>
        /// 无意义的，防止某些序列化工具在序列化时报错
        /// </summary>
        [Description("无意义的，防止某些序列化工具在序列化时报错")]
        None = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 2,

        /// <summary>
        /// 未知异常
        /// </summary>
        [Description("未知异常")]
        Error = 3,

        /// <summary>
        /// 参数异常
        /// </summary>
        [Description("参数异常")]
        ParamsError = 4,

        /// <summary>
        /// 重复提交数据
        /// </summary>
        [Description("重复提交数据")]
        RepeatSubmit = 5,

        /// <summary>
        /// 配置错误
        /// </summary>3
        [Description("配置错误")]
        ConfigIsError = 6,

        /// <summary>
        /// 暂无数据
        /// </summary>
        [Description("暂无数据")]
        DataIsNull = 7,

        /// <summary>
        /// 数据已存在
        /// </summary>
        [Description("数据已存在")]
        DataAlreadyExists = 8,

        #endregion
    }
}
