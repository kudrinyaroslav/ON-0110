using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Exceptions;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using Event = TestTool.Proxies.Event;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using System.Threading;
using TestTool.Tests.Common.Trace;
using System.IO;
using TestTool.Proxies.Event;
using TestTool.Tests.TestCases.Utils.Events;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.TestCases.Utils;
using System.Reflection;
using System.Xml.Serialization;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.TestCases.TestSuites.Events;

namespace TestTool.Tests.TestCases.TestSuites.Recording
{
    partial class RecordingControlRecordingTestSuite
    {

        #region Media Client

        MediaClient _mediaClient;
        string _mediaAddress;

        protected MediaClient MediaClient
        {
            get
            {
                if (_mediaClient == null)
                {

                    BeginStep("Get Media service address");
                    _mediaAddress = DeviceClient.GetMediaServiceAddress(Features);
                    LogStepEvent(string.Format("Media service address: {0}", _mediaAddress));
                    if (string.IsNullOrEmpty(_mediaAddress))
                    {
                        throw new AssertException("Media service not supported");
                    }
                    else
                    {
                        if (!_mediaAddress.IsValidUrl())
                        {
                            throw new AssertException("Media service address is invalid");
                        }
                    }

                    StepPassed();

                    Binding binding = CreateBinding(
                        false,
                        new IChannelController[] { new SoapValidator(MediaSchemasSet.GetInstance()) });
                    _mediaClient = new MediaClient(binding, new EndpointAddress(_mediaAddress));
                    AttachSecurity(_mediaClient.Endpoint);

                }

                return _mediaClient;
            }
        }

        protected Profile[] GetProfiles()
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetProfiles(this, client);
        }

        protected Profile GetProfile(string token)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetProfile(this, client, token);
        }

        /// <summary>
        /// Retrieves lists of video encoder configurations compatible with specified profile from DUT
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <returns>Array of video encoder configurations</returns>
        protected VideoEncoderConfiguration[] GetCompatibleVideoEncoderConfigurations(string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetCompatibleVideoEncoderConfigurations(this, client, profile);
        }

        protected VideoSourceConfiguration[] GetCompatibleVideoSourceConfigurations(string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetCompatibleVideoSourceConfigurations(this, client, profile);
        }

        /// <summary>
        /// Adds video encoder configuration to profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        /// <param name="configuration">Token of configuration</param>
        protected void AddVideoEncoderConfiguration(string profile, string configuration)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.AddVideoEncoderConfiguration(this, client, profile, configuration);
        }

        /// <summary>
        /// Removes video encoder configuration from profile
        /// </summary>
        /// <param name="profile">Token of profile</param>
        protected void RemoveVideoEncoderConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveVideoEncoderConfiguration(this, client, profile);
        }

        protected void RemoveVideoSourceConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveVideoSourceConfiguration(this, client, profile);
        }

        protected void RemoveAudioEncoderConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveAudioEncoderConfiguration(this, client, profile);
        }

        protected void RemoveAudioSourceConfiguration(string profile)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.RemoveAudioSourceConfiguration(this, client, profile);
        }

        protected VideoEncoderConfigurationOptions GetVideoEncoderConfigurationOptions(string configuration, string profile)
        {
            MediaClient client = MediaClient;
            return CommonMethodsProvider.GetVideoEncoderConfigurationOptions (this, client, configuration, profile);
        }

        protected void SetVideoEncoderConfiguration(VideoEncoderConfiguration config)
        {
            MediaClient client = MediaClient;
            CommonMethodsProvider.SetVideoEncoderConfiguration(this, client, config, true);
        }

        #endregion

        #region Profile selection

        protected Profile GetProfileForRecordingTest(MediaConfigurationChangeLog changeLog)
        {
            //1.	ONVIF Client will invoke GetServiceCapabilitiesRequest message to get supported encoding list from the DUT.
            //2.	Verify GetServiceCapabilitiesResponse message (Capabilities.Encoding value).

            RecordingServiceCapabilities capabilities = GetServiceCapabilities();
            Assert(capabilities.Encoding != null && capabilities.Encoding.Length > 0,
                "No encodings supportes", 
                "Validate recording capabilities");       
            
            //3.	ONVIF Client will invoke GetProfilesRequest message to get full list of media profiles.
            Profile[] profiles = GetProfiles();

            //Assert(profiles != null && profiles.Length > 0, "No profiles returned", "Check that the DUT returned list of profiles");

            if (profiles != null)
            {
                //4.	Verify GetProfilesResponse message. Find profile with encoding from Capabilities.Encoding list. If such profile is found then skip other steps and use this profile for test.
                foreach (Profile p in profiles)
                {
                    if (p.VideoEncoderConfiguration != null)
                    {
                        string profileEncoding = p.VideoEncoderConfiguration.Encoding.ToString();
                        if (capabilities.Encoding.Contains(profileEncoding))
                        {
                            LogTestEvent(string.Format("Use profile '{0}'{1}", p.token, Environment.NewLine));
                            return p;
                        }
                    }
                }
            }

            //5.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) 
            // to create  new profile.
            //6.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
            // or SOAP 1.2 fault message (Action/MaxNVTProfiles) from the DUT. If CreateProfileResponse 
            // message was received go to the step 7.
            bool deleteProfile = false;
            Profile newProfile = null;
            try
            {
                BeginStep("Create profile");
                newProfile = MediaClient.CreateProfile("TestProfile1", null);
                changeLog.CreatedProfiles.Add(newProfile);
                StepPassed();
            }
            catch (FaultException exc)
            {
                LogFault(exc);
                deleteProfile = true;
                LogStepEvent("Unable to create profile - delete one or select existing for test");
                StepPassed();
            }
            //7.	ONVIF Client will invoke DeleteProfileRequest message (ProfileToken = “Profile2”, 
            // where “Profile2” is token of profile with fixed=”false”) to remove profile. If there are 
            // no profiles with fixed=”false” remove all configurations from one fixed profile, 
            // skip steps 7-10 and use this profile as profile with ProfileToken = “ProfileToken1”. 
            // If there are no profiles skip other steps and fail test.
            //8.	Verify the DeleteProfilesResponse message from the DUT.
            //9.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) to create 
            // new profile.
            //10.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
            // from the DUT.
            if (deleteProfile)
            {
                Assert(profiles != null && profiles.Length > 0, "No profiles returned", "Check if there are any profiles to be deleted or used for test");

                bool nonFixedFound = false;

                foreach (Profile p in profiles)
                {
                    if (!(p.fixedSpecified && p.@fixed))
                    {
                        nonFixedFound = true;
                        changeLog.TrackDeletedProfile(p);
                        CommonMethodsProvider.DeleteProfile(this, MediaClient, p.token);
                        break;
                    }
                }

                if (nonFixedFound)
                {
                    newProfile = CommonMethodsProvider.CreateProfile(this, MediaClient, "testprofileX", null);
                    changeLog.CreatedProfiles.Add(newProfile);
                }
                else
                {
                    Profile profile = profiles[0];
                    Profile backup = Utils.CopyMaker.CreateCopy(profile);
                    changeLog.ModifiedProfiles.Add(backup);
                    CommonMethodsProvider.RemoveAllConfigurations(this, MediaClient, profile);
                    newProfile = profile;
                }            
            }           
            
            //11.	ONVIF Client will invoke GetCompatibleVideoSourceConfigurationsRequest message 
            // (ProfileToken = “ProfileToken1”) to retrieve compatible video source configurations list.
            //12.	Verify the GetCompatibleVideoSourceConfigurationsResponse message from the DUT. 
            // If GetCompatibleVideoSourceConfigurationsResponse message contains empty list skip 
            // other steps (this will means that it is not possible to find or create profile for specified 
            // video codec).

            VideoSourceConfiguration[] compatibleVSC =
                GetCompatibleVideoSourceConfigurations(newProfile.token);

                       
            //13.	ONVIF Client will invoke AddVideoSourceConfigurationRequest message 
            // (ProfileToken = “ProfileToken1”, ConfigurationToken = “VSCToken1”, where “VSCToken1” 
            // is the first video source configuration from GetCompatibleVideoSourceConfigurationsResponse 
            // message) to add video source configuration to profile.
            //14.	Verify the AddVideoSourceConfigurationResponse message from the DUT.

            if (compatibleVSC != null)
            {
                bool jpegSupported = capabilities.Encoding.Contains(VideoEncoding.JPEG.ToString());
                bool mpeg4Supported = capabilities.Encoding.Contains(VideoEncoding.MPEG4.ToString());
                bool h264Supported = capabilities.Encoding.Contains(VideoEncoding.H264.ToString());

                foreach (VideoSourceConfiguration config in compatibleVSC)
                {
                    CommonMethodsProvider.AddVideoSourceConfiguration(
                        this, MediaClient, newProfile.token, config.token);
                
                    //15.	ONVIF Client will invoke GetCompatibleVideoEncoderConfigurationsRequest message 
                    // (ProfileToken = “ProfileToken1”) to retrieve compatible video encoder configurations 
                    // list.
                    //16.	Verify the GetCompatibleVideoEncoderConfigurationsResponse message from the DUT. 
                    // If GetCompatibleVideoEncoderConfigurationsResponse message does not contains video 
                    // encoder configurations repeat steps 13-16 for other video source configuration from 
                    // GetCompatibleVideoSourceConfigurationsResponse message.
                    VideoEncoderConfiguration[] compatible = 
                        this.GetCompatibleVideoEncoderConfigurations(newProfile.token);

                    if (compatible != null)
                    {
                        foreach (VideoEncoderConfiguration vec in compatible)
                        {
                            string vecEncoding = vec.Encoding.ToString();
                            if (capabilities.Encoding.Contains(vecEncoding))
                            {
                                AddVideoEncoderConfiguration(newProfile.token, vec.token);
                                return newProfile;
                            } 
                        }
                        
                        foreach (VideoEncoderConfiguration vec in compatible)
                        {
                            //17.	ONVIF Client will invoke AddVideoEncoderConfigurationRequest message 
                            // (ProfileToken = “ProfileToken1”, ConfigurationToken = “VECToken1”, where “VECToken1” 
                            // is the first video encoder configuration from GetCompatibleVideoEncoderConfigurationsResponse message) 
                            // to add video encoder configuration to profile.
                            AddVideoEncoderConfiguration(newProfile.token, vec.token);
                            
                            //18.	Retrieve supported video encoder configuration options for a media profile by 
                            // invoking GetVideoEncoderConfigurationOptions (media profile token) command. Check 
                            // whether the selected media profile supports the required video codec.

                            VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(null, newProfile.token);
                            bool supportOk = false;

                            VideoEncoderConfiguration vecCopy = Utils.CopyMaker.CreateCopy(vec);

                            if (options.JPEG != null && jpegSupported)
                            {
                                MediaTestUtils.UpdateVideoEncoderConfiguration(vec, VideoEncoding.JPEG, options);
                                supportOk = true;
                            }

                            if (options.MPEG4 != null && mpeg4Supported)
                            {
                                MediaTestUtils.UpdateVideoEncoderConfiguration(vec, VideoEncoding.MPEG4, options);
                                supportOk = true;
                            }

                            if (options.H264 != null && h264Supported)
                            {
                                MediaTestUtils.UpdateVideoEncoderConfiguration(vec, VideoEncoding.H264, options);
                                supportOk = true;
                            }

                            if (supportOk)
                            {
                                changeLog.TrackModifiedConfiguration(vecCopy);
                                SetVideoEncoderConfiguration(vec);
                                return newProfile;
                            }
                            //19.	Repeat steps 17-18 for all video encoder configurations received on step 16 till 
                            // a media profile with the required video codec support is created (previously remove 
                            // video encoder configuration from the profile). If such profile was created skip step 20.
                            //20.	Repeat steps 13-19 for all video source configurations received on step 12 till 
                            // a media profile with the required video codec support is created (previously remove 
                            // video encoder configuration and video source configuration from the profile).
                            //21.	ONVIF Client will invoke SetVideoEncoderConfigurationRequest message to set 
                            // required video codec.
                            //22.	Verify SetVideoEncoderConfigurationResponse from the DUT.
                        }
                        RemoveVideoEncoderConfiguration(newProfile.token);
                    }
                }
            }

            Assert(false,
                "No ready to use profile can be found and no profile can be updated to use one of supported encoders",
                "Check that profile for the test has been prepared");
            return null;

        }

        #endregion

        #region Create recording

        protected string GetRecordingForTest(out RecordingConfiguration replacedConfiguration, out bool recordingCreated)
        {
            string recordingToken = null;
            replacedConfiguration = null;
            recordingCreated = false;
            GetRecordingsResponseItem deletedRecording = null;

            //1. If GetServices is not supported, go to step 3.
            //2. If GetServices supported, get maxRecordings and dynamicRecording from GetServiceCapabilities and go to step 4
            //3. Get dynamicRecording from GetCapabilities.

            bool dynamicRecordingSupported = false;
            int? maxNumberOfRecordings = null;
            
            if (Features.ContainsFeature(Feature.GetServices))
            {
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
                maxNumberOfRecordings = (int)capabilities.MaxRecordings;
            }
            else
            {
                Proxies.Onvif.Capabilities commonCapabilities = DeviceClient.GetCapabilities(null);
                Assert(commonCapabilities != null && commonCapabilities.Extension != null && commonCapabilities.Extension.Recording != null,
                    "Recording capabilities not found",
                    "Check that the DUT returned Recording capabilities");
                dynamicRecordingSupported = commonCapabilities.Extension.Recording.DynamicRecordings;
            }
            
            RecordingConfiguration config = new RecordingConfiguration();
            config.MaximumRetentionTime = _retentionTime;
            config.Content = "Recording from device";
            config.Source = new RecordingSourceInformation();
            config.Source.SourceId = _cameraAddress.Trim();
            config.Source.Name = "CameraName";
            config.Source.Location = "LocationDescription";
            config.Source.Description = "SourceDescription";
            config.Source.Address = _cameraAddress.Trim();


            GetRecordingsResponseItem[] recordings = null;
            bool reconfigure = false;

            //4. If dynamicRecording is supported, go to step 6
            if (dynamicRecordingSupported)
            {
                bool execDelete = true;

                //6. if GetServices is not supported and maxRecording is unknown, go to step 8
                //7. if maxRecordings == total number of recordings, go to step 8.5, else go to step 11.
                //8. Try to create recording with required properties. If the recording is created successsfully, skip other steps and use it for test
                if (!maxNumberOfRecordings.HasValue)
                {
                    try
                    {
                        BeginStep("Create recording");
                        string token = Client.CreateRecording(config);
                        recordingCreated = true;
                        StepPassed();
                        return token;
                    }
                    catch (FaultException exc)
                    {
                        LogFault(exc);
                        StepPassed();
                    }
                }

                //8.5 Call GetRecordings 
                //9. Try to delete first recording. If deletion succeeds, go to step 11
                //10 Try to delete second recording. If there are no second recording, skip other steps and fail the test. If deletion fails, skip other steps and fail the test.
                //11. Try to create recording with required properties. If the recording is created successsfully, skip other steps and use it for test.
                //12. Select recording with track of type video
                //13. Reconfigure this recording
                recordings = GetRecordings();

                if (recordings != null)
                {
                    execDelete = recordings.Length == maxNumberOfRecordings.Value;
                }
                else
                {
                    // if no maxNumberOfRecordings is known, we have already tried to create recording
                    reconfigure = !maxNumberOfRecordings.HasValue;
                    execDelete = false;
                }

                if (!reconfigure)
                { 
                    if (execDelete)
                    {
                        foreach (GetRecordingsResponseItem item in recordings)
                        {
                            try
                            {
                                DeleteRecording(item.RecordingToken);
                                deletedRecording = item;
                                break;
                            }
                            catch (FaultException exc)
                            {
                                //LogFault(exc);
                                StepPassed();
                            }
                        }
                        if (deletedRecording == null)
                        {
                            reconfigure = true;
                        }
                    }
                }

                if (!reconfigure)
                {
                    try
                    {
                        BeginStep("Create recording");
                        string token = Client.CreateRecording(config);
                        recordingCreated = true;
                        StepPassed();
                        return token;
                    }
                    catch (FaultException exc)
                    {
                        LogFault(exc);
                        StepPassed();
                    }               
                }
            }
            else
            {
                //5. Call GetRecordings and go to step 12
                recordings = GetRecordings();

            }

            Assert(recordings != null && recordings.Length > 0,
                   "Recordings list is empty",
                   "Check that recordings list is not empty");
            // reconfigure existing recording


            // select recording with video track
            RecordingConfiguration localReplacedConfiguration = null;
            foreach (GetRecordingsResponseItem recording in recordings)
            {
                if (recording.Tracks != null)
                {
                    foreach (GetTracksResponseItem track in recording.Tracks.Track)
                    {
                        if (track.Configuration != null)
                        {
                            if (track.Configuration.TrackType == TrackType.Video)
                            {
                                recordingToken = recording.RecordingToken;
                                break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(recordingToken))
                {
                    localReplacedConfiguration = recording.Configuration;
                    break;
                }
            }

            Assert(!string.IsNullOrEmpty(recordingToken),
                   "Recording with track of type 'Video' not found",
                   "Check that existing recording can be used for test");

            // set configuration
            SetRecordingConfiguration(recordingToken, config);
            replacedConfiguration = localReplacedConfiguration;

            return recordingToken;
        }

        #endregion

        #region Select recording for track creation

        // A.14: copy of this function is in ...\TestSuites\Recording\RecordingControlEventsTestSuit.utils.cs
        // all changes here should be copied there and vise versa
        protected void GetRecordingForTrackCreation(out string recordingToken, 
                                                                                            out string trackType, 
                                                                                            out bool recordingCreated,
                                                                                            out bool trackDeleted,
                                                                                            out TrackConfiguration deletedTrackConf)
        {
            recordingToken = string.Empty;
            trackType = string.Empty;

            recordingCreated = false;
            trackDeleted = false;

            deletedTrackConf = null;

            // get recordings
            GetRecordingsResponseItem[] recordings = GetRecordings();

            // in case of empty recordings list try to create recording
            if (recordings == null || recordings.Length == 0)
            {
                // check for DynamicRecordings capability
                bool dynamicRecordingSupported = false;
                if (Features.ContainsFeature(Feature.GetServices))
                {
                    RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                    dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
                }
                else
                {
                    TestTool.Proxies.Onvif.Capabilities capabilities = DeviceClient.GetCapabilities(null);

                    Assert(capabilities.Extension != null && capabilities.Extension.Recording != null,
                           "No Recording service capabilities found",
                           "Check if the DUT returned Recording service capabilities");

                    dynamicRecordingSupported = capabilities.Extension.Recording.DynamicRecordings;
                }

                Assert(dynamicRecordingSupported, "Can't create recording because DynamicRecordings isn't supported", 
                                                                                  "Check for DynamicRecordings capability");

                // prepare recording configuration
                RecordingConfiguration conf = new RecordingConfiguration();
                conf.Source = new RecordingSourceInformation();
                conf.Source.Description = "SourceDescription";
                conf.Source.SourceId = "http://localhost/sourceID";
                conf.Source.Location = "LocationDescription";
                conf.Source.Name = "CameraName";
                conf.Source.Address = "http://localhost/address";
                conf.MaximumRetentionTime = "PT0S";
                conf.Content = "Recording from device";

                // create recording
                recordingToken = CreateRecording(conf);

                recordingCreated = true;

                // refresh recordings after creation
                recordings = GetRecordings();
                Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");
            }

            bool spareTotal = false;

            // search for recording with possibility to create track in it
            SearchForSpareTrack(recordings, out recordingToken, out spareTotal, out trackType);

            bool noTracks = true;

            // if all recorings doesn't have SpareTotal > 0
            if (!spareTotal)
            {
                LogTestEvent(string.Format("There is no any spare track in any recording so we need to delete some track ...{0}", Environment.NewLine));

                for (int i = 0; i < recordings.Length; i++)
                {
                    recordingToken = recordings[i].RecordingToken;

                    if (recordings[i].Tracks.Track == null)
                        continue;

                    noTracks = false;

                    for (int j = 0; j < recordings[i].Tracks.Track.Length; j++)
                    {
                        try 
                        {
                            DeleteTrack(recordingToken, recordings[i].Tracks.Track[j].TrackToken);
                            deletedTrackConf = recordings[i].Tracks.Track[j].Configuration;
                            trackDeleted = true;
                            break;
                        }
                        catch (FaultException fault)
                        {
                            if (fault.IsValidOnvifFault("Receiver/Action/CannotDelete"))
                            {
                                StepPassed();
                                continue;
                            }
                        }
                    }

                    if (trackDeleted)
                        break;
                }

                Assert(!noTracks, "There is no any tracks in any recording for deletion", "Check that track for deletion was found");

                Assert(!(!spareTotal && !trackDeleted), "Can't delete any track from any existing recording", "Check that track was deleted successfully");

                // refresh recordings after deletion
                recordings = GetRecordings();
                Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

                // search for recording with possibility to create track in it after DeleteTrack
                SearchForSpareTrack(recordings, out recordingToken, out spareTotal, out trackType);

                Assert(spareTotal, "Can't find recording with spare track after track deletion", "Search for recording with spare track");

            }
        }

        // search for recording with possibility to create track in it
        private void SearchForSpareTrack(GetRecordingsResponseItem[] recordings, 
                                                                      out string recordingToken, 
                                                                      out bool spareTotal, 
                                                                      out string trackType)
        {
            recordingToken = string.Empty;
            trackType = string.Empty;
            spareTotal = false;

             for (int i = 0; i < recordings.Length; i++)
            {
                recordingToken = recordings[i].RecordingToken;
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);

                if (recOptions.Track.SpareTotal > 0)
                {
                    spareTotal = true;

                    if (recOptions.Track.SpareVideo > 0)
                    {
                        trackType = "Video";
                    }
                    else if (recOptions.Track.SpareAudio > 0)
                    {
                        trackType = "Audio";
                    }
                    else if (recOptions.Track.SpareMetadata > 0)
                    {
                        trackType = "Metadata";
                    }
                    else
                    {
                        Assert(!(recOptions.Track.SpareTotal > 0 && recOptions.Track.SpareVideo + recOptions.Track.SpareAudio + recOptions.Track.SpareMetadata == 0),
                               String.Format("There should be any Spare Video, or Audio, or Metadata tracks because Total Spare tracks number is {0} (RecordingToken = {1})", recOptions.Track.SpareTotal, recordingToken),
                               "Check for spare tracks correctness");
                    }

                    break;
                }
            }
        }

        #endregion

        #region A.12 - Selection or Creation of Recording for recording job creation

        // A.12: copy of these annex functions is in ...\TestSuites\Recording\RecordingControlEventsTestSuit.utils.cs
        // all changes here should be copied there and vise versa
        protected void GetRecordingForJobCreation(out string recordingToken, out bool recordingCreated, int jobsNumber)
        {
            recordingToken = string.Empty;
            recordingCreated = false;

            bool recordingFound = false;

            // check for DynamicRecordings capability
            bool dynamicRecordingSupported = false;
            DynamicRecordingsSupported(ref dynamicRecordingSupported);

            // dynamic recording supported
            if (dynamicRecordingSupported)
            {
                // prepare recording configuration
                RecordingConfiguration conf = new RecordingConfiguration();
                conf.Source = new RecordingSourceInformation();
                conf.Source.Description = "SourceDescription";
                conf.Source.SourceId = "http://localhost/sourceID";
                conf.Source.Location = "LocationDescription";
                conf.Source.Name = "CameraName";
                conf.Source.Address = "http://localhost/address";
                conf.MaximumRetentionTime = "PT0S";
                conf.Content = "Recording from device";

                // create recording
                try
                {
                    recordingToken = CreateRecording(conf);
                    recordingCreated = true;
                }
                catch (FaultException e)
                {
                    StepPassed();

                    FindExistingRecording(ref recordingToken, ref recordingFound, ref recordingCreated, jobsNumber);

                    return;
                }
                
                // get recording options
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);

                // if there is 2 spare jobs then finish annex
                if (recOptions.Job.Spare > jobsNumber)
                    return;
                // else delete recording jobs
                else
                {
                    GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                    if (jobsNumber == 0)
                        Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                    // there are no recording jobs for deletion, pass test
                    else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                    {
                        LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        recordingToken = string.Empty;
                        recordingCreated = false;
                        return;
                    }

                    if (recordingJobs != null && recordingJobs.Length != 0)
                    {
                        foreach (var item in recordingJobs)
                        {
                            DeleteRecordingJob(item.JobToken);
                            recOptions = GetRecordingOptions(recordingToken);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingFound = true;
                                break;
                            }
                        }

                        // pass test, if after deletion of all jobs there is no Job.Spare > 1
                        // fail test, if after deletion of all jobs there is no Job.Spare > 0
                        if (!recordingFound)
                        {
                            if (jobsNumber == 1)
                            {
                                LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                    DeleteRecording(recordingToken);

                                recordingToken = string.Empty;
                                recordingCreated = false;
                                return;
                            }

                            if (jobsNumber == 0)
                            {
                                Assert(recordingFound, 
                                    "There wasn't found any recording with possibility to create 1 recording job",
                                    "Analyzing for possibility to create 1 recording job");
                            }
                        }
                    }
                }
            }
            // dynamic recording isn't supported
            else
            {
                FindExistingRecording(ref recordingToken, ref recordingFound, ref recordingCreated, jobsNumber);
            }
        }

        private void DynamicRecordingsSupported(ref bool dynamicRecordingSupported)
        {
            // check for DynamicRecordings capability
            dynamicRecordingSupported = false;
            if (Features.ContainsFeature(Feature.GetServices))
            {
                RecordingServiceCapabilities capabilities = GetServiceCapabilities();
                dynamicRecordingSupported = capabilities.DynamicRecordingsSpecified && capabilities.DynamicRecordings;
            }
            else
            {
                TestTool.Proxies.Onvif.Capabilities capabilities = DeviceClient.GetCapabilities(null);

                Assert(capabilities.Extension != null && capabilities.Extension.Recording != null,
                        "No Recording service capabilities found",
                        "Check if the DUT returned Recording service capabilities");

                dynamicRecordingSupported = capabilities.Extension.Recording.DynamicRecordings;
            }
        }

        private void FindExistingRecording(ref string recordingToken, ref bool recordingFound, ref bool recordingCreated, int jobsNumber)
        {
            GetRecordingsResponseItem[] recordings = GetRecordings();
            Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

            // create recording list with video tracks
            List<GetRecordingsResponseItem> recordingsVideoTrack = new List<GetRecordingsResponseItem>();
            foreach (var rec in recordings)
            {
                if (rec.Tracks.Track != null && rec.Tracks.Track.Length != 0)
                {
                    foreach (var track in rec.Tracks.Track)
                    {
                        if (track.Configuration.TrackType == TrackType.Video)
                            recordingsVideoTrack.Add(rec);
                    } 
                }
            }

            Assert(recordingsVideoTrack != null && recordingsVideoTrack.Count != 0,
                          "There are no recordings with video track. Please, configure recording with video track manually and rerun test",
                          "Search for recordings with video track");

            foreach (var item in recordingsVideoTrack)
            {
                RecordingOptions recOptions = GetRecordingOptions(item.RecordingToken);
                if (recOptions.Job.Spare > jobsNumber)
                {
                    recordingFound = true;
                    recordingToken = item.RecordingToken;
                    break;
                }
            }

            // if recording wasn't found then delete recording jobs
            if (!recordingFound)
            {
                GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                if (jobsNumber == 0)
                    Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                {
                    LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                    if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                        DeleteRecording(recordingToken);

                    recordingToken = string.Empty;
                    recordingCreated = false;
                    return;
                }

                if (recordingJobs != null && recordingJobs.Length != 0)
                {
                    foreach (var job in recordingJobs)
                    {
                        DeleteRecordingJob(job.JobToken);
                        foreach (var rec in recordingsVideoTrack)
                        {
                            RecordingOptions recOptions = GetRecordingOptions(rec.RecordingToken);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingToken = rec.RecordingToken;
                                recordingFound = true;
                                break;
                            }
                        }

                        if (recordingFound)
                            break;
                    }

                    // pass test if after deletion of all jobs there is no Job.Spare > 1
                    // fail test, if after deletion of all jobs there is no Job.Spare > 0
                    if (!recordingFound)
                    {
                        if (jobsNumber == 1)
                        {
                            LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                            if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                DeleteRecording(recordingToken);

                            recordingToken = string.Empty;
                            recordingCreated = false;
                            return;
                        }

                        if (jobsNumber == 0)
                        {
                            Assert(recordingFound,
                                "There wasn't found any recording with possibility to create 1 recording job",
                                "Analyzing for possibility to create 1 recording job");
                        }
                    }
                }
            }
        }

        #endregion

        #region A.13 - Auto Creation of Receiver

        protected void AutoCreationReceiver(string recordingToken, out string jobToken, out RecordingJobConfiguration jobConfiguration)
        {
            jobToken = null;
            jobConfiguration = null;

            var source = new RecordingJobSource { AutoCreateReceiver = true, AutoCreateReceiverSpecified = true };
            jobConfiguration = new RecordingJobConfiguration
            {
                RecordingToken = recordingToken,
                Mode = IDLE,
                Priority = 1,
                Source = new RecordingJobSource[] { source }
            };
            var confPrev = (RecordingJobConfiguration)CopyObject(jobConfiguration);

            try
            {
                jobToken = CreateRecordingJob(ref jobConfiguration);
                Assert(!string.IsNullOrEmpty(jobToken),
                             "Job token wasn't returned",
                             "Check that job token was returned");

                ValidateRecordingJobConfiguration(jobConfiguration, confPrev);

                Receiver[] receivers = GetReceivers();
                Assert(receivers != null, "Receiver list is empty", "Check that receiver list is not empty");

                string receiverToken = null;

                if (jobConfiguration != null &&
                    jobConfiguration.Source != null &&
                    jobConfiguration.Source[0] != null &&
                    jobConfiguration.Source[0].SourceToken != null
                    )
                {
                    receiverToken = jobConfiguration.Source[0].SourceToken.Token;
                }

                Receiver receiver = receivers.FirstOrDefault(rec => rec.Token == receiverToken);
                Assert(receiver != null, string.Format("Receiver (token = '{0}') wasn't found in receiver list", receiverToken),
                                                          string.Format("Check that receiver (token = '{0}') exists in receiver list", receiverToken));
            }
            catch (FaultException e)
            {
                if (e.IsValidOnvifFault("Receiver/Action/MaxReceivers"))
                {
                    LogStepEvent(string.Format("Receiver/Action/MaxReceivers fault has been obtained{0}", Environment.NewLine));
                    LogStepEvent(string.Format("Please, delete receiver manually and rerun test{0}", Environment.NewLine));
                    StepFailed();
                    return;
                }
                throw;
            }
        }

        #endregion

        #region A.15 - Selection or Creation of Recording for recording job creation on a Media profile

        // A.15: copy of these annex functions is in ...\TestSuites\Recording\RecordingControlEventsTestSuit.utils.cs
        // all changes here should be copied there and vise versa
        protected void GetRecordingForJobCreationMediaProfile(out string recordingToken, out string profileToken, 
                                                                                                               out bool recordingCreated, int jobsNumber)
        {
            recordingToken = string.Empty;
            profileToken = string.Empty;
            recordingCreated = false;

            bool recordingFound = false;

            // check for DynamicRecordings capability
            bool dynamicRecordingSupported = false;
            DynamicRecordingsSupported(ref dynamicRecordingSupported);

            // dynamic recording supported
            if (dynamicRecordingSupported)
            {
                // prepare recording configuration
                RecordingConfiguration conf = new RecordingConfiguration();
                conf.Source = new RecordingSourceInformation();
                conf.Source.Description = "SourceDescription";
                conf.Source.SourceId = "http://localhost/sourceID";
                conf.Source.Location = "LocationDescription";
                conf.Source.Name = "CameraName";
                conf.Source.Address = "http://localhost/address";
                conf.MaximumRetentionTime = "PT0S";
                conf.Content = "Recording from device";

                // create recording
                try
                {
                    recordingToken = CreateRecording(conf);
                    recordingCreated = true;
                }
                catch (FaultException e)
                {
                    StepPassed();

                    FindExistingRecording2(ref recordingToken, ref profileToken,
                                                                ref recordingFound, ref recordingCreated, jobsNumber);
                    return;
                }

                // get recording options
                RecordingOptions recOptions = GetRecordingOptions(recordingToken);
                Assert(recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0,
                             "Compatible sources list is empty", "Check that compatible sources list is not empty");


                if (recOptions.Job.Spare > jobsNumber)
                {
                    profileToken = recOptions.Job.CompatibleSources[0];
                    return;
                }
                // else delete recording jobs
                else
                {
                    GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                    if (jobsNumber == 0)
                        Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                    // there are no recording jobs for deletion, pass test
                    else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                    {
                        LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                        if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                            DeleteRecording(recordingToken);

                        recordingToken = string.Empty;
                        profileToken = string.Empty;
                        recordingCreated = false;
                        return;
                    }

                    if (recordingJobs != null && recordingJobs.Length != 0)
                    {
                        foreach (var item in recordingJobs)
                        {
                            DeleteRecordingJob(item.JobToken);
                            recOptions = GetRecordingOptions(recordingToken);
                            Assert(recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0,
                                         "Compatible sources list is empty", "Check that compatible sources list is not empty");

                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                profileToken = recOptions.Job.CompatibleSources[0];
                                recordingFound = true;
                                break;
                            }
                        }

                        // pass test, if after deletion of all jobs there is no Job.Spare > 1
                        // fail test, if after deletion of all jobs there is no Job.Spare > 0
                        if (!recordingFound)
                        {
                            if (jobsNumber == 1)
                            {
                                LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                                if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                    DeleteRecording(recordingToken);

                                recordingToken = string.Empty;
                                profileToken = string.Empty;
                                recordingCreated = false;
                                return;
                            }

                            if (jobsNumber == 0)
                            {
                                Assert(recordingFound,
                                    "There wasn't found any recording with possibility to create 1 recording job",
                                    "Analyzing for possibility to create 1 recording job");
                            }
                        }
                    }
                }
            }
            else
            {
                FindExistingRecording2(ref recordingToken, ref profileToken,
                                                            ref recordingFound, ref recordingCreated, jobsNumber);
            }
         }

        private void FindExistingRecording2(ref string recordingToken, ref string profileToken,
                                                                           ref bool recordingFound, ref bool recordingCreated, int jobsNumber)
        {

            bool emptyCompatibleSources = true;

            GetRecordingsResponseItem[] recordings = GetRecordings();
            Assert(recordings != null, "Recording list is empty", "Check that recording list is not empty");

            // create dictionary with recordings which have not empy compatible sources
            Dictionary<string, string> compatibleSources = new Dictionary<string,string>();
            foreach (var rec in recordings)
            {
                RecordingOptions recOptions = GetRecordingOptions(rec.RecordingToken);
                if (recOptions.Job.CompatibleSources != null && recOptions.Job.CompatibleSources.Length != 0)
                {
                    emptyCompatibleSources = false;
                    compatibleSources.Add(rec.RecordingToken, recOptions.Job.CompatibleSources[0]);
                }
            }

            // fail test if there are no any recording with not empty compatible sources list
            Assert(!emptyCompatibleSources && (compatibleSources.Count != 0),
              string.Format("There wasn't found any recording with not empty compatible sources list{0}", Environment.NewLine),
              "Search for recordings with not empty compatible sources list");

            foreach (var compatibleSource in compatibleSources)
            {
                RecordingOptions recOptions = GetRecordingOptions(compatibleSource.Key);
                if (recOptions.Job.Spare > jobsNumber)
                {
                    recordingFound = true;
                    recordingToken = compatibleSource.Key;
                    profileToken = compatibleSource.Value;
                    break;
                }
            }

            // if recording wasn't found then delete recording jobs
            if (!recordingFound)
            {
                GetRecordingJobsResponseItem[] recordingJobs = GetRecordingJobs();
                if (jobsNumber == 0)
                    Assert(recordingJobs != null && recordingJobs.Length != 0, "Recording job list is empty", "Check that recording job list is not empty");
                else if (jobsNumber == 1 && (recordingJobs == null || recordingJobs.Length == 0))
                {
                    LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                    if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                        DeleteRecording(recordingToken);

                    recordingToken = string.Empty;
                    recordingCreated = false;
                    return;
                }

                if (recordingJobs != null && recordingJobs.Length != 0)
                {
                    foreach (var job in recordingJobs)
                    {
                        DeleteRecordingJob(job.JobToken);
                        foreach (var compatibleSource in compatibleSources)
                        {
                            RecordingOptions recOptions = GetRecordingOptions(compatibleSource.Key);
                            if (recOptions.Job.Spare > jobsNumber)
                            {
                                recordingToken = compatibleSource.Key;
                                profileToken = compatibleSource.Value;
                                recordingFound = true;
                                break;
                            }
                        }

                        if (recordingFound)
                            break;
                    }

                    // pass test, if after deletion of all jobs there is no Job.Spare > 1
                    // fail test, if after deletion of all jobs there is no Job.Spare > 0
                    if (!recordingFound)
                    {
                        if (jobsNumber == 1)
                        {
                            LogTestEvent(string.Format("There wasn't found any recording with possibility to create 2 recording jobs{0}", Environment.NewLine));

                            if (recordingCreated && !string.IsNullOrEmpty(recordingToken))
                                DeleteRecording(recordingToken);

                            recordingToken = string.Empty;
                            profileToken = string.Empty;
                            recordingCreated = false;
                            return;
                        }

                        if (jobsNumber == 0)
                        {
                            Assert(recordingFound,
                                "There wasn't found any recording with possibility to create 1 recording job",
                                "Analyzing for possibility to create 1 recording job");
                        }
                    }
                }
            }
        }

        #endregion

        #region Recording Job

        protected void CheckReceiversMaxCount()
        {
            //13.	ONVIF Client will invoke GetServiceCapabilitiesRequest message to retrieve service 
            // capabilities of the DUT.
            //14.	Verify the GetServiceCapabilitiesResponse message (Capabilities.SupportedReceivers) 
            // from the DUT.

            ReceiverServiceCapabilities receiverServiceCapabilities = GetReceiverServiceCapabilities();

            int supportedReceivers = receiverServiceCapabilities.SupportedReceivers;

            Assert(supportedReceivers > 0,
                "SupportedReceivers=0",
                "Check that the DUT supports Receivers capability");

            //15.	ONVIF Client will invoke GetReceiversRequest message to retrieve complete receivers list.
            //16.	Verify the GetReceiversResponse message from the DUT.
            //17.	If number of Receivers from GetReceiversResponse message is less than 
            // specified in Capabilities.SupportedReceivers then skip steps 18-19 and go to step 20
            //18.	ONVIF Client will invoke DeleteReceiverRequest message (ReceiverToken=Token1) 
            // to delete Receiver.
            //19.	Verify the DeleteReceiverResponse message from the DUT.

            Receiver[] receivers = GetReceivers();

            if (receivers != null && receivers.Length == supportedReceivers)
            {
                LogTestEvent("Delete one receiver to proceed with the test" + Environment.NewLine);

                DeleteReceiver(receivers[0].Token);
            }
        }

        protected string CreateRecordingJobForTest(ref RecordingJobConfiguration config,
            out System.DateTime T1,
            bool local)
        {
            string jobToken = null;
            string recordingToken = config.RecordingToken;
            string sourceToken = null;
            if (config != null && config.Source != null && config.Source.Length > 0
                && config.Source[0].SourceToken != null)
                sourceToken = config.Source[0].SourceToken.Token;
            int expectedPriority = config.Priority;

            T1 = System.DateTime.MinValue;

            //Annex 10
            //1.	ONVIF Client will invoke CreateRecordingJobRequest message 
            // (JobConfiguration.RecordingToken=”RecordingToken1”, JobConfiguration.Mode=”Idle”, 
            // JobConfiguration.Priority=1, no JobConfiguration.Source.SourceToken.Token, 
            // JobConfiguration.Source.SourceToken.Type=”http://www.onvif.org/ver10/schema/Receiver”, 
            // JobConfiguration.Source.AutoCreateReceiver=true) to create a recording job and 
            // auto create receiver.
            //2.	Verify the CreateRecordingJobResponse message (JobToken=”JobToken1”) or SOAP 
            // 1.2 fault message (Action/MaxRecordingJobs). If CreateRecordingJobResponse message 
            // was received then go to the step 9.


            //3.	ONVIF Client will invoke GetRecordingJobsRequest message to retrieve complete 
            // recording jobs list.
            //4.	Verify the GetRecordingJobsResponse message from the DUT.
            //11.	ONVIF Client will invoke DeleteRecordingJobRequest message (JobToken as Token 
            // of the first job in the GetRecordingJobsResponse message) to delete Recording Job.
            //5.	Verify the DeleteRecordingJobResponse message from the DUT. Go to the step 4.


            bool retry = false;
            try
            {
                BeginStep("Create Recording Job");
                jobToken = Client.CreateRecordingJob(ref config);
                T1 = System.DateTime.Now;
                DoRequestDelay();
                StepPassed();
            }
            catch (FaultException exc)
            {
                LogFault(exc);
                string err;
                if (exc.IsValidOnvifFault("Receiver/Action/MaxRecordingJobs", out err))
                {
                    LogStepEvent("Max number of recording jobs exceeded. Delete one job and try again");
                    StepPassed();
                    retry = true;
                }
                else
                {
                    throw exc;
                }
            }

            if (retry)
            {
                GetRecordingJobsResponseItem[] jobs = GetRecordingJobs();

                Assert(jobs != null && jobs.Length > 0, "No jobs to delete", "Check if there are any jobs to delete");

                DeleteRecordingJob(jobs[0].JobToken);

                jobToken = CreateRecordingJob(ref config);
                T1 = System.DateTime.UtcNow;
            }

            {
                StringBuilder logger = new StringBuilder("Recording job configuration validation failed:" + Environment.NewLine);
                bool ok = local ?
                    ValidateRecordingJob(config, logger, recordingToken, sourceToken, ACTIVE, expectedPriority, PROFILESOURCETYPE) :
                    ValidateRecordingJob(config, logger, recordingToken, sourceToken, IDLE, expectedPriority, RECEIVERSOURCETYPE);

                Assert(ok, logger.ToStringTrimNewLine(), "Validate recording job configuration");
            }
                     
            if (!local)
            {
                
                Receiver[] receivers = GetReceivers();

                Assert(receivers != null && receivers.Length > 0,
                    "No receivers returned",
                    "Check that the DUT returned Receivers");

                CheckAutoCreatedReceiver(receivers, config.Source[0].SourceToken.Token);

            }

            return jobToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="recordingToken"></param>
        /// <param name="expectedMode"></param>
        /// <returns></returns>
        /// <remarks> Bound to specification. </remarks>
        bool ValidateRecordingJobBase(RecordingJobConfiguration config,
            StringBuilder logger, string recordingToken, string expectedMode, int expectedPriority)
        {
            bool ok = true;

            if (config.RecordingToken != recordingToken)
            {
                ok = false;
                logger.AppendLine(string.Format("   RecordingToken is incorrect: expected '{0}', actual '{1}'", recordingToken, config.RecordingToken));
            }

            if (config.Mode != expectedMode)
            {
                ok = false;
                logger.AppendLine(string.Format("   Mode is incorrect: expected '{0}', actual '{1}'", expectedMode, config.Mode));
            }

            if (config.Priority != expectedPriority)
            {
                ok = false;
                logger.AppendLine(string.Format("   Priority is incorrect: expected '{0}', actual '{1}'", expectedPriority, config.Priority));
            }

            return ok;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="recordingToken"></param>
        /// <param name="expectedMode"></param>
        /// <param name="sourceInfo"></param>
        /// <returns></returns>
        /// <remarks> Bound to specification. </remarks>
        bool ValidateRecordingJob(RecordingJobConfiguration config,
            StringBuilder logger, string recordingToken, string sourceToken, string expectedMode, int expectedPriority, string sourceType)
        {
            bool ok = ValidateRecordingJobBase(config, logger, recordingToken, expectedMode, expectedPriority);

            if (config.Source == null || config.Source.Length == 0)
            {
                ok = false;
                logger.AppendLine("   Source information is missing");
            }
            else
            {
                if (config.Source.Length > 1)
                {
                    ok = false;
                    logger.AppendLine("   More than one sources found");
                }
                else
                {
                    RecordingJobSource source = config.Source[0];
                    // validate source
                    // should contain automatically created receiver ?

                    if (source.SourceToken != null)
                    {
                        if (source.SourceToken.Type != sourceType)
                        {
                            ok = false;
                            logger.AppendLine(string.Format("   SourceToken.Type is incorrect: expected {0}, actual {1}", sourceType, source.SourceToken.Type));
                        }
                        if (string.IsNullOrEmpty(source.SourceToken.Token))
                        {
                            ok = false;
                            logger.AppendLine("   Source.SourceToken.Token is empty");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sourceToken))
                            {
                                if (source.SourceToken.Token != sourceToken)
                                {
                                    ok = false;
                                    logger.AppendLine(string.Format("   Source.SourceToken.Token is {0} but must be {1}",
                                        source.SourceToken.Token, sourceToken));
                                }
                            }
                        }
                    }
                    else
                    {
                        ok = false;
                        logger.AppendLine("   SourceToken not found in Source");
                    }
                }
            }
            return ok;
        }


        #endregion

        private void SetNewVideoEncoderConfiguration(VideoEncoderConfiguration conf, VideoEncoderConfigurationOptions options)
        {
            conf.Quality = conf.Quality == options.QualityRange.Min ? options.QualityRange.Max :
                options.QualityRange.Min;
            switch (conf.Encoding)
            {
                case VideoEncoding.H264 :
                    if (options.H264.ResolutionsAvailable.Length > 1)
                    {
                        conf.Resolution = (conf.Resolution.Height == options.H264.ResolutionsAvailable[0].Height &&
                            conf.Resolution.Width == options.H264.ResolutionsAvailable[0].Width) ?
                            options.H264.ResolutionsAvailable[1] : options.H264.ResolutionsAvailable[0];
                    }
                    conf.RateControl.FrameRateLimit = conf.RateControl.FrameRateLimit == options.H264.FrameRateRange.Max ?
                        options.H264.FrameRateRange.Min : options.H264.FrameRateRange.Max;

                    if (options.Extension != null && options.Extension.H264 != null)
                    {
                        conf.RateControl.BitrateLimit = conf.RateControl.BitrateLimit == options.Extension.H264.BitrateRange.Max ?
                            options.Extension.H264.BitrateRange.Min : options.Extension.H264.BitrateRange.Max;
                    }
                    break;
                case VideoEncoding.JPEG:
                    if (options.JPEG.ResolutionsAvailable.Length > 1)
                    {
                        conf.Resolution = (conf.Resolution.Height == options.JPEG.ResolutionsAvailable[0].Height &&
                            conf.Resolution.Width == options.JPEG.ResolutionsAvailable[0].Width) ?
                            options.JPEG.ResolutionsAvailable[1] : options.JPEG.ResolutionsAvailable[0];
                    }
                    conf.RateControl.FrameRateLimit = conf.RateControl.FrameRateLimit == options.JPEG.FrameRateRange.Max ?
                        options.JPEG.FrameRateRange.Min : options.JPEG.FrameRateRange.Max;

                    if (options.Extension != null && options.Extension.JPEG != null)
                    {
                        conf.RateControl.BitrateLimit = conf.RateControl.BitrateLimit == options.Extension.JPEG.BitrateRange.Max ?
                            options.Extension.JPEG.BitrateRange.Min : options.Extension.JPEG.BitrateRange.Max;
                    }
                    break;

                case VideoEncoding.MPEG4:
                    if (options.MPEG4.ResolutionsAvailable.Length > 1)
                    {
                        conf.Resolution = (conf.Resolution.Height == options.MPEG4.ResolutionsAvailable[0].Height &&
                            conf.Resolution.Width == options.MPEG4.ResolutionsAvailable[0].Width) ?
                            options.MPEG4.ResolutionsAvailable[1] : options.MPEG4.ResolutionsAvailable[0];
                    }
                    conf.RateControl.FrameRateLimit = conf.RateControl.FrameRateLimit == options.MPEG4.FrameRateRange.Max ?
                        options.MPEG4.FrameRateRange.Min : options.MPEG4.FrameRateRange.Max;

                    if (options.Extension != null && options.Extension.H264 != null)
                    {
                        conf.RateControl.BitrateLimit = conf.RateControl.BitrateLimit == options.Extension.MPEG4.BitrateRange.Max ?
                            options.Extension.MPEG4.BitrateRange.Min : options.Extension.MPEG4.BitrateRange.Max;
                    }
                    break;
            }
        }

        void CompareConfigurations(RecordingConfiguration configuration1, RecordingConfiguration configuration2)
        {
            BeginStep("Compare Recording Configurations");
            StringBuilder dump = new StringBuilder("Configurations are different" + Environment.NewLine);
            bool equal = StorageTestsUtils.CompareConfigurations(configuration1, configuration2, dump, "GetRecordings", "CreateRecording");

            if (!equal)
            {
                throw new AssertException(dump.ToStringTrimNewLine());
            }

            StepPassed();
        }

        protected void CheckAutoCreatedReceiver(Receiver[] receivers, string receiverToken)
        {
            BeginStep("Check that Receiver has been created with proper parameters");
            {
                StringBuilder logger = new StringBuilder();
                bool receiverOk = true;

                Receiver newReceiver = receivers.Where(R => R.Token == receiverToken).FirstOrDefault();

                if (newReceiver != null)
                {
                    receiverOk = CheckAutoCreatedReceiver(newReceiver, logger);
                }
                else
                {
                    receiverOk = false;
                    logger.AppendLine(string.Format("Receiver with token '{0}' not found", receiverToken));
                }

                if (!receiverOk)
                {
                    throw new AssertException(logger.ToStringTrimNewLine());
                }
            }

            StepPassed();
        }

        protected void CheckAutoCreatedReceiver(Receiver newReceiver, string receiverToken)
        {
            BeginStep("Check that Receiver has been created with proper parameters");

            StringBuilder logger = new StringBuilder();

            bool receiverOk = CheckAutoCreatedReceiver(newReceiver, logger);

            if (!receiverOk)
            {
                throw new AssertException(logger.ToStringTrimNewLine());
            }
            StepPassed();
        }

        bool CheckAutoCreatedReceiver(Receiver newReceiver, StringBuilder logger)
        {
            bool receiverOk = true;



            return receiverOk;
        }

        void CheckReceiverConfigurationApplied(ReceiverConfiguration config, Receiver receiver)
        {
            BeginStep("Check that configuration has been updated");

            StringBuilder logger = new StringBuilder();
            bool configOk = true;

            ReceiverConfiguration actualConfig = receiver.Configuration;

            if (config.MediaUri != actualConfig.MediaUri)
            {
                configOk = false;
                logger.AppendLine(string.Format("MediaURI has not been applied: expected '{0}', actulal '{1}'",
                    config.MediaUri, actualConfig.MediaUri));
            }
            if (config.Mode != actualConfig.Mode)
            {
                configOk = false;
                logger.AppendLine(string.Format("Mode has not been applied: expected '{0}', actulal '{1}'",
                    config.Mode, actualConfig.Mode));
            }
            if (config.StreamSetup.Stream != actualConfig.StreamSetup.Stream)
            {
                configOk = false;
                logger.AppendLine(string.Format("StreamSetup.Stream has not been applied: expected '{0}', actulal '{1}'",
                    config.StreamSetup.Stream, actualConfig.StreamSetup.Stream));
            }
            if (config.StreamSetup.Transport != null && actualConfig.StreamSetup.Transport != null)
            {
                if (config.StreamSetup.Transport.Protocol != actualConfig.StreamSetup.Transport.Protocol)
                {
                    configOk = false;
                    logger.AppendLine(string.Format("StreamSetup.Transport.Protocol has not been applied: expected '{0}', actulal '{1}'",
                        config.StreamSetup.Transport.Protocol, actualConfig.StreamSetup.Transport.Protocol));
                }

                if (actualConfig.StreamSetup.Transport.Tunnel != null)
                {
                    configOk = false;
                    logger.AppendLine("StreamSetup.Transport.Tunnel was not specified, but found in actual configuration");
                }
            }
            else
            {
                if (actualConfig.StreamSetup.Transport == null)
                {
                    configOk = false;
                    logger.AppendLine("StreamSetup.Transport is missing in actual configuration");
                }
            }

            if (!configOk)
            {
                throw new AssertException(logger.ToStringTrimNewLine());
            }
            StepPassed();

        }



        #region Events

        protected void AttachAddressing(ServiceEndpoint endpoint,
            Proxies.Event.EndpointReferenceType endpointReference)
        {
            if (endpointReference.ReferenceParameters != null && endpointReference.ReferenceParameters.Any != null)
            {
                EndpointReferenceBehaviour behaviour = new EndpointReferenceBehaviour(endpointReference);
                endpoint.Behaviors.Add(behaviour);
            }

        }

        private Binding CreateEventServiceBinding(string address)
        {
            IChannelController[] controllers;
            // add mandatory controllers.
            // _trafficListener is used to monitor data sent and received via Client.
            // _semaphore is used to stop waiting for the answer.

            EndpointController controller = new EndpointController(new EndpointAddress(address));

            WsaController wsaController = new WsaController();

            controllers = new IChannelController[]
                              {
                                  _trafficListener, 
                                  _semaphore, 
                                  _credentialsProvider, 
                                  controller, 
                                  wsaController,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };

            Binding binding = CreateBinding(controllers);

            return binding;

        }

        protected string GetEventServiceAddress()
        {
            BeginStep("Connect to Event service");
            string eventServiceAddress = DeviceClient.GetEventServiceAddress(Features);
            LogStepEvent(string.Format("Event service address: {0}", eventServiceAddress));
            if (!eventServiceAddress.IsValidUrl())
            {
                throw new AssertException("Event service address is invalid");
            }
            StepPassed();
            return eventServiceAddress;
        }

        protected Event.EndpointReferenceType CreatePullPointSubscription(TopicInfo topicInfo,
            out System.DateTime subscribed)
        {
            // Get event service address
            string eventServiceAddress = GetEventServiceAddress();

            // create EventPortTypeClient 
            Binding eventServiceBinding = CreateEventServiceBinding(eventServiceAddress);

            Event.EventPortTypeClient eventPortTypeClient = new Event.EventPortTypeClient(eventServiceBinding, new EndpointAddress(eventServiceAddress));

            System.Net.ServicePointManager.Expect100Continue = false;

            AttachSecurity(eventPortTypeClient.Endpoint);

            SetupChannel(eventPortTypeClient.InnerChannel);

            // Create filter from TopicInfo

            Event.FilterType filter = CreateFilter(topicInfo);

            // Create subscription
            int subscriptionTimeout = 60; // from specification

            string terminationTimeString = string.Format("PT{0}S", subscriptionTimeout);

            XmlElement[] any = null;
            System.DateTime currentTime = System.DateTime.MinValue;
            System.DateTime? terminationTime = null;

            Event.EndpointReferenceType subscription = null;

            try
            {
                RunStep(() =>
                {
                    subscription = eventPortTypeClient.CreatePullPointSubscription(
                        filter,
                        terminationTimeString,
                        null,
                        ref any,
                        out currentTime,
                        out terminationTime);
                },
                 "Create Pull Point Subsciption");
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                eventPortTypeClient.Close();
            }
            // Validate Subscription
            subscribed = System.DateTime.Now;

            Utils.EventServiceUtils.ValidateSubscription(terminationTime,
                currentTime,
                subscriptionTimeout,
                subscription,
                Assert);

            return subscription;
        }

        /// <summary>
        /// Delegate definition for GetMessages.
        /// </summary>
        /// <returns></returns>
        private delegate System.DateTime PullMessageDelegate(ref System.DateTime localCurrentTime,
            ref System.DateTime localTerminationTime);

        private class PullMessagesData
        {
            public Event.NotificationMessageHolderType[] NotificationMessages;
            public string PullMessagesResponseData;
        }

        protected Dictionary<Event.NotificationMessageHolderType, string> GetMessages(
            Event.EndpointReferenceType subscription, System.DateTime operationTime)
        {
            return GetMessages(subscription, operationTime, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="operationTime">UTC time</param>
        /// <param name="exitCheck"></param>
        /// <returns></returns>
        protected Dictionary<Event.NotificationMessageHolderType, string> GetMessages(
            Event.EndpointReferenceType subscription, System.DateTime operationTime,
            Func<Event.NotificationMessageHolderType, bool> exitCheck)
        {
            Event.PullPointSubscriptionClient pullPointClient = null;

            try
            {
                // Create new service client to pass "local" traffic listener
                IChannelController[] controllers;

                EndpointAddress address = new EndpointAddress(subscription.Address.Value);
                EndpointController controller = new EndpointController(address);
                WsaController wsaController = new WsaController();

                controllers = new IChannelController[]
                              {
                                  _trafficListener, 
                                  controller, 
                                  wsaController,
                                  _semaphore, 
                                  _credentialsProvider,
                                  new SoapValidator(EventsSchemasSet.GetInstance())
                              };
                Binding binding = CreateBinding(controllers);

                pullPointClient = new TestTool.Proxies.Event.PullPointSubscriptionClient(binding, new EndpointAddress(subscription.Address.Value));

                AttachSecurity(pullPointClient.Endpoint);
                SetupChannel(pullPointClient.InnerChannel);
                AttachAddressing(pullPointClient.Endpoint, subscription);

                // from spec
                int messagesLimit = 1;
                string timeString = "PT60S";
                //string timeString = "PT20S";

                // Total list of notifications
                Dictionary<Event.NotificationMessageHolderType, string> totalMessagesList =
                    new Dictionary<TestTool.Proxies.Event.NotificationMessageHolderType, string>();


                PullMessagesData pullMessagesData = null;

                AutoResetEvent requestSentErrorEvent = new AutoResetEvent(false);

                // initialize delegate
                PullMessageDelegate del =
                    new PullMessageDelegate(
                        (ref System.DateTime localCurrentTime, ref System.DateTime localTerminationTime) =>
                        {
                            Event.NotificationMessageHolderType[] notificationMessageCopy = null;
                            System.DateTime terminationTimeCopy = System.DateTime.MinValue;
                            System.DateTime result = System.DateTime.MinValue;

                            try
                            {
                                result = pullPointClient.PullMessages(timeString,
                                                                           messagesLimit,
                                                                           null,
                                                                           out terminationTimeCopy,
                                                                           out notificationMessageCopy);
                            }
                            catch (System.Net.Sockets.SocketException exc)
                            {
                                if (InStep)
                                {
                                    StepFailed(exc);
                                }
                                requestSentErrorEvent.Set();
                            }
                            localTerminationTime = terminationTimeCopy.ToUniversalTime();
                            pullMessagesData.NotificationMessages = notificationMessageCopy;
                            localCurrentTime = result.ToUniversalTime();

                            return localCurrentTime;
                        });


                // create event handler to save response
                _trafficListener.ResponseReceived += new Action<string>((data) =>
                {
                    pullMessagesData.PullMessagesResponseData = data;
                });

                System.DateTime eventLastTime = operationTime.AddSeconds(_operationDelay/1000).ToUniversalTime();

                while (true)
                {
                    pullMessagesData = new PullMessagesData();
                    System.DateTime dutCurrentTime = GetMessages(messagesLimit, pullMessagesData, del);


                    BeginStep("Check that more PullMessages requests are needed");
                    if (pullMessagesData.NotificationMessages.Length == 0)
                    {
                        if (dutCurrentTime > eventLastTime)
                        {
                            LogStepEvent("Allowed interval for event generation is expired, stop getting notifications");
                            StepPassed();
                            break;
                        }
                    }
                    else
                    {
                        TestTool.Proxies.Event.NotificationMessageHolderType message = pullMessagesData.NotificationMessages[0];

                        string utcTimeValue = message.Message.Attributes[OnvifMessage.UTCTIMEATTRIBUTE].Value;
                        // xs:dateTime string

                        System.DateTime messageTime = XmlConvert.ToDateTime(utcTimeValue, XmlDateTimeSerializationMode.Utc);

                        if (messageTime > eventLastTime)
                        {
                            LogStepEvent("Last message received is out of interval of interest, stop getting messages");
                            StepPassed();
                            break;
                        }

                        string rawSoapPacket = Utils.EventServiceUtils.GetSoapPacket(pullMessagesData.PullMessagesResponseData);

                        if (exitCheck != null)
                        {
                            bool messageFound = exitCheck(message);
                            if (messageFound)
                            {
                                LogStepEvent("Expected message found, stop getting results with PullMessages");

                                foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                                {
                                    totalMessagesList.Add(m, rawSoapPacket);
                                }

                                StepPassed();
                                break;
                            }
                        }
                        else
                        {
                            foreach (TestTool.Proxies.Event.NotificationMessageHolderType m in pullMessagesData.NotificationMessages)
                            {
                                totalMessagesList.Add(m, rawSoapPacket);
                            }
                        }
                    }

                    StepPassed();
                }

                return totalMessagesList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                pullPointClient.Close();
            }
        }

        private System.DateTime GetMessages(int messagesLimit,
            PullMessagesData pullMessagesData,
            PullMessageDelegate del)
        {
            // declare parameters
            System.DateTime localTerminationTime = System.DateTime.MinValue;
            System.DateTime localCurrentTime = System.DateTime.MinValue;

            //
            // Send PullMessages request
            BeginStep("PullMessages");
            System.DateTime dateTime = del.Invoke(ref localCurrentTime, ref localTerminationTime);
            localCurrentTime = dateTime;
            StepPassed();
            //                

            Assert(localCurrentTime < localTerminationTime,
                "TerminationTime <= CurrentTime",
                "Validate CurrentTime and TerminationTime", null);

            Assert(pullMessagesData.NotificationMessages.Length <= messagesLimit,
                "Maximum number of messages exceeded",
                string.Format("Check that a maximum number of {0} Notification Messages is included in PullMessagesResponse", messagesLimit),
                null);

            return localCurrentTime;
        }

        void ReleaseSubscription(Event.EndpointReferenceType subscription, System.DateTime subscribed)
        {
            int timeout = 0;

            if (subscribed != System.DateTime.MinValue)
            {
                System.DateTime now = System.DateTime.Now;
                double seconds = (now - subscribed).TotalSeconds;
                if (seconds <= 60)
                {
                    // need to unsubscribe or release
                    timeout = (int)(60 - seconds);

                    Binding binding = CreateEventServiceBinding(subscription.Address.Value);
                    SubscriptionManagerClient client = new SubscriptionManagerClient(binding, new EndpointAddress(subscription.Address.Value));
                    AttachSecurity(client.Endpoint);
                    AttachAddressing(client.Endpoint, subscription);

                    LogTestEvent("Delete Subscription Manager" + Environment.NewLine);

                    bool unsubscribeByRequest = false;
                    try
                    {
                        RunStep(() => { client.Unsubscribe(new Unsubscribe()); }, "Unsubscribe");
                        unsubscribeByRequest = true;
                    }
                    catch (FaultException exc)
                    {
                        LogFault(exc);
                        LogStepEvent("Failed to unsubscribe through request.");
                        StepPassed();
                    }
                    catch (System.Net.Sockets.SocketException exc)
                    {
                        LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
                        StepPassed();
                    }
                    catch (Exception exc)
                    {
                        LogStepEvent(string.Format("Failed to unsubscribe through request. Error received: {0}", exc.Message));
                        StepPassed();
                    }
                    finally
                    {
                        client.Close();
                    }

                    if (!unsubscribeByRequest)
                    {
                        RunStep(() => { Sleep(timeout); }, "Wait until Subscription Manager is deleted by timeout");
                    }

                }
            }
        }

        /// <summary>
        /// Creates filter element for Subscribe request.
        /// </summary>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="messageDescription">Message description</param>
        /// <returns></returns>
        TestTool.Proxies.Event.FilterType CreateFilter(TopicInfo topicInfo)
        {
            Event.FilterType filter = new Event.FilterType();

            XmlDocument filterDoc = new XmlDocument();
            XmlElement filterTopicElement = filterDoc.CreateTopicElement();

            string topicPath = TopicInfo.CreateTopicPath(filterTopicElement, topicInfo);

            filterTopicElement.InnerText = topicPath;

            filter.Any = new XmlElement[] { filterTopicElement };

            return filter;
        }

        bool CheckJobMessage(Event.NotificationMessageHolderType message, string operationType, string jobToken, string state)
        {
            if (message.Message.HasAttribute(OnvifMessage.PROPERTYOPERATIONTYPE))
            {
                XmlAttribute propertyOperationType = message.Message.Attributes[OnvifMessage.PROPERTYOPERATIONTYPE];
                if (propertyOperationType.Value != operationType)
                {
                    return false;
                }
            }

            Dictionary<string, string> sourceSimpleItems = BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.SOURCE, message.Message);

            if (!sourceSimpleItems.ContainsKey(RECORDINGJOBTOKENSIMPLEITEM))
            {
                return false;
            }

            if (sourceSimpleItems[RECORDINGJOBTOKENSIMPLEITEM] != jobToken)
            {
                return false;
            }

            Dictionary<string, string> dataSimpleItems = BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.DATA, message.Message);

            if (!dataSimpleItems.ContainsKey(STATESIMPLEITEM))
            {
                return false;
            }
            if (dataSimpleItems[STATESIMPLEITEM] != state)
            {
                return false;
            }
            return true;
        }

        bool CheckReceiverChangedMessage(Event.NotificationMessageHolderType message, string receiverToken, ReceiverState state)
        {
            Dictionary<string, string> sourceSimpleItems =
                BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.SOURCE, message.Message);

            if (!sourceSimpleItems.ContainsKey(RECEIVERTOKENSIMPLEITEM))
            {
                return false;
            }

            if (sourceSimpleItems[RECEIVERTOKENSIMPLEITEM] != receiverToken)
            {
                return false;
            }

            Dictionary<string, string> dataSimpleItems = BaseNotificationUtils.GetMessageSimpleItems(OnvifMessage.DATA, message.Message);

            if (!dataSimpleItems.ContainsKey(NEWSTATESIMPLEITEM))
            {
                return false;
            }
            if (dataSimpleItems[NEWSTATESIMPLEITEM] != state.ToString())
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Validate messages

        void ValidateNotificationMessages(Dictionary<Event.NotificationMessageHolderType, XmlElement> notificationMessages,
                                          TopicInfo topic, bool propertyEvent)
        {
            //29.	Verify that property event is returned.
            //  - this means Property attribute;
            //30.	Verify that this NotificationMessage is well formed; 
            //  - Source/SimpleItem, Data/SimpleItem
            //31.	Verify that the Topic of the NotificationMessage matches the filter
            //  - just matching

            // Find raw elements 

            BeginStep("Validate Messages");


            string reason = string.Empty;
            bool ok = true;
            foreach (Event.NotificationMessageHolderType message in notificationMessages.Keys)
            {
                XmlElement messageRawElement = notificationMessages[message];
                XmlNamespaceManager manager = EventsMainHelper.CreateNamespaceManager(messageRawElement.OwnerDocument);

                ok = EventServiceUtils.IsValidMessageElement(message.Message, propertyEvent, out reason);

                // validate topic
                if (ok)
                {
                    TopicInfo actualTopic = EventServiceUtils.ExtractTopicInfo(message, messageRawElement, manager, out reason);

                    if (actualTopic == null)
                    {
                        ok = false;
                    }
                    else
                    {
                        string expectedTopicDescription = topic.GetDescription();
                        string actualTopicDescription = actualTopic.GetDescription();

                        bool match = TopicInfo.TopicsMatch(actualTopic, topic);

                        if (!match)
                        {
                            reason = string.Format("Invalid topic. {0}Expected: {1}{0}Actual: {2}",
                                                   Environment.NewLine,
                                                   expectedTopicDescription,
                                                   actualTopicDescription);
                            ok = false;
                        }
                    }

                }
            }

            if (!ok)
            {
                throw new AssertException(reason);
            }
            StepPassed();

        }

        /// <summary>
        /// This is test-specific. I.E. this method checks presence of exactly ONE element item,
        /// as specified for the topic of interest!
        /// </summary>
        /// <param name="messageElement"></param>
        /// <returns></returns>
        XmlElement ValidateElementItem(XmlElement messageElement)
        {
            List<XmlElement> elementItems = BaseNotificationUtils.GetMessageElementItems("Data", messageElement);

            XmlElement infoElement = null;
            {
                bool ok = true;
                string errorMessage = null;
                if (elementItems.Count == 0)
                {
                    ok = false;
                    errorMessage = "Notification message does not contain any ElementItems";
                }
                else if (elementItems.Count > 1)
                {
                    ok = false;
                    errorMessage = "Notification message contains more than one ElementItems";
                }
                else
                {
                    XmlElement elementItem = elementItems[0];
                    if (elementItem.ChildNodes.OfType<XmlElement>().Count() != 1)
                    {
                        ok = false;
                        // Core spec, p. 9.5.2
                        // In the case of an ElementItem, the value is expressed by one XML element within the ElementItem element.
                        errorMessage = "Element item should contain only one XML Element inside";
                    }
                    else
                    {
                        infoElement = elementItem.ChildNodes.OfType<XmlElement>().First();
                    }
                }
                Assert(ok, errorMessage, "Check that notification message contains only one well-formed ElementItem");
            }

            return infoElement;
        }

        void ValidateElementItemContent(XmlElement element, string name, string ns)
        {
            {
                // validator 
                XmlElementValidator validator = null;
                StringBuilder sb = new StringBuilder();

                bool hasErrors = false;

                if (element.LocalName != name ||
                    element.NamespaceURI.ToLower() != ns.ToLower())
                {
                    hasErrors = true;

                    sb.AppendFormat("Content of ElementItem element is incorrect: expected {0} from {1} namespace, actual {2} from {3} namespace",
                                    name, ns, element.LocalName, element.NamespaceURI);
                }
                else
                {
                    // schema validation will be performed automatically only for Device service
                    BaseSchemaSet schemaSet = TypesSchemaSet.GetInstance();
                    validator = new XmlElementValidator(schemaSet);

                    //validate
                    string error = string.Empty;
                    try
                    {
                        validator.Validate(element);
                    }
                    catch (Exception exc)
                    {
                        hasErrors = true;
                        error = exc.Message;

                        sb.AppendFormat("Content of ElementItem element is not valid according to the schema: {0}",
                                        error);
                    }
                }
                string errDump = sb.ToStringTrimNewLine();
                Assert(!hasErrors, errDump, "Check that ElementItem content is correct");

            }
        }

        protected T ExtractElementItemsContent<T>(XmlElement element, string name, string ns)
        {
            BeginStep("Parse ElementItem content");

            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = name;
            xRoot.IsNullable = true;
            xRoot.Namespace = ns;

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            T capabilities;
            try
            {
                capabilities = (T)serializer.Deserialize(reader);
            }
            catch (Exception exc)
            {
                string message;
                if (exc.InnerException != null)
                {
                    message = string.Format("{0} {1}", exc.Message, exc.InnerException.Message);
                }
                else
                {
                    message = exc.Message;
                }
                throw new ApplicationException(message);
            }
            StepPassed();
            return capabilities;
        }

        #endregion


        void ValidateJobState(RecordingJobStateInformation state, string recordingToken, string expectedState)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = true;

            if (state.RecordingToken != recordingToken)
            {
                ok = false;
                sb.AppendLine(string.Format("RecordingToken is incorrect: expected {0}, actual {1}", recordingToken, state.RecordingToken));
            }

            if (state.State != expectedState)
            {
                ok = false;
                sb.AppendLine(string.Format("State is incorrect: expected {0}, actual {1}", expectedState, state.State));
            }

            if (state.Sources != null)
            {
                foreach (RecordingJobStateSource source in state.Sources)
                {
                    //if (source.State != null)
                    if (source.State != null && source.State != expectedState)
                    {
                        ok = false;
                        sb.AppendLine(string.Format("State for source with token '{0}' is incorrect: expected {1}, actual {2}",
                            source.SourceToken.Token, expectedState, source.State));
                    }
                }
            }

            Assert(ok, sb.ToStringTrimNewLine(), "Validate RecordingJobStateInformation");
        }

        void ValidateReceiverState(ReceiverStateInformation state, ReceiverState expectedState)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = true;

            if (state.State != expectedState)
            {
                ok = false;
                sb.AppendLine(string.Format("State is incorrect: expected {0}, actual {1}", expectedState, state.State));
            }

            //if (state.Sources != null)
            //{
            //    foreach (RecordingJobStateSource source in state.Sources)
            //    {
            //        if (source.State != null)
            //        {
            //            ok = false;
            //            sb.AppendLine(string.Format("State for source with token '{0}' is incorrect: expected {1}, actual {2}",
            //                source.SourceToken.Token, expectedState, state.State));
            //        }
            //    }
            //}

            Assert(ok, sb.ToStringTrimNewLine(), "Validate ReceiverStateInformation");
        }

        #region RTSP simulator

        void StartSimulator(RTSPSimulator sim)
        {
            RunStep(() => { sim.StartRTSP(); }, "Start simulator");
        }

        void StopSimulator(RTSPSimulator sim)
        {
            RunStep(() => { sim.StopRTSP(); }, "Stop simulator");
        }

        string GetCurrentDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);
            return path;
        }

        const string STREAM1 = "Stream1";
        const string STREAM2 = "Stream2";

        string GetStreamName(RTSPSimulator.Codecs codec, string streamName)
        {
            return string.Format("{0}_{1}", codec, streamName);
        }

        void CreateStreams(RTSPSimulator sim, IEnumerable<string> encodingsSupported)
        {
            if (encodingsSupported.Contains("JPEG"))
            {
                _simulator.Add(RTSPSimulator.Codecs.H264,
                    System.IO.Path.Combine(GetCurrentDirectory(), "Streams\\Jpeg\\video_480x360_fps30-%04d.jpeg"),
                    GetStreamName(RTSPSimulator.Codecs.H264, STREAM1));
                _simulator.Add(RTSPSimulator.Codecs.H264,
                    System.IO.Path.Combine(GetCurrentDirectory(), "Streams\\Jpeg\\video_640x480_fps15-%04d.jpeg"),
                    GetStreamName(RTSPSimulator.Codecs.H264, STREAM2));
            }
            if (encodingsSupported.Contains("H264"))
            {
                _simulator.Add(RTSPSimulator.Codecs.H264,
                    System.IO.Path.Combine(GetCurrentDirectory(), "video_480x360_fps30.264"),
                    GetStreamName(RTSPSimulator.Codecs.H264, STREAM1));
                _simulator.Add(RTSPSimulator.Codecs.H264,
                    System.IO.Path.Combine(GetCurrentDirectory(), "video_640x480_fps15.264"),
                    GetStreamName(RTSPSimulator.Codecs.H264, STREAM2));
            }
            if (encodingsSupported.Contains("MPEG4"))
            {
                _simulator.Add(RTSPSimulator.Codecs.MPEG4,
                    System.IO.Path.Combine(GetCurrentDirectory(), "video_480x360_fps30.m4e"),
                    GetStreamName(RTSPSimulator.Codecs.MPEG4, STREAM1));
                _simulator.Add(RTSPSimulator.Codecs.MPEG4,
                    System.IO.Path.Combine(GetCurrentDirectory(), "video_640x480_fps15.m4e"),
                    GetStreamName(RTSPSimulator.Codecs.MPEG4, STREAM2));
            }
            if (encodingsSupported.Contains("G711"))
            {
                _simulator.Add(RTSPSimulator.Codecs.G711,
                    System.IO.Path.Combine(GetCurrentDirectory(), "test.711"),
                    GetStreamName(RTSPSimulator.Codecs.G711, STREAM1));
            }
            if (encodingsSupported.Contains("AAC"))
            {
                _simulator.Add(RTSPSimulator.Codecs.AAC,
                    System.IO.Path.Combine(GetCurrentDirectory(), "test.AAC"),
                    GetStreamName(RTSPSimulator.Codecs.AAC, STREAM1));
            }
        }

        string GetUrl(RTSPSimulator sim, IEnumerable<string> encodingsSupported, string name)
        { 
            //"G711", "G726", "AAC", "JPEG", "MPEG4", "H264" 
            if (encodingsSupported.Contains("H264"))
            {
                return _simulator.GetUrl(RTSPSimulator.Codecs.H264, GetStreamName(RTSPSimulator.Codecs.H264, name));
            }
            if (encodingsSupported.Contains("MPEG4"))
            {
                return _simulator.GetUrl(RTSPSimulator.Codecs.MPEG4, GetStreamName(RTSPSimulator.Codecs.MPEG4, name));
            }
            if (encodingsSupported.Contains("G711"))
            {
                return _simulator.GetUrl(RTSPSimulator.Codecs.G711, GetStreamName(RTSPSimulator.Codecs.G711, name));
            }
            if (encodingsSupported.Contains("AAC"))
            {
                return _simulator.GetUrl(RTSPSimulator.Codecs.AAC, GetStreamName(RTSPSimulator.Codecs.AAC, name));
            }
            return null;
        }

        #endregion

        private void VerifyProfile(Profile profile)
        {
            
            bool ok = true;
            var logger = new StringBuilder();
            if (profile != null)
            {
                if (profile.VideoEncoderConfiguration != null)
                {
                    if (profile.VideoEncoderConfiguration.Resolution == null)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("profile doesn't contain resolution in VideoEncoderConfiguration{0}",
                                          Environment.NewLine));
                    }
                    if (profile.VideoEncoderConfiguration.Quality <= 0)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("profile doesn't contain correct quality in VideoEncoderConfiguration{0}",
                                          Environment.NewLine));
                    }
                    if (profile.VideoEncoderConfiguration.RateControl.FrameRateLimit <= 0)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("profile doesn't contain correct FrameRateLimit in VideoEncoderConfiguration{0}",
                                          Environment.NewLine));
                    }
                    if (profile.VideoEncoderConfiguration.RateControl.BitrateLimit <= 0)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("profile doesn't contain correct BitrateLimit in VideoEncoderConfiguration{0}",
                                          Environment.NewLine));
                    }
                }
                else
                {
                    ok = false;
                    logger.Append(string.Format("profile doesn't contain VideoEncoderConfiguration{0}", Environment.NewLine));
                }
            }
            else
            {
                ok = false;
                logger.Append(string.Format("profile wasn't returned{0}", Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Check profile");
        }

        private void VerifyOptions(VideoEncoderConfigurationOptions options, VideoEncoding encoding)
        {
            // 15.	Verify GetVideoEncoderConfigurationOptionsResponse message. 
            // Verify Options.QualityRange, 
            // Options.<Encoding>.ResolutionsAvailable, 
            // Options. <Encoding>.FrameRateRange, 
            // Options.Extension. <Encoding>.FrameRateRange.

            bool ok = true;
            var logger = new StringBuilder();
            if (options != null)
            {
                if (options.QualityRange == null)
                {
                    ok = false;
                    logger.Append(
                        string.Format("Options doesn't contain QualityRange{0}",
                                        Environment.NewLine));
                }

                switch (encoding)
                {
                    case VideoEncoding.JPEG:
                    {
                        if (options.JPEG != null)
                        {
                            if (options.JPEG.ResolutionsAvailable == null)
                            {
                                ok = false;
                                logger.Append(
                                    string.Format("Options doesn't contain ResolutionsAvailable for JPEG{0}",
                                                    Environment.NewLine));
                            }
                            if (options.Extension != null)
                            {
                                if (options.Extension.JPEG != null)
                                {
                                    if (options.Extension.JPEG.FrameRateRange == null)
                                    {
                                        ok = false;
                                        logger.Append(
                                            string.Format("Options doesn't contain FrameRateRange for JPEG in extensions{0}",
                                                            Environment.NewLine));
                                    }
                                }
                            }
                        }
                        else
                        {
                            ok = false;
                            logger.Append(
                                string.Format("Options doesn't contain JPEG{0}",
                                                Environment.NewLine));
                        }

                        break;
                    }

                    case VideoEncoding.H264:
                    {
                        if (options.H264 != null)
                        {
                            if (options.H264.ResolutionsAvailable == null)
                            {
                                ok = false;
                                logger.Append(
                                    string.Format("Options doesn't contain ResolutionsAvailable for H264{0}",
                                                    Environment.NewLine));
                            }
                            if (options.Extension != null)
                            {
                                if (options.Extension.H264 != null)
                                {
                                    if (options.Extension.H264.FrameRateRange == null)
                                    {
                                        ok = false;
                                        logger.Append(
                                            string.Format("Options doesn't contain FrameRateRange for H264 in extensions{0}",
                                                            Environment.NewLine));
                                    }
                                }
                            }
                        }
                        else
                        {
                            ok = false;
                            logger.Append(
                                string.Format("Options doesn't contain H264{0}",
                                                Environment.NewLine));
                        }

                        break;
                    }

                    case VideoEncoding.MPEG4:
                    {
                        if (options.MPEG4 != null)
                        {
                            if (options.MPEG4.ResolutionsAvailable == null)
                            {
                                ok = false;
                                logger.Append(
                                    string.Format("Options doesn't contain ResolutionsAvailable for MPEG4{0}",
                                                    Environment.NewLine));
                            }
                            if (options.Extension != null)
                            {
                                if (options.Extension.MPEG4 != null)
                                {
                                    if (options.Extension.MPEG4.FrameRateRange == null)
                                    {
                                        ok = false;
                                        logger.Append(
                                            string.Format("Options doesn't contain FrameRateRange for MPEG4 in extensions{0}",
                                                            Environment.NewLine));
                                    }
                                }
                            }
                        }
                        else
                        {
                            ok = false;
                            logger.Append(
                                string.Format("Options doesn't contain MPEG4{0}",
                                                Environment.NewLine));
                        }

                        break;
                    }
                }
            }
            else
            {
                ok = false;
                logger.Append(string.Format("There are no options were returned", Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Check options");
        }

        private void VerifyVideoConfiguration(VideoEncoderConfiguration conf, VideoEncoderConfiguration newConf)
        {
            StringBuilder logger = new StringBuilder();
            bool ok = true;
            if (newConf != null)
            {
                if (newConf.Quality!= conf.Quality)
                {
                    ok = false;
                    logger.Append(
                        string.Format("Quality is {0} but must be {1}{2}", 
                        newConf.Quality, conf.Quality, Environment.NewLine));
                }
                if (newConf.Resolution != null)
                {
                    if (newConf.Resolution.Height!= conf.Resolution.Height)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("Resolution.Height is {0} but must be {1}{2}",
                            newConf.Resolution.Height, conf.Resolution.Height, Environment.NewLine));
                    }
                    if (newConf.Resolution.Width != conf.Resolution.Width)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("Resolution.Width is {0} but must be {1}{2}",
                            newConf.Resolution.Width, conf.Resolution.Width, Environment.NewLine));
                    }
                }
                else
                {
                    ok = false;
                    logger.Append(
                        string.Format("VideoEncoderConfigurationResponse doesn't contain Resolution{0}",
                        Environment.NewLine));
                }
                if (newConf.RateControl != null)
                {
                    if (newConf.RateControl.BitrateLimit != conf.RateControl.BitrateLimit)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("RateControl.BitrateLimit is {0} but must be {1}{2}",
                            newConf.RateControl.BitrateLimit, conf.RateControl.BitrateLimit, Environment.NewLine));
                    }
                    if (newConf.RateControl.FrameRateLimit != conf.RateControl.FrameRateLimit)
                    {
                        ok = false;
                        logger.Append(
                            string.Format("RateControl.FrameRateLimit is {0} but must be {1}{2}",
                            newConf.RateControl.FrameRateLimit, conf.RateControl.FrameRateLimit, Environment.NewLine));
                    }
                }
                else
                {
                    ok = false;
                    logger.Append(
                        string.Format("VideoEncoderConfigurationResponse doesn't contain RateControl{0}",
                        Environment.NewLine));
                }
            }
            else
            {
                ok = false;
                logger.Append(
                    string.Format("Video encoder configuration wasn't returned{0}",Environment.NewLine));
            }
            Assert(ok, logger.ToStringTrimNewLine(), "Verify video encoder configuration");

        }
    }
}
