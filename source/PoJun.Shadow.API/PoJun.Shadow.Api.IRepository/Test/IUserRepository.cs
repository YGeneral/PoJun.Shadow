using PoJun.Shadow.Entity.Test;
using PoJun.Shadow.Repository.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.IRepository.Test
{
    /// <summary>
	/// 用户表仓储层
	/// 创建人：杨江军
	/// 创建时间：2020/4/7 12:00:33
	/// </summary>
	public interface IUserRepository : IBaseRepository<User>
	{
        
    }
}
