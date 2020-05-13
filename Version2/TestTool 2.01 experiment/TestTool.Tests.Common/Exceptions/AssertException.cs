///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Common.Exceptions
{
    /// <summary>
    /// Assert exception
    /// </summary>
    public class AssertException : ApplicationException
    {
        public AssertException(string message)
            :base(message)
        {

        }

        public AssertException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
