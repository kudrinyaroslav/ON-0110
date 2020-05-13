using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DUT.PACS.Simulator.Events10
{
    public class Utils
    {
        public static XmlElement GetFilterElements(string rawRequest)
        {
            return GetFilterElements(rawRequest, false);
        }

        public static XmlElement GetFilterElements(string rawRequest, bool pullpoint)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawRequest);

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");
            manager.AddNamespace("tt", "http://www.onvif.org/ver10/events/wsdl");

            string filterPath; 
            if (pullpoint)
            {
                filterPath = "/s:Envelope/s:Body/tt:CreatePullPointSubscription/tt:Filter/b2:TopicExpression";
            }
            else
            {
                filterPath = "/s:Envelope/s:Body/b2:Subscribe/b2:Filter/b2:TopicExpression";
            }
            XmlNodeList nodes = doc.SelectNodes(filterPath, manager);

            XmlElement filterExpression = null;
            foreach (XmlNode node in nodes)
            {
                filterExpression = node as XmlElement;
                if (filterExpression != null)
                {
                    break;
                }
            }

            return filterExpression;
        }
    }
}
