///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Collections.Generic;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;

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
            Order = "03.01.01",
            Id = "3-1-1",
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
            Profile profile = null;
            RunTest<Profile>(
                new Backup<Profile>(() => { return null; }),
                () =>
                {
                    profile = CreateProfile("testprofilex", null);
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
                    DeleteProfile(profile.token);
                    profile = null;
                },
                (param) =>
                {
                    if (profile != null)
                    {
                        DeleteProfile(profile.token);
                    }
                });
        }
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.02",
            Id = "3-1-2",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetAudioEncoderConfigurations, 
                Functionality.GetCompatibleAudioEncoderConfigurations, 
                Functionality.AddAudioEncoderConfiguration})]
        public void AudioEncoderConfiguration()
        {
            Profile profile = null;
            RunTest<Profile>(
               new Backup<Profile>(() => { return null; }),
               () =>
               {
                   profile = CreateProfile("testprofilex", null);
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
                   DeleteProfile(profile.token);
                   profile = null;
               },
               (param) =>
               {
                   if (profile != null)
                   {
                       DeleteProfile(profile.token);
                   }
               });
        }
        
        [Test(Name = "G.711 AUDIO ENCODER CONFIGURATION",
            Path = PATH_3_1,
            Order = "03.01.03",
            Id = "3-1-3",
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
                config.SampleRate = options.SampleRateList[0] + 1;
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
            Order = "03.01.04",
            Id = "3-1-4",
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
                config.SampleRate = options.SampleRateList[0] + 1;
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
            Order = "03.01.05",
            Id = "3-1-5",
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
                config.SampleRate = options.SampleRateList[0] + 1;
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



        /// <summary>
        /// Author: Anna Tarasova
        /// </summary>
        #region Audio Source


        [Test(Name = "GET AUDIO SOURCE CONFIGURATION – INVALID CONFIGURATIONTOKEN",
            Order = "03.01.06",
            Id = "3-1-6",
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
            Order = "03.01.08",
            Id = "3-1-8",
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
            Order = "03.01.09",
            Id = "3-1-9",
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
            Order = "03.01.10",
            Id = "3-1-10",
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

    
    
    }
}
