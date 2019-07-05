using MongoDB.Bson.Serialization.Attributes;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Entity.Log
{
    /// <summary>
    /// 接口日志
    /// </summary>
    [BsonIgnoreExtraElements]
    public class APILogs : IEntity<string>
    {
        #region 初始化

        /// <summary>
        /// 父追踪ID（简写）
        /// </summary>
        public const string PTID = "PTID";

        #endregion

        /// <summary>
        /// 日志追踪ID
        /// </summary>
        [BsonId]
        public string ID { get; set; }

        /// <summary>
        /// 父追踪ID
        /// </summary>
        [BsonElement(APILogs.PTID)]
        public string ParentTraceID { get; set; }

        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string APIName { get; set; }

        /// <summary>
        /// 客户端的域名或IP
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// 服务器的域名或IP
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 等级ID（从1开始）
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// [响应/错误]时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ResponseTime { get; set; }

        /// <summary>
        /// 请求耗时（毫秒）
        /// </summary>
        public int TimeCost { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string ErrorBody { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 是否为报错日志
        /// </summary>
        public bool IsError { get; set; } = false;
    }
}
