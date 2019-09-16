using PoJun.Shadow.Api.ContractModel.Inside.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.IRepository.Test
{
    public interface IUserInfoRepository
    {
        Task<AddTest_UserModel> Add(AddTest_UserParam param);
    }
}
