using PoJun.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Tools
{
    /// <summary>
    /// AppMetrics配置信息
    /// </summary>
    public class AppMetricsConfig
    {
        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static AppMetricsConfig GetInstance()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AppMetricsConfig>(ConfigurationManager.AppSettings[nameof(AppMetricsConfig)]);
        }

        #endregion

        /// <summary>
        /// InfluxDB配置 
        /// </summary>
        public InfluxDB InfluxDB { get; set; }

        /// <summary>
        /// 私有内存阈值
        /// </summary>
        public long PrivateMemoryThreshold { get; set; }

        /// <summary>
        /// 虚拟内存阈值
        /// </summary>
        public long VirtualMemoryThreshold { get; set; }

        /// <summary>
        /// 物理内存阈值
        /// </summary>
        public long PhysicalMemoryThreshold { get; set; }
    }

    /// <summary>
    /// InfluxDB配置
    /// </summary>
    public class InfluxDB
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string DBUrl { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DBName { get; set; }
    }
}
