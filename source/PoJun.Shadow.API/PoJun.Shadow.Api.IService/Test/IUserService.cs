using PoJun.Shadow.Api.ContractModel.External.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.IService.Test
{
    /// <summary>
    /// 用户表服务
    /// 创建人：杨江军
    /// 创建时间：2020/4/7 12:58:59
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// MySQL框架功能测试（本功能为测试使用）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Test(OperatingUserParam param);
    }
}
