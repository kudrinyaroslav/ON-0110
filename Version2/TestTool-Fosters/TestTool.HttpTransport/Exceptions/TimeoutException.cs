using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Exceptions
{
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
