using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 没有权限
    /// </summary>
    public class NoPermissionException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public NoPermissionException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NoPermissionException(string message) : base(message)
        {
        }
    }
}
