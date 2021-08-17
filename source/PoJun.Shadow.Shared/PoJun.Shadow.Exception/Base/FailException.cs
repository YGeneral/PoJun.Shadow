using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 失败异常
    /// </summary>
    public class FailException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        public FailException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FailException(string message) : base(message)
        {
        }
    }
}
