using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig
    {
        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static DBConfig GetInstance()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DBConfig>(ConfigurationManager.AppSettings[nameof(DBConfig)]);
        }

        #endregion

        /// <summary>
        /// MySql数据库配置
        /// </summary>
        public MySqlConfig MySql { get; set; }

        /// <summary>
        /// MongoDB数据库配置
        /// </summary>
        public MongoDBConfig MongoDB { get; set; }

        /// <summary>
        /// SqlServer数据库配置
        /// </summary>
        public SqlServerConfig SqlServer { get; set; }
    }

    #region MySql数据库配置

    /// <summary>
    /// MySql数据库配置
    /// </summary>
    public class MySqlConfig
    {
        /// <summary>
        /// 业务库地址
        /// </summary>
        public string PoJun_Shadow { get; set; }

        /// <summary>
        /// 日志库地址(字段名就是数据库名)
        /// </summary>
        public string PoJun_Shadow_Logs { get; set; }
    }

    #endregion

    #region SqlServer数据库配置

    /// <summary>
    /// MySql数据库配置
    /// </summary>
    public class SqlServerConfig
    {
        /// <summary>
        /// 业务库地址
        /// </summary>
        public string PoJun_Shadow { get; set; }

        /// <summary>
        /// 日志库地址(字段名就是数据库名)
        /// </summary>
        public string PoJun_Shadow_Logs { get; set; }
    }

    #endregion

    #region MongoDB数据库配置

    /// <summary>
    /// MongoDB数据库配置
    /// </summary>
    public class MongoDBConfig
    {
        /// <summary>
        /// 业务库地址
        /// </summary>
        public string PoJun_Shadow { get; set; }

        /// <summary>
        /// 日志库地址(字段名就是数据库名)
        /// </summary>
        public string PoJun_Shadow_Logs { get; set; }

    }

    #endregion
}
