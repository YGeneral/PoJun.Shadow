﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Tools 
{
    /// <summary>
    /// 接口相关配置
    /// </summary>
    public class APIConfig
    {
        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static APIConfig GetInstance()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<APIConfig>(ConfigurationManager.AppSettings[nameof(APIConfig)]);
        }

        #endregion

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 环境
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// 是否为小驼峰格式
        /// </summary>
        public bool IsLittleCamelFormat { get; set; }
    }
}
