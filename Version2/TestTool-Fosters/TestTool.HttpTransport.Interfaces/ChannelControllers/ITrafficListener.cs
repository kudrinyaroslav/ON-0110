///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Traffic listener.
    /// Since channel is created transparently, it's easier to pass our TrafficListener that to add 
    /// event handlers.
    /// </summary>
    public interface ITrafficListener : ITransportController
    {
        /// <summary>
        /// Is called after rquest is sent.
        /// </summary>
        /// <param name="data">Request.</param>
        void LogRequest(string data);
        /// <summary>
        /// Is called after answer is received.
        /// </summary>
        /// <param name="data">Response.</param>
        void LogResponse(string data);
    }
}
