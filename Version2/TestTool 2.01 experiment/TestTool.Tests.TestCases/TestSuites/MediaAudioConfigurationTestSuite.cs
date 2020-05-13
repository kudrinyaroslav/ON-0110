///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.TestCases;
using TestTool.Proxies.Media;
using System.Xml;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaAudioConfigurationTestSuite : Base.MediaTest
    {
        public MediaAudioConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }
        [Test(Name = "AUDIO SOURCE CONFIGURATION",
            Path = "Media Configuration\\Audio Configuration",
            Order = "07.03.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
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
            Path = "Media Configuration\\Audio Configuration",
            Order = "07.03.02",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequiredFeatures = new Feature[]{Feature.Audio},
            RequirementLevel = RequirementLevel.ConditionalMust)]
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
            Path = "Media Configuration\\Audio Configuration",
            Order = "07.03.03",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
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
            Path = "Media Configuration\\Audio Configuration",
            Order = "07.03.04",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequiredFeatures =  new Feature[]{Feature.Audio, Feature.G726},
            RequirementLevel = RequirementLevel.ConditionalMust)]
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
            Path = "Media Configuration\\Audio Configuration",
            Order = "07.03.05",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequiredFeatures = new Feature[] {Feature.Audio, Feature.AAC},
            RequirementLevel = RequirementLevel.ConditionalMust)]
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
    }
}
