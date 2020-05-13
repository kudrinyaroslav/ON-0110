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

namespace DUT.CameraWebService.Recording10
{



    /// <summary>
    /// Class for Search Service tests
    /// </summary>
    public class RecordingServiceTest : Base.BaseServiceTest
    {

        #region Const

        protected override string ServiceName { get { return "Recording10"; } }

        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int GetRecordings = 0;
        private const int GetServiceCapabilities = 1;
        private const int CreateRecording = 2;
        private const int SetRecordingConfiguration = 3; 
        private const int DeleteRecording = 4;
        private const int GetRecordingConfiguration = 5;
        private const int CreateTrack = 6;
        private const int DeleteTrack = 7;
        private const int GetTrackConfiguration = 8;
        private const int SetTrackConfiguration = 9;
        private const int CreateRecordingJob = 10;
        private const int DeleteRecordingJob = 11;
        private const int GetRecordingJobs = 12;
        private const int SetRecordingJobConfiguration = 13;
        private const int GetRecordingJobConfiguration = 14;
        private const int SetRecordingJobMode = 15;
        private const int GetRecordingJobState = 16;
        private const int GetRecordingOptions = 17;
        
        private const int MaxCommands = 18;


        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public RecordingServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        //***************************************************************************************

        internal StepType GetRecordingsTest(out GetRecordingsResponseItem[] target, out SoapException ex, out int Timeout)
        {
            StepType res = StepType.None;
            Timeout = 0;
            ex = null;
            target = null;
            bool passed = true;
            string logMessage = "";

            string tmpCommandName = "GetRecordings";
            int tmpCommandNumber = GetRecordings;

            //Get step list for command
            XmlNodeList m_testList = TestCommon.GetStepsForCommand(ServiceName + "." + tmpCommandName);

            if (m_testList.Count != 0)
            {
                //Get current step
                XmlNode test = m_testList[CommandCount[tmpCommandNumber]];

                //Generate response
                object targetObj;
                res = TestCommon.GenerateResponseStepTypeNotVoid(test, out targetObj, out ex, out Timeout, typeof(GetRecordingsResponseItem[]));
                target = (GetRecordingsResponseItem[])targetObj;

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
        
        
        internal GetRecordingsResponseItem[] GetRecordingsTest(ParametersValidation validation, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<GetRecordingsResponseItem[]>("GetRecordings", GetRecordings, validation, out stepType, out ex, out Timeout);
            
        }

        internal Capabilities GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<Capabilities>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateRecordingTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("CreateRecording", CreateRecording, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetRecordingConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetRecordingConfiguration", SetRecordingConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteRecordingTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteRecording", DeleteRecording, validationRequest, out stepType, out ex, out Timeout);
        }

        internal RecordingConfiguration GetRecordingConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingConfiguration>("GetRecordingConfiguration", GetRecordingConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal RecordingOptions GetRecordingOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingOptions>("GetRecordingOptions", GetRecordingOptions, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateTrackTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("CreateTrack", CreateTrack, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void DeleteTrackTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteTrack", DeleteTrack, validationRequest, out stepType, out ex, out Timeout);
        }

        internal TrackConfiguration GetTrackConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<TrackConfiguration>("GetTrackConfiguration", GetTrackConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetTrackConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetTrackConfiguration", SetTrackConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal string CreateRecordingJobTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<string>("CreateRecordingJob", CreateRecordingJob, validationRequest, out stepType, out ex, out Timeout);
        }

        internal RecordingJobConfiguration TakeRecordingJobConfiguration()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("TakeSpecialParameter for CreateRecordingJob [{0}]", CommandCount[CreateRecordingJob]));
            return TakeSpecialParameter<RecordingJobConfiguration>("CreateRecordingJob", CreateRecordingJob, "RecordingJobConfiguration");
        }

        internal void DeleteRecordingJobTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("DeleteRecordingJob", DeleteRecordingJob, validationRequest, out stepType, out ex, out Timeout);
        }

        internal GetRecordingJobsResponseItem[] GetRecordingJobsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<GetRecordingJobsResponseItem[]>("GetRecordingJobs", GetRecordingJobs, validationRequest, out stepType, out ex, out Timeout);
        }
        
        internal void SetRecordingJobConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetRecordingJobConfiguration", SetRecordingJobConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal RecordingJobConfiguration GetRecordingJobConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingJobConfiguration>("GetRecordingJobConfiguration", GetRecordingJobConfiguration, validationRequest, out stepType, out ex, out Timeout);
        }

        internal void SetRecordingJobModeTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            VoidCommand("SetRecordingJobMode", SetRecordingJobMode, validationRequest, out stepType, out ex, out Timeout);
        }

        internal RecordingJobStateInformation GetRecordingJobStateTest(ParametersValidation validationRequest, out StepType stepType, out SoapException ex, out int Timeout)
        {
            return GetCommand<RecordingJobStateInformation>("GetRecordingJobState", GetRecordingJobState, validationRequest, out stepType, out ex, out Timeout);
        }


    
    
    }
}
