using PoJun.Shadow.Api.IRepository.Test;
using PoJun.Shadow.Entity.Test;
using PoJun.Shadow.Repository.MySql;
using PoJun.Shadow.Tools;
using SqlSugar;
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
	public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(ISqlSugarClient context = null) : base(context)
        {

        }

        //方法名                             描述                               返回值
        //this.Context.Ado.SqlQuery<T>       查询所有返回实体集合               List
        //this.Context.Ado.SqlQuery<T, T2>   可以返回2个结果集                  Tuple<List, List>
        //this.Context.Ado.SqlQuerySingle    查询第一条记录                     T
        //this.Context.Ado.GetDataTable      查询所有                           DataTable
        //this.Context.Ado.GetDataReader     读取DR需要手动释放DR               DataReader
        //this.Context.Ado.GetDataSetAll     获取多个结果集                     DataSet
        //this.Context.Ado.ExecuteCommand    返回受影响行数，一般用于增删改     int
        //this.Context.Ado.GetScalar         获取首行首列                       object
        //this.Context.Ado.GetString         获取首行首列                       string
        //this.Context.Ado.GetInt            获取首行首列                       int
        //this.Context.Ado.GetLong           获取首行首列                       long
        //this.Context.Ado.GetDouble         获取首行首列                       Double
        //this.Context.Ado.GetDecimal        获取首行首列                       Decimal
        //this.Context.Ado.GetDateTime       获取首行首列                       DateTime
    }
}
