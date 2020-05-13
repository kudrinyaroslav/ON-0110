///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Tests.Common.Trace
{
    /// <summary>
    /// Object to track requests and responses.
    /// </summary>
    public class TrafficListener : TestTool.HttpTransport.Interfaces.ITrafficListener
    {
        /// <summary>
        /// Raised after a request is sent.
        /// </summary>
        public event Action<string> RequestSent;
        /// <summary>
        /// Raised when a response is received.
        /// </summary>
        public event Action<string> ResponseReceived;

        #region ITrafficListener Members

        public void LogRequest(string data)
        {
            if (RequestSent != null)
            {
                RequestSent(data);
            }
        }

        public void LogResponse(string data)
        {
            if (ResponseReceived != null)
            {
                ResponseReceived(data);
            }
        }
        
        #endregion
    }
}
