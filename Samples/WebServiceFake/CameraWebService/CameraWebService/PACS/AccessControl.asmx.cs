using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PACS.AccessControl;

namespace CameraWebService.PACS
{
    /// <summary>
    /// Summary description for AccessControl
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AccessControl : PACSService
    {


        public override AreaInfo[] GetAreaInfoList(string[] TokenList)
        {
            throw new NotImplementedException();
        }

        public override AccessPointInfo[] GetAccessPointInfoList(string[] TokenList)
        {
            throw new NotImplementedException();
        }

        public override AccessControllerInfo[] GetAccessControllerInfoList(string[] TokenList)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            return new ServiceCapabilities();
        }

        public override CredentialInfo[] GetCredentialInfoList(string[] TokenList, int Limit, bool LimitSpecified, int Offset, bool OffsetSpecified, string OrderBy)
        {
            throw new NotImplementedException();
        }
    }
}
