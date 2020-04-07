using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 【参数错误】异常
    /// </summary>
    public class ParamErrorException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ParamErrorException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ParamErrorException(string message) : base(message)
        {
        }
    }
}
