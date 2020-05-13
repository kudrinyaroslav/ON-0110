using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestTool.HttpTransport;

namespace HttpClient
{
    class TrafficListener : ITrafficListener, IExecutionController
    {
        public TrafficListener()
        {
            _stopEvent = new ManualResetEvent(false);
        }

        public event Action<string> OnRequest;
        public event Action<string> OnResponse;
        
        #region ITrafficListener Members

        public void LogRequest(string data)
        {
            if (OnRequest != null)
            {
                OnRequest(data + Environment.NewLine);
            }
        }

        public void LogResponse(string data)
        {
            if (OnResponse != null)
            {
                OnResponse(data + Environment.NewLine);
            }
        }

        private ManualResetEvent _stopEvent;

        #endregion

        #region ITrafficListener Members

        public void Stop()
        {
            _stopEvent.Set();
        }

        public void Reset()
        {
            _stopEvent.Reset();
        }

        public WaitHandle StopEvent
        {
            get { return _stopEvent; }
        }

        public void ReportStop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
