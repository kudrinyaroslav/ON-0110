using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using DUT.CameraWebService;
using CameraWebService;
using DUT.CameraWebService.Common;

namespace DUT.CameraWebService.Search10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class SearchServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName { get { return "Search10"; } }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetRecordingInformation = 0;
        private const int FindEvents = 1;
        private const int GetEventSearchResults = 2;
        private const int FindRecordings = 3;
        private const int GetRecordingSearchResults = 4;
        private const int GetRecordingSummary = 5;
        private const int EndSearch = 6;
        private const int GetServiceCapabilities = 7;
        private const int FindMetadata = 8;
        private const int GetMetadataSearchResults = 9;
        private const int FindPTZPosition = 10;
        private const int GetPTZPositionSearchResults = 11;
        private const int GetMediaAttributes = 12;


        private const int MaxCommands = 20;

        #endregion //Const

 
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchServiceTest(TestCommon testCommon)
            :base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors


        //***************************************************************************************

        #region General

        internal StepType GetServiceCapabilitiesTest(out Capabilities target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetServiceCapabilities";
            int tmpCommandNumber = GetServiceCapabilities;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(Capabilities));
                target = (Capabilities)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        // to be used after refactoring
        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        internal StepType GetRecordingInformationTest(out RecordingInformation target, out SoapException ex, out int Timeout, string RecordingToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetRecordingInformation";
            int tmpCommandNumber = GetRecordingInformation;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //RecordingToken
                CommonCompare.StringCompare("RequestParameters/RecordingToken", "RecordingToken", RecordingToken, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(RecordingInformation));
                target = (RecordingInformation)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        // to be used after refactoring
        internal RecordingInformation GetRecordingInformationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingInformation>("GetRecordingInformation", GetRecordingInformation, validationRequest, out stepType, out ex, out Timeout);
        }

        internal StepType GetRecordingSummaryTest(out RecordingSummary target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetRecordingSummary";
            int tmpCommandNumber = GetRecordingSummary;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(RecordingSummary));
                target = (RecordingSummary)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        // to be used after refactoring
        internal RecordingSummary GetRecordingSummaryTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingSummary>("GetRecordingSummary", GetRecordingSummary, validationRequest, out stepType, out ex, out Timeout);
        }

        internal StepType EndSearchTest(out DateTime target, out SoapException ex, out int Timeout, string SearchToken)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "EndSearch";
            int tmpCommandNumber = EndSearch;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //SearchToken
                CommonCompare.StringCompare("RequestParameters/SearchToken", "SearchToken", SearchToken, ref logMessage, ref passed, test);


                #endregion //Analyze request

                //Generate response
                int useTimeOut = 0;
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeVoid(test, out ex, out Timeout);
                //target = (DateTime)targetObj;
                target = DateTime.UtcNow;

                switch (useTimeOut)
                {
                    case 1:
                        {
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                    case 2:
                        {
                            System.Threading.Thread.Sleep(10000);
                            break;
                        }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        // to be used after refactoring
        internal DateTime EndSearchTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("EndSearch", EndSearch, validationRequest, out stepType, out ex, out Timeout);
            return DateTime.UtcNow;
        }
        
        #endregion //General

        //***************************************************************************************


        #region Events

        internal StepType FindEventsTest(out string target, out SoapException ex, out int Timeout, DateTime StartPoint, DateTime EndPoint, bool EndPointSpecified, SearchScope Scope, EventFilter SearchFilter, bool IncludeStartState, int MaxMatches, bool MaxMatchesSpecified, string KeepAliveTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "FindEvents";
            int tmpCommandNumber = FindEvents;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //KeepAliveTime
                CommonCompare.StringCompare("RequestParameters/KeepAliveTime", "KeepAliveTime", KeepAliveTime, ref logMessage, ref passed, test);

                //IncludedRecordings
                CommonCompare.StringCompare("RequestParameters/IncludedRecordings", "IncludedRecordings", Scope.IncludedRecordings[0], ref logMessage, ref passed, test);

                //StartPoint
                CommonCompare.StringCompare("RequestParameters/StartPoint", "StartPoint", XmlConvert.ToString(StartPoint, XmlDateTimeSerializationMode.Utc), ref logMessage, ref passed, test);

                //EndPoint
                CommonCompare.StringCompare("RequestParameters/EndPoint", "EndPoint", XmlConvert.ToString(EndPoint, XmlDateTimeSerializationMode.Utc), ref logMessage, ref passed, test);

                //IncludeStartState
                CommonCompare.StringCompare("RequestParameters/IncludeStartState", "IncludeStartState", IncludeStartState.ToString(), ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal string FindEventsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("FindEvents", FindEvents, validationRequest, out stepType, out ex, out Timeout);
        }

        internal StepType GetEventSearchResultsTest(out FindEventResultList target, out SoapException ex, out int Timeout, string SearchToken, int MinResults, bool MinResultsSpecified, int MaxResults, bool MaxResultsSpecified, string WaitTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetEventSearchResults";
            int tmpCommandNumber = GetEventSearchResults;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //SearchToken
                CommonCompare.StringCompare("RequestParameters/SearchToken", "SearchToken", SearchToken, ref logMessage, ref passed, test);

                //MinResults
                if (CommonCompare.Exist2("RequestParameters/MinResults", "MinResults", MinResultsSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/MinResults", "MinResults", MinResults, ref logMessage, ref passed, test);
                }

                //MaxResults
                if (CommonCompare.Exist2("RequestParameters/MaxResults", "MaxResults", MaxResultsSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/MaxResults", "MaxResults", MaxResults, ref logMessage, ref passed, test);
                }

                //WaitTime
                CommonCompare.StringCompare("RequestParameters/WaitTime", "WaitTime", WaitTime, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(FindEventResultList));
                target = (FindEventResultList)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal FindEventResultList GetEventSearchResultsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<FindEventResultList>("GetEventSearchResults", GetEventSearchResults, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Events

        //***************************************************************************************

        #region Recordings

        internal StepType FindRecordingsTest(out string target, out SoapException ex, out int Timeout, SearchScope Scope, int MaxMatches, bool MaxMatchesSpecified, string KeepAliveTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "FindRecordings";
            int tmpCommandNumber = FindRecordings;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //KeepAliveTime
                CommonCompare.StringCompare("RequestParameters/KeepAliveTime", "KeepAliveTime", KeepAliveTime, ref logMessage, ref passed, test);

                //MaxMatches
                if (CommonCompare.Exist2("RequestParameters/MaxMatches", "MaxMatches", MaxMatchesSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/MaxMatches", "MaxMatches", MaxMatches, ref logMessage, ref passed, test);
                }

                //RecordingInformationFilter
                CommonCompare.StringCompare("RequestParameters/RecordingInformationFilter", "RecordingInformationFilter", Scope.RecordingInformationFilter, ref logMessage, ref passed, test);
                

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(string));
                target = (string)targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal string FindRecordingsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("FindRecordings", FindRecordings, validationRequest, out stepType, out ex, out Timeout);
        }

        internal StepType GetRecordingSearchResultsTest(out FindRecordingResultList target, out SoapException ex, out int Timeout, string SearchToken, int MinResults, bool MinResultsSpecified, int MaxResults, bool MaxResultsSpecified, string WaitTime)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetRecordingSearchResults";
            int tmpCommandNumber = GetRecordingSearchResults;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //SearchToken
                CommonCompare.StringCompare("RequestParameters/SearchToken", "SearchToken", SearchToken, ref logMessage, ref passed, test);

                //MinResults
                if (CommonCompare.Exist2("RequestParameters/MinResults", "MinResults", MinResultsSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/MinResults", "MinResults", MinResults, ref logMessage, ref passed, test);
                }

                //MaxResults
                if (CommonCompare.Exist2("RequestParameters/MaxResults", "MaxResults", MaxResultsSpecified, ref logMessage, ref passed, test))
                {
                    CommonCompare.IntCompare("RequestParameters/MaxResults", "MaxResults", MaxResults, ref logMessage, ref passed, test);
                }

                //WaitTime
                CommonCompare.StringCompare("RequestParameters/WaitTime", "WaitTime", WaitTime, ref logMessage, ref passed, test);

                #endregion //Analyze request

                //Generate response
                int useTimeOut = 0;
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoidSpecial(test, out targetObj, out ex, out Timeout, typeof(FindRecordingResultList), out useTimeOut);
                target = (FindRecordingResultList)targetObj;

                switch (useTimeOut)
                {
                    case 1:
                        {
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                    case 2:
                        {
                            System.Threading.Thread.Sleep(10000);
                            break;
                        }
                }

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }

        internal FindRecordingResultList GetRecordingSearchResultsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<FindRecordingResultList>("GetRecordingSearchResults", GetRecordingSearchResults, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion //Recordings

        #region Metadata
        
        internal string FindMetadataTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("FindMetadata", FindMetadata, validationRequest, out stepType, out ex, out Timeout);
        }

        internal FindMetadataResultList GetMetadataSearchResultsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<FindMetadataResultList>("GetMetadataSearchResults", GetMetadataSearchResults, validationRequest, out stepType, out ex, out Timeout);
        }

        #endregion

        #region PTZ Position

        internal string FindPtzPositionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("FindPTZPosition", FindPTZPosition, validationRequest, out stepType, out ex, out Timeout);
        }
        
        internal FindPTZPositionResultList GetPTZPositionSearchResultsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<FindPTZPositionResultList>("GetPTZPositionSearchResults", GetPTZPositionSearchResults, validationRequest, out stepType, out ex, out Timeout);
        }
        
        #endregion


        internal StepType GetMediaAttributesTest(out MediaAttributes[] target, out SoapException ex, out int Timeout, string[] RecordingTokens, DateTime Time)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetMediaAttributes";
            int tmpCommandNumber = GetMediaAttributes;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                #region Analyze request

                //TODO

                #endregion //Analyze request

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(MediaAttributes[]));
                target = (MediaAttributes[])targetObj;

                //Log message
                TestCommon.writeToLog(test, logMessage, passed);

                Increment(m_testList.Count, tmpCommandNumber);
            }
            else
            {
                throw new SoapException("NO " + ServiceName + "." + tmpCommandName + " COMMAND IN SCRIPT", SoapException.ServerFaultCode);
            }
            return res;
        }
    }
}
