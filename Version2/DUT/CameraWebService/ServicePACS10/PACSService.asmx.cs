using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.PACS10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PACSBinding", Namespace = "http://www.onvif.org/v3/AccessControl/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataEntity))]
    public class PACSService : PACS10ServiceBinding
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

            if (Application["m_PACSServiceTest"] != null)
            {
                m_PACSServiceTest = (PACSServiceTest)Application["m_PACSServiceTest"];
            }
            else
            {
                m_PACSServiceTest = new PACSServiceTest(m_TestCommon);
                Application["m_PACSServiceTest"] = m_PACSServiceTest;
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAreaInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AreaInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AreaInfo[] GetAreaInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            AreaInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAreaInfoListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAccessPointInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessPointInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessPointInfo[] GetAccessPointInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            AccessPointInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAccessPointInfoListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        public override void EnableAccessPoint(string AccessPointToken)
        {
            throw new NotImplementedException();
        }

        public override void DisableAccessPoint(string AccessPointToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/v3/AccessControl/wsdl/GetAccessControllerInfoList", RequestNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", ResponseNamespace = "http://www.onvif.org/v3/AccessControl/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("AccessControllerInfoList")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public override AccessControllerInfo[] GetAccessControllerInfoList([System.Xml.Serialization.XmlArrayItemAttribute("Token", IsNullable = false)] string[] TokenList)
        {
            TestSuitInit();
            AccessControllerInfo[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_PACSServiceTest.GetAccessControllerInfoListTest(out res, out ex, out timeOut, TokenList);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
    }
}
