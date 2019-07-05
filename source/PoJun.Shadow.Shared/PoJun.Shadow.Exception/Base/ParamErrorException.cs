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
        public ParamErrorException() : base()
        {
        }

        public ParamErrorException(string message) : base(message)
        {
        }
    }
}
