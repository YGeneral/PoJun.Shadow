using System;
using System.Collections.Generic;
using System.Text;

namespace PoJun.Shadow.Exception
{
    /// <summary>
    /// 【重复提交】异常
    /// </summary>
    public class RepeatSubmitException : System.Exception
    {
        public RepeatSubmitException() : base()
        {
        }

        public RepeatSubmitException(string message) : base(message)
        {
        }
    }
}
