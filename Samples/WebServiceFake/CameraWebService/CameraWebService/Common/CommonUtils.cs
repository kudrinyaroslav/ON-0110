using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService
{
    public class CommonUtils
    {
        public static void ReturnFault(string[] codes)
        {
            SoapFaultSubCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                SoapFaultSubCode currentSubCode = new SoapFaultSubCode(new XmlQualifiedName(codes[i], "http://www.onvif.org/ver10/error"), subCode);
                subCode = currentSubCode;
            }
            throw new SoapException("Error", new XmlQualifiedName(codes[0], "http://www.w3.org/2003/05/soap-envelope"), subCode);
        }
    }
}
