using PoJun.Dapper.IRepository;
using PoJun.Dapper.Repository.MySql;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.MySql
{
    /// <summary>
    /// 【XXX业务库】基础仓储(MySql)
    /// </summary>
    public class BaseRepositoryToPoJun_Shadow<T> : BaseRepository<T>, IBaseRepositoryToPoJun_Shadow<T> where T : class
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepositoryToPoJun_Shadow() : base(DBConfig.GetInstance().MySql.PoJun_Shadow)
        {

        }
    }

    /// <summary>
    /// 【XXX业务库】基础仓储(MySql)
    /// </summary>
    public interface IBaseRepositoryToPoJun_Shadow<T> : IBaseRepository<T>
    {
    }
}
