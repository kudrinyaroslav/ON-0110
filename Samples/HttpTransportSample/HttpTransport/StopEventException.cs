using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpTransport
{
    public class StopEventException : ApplicationException
    {
        public StopEventException ()
            :base()
        {

        }

        public StopEventException(string message)
            : base(message)
        {

        }
    }
}
