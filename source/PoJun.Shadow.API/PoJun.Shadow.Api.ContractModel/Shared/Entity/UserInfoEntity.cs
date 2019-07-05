using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Api.ContractModel.Shared.Entity
{
    public class UserInfoEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
