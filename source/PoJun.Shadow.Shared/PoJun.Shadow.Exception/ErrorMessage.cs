using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 异常信息
    /// </summary>
    public static class ErrorMessage
    {
        /// <summary>
        /// 数据库类型不存在，工厂创建失败
        /// </summary>
        public static string RFE_NotDatabaseType_Msg = "数据库类型不存在，工厂创建失败！";

        /// <summary>
        /// 选择的数据类型下没有对应的仓储层，工厂创建失败
        /// </summary>
        public static string RFE_NotRepository_Msg = " 选择的数据类型下没有对应的仓储层，工厂创建失败！";

        /// <summary>
        /// 数据存储类型不存在，工厂创建失败
        /// </summary>
        public static string FFE_NotDataStorageType_Msg = "数据存储类型不存在，工厂创建失败！";
    }
}
