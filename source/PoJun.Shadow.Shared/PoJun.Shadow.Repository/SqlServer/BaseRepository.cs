using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Repository.SqlServer
{
    /// <summary>
    /// 【 PoJun.Shadow 业务库】基础仓储(SqlServer)
    /// </summary>
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseRepository(ISqlSugarClient context = null) : base(context)
        {
            if (context == null)
            {
                base.Context = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = DBConfig.GetInstance().SqlServer.PoJun_Shadow,
                    AopEvents = new AopEvents()
                    {
                        OnLogExecuting = (sql, pars) =>
                        {
                            if (Convert.ToBoolean(DBConfig.GetInstance().SqlServer.IsSqlDebug))
                                System.Diagnostics.Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} SqlSugar生成的SQL语句：【{sql}】");
                        }
                    }
                });
            }
        }
    }

    /// <summary>
    /// 【 PoJun.Shadow 业务库】基础仓储(SqlServer) 
    /// </summary>
    public interface IBaseRepository<T> : ISimpleClient<T> where T : class, new()
    {
    }
}
