using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using TestTool.Tests.Common.Discovery;
using TestTool.Proxies.Event;

namespace TestTool.Tests.Common.NotificationConsumer
{
    public class NotificationConsumer : HttpSoapServer
    {
        public Action<SoapMessage<Notify>> OnNotify;

        public NotificationConsumer(string address) : base (address)
        {
        }

        private byte[] _rawData;

        public byte[] RawData
        {
            get { return _rawData; }
        }

        protected override void  OnMessageReceived(byte[] data)
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
                if(OnError != null)
                {
                    OnError(ex);
                }
            }
        }
    }
}
