using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.ServiceSchedule10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ScheduleBinding", Namespace = "http://www.onvif.org/ver10/schedule/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class ServiceSchedule : ScheduleServiceBinding
    {
        //TestSuit
        ScheduleServiceTest ScheduleServiceTest
        {
            get
            {
                if (Application[Base.AppVars.SCHEDULESERVICE] != null)
                {
                    return (ScheduleServiceTest)Application[Base.AppVars.SCHEDULESERVICE];
                }
                else
                {
                    ScheduleServiceTest serviceTest = new ScheduleServiceTest(TestCommon);
                    Application[Base.AppVars.SCHEDULESERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //GetServiceCapabilities: MaxLimit is missed
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\"><Capabilities MaxLimit=\"3\" MaxSchedules=\"-1\" MaxTimePeriodsPerDay=\"-10\" MaxSpecialDayGroups=\"-2\" MaxSpecialDaysInSpecialDayGroup=\"-1\" MaxSpecialDaysSchedules=\"1\" ExtendedRecurrenceSupported=\"False\" SpecialDaysSupported=\"True\" StateReportingSupported=\"False\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Sch111_GetSvcCap_MaxLimitSkipped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.Sch111_GetSvcCap_MaxLimitMinusOne)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_AccessRulesCapabilitiesIncorrectResponseTag)]
            
            public override ServiceCapabilities GetServiceCapabilities()
        {
            ParametersValidation validation = new ParametersValidation();
            ServiceCapabilities result = (ServiceCapabilities)ExecuteGetCommand(validation, ScheduleServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleState", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ScheduleState")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ActiveSkipped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ActiveNotBool)]
        public override ScheduleState GetScheduleState(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ScheduleState result = (ScheduleState)ExecuteGetCommand(validation, ScheduleServiceTest.GetScheduleStateTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfo", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")]
        public override ScheduleInfo[] GetScheduleInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            ScheduleInfo[] result = (ScheduleInfo[])ExecuteGetCommand(validation, ScheduleServiceTest.GetScheduleInfoTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleInfoList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetScheduleInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("ScheduleInfo")] out ScheduleInfo[] ScheduleInfo)
        {
            ParametersValidation validation = new ParametersValidation();

            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.String, "StartReference", StartReference);
            ScheduleInfo[] scheduleInfoes = ScheduleServiceTest.TakeScheduleInfoList();
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.GetScheduleInfoListTest);
            ScheduleInfo = scheduleInfoes;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSchedules", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Schedule")]
        public override Schedule[] GetSchedules([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            Schedule[] result = (Schedule[])ExecuteGetCommand(validation, ScheduleServiceTest.GetSchedulesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetScheduleList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetScheduleList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("Schedule")] out Schedule[] Schedule)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);

            Schedule[] schedules = ScheduleServiceTest.TakeScheduleList();            
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.GetScheduleListTest);
            Schedule = schedules;
            
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/CreateSchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateSchedule(Schedule Schedule)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", Schedule.token);
            validation.Add(ParameterType.String, "Name", Schedule.Name);
            validation.Add(ParameterType.String, "Description", Schedule.Description);
            validation.Add(ParameterType.Log, "Standard", Schedule.Standard);
            if ((Schedule.SpecialDays != null) && (Schedule.SpecialDays.Count() != 0))
            {
                validation.Add(ParameterType.String, "SpecialDays/GroupToken", Schedule.SpecialDays[0].GroupToken);
                if ((Schedule.SpecialDays[0].TimeRange != null) && (Schedule.SpecialDays[0].TimeRange.Count() != 0))
                {
                    validation.Add(ParameterType.Log, "SpecialDays/TimeRange/From", Schedule.SpecialDays[0].TimeRange[0].From);
                    if (Schedule.SpecialDays[0].TimeRange[0].UntilSpecified)
                    {
                        validation.Add(ParameterType.Log, "SpecialDays/TimeRange/Until", Schedule.SpecialDays[0].TimeRange[0].Until);
                    }
                }
            }
            
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.CreateScheduleTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/ModifySchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifySchedule(Schedule Schedule)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", Schedule.token);
            validation.Add(ParameterType.String, "Name", Schedule.Name);
            validation.Add(ParameterType.String, "Description", Schedule.Description);
            validation.Add(ParameterType.Log, "Standard", Schedule.Standard);
            if ((Schedule.SpecialDays != null) && (Schedule.SpecialDays.Count() != 0))
            {
                validation.Add(ParameterType.String, "SpecialDays/GroupToken", Schedule.SpecialDays[0].GroupToken);
                if ((Schedule.SpecialDays[0].TimeRange != null) && (Schedule.SpecialDays[0].TimeRange.Count() != 0))
                {
                    validation.Add(ParameterType.Log, "SpecialDays/TimeRange/From", Schedule.SpecialDays[0].TimeRange[0].From);
                    if (Schedule.SpecialDays[0].TimeRange[0].UntilSpecified)
                    {
                        validation.Add(ParameterType.Log, "SpecialDays/TimeRange/Until", Schedule.SpecialDays[0].TimeRange[0].Until);
                    }
                }
            }
            ExecuteVoidCommand(validation, ScheduleServiceTest.ModifyScheduleTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/DeleteSchedule", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteSchedule(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, ScheduleServiceTest.DeleteScheduleTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfo", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")]
        public override SpecialDayGroupInfo[] GetSpecialDayGroupInfo([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            SpecialDayGroupInfo[] result = (SpecialDayGroupInfo[])ExecuteGetCommand(validation, ScheduleServiceTest.GetSpecialDayGroupInfoTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupInfoList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1356_ScheduleFromTicket)] //response from Ticket - not validate unexpected parameter GetSpecialDayGroupInfoListResponse.SpecialDayGroupInfoList[0].Days
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1356_ScheduleFromSimulator)]//response that we getfrom simulator -validate Unexpexted parameter Days
     
        public override string GetSpecialDayGroupInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroupInfo")] out SpecialDayGroupInfo[] SpecialDayGroupInfo)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            SpecialDayGroupInfo[] specialDayInfoes = ScheduleServiceTest.TakeSpecialDayGroupInfoList();
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.GetSpecialDayGroupInfoListTest);
            SpecialDayGroupInfo = specialDayInfoes;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroups", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")]
        public override SpecialDayGroup[] GetSpecialDayGroups([System.Xml.Serialization.XmlElementAttribute("Token")] string[] Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.StringArray, "Token", Token);
            SpecialDayGroup[] result = (SpecialDayGroup[])ExecuteGetCommand(validation, ScheduleServiceTest.GetSpecialDayGroupsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/GetSpecialDayGroupList", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("NextStartReference")]
        public override string GetSpecialDayGroupList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, string StartReference, [System.Xml.Serialization.XmlElementAttribute("SpecialDayGroup")] out SpecialDayGroup[] SpecialDayGroup)
        {
            ParametersValidation validation = new ParametersValidation();
            int? limit = null;
            if (LimitSpecified)
            {
                limit = Limit;
            }
            validation.Add(ParameterType.OptionalInt, "Limit", limit);
            validation.Add(ParameterType.OptionalString, "StartReference", StartReference);
            SpecialDayGroup[] specialDayGroups = ScheduleServiceTest.TakeSpecialDayGroupList();
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.GetSpecialDayGroupListTest);
            SpecialDayGroup = specialDayGroups;
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/CreateSpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Token")]
        public override string CreateSpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", SpecialDayGroup.token);
            validation.Add(ParameterType.String, "Name", SpecialDayGroup.Name);
            validation.Add(ParameterType.String, "Description", SpecialDayGroup.Description);
            validation.Add(ParameterType.Log, "Days", SpecialDayGroup.Days);
            string result = (string)ExecuteGetCommand(validation, ScheduleServiceTest.CreateSpecialDayGroupTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/ModifySpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void ModifySpecialDayGroup(SpecialDayGroup SpecialDayGroup)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "token", SpecialDayGroup.token);
            validation.Add(ParameterType.String, "Name", SpecialDayGroup.Name);
            validation.Add(ParameterType.String, "Description", SpecialDayGroup.Description);
            validation.Add(ParameterType.Log, "Days", SpecialDayGroup.Days);
            ExecuteVoidCommand(validation, ScheduleServiceTest.ModifySpecialDayGroupTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/schedule/wsdl/DeleteSpecialDayGroup", RequestNamespace = "http://www.onvif.org/ver10/schedule/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/schedule/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteSpecialDayGroup(string Token)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "Token", Token);
            ExecuteVoidCommand(validation, ScheduleServiceTest.DeleteSpecialDayGroupTest);
        }
    }
}
