///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;

namespace TestTool.Tests.Definitions.Exceptions
{
    /// <summary>
    /// Pre-requisites condition broken.
    /// </summary>
    /// <remarks>Since now TestTool tries to setup device where possible, 
    /// this type of exception is not used.</remarks>
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
