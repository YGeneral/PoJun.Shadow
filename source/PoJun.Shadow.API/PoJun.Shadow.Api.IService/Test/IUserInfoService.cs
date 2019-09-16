using PoJun.Shadow.Api.ContractModel.External.Test.v1;
using PoJun.Shadow.ContractModel;
using PoJun.Shadow.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.IService.Test
{
    public interface IUserInfoService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResponse> Add(AddTest_UserParam param);
    }
}
