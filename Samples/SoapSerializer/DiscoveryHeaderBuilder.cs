using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Soap
{
    class DiscoveryHeaderBuilder : ISoapHeaderBuilder
    {
        private const string _wsa = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        private const string _probeAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe";

        public void WriteHeader(XmlWriter writer, object message)
        {
            writer.WriteElementString("wsa", "MessageID", _wsa, "uuid:" + Guid.NewGuid().ToString());
            writer.WriteElementString("wsa", "To", _wsa, "urn:schemas-xmlsoap-org:ws:2005:04:discovery");
            writer.WriteElementString("wsa", "Action", _wsa, GetAction(message));
        }

        protected string GetAction(object message)
        {
            string action = string.Empty;
            if(message is ProbeType)
            {
                action = _probeAction;
            }
            return action;
        }
    }
}
