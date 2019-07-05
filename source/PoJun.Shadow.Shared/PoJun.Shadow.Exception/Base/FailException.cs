using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 失败异常
    /// </summary>
    public class FailException : System.Exception
    {
        public FailException() : base()
        {
        }

        public FailException(string message) : base(message)
        {
        }
    }
}
