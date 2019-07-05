using PoJun.MongoDB.Repository;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.MongoDB
{
    /// <summary>
    /// 【XXX业务库】基础仓储(MongoDB)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepositoryToPoJun_Shadow<TEntity, TKey> : MongoRepositoryAsync<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow() : base(DBConfig.GetInstance().MongoDB.PoJun_Shadow, nameof(MongoDBConfig.PoJun_Shadow))
        {

        }
    }
}
