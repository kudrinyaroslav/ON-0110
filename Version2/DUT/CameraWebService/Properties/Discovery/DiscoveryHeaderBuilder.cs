///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestTool.Proxies.WSDiscovery;

namespace TestTool.Tests.Common.Discovery
{
    public class DiscoveryHeaderBuilder : ISoapHeaderBuilder
    {
        private const string _wsa = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        private const string _probeAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe";
        private const string _helloAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Hello";

        public string OrigingMessageId { get; set; }

        public void WriteHeader(XmlWriter writer, object message)
        {
            writer.WriteElementString("wsa", "MessageID", _wsa, "uuid:" + Guid.NewGuid().ToString());
            writer.WriteElementString("wsa", "To", _wsa, "urn:schemas-xmlsoap-org:ws:2005:04:discovery");
            writer.WriteElementString("wsa", "Action", _wsa, GetAction(message));
            if (!string.IsNullOrEmpty(OrigingMessageId))
            {
                writer.WriteElementString("wsa", "RelatesTo", _wsa, OrigingMessageId);
            }
        }

        protected string GetAction(object message)
        {
            string action = string.Empty;
            if(message is ProbeType)
            {
                action = _probeAction;
            }
            else if(message is HelloType)
            {
                action = _helloAction;
            }
            return action;
        }
    }
}
