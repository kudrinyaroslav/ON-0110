using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraClient.Tests
{
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
