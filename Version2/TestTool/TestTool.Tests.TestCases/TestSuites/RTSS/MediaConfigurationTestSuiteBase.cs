using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{

    class MediaConfigurationTestSuiteBase : RTSSTestSuite
    {
        public MediaConfigurationTestSuiteBase(TestLaunchParam param)
            : base(param)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="changeLog"></param>
        /// <returns></returns>
        protected Profile GetProfileWithAudioEncoderConfiguration(AudioEncoding encoding,
            MediaConfigurationChangeLog changeLog)
        {
            Func<AudioEncoderConfiguration, bool> configTest = (C) => { return C.Encoding == encoding; };
            return GetProfileWithAudioEncoderConfiguration(configTest, changeLog);
        }

        protected Profile GetProfileWithAudioEncoderConfiguration(string configName,
            MediaConfigurationChangeLog changeLog)
        {
            Func<AudioEncoderConfiguration, bool> configTest = (C) => { return C.token == configName; };
            return GetProfileWithAudioEncoderConfiguration(configTest, changeLog);
        }

        /// <summary>
        /// Annex A.5	Find or Create Media Profile Containing Specified Audio Encoder Configuration
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="changeLog"></param>
        /// <returns></returns>
        protected Profile GetProfileWithAudioEncoderConfiguration(Func<AudioEncoderConfiguration, bool> configTest, 
            MediaConfigurationChangeLog changeLog)
        {
            string TESTPROFILENAME = "TestProfile1";

            Profile profile = null;

            //1.	ONVIF Client will invoke GetProfilesRequest message to retrieve complete 
            // profiles list.
            //2.	Verify the GetProfilesResponse message from the DUT.

            Profile[] profiles = GetProfiles();
            CheckProfilesList(profiles);
                        
            //3.	Try to find profile that contains Audio Encoder Configuration with specified 
            // token and Audio Source Configuration. If there is no such Profiles go to the next 
            // step, otherwise use one of profiles that fit to the requirements and skip other steps.


            Profile nonFixedProfile = null;
            Profile oldProfile = profiles[0];

            foreach (Profile p in profiles)
            {
                if (p.AudioSourceConfiguration != null &&
                    p.AudioEncoderConfiguration != null &&
                    configTest(p.AudioEncoderConfiguration))
                {
                    profile = p;
                    LogTestEvent(string.Format("Use profile with token '{0}'{1}", p.token, Environment.NewLine));
                    break;
                }

                // look also at @fixed attribute = may be will be usefull...
                if (nonFixedProfile == null)
                {
                    if (!(p.fixedSpecified && p.@fixed))
                    {
                        nonFixedProfile = p;
                    }
                }
            }
            
            if (profile == null)
            {
                //5.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
                // or SOAP 1.2 fault message (Action/MaxNVTProfiles) from the DUT. If CreateProfileResponse 
                // message was received go to the step 10.
                //6.	ONVIF Client will invoke DeleteProfileRequest message (ProfileToken = “Profile2”, 
                // where “Profile2” is token of profile with fixed=”false”) to remove profile. If there are 
                // no profiles with fixed=”false” skip other steps (this will means that it is not possible 
                // to find or create profile for specified audio encoder configuration).
                //7.	Verify the DeleteProfilesResponse message from the DUT.
                //8.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) to 
                // create new profile.
                //9.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) from the DUT.
                
                bool isNew = false;
                Profile deletedProfile = null;
                profile = CreateProfileHandleMaxNvtProfiles(TESTPROFILENAME,
                    nonFixedProfile, oldProfile, false, out isNew, out deletedProfile, changeLog);
                
                if (!isNew)
                {
                    Profile backup = Utils.CopyMaker.CreateCopy(profile);
                    changeLog.TrackModifiedProfile(backup);
                    RemoveAudioConfigurations(profile, changeLog);
                }              

                //10.	ONVIF Client will invoke GetCompatibleAudioSourceConfigurationsRequest message 
                // (ProfileToken = “ProfileToken1”) to retrieve compatible audio source configurations 
                // list.
                //11.	Verify the GetCompatibleAudioSourceConfigurationsResponse message from the DUT. 
                // If GetCompatibleAudioSourceConfigurationsResponse message contains empty list skip 
                // other steps (this will means that it is not possible to fined or create profile for 
                // specified audio encoder configuration).

                AudioSourceConfiguration[] compatibleSourceConfigurations = GetCompatibleAudioSourceConfigurations(profile.token);

                Assert(compatibleSourceConfigurations != null && compatibleSourceConfigurations.Length > 0,
                    "The DUT returned no Audio Source Configurations that can be used to configure profile",
                    "Check that the DUT returned any compatible Audio Source Configurations");

                bool profileConfigured = false;
                foreach (AudioSourceConfiguration config in compatibleSourceConfigurations)
                {

                    //12.	ONVIF Client will invoke AddAudioSourceConfigurationRequest message 
                    // (ProfileToken = “ProfileToken1”, ConfigurationToken = “ASCToken1”, where “ASCToken1” 
                    // is audio source configuration from GetCompatibleAudioSourceConfigurationsResponse 
                    // message) to add audio source configuration to profile.
                    //13.	Verify the AddAudioSourceConfigurationResponse message from the DUT.

                    AddAudioSourceConfiguration(profile.token, config.token);

                    //14.	ONVIF Client will invoke GetCompatibleAudioEncoderConfigurationsRequest message 
                    // (ProfileToken = “ProfileToken1”) to retrieve compatible audio encoder configurations 
                    // list.
                    AudioEncoderConfiguration[] compatibleAudioEncoderConfigurations =
                        GetCompatibleAudioEncoderConfigurations(profile.token);

                    //15.	Verify the GetCompatibleAudioEncoderConfigurationsResponse message from the DUT. 
                    // If GetCompatibleAudioEncoderConfigurationsResponse message does not contains specified 
                    // audio encoder configuration repeat steps 12-15 for other audio source configuration 
                    // from GetCompatibleAudioSourceConfigurationsResponse message. If there is no audio source 
                    // configuration that was not used in steps 12-15, skip other steps (this will means that 
                    // it is not possible to find or create profile for specified audio encoder configuration).

                    Assert(compatibleAudioEncoderConfigurations != null && compatibleAudioEncoderConfigurations.Length > 0,
                        "No compatible Audio Encoder Configuration returned", 
                        "Check if the DUT returned any compatible Audio Encoder Configurations");

                    AudioEncoderConfiguration encoderConfig = compatibleAudioEncoderConfigurations.FirstOrDefault(configTest);

                    if (encoderConfig != null)
                    {

                        //16.	ONVIF Client will invoke AddAudioEncoderConfigurationRequest message 
                        // (ProfileToken = “ProfileToken1”, ConfigurationToken = “AECToken1”, where “AECToken1” is 
                        // audio encoder configuration from GetCompatibleAudioEncoderConfigurationsResponse message) 
                        // to add audio encoder configuration to profile. Use this profile as result of procedure.

                        AddAudioEncoderConfiguration(profile.token, encoderConfig.token);
                        profileConfigured = true;
                        profile = GetProfile(profile.token);
                        break;
                    }
                }

                if (!profileConfigured)
                {
                    return null;
                }
            }
            
            return profile;
        }
        
        protected Profile GetProfileWithVideoEncoderConfiguration(string configName,
            MediaConfigurationChangeLog changeLog)
        {
            Func<VideoEncoderConfiguration, bool> configTest = (C) => { return C.token == configName; };
            return GetProfileWithVideoEncoderConfiguration(configTest, changeLog);
        }
                
        /// <summary>
        /// A.7	Find or Create Media Profile Containing Specified Video Encoder Configuration
        /// </summary>
        /// <param name="configTest"></param>
        /// <param name="changeLog"></param>
        /// <returns></returns>
        protected Profile GetProfileWithVideoEncoderConfiguration(Func<VideoEncoderConfiguration, bool> configTest,
            MediaConfigurationChangeLog changeLog)
        {

            //1.	ONVIF Client will invoke GetProfilesRequest message to retrieve complete profiles list.
            //2.	Verify the GetProfilesResponse message from the DUT.
            string TESTPROFILENAME = "TestProfile1";

            Profile profile = null;

            Profile[] profiles = GetProfiles();
            Assert(profiles != null && profiles.Length > 0,
                "The DUT return no profiles", "Check if the DUT returned any profiles");
                        
            //3.	Try to find profile that contains Video Encoder Configuration with specified 
            // token and Video Source Configuration. If there is no such Profiles go to the next step, 
            // otherwise use one of profiles that fit to the requirements and skip other steps.

            Profile nonFixedProfile = null;
            Profile oldProfile = profiles[0];

            foreach (Profile p in profiles)
            {
                if (p.VideoSourceConfiguration != null &&
                    p.VideoEncoderConfiguration != null &&
                    configTest(p.VideoEncoderConfiguration))
                {
                    profile = p;
                    LogTestEvent(string.Format("Use profile with token '{0}'", p.token));
                    break;
                }

                // look also at @fixed attribute = may be will be usefull...
                if (nonFixedProfile == null)
                {
                    if (!(p.fixedSpecified && p.@fixed))
                    {
                        nonFixedProfile = p;
                    }
                }
            }                      
                                 
            if (profile == null)
            {
                //4.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) 
                // to create new profile.
                //5.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
                // or SOAP 1.2 fault message (Action/MaxNVTProfiles) from the DUT. If CreateProfileResponse 
                // message was received go to the step 10.
                //6.	ONVIF Client will invoke DeleteProfileRequest message 
                // (ProfileToken = “Profile2”, where “Profile2” is token of profile with fixed=”false”) 
                // to remove profile. If there are no profiles with fixed=”false” skip other steps (this will means that it is not possible to fined or create profile for specified audio encoder configuration).
                //7.	Verify the DeleteProfilesResponse message from the DUT.
                //8.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) 
                // to create new profile.
                //9.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
                // from the DUT.
                bool isNew;
                Profile deletedProfile = null;
                profile = CreateProfileHandleMaxNvtProfiles(TESTPROFILENAME, nonFixedProfile, oldProfile, false, out isNew, out deletedProfile, changeLog);

                if (!isNew)
                {
                    Profile backup = Utils.CopyMaker.CreateCopy(profile);
                    changeLog.TrackModifiedProfile(backup);
                    RemoveVideoConfigurations(profile, changeLog);
                }

                //10.	ONVIF Client will invoke GetCompatibleVideoSourceConfigurationsRequest message 
                // (ProfileToken = “ProfileToken1”) to retrieve compatible video source configurations list.
                //11.	Verify the GetCompatibleVideoSourceConfigurationsResponse message from the DUT. 
                // If GetCompatibleVideoSourceConfigurationsResponse message contains empty list skip other 
                // steps (this will means that it is not possible to fined or create profile for specified 
                // video encoder configuration).
                
                VideoSourceConfiguration[] compatibleSourceConfigurations = GetCompatibleVideoSourceConfigurations(profile.token);

                Assert(compatibleSourceConfigurations != null && compatibleSourceConfigurations.Length > 0,
                    "The DUT returned no Video Source Configurations that can be used to configure profile",
                    "Check that the DUT returned any compatible Video Source Configurations");

                bool profileConfigured = false;
                foreach (VideoSourceConfiguration config in compatibleSourceConfigurations)
                {                
                    //12.	ONVIF Client will invoke AddVideoSourceConfigurationRequest message 
                    // (ProfileToken = “ProfileToken1”, ConfigurationToken = “VSCToken1”, where 
                    // “VSCToken1” is video source configuration from 
                    // GetCompatibleVideoSourceConfigurationsResponse message) to add video source 
                    // configuration to profile.
                    //13.	Verify the AddVideoSourceConfigurationResponse message from the DUT.

                    AddVideoSourceConfiguration(profile.token, config.token);
                    
                    //14.	ONVIF Client will invoke GetCompatibleVideoEncoderConfigurationsRequest 
                    // message (ProfileToken = “ProfileToken1”) to retrieve compatible video encoder 
                    // configurations list.
                    VideoEncoderConfiguration[] compatibleVideoEncoderConfigurations =
                        GetCompatibleVideoEncoderConfigurations(profile.token);

                    if (compatibleVideoEncoderConfigurations != null)
                    {
                        //15.	Verify the GetCompatibleVideoEncoderConfigurationsResponse message from 
                        // the DUT. If GetCompatibleVideoEncoderConfigurationsResponse message does not 
                        // contains specified video encoder configuration repeat steps 12-15 for other video 
                        // source configuration from GetCompatibleVideoSourceConfigurationsResponse message. 
                        // If there is no video source configuration that was not used in steps 12-15, skip 
                        // other steps (this will means that it is not possible to fined or create profile 
                        // for specified video encoder configuration).

                        VideoEncoderConfiguration encoderConfig = compatibleVideoEncoderConfigurations.FirstOrDefault(configTest);
                        if (encoderConfig != null)
                        {
                            //16.	ONVIF Client will invoke AddVideoEncoderConfigurationRequest message 
                            // (ProfileToken = “ProfileToken1”, ConfigurationToken = “VECToken1”, where 
                            // “VECToken1” is video encoder configuration from 
                            // GetCompatibleVideoEncoderConfigurationsResponse message) to add video encoder 
                            // configuration to profile. Use this profile as result of procedure.
                            AddVideoEncoderConfiguration(profile.token, encoderConfig.token);
                            profile.VideoSourceConfiguration = config;
                            profile.VideoEncoderConfiguration = encoderConfig;
                            profileConfigured = true;
                            break;
                        }
                    }
                }

                if (!profileConfigured)
                {
                    return null;
                }
            }

            return profile;
        }

        Profile CreateProfileHandleMaxNvtProfiles(string profileName,
            Profile nonFixedProfile,
            Profile profileForUpdate,
            bool queryProfilesToDefineNonFixed,
            out bool isNew,
            out Profile deletedProfile,
            MediaConfigurationChangeLog changeLog)
        {
            bool retry = false;
            Profile profile = null;
            isNew = true;
            deletedProfile = null;
            try
            {
                BeginStep("Create profile for test");
                profile = Client.CreateProfile(profileName, null);
                DoRequestDelay();
                changeLog.CreatedProfiles.Add(profile);
                StepPassed();

                Assert(profile.@fixed == false && profile.Name == profileName,
                    "Profile Name or/and 'fixed' attribute are unexpected",
                    "Check that profile has been created properly");

            }
            catch (FaultException exc)
            {
                LogFault(exc);
                string err;
                if (exc.IsValidOnvifFault("Receiver/Action/MaxNVTProfiles", out err))
                {
                    LogStepEvent("Maximum number of profiles exceeded.");
                    retry = true;
                    StepPassed();
                }
                else
                {
                    throw exc;
                }
            }

            if (retry)
            {
                Profile profileToDelete = nonFixedProfile;
                Profile profileToUpdate = profileForUpdate;

                if (queryProfilesToDefineNonFixed)
                {
                    Profile[] profiles = GetProfiles();

                    Assert(profiles != null,
                        "There are no profiles which could be deleted or updated",
                        "Check if there are some profiles to delete or update for test");

                    profileToUpdate = profiles[0];

                    foreach (Profile p in profiles)
                    {
                        if (profileToDelete == null)
                        {
                            if (!(p.fixedSpecified && p.@fixed))
                            {
                                profileToDelete = p;
                                break;
                            }
                        }                    
                    }                
                }

                if (profileToDelete != null)
                {
                    deletedProfile = profileToDelete;
                    DeleteProfile(profileToDelete.token);
                    changeLog.TrackDeletedProfile(profileToDelete);

                    profile = CreateProfile(profileName, null);
                    changeLog.CreatedProfiles.Add(profile);

                    Assert(profile.@fixed == false && profile.Name == profileName,
                        "Profile Name or/and 'fixed' attribute are unexpected",
                        "Check that profile has been created properly");
                }
                else
                {
                    Assert(profileToUpdate != null,
                        "There are no profiles which can be used", 
                        "Check if an existing profile can be used for test");
                    isNew = false;
                    profile = profileToUpdate;
                }
            }

            return profile;
        }

        void RemoveVideoConfigurations(Profile profile, MediaConfigurationChangeLog changeLog)
        {
            if (profile.VideoEncoderConfiguration != null || profile.VideoSourceConfiguration != null)
            {            
                Profile backup = Utils.CopyMaker.CreateCopy(profile);
                if (profile.VideoEncoderConfiguration != null)
                {
                    RemoveVideoEncoderConfiguration(profile.token);
                    changeLog.TrackModifiedProfile(backup);
                }
                if (profile.VideoSourceConfiguration != null)
                {
                    RemoveVideoSourceConfiguration(profile.token);
                    changeLog.TrackModifiedProfile(backup);
                }
            }
        }

        void RemoveAudioConfigurations(Profile profile, MediaConfigurationChangeLog changeLog)
        {
            if (profile.AudioEncoderConfiguration != null || profile.AudioSourceConfiguration != null)
            {
                Profile backup = Utils.CopyMaker.CreateCopy(profile);

                if (profile.AudioEncoderConfiguration != null)
                {
                    RemoveAudioEncoderConfiguration(profile.token);
                    changeLog.TrackModifiedProfile(backup);
                }
                if (profile.AudioSourceConfiguration != null)
                {
                    RemoveAudioSourceConfiguration(profile.token);
                    changeLog.TrackModifiedProfile(backup);
                }
            }
        }

        protected Profile GetProfileForSpecificConfigurationAndCodec(string configToken, 
            VideoEncoding encoding, 
            MediaConfigurationChangeLog changeLog)
        {
            Profile profile = null;

            Profile[] profiles = GetProfiles();
            CheckProfilesList(profiles);

            foreach (Profile p in profiles.Where(P => P.VideoSourceConfiguration != null && P.VideoEncoderConfiguration != null))
            {
                if (p.VideoEncoderConfiguration.token == configToken)
                {
                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(configToken, p.token);
                    if (OptionsAllowEncoding(options, encoding))
                    {
                        profile = p;
                        BeginStep("Select profile for test");
                        LogStepEvent(string.Format("Use profile with token '{0}'", p.token));
                        StepPassed();
                        // profile found!
                        break;                    
                    }
                }            
            }

            if (profile == null)
            {
                string testProfileName = "TestProfile1";
                bool isNew = false;
                Profile deletedProfile = null;
                Profile newProfile = CreateProfileHandleMaxNvtProfiles(testProfileName, null, null, true, out isNew, out deletedProfile, changeLog);
                if (!isNew)
                {
                    Profile backup = Utils.CopyMaker.CreateCopy(newProfile);
                    changeLog.TrackModifiedProfile(backup);

                    RemoveVideoConfigurations(newProfile, changeLog);
                }

                VideoSourceConfiguration[] sourceConfigurations = GetCompatibleVideoSourceConfigurations(newProfile.token);

                CheckConfigurationsList(sourceConfigurations, "compatible Video Source Configurations");

                foreach (VideoSourceConfiguration config in sourceConfigurations)
                {
                    AddVideoSourceConfiguration(newProfile.token, config.token);

                    VideoEncoderConfiguration[] encoderConfigurations = GetCompatibleVideoEncoderConfigurations(newProfile.token);

                    if (encoderConfigurations != null)
                    {
                        if (encoderConfigurations.Where(C => C.token == configToken).FirstOrDefault() != null)
                        {
                            AddVideoEncoderConfiguration(newProfile.token, configToken);

                            VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(configToken, newProfile.token);
                            if (OptionsAllowEncoding(options, encoding))
                            {
                                profile = newProfile;
                                break;
                            }

                            RemoveVideoEncoderConfiguration(newProfile.token); // ???
                        }
                    }
                }
            }

            return profile;
        }

        bool CheckEncodingType(
                        VideoEncoding Encoding,
                        int? jpeg, int usedJpeg,
                        int? mpeg, int usedMpeg,
                        int? h264, int usedH264)
        {
            switch (Encoding)
            {
                case VideoEncoding.JPEG:
                    return jpeg.HasValue ? jpeg.Value > usedJpeg : true;
                case VideoEncoding.MPEG4:
                    return mpeg.HasValue ? mpeg.Value > usedMpeg : true;
                case VideoEncoding.H264:
                    return h264.HasValue ? h264.Value > usedH264 : true;
            }
            return false;
        }

        void AddCounters(
                        VideoEncoding Encoding,
                        ref int usedJpeg,
                        ref int usedMpeg,
                        ref int usedH264)
        {
            switch (Encoding)
            {
                case VideoEncoding.JPEG:
                    usedJpeg++;
                    break;
                case VideoEncoding.MPEG4:
                    usedMpeg++;
                    break;
                case VideoEncoding.H264:
                    usedH264++;
                    break;
            }
        }

        private void RemoveProfileFromList(List<Profile> from, Profile to)
        {
            Profile i = from.FirstOrDefault(P => P.token == to.token);
            if (i != null)
            {
                from.Remove(i);
            }
        }

        protected List<Profile> GetProfilesForMultiStreamingTest(string sourceConfigurationToken,
            int totalNumber,
            int? jpeg,
            int? mpeg,
            int? h264,
            MediaConfigurationChangeLog changeLog)
        {
            LogTestEvent(string.Format("{0} profiles with VideoSourceConfiguration '{1}' are needed for test{2}", totalNumber, sourceConfigurationToken, Environment.NewLine));

            List<Profile> profiles = new List<Profile>();

            Profile[] allProfiles = GetProfiles();
            CheckProfilesList(allProfiles);

            List<Profile> profilesToDelete = new List<Profile>();
            List<Profile> profilesToUpdate = new List<Profile>();
            List<string> usedEncoderConfig = new List<string>();
            int usedJpeg = 0;
            int usedMpeg = 0;
            int usedH264 = 0;
            foreach (Profile profile in allProfiles)
            {
                if (profile.VideoSourceConfiguration != null &&
                    profile.VideoSourceConfiguration.token == sourceConfigurationToken &&
                    profile.VideoEncoderConfiguration != null &&
                    // check encoding-type limitations
                    CheckEncodingType(profile.VideoEncoderConfiguration.Encoding,
                                      jpeg, usedJpeg,
                                      mpeg, usedMpeg, 
                                      h264, usedH264) &&
                    // check encoder configuration limit
                    (usedEncoderConfig.FirstOrDefault(s => s == profile.VideoEncoderConfiguration.token) == null))
                {
                    AddCounters(profile.VideoEncoderConfiguration.Encoding, ref usedJpeg, ref usedMpeg, ref usedH264);
                    usedEncoderConfig.Add(profile.VideoEncoderConfiguration.token);
                    profiles.Add(profile);

                    if (profiles.Count == totalNumber)
                    {
                        break;
                    }
                }
                else
                {
                    if (!(profile.fixedSpecified && profile.@fixed))
                    {
                        // delete newly create rather than old ones...
                        if (changeLog.CreatedProfiles.FirstOrDefault(P => P.token == profile.token) != null)
                        {
                            profilesToDelete.Insert(0, profile);
                        }
                        else
                        {
                            profilesToDelete.Add(profile);
                        }
                    }
                    else
                    {
                        if (changeLog.CreatedProfiles.FirstOrDefault(P => P.token == profile.token) != null)
                        {
                            profilesToUpdate.Insert(0, profile);
                        }
                        else
                        {
                            profilesToUpdate.Add(profile);
                        }
                    }
                }            
            }

            if (profiles.Count < totalNumber)
            {
                int diff = totalNumber - profiles.Count;

                LogTestEvent(string.Format("{0} profiles more are needed{1}", diff, Environment.NewLine));

                // add more profiles
                while (true)
                {
                    Profile nextProfileToBeDeleted = null;
                    Profile nextProfileToBeUpdated = null;
                    if (0 < profilesToDelete.Count)
                    {
                        nextProfileToBeDeleted = profilesToDelete[0];
                    }

                    if (0 < profilesToUpdate.Count)
                    {
                        nextProfileToBeUpdated = profilesToUpdate[0];
                    }

                    bool isNew;
                    Profile deletedProfile = null;
                    string profileName = string.Format("TestProfile{0}", changeLog.LastProfileNumberUsed + 1);
                    Profile newProfile = CreateProfileHandleMaxNvtProfiles(profileName, nextProfileToBeDeleted, nextProfileToBeUpdated, false, out isNew, out deletedProfile, changeLog);

                    /// [AR] remove returned profile from the list of available profiles for delete / change
                    RemoveProfileFromList(profilesToDelete, deletedProfile);
                    RemoveProfileFromList(profilesToDelete, newProfile);
                    RemoveProfileFromList(profilesToUpdate, newProfile);

                    if (!isNew)
                    {
                        Profile backup = Utils.CopyMaker.CreateCopy(newProfile);
                        changeLog.TrackModifiedProfile(backup);

                        RemoveVideoConfigurations(newProfile, changeLog);
                    }

                    changeLog.LastProfileNumberUsed += 1;

                    //----------------- to be deleted, as useless
                    VideoSourceConfiguration[] sourceConfigs = GetCompatibleVideoSourceConfigurations(newProfile.token);
                    CheckConfigurationsList(sourceConfigs, "Video Source Configuration");

                    VideoSourceConfiguration found = sourceConfigs.FirstOrDefault(VSC => VSC.token == sourceConfigurationToken);
                    if (found == null)
                    {
                        break;
                    }
                    //------------------
                    AddVideoSourceConfiguration(newProfile.token, sourceConfigurationToken);
                    newProfile.VideoSourceConfiguration = found;

                    VideoEncoderConfiguration[] encoderConfigurations = GetCompatibleVideoEncoderConfigurations(newProfile.token);
#if true
                    // try to add existing configuration
                    bool ConfigurationFound = false;
                    foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                    {
                        if (CheckEncodingType(vec.Encoding,
                                              jpeg, usedJpeg,
                                              mpeg, usedMpeg, 
                                              h264, usedH264) &&
                            (usedEncoderConfig.FirstOrDefault(s => s == vec.token) == null))
                        {
                            AddVideoEncoderConfiguration(newProfile.token, vec.token);
                            AddCounters(vec.Encoding, ref usedJpeg, ref usedMpeg, ref usedH264);
                            usedEncoderConfig.Add(vec.token);
                            newProfile.VideoEncoderConfiguration = vec;
                            ConfigurationFound = true;
                            break;
                        }
                    }
                    if (!ConfigurationFound)
                    {
                        foreach (VideoEncoderConfiguration vec in encoderConfigurations)
                        {
                            if (usedEncoderConfig.FirstOrDefault(s => s == vec.token) != null)
                            {
                                continue;
                            }
                            // select encoding
                            VideoEncoding enc = VideoEncoding.JPEG;
                            if (jpeg.HasValue && jpeg.Value > usedJpeg)
                            {
                                enc = VideoEncoding.JPEG;
                            };
                            if (mpeg.HasValue && mpeg.Value > usedMpeg)
                            {
                                enc = VideoEncoding.MPEG4;
                            };
                            if (h264.HasValue && h264.Value > usedH264)
                            {
                                enc = VideoEncoding.H264;
                            };
                            VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(vec.token, newProfile.token);
                            vec.Encoding = enc;
                            AdjustVideoEncoderConfiguration(enc, vec, options);
                            switch (enc)
                            {
                                case VideoEncoding.JPEG:
                                    AdjustJPEGVideoEncoderConfiguration(vec, options, true);
                                    break;
                                case VideoEncoding.MPEG4:
                                    AdjustMpeg4VideoEncoderConfiguration(vec, options, true);
                                    break;
                                case VideoEncoding.H264:
                                    AdjustH264VideoEncoderConfiguration(vec, options, true);
                                    break;
                            }
                            OptimizeVEC(null, vec, options);
                            SetVideoEncoderConfiguration(vec, false);
                            AddVideoEncoderConfiguration(newProfile.token, vec.token);
                            AddCounters(vec.Encoding, ref usedJpeg, ref usedMpeg, ref usedH264);
                            usedEncoderConfig.Add(vec.token);
                            newProfile.VideoEncoderConfiguration = encoderConfigurations[0];
                            ConfigurationFound = true;
                            break;
                        }
                    }

                    Assert(ConfigurationFound, "There is no suitable Video Encoder Configuration for just created Profile", "Check if Video Encoder Configuration for Profile is found");
#else
                    if (encoderConfigurations != null && encoderConfigurations.Length > 0)
                    {
                        AddVideoEncoderConfiguration(newProfile.token, encoderConfigurations[0].token);
                        newProfile.VideoEncoderConfiguration = encoderConfigurations[0];
                    }
#endif
                    profiles.Add(newProfile);

                    if (profiles.Count == totalNumber)
                    {
                        break;
                    }
                    //i++;
                }
            }
            else
            {
                LogTestEvent("Use existing profiles for test" + Environment.NewLine);
            }

            return profiles;
        }

        protected bool OptimizeVEC(MediaConfigurationChangeLog changeLog, VideoEncoderConfiguration vec, VideoEncoderConfigurationOptions options)
        {
            VideoResolution[] resolutionsAvailable = null;
            IntRange fpsRange = null;
            switch (vec.Encoding)
            {
                case VideoEncoding.JPEG:
                    if (options.JPEG != null)
                    {
                        resolutionsAvailable = options.JPEG.ResolutionsAvailable;
                        fpsRange = options.JPEG.FrameRateRange;
                    }
                    break;
                case VideoEncoding.H264:
                    if (options.H264 != null)
                    {
                        resolutionsAvailable = options.H264.ResolutionsAvailable;
                        fpsRange = options.H264.FrameRateRange;
                    }
                    break;
                case VideoEncoding.MPEG4:
                    if (options.MPEG4 != null)
                    {
                        resolutionsAvailable = options.MPEG4.ResolutionsAvailable;
                        fpsRange = options.MPEG4.FrameRateRange;
                    }
                    break;
            }

            VideoResolution minimalResolution = null;
            bool updateResolution = false;
            if (resolutionsAvailable != null)
            {
                VideoResolution currentResolution = vec.Resolution;
                foreach (VideoResolution resolution in resolutionsAvailable)
                {
                    if (minimalResolution == null)
                    {
                        minimalResolution = resolution;
                    }
                    else
                    {
                        if (minimalResolution.Height * minimalResolution.Width > resolution.Height * resolution.Width)
                        {
                            minimalResolution = resolution;
                        }
                    }
                }
                updateResolution = (minimalResolution.Width * minimalResolution.Height < currentResolution.Width * currentResolution.Height);
            }

            bool updateFps = false;
            if (fpsRange != null)
            {
                if (vec.RateControl != null)
                {
                    if (vec.RateControl.FrameRateLimit > fpsRange.Min)
                    {
                        updateFps = true;
                    }
                }
                else
                {
                    updateFps = true;
                }
            }

            if (updateResolution || updateFps)
            {
                VideoEncoderConfiguration backup = Utils.CopyMaker.CreateCopy(vec);
                if (changeLog != null)
                {
                    changeLog.TrackModifiedConfiguration(backup);
                }
                if (updateResolution)
                {
                    vec.Resolution = minimalResolution;
                }
                if (updateFps)
                {
                    if (vec.RateControl == null)
                    {
                        vec.RateControl = new VideoRateControl();
                    }
                    vec.RateControl.FrameRateLimit = fpsRange.Min;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Annex A15
        /// </summary>
        /// <param name="videoEncoding">Required video encoding.</param>
        /// <param name="audioEncoding">Required audio encoding.</param>
        /// <param name="changeLog">Change log for restoring configuration.</param>
        /// <param name="videoOptions">Video encoder configuration options for profile.</param>
        /// <param name="audioOptions">Audio encoder configuration options for profile.</param>
        /// <returns></returns>
        protected Profile GetProfileWithAudioAndVideoSupport(VideoEncoding videoEncoding, 
            AudioEncoding audioEncoding, 
            MediaConfigurationChangeLog changeLog,
            out VideoEncoderConfigurationOptions videoOptions, 
            out AudioEncoderConfigurationOptions audioOptions) 
        {
            Profile profile = null;
            videoOptions = null;
            audioOptions = null;

            //1.	Retrieve media profiles by invoking GetProfiles command.
            Profile[] allProfiles = GetProfiles();

            List<Profile> profilesWithConfigurations = new List<Profile>();
            List<Profile> profilesWithVideoEncoder = new List<Profile>();

            if (allProfiles != null)
            {
                //2.	Select media profiles that contain Video Encoder Configuration, Video Source Configuration, 
                // Audio Encoder Configuration and Audio Source Configuration.
                foreach (Profile p in allProfiles)
                {
                    if (p.VideoEncoderConfiguration != null)
                    {
                        if (p.AudioEncoderConfiguration != null)
                        {
                            profilesWithConfigurations.Add(p);
                        }
                        profilesWithVideoEncoder.Add(p);
                    }
                }
            }

            Dictionary<string, VideoEncoderConfigurationOptions> profileVideoEncoderConfigurationOptions = 
                new Dictionary<string, VideoEncoderConfigurationOptions>();
            
            //3.	Retrieve supported video encoder configuration options for a media profile by 
            // invoking GetVideoEncoderConfigurationOptions (media profile token) command. Check whether 
            // the selected media profile supports the required video codec. If there is no required 
            // codec in options skip next step and go to the step 5.
            //4.	Retrieve supported audio encoder configuration options for a media profile by invoking 
            // GetAudioEncoderConfigurationOptions (media profile token) command. Check whether the selected 
            // media profile supports the required audio codec.
            //5.	Repeat steps 3-4 for all media profiles selected on step 2 till a media profile with 
            // the required video and audio codec support is found. If such profile exists skip other steps 
            // and use selected profile.
            foreach (Profile p in profilesWithConfigurations)
            {
                VideoEncoderConfigurationOptions videoEncoderOptions = 
                    GetVideoEncoderConfigurationOptions(null, p.token);

                profileVideoEncoderConfigurationOptions.Add(p.token, videoEncoderOptions);

                if (OptionsAllowEncoding(videoEncoderOptions, videoEncoding))
                {
                    AudioEncoderConfigurationOptions audioEncoderOptions = 
                        GetAudioEncoderConfigurationOptions(null, p.token);

                    if (OptionsAllowEncoding(audioEncoderOptions, audioEncoding))
                    {
                        profile = p;
                        videoOptions = videoEncoderOptions;
                        audioOptions = audioEncoderOptions;
                        break;
                    }                
                }            
            }

            if (profile != null)
            {
                LogTestEvent(string.Format("Profile with token '{0}' contains encoder configurations which can be changed to use specified encodings. Use this profile for test.{1}", profile.token,  Environment.NewLine));
                // out options parameters have been initialized when codecs support was being checked
                return profile;
            }

            // 6.	Select media profiles that contain Video Encoder Configuration, Video Source Configuration. 
            // If there is no such profile, create profile with using procedure described in Annex A.16.
            Profile videoProfile = null;
            
            foreach (Profile p in profilesWithVideoEncoder)
            {
                VideoEncoderConfigurationOptions videoEncoderOptions = null;
                if (profileVideoEncoderConfigurationOptions.ContainsKey(p.token))
                {
                    videoEncoderOptions = profileVideoEncoderConfigurationOptions[p.token];
                }
                else
                {
                    videoEncoderOptions = GetVideoEncoderConfigurationOptions(null, p.token);
                }

                if (OptionsAllowEncoding(videoEncoderOptions, videoEncoding))
                {
                    videoOptions = videoEncoderOptions;
                    videoProfile = p;
                    LogTestEvent(string.Format("Update AudioEncoderConfiguration for profile with token '{0}'{1}",
                        p.token, Environment.NewLine));
                    break;
                }
            }

            // covering "If there is no such profile, create profile with using procedure described in Annex A.16."
            if (videoProfile == null)
            {
                LogTestEvent(string.Format("It is impossible to find profile with suitable Video Encoder Configuration. Create new one.{0}",
                    Environment.NewLine));

                Profile profileToDelete = null;
                Profile profileToUpdate = null;

                if (allProfiles != null)
                {
                    profileToUpdate = allProfiles[0];
                    foreach (Profile p in allProfiles)
                    {
                        if (!(p.fixedSpecified && p.@fixed))
                        {
                            profileToDelete = p;
                            break;
                        }                    
                    }                
                }

                videoProfile = CreateProfileForVideoStreaming(videoEncoding, profileToDelete, profileToUpdate, out videoOptions, changeLog);

                LogTestEvent(string.Format("Configuring Video Encoder succeeded. Configure Audio...{0}", Environment.NewLine));
            }
            else
            {
                Profile backupCopy = Utils.CopyMaker.CreateCopy(videoProfile);
                changeLog.TrackModifiedProfile(backupCopy);
            }
            //7.	Retrieve supported video encoder configuration options for a media profile by invoking 
            // GetVideoEncoderConfigurationOptions (media profile token) command. Check whether the selected 
            // media profile supports the required video codec. If there is no required codec in options 
            // skip next step and go to the step 8. If there is no such profile, create profile with using 
            // procedure described in Annex A.16.


            //8.	Remove audio encoder configuration with using RemoveAudioEncoderConfigurations command 
            // if it is included in profile.
            if (videoProfile.AudioEncoderConfiguration != null)
            {
                RemoveAudioEncoderConfiguration(videoProfile.token);
            }
            
            //9.	Remove audio source configuration with using RemoveAudioSourceConfigurations command 
            // if it is included in profile.
            if (videoProfile.AudioSourceConfiguration != null)
            {
                RemoveAudioSourceConfiguration(videoProfile.token);
            }
                                    
            //10.	ONVIF Client will invoke GetCompatibleAudioSourceConfigurationsRequest message (media 
            // profile token) to retrieve compatible audio source configurations list.

            AudioSourceConfiguration[] compatibleSourceConfigurations =
                GetCompatibleAudioSourceConfigurations(videoProfile.token);
            
            //11.	Verify the GetCompatibleAudioSourceConfigurationsResponse message from the DUT. If 
            // GetCompatibleVideoSourceConfigurationsResponse message contains empty list skip steps 
            // 12-20. It is not possible to create required profile in this case.

            Assert(compatibleSourceConfigurations != null && compatibleSourceConfigurations.Length>0,
                "The DUT returned no compatible Audio Source configurations. Profile with required properties can be configured.", 
                "Check if any compatible Audio Source Configurations returned");

            bool profileConfigured = false;
            foreach (AudioSourceConfiguration asc in compatibleSourceConfigurations)
            {                
                //12.	ONVIF Client will invoke AddAudioSourceConfigurationRequest message (media profile 
                // token, audio source configuration from GetCompatibleAudioSourceConfigurationsResponse message) 
                // to add audio source configuration to profile.
                //13.	Verify the AddAudioSourceConfigurationResponse message from the DUT.

                AddAudioSourceConfiguration(videoProfile.token, asc.token);

                //14.	ONVIF Client will invoke GetCompatibleAudioEncoderConfigurationsRequest message (media 
                // profile token) to retrieve compatible audio encoder configurations list.

                AudioEncoderConfiguration[] compatibleAudioEncoderConfigurations =
                    GetCompatibleAudioEncoderConfigurations(videoProfile.token);

                if (compatibleAudioEncoderConfigurations != null)
                {
                    //15.	Verify the GetCompatibleAudioEncoderConfigurationsResponse message from the DUT. If 
                    // GetCompatibleVideoEncoderConfigurationsResponse message contains empty list skip steps 16-19 
                    // and go to the step 20.

                    foreach (AudioEncoderConfiguration aec in compatibleAudioEncoderConfigurations)
                    {
                        //16.	ONVIF Client will invoke AddAudioEncoderConfigurationRequest message (media profile 
                        // token, audio encoder configuration from GetCompatibleAudioEncoderConfigurationsResponse 
                        // message) to add audio encoder configuration to profile.
                        //17.	Verify the AddAudioEncoderConfigurationResponse message from the DUT.

                        AddAudioEncoderConfiguration(videoProfile.token, aec.token);
                        
                        //18.	Retrieve supported audio encoder configuration options for a media profile by invoking 
                        // GetAudioEncoderConfigurationOptions (media profile token) command. Check whether the selected 
                        // media profile supports the required audio codec.

                        AudioEncoderConfigurationOptions audioEncoderOptions =
                            GetAudioEncoderConfigurationOptions(null, videoProfile.token);
                        if (OptionsAllowEncoding(audioEncoderOptions, audioEncoding))
                        {
                            audioOptions = audioEncoderOptions;
                            videoProfile.AudioSourceConfiguration = asc;
                            videoProfile.AudioEncoderConfiguration = aec;
                            profile = videoProfile;
                            profileConfigured = true;
                        }

                        //19.	Repeat steps 16-18 for all audio encoder configurations received on step 15 till a media 
                        // profile with the required video and audio codec support is created (previously remove audio 
                        // encoder configuration from the profile). If such profile was created skip other steps and use 
                        // this profile.                
                    }
                    if (!profileConfigured)
                    {
                        LogTestEvent(string.Format("No Audio Encoder configurations compatible with Source Configuration '{0}' allow setting {1} Audio encoding{2}",
                            asc.token, audioEncoding, Environment.NewLine));

                        RemoveAudioEncoderConfiguration(videoProfile.token);
                    }
                }

                if (profileConfigured)
                {
                    break;
                }

                //20.	Repeat steps 12-19 for all audio source configurations received on step 11 till a 
                // media profile with the required video and audio codec support is created (previously remove 
                // audio encoder configuration and audio source configuration from the profile). If such profile was created skip other steps and use this profile.
            }
            
            Assert(profileConfigured, 
                "Profile with required Video and Audio encoders has not been created",
                "Check if profile has been configured");

            return profile;
        }

        /// <summary>
        /// Annex A16
        /// </summary>
        /// <param name="changeLog"></param>
        /// <returns></returns>
        Profile CreateProfileForVideoStreaming(VideoEncoding videoEncoding, 
            Profile nonFixedProfile,
            Profile profileForUpdate,
            out VideoEncoderConfigurationOptions videoEncoderOptions,
            MediaConfigurationChangeLog changeLog)
        {
            videoEncoderOptions = null;
            Profile profile = null;
            //1.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) to create 
            // new profile.
            //2.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) 
            // or SOAP 1.2 fault message (Action/MaxNVTProfiles) from the DUT. If CreateProfileResponse 
            // message was received go to the step 7.
            //3.	ONVIF Client will invoke DeleteProfileRequest message (ProfileToken = “Profile2”, 
            // where “Profile2” is token of profile with fixed=”false”) to remove profile. If there are 
            // no profiles with fixed=”false” skip other steps (this will means that it is not possible 
            // to find or create profile for specified video encoder configuration).
            //4.	Verify the DeleteProfilesResponse message from the DUT.
            //5.	ONVIF Client will invoke CreateProfileRequest message (Name = “TestProfile1”) to 
            // create new profile.
            //6.	Verify the CreateProfileResponse message (token = “ProfileToken1”, fixed=”false”) from 
            // the DUT.

            {
                string profileName = "TestProfile1";
                bool isNew;
                Profile deletedProfile = null;
                profile = CreateProfileHandleMaxNvtProfiles(profileName, nonFixedProfile, profileForUpdate, false, out isNew, out deletedProfile, changeLog);
                if (!isNew)
                {
                    Profile backup = Utils.CopyMaker.CreateCopy(profile);
                    changeLog.TrackModifiedProfile(backup);

                    RemoveVideoConfigurations(profile, changeLog);
                }            
            }

            LogTestEvent(
                string.Format("Add Video Source configuration and Video Encoder Configuration which allowing to set {0} Video encoding{1}",
                videoEncoding, Environment.NewLine));

            //7.	ONVIF Client will invoke GetCompatibleVideoSourceConfigurationsRequest message 
            // (ProfileToken = “ProfileToken1”) to retrieve compatible video source configurations list.
            VideoSourceConfiguration[] sourceConfigurations =
                GetCompatibleVideoSourceConfigurations(profile.token);

            //8.	Verify the GetCompatibleVideoSourceConfigurationsResponse message from the DUT. If 
            // GetCompatibleVideoSourceConfigurationsResponse message contains empty list skip other steps 
            // (this will means that it is not possible to find or create profile for specified video 
            // encoder configuration).
            CheckConfigurationsList(sourceConfigurations, "Video Source Configuration");

            bool profileConfigured = false;
            foreach (VideoSourceConfiguration vsc in sourceConfigurations)
            {
                //9.	ONVIF Client will invoke AddVideoSourceConfigurationRequest message (ProfileToken = 
                // “ProfileToken1”, ConfigurationToken = “VSCToken1”, where “VSCToken1” is video source 
                // configuration from GetCompatibleVideoSourceConfigurationsResponse message) to add video 
                // source configuration to profile.
                //10.	Verify the AddVideoSourceConfigurationResponse message from the DUT.

                AddVideoSourceConfiguration(profile.token, vsc.token);

                //11.	ONVIF Client will invoke GetCompatibleVideoEncoderConfigurationsRequest message 
                // (ProfileToken = “ProfileToken1”) to retrieve compatible video encoder configurations list.

                VideoEncoderConfiguration[] compatibleEncoderConfigurations =
                    GetCompatibleVideoEncoderConfigurations(profile.token);

                //12.	Verify the GetCompatibleVideoEncoderConfigurationsResponse message from the DUT. If 
                // GetCompatibleVideoEncoderConfigurationsResponse message does not contains specified video 
                // encoder configuration repeat steps 9-12 for other video source configuration from 
                // GetCompatibleVideoSourceConfigurationsResponse message. If there is no video source 
                // configuration that was not used in steps 9-12, skip other steps (this will means that it 
                // is not possible to find or create profile for specified video encoder configuration).
                if (compatibleEncoderConfigurations != null)
                {
                    foreach (VideoEncoderConfiguration vec in compatibleEncoderConfigurations)
                    {
                        //13.	ONVIF Client will invoke AddVideoEncoderConfigurationRequest message (ProfileToken = 
                        // “ProfileToken1”, ConfigurationToken = “VECToken1”, where “VECToken1” is video encoder 
                        // configuration from GetCompatibleVideoEncoderConfigurationsResponse message) to add video 
                        // encoder configuration to profile.

                        AddVideoEncoderConfiguration(profile.token, vec.token);

                        //14.	Retrieve supported video encoder configuration options for a media profile by invoking 
                        // GetVideoEncoderConfigurationOptions (media profile token) command. Check whether the selected 
                        // media profile supports the required video codec.

                        VideoEncoderConfigurationOptions encoderOptions =
                            GetVideoEncoderConfigurationOptions(null, profile.token);

                        if (OptionsAllowEncoding(encoderOptions, videoEncoding))
                        {
                            profileConfigured = true;
                            videoEncoderOptions = encoderOptions;
                            profile.VideoSourceConfiguration = vsc;
                            profile.VideoEncoderConfiguration = vec;
                            break;
                        }
                        //15.	Repeat steps 13-14 for all video encoder configurations received on step 12 till a 
                        // media profile with the required video codec support is created (previously remove video 
                        // encoder configuration from the profile). If such profile was created skip other steps and 
                        // use this profile.
                    }
                    if (!profileConfigured)
                    {
                        LogTestEvent(string.Format("No Video Encoder configurations compatible with Source Configuration '{0}' allow setting {1} Video encoding{2}",
                            vsc.token, videoEncoding, Environment.NewLine));
                        RemoveVideoEncoderConfiguration(profile.token);
                    }
                }

                if (profileConfigured)
                {
                    break;
                }

                //16.	Repeat steps 9-15 for all video source configurations received on step 8 till a media 
                // profile with the required video codec support is created (previously remove video encoder 
                // configuration and video source configuration from the profile). If such profile was created 
                // skip other steps and use this profile.
            }

            Assert(profileConfigured,
                string.Format("It is impossible to create profile with {0} Video encoding support", videoEncoding),
                "Check if profile configuration succeeded");

            return profile;
        }


        #region Multicast Utility

        protected void SetMulticast(MulticastConfiguration multicast,
            IPType addressType, string address, int port)
        { 
            multicast.TTL = 1;
            multicast.Address.Type = addressType;
            if (addressType == IPType.IPv4)
            {
                multicast.Address.IPv6Address = null;
                multicast.Address.IPv4Address = address;
            }
            else // IPType.IPv6
            {
                multicast.Address.IPv4Address = null;
                multicast.Address.IPv6Address = address;
            }
            multicast.Port = port;
        
        }

        protected bool IsValidMulticastConfiguration(MulticastConfiguration multicast, IPType addressType)
        {
            if (multicast == null)
            {
                return false;
            }

            if (multicast.Address == null)
            {
                return false;
            }
            else
            {
                if (multicast.Address.Type != addressType)
                {
                    return false;
                }
                if (multicast.Address.Type == IPType.IPv4)
                {
                    return (multicast.Address.IPv6Address == null && 
                            !string.IsNullOrEmpty(multicast.Address.IPv4Address) && 
                            multicast.TTL > 0 && 
                            multicast.Port > 0);
                }
                else // IPType.IPv6
                {
                    return (multicast.Address.IPv4Address == null &&
                            !string.IsNullOrEmpty(multicast.Address.IPv6Address) &&
                            multicast.TTL > 0 &&
                            multicast.Port > 0);
                }
            }        
        }

        // from 224.0.0.0 to 239.255.255.255, 
        // but from 224.0.0.0 to 224.0.0.255 are reserved
        protected string GetMulticastAddress()
        {
            // generate ip from 225.0.0.0 to 239.255.255.255
            Random MulticastIPv4 = new Random();
            int oct1 = MulticastIPv4.Next(225, 240);
            int oct2 = MulticastIPv4.Next(0, 256);
            int oct3 = MulticastIPv4.Next(0, 256);
            int oct4 = MulticastIPv4.Next(0, 256);

            string randomMulticastIPv4 = oct1 + "." + oct2 + "." + oct3 + "." + oct4;

            return randomMulticastIPv4;//return "224.0.0.1";
        }

        // from 224.0.0.0 to 239.255.255.255, 
        // but from 224.0.0.0 to 224.0.0.255 are reserved
        protected string GetMulticastAddress2(List<string> usedIPv4)
        {
            // generate ip from 225.0.0.0 to 239.255.255.255
            Random MulticastIPv4 = new Random();

            string newIPv4 = "";
            string randomMulticastIPv4 = "";

            while (newIPv4 == randomMulticastIPv4)
            {
                int oct1 = MulticastIPv4.Next(225, 240);
                int oct2 = MulticastIPv4.Next(0, 256);
                int oct3 = MulticastIPv4.Next(0, 256);
                int oct4 = MulticastIPv4.Next(0, 256);

                randomMulticastIPv4 = oct1 + "." + oct2 + "." + oct3 + "." + oct4;

                newIPv4 = usedIPv4.Find(
                delegate(string ip)
                {
                    return ip == randomMulticastIPv4;
                }
                );
            }

            return randomMulticastIPv4;
        }

        // from 224.0.0.0 to 239.255.255.255, 
        // but from 224.0.0.0 to 224.0.0.255 are reserved
        protected string GetMulticastAddress3(List<string> usedIPv4)
        {
            // generate ip from 239.0.0.0 and sequential increases by 1
            // e.g. 239.0.0.0, 239.0.0.1, ... , 239.255.255.255
 
            string MulticastIPv4 = "";

            if (usedIPv4.Count <= 255)
            {
                MulticastIPv4 = string.Format("239.0.0.{0}", usedIPv4.Count);
            }
            else if (usedIPv4.Count <= 510)
            {
                MulticastIPv4 = string.Format("239.0.{0}.{1}", usedIPv4.Count - 255, 255);
            }
            else if (usedIPv4.Count <= 765)
            {
                MulticastIPv4 = string.Format("239.{0}.{1}.{2}", usedIPv4.Count - 510, 255, 255);
            }

            return MulticastIPv4;
        }

        protected int GetMulticastPort()
        {
            return 1234;
        }

        protected int GetMulticastPort2(List<int> usedPorts)
        {
            Random MulticastPort = new Random();

            //int newPort = 0;
            int randomMulticastPort = 0;

            while ( true)
            {
                randomMulticastPort = (usedPorts.Count == 0 ? 1234 : usedPorts[usedPorts.Count - 1] + 2);
                if (!usedPorts.Contains(randomMulticastPort))
                {
                    break;
                }
            }

            return randomMulticastPort;
        }

        /// <summary>
        /// Copied with some changes from RTSSTestSuite.cs
        /// ToDo : delete unnecessary methods, if classes hierarchy is changed
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="addressType"></param>
        protected void SetMulticastSettings(Profile profile, 
            IPType addressType, 
            MediaConfigurationChangeLog changeLog,
            string addressAudio,
            int portAudio,
            string addressVideo,
            int portVideo,
            string addressMetadata,
            int portMetadata)
        {
            if (profile.VideoEncoderConfiguration != null)
            {
                bool update = false;
                // create backup copy
                VideoEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.VideoEncoderConfiguration);
                if (profile.VideoEncoderConfiguration.Multicast == null)
                {
                    update = true;
                    profile.VideoEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                else 
                {
                    update = !IsValidMulticastConfiguration(profile.VideoEncoderConfiguration.Multicast, addressType);   
                }
                if (update)
                {
                    changeLog.ModifiedVideoEncoderConfigurations.Add(configCopy);

                    SetMulticast(profile.VideoEncoderConfiguration.Multicast, addressType, addressVideo, portVideo);
                    SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, false, true);
                }
            }
            if (profile.AudioEncoderConfiguration != null)
            {
                bool update = false;
                // create backup copy
                AudioEncoderConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.AudioEncoderConfiguration);

                if (profile.AudioEncoderConfiguration.Multicast == null)
                {
                    update = true;
                    profile.AudioEncoderConfiguration.Multicast = new MulticastConfiguration();
                }
                else 
                {
                    update = !IsValidMulticastConfiguration(profile.AudioEncoderConfiguration.Multicast, addressType);
                }
                if (update)
                {
                    changeLog.TrackModifiedConfiguration(configCopy);

                    SetMulticast(profile.AudioEncoderConfiguration.Multicast, addressType, addressAudio, portAudio);
                    SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, false, true);
                }
            }
            if (profile.MetadataConfiguration != null)
            {
                bool update = false;
                MetadataConfiguration configCopy = Utils.CopyMaker.CreateCopy(profile.MetadataConfiguration);

                if (profile.MetadataConfiguration.Multicast == null)
                {
                    update = true;
                    profile.MetadataConfiguration.Multicast = new MulticastConfiguration();
                }
                else
                {
                    update = !IsValidMulticastConfiguration(profile.MetadataConfiguration.Multicast, addressType);
                }
                if (update)
                {
                    changeLog.ModifiedMetadataConfigurations.Add(configCopy);

                    SetMulticast(profile.MetadataConfiguration.Multicast, addressType, addressMetadata, portMetadata);
                    SetMetadataConfiguration(profile.MetadataConfiguration, false);
                }
            }
        }
        
        protected void CheckMulticastSettings(MulticastConfiguration expected, MulticastConfiguration actual)
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder("Multicast configurations are different:");

            if (expected != null && actual != null)
            {

                if (expected.Address != null && actual.Address != null)
                {
                    if (expected.Address.Type != actual.Address.Type)
                    {
                        ok = false;
                        sb.Append(string.Format("Address.Type is different: expected {0}, actual {1}{2}",
                            expected.Address.Type, actual.Address.Type, Environment.NewLine));
                    }

                    if (expected.Address.Type == IPType.IPv4)
                    {
                        if (expected.Address.IPv4Address != actual.Address.IPv4Address)
                        {
                            ok = false;
                            sb.Append(string.Format("Address.IPv4Address is different: expected {0}, actual {1}{2}",
                                expected.Address.IPv4Address, actual.Address.IPv4Address, Environment.NewLine));
                        }
                    }

                    if (expected.Address.Type == IPType.IPv6)
                    {
                        if (expected.Address.IPv6Address != actual.Address.IPv6Address)
                        {
                            ok = false;
                            sb.Append(string.Format("Address.IPv6Address is different: expected {0}, actual {1}{2}",
                                expected.Address.IPv6Address, actual.Address.IPv6Address, Environment.NewLine));
                        }
                    }
                }
                else
                {
                    if (expected.Address != null)
                    {
                        ok = false;
                        sb.AppendLine("Address not found in actual configuration");
                    }
                    if (actual.Address != null)
                    {
                        ok = false;
                        sb.AppendLine("Address is not specified in expected configuration");
                    }
                }
                if (expected.Port != actual.Port)
                {
                    ok = false;
                    sb.Append(string.Format("Port is different: expected {0}, actual {1}{2}",
                                expected.Port, actual.Port, Environment.NewLine));
                }
            }

            else 
            {
                if (expected != null)
                {
                    ok = false;
                    sb.AppendLine("Actual multicast settings are empty");
                }
                if (actual != null)
                {
                    ok = false;
                    sb.AppendLine("Multicast settings are not expected to be present");
                }
            }

            Assert(ok, sb.ToStringTrimNewLine(), "Compare expected Multicast configuration and actual");
        }

        #endregion

        #region Configuration lists

        public void CheckConfigurationsList<T>(T[] list, string entityName)
        {
            Assert(list != null && list.Length > 0,
                string.Format("The DUT returned no {0}s", entityName),
                string.Format("Check if there are {0}s at the DUT", entityName));
        }

        protected void CheckVideoEncoderConfigurationsList(VideoEncoderConfiguration[] configurations)
        {
            CheckConfigurationsList(configurations, "Video Encoder Configuration");        
        }

        protected void CheckAudioEncoderConfigurationsList(AudioEncoderConfiguration[] configurations)
        {
            CheckConfigurationsList(configurations, "Audio Encoder Configuration");
        }

        protected void CheckProfilesList(Profile[] profiles)
        {
            Assert(profiles != null && profiles.Length > 0,
                "The DUT return no profiles", "Check if the DUT returned any profiles");
        }

        #endregion

        #region utils


        protected bool OptionsAllowEncoding(AudioEncoderConfigurationOptions opt, AudioEncoding encoding)
        {
            if (opt.Options != null)
            {
                return (opt.Options.Where(o => o.Encoding == encoding).FirstOrDefault() != null);
            }
            else
            {
                return false;
            }
        }
                        
        // copied from MediaVideoConfigurationTestSuite
        protected void FindResolutions(IEnumerable<VideoResolution> videoResolutions,
            out VideoResolution highest, out VideoResolution lowest, out VideoResolution median)
        {
            BeginStep("Find highest and lowest resolutions for further testing");

            List<VideoResolution> resolutions = videoResolutions.OrderBy(VR => VR.Height * VR.Width).ToList();
            lowest = resolutions.First();
            highest = resolutions.Last();

            int cnt = resolutions.Count;
            int middle = (cnt + 1) / 2 - 1;
            median = resolutions[middle];

            LogStepEvent(string.Format("Highest resolution: width ={0}, height = {1} ", highest.Width, highest.Height));
            LogStepEvent(string.Format("Median resolution: width ={0}, height = {1} ", median.Width, median.Height));
            LogStepEvent(string.Format("Lowest resolution: width ={0}, height = {1} ", lowest.Width, lowest.Height));

            StepPassed();

        }
                
        protected void ValidateAudioEncoderConfigurationOption(AudioEncoderConfigurationOption options)
        {
            StringBuilder sb = 
                new StringBuilder(string.Format("Options for Encoding={0} are not valid:", options.Encoding));
            bool ok = true;

            if (options.BitrateList == null || options.BitrateList.Length == 0)
            {
                ok = false;
                sb.AppendLine("Bitrate list is empty");
            }

            if (options.SampleRateList == null || options.SampleRateList.Length == 0)
            {
                ok = false;
                sb.AppendLine("SampleRate list is empty");
            }

            Assert(ok, sb.ToStringTrimNewLine(), "Check that Audio Encoder configuration options are valid");

        }


        #endregion


    }
}
