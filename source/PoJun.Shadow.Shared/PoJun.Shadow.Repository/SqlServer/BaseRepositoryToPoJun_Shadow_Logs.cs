using PoJun.Dapper.IRepository;
using PoJun.Dapper.Repository.SqlServer;
using PoJun.MongoDB.Repository;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.SqlServer
{
    /// <summary>
    /// 【XXX日志库】基础仓储(SqlServer)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepositoryToPoJun_Shadow_Logs<T> : BaseRepository<T>, IBaseRepositoryToPoJun_Shadow_Logs<T> where T : class
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow_Logs() : base(DBConfig.GetInstance().SqlServer.PoJun_Shadow)
        {

        }
    }

    /// <summary>
    /// 【XXX日志库】基础仓储(SqlServer)
    /// </summary>
    public interface IBaseRepositoryToPoJun_Shadow_Logs<T> : IBaseRepository<T>
    {
    }
}
