using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Transport
{
    public interface ILogger
    {
        void LogEvent(string message);
    }
}
