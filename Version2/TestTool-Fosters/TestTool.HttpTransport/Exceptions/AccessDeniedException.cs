using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Exceptions
{
    public class AccessDeniedException : ApplicationException
    {
        public AccessDeniedException ()
            :base()
        {

        }

        public AccessDeniedException(string message)
            : base(message)
        {

        }

    }
}
