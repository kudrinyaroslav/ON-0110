///////////////////////////////////////////////////////////////////////////
//!  @author        Alexander Ryltsov
////
using System;

namespace TestTool.Tests.Common.Exceptions
{
    /// <summary>
    /// Assert exception
    /// </summary>
    public class VideoException : ApplicationException
    {
        public VideoException(string message)
            :base(message)
        {

        }

        public VideoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
