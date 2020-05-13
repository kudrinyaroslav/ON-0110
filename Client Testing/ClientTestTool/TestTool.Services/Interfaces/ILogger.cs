using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Services
{
    public delegate void TrafficEvent(string log);
    public delegate void MessageEvent(string log);

    public interface ILogger
    {
        event TrafficEvent RequestReceived;
        event TrafficEvent ResponseSent;
    }
}
