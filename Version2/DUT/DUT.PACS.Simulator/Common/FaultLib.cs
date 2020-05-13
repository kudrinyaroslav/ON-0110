using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;

namespace DUT.PACS.Simulator
{
    public enum FaultType
    {
        General,
        Custom
    }


    static public class FaultLib
    {
        static public SoapException GetSoapException(FaultType faultType, string message)
        {
            SoapException res = null;

            switch (faultType)
            {
                case FaultType.General:
                    {
                        System.Xml.XmlQualifiedName code = SoapException.ClientFaultCode;
                        SoapFaultSubCode subCode = null;
                        res = new SoapException(message, code, subCode);
                        break;
                    }
                case FaultType.Custom:
                    {
                        System.Xml.XmlQualifiedName code = SoapException.ClientFaultCode;
                        SoapFaultSubCode subCode = null;
                        res = new SoapException(message, code, subCode);
                        break;
                    }
            }
            return res;
        }

        static public void ReturnFault(string message, string[] codes)
        {
            SoapFaultSubCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                SoapFaultSubCode currentSubCode = new SoapFaultSubCode(new XmlQualifiedName(codes[i], "http://www.onvif.org/ver10/error"), subCode);
                subCode = currentSubCode;
            }
            throw new SoapException(message, new XmlQualifiedName(codes[0], "http://www.w3.org/2003/05/soap-envelope"), subCode);
        }
    }
}
