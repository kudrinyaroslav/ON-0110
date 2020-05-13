using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Door10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class DoorService : Door10ServiceBinding
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

            if (Application["m_DoorServiceTest"] != null)
            {
                m_DoorServiceTest = (DoorServiceTest)Application["m_DoorServiceTest"];
            }
            else
            {
                m_DoorServiceTest = new DoorServiceTest(m_TestCommon);
                Application["m_DoorServiceTest"] = m_DoorServiceTest;
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorState", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetDoorInfoList", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("DoorInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override DoorInfo[] GetDoorInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList, int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified)
        {
            TestSuitInit();
            DoorInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.GetDoorInfoListTest(out res, out ex, out timeOut, TokenList, Limit, LimitSpecified, Offset, OffsetSpecified);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override DoorInfo GetDoorInfo(string Token)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/AccessDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void AccessDoor(string Token, bool UseExtendedTime, [System.Xml.Serialization.XmlIgnoreAttribute()] bool UseExtendedTimeSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string AccessTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string OpenTooLongTime, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string PreAlarmTime, AccessDoorExtension Extension)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.AccessDoorTest(out ex, out timeOut, Token, UseExtendedTime, UseExtendedTimeSpecified, AccessTime, OpenTooLongTime, PreAlarmTime, Extension);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/UnlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void UnlockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.UnlockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/BlockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void BlockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.BlockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDownDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockDownReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockDownReleaseDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockDownReleaseDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockOpenDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/LockOpenReleaseDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void LockOpenReleaseDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.LockOpenReleaseDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/DoubleLockDoor", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DoubleLockDoor(string Token)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_DoorServiceTest.DoubleLockDoorTest(out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/DoorControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/DoorControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
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
    }
}
