using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Tools
{
    /// <summary>
    /// Redis配置文件
    /// </summary>
    public class RedisConfig
    {
        #region 初始化

        /// <summary>
        /// 
        /// </summary>
        private RedisConfig()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static RedisConfig GetInstance()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RedisConfig>(ConfigurationManager.AppSettings[nameof(RedisConfig)]);
        }

        #endregion

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public string RedisKey { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string RedisExchangeHosts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RedisDB { get; set; }
    }
}
