using System;
using TestTool.Tests.Common.Discovery;
using TestTool.Proxies.Event;

namespace TestTool.Tests.Common.NotificationConsumer
{
    /// <summary>
    /// Class for receiving Notifications from the DUT. 
    /// </summary>
    public class NotificationConsumer : HttpSoapServer
    {
        /// <summary>
        /// Is raised when a notification is received.
        /// </summary>
        public Action<SoapMessage<Notify>> OnNotify;

        public NotificationConsumer(string address)
            : base(address)
        {
        }

        private byte[] _rawData;

        /// <summary>
        /// Raw notification data.
        /// </summary>
        public byte[] RawData
        {
            get { return _rawData; }
        }

        /// <summary>
        /// Data receiving. 
        /// </summary>
        /// <param name="data"></param>
        protected override void OnMessageReceived(byte[] data)
        {
            try
            {
                _rawData = data;
                SoapMessage<Notify> notify = SoapBuilder.ParseMessage<Notify>(data, null);
                if (OnNotify != null)
                {
                    OnNotify(notify);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(ex);
                }
            }
        }
    }
}
