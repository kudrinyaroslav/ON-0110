using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using DUT.CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Search10
{
    /// <summary>
    /// Summary description for MediaService
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://www.onvif.org/ver10/search/wsdl")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "SearchBinding", Namespace = "http://www.onvif.org/ver10/search/wsdl")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FilterType))]
    public class SearchService : Search10ServiceBinding
    {
        public void TestSuitInit()
        {

        }

        SearchServiceTest SearchServiceTest
        {
            get
            {
                if (Application[Base.AppVars.SEARCHSERVICE] != null)
                {
                    return (SearchServiceTest)Application[Base.AppVars.SEARCHSERVICE];
                }
                else
                {
                    SearchServiceTest serviceTest = new SearchServiceTest(TestCommon);
                    Application[Base.AppVars.SEARCHSERVICE] = serviceTest;
                    return serviceTest;
                }
            }
        }


        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        //[XmlReplySubstituteExtension(DUT.CameraWebService.Common.ResponsesConst.ResponseTicket1405_SearchCapabilitiesIncorrectResponseTag)]
        public override Capabilities GetServiceCapabilities()
        {
            Capabilities res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetServiceCapabilitiesTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingSummary", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Summary")]
        public override RecordingSummary GetRecordingSummary()
        {
            TestSuitInit();
            RecordingSummary res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetRecordingSummaryTest(out res, out ex, out timeOut);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingInformation", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingInformation")]
        public override RecordingInformation GetRecordingInformation(string RecordingToken)
        {
            TestSuitInit();
            RecordingInformation res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetRecordingInformationTest(out res, out ex, out timeOut, RecordingToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetMediaAttributes", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("MediaAttributes")]
        public override MediaAttributes[] GetMediaAttributes([System.Xml.Serialization.XmlElementAttribute("RecordingTokens")] string[] RecordingTokens, System.DateTime Time)
        {
            TestSuitInit();
            MediaAttributes[] res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetMediaAttributesTest(out res, out ex, out timeOut, RecordingTokens, Time);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindRecordings", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindRecordings(SearchScope Scope, int MaxMatches, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxMatchesSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string KeepAliveTime)
        {
            TestSuitInit();
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.FindRecordingsTest(out res, out ex, out timeOut, Scope, MaxMatches, MaxMatchesSpecified, KeepAliveTime);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindRecordingResultList GetRecordingSearchResults(string SearchToken, int MinResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MinResultsSpecified, int MaxResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxResultsSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string WaitTime)
        {
            TestSuitInit();
            FindRecordingResultList res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetRecordingSearchResultsTest(out res, out ex, out timeOut, SearchToken, MinResults, MinResultsSpecified, MaxResults, MaxResultsSpecified, WaitTime);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindEvents", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindEvents(System.DateTime StartPoint, System.DateTime EndPoint, [System.Xml.Serialization.XmlIgnoreAttribute()] bool EndPointSpecified, SearchScope Scope, EventFilter SearchFilter, bool IncludeStartState, int MaxMatches, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxMatchesSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string KeepAliveTime)
        {
            TestSuitInit();
            string res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.FindEventsTest(out res, out ex, out timeOut, StartPoint, EndPoint, EndPointSpecified, Scope, SearchFilter, IncludeStartState, MaxMatches, MaxMatchesSpecified, KeepAliveTime);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetEventSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[XmlReplySubstituteExtension("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetEventSearchResultsResponse xmlns=\"http://www.onvif.org/ver10/search/wsdl\"><tse:ResultList xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\"><tt:SearchState>Completed</tt:SearchState><tt:Result><tt:RecordingToken>cam1idx1</tt:RecordingToken><tt:TrackToken>VIDEO001</tt:TrackToken><tt:Time>2012-05-28T07:44:50Z</tt:Time><tt:Event><wsnt:SubscriptionReference><wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Recording/State</wsnt:Topic><wsnt:Message><tt:Message UtcTime=\"2012-05-28T07:44:50.000\" PropertyOperation=\"Changed\"><tt:Source><tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem></tt:Source><tt:Data><tt:SimpleItem Name=\"IsRecording\" Value=\"TRUE\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event><tt:StartStateEvent>false</tt:StartStateEvent></tt:Result></tse:ResultList></GetEventSearchResultsResponse></soap:Body></soap:Envelope>")]
        //Ticket #441
        //[XmlReplySubstituteExtension(Stringonst.Test3)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindEventResultList GetEventSearchResults(string SearchToken, int MinResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MinResultsSpecified, int MaxResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxResultsSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string WaitTime)
        {
            TestSuitInit();
            FindEventResultList res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.GetEventSearchResultsTest(out res, out ex, out timeOut, SearchToken, MinResults, MinResultsSpecified, MaxResults, MaxResultsSpecified, WaitTime);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindPTZPosition", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindPTZPosition(DateTime StartPoint, DateTime EndPoint, [System.Xml.Serialization.XmlIgnoreAttribute()] bool EndPointSpecified, SearchScope Scope, PTZPositionFilter SearchFilter, int MaxMatches, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxMatchesSpecified, string KeepAliveTime)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "StartPoint", XmlConvert.ToString(StartPoint, XmlDateTimeSerializationMode.Utc));
            string endPoint = null;
            if (EndPointSpecified)
            { 
                endPoint = XmlConvert.ToString(EndPoint, XmlDateTimeSerializationMode.Utc);
            }
            validation.Add(ParameterType.OptionalString, "EndPoint", endPoint);
            if (Scope.IncludedRecordings != null)
            {
                if (Scope.IncludedRecordings.Length > 0)
                {
                    validation.Add(ParameterType.String, "IncludedRecordings", "IncludedRecordings", Scope.IncludedRecordings[0]);
                }
            }
            validation.Add(ParameterType.String, "KeepAliveTime", KeepAliveTime);
            int? maxMatches = null;
            if (MaxMatchesSpecified)
            {
                maxMatches = MaxMatches;
            }
            validation.Add(ParameterType.OptionalInt, "MaxMatches", maxMatches);
                        
            string result = (string)ExecuteGetCommand(validation, SearchServiceTest.FindPtzPositionsTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetPTZPositionSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindPTZPositionResultList GetPTZPositionSearchResults(string SearchToken, int MinResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MinResultsSpecified, int MaxResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxResultsSpecified, string WaitTime)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "SearchToken", SearchToken);
            int? minResults = null;
            if (MinResultsSpecified)
            {
                minResults = MinResults;
            }
            validation.Add(ParameterType.OptionalInt, "MinResults", minResults);
            int? maxResults = null;
            if (MaxResultsSpecified)
            {
                maxResults = MaxResults;
            }
            validation.Add(ParameterType.OptionalInt, "MaxResults", maxResults);
            validation.Add(ParameterType.String, "WaitTime", WaitTime);

            FindPTZPositionResultList result = (FindPTZPositionResultList)ExecuteGetCommand(validation, SearchServiceTest.GetPTZPositionSearchResultsTest);
            return result;
        }

        public override SearchState GetSearchState(string SearchToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/EndSearch", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Endpoint")]
        public override DateTime EndSearch(string SearchToken)
        {
            TestSuitInit();
            DateTime res;
            int timeOut;
            SoapException ex;

            StepType stepType = SearchServiceTest.EndSearchTest(out res, out ex, out timeOut, SearchToken);
            StepTypeProcessing(stepType, ex, timeOut);

            return res;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindMetadata", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindMetadata(DateTime StartPoint, DateTime EndPoint, [System.Xml.Serialization.XmlIgnoreAttribute()] bool EndPointSpecified, SearchScope Scope, MetadataFilter MetadataFilter, int MaxMatches, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxMatchesSpecified, string KeepAliveTime)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "StartPoint", XmlConvert.ToString(StartPoint, XmlDateTimeSerializationMode.Utc));
            validation.Add(ParameterType.String, "EndPoint", XmlConvert.ToString(EndPoint, XmlDateTimeSerializationMode.Utc));
            validation.Add(ParameterType.String, "IncludedRecordings", Scope.IncludedRecordings[0]);
            validation.Add(ParameterType.String, "KeepAliveTime", KeepAliveTime);
            int? maxMatches = null;
            if (MaxMatchesSpecified)
            {
                maxMatches = MaxMatches;
            }
            validation.Add(ParameterType.OptionalInt, "MaxMatches", maxMatches);

            string result = (string)ExecuteGetCommand(validation, SearchServiceTest.FindMetadataTest);
            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetMetadataSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindMetadataResultList GetMetadataSearchResults(string SearchToken, int MinResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MinResultsSpecified, int MaxResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxResultsSpecified, string WaitTime)
        {
            ParametersValidation validation = new ParametersValidation();
            validation.Add(ParameterType.String, "SearchToken", SearchToken);
            int? minResults = null;
            if (MinResultsSpecified)
            {
                minResults = MinResults;
            }
            validation.Add(ParameterType.OptionalInt, "MinResults", minResults);
            int? maxResults = null;
            if (MaxResultsSpecified)
            {
                maxResults = MaxResults;
            }
            validation.Add(ParameterType.OptionalInt, "MaxResults", maxResults);
            validation.Add(ParameterType.String, "WaitTime", WaitTime);

            FindMetadataResultList result = (FindMetadataResultList)ExecuteGetCommand(validation, SearchServiceTest.GetMetadataSearchResultsTest);
            return result;
        }
    }
}
