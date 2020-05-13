using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Door11
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DoorControlBinding", Namespace = "http://www.onvif.org/ver10/doorcontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class DoorService : Door11ServiceBinding
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        DoorServiceTest m_DoorServiceTest = null;

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

            if (Application["m_Door11ServiceTest"] != null)
            {
                m_DoorServiceTest = (DoorServiceTest)Application["m_Door11ServiceTest"];
            }
            else
            {
                m_DoorServiceTest = new DoorServiceTest(m_TestCommon);
                Application["m_Door11ServiceTest"] = m_DoorServiceTest;
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

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorState")]
        public override DoorState GetDoorState(string Token)
        {
            TestSuitInit();
            DoorState res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetDoorStateTest(out res, out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfo", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("DoorInfo")]
        public override DoorInfo GetDoorInfo(string Token)
        {
            TestSuitInit();
            DoorInfo res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetDoorInfoTest(out res, out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.AccessDoorTest(out ex, out timeOut, Token, UseExtendedTime, UseExtendedTimeSpecified, AccessTime, OpenTooLongTime, PreAlarmTime, Extension);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void UnlockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.UnlockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void BlockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.BlockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDownDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownReleaseDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDownReleaseDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockOpenDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenReleaseDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockOpenReleaseDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DoubleLockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.DoubleLockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            TestSuitInit();
            ServiceCapabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override DoorInfo[] GetDoorInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified)
        {
            TestSuitInit();
            DoorInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetDoorInfoListTest(out res, out ex, out timeOut, Limit, LimitSpecified, Offset, OffsetSpecified);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/doorcontrol/wsdl/GetDoorInfoListByTokenList", RequestNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/doorcontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override DoorInfo[] GetDoorInfoListByTokenList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            DoorInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetDoorInfoListByTokenListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
    }
}
