using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Events10
{
    /// <summary>
    /// Summary description for SubscriptionManagerService
    /// </summary>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/events/wsdl")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SubscriptionManagerService : SubscriptionManagerBinding
    {

        //TestSuit
        TestCommon m_TestCommon = null;
        EventServiceTest m_EventServiceTest = null;

        // Receive all SOAP headers.
        public SoapUnknownHeader[] unknownHeaders;

        public ActionHeader actionHeader = new ActionHeader();

        private void SoapHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }
        }

        public void TestSuitInit()
        {
            if (Application["m_TestCommon"] != null)
            {
                m_TestCommon = (TestCommon)Application["m_TestCommon"];
            }
            else
            {
                m_TestCommon = new TestCommon();
                m_TestCommon.LoadTestSuit();
                Application["m_TestCommon"] = m_TestCommon;
            }

            if (Application["m_EventServiceTest"] != null)
            {
                m_EventServiceTest = (EventServiceTest)Application["m_EventServiceTest"];
            }
            else
            {
                m_EventServiceTest = new EventServiceTest(m_TestCommon);
                Application["m_EventServiceTest"] = m_EventServiceTest;
            }
        }

        private void StepTypeProcessing(StepType stepType, SoapException ex, int timeOut)
        {
            switch (stepType)
            {
                case StepType.Normal:
                    {
                        break;
                    }
                case StepType.Fault:
                    {
                        throw ex;
                    }
                case StepType.NoResponse:
                    {
                        System.Threading.Thread.Sleep(timeOut);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Renew", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenewResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        public override RenewResponse Renew([System.Xml.Serialization.XmlElementAttribute("Renew", Namespace = "http://docs.oasis-open.org/wsn/b-2")]Renew Renew1)
        {

            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            RenewResponse res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.SMSRenewTest(out res, out ex, out timeOut, Renew1);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/RenewResponse";
            
            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Event_UnsubscribeResponseIncorrectNamespace)]
       // [XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Event_UnsubscribeResponseIncorrectResponseTag)]
        public override UnsubscribeResponse Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://docs.oasis-open.org/wsn/b-2")] Unsubscribe Unsubscribe1)
        {

            SoapHeaderProcessing(unknownHeaders);
            TestSuitInit();
            UnsubscribeResponse res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.SMSUnsubscribeTest(out res, out ex, out timeOut, Unsubscribe1);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse";
            
            return res;
        }

        [WebMethod]
        [SoapHeader("unknownHeaders")]
        [SoapHeader("actionHeader", Direction = SoapHeaderDirection.Out | SoapHeaderDirection.Fault)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", RequestNamespace = "http://www.onvif.org/ver10/events/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/events/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_Event_SetSynchronizationPointResponseIncorrectNamespace)]
        public void SetSynchronizationPoint()
        {
            SoapHeaderProcessing(unknownHeaders);

            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_EventServiceTest.PMSSetSynchronizationPointTest(out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            actionHeader.actionValue = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse";
            
        }
    }
}
