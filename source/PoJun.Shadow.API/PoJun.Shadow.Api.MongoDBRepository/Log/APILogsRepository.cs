using MongoDB.Driver;
using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Api.IRepository.Log;
using PoJun.Shadow.Entity.Log;
using PoJun.Shadow.Repository.MongoDB;
using PoJun.Util.Maps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.MongoDBRepository.Log
{
    /// <summary>
    /// 【接口日志】仓储
    /// </summary>
    public class APILogsRepository : BaseRepositoryToPoJun_Shadow_Logs<APILogs, string>, IAPILogsRepository
    {
        #region 新增请求日志

        /// <summary>
        /// 新增请求日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<AddRequestLogModel> AddRequestLogAsync(AddRequestLogParam param)
        {
            var entity = param.MapTo(new APILogs());
            entity.ID = param.TraceID;
            entity.RequestTime = DateTime.Now;
            await this.InsertAsync(entity);
            return new AddRequestLogModel() { RequestTime = entity.RequestTime, TraceID = param.TraceID };
        }

        #endregion

        #region 新增响应日志

        /// <summary>
        /// 新增响应日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task AddResponseLogAsync(AddResponseLogParam param)
        {
            var filter = Filter.Eq(nameof(APILogs.ID), param.ParentTraceID);
            var update = Update
                .Set(nameof(APILogs.ResponseBody), param.ResponseBody)
                .Set(nameof(APILogs.ResponseTime), DateTime.Now)
                .Set(nameof(APILogs.TimeCost), param.TimeCost);

            if (param.IsError)
            {
                update = update.Set(nameof(APILogs.IsError), param.IsError)
                    .Set(nameof(APILogs.ErrorBody), param.ErrorBody)
                    .Set(nameof(APILogs.ErrorBody), param.ErrorBody);
            }

            return this.UpdateOneAsync(filter, update);
        }

        #endregion
    }
}
