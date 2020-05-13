using System;
using System.Xml.Serialization;

namespace TestTool.Transport.Security
{
    public class UsernameToken
    {
        public string Username;
        public Password Password;
        public string Nonce;
        [XmlElement(ElementName = "Created", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
        public DateTime Created;
    }

    public class Password
    {
        [XmlText]
        public string Value;

        [XmlAttribute]
        public string Type;

    }

    [XmlRoot(Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", ElementName = "Security")]
    public class UsernameTokenHeader
    {
        public const string NAME = "Security";
        public const string NAMESPACE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        [XmlElement(ElementName = "UsernameToken", Namespace = NAMESPACE)]
        public UsernameToken UsernameToken;

    }
}
