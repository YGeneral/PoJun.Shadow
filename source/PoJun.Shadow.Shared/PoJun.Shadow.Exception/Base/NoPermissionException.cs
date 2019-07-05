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
        public NoPermissionException() : base()
        {
        }

        public NoPermissionException(string message) : base(message)
        {
        }
    }
}
