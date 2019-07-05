using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Api.IRepository.Log;
using PoJun.Shadow.Entity.Log;
using PoJun.Shadow.Repository.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.MySqlRepository.Log
{
    /// <summary>
    /// 【接口日志】仓储
    /// </summary>
    public class APILogsRepository : BaseRepositoryToPoJun_Shadow_Logs, IAPILogsRepository
    {
        #region 新增请求日志

        /// <summary>
        /// 新增请求日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<AddRequestLogModel> AddRequestLogAsync(AddRequestLogParam param)
        {
            var result = new AddRequestLogModel();
            param.RequestTime = DateTime.Now;
            result.RequestTime = param.RequestTime;
            result.TraceID = param.TraceID;
            //创建SQL语句
            var sqlStr = $"insert into {nameof(APILogs)} ({nameof(APILogs.ID)},{nameof(APILogs.ParentTraceID)},{nameof(APILogs.SystemID)},{nameof(APILogs.APIName)},{nameof(APILogs.ClientHost)},{nameof(APILogs.ServerHost)},{nameof(APILogs.Level)},{nameof(APILogs.RequestBody)},{nameof(APILogs.Headers)},{nameof(APILogs.RequestTime)}) values(@{nameof(AddRequestLogParam.TraceID)},@{nameof(AddRequestLogParam.ParentTraceID)},@{nameof(AddRequestLogParam.SystemID)}, @{nameof(AddRequestLogParam.APIName)} , @{nameof(AddRequestLogParam.ClientHost)} , @{nameof(AddRequestLogParam.ServerHost)},@{nameof(AddRequestLogParam.Level)},@{nameof(AddRequestLogParam.RequestBody)},@{nameof(AddRequestLogParam.Headers)},@{nameof(AddRequestLogParam.RequestTime)})";
            //新增日志
            await this.ExecuteAsync(sqlStr, param);
            return result;
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
            var sqlStr = $"update {nameof(APILogs)} set {nameof(APILogs.ResponseBody)} = @{nameof(AddResponseLogParam.ResponseBody)},{nameof(APILogs.ResponseTime)} = @{nameof(AddResponseLogParam.ResponseTime)},{nameof(APILogs.TimeCost)} = @{nameof(AddResponseLogParam.TimeCost)},{nameof(APILogs.IsError)} = @{nameof(AddResponseLogParam.IsError)},{nameof(APILogs.ErrorBody)} = @{nameof(AddResponseLogParam.ErrorBody)},{nameof(APILogs.ErrorBody)} = @{nameof(AddResponseLogParam.ErrorBody)} where {nameof(APILogs.ID)} = @{nameof(AddResponseLogParam.ParentTraceID)}";

            return this.ExecuteAsync(sqlStr, param);
        }

        #endregion
    }
}
