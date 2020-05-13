using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace CameraWebService.Events
{
    public class WsaHeader : SoapHeader
    {
        public WsaHeader()
        {
            xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            xmlns.Add("a", "http://www.w3.org/2005/08/addressing");
            MustUnderstand = true;
        }

        private System.Xml.Serialization.XmlSerializerNamespaces xmlns;

        [System.Xml.Serialization.XmlNamespaceDeclarations]
        public System.Xml.Serialization.XmlSerializerNamespaces Xmlns
        {
            get { return xmlns; }
            set { xmlns = value; }
        }
        
    }

    public class WsaTextHeader : WsaHeader
    {
        private string _value;

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("Action", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class ActionHeader : WsaTextHeader
    {

    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("To", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class WsaToHeader : WsaTextHeader
    {

    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("RelatesTo", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class RelatesToHeader : WsaTextHeader
    {
        
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("MessageID", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class MessageIdHeader : WsaTextHeader
    {
        // sample: "uuid:6B29FC40-CA47-1067-B31D-00DD010662DA"
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("ReplyTo", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class ReplyToHeader : WsaHeader
    {
        private string _value;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "Address", Namespace = "http://www.w3.org/2005/08/addressing")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

}
