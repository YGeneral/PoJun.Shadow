using PoJun.MongoDB.Repository;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.MongoDB
{
    /// <summary>
    /// 【 PoJun.Shadow 日志库】基础仓储(MongoDB)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseLogsRepository<TEntity, TKey> : MongoRepositoryAsync<TEntity, TKey>, IBaseLogsRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseLogsRepository() : base(DBConfig.GetInstance().MongoDB.PoJun_Shadow_Logs, nameof(MongoDBConfig.PoJun_Shadow_Logs))
        {

        }
    }

    /// <summary>
    /// 【 PoJun.Shadow 日志库】基础仓储(MongoDB)
    /// </summary>
    public interface IBaseLogsRepository<TEntity, TKey> : IMongoRepositoryAsync<TEntity, TKey>
    {
    }
}
