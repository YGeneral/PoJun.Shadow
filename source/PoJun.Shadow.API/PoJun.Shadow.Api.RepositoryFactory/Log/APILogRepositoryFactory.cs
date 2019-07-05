using PoJun.Shadow.Api.IRepository.Log;
using PoJun.Shadow.Enum;
using PoJun.Shadow.Exception;
using PoJun.MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Api.RepositoryFactory.Log
{
    /// <summary>
    /// 【接口日志仓储】工厂
    /// </summary>
    public static class APILogRepositoryFactory
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <returns></returns>
        public static IAPILogsRepository Init(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MongoDB:
                    return PoJun.MongoDB.Repository.RepositoryContainer.Resolve<PoJun.Shadow.Api.MongoDBRepository.Log.APILogsRepository>();
                case DatabaseType.MySql:
                    return new MySqlRepository.Log.APILogsRepository();
                case DatabaseType.SqlServer:
                    return new SqlServerRepository.Log.APILogsRepository();
                default:
                    throw new RepositoryFactoryException(ErrorMessage.RFE_NotDatabaseType_Msg);
            }
        }
    }
}
