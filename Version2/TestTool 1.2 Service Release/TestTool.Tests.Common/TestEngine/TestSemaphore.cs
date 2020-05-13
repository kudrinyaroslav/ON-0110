///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Threading;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Class which allows stopping waiting for service answer.
    /// </summary>
    public class TestSemaphore : HttpTransport.IExecutionController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TestSemaphore()
        {
            _stopEvent = new AutoResetEvent(false);
        }

        private AutoResetEvent _stopEvent;

        /// <summary>
        /// Method for setting "Stop" event.
        /// </summary>
        public void Stop()
        {
            _stopEvent.Set();
        }

        /// <summary>
        /// Event to be monitored
        /// </summary>
        public WaitHandle StopEvent
        {
            get { return _stopEvent; }
        }

        /// <summary>
        /// Method to be called when an executing code handles "Stop" event setting.
        /// </summary>
        public void ReportStop()
        {
            System.Diagnostics.Debug.WriteLine("TestSemaphore.ReportStop()");
            throw new TestTool.Tests.Common.Exceptions.StopEventException();   
        }

    }
}
