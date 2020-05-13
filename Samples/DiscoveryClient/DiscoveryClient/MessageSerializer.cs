using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Xml;

namespace DiscoveryClient
{
    public class MessageSerializer
    {
        public static object ParseSoapMessage(string message, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            StringReader sr = new StringReader(message);
            return serializer.Deserialize(sr);
        }
    }
}
