using System;

namespace CameraClient.Tests
{
    class AssertException : Exception
    {
        public AssertException(string message)
            :base(message)
        {

        }
    }
}
