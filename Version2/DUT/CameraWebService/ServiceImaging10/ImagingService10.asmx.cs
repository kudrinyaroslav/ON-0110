using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;

namespace DUT.CameraWebService.Imaging10
{
    /// <summary>
    /// Summary description for ImagingService10
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ImagingService10 : Imaging10ServiceBinding
    {
        //TestSuit
        TestCommon m_TestCommon = null;
        ImagingService10Test m_ImagingService10Test = null;

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

            if (Application["m_ImagingService10Test"] != null)
            {
                m_ImagingService10Test = (ImagingService10Test)Application["m_ImagingService10Test"];
            }
            else
            {
                m_ImagingService10Test = new ImagingService10Test(m_TestCommon);
                Application["m_ImagingService10Test"] = m_ImagingService10Test;
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
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/GetImagingSettings", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingSettings")]
        public override Imaging10.ImagingSettings GetImagingSettings([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken)
        {
            TestSuitInit();
            Imaging10.ImagingSettings res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.GetImagingSettingsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/SetImagingSettings", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetImagingSettings([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] Imaging10.ImagingSettings ImagingSettings, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] bool ForcePersistence, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] [System.Xml.Serialization.XmlIgnoreAttribute()] bool ForcePersistenceSpecified)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.SetImagingSettingsTest(out ex, out timeOut, VideoSourceToken, ImagingSettings, ForcePersistence, ForcePersistenceSpecified);
            StepTypeProcessing(stepType, ex, timeOut);

        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/GetOptions", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ImagingOptions")]
        public override Imaging10.ImagingOptions GetOptions([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken)
        {
            TestSuitInit();
            Imaging10.ImagingOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.GetOptionsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/Move", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Move([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] Imaging10.FocusMove Focus)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.MoveTest(out ex, out timeOut, VideoSourceToken, Focus);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/GetMoveOptions", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MoveOptions")]
        public override Imaging10.MoveOptions GetMoveOptions([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken)
        {
            TestSuitInit();
            Imaging10.MoveOptions res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.GetMoveOptionsTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/FocusStop", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void Stop([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken)
        {
            TestSuitInit();
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.StopTest(out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/imaging/wsdl/GetStatus", RequestNamespace = "http://www.onvif.org/ver10/imaging/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/imaging/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Status")]
        public override Imaging10.ImagingStatus GetStatus([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)] string VideoSourceToken)
        {
            TestSuitInit();
            Imaging10.ImagingStatus res;
            int timeOut;
            SoapException ex;

            StepType stepType = m_ImagingService10Test.GetStatusTest(out res, out ex, out timeOut, VideoSourceToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }
       
    }
}
