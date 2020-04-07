using PoJun.Dapper;
using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Entity.Test;
using PoJun.Shadow.Repository.MySql;
using PoJun.Shadow.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Api.MySqlRepository.Test
{
    /// <summary>
	/// 用户表仓储层
	/// 创建人：杨江军
	/// 创建时间：2020/4/7 11:42:16
	/// </summary>
	public class UserRepository : BaseRepositoryToPoJun_Shadow<User>, IUserRepository
    {
    }

    
}
