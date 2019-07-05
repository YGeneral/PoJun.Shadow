using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoJun.Shadow.Enum
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    [Description("数据库类型")]
    public enum DatabaseType : int
    {
        /// <summary>
        /// 无意义的，防止某些序列化工具在序列化时报错
        /// </summary>
        [Description("无意义的，防止某些序列化工具在序列化时报错")]
        None = 0,

        /// <summary>
        /// MongoDB
        /// </summary>
        [Description("MongoDB")]
        MongoDB = 1,

        /// <summary>
        /// MySql
        /// </summary>
        [Description("MySql")]
        MySql = 2,

        /// <summary>
        /// SqlServer
        /// </summary>
        [Description("SqlServer")]
        SqlServer = 3,
    }
}
