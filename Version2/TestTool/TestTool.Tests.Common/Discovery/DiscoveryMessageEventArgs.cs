///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;

using System.Net;
using TestTool.Tests.Common.Soap;

namespace TestTool.Tests.Common.Discovery
{
    public class DiscoveryErrorEventArgs : EventArgs
    {
        public Exception Exception { get; protected set; }
        public Fault Fault { get; protected set; }

        public DiscoveryErrorEventArgs(Exception exception, Fault fault)
        {
            Fault = fault;
            Exception = exception;
        }
    }
    public class DiscoveryMessageEventArgs : EventArgs
    {
        public Soap.SoapMessage<object> Message { get; protected set; }
        public IPAddress Sender { get; protected set; }

        public DiscoveryMessageEventArgs(SoapMessage<object> message, IPAddress sender)
        {
            Message = message;
            Sender = sender;
        }
    }
}
