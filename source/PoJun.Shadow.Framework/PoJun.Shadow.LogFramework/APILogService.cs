using PoJun.Shadow.Api.ContractModel.Framework.Log;
using PoJun.Shadow.Api.IRepository.Log;
using PoJun.Shadow.Enum;
using PoJun.Shadow.IFramework.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NIO.SpecialCar.LogFramework
{
    /// <summary>
    /// 接口日志服务
    /// </summary>
    public class APILogService : IAPILogService
    {
        /// <summary>
        /// 日志接口仓储
        /// </summary>
        public IAPILogsRepository apiLogsRepository;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_apiLogsRepository"></param>
        public APILogService(IAPILogsRepository _apiLogsRepository)
        {
            apiLogsRepository = _apiLogsRepository;
        }

        /// <summary>
        /// 新增请求日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<AddRequestLogModel> AddRequestLogAsync(AddRequestLogParam param)
        {
            return apiLogsRepository.AddRequestLogAsync(param);
        }

        /// <summary>
        /// 新增[响应/错误]日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task AddResponseLogAsync(AddResponseLogParam param)
        {
            return apiLogsRepository.AddResponseLogAsync(param);
        }
    }
}
