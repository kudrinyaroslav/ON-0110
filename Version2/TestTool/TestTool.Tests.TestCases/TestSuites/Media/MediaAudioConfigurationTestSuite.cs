///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Collections.Generic;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;
using System.Linq;
using System.ServiceModel;
using TestTool.Tests.Common.TestBase;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaAudioConfigurationTestSuite : Base.MediaTest
    {
        private const string PATH_3_1 = "Media Configuration\\Audio Configuration\\General";

        public MediaAudioConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.22",
            Id = "3-1-22",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] {Feature.MediaService, Feature.Audio},
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioSourceConfigurations, 
                Functionality.GetCompatibleAudioSourceConfigurations, 
                Functionality.GetAudioSourceConfiguration, 
                Functionality.AddAudioSourceConfiguration,
                Functionality.GetAudioSourceConfigurationOptions})]
        public void AudioSourceConfiguration()
        {
            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;
            Profile profile = null;
            RunTest(
                () =>
                {
                    profile = CreateProfileByAnnex3("testprofileX", null, out deletedProfile, out createdProfile, out modifiedProfile);
                    string reason;
                    Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);

                    AudioSource[] sources = GetAudioSources();
                    Assert(ValidateAudioSources(sources, out reason), reason, Resources.StepValidatingAudioSources_Title);

                    AudioSourceConfiguration[] configs = GetAudioSourceConfigurations();
                    Assert(ValidateAudioSourceConfigs(configs, out reason), reason, Resources.StepValidatingAudioSources_Title);

                    configs = GetCompatibleAudioSourceConfigurations(profile.token);
                    Assert(ValidateAudioSourceConfigs(configs, out reason), reason, Resources.StepValidatingAudioSources_Title);

                    AudioSourceConfiguration config = configs[0];
                    AddAudioSourceConfiguration(profile.token, config.token);

                    AudioSourceConfigurationOptions options = GetAudioSourceConfigurationOptions(null, config.token);

                    string sourceToken = config.SourceToken;
                    config.SourceToken = "InvalidToken";
                    //TODO make sure that token is invalid
                    string details = string.Format("Setting invalid configuration (/AudioSourceConfiguration/SourceToken = '{0}')", config.SourceToken);
                    SetInvalidAudioSourceConfiguration(config, false, details);

                    config.SourceToken = sourceToken;
                    SetAudioSourceConfiguration(config, false);

                    AudioSourceConfiguration newConfig = GetAudioSourceConfiguration(config.token);
                    Assert(EqualConfigurations(config, newConfig, out reason), 
                        string.Format(Resources.ErrorAudioSourceConfigNotEqual_Format, reason),
                        Resources.StepCompareAudioSourceConfigs_Title);

                    RemoveAudioSourceConfiguration(profile.token);
                },
                () =>
                {
                    RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
                });
        }
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.23",
            Id = "3-1-23",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioEncoderConfigurations, 
                Functionality.GetCompatibleAudioEncoderConfigurations, 
                Functionality.AddAudioEncoderConfiguration})]
        public void AudioEncoderConfiguration()
        {
            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;
            Profile profile = null;
            RunTest<Profile>(
               new Backup<Profile>(() => { return null; }),
               () =>
               {
                   profile = CreateProfileByAnnex3("testprofileX", null, out deletedProfile, out createdProfile, out modifiedProfile);
                   string reason;
                   Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);

                   AudioSourceConfiguration[] sourceConfigs = GetAudioSourceConfigurations();
                   Assert(ValidateAudioSourceConfigs(sourceConfigs, out reason), reason, Resources.StepValidatingAudioSources_Title);

                   AudioSourceConfiguration sourceConfig = sourceConfigs[0];
                   AddAudioSourceConfiguration(profile.token, sourceConfig.token);

                   AudioEncoderConfiguration[] configs = GetAudioEncoderConfigurations();
                   Assert(ValidateAudioEncoderConfigs(configs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                   configs = GetCompatibleAudioEncoderConfigurations(profile.token);
                   Assert(ValidateAudioEncoderConfigs(configs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                   AudioEncoderConfiguration encoderConfig = configs[0];
                   AddAudioEncoderConfiguration(profile.token, encoderConfig.token);

                   RemoveAudioEncoderConfiguration(profile.token);
                   RemoveAudioSourceConfiguration(profile.token);
               },
               (param) =>
               {
                    RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
               });
        }
        
        [Test(Name = "G.711 AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.14",
            Id = "3-1-14",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioEncoderConfigurations, 
                Functionality.GetAudioEncoderConfiguration, 
                Functionality.SetAudioEncoderConfiguration})]
        public void AudioEncoderG711Configuration()
        {
            RunTest(() =>
            {
                string reason;
                AudioEncoderConfiguration[] configs = GetAudioEncoderConfigurations();
                Assert(ValidateAudioEncoderConfigs(configs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                AudioEncoderConfigurationOption options = null;
                AudioEncoderConfiguration config = GetAudioEncoderConfiguration(configs, AudioEncoding.G711, out options);

                config.Encoding = AudioEncoding.G711;
                config.SampleRate = GetInvalidSampleRate(options.SampleRateList);
                config.Bitrate = options.BitrateList[0];
                //config.SessionTimeout = "PT600S";//send the same as received
                string details = string.Format("Setting invalid configuration (/AudioEncoderConfiguration/SampleRate = {0})", config.SampleRate);
                SetInvalidAudioEncoderConfiguration(config, false, details);

                config.SampleRate = options.SampleRateList[0];
                SetAudioEncoderConfiguration(config, false);

                AudioEncoderConfiguration newConfig = GetAudioEncoderConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorAudioEncoderConfigNotEqual_Format, reason), 
                    Resources.StepCompareAudioEncoderConfigs_Title);
            });
        }
        
        [Test(Name = "G.726 AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.15",
            Id = "3-1-15",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures =  new Feature[]{Feature.MediaService, Feature.Audio, Feature.G726},
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioEncoderConfigurations, 
                Functionality.GetAudioEncoderConfiguration, 
                Functionality.SetAudioEncoderConfiguration})]
        public void AudioEncoderG726Configuration()
        {
            RunTest(() =>
            {
                string reason;
                AudioEncoderConfiguration[] configs = GetAudioEncoderConfigurations();
                Assert(ValidateAudioEncoderConfigs(configs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                AudioEncoderConfigurationOption options = null;
                AudioEncoderConfiguration config = GetAudioEncoderConfiguration(configs, AudioEncoding.G726, out options);

                config.Encoding = AudioEncoding.G726;
                config.SampleRate = GetInvalidSampleRate(options.SampleRateList);
                config.Bitrate = options.BitrateList[0];
                //config.SessionTimeout = "PT600S";//send the same as received
                string details = string.Format("Setting invalid configuration (/AudioEncoderConfiguration/SampleRate = {0})", config.SampleRate);
                SetInvalidAudioEncoderConfiguration(config, false, details);

                config.SampleRate = options.SampleRateList[0];
                SetAudioEncoderConfiguration(config, false);

                AudioEncoderConfiguration newConfig = GetAudioEncoderConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorAudioEncoderConfigNotEqual_Format, reason),
                    Resources.StepCompareAudioEncoderConfigs_Title);
            });
        }
        
        [Test(Name = "AAC AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.16",
            Id = "3-1-16",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] {Feature.MediaService, Feature.Audio, Feature.AAC},
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioEncoderConfigurations, 
                Functionality.GetAudioEncoderConfiguration, 
                Functionality.SetAudioEncoderConfiguration})]
        public void AudioEncoderAACConfiguration()
        {
            RunTest(() =>
            {
                string reason;
                AudioEncoderConfiguration[] configs = GetAudioEncoderConfigurations();
                Assert(ValidateAudioEncoderConfigs(configs, out reason), reason, Resources.StepValidatingAudioEncoders_Title);

                AudioEncoderConfigurationOption options = null;
                AudioEncoderConfiguration config = GetAudioEncoderConfiguration(configs, AudioEncoding.AAC, out options);

                config.Encoding = AudioEncoding.AAC;
                config.SampleRate = GetInvalidSampleRate(options.SampleRateList);
                config.Bitrate = options.BitrateList[0];
                //config.SessionTimeout = "PT600S";//send the same as received
                string details = string.Format("Setting invalid configuration (/AudioEncoderConfiguration/SampleRate = {0})", config.SampleRate);
                SetInvalidAudioEncoderConfiguration(config, false, details);

                config.SampleRate = options.SampleRateList[0];
                SetAudioEncoderConfiguration(config, false);

                AudioEncoderConfiguration newConfig = GetAudioEncoderConfiguration(config.token);
                Assert(EqualConfigurations(config, newConfig, out reason), 
                    string.Format(Resources.ErrorAudioEncoderConfigNotEqual_Format, reason),
                    Resources.StepCompareAudioEncoderConfigs_Title);
            });
        }

        int GetInvalidSampleRate(int[] sampleRates)
        {
            return sampleRates.Max() + 1;
        }

        /// <summary>
        /// Author: Anna Tarasova
        /// </summary>
        #region Audio Source


        [Test(Name = "GET AUDIO SOURCE CONFIGURATION – INVALID CONFIGURATIONTOKEN",
            Order = "03.01.17",
            Id = "3-1-17",
            Category = Category.MEDIA,
            Path = PATH_3_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioSourceConfiguration})]
        public void AudioSourceConfigurationInvalidTokenTest()
        {
            RunTest(() =>
            {
                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "No Audio source configurations returned",
                    "Check if the DUT returned audio source configurations");

                List<string> tokens = new List<string>();
                foreach (AudioSourceConfiguration configuration in configurations)
                {
                    tokens.Add(configuration.token);
                }

                string token = tokens.GetNonMatchingString();

                RunStep(
                    () => { Client.GetAudioSourceConfiguration(token); },
                    "Get audio source configuration - negative test",
                    "Sender/InvalidArgVal/NoConfig",
                    true);

                DoRequestDelay();
            });
        }


        [Test(Name = "GET AUDIO SOURCE CONFIGURATION OPTIONS",
            Order = "03.01.07",
            Id = "3-1-7",
            Category = Category.MEDIA,
            Path = PATH_3_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioSourceConfigurationOptions })]
        public void GetAudioSourceConfigurationOptionsTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null && profiles.Length > 0,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null && configurations.Length > 0,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                AudioSourceConfigurationOptions options =
                    GetAudioSourceConfigurationOptions(null, configurations[0].token,
                    string.Format("Get Audio source configuration options for configuration [token='{0}']", configurations[0].token));

                options = GetAudioSourceConfigurationOptions(profiles[0].token, null,
                    string.Format("Get Audio source configuration options for profile [token='{0}']", profiles[0].token));

                options = GetAudioSourceConfigurationOptions(profiles[0].token, configurations[0].token,
                    string.Format("Get Audio source configuration options for configuration [token='{0}'] and profile [token = '{1}']", configurations[0].token, profiles[0].token));

                options = GetAudioSourceConfigurationOptions(null, null, "Get Audio source configuration options (empty message)");


            });
        }


        [Test(Name = "GET AUDIO SOURCE CONFIGURATION OPTIONS – INVALID PROFILETOKEN",
            Order = "03.01.18",
            Id = "3-1-18",
            Category = Category.MEDIA,
            Path = PATH_3_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioSourceConfigurationOptions })]
        public void GetAudioSourceConfigurationOptionsInvalidProfileTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                List<string> tokens = new List<string>();
                foreach (Profile profile in profiles)
                {
                    tokens.Add(profile.token);
                }

                string token = tokens.GetNonMatchingString();

                RunStep(
                    () => { Client.GetAudioSourceConfigurationOptions(null, token); },
                    "Get audio source configuration options - negative test",
                    "Sender/InvalidArgVal/NoProfile",
                    true);

                DoRequestDelay();
            });
        }


        [Test(Name = "GET AUDIO SOURCE CONFIGURATION OPTIONS – INVALID CONFIGURATION TOKEN",
            Order = "03.01.19",
            Id = "3-1-19",
            Category = Category.MEDIA,
            Path = PATH_3_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioSourceConfigurationOptions })]
        public void GetAudioSourceConfigurationOptionsInvalidConfigurationTest()
        {
            RunTest(() =>
            {
                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                List<string> tokens = new List<string>();
                foreach (AudioSourceConfiguration config in configurations)
                {
                    tokens.Add(config.token);
                }

                string token = tokens.GetNonMatchingString();

                RunStep(
                    () => { Client.GetAudioSourceConfigurationOptions(token, null); },
                    "Get audio source configuration options - negative test",
                    "Sender/InvalidArgVal/NoConfig",
                    true);

                DoRequestDelay();
            });
        }


        [Test(Name = "SET AUDIO SOURCE CONFIGURATION – INVALID TOKEN",
            Order = "03.01.20",
            Id = "3-1-20",
            Category = Category.MEDIA,
            Path = PATH_3_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioSourceConfiguration })]
        public void SetAudioSourceConfigurationInvalidConfigurationTest()
        {
            RunTest(() =>
            {
                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                List<string> tokens = new List<string>();
                foreach (AudioSourceConfiguration config in configurations)
                {
                    tokens.Add(config.token);
                }

                string token = tokens.GetNonMatchingString();

                AudioSourceConfiguration configuration = new AudioSourceConfiguration();
                configuration.token = token;
                configuration.Name = configurations[0].Name;
                configuration.SourceToken = configurations[0].SourceToken;
                configuration.UseCount = configurations[0].UseCount;

                RunStep(
                    () => { Client.SetAudioSourceConfiguration(configuration, false); },
                    "Set audio source configuration - negative test",
                    "Sender/InvalidArgVal/NoConfig",
                    true);
                DoRequestDelay();
            });
        }

        #endregion



        // Heineken test
        [Test(Name = "SET AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.21",
            Id = "3-1-21",
            Category = Category.MEDIA,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioSourceConfigurations, 
                Functionality.GetCompatibleAudioSourceConfigurations, 
                Functionality.GetAudioSourceConfiguration, 
                Functionality.AddAudioSourceConfiguration,
                Functionality.GetAudioSourceConfigurationOptions})]
        public void SetAudioSourceConfigurationTest()
        {

            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;

            AudioEncoding backupEncoding = AudioEncoding.G711;
            AudioEncoderConfiguration configBackup = null;

            RunTest(
                () =>
                {
                    Profile profile = CreateProfileByAnnex3("testprofileX", null, out deletedProfile, out createdProfile, out modifiedProfile);
                    string reason;

                    //5.	ONVIF Client will invoke GetCompatibleAudioSourceConfigurationsRequest message 
                    // (ProfileToken = ‘testprofileX’) to retrieve the list of audio source configurations 
                    // compatible with profile. 
                    //6.	ONVIF Client verifies the list of audio source configurations sent by DUT. 
                    // Audio source
                    AudioSourceConfiguration[] configs = GetCompatibleAudioSourceConfigurations(profile.token);
                    Assert(ValidateAudioSourceConfigs(configs, out reason), reason, Resources.StepValidatingAudioSources_Title);
                    
                    //7.	ONVIF Client invokes AddAudioSourceConfigurationRequest message 
                    // (ProfileToken = ‘testprofileX’, ConfigurationToken as one of the tokens received in the 
                    // GetCompatibleAudioSourceConfigurationsResponse message) to add audio source configuration 
                    // to profile.
                    //8.	DUT adds the audio source configuration to the profile and sends the response. 
                    AudioSourceConfiguration config = configs[0];
                    AddAudioSourceConfiguration(profile.token, config.token);
                                        

                    //9.	ONVIF Client invokes GetCompatibleAudioEncoderConfigurationsRequest message 
                    // (ProfileToken = ‘testprofileX’) to retrieve audio encoder configurations compatible with 
                    // profile.
                    //10.	DUT sends the list of audio encoder configurations compatible with the received 
                    // media profile token.
                    // Audio encoder
                    AudioEncoderConfiguration[] encoderConfigurations = GetCompatibleAudioEncoderConfigurations(profile.token);
                    Assert(ValidateAudioEncoderConfigs(encoderConfigurations, out reason), reason, Resources.StepValidatingAudioEncoders_Title);
                    
                    //11.	ONVIF Client invokes AddAudioEncoderConfigurationRequest message (ProfileToken = 
                    // ‘testprofileX’, ConfigurationToken as one of the tokens received in the 
                    // GetCompatibleAudioencoderConfigurationsResponse message) to add audio encoder 
                    // configuration to profile.
                    //12.	DUT adds the audio encoder configuration to the profile and sends the response.
                    AudioEncoderConfiguration encoderConfig = encoderConfigurations[0];
                    AddAudioEncoderConfiguration(profile.token, encoderConfig.token);

                    //13.	ONVIF Client invokes GetAudioEncoderConfigurationOptionsRequest 
                    // (ProfileToken = ‘testprofileX’) request to retrieve audio encoder options for 
                    // specified profile. 
                    //14.	DUT sends the audio encoder configuration options which could be applied 
                    // to audio encoder from specified profile.
                    AudioEncoderConfigurationOptions options = GetAudioEncoderConfigurationOptions(null, profile.token);
                    Assert(options != null && options.Options != null, 
                        "No Audio Encoder Configuration options returned",
                        "Validate response received");


                    // Select valid options
                    List<AudioEncoderConfigurationOption> validOptions = options.Options.Where(o => o.BitrateList != null && o.SampleRateList != null).ToList();
                    Assert(validOptions.Count > 0,
                        "No valid options can be selected",
                        "Select AudioEncoderConfigurationOption to check configuration changing");
                    configBackup = Utils.CopyMaker.CreateCopy(encoderConfig);
                    
                    backupEncoding = encoderConfig.Encoding;
                    List<AudioEncoderConfigurationOption> opts = validOptions.Where(O => O.Encoding != backupEncoding).ToList();

                    AudioEncoderConfigurationOption encodingDifferent = opts.FirstOrDefault();

                    if (opts.Count == 0)
                    {
                        opts = validOptions;
                    }

                    // select with different encoding
                    AudioEncoderConfigurationOption selectedOptions = null;
                                    
                    AudioEncoderConfigurationOption bitrateDifferent = null;
                    AudioEncoderConfigurationOption sampleRateDifferent = null;
                    foreach (AudioEncoderConfigurationOption opt in opts)
                    {             
                        bool bitrateDiffers = opt.BitrateList.Where(B => B != encoderConfig.Bitrate).Count() > 0;
                        bool sampleRateDiffers = opt.SampleRateList.Where(SR => SR != encoderConfig.SampleRate).Count() > 0;

                        if (bitrateDiffers && sampleRateDiffers)
                        {
                            selectedOptions = opt;
                            break;
                        }
                        if (bitrateDiffers)
                        {
                            bitrateDifferent = opt;
                        }
                        if (sampleRateDiffers)
                        {
                            sampleRateDifferent = opt;
                        }
                    }

                    if (selectedOptions == null)
                    {
                        selectedOptions =  (encodingDifferent != null ) ? encodingDifferent : (bitrateDifferent != null ? bitrateDifferent : sampleRateDifferent);
                    }

                    if (selectedOptions != null)
                    {
                        //15.	ONVIF Client invokes SetAudioEncoderConfigurationRequest message 
                        // (ConfigurationToken, Encoding=[other than current], Bitrate = [other than current], 
                        // SampleRate = [other than current], ForcePersistence = false, where all values was 
                        // taken from audio encoder configuration options) to change 
                        //16.	DUT sends SetAudioEncoderConfigurationResponse message.
                        // Update encoder configuration
                        encoderConfig.Encoding = selectedOptions.Encoding;

                        List<int> bitrates = selectedOptions.BitrateList.Where(B => B != encoderConfig.Bitrate).ToList();
                        if (bitrates.Count > 0)
                        {
                            encoderConfig.Bitrate = bitrates[0];
                        }

                        List<int> sampleRates = selectedOptions.SampleRateList.Where(SR => SR != encoderConfig.SampleRate).ToList();
                        if (sampleRates.Count > 0)
                        {
                            encoderConfig.SampleRate = sampleRates[0];
                        }

                        SetAudioEncoderConfiguration(encoderConfig, false);

                        //17.	ONVIF Client invokes GetAudioEncoderConfigurationRequest message 
                        // (ConfigurationToken) to get new audio encoder configuration parameters.
                        //18.	DUT sends GetAudioEncoderConfigurationResponse message with parameters 
                        // specified in set request.
                        AudioEncoderConfiguration actual = GetAudioEncoderConfiguration(encoderConfig.token);


                        //19.	ONVIF Client checks that Audio configuration in GetAudioEncoderConfigurationResponse 
                        // message is the same as in SetAudioEncoderConfigurationRequest message.
                        string err = null;
                        bool equal = EqualConfigurations(encoderConfig, actual, out err);
                        string message = string.Format(Resources.ErrorAudioEncoderConfigNotEqual_Format, System.Environment.NewLine + err);
                        Assert(equal,
                            message,
                            Resources.StepCompareAudioEncoderConfigs_Title);                        
                    }
                },
                () =>
                {
                    if (configBackup != null)
                    {
                        SetAudioEncoderConfiguration(configBackup, true);
                    }
                    RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
                });
        }
    
    }
}
