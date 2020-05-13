///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.HttpTransport.Exceptions
{
    /// <summary>
    /// Is raised when incoming HTTP-packet is incorrect.
    /// </summary>
    public class HttpProtocolException : ApplicationException
    {
        public HttpProtocolException ()
            :base()
        {

        }

        public HttpProtocolException(string message)
            : base(message)
        {

        }

    }
}
