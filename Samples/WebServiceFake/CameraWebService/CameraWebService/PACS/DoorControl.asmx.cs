using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PACS.DoorControl;

namespace CameraWebService.PACS
{
    /// <summary>
    /// Summary description for DoorControl
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DoorControl : DoorControlService
    {


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        public override DoorState GetDoorState(string Token)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorCapabilities")]
        public override DoorCapabilities GetDoorCapabilities(string Token)
        {
            return new DoorCapabilities();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override DoorInfo[] GetDoorInfoList(string[] TokenList)
        {
            if (TokenList == null || TokenList.Length == 0)
            {
                return Storage.Instance.DoorInfos.ToArray();
            }

            List<DoorInfo> result = new List<DoorInfo>();
            foreach (DoorInfo info in Storage.Instance.DoorInfos)
            {
                if (TokenList.Contains(info.token))
                {
                    result.Add(info);
                }
            }

            return result.ToArray();
        }

        public override void AccessDoor(string Token, bool UseExtendedTime, bool UseExtendedTimeSpecified, string AccessTime, string OpenTooLongTime, string PreAlarmTime, AccessDoorExtension Extension)
        {
            throw new NotImplementedException();
        }

        public override void LockDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void UnlockDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void BlockDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void LockDownDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void LockDownReleaseDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void LockOpenDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void LockOpenReleaseDoor(string Token)
        {
            throw new NotImplementedException();
        }

        public override void DoubleLockDoor(string Token)
        {
            throw new NotImplementedException();
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            return new ServiceCapabilities();
        }
    }
}
