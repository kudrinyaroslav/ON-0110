using System;

namespace HttpTransport
{
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
