///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Common.Exceptions
{
    public class DutPropertiesException : ApplicationException
    {
        public DutPropertiesException(string message)
            :base(message)
        {

        }

        public DutPropertiesException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
