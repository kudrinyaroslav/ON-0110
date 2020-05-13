using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DUT.CameraWebService.Common;
using System.Web.Services.Protocols;

namespace DUT.CameraWebService.Media2SVC
{
    public class Media2SVCServiceTest : Base.BaseServiceTest
    {

        #region Const


        /// <summary>
        /// Constant for Command index
        /// </summary>
        private const int DeleteProfile = 0;
        private const int GetProfiles = 1;
        private const int GetVideoEncoderConfigurations = 2;
        private const int GetVideoEncoderConfigurationOptions = 3;
        private const int GetOSDs = 4;
        private const int GetOSDOptions = 5;
        private const int DeleteOSD = 6;
        private const int CreateOSD = 7;
        private const int GetServiceCapabilities = 8;
        private const int CreateProfile = 9;
        private const int AddConfiguration = 10;
        private const int GetVideoSourceConfigurations = 11;
        private const int GetAudioEncoderConfigurations = 12;
        private const int GetAudioEncoderConfigurationOptions = 13;
        private const int SetAudioEncoderConfiguration = 14;
        private const int SetOSD = 15;
        private const int GetStreamUri = 16;
        private const int GetAudioSourceConfigurations = 17;
        private const int GetSnapshotUri = 18;
        private const int StartMulticastStreaming = 19;
        private const int StopMulticastStreaming = 20;
        private const int SetSynchronizationPoint = 21;
        private const int GetAnalyticsConfigurations = 22;
        private const int GetMetadataConfigurations = 23;
        private const int GetAudioOutputConfigurations = 24;
        private const int GetAudioDecoderConfigurations = 25;
        private const int GetVideoSourceConfigurationOptions = 26;
        private const int GetAudioSourceConfigurationOptions = 27;
        private const int GetMetadataConfigurationOptions = 28;
        private const int GetAudioOutputConfigurationOptions = 29;
        private const int GetAudioDecoderConfigurationOptions = 30;
        private const int GetVideoSourceModes = 31;
        private const int GetVideoEncoderInstances = 32;
        private const int SetAudioSourceConfiguration = 33;
        private const int SetVideoEncoderConfiguration = 34;
        private const int SetVideoSourceConfiguration = 35;
        private const int SetVideoSourceMode = 36;
        private const int SetMetadataConfiguration = 37;
        private const int SetAudioOutputConfiguration = 38;
        private const int SetAudioDecoderConfiguration = 39;
        private const int RemoveConfiguration = 40;
        private const int MaxCommands = 41;        

        #endregion //Const

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Media2SVCServiceTest(TestCommon testCommon)
            : base(testCommon)
        {
            InitCommandsCount(MaxCommands);
        }

        #endregion //Constructors

        protected override string ServiceName
        {
            get { return "Media210"; }
        }

        internal void DeleteProfileTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteProfile", DeleteProfile, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal MediaProfile[] GetProfilesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<MediaProfile[]>("GetProfiles", GetProfiles, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal VideoEncoder2Configuration[] GetVideoEncoderConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<VideoEncoder2Configuration[]>("GetVideoEncoderConfigurations", GetVideoEncoderConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal VideoEncoder2ConfigurationOptions[] GetVideoEncoderConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<VideoEncoder2ConfigurationOptions[]>("GetVideoEncoderConfigurationOptions", GetVideoEncoderConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal OSDConfiguration[] GetOSDsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<OSDConfiguration[]>("GetOSDs", GetOSDs, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal OSDConfigurationOptions GetOSDOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<OSDConfigurationOptions>("GetOSDOptions", GetOSDOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void DeleteOSDTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("DeleteOSD", DeleteOSD, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string CreateOSDTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("CreateOSD", CreateOSD, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal Capabilities2 GetServiceCapabilitiesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<Capabilities2>("GetServiceCapabilities", GetServiceCapabilities, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string CreateProfileTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("CreateProfile", CreateProfile, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void AddConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("AddConfiguration", AddConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal VideoSourceConfiguration[] GetVideoSourceConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<VideoSourceConfiguration[]>("GetVideoSourceConfigurations", GetVideoSourceConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioEncoder2Configuration[] GetAudioEncoderConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioEncoder2Configuration[]>("GetAudioEncoderConfigurations", GetAudioEncoderConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioEncoder2ConfigurationOptions[] GetAudioEncoderConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioEncoder2ConfigurationOptions[]>("GetAudioEncoderConfigurationOptions", GetAudioEncoderConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetAudioEncoderConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetAudioEncoderConfiguration", SetAudioEncoderConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetOSDTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetOSD", SetOSD, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string GetStreamUriTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetStreamUri", GetStreamUri, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioSourceConfiguration[] GetAudioSourceConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioSourceConfiguration[]>("GetAudioSourceConfigurations", GetAudioSourceConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal string GetSnapshotUriTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<string>("GetSnapshotUri", GetSnapshotUri, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void StartMulticastStreamingTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("StartMulticastStreaming", StartMulticastStreaming, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal void StopMulticastStreamingTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("StopMulticastStreaming", StopMulticastStreaming, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal void SetSynchronizationPointTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetSynchronizationPoint", SetSynchronizationPoint, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal VideoAnalyticsConfiguration[] GetAnalyticsConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<VideoAnalyticsConfiguration[]>("GetAnalyticsConfigurations", GetAnalyticsConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal MetadataConfiguration[] GetMetadataConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<MetadataConfiguration[]>("GetMetadataConfigurations", GetMetadataConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }
        internal AudioOutputConfiguration[] GetAudioOutputConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioOutputConfiguration[]>("GetAudioOutputConfigurations", GetAudioOutputConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioDecoderConfiguration[] GetAudioDecoderConfigurationsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioDecoderConfiguration[]>("GetAudioDecoderConfigurations", GetAudioDecoderConfigurations, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal VideoSourceConfigurationOptions GetVideoSourceConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<VideoSourceConfigurationOptions>("GetVideoSourceConfigurationOptions", GetVideoSourceConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioSourceConfigurationOptions GetAudioSourceConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioSourceConfigurationOptions>("GetAudioSourceConfigurationOptions", GetAudioSourceConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal MetadataConfigurationOptions GetMetadataConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<MetadataConfigurationOptions>("GetMetadataConfigurationOptions", GetMetadataConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal AudioOutputConfigurationOptions GetAudioOutputConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<AudioOutputConfigurationOptions>("GetAudioOutputConfigurationOptions", GetAudioOutputConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }
    
        internal AudioEncoder2ConfigurationOptions[] GetAudioDecoderConfigurationOptionsTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
 	        return GetCommand<AudioEncoder2ConfigurationOptions[]>("GetAudioDecoderConfigurationOptions", GetAudioDecoderConfigurationOptions, validationRequest, true, out stepType, out exc, out timeout);
        }
    
        internal VideoSourceMode[] GetVideoSourceModesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
         	return GetCommand<VideoSourceMode[]>("GetVideoSourceModes", GetVideoSourceModes, validationRequest, true, out stepType, out exc, out timeout);
        }
    
        internal EncoderInstanceInfo GetVideoEncoderInstancesTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
         	return GetCommand<EncoderInstanceInfo>("GetVideoEncoderInstances", GetVideoEncoderInstances, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetAudioSourceConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetAudioSourceConfiguration", SetAudioSourceConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetVideoEncoderConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetVideoEncoderConfiguration", SetVideoEncoderConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetVideoSourceConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetVideoSourceConfiguration", SetVideoSourceConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal bool SetVideoSourceModeTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            return GetCommand<bool>("SetVideoSourceMode", SetVideoSourceMode, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetMetadataConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetMetadataConfiguration", SetMetadataConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal  void SetAudioOutputConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetAudioOutputConfiguration", SetAudioOutputConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void SetAudioDecoderConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("SetAudioDecoderConfiguration", SetAudioDecoderConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }

        internal void RemoveConfigurationTest(ParametersValidation validationRequest, out StepType stepType, out SoapException exc, out int timeout)
        {
            VoidCommand("RemoveConfiguration", RemoveConfiguration, validationRequest, true, out stepType, out exc, out timeout);
        }
    }
}