using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.PACS11
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/ver10/accesscontrol/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class PACSService : PACS11Binding
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        PACSServiceTest m_PACSServiceTest = null;

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

            if (Application["m_PACS11ServiceTest"] != null)
            {
                m_PACSServiceTest = (PACSServiceTest)Application["m_PACS11ServiceTest"];
            }
            else
            {
                m_PACSServiceTest = new PACSServiceTest(m_TestCommon);
                Application["m_PACS11ServiceTest"] = m_PACSServiceTest;
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override ServiceCapabilities GetServiceCapabilities()
        {
            TestSuitInit();
            ServiceCapabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/EnableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void EnableAccessPoint(string AccessPointToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.EnableAccessPointTest(out ex, out timeOut, AccessPointToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/DisableAccessPoint", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DisableAccessPoint(string AccessPointToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.DisableAccessPointTest(out ex, out timeOut, AccessPointToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AreaInfo")]
        public override AreaInfo GetAreaInfo(string Token)
        {
            TestSuitInit();
            AreaInfo res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAreaInfoTest(out res, out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AreaInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AreaInfo[] GetAreaInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified)
        {
            TestSuitInit();
            AreaInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAreaInfoListTest(out res, out ex, out timeOut, Limit, LimitSpecified, Offset, OffsetSpecified);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAreaInfoListByTokenList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AreaInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AreaInfo[] GetAreaInfoListByTokenList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            AreaInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAreaInfoListByTokenListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfo", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("AccessPointInfo")]
        public override AccessPointInfo GetAccessPointInfo(string Token)
        {
            TestSuitInit();
            AccessPointInfo res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAccessPointInfoTest(out res, out ex, out timeOut, Token);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessPointInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessPointInfo[] GetAccessPointInfoList(int Limit, [System.Xml.Serialization.XmlIgnoreAttribute()] bool LimitSpecified, int Offset, [System.Xml.Serialization.XmlIgnoreAttribute()] bool OffsetSpecified)
        {
            TestSuitInit();
            AccessPointInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAccessPointInfoListTest(out res, out ex, out timeOut, Limit, LimitSpecified, Offset, OffsetSpecified);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/accesscontrol/wsdl/GetAccessPointInfoListByTokenList", RequestNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/accesscontrol/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessPointInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessPointInfo[] GetAccessPointInfoListByTokenList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            AccessPointInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAccessPointInfoListByTokenListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
    }
}
