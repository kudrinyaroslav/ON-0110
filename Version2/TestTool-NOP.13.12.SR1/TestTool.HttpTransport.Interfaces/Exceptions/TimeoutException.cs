using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Interfaces.Exceptions
{
    /// <summary>
    /// Is raised when timeout is expired
    /// </summary>
    public class TimeoutException : Exception
    {
        public TimeoutException ()
            :base()
        {

        }

        public TimeoutException(string message)
            : base(message)
        {

        }
    }
}
