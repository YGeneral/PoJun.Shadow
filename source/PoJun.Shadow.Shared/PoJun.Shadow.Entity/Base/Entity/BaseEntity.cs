using PoJun.Shadow.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Entity
{
    /// <summary>
    /// 基础实体
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 创建人Id
        /// </summary>
        public long Sys_CreateById { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string Sys_CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Sys_CreateTime { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        public long Sys_UpdateById { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string Sys_UpdateByName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Sys_UpdateTime { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public DataStateType Sys_DataState { get; set; } = DataStateType.Normal;
    }
}
