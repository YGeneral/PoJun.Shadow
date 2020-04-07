using PoJun.Dapper.IRepository;
using PoJun.Dapper.Repository.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.SqlServer
{
    /// <summary>
    /// 【XXX业务库】基础仓储(MySql)
    /// </summary>
    public class BaseRepositoryToPoJun_Shadow<T> : BaseRepository<T>, IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow() : base(DBConfig.GetInstance().SqlServer.PoJun_Shadow)
        {

        }
    }
}
