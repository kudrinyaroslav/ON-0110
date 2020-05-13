using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Recording10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/recording/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "RecordingBinding", Namespace = "http://www.onvif.org/ver10/recording/wsdl")]
    public class RecordingService : RecordingBinding
    {
        RecordingServiceTest RecordingServiceTest
        {
            get
            {
                if (Application[Base.AppVars.RECORDINGSERVICE] != null)
                {
                    return (RecordingServiceTest)Application[Base.AppVars.RECORDINGSERVICE];
                }
                else
                {
                    RecordingServiceTest serviceTest = new RecordingServiceTest(TestCommon);
                    Application[Base.AppVars.RECORDINGSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }

        public void TestSuitInit()
        {

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_RecordingCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            Capabilities result = (Capabilities)ExecuteGetCommand(null, RecordingServiceTest.GetServiceCapabilitiesTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/CreateRecording", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingToken")]
        public override string CreateRecording(RecordingConfiguration RecordingConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            // ToDo : add parameters for validation 
            return (string)ExecuteGetCommand(validation, RecordingServiceTest.CreateRecordingTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/DeleteRecording", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteRecording(string RecordingToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            ExecuteVoidCommand(validation, RecordingServiceTest.DeleteRecordingTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordings", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingItem")]
        public override GetRecordingsResponseItem[] GetRecordings()
        {
            TestSuitInit();
            GetRecordingsResponseItem[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = RecordingServiceTest.GetRecordingsTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
         public override void SetRecordingConfiguration(string RecordingToken, RecordingConfiguration RecordingConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            // ToDo: add parameters from configuration passed
            ExecuteVoidCommand(validation, RecordingServiceTest.SetRecordingConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingConfiguration")]
        public override RecordingConfiguration GetRecordingConfiguration(string RecordingToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            return (RecordingConfiguration)ExecuteGetCommand(validation, RecordingServiceTest.GetRecordingConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/CreateTrack", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TrackToken")]
        public override string CreateTrack(string RecordingToken, TrackConfiguration TrackConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            // ToDo: add parameters from configuration passed
            return (string)ExecuteGetCommand(validation, RecordingServiceTest.CreateTrackTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/DeleteTrack", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteTrack(string RecordingToken, string TrackToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            validation.Add(ParameterType.String, "TrackToken", TrackToken);
            ExecuteVoidCommand(validation, RecordingServiceTest.DeleteTrackTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetTrackConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("TrackConfiguration")]
        public override TrackConfiguration GetTrackConfiguration(string RecordingToken, string TrackToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            validation.Add(ParameterType.String, "TrackToken", TrackToken);
            return (TrackConfiguration)ExecuteGetCommand(validation, RecordingServiceTest.GetTrackConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetTrackConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetTrackConfiguration(string RecordingToken, string TrackToken, TrackConfiguration TrackConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            validation.Add(ParameterType.String, "TrackToken", TrackToken);
            // ToDo: add parameters from configuration
            ExecuteVoidCommand(validation, RecordingServiceTest.SetTrackConfigurationTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/CreateRecordingJob", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobToken")]
        //CreateRecordingJob response with SourceToken.Type
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?> <soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">   <soap:Body>     <CreateRecordingJobResponse xmlns=\"http://www.onvif.org/ver10/recording/wsdl\">       <JobToken>job001</JobToken>       <JobConfiguration>         <RecordingToken xmlns=\"http://www.onvif.org/ver10/schema\">recording001</RecordingToken>         <Mode xmlns=\"http://www.onvif.org/ver10/schema\">Idle</Mode>         <Priority xmlns=\"http://www.onvif.org/ver10/schema\">1</Priority>         <Source xmlns=\"http://www.onvif.org/ver10/schema\">           <SourceToken Type=\"http://www.onvif.org/ver10/schema/Receiver\">             <Token>ReceiverToken001</Token>           </SourceToken>           <Tracks>             <SourceTag />             <Destination />           </Tracks>         </Source>       </JobConfiguration>     </CreateRecordingJobResponse>   </soap:Body> </soap:Envelope>")]
        public override string CreateRecordingJob(ref RecordingJobConfiguration JobConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            JobConfiguration = RecordingServiceTest.TakeRecordingJobConfiguration();
            string token = (string)ExecuteGetCommand(validation, RecordingServiceTest.CreateRecordingJobTest);
            return token;
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/DeleteRecordingJob", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void DeleteRecordingJob(string JobToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "JobToken", JobToken);
            ExecuteVoidCommand(validation, RecordingServiceTest.DeleteRecordingJobTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobs", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobItem")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:trc2=\"http://www.onvif.org/ver10/schema\" xmlns:trc3=\"http://www.w3.org/2005/05/xmlmime\" xmlns:trc4=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:trc5=\"http://www.w3.org/2004/08/xop/include\" xmlns:trc6=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:trc7=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><SOAP-ENV:Body><trc:GetRecordingJobsResponse></trc:GetRecordingJobsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:rc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><rc:GetRecordingJobsResponse/></soap:Body></soap:Envelope>")]
        public override GetRecordingJobsResponseItem[] GetRecordingJobs()
        {
            return (GetRecordingJobsResponseItem[])ExecuteGetCommand(null, RecordingServiceTest.GetRecordingJobsTest);
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingJobConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRecordingJobConfiguration(string JobToken, ref RecordingJobConfiguration JobConfiguration)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "JobToken", JobToken);
            // ToDo: add parameters from configuration
            ExecuteVoidCommand(validation, RecordingServiceTest.SetRecordingJobConfigurationTest);

        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobConfiguration", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("JobConfiguration")]
        public override RecordingJobConfiguration GetRecordingJobConfiguration(string JobToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "JobToken", JobToken);
            return (RecordingJobConfiguration)ExecuteGetCommand(validation, RecordingServiceTest.GetRecordingJobConfigurationTest);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/SetRecordingJobMode", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public override void SetRecordingJobMode(string JobToken, string Mode)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "JobToken", JobToken);
            validation.Add(ParameterType.String, "Mode", Mode);
            ExecuteVoidCommand(validation, RecordingServiceTest.SetRecordingJobModeTest);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingJobState", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("State")]
        public override RecordingJobStateInformation GetRecordingJobState(string JobToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "JobToken", JobToken);
            return (RecordingJobStateInformation)ExecuteGetCommand(validation, RecordingServiceTest.GetRecordingJobStateTest);

        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/recording/wsdl/GetRecordingOptions", RequestNamespace = "http://www.onvif.org/ver10/recording/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/recording/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Options")]
        //Response for 466 ticket
        //[XmlReplySubstituteExtension("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:trc2=\"http://www.onvif.org/ver10/schema\" xmlns:trc3=\"http://www.w3.org/2005/05/xmlmime\" xmlns:trc4=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:trc5=\"http://www.w3.org/2004/08/xop/include\" xmlns:trc6=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:trc7=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><SOAP-ENV:Body><trc:GetRecordingOptionsResponse><trc:Options><trc:Job Spare=\"3\"></trc:Job><trc:Track SpareMetadata=\"0\" SpareAudio=\"1\" SpareVideo=\"0\" SpareTotal=\"1\"></trc:Track></trc:Options></trc:GetRecordingOptionsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>")]
        public override RecordingOptions GetRecordingOptions(string RecordingToken)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "RecordingToken", RecordingToken);
            return (RecordingOptions)ExecuteGetCommand(validation, RecordingServiceTest.GetRecordingOptionsTest);
        }
    }
}
