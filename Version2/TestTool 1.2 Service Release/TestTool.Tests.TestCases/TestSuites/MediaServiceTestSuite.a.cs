///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    partial class MediaServiceTestSuite : Base.MediaTest
    {
        public MediaServiceTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH_1_1 = "Media Configuration\\Media Profile";

        private const string PATH_2_2 = "Media Configuration\\Video Configuration\\Video Source Configuration";

        private const string PATH_2_3 = "Media Configuration\\Video Configuration\\Video Encoder Configuration";

        private const string PATH_3_2 = "Media Configuration\\Audio Configuration\\Audio Source Configuration";

        private const string PATH_3_3 = "Media Configuration\\Audio Configuration\\Audio Encoder Configuration";

        private const string PATH_4_1 = "Media Configuration\\PTZ Configuration";
       
        #region Profiles

        [Test(Name = "PROFILES CONSISTENCY",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.MEDIA,
            Path = PATH_1_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must)]
        public void ProfileConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (Profile profile in profiles)
                {
                    if (!tokens.Contains(profile.token))
                    {
                        tokens.Add(profile.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(profile.token))
                        {
                            duplicates.Add(profile.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate profiles");
                }

                // validate profiles
                foreach (Profile profile in profiles)
                {
                    Profile profile2 = GetProfile(profile.token);

                    CompareProfiles(profile, profile2);
                }
            });
        }

        #endregion

        #region Audio Source

        [Test(Name = "AUDIO SOURCE CONFIGURATIONS AND PROFILES CONSISTENCY",
            Order = "03.02.01",
            Id = "3-2-1",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioSourceConfigurationAndProfileConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                // Audio source configurations 

                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (AudioSourceConfiguration configuration in configurations)
                {
                    if (!tokens.Contains(configuration.token))
                    {
                        tokens.Add(configuration.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(configuration.token))
                        {
                            duplicates.Add(configuration.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate audio source configurations");
                }

                // Check that each AudioSourceConfigurations from the GetProfilesResponse message 
                // are included in the GetAudioSourceConfigurationsResponse message


                foreach (Profile profile in profiles)
                {
                    if (profile.AudioSourceConfiguration != null)
                    {
                        string token = profile.AudioSourceConfiguration.token;

                        AudioSourceConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        Assert(configuration != null,
                               string.Format("Audio source configuration with token '{0}' not found", token),
                               string.Format(
                                   "Check that audio source configuration for profile with token '{0}' exists",
                                   profile.token));
                    }
                }

                // Check that AudioSourceConfiguration parameters are same in GetProfilesResponse 
                // message and in GetAudioSourceConfigurationsResponse message for each 
                // AudioSourceConfiguration

                foreach (Profile profile in profiles)
                {
                    if (profile.AudioSourceConfiguration != null)
                    {
                        string token = profile.AudioSourceConfiguration.token;

                        AudioSourceConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        CompareConfigurations(profile.AudioSourceConfiguration, configuration, true);

                    }
                }
            });
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATIONS AND AUDIO SOURCE CONFIGURATION CONSISTENCY",
            Order = "03.02.02",
            Id = "3-2-2",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioSourceConfigurationConsistencyTest()
        {
            RunTest(() =>
            {
                // Audio source configurations 

                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (AudioSourceConfiguration configuration in configurations)
                {
                    AudioSourceConfiguration config = GetAudioSourceConfiguration(configuration.token);

                    CompareConfigurations(configuration, config, true);
                }
            });
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATIONS AND AUDIO SOURCE CONFIGURATION OPTIONS CONSISTENCY",
            Order = "03.02.03",
            Id = "3-2-3",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioSourceConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                // Audio source configurations 

                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (AudioSourceConfiguration configuration in configurations)
                {
                    AudioSourceConfigurationOptions options = GetAudioSourceConfigurationOptions(null, configuration.token);

                    Assert(options != null,
                        "DUT did not return audio source configuration options",
                        "Check if the DUT returned audio source configuration options");
                    
                    Assert(options.InputTokensAvailable != null && options.InputTokensAvailable.Length > 0,
                        "No InputTokensAvailable returned", "Check if the DUT returned available input tokens");

                    // check that tokens are unique
                    List<string> duplicates = new List<string>();
                    List<string> tokens = new List<string>();
                    foreach (string inputToken in options.InputTokensAvailable)
                    {
                        if (tokens.Contains(inputToken))
                        {
                            if (!duplicates.Contains(inputToken))
                            {
                                duplicates.Add(inputToken);
                            }
                        }
                        else
                        {
                            tokens.Add(inputToken);
                        }
                    }

                    StringBuilder sb = new StringBuilder("The following input tokens in InputTokensAvailable are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert((duplicates.Count == 0), sb.ToString(), "Check that input tokens listed are unique");


                    Assert(options.InputTokensAvailable.Contains(configuration.SourceToken),
                        string.Format("InputTokensAvailable does not contain '{0}'", configuration.SourceToken),
                        string.Format("Check that InputTokensAvailable contains SourceToken"));


                }
            });
        }

        [Test(Name = "PROFILES AND AUDIO SOURCE CONFIGURATION OPTIONS CONSISTENCY",
            Order = "03.02.04",
            Id = "3-2-4",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void ProfilesAudioSourceConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");


                foreach (Profile profile in profiles)
                {
                    if (profile.AudioSourceConfiguration != null)
                    {
                        AudioSourceConfigurationOptions options =
                            GetAudioSourceConfigurationOptions(profile.token, profile.AudioSourceConfiguration.token);

                        Assert(options != null, "No configuration options received",
                               "Check that the DUT sent audio source configuration options");
                        
                        Assert(options.InputTokensAvailable != null, "Input tokens available not specified", "Check that input tokens list is not empty");

                        // check that tokens are unique
                        List<string> duplicates = new List<string>();
                        List<string> tokens = new List<string>();
                        foreach (string inputToken in options.InputTokensAvailable)
                        {
                            if (tokens.Contains(inputToken))
                            {
                                if (!duplicates.Contains(inputToken))
                                {
                                    duplicates.Add(inputToken);
                                }
                            }
                            else
                            {
                                tokens.Add(inputToken);
                            }
                        }

                        StringBuilder sb = new StringBuilder("The following input tokens in InputTokensAvailable are not unique: ");
                        bool first = true;
                        foreach (string token in duplicates)
                        {
                            sb.Append(first ? token : string.Format(", {0}", token));
                            first = false;
                        }

                        Assert((duplicates.Count == 0), sb.ToString(), "Check that input tokens listed are unique");

                        Assert(options.InputTokensAvailable.Contains(profile.AudioSourceConfiguration.SourceToken), 
                            string.Format("Source token specified ('{0}') not found in options", profile.AudioSourceConfiguration.SourceToken), 
                            "Check that source token is presented in the list of available input tokens");

                    }
                }

            });
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATIONS AND AUDIO SOURCES CONSISTENCY",
            Order = "03.02.05",
            Id = "3-2-5",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioSourceConfigurationAndAudioSourcesConsistencyTest()
        {
            RunTest(() =>
            {
                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");
                
                AudioSource[] audioSources = GetAudioSources();

                Assert(audioSources != null,
                    "No Audio sources returned",
                    "Check if the DUT returned audio sources");

                // Check that each AudioSource from GetAudioSourcesResponse message has unique token.
                List<string> duplicates = new List<string>();
                List<string> tokens = new List<string>();
                foreach (AudioSource audioSource in audioSources)
                {
                    if (tokens.Contains(audioSource.token))
                    {
                        if (!duplicates.Contains(audioSource.token))
                        {
                            duplicates.Add(audioSource.token);
                        }
                    }
                    else
                    {
                        tokens.Add(audioSource.token);
                    }
                }

                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following audio source tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate audio sources");
                }

                // Check that every AudioSourceConfiguration.SourceToken from GetAudioSourceConfigurationsResponse 
                // message exists in GetAudioSourcesResponse message (AudioSource.token).

                foreach (AudioSourceConfiguration configuration in configurations)
                {
                    AudioSource source = audioSources.Where(S => S.token == configuration.SourceToken).FirstOrDefault();

                    Assert(source != null, 
                        string.Format("Audio source with token '{0}' not found", configuration.SourceToken),
                        string.Format("Check that SourceToken for configuration '{0}' exists", configuration.token));

                }


            });
        }
        
        #endregion

        #region Audio Encoder

        [Test(Name = "AUDIO ENCODER CONFIGURATIONS AND PROFILES CONSISTENCY",
            Order = "03.03.01",
            Id = "3-3-1",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioEncoderConfigurationAndProfileConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");
                
                // Audio encoder configurations 

                AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (AudioEncoderConfiguration configuration in configurations)
                {
                    if (!tokens.Contains(configuration.token))
                    {
                        tokens.Add(configuration.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(configuration.token))
                        {
                            duplicates.Add(configuration.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate audio source configurations");
                }

                // Check that each AudioEncoderConfigurations from the GetProfilesResponse message 
                // are included in the GetAudioEncoderConfigurationsResponse message


                foreach (Profile profile in profiles)
                {
                    if (profile.AudioEncoderConfiguration != null)
                    {
                        string token = profile.AudioEncoderConfiguration.token;

                        AudioEncoderConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        Assert(configuration != null,
                               string.Format("Audio encoder configuration with token '{0}' not found", token),
                               string.Format(
                                   "Check that audio encoder configuration for profile with token '{0}' exists",
                                   profile.token));
                    }
                }

                // Check that AudioSourceConfiguration parameters are same in GetProfilesResponse 
                // message and in GetAudioSourceConfigurationsResponse message for each 
                // AudioSourceConfiguration

                foreach (Profile profile in profiles)
                {
                    if (profile.AudioEncoderConfiguration != null)
                    {
                        string token = profile.AudioEncoderConfiguration.token;

                        AudioEncoderConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        CompareConfigurations(profile.AudioEncoderConfiguration, configuration, true);

                    }
                }
            });
        }

        
        [Test(Name = "AUDIO ENCODER CONFIGURATIONS AND AUDIO ENCODER CONFIGURATION CONSISTENCY",
            Order = "03.03.02",
            Id = "3-3-2",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioEncoderConfigurationConsistencyTest()
        {
            RunTest(() =>
            {
                // Audio Encoder configurations 

                AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (AudioEncoderConfiguration configuration in configurations)
                {
                    AudioEncoderConfiguration config = GetAudioEncoderConfiguration(configuration.token);

                    CompareConfigurations(configuration, config, true);
                }
            });
        }


        [Test(Name = "AUDIO ENCODER CONFIGURATIONS AND AUDIO ENCODER CONFIGURATION OPTIONS CONSISTENCY",
            Order = "03.03.03",
            Id = "3-3-3",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioEncoderConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                // Audio Encoder configurations 

                AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (AudioEncoderConfiguration configuration in configurations)
                {
                    AudioEncoderConfigurationOptions options = GetAudioEncoderConfigurationOptions(configuration.token, null);

                    Assert(options != null,
                        "DUT did not return audio encoder configuration options",
                        "Check if the DUT returned audio encoder configuration options");

                    CheckOptionsExist(options, configuration );

                }
            });
        }
        

        [Test(Name = "PROFILES AND AUDIO ENCODER CONFIGURATION OPTIONS CONSISTENCY",
            Order = "03.03.04",
            Id = "3-3-4",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void ProfilesAudioEncoderConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");


                foreach (Profile profile in profiles)
                {
                    if (profile.AudioEncoderConfiguration != null)
                    {
                        AudioEncoderConfigurationOptions options =
                            GetAudioEncoderConfigurationOptions(profile.AudioEncoderConfiguration.token, profile.token);

                        CheckOptionsExist(options, profile.AudioEncoderConfiguration);
                    }
                }


            });
        }

        #endregion

        #region Video Source
        
        [Test(Name = "VIDEO SOURCE CONFIGURATIONS AND PROFILES CONSISTENCY",
            Order = "02.02.01",
            Id = "2-2-1",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video  })]
        public void VideoSourceConfigurationAndProfileConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                // Video source configurations 

                VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                // Check that each VideoSourceConfiguration from GetVideoSourceConfigurationsResponse 
                // message has unique token.
                
                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (VideoSourceConfiguration configuration in configurations)
                {
                    if (!tokens.Contains(configuration.token))
                    {
                        tokens.Add(configuration.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(configuration.token))
                        {
                            duplicates.Add(configuration.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate audio source configurations");
                }

                // Check that each VideoSourceConfigurations from the GetProfilesResponse message 
                // are included in the GetVideoSourceConfigurationsResponse message


                foreach (Profile profile in profiles)
                {
                    if (profile.VideoSourceConfiguration != null)
                    {
                        string token = profile.VideoSourceConfiguration.token;

                        VideoSourceConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        Assert(configuration != null,
                               string.Format("Video source configuration with token '{0}' not found", token),
                               string.Format(
                                   "Check that video source configuration for profile with token '{0}' exists",
                                   profile.token));
                    }
                }

                // Check that VideoSourceConfiguration parameters are same in GetProfilesResponse 
                // message and in GetAudioSourceConfigurationsResponse message for each 
                // VideoSourceConfiguration

                foreach (Profile profile in profiles)
                {
                    if (profile.VideoSourceConfiguration != null)
                    {
                        string token = profile.VideoSourceConfiguration.token;

                        VideoSourceConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        CompareConfigurations(profile.VideoSourceConfiguration, configuration, true);

                    }
                }
            });
        }
        

        [Test(Name = "VIDEO SOURCE CONFIGURATIONS AND VIDEO SOURCE CONFIGURATION CONSISTENCY",
            Order = "02.02.02",
            Id = "2-2-2",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoSourceConfigurationConsistencyTest()
        {
            RunTest(() =>
            {
                // VideoSource configurations 

                VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (VideoSourceConfiguration configuration in configurations)
                {
                    VideoSourceConfiguration config = GetVideoSourceConfiguration(configuration.token);

                    CompareConfigurations(configuration, config, true);
                }
            });
        }


        [Test(Name = "VIDEO SOURCE CONFIGURATIONS AND VIDEO SOURCE CONFIGURATION OPTIONS CONSISTENCY",
            Order = "02.02.03",
            Id = "2-2-3",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoSourceConfigurationAndOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                // VideoSource configurations 

                VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (VideoSourceConfiguration configuration in configurations)
                {
                    ValidateConfiguration(configuration);

                    VideoSourceConfigurationOptions options = GetVideoSourceConfigurationOptions(null,
                                                                                                 configuration.token);
                    
                    Assert(options != null,
                        "Video source configuration options not returned",
                        "Check if the DUT returned video source configuration options");
                    
                    ValidateOptions(options);
                    
                    CheckOptions(options, configuration);
                }
            });
        }


        [Test(Name = "PROFILES AND VIDEO SOURCE CONFIGURATION OPTIONS CONSISTENCY",
            Order = "02.02.04",
            Id = "2-2-4",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void ProfilesVideoSourceConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");


                foreach (Profile profile in profiles)
                {
                    if (profile.VideoSourceConfiguration != null)
                    {
                        VideoSourceConfigurationOptions options = 
                            GetVideoSourceConfigurationOptions(profile.token, profile.VideoSourceConfiguration.token);
                        
                        Assert(options != null, 
                            "Video source configuration options not returned", 
                            "Check if the DUT returned video source configuration options");

                        CheckOptions(options, profile.VideoSourceConfiguration);
                    }
                }
            });
        }
        

        [Test(Name = "VIDEO SOURCE CONFIGURATIONS AND VIDEO SOURCES CONSISTENCY",
            Order = "02.02.05",
            Id = "2-2-5",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoSourceConfigurationAndVideoSourcesConsistencyTest()
        {
            RunTest(() =>
            {
                // VideoSource configurations 

                VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                VideoSource[] sources = GetVideoSources();

                Assert(sources != null && sources.Length > 0,
                    "DUT did not return any video sources",
                    "Check if the DUT returned video sources");


                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (VideoSource source in sources)
                {
                    if (!tokens.Contains(source.token))
                    {
                        tokens.Add(source.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(source.token))
                        {
                            duplicates.Add(source.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate video sources");
                }
                
                foreach (VideoSourceConfiguration configuration in configurations)
                {
                    Assert(tokens.Contains(configuration.SourceToken), 
                        string.Format("Video source with token '{0}' not found", configuration.SourceToken), 
                        string.Format("Check if video source exists for configuration '{0}'", configuration.token));
                }
            });
        }

        
        #endregion

        #region Video Encoder

        [Test(Name = "VIDEO ENCODER CONFIGURATIONS AND PROFILES CONSISTENCY",
            Order = "02.03.01",
            Id = "2-3-1",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoEncoderConfigurationAndProfileConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                // Audio encoder configurations 

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                // Check that each VideoEncoderConfiguration from VideoEncoderConfigurationsResponse 
                // message has unique token.

                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (VideoEncoderConfiguration configuration in configurations)
                {
                    if (!tokens.Contains(configuration.token))
                    {
                        tokens.Add(configuration.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(configuration.token))
                        {
                            duplicates.Add(configuration.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate video encoder configurations");
                }

                // Check that each VideoEncoderConfiguration from the GetProfilesResponse message 
                // are included in the GetVideoEncoderConfigurationsResponse message


                foreach (Profile profile in profiles)
                {
                    if (profile.VideoEncoderConfiguration != null)
                    {
                        string token = profile.VideoEncoderConfiguration.token;

                        VideoEncoderConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        Assert(configuration != null,
                               string.Format("Video encoder configuration with token '{0}' not found", token),
                               string.Format(
                                   "Check that video encoder configuration for profile with token '{0}' exists",
                                   profile.token));
                    }
                }

                // Check that VideoEncoderConfiguration parameters are same in GetProfilesResponse 
                // message and in GetVideoEncoderConfigurationsResponse message for each 
                // VideoEncoderConfiguration

                foreach (Profile profile in profiles)
                {
                    if (profile.VideoEncoderConfiguration != null)
                    {
                        string token = profile.VideoEncoderConfiguration.token;

                        VideoEncoderConfiguration configuration =
                            configurations.Where(C => C.token == token).FirstOrDefault();

                        CompareConfigurations(profile.VideoEncoderConfiguration, configuration, true);

                    }
                }
            });
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATIONS AND VIDEO ENCODER CONFIGURATION CONSISTENCY",
            Order = "02.03.02",
            Id = "2-3-2",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoEncoderConfigurationConsistencyTest()
        {
            RunTest(() =>
            {

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (VideoEncoderConfiguration configuration in configurations)
                {
                    VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configuration.token);

                    CompareConfigurations(configuration, config, true);
                }
            });
        }


        [Test(Name = "VIDEO ENCODER CONFIGURATIONS AND VIDEO ENCODER CONFIGURATION OPTIONS CONSISTENCY",
            Order = "02.03.03",
            Id = "2-3-3",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoEncoderConfigurationAndOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                // VideoEncoder configurations 

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (VideoEncoderConfiguration configuration in configurations)
                {
                    ValidateConfiguration(configuration);

                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(configuration.token, null);

                    Assert(options != null,
                        "Video encoder configuration options not returned",
                        "Check if the DUT returned video encoder configuration options");

                    CheckOptions(options, configuration);
                }
            });
        }

        [Test(Name = "PROFILES AND VIDEO ENCODER CONFIGURATION OPTIONS CONSISTENCY",
            Order = "02.03.04",
            Id = "2-3-4",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void ProfilesVideoEncoderConfigurationAndConfigurationOptionsConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");


                foreach (Profile profile in profiles)
                {
                    if (profile.VideoEncoderConfiguration != null)
                    {
                        VideoEncoderConfigurationOptions options =
                            GetVideoEncoderConfigurationOptions(profile.VideoEncoderConfiguration.token, profile.token);

                        Assert(options != null,
                            "Video encoder configuration options not returned",
                            "Check if the DUT returned video encoder configuration options");

                        CheckOptions(options, profile.VideoEncoderConfiguration);
                    }
                }
            });
        }
        
        #endregion
        
        #region PTZ


        [Test(Name = "PTZ CONFIGURATIONS AND PROFILES CONSISTENCY",
            Order = "04.01.02",
            Id = "4-1-2",
            Category = Category.MEDIA,
            Path = PATH_4_1,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.PTZ })]
        public void ProfilesAndPTZConfigurationConsistencyTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                Assert(profiles != null,
                    "DUT did not return any profile",
                    "Check if the DUT returned media profiles");

                TestTool.Proxies.Onvif.PTZConfiguration[] ptzConfigurations = GetPtzConfigurations();

                Assert(ptzConfigurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");


                // check that configurations have unique names
                List<string> tokens = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (TestTool.Proxies.Onvif.PTZConfiguration configuration in ptzConfigurations)
                {
                    if (!tokens.Contains(configuration.token))
                    {
                        tokens.Add(configuration.token);
                    }
                    else
                    {
                        if (!duplicates.Contains(configuration.token))
                        {
                            duplicates.Add(configuration.token);
                        }
                    }
                }
                if (duplicates.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("The following tokens are not unique: ");
                    bool first = true;
                    foreach (string token in duplicates)
                    {
                        sb.Append(first ? token : string.Format(", {0}", token));
                        first = false;
                    }

                    Assert(false, sb.ToString(), "Validate PTZ configurations");
                }

                foreach (Profile profile in profiles)
                {
                    if (profile.PTZConfiguration != null)
                    {
                        Assert(tokens.Contains(profile.PTZConfiguration.token), 
                            string.Format("PTZ configuration '{0}' not found", profile.PTZConfiguration.token),
                            string.Format("Check if PTZ configuration for profile '{0}' exists", profile.token));
                        
                        TestTool.Proxies.Onvif.PTZConfiguration config =
                            ptzConfigurations.Where(c => c.token == profile.PTZConfiguration.token).FirstOrDefault();

                        CompareConfigurations(config, profile.PTZConfiguration);
                    
                    }
                }



            });
        }

        #endregion




    }
}
