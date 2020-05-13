using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Door12
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DoorControlBinding", Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class DoorService : DoorControlBinding
    {
        //TestSuit
        DoorServiceTest DoorServiceTest
        {
            get
            {
                if (Application[Base.AppVars.DOORSERVICE] != null)
                {
                    return (DoorServiceTest)Application[Base.AppVars.DOORSERVICE];
                }
                else
                {
                    DoorServiceTest serviceTest = new DoorServiceTest(TestCommon);
                    Application[Base.AppVars.DOORSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        //GetDoorState response with invalid door mode
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>    <GetDoorStateResponse xmlns=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">      <DoorState>        <DoorMode>Invalid</DoorMode>        <tdc:DoorMonitor xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">Open</tdc:DoorMonitor>       <tdc:DoorLockMonitor xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">Locked</tdc:DoorLockMonitor>        <tdc:DoorTamper xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">NotSupported</tdc:DoorTamper>  <tdc:DoorAlarm xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">Normal</tdc:DoorAlarm></DoorState></GetDoorStateResponse></soap:Body></soap:Envelope>")]
        public override DoorState GetDoorState(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            DoorState result = (DoorState)ExecuteGetCommand(validation, DoorServiceTest.GetDoorStateTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetDoorInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("DoorInfo")] out DoorInfo[] DoorInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }            
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            DoorInfo[] infos = DoorServiceTest.TakeDoorInfoList();
            string result = (string)ExecuteGetCommand(validation, DoorServiceTest.GetDoorInfoListTest);
            DoorInfo = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfo", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public override DoorInfo[] GetDoorInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            DoorInfo[] result = (DoorInfo[])ExecuteGetCommand(validation, DoorServiceTest.GetDoorInfoTest);
            return result;
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            string extendedTimeString = null;
            if (UseExtendedTimeSpecified)
            {
                extendedTimeString = UseExtendedTime.ToString();
            }
            validation.Add(ParameterType.OptionalString, "UseExtendedTime", extendedTimeString);
            validation.Add(ParameterType.String, "AccessTime", AccessTime);
            validation.Add(ParameterType.String, "OpenTooLongTime", OpenTooLongTime);
            validation.Add(ParameterType.String, "PreAlarmTime", PreAlarmTime);
            ExecuteVoidCommand(validation, DoorServiceTest.AccessDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.LockDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void UnlockDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.UnlockDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void BlockDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.BlockDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.LockDownDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownReleaseDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.LockDownReleaseDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.LockOpenDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenReleaseDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.LockOpenReleaseDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DoubleLockDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.DoubleLockDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_DoorControlCapabilitiesIncorrectResponseTag)]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            ServiceCapabilities result = (ServiceCapabilities)ExecuteGetCommand(validation, DoorServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorList", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetDoorList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("Door")] out Door[] Door)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            Door[] infos = DoorServiceTest.TakeDoorList();
            string result = (string)ExecuteGetCommand(validation, DoorServiceTest.GetDoorListTest);
            Door = infos;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoors", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Door")]        
        public override Door[] GetDoors([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            Door[] result = (Door[])ExecuteGetCommand(validation, DoorServiceTest.GetDoorsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/CreateDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateDoor(Door Door)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Door.token", Door.token);
            validation.Add(ParameterType.String, "Door.Name", Door.Name);
            validation.Add(ParameterType.String, "Door.Description", Door.Description);
            validation.Add(ParameterType.String, "Door.DoorType", Door.DoorType);
            validation.Add(ParameterType.String, "Door.Timings.ReleaseTime", Door.Timings.ReleaseTime);
            validation.Add(ParameterType.String, "Door.Timings.OpenTime", Door.Timings.OpenTime);

            string result = (string)ExecuteGetCommand(validation, DoorServiceTest.CreateDoorTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/SetDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetDoor(Door Door)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Door.token", Door.token);
            validation.Add(ParameterType.String, "Door.Name", Door.Name);
            validation.Add(ParameterType.String, "Door.Description", Door.Description);
            validation.Add(ParameterType.String, "Door.DoorType", Door.DoorType);
            validation.Add(ParameterType.String, "Door.Timings.ReleaseTime", Door.Timings.ReleaseTime);
            validation.Add(ParameterType.String, "Door.Timings.OpenTime", Door.Timings.OpenTime);

            ExecuteVoidCommand(validation, DoorServiceTest.SetDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/ModifyDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifyDoor(Door Door)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Door.token", Door.token);
            validation.Add(ParameterType.String, "Door.Name", Door.Name);
            validation.Add(ParameterType.String, "Door.Description", Door.Description);
            validation.Add(ParameterType.String, "Door.DoorType", Door.DoorType);
            validation.Add(ParameterType.String, "Door.Timings.ReleaseTime", Door.Timings.ReleaseTime);
            validation.Add(ParameterType.String, "Door.Timings.OpenTime", Door.Timings.OpenTime);

            ExecuteVoidCommand(validation, DoorServiceTest.ModifyDoorTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/DeleteDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteDoor(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, DoorServiceTest.DeleteDoorTest);
        }
    }
}
