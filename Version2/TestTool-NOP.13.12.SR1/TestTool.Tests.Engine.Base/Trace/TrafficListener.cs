///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.IO;
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

        public event Action<int> MessageProcessingTime;

        //[24.05.2013] AKS: Added logging of processing times for all requests except below.
        static public string[] untrackedRequests = new[] { "PullMessages", "GetEventSearchResults", "GetMetadataSearchResults", "GetPTZPositionSearchResults", "GetRecordingSearchResults" };


        DateTime RequestDate;
        static List<int> Timings = new List<int>();
        static void AddTime(int ms)
        {
            lock (Timings)
            {
                if (Timings == null)
                {
                    Timings = new List<int>();
                }
                Timings.Add(ms);
            }
        }
        static public void ResetStatistics()
        {
            lock (Timings)
            {
                Timings.Clear();
            }
        }
        static public void GetAggregated(out int Max, out int Median, out int Mean)
        {
            lock (Timings)
            {
                if (Timings.Count <= 0)
                {
                    Max = 0;
                    Median = 0;
                    Mean = 0;
                    return;
                }

                var array = Timings.ToArray();

                Array.Sort(array);

                Max = array.Last();

                Mean = Convert.ToInt32(array.Sum(e => (long)e) / array.Count());
                Median = array[array.Count() / 2];
            }
        }

        #region ITrafficListener Members

        private string _request;
        public void LogRequest(string data)
        {
            RequestDate = DateTime.Now;
            _request = data;
            if (RequestSent != null)
            {
                RequestSent(data);
            }
        }

        public void LogResponse(string data)
        {
            int LastDelayMs = Convert.ToInt32((DateTime.Now - RequestDate).TotalMilliseconds);

            if (ResponseReceived != null)
            {
                ResponseReceived(data);
            }
            if (MessageProcessingTime != null)
            {
                MessageProcessingTime(LastDelayMs);

                if (0 != LastDelayMs && !string.IsNullOrEmpty(_request) && !untrackedRequests.Any(_request.Contains))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("MessageTime = {0} ms, used", LastDelayMs));

                    AddTime(LastDelayMs);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("MessageTime = {0} ms, NOT used", LastDelayMs));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("MessageTime = {0} ms, NOT used (no filter)", LastDelayMs));
                //AddTime(LastDelayMs);
            }
        }
        
        #endregion
    }
}
