using MongoDB.Bson.Serialization.Attributes;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Test_UserInfo : IAutoIncr<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [BsonId]
        public long ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
