using PoJun.Dapper.Repository.MySql;
using PoJun.MongoDB.Repository;
using PoJun.MongoDB.Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.MySql
{
    /// <summary>
    /// 【XXX日志库】基础仓储(MongoDB)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepositoryToPoJun_Shadow_Logs : BaseRepository
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow_Logs() : base(DBConfig.GetInstance().MySql.PoJun_Shadow)
        {

        }
    }
}
