using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Entity.Test
{
    /// <summary>
	/// 用户信息表
	/// 创建人：杨江军
	/// 创建时间：2020/4/7 11:40:15
	/// </summary>
	public class User
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
