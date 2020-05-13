using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Interfaces.Exceptions
{
    /// <summary>
    /// Access denied exception is raised when Digest authentication fails.
    /// </summary>
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
