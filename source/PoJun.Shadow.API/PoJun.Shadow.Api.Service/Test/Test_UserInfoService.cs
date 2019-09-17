using PoJun.Shadow.Api.ContractModel.External.Test.v1;
using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Api.IService.Test;
using PoJun.Shadow.ContractModel;
using PoJun.Shadow.Exception;
using PoJun.Util.Maps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.Service.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class Test_UserInfoService : ITest_UserInfoService
    {
        #region 初始化

        private IUserInfoRepository userInfoRepository;

        public Test_UserInfoService(IUserInfoRepository _userInfoRepository)
        {
            userInfoRepository = _userInfoRepository;
        }

        #endregion

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Add(AddTest_UserParam param)
        {
            var result = await userInfoRepository.Add(param.MapTo<PoJun.Shadow.Api.ContractModel.Inside.Test.AddTest_UserParam>());
            if (result == null)
                throw new FailException();
            return new BaseResponse();
        }
    }
}
