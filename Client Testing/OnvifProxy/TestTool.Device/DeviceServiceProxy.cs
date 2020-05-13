using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestTool.Device
{
    class DeviceServiceProxy : ServiceProxy
    {
        public DeviceServiceProxy(string endpointAddress, string deviceServiceAddress)
            :base(endpointAddress, deviceServiceAddress)
        {


        }

        private Dictionary<string, string> _addresses;
        public void SetReplacements(Dictionary<string, string> addresses)
        {
            _addresses = addresses;
        }

        protected override string GetResponseForClient(string response)
        {
            string modifiedResponse = response;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(response);

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
            manager.AddNamespace("onvif", "http://www.onvif.org/ver10/device/wsdl");

            XmlNode node = doc.SelectSingleNode("s:Envelope/s:Body/onvif:GetServicesResponse", manager);

            XmlElement element = node as XmlElement;

            if (element != null)
            {
                foreach (XmlElement service in element.ChildNodes)
                {
                    XmlElement namespaceNode = service.SelectSingleNode("onvif:Namespace", manager) as XmlElement;
                    XmlElement addressNode = service.SelectSingleNode("onvif:XAddr", manager) as XmlElement;
                    
                    if (_addresses.ContainsKey(namespaceNode.InnerText))
                    {
                        addressNode.InnerText = _addresses[namespaceNode.InnerText];
                    }
                }
            }

            MemoryStream ms = new MemoryStream();
            XmlTextWriter tw = new XmlTextWriter(ms, new UTF8Encoding(false));
            doc.Save(tw);
            ms.Seek(0, SeekOrigin.Begin);
            modifiedResponse = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            tw.Close();

            return modifiedResponse;
        }

    }
}
