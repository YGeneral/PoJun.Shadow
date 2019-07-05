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
        public APILogException() : base()
        {
        }

        public APILogException(string message) : base(message)
        {
        }
    }
}
