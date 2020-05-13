using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService
{
    public class SecurityCheck
    {
        public static void Check(SoapHeaderCollection headers)
        {
            string wsu = ConfigurationSettings.AppSettings["wsu"];
            if (wsu != "true")
            {
                return;
            }
            
            bool authorized = false;
            foreach (SoapHeader header in headers)
            {
                SoapUnknownHeader unknown = header as SoapUnknownHeader;
                if (unknown != null)
                {
                    if (unknown.Element.LocalName == "Security" 
                        && unknown.Element.NamespaceURI == "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")
                    {
                        authorized = true;
                    }
                }
            }

            if (!authorized)
            {
                SoapFaultSubCode subCode = new SoapFaultSubCode(new XmlQualifiedName("SenderNotAuthorized", "http://www.onvif.org/ver10/error"));
                SoapFaultSubCode subCode1 = new SoapFaultSubCode(new XmlQualifiedName("NotAuthorized", "http://www.onvif.org/ver10/error"), subCode);
                throw new SoapException("AccessDenied", new XmlQualifiedName("Sender", "http://www.w3.org/2003/05/soap-envelope"), subCode1);
            }        
        
        }
    }
}
