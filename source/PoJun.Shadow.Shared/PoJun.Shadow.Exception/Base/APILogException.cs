using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 【接口日志】异常
    /// </summary>
    public class APILogException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public APILogException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public APILogException(string message) : base(message)
        {
        }
    }
}
