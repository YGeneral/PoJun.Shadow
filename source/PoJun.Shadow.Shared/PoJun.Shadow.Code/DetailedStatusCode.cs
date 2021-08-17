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

        /// <summary>
        /// 重复登录
        /// </summary>
        [Description("重复登录")]
        RepeatLogin = 9,

        /// <summary>
        /// 验证码已过期或已使用
        /// </summary>
        [Description("验证码已过期或已使用")]
        VerifyCodeExpire = 10,

        /// <summary>
        /// Token已过期
        /// </summary>
        [Description("Token已过期")]
        TokenExpire = 11,

        /// <summary>
        /// 部分成功(操作数据时只有部分数据操作成功)
        /// </summary>
        [Description("部分成功(操作数据时只有部分数据操作成功)")]
        PartialSuccess = 12,

        /// <summary>
        /// 用户不存在
        /// </summary>
        [Description("用户不存在")]
        UserDoesNotExist = 13,

        /// <summary>
        /// 其他客户端登录
        /// </summary>
        [Description("其他客户端登录")]
        OtherClientLogin = 14,

        /// <summary>
        /// 该用户已经退出登录
        /// </summary>
        [Description("该用户已经退出登录")]
        LogOutExpire = 15,

        /// <summary>
        /// 用户已禁用
        /// </summary>
        [Description("用户已禁用")]
        UserDisabled = 16,

        /// <summary>
        /// 账号已过期
        /// </summary>
        [Description("账号已过期")]
        AccountExpired = 17,

        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermission = 18,

        /// <summary>
        /// 账号已冻结
        /// </summary>
        [Description("账号已冻结")]
        AccountFrozen = 19,

        /// <summary>
        /// 用户已冻结
        /// </summary>
        [Description("用户已冻结")]
        UserFrozen = 20,

        #endregion
    }
}
