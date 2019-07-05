using PoJun.Shadow.Api.ContractModel.Framework.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.IRepository.Log
{
    /// <summary>
    /// 【接口日志】仓储
    /// </summary>
    public interface IAPILogsRepository
    {
        /// <summary>
        /// 新增请求日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<AddRequestLogModel> AddRequestLogAsync(AddRequestLogParam param);

        /// <summary>
        /// 新增响应日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddResponseLogAsync(AddResponseLogParam param);
    }
}
