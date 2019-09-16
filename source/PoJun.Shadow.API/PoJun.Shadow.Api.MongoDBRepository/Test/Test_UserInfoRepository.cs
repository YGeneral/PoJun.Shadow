using PoJun.Shadow.Api.ContractModel.Inside.Test;
using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Entity;
using PoJun.Shadow.Repository.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PoJun.Util.Maps;
using PoJun.Shadow.Api.ContractModel.Shared.Entity;

namespace PoJun.Shadow.Api.MongoDBRepository.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class Test_UserInfoRepository : BaseRepositoryToPoJun_Shadow<Test_UserInfo, long>, IUserInfoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<AddTest_UserModel> Add(AddTest_UserParam param)
        {
            var result = await this.InsertAsync(param.MapTo<Test_UserInfo>());
            return new AddTest_UserModel() { User = result.MapTo<Test_UserInfoEntity>() };
        }
    }
}
