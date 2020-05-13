///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestTool.HttpTransport.Interfaces
{
    /// <summary>
    /// Controller used to affect waiting for answers.
    /// </summary>
    public interface IExecutionController : ITransportController
    {
        /// <summary>
        /// Enevt to listen.
        /// </summary>
        WaitHandle StopEvent { get; }
        /// <summary>
        /// Method for reporting that settings event has been processed.
        /// </summary>
        void ReportStop();
    }
}
