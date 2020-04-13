using PoJun.MongoDB.Repository;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.MongoDB
{
    /// <summary>
    /// 【XXX日志库】基础仓储(MongoDB)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepositoryToPoJun_Shadow_Logs<TEntity, TKey> : MongoRepositoryAsync<TEntity, TKey>, IBaseRepositoryToPoJun_Shadow_Logs<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow_Logs() : base(DBConfig.GetInstance().MongoDB.PoJun_Shadow_Logs, nameof(MongoDBConfig.PoJun_Shadow_Logs))
        {

        }
    }

    /// <summary>
    /// 【XXX业务库】基础仓储(MongoDB)
    /// </summary>
    public interface IBaseRepositoryToPoJun_Shadow_Logs<TEntity, TKey> : IMongoRepositoryAsync<TEntity, TKey>
    {
    }
}
