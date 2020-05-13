///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Definitions.Exceptions
{
    /// <summary>
    /// This exception is thrown to stop test execution.
    /// </summary>
    public class StopEventException : ApplicationException
    {
        public StopEventException(string message)
            :base(message)
        {

        }
         
        public StopEventException()
        {

        }


    }
}
