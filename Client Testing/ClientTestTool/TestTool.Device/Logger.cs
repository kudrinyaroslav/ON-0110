using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Services;
using TestTool.Transport.Interfaces;

namespace TestTool.Device
{
    class Logger : ILogger, ITrafficListener, Transport.ILogger
    {

        #region ILogger Members

        public event TrafficEvent RequestReceived;

        public event TrafficEvent ResponseSent;

        public event MessageEvent EventLogged;

        #endregion

        #region ITrafficListener Members

        public void LogRequest(string data)
        {
            if (RequestReceived != null)
            {
                RequestReceived(data);
            }
        }

        public void LogResponse(string data)
        {
            if (ResponseSent != null)
            {
                ResponseSent(data);
            }
        }

        #endregion

        #region ILogger Members

        public void LogEvent(string message)
        {
            if (EventLogged != null)
            {
                EventLogged(message);
            }
        }

        #endregion
    }
}
