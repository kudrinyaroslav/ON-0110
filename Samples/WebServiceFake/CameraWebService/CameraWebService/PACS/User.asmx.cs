using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PACS.UserService;

namespace CameraWebService.PACS
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class User : UserService
    {


        public override UserInfo[] GetUserInfoList(string[] TokenList)
        {
            throw new NotImplementedException();
        }

        public override UserInfo[] GetUserInfoByCredentialToken(string CredentialToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/User/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/User/wsdl", ResponseNamespace = "http://www.onvif.org/v3/User/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            return new ServiceCapabilities();
        }
    }
}
