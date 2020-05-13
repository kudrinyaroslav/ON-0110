using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.CameraWebService;
using System.Web.Services.Protocols;
using DUT.CameraWebService.Common;


using System.Web.Services;


namespace DUT.CameraWebService.Base
{

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    [System.Xml.Serialization.XmlRootAttribute("Action", Namespace = "http://www.w3.org/2005/08/addressing", IsNullable = false)]
    public class ActionHeader : SoapHeader
    {
        public ActionHeader()
        {
            actionValue = "http://www.w3.org/2005/08/addressing/soap/fault";
            xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            xmlns.Add("a", "http://www.w3.org/2005/08/addressing");
            MustUnderstand = true;
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string actionValue;

        private System.Xml.Serialization.XmlSerializerNamespaces xmlns;

        [System.Xml.Serialization.XmlNamespaceDeclarations]
        public System.Xml.Serialization.XmlSerializerNamespaces Xmlns
        {
            get { return xmlns; }
            set { xmlns = value; }
        }
    }

    public delegate object GetMethodDelegate(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout);
    public delegate void MethodDelegate(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout);

    public class BaseDutService : System.Web.Services.WebService
    {
        protected TestCommon TestCommon
        {
            get
            {
                if (Application[AppVars.TESTCOMMON] != null)
                {
                    return (TestCommon)Application[AppVars.TESTCOMMON];
                }
                else
                {
                    TestCommon testCommon = new TestCommon();
                    testCommon.LoadTestSuit();
                    Application[AppVars.TESTCOMMON] = testCommon;
                    return testCommon;
                }
            }
        }

        protected void StepTypeProcessing(StepType stepType, SoapException ex, int timeOut)
        {
            switch (stepType)
            {
                case StepType.Normal:
                    {
                        //For Halt testing
                        //System.Threading.Thread.Sleep(5000);
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

        // Receive all SOAP headers.
        public SoapUnknownHeader[] unknownHeaders;
        public ActionHeader actionHeader = new ActionHeader();

        public void SoapHeaderProcessing(SoapUnknownHeader[] unknownHeaders)
        {
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }
        }

        protected object ExecuteGetCommand(ParametersValidation validationRequest, GetMethodDelegate method)
        {
            object res;
            int timeOut;
            SoapException ex;
            StepType stepType;

            res = method(validationRequest, out stepType, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        protected void ExecuteVoidCommand(ParametersValidation validationRequest, MethodDelegate method)
        {
            int timeOut;
            SoapException ex;
            StepType stepType;

            method(validationRequest, out stepType, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

        }
    
    }
}
