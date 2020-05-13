///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Exceptions;

namespace TestTool.Tests.TestCases.TestSuites
{
    partial class MediaServiceTestSuite 
    {

        #region AudioSourceConfiguration

        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (CURRENT STATE)",
            Path = PATH_3_2,
            Order = "03.02.06",
            Id = "3-2-6",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioSourceConfigurationUseCountTest()
        {
            RunTest(
                () =>
                {
                    AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                        "DUT did not return any configuration",
                        "Check if the DUT returned configurations");

                    Profile[] profiles = GetProfiles();

                    Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");
                    
                    foreach (AudioSourceConfiguration configuration in configurations)
                    {
                        int useCount = configuration.UseCount;

                        int cnt = 0;
                        List<string> invalidProfiles = new List<string>();

                        foreach (Profile profile in profiles)
                        {
                            if (profile.AudioSourceConfiguration != null)
                            {
                                if (profile.AudioSourceConfiguration.token == configuration.token)
                                {
                                    cnt++;
                                    if (profile.AudioSourceConfiguration.UseCount != useCount)
                                    {
                                        invalidProfiles.Add(profile.token);
                                    }
                                }
                            }
                        }

                        Assert(cnt <= useCount, 
                            string.Format("UseCount value for configuration with token '{0}' is invalid. Value in configuration: {1}, configuration is used in {2} profile(s)", 
                            configuration.token, useCount, cnt));

                        if (invalidProfiles.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder("UseCount value is different when configuration is referenced from the following profiles: ");
                            bool first = true;
                            foreach (string token in invalidProfiles)
                            {
                                sb.Append(first ? token : string.Format(", {0}", token));
                                first = false;
                            }

                            Assert(false, sb.ToString(), string.Format("Check UseCount value for configuration with token '{0}' referenced from profiles", configuration.token));
                        }

                        AudioSourceConfiguration config = GetAudioSourceConfiguration(configuration.token);

                        Assert(config.UseCount == configuration.UseCount, 
                            string.Format("UseCount is different when getting single configuration with token '{0}' and list of all configuration.", configuration.token), 
                            "Check UseCount value");


                    }

                });
        }
        
        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (ADD SAME AUDIO SOURCE CONFIGURATION TO PROFILE TWICE)",
            Path = PATH_3_2,
            Order = "03.02.07",
            Id = "3-2-7",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioSourceConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioSourceConfiguration configuration1 = configurations[0];

                    if (newProfile.AudioSourceConfiguration != null)
                    {
                        RemoveAudioSourceConfigurationFromProfile(newProfile);

                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    AddAudioSourceConfiguration(token, configuration1.token);

                    AudioSourceConfiguration newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");


                    AddAudioSourceConfiguration(token, configuration1.token);

                    newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount+1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding the same configuration to a profile twice");
                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile); 
                        }
                    }
                });
        }
        
        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (ADD DIFFERENT AUDIO SOURCE CONFIGURATIONS IN PROFILE)",
            Path = PATH_3_2,
            Order = "03.02.08",
            Id = "3-2-8",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioSourceConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                    BeginStep("Check if the DUT returned audio source configurations");

                    if (configurations == null || configurations.Length == 0)
                    {
                        throw new AssertException("DUT did not return any audio source configuration");
                    }
                    if (configurations.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (configurations.Length == 1)
                    {
                        return;
                    }

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioSourceConfiguration configuration1 = configurations[0];

                    AudioSourceConfiguration configuration2 = configurations[1];

                    if (newProfile.AudioSourceConfiguration != null)
                    {
                        RemoveAudioSourceConfigurationFromProfile(newProfile);
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
                        if (configuration2.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration2.UseCount = configuration2.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddAudioSourceConfiguration(token, configuration1.token);

                    AudioSourceConfiguration newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");

                    AddAudioSourceConfiguration(token, configuration2.token);

                    newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    AudioSourceConfiguration newConfiguration2 = GetAudioSourceConfiguration(configuration2.token);

                    Assert(newConfiguration2.UseCount == configuration2.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration2.UseCount + 1, newConfiguration2.UseCount),
                        "Check UseCount value after adding configuration to a profile (for added configuration)");

                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (REMOVE AUDIO SOURCE CONFIGURATION)",
            Path = PATH_3_2,
            Order = "03.02.09",
            Id = "3-2-9",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioSourceConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");

                   
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioSourceConfiguration configuration1 = configurations[0];

                    if (newProfile.AudioSourceConfiguration != null)
                    {
                        RemoveAudioSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddAudioSourceConfiguration(token, configuration1.token);

                    RemoveAudioSourceConfiguration(token);
                    
                    AudioSourceConfiguration newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after removing configuration from a profile");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (PROFILE DELETION WITH AUDIO SOURCE CONFIGURATION)",
            Path = PATH_3_2,
            Order = "03.02.10",
            Id = "3-2-10",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioSourceConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;
            
            RunTest(
                () =>
                {

                    AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");

                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }
                    
                    AudioSourceConfiguration configuration = configurations[0];

                    if (newProfile.AudioSourceConfiguration != null)
                    {
                        RemoveAudioSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddAudioSourceConfiguration(token, configuration.token);

                    DeleteProfile(newProfile.token);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    AudioSourceConfiguration newConfiguration = GetAudioSourceConfiguration(configuration.token);

                    Assert(newConfiguration.UseCount == configuration.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration.UseCount, newConfiguration.UseCount),
                        "Check UseCount value after deleting profile with configuration");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            RestoreProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (SET AUDIO SOURCE CONFIGURATION)",
            Order = "03.02.11",
            Id = "3-2-11",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioSourceConfigurationChangeUseCountTest()
        {
            RunTest(() =>
            {
                // Audio source configurations 

                AudioSourceConfiguration[] configurations = GetAudioSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                AudioSourceConfiguration configuration = configurations[0];

                int useCount = configuration.UseCount;
                configuration.UseCount = useCount + 1;
                SetAudioSourceConfiguration(configuration, true);
               
                AudioSourceConfiguration actualConfiguration = GetAudioSourceConfiguration(configuration.token);

                Assert(actualConfiguration.UseCount == useCount,
                string.Format("Use count value is invalid. Expected: {0}, actual: {1}", useCount, actualConfiguration.UseCount),
                "Check UseCount after setting new value via SetAudioSourceConfiguration");
            });
        }

        #endregion
        
        #region AudioEncoderConfiguration
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (CURRENT STATE)",
            Path = PATH_3_3,
            Order = "03.03.05",
            Id = "3-3-5",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioEncoderConfigurationUseCountTest()
        {
            RunTest(
                () =>
                {
                    AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                        "DUT did not return any configuration",
                        "Check if the DUT returned configurations");

                    Profile[] profiles = GetProfiles();

                    Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                    foreach (AudioEncoderConfiguration configuration in configurations)
                    {
                        int useCount = configuration.UseCount;

                        int cnt = 0;
                        List<string> invalidProfiles = new List<string>();

                        foreach (Profile profile in profiles)
                        {
                            if (profile.AudioEncoderConfiguration != null)
                            {
                                if (profile.AudioEncoderConfiguration.token == configuration.token)
                                {
                                    cnt++;
                                    if (profile.AudioEncoderConfiguration.UseCount != useCount)
                                    {
                                        invalidProfiles.Add(profile.token);
                                    }
                                }
                            }
                        }

                        Assert(cnt <= useCount,
                            string.Format("UseCount value for configuration with token '{0}' is invalid. Value in configuration: {1}, configuration is used in {2} profile(s)",
                            configuration.token, useCount, cnt));

                        if (invalidProfiles.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder("UseCount value is different when configuration is referenced from the following profiles: ");
                            bool first = true;
                            foreach (string token in invalidProfiles)
                            {
                                sb.Append(first ? token : string.Format(", {0}", token));
                                first = false;
                            }

                            Assert(false, sb.ToString(), string.Format("Check UseCount value for configuration with token '{0}' referenced from profiles", configuration.token));
                        }

                        AudioEncoderConfiguration config = GetAudioEncoderConfiguration(configuration.token);

                        Assert(config.UseCount == configuration.UseCount,
                            string.Format("UseCount is different when getting single configuration with token '{0}' and list of all configuration.", configuration.token),
                            "Check UseCount value");
                    }
                });
        }
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (ADD SAME AUDIO ENCODER CONFIGURATION TO PROFILE TWICE)",
            Path = PATH_3_3,
            Order = "03.03.06",
            Id = "3-3-6",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioEncoderConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;
                    
            RunTest(
                () =>
                {

                    AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");

                    AudioSourceConfiguration[] sourceConfigurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                       "DUT did not return any audio source configuration",
                       "Check if the DUT returned audio source configurations");
                    
                    newProfile = CreateTestProfile(out existing);
                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioEncoderConfiguration configuration1 = configurations[0];

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        RemoveAudioEncoderConfiguration(newProfile.token);

                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    AudioSourceConfiguration sc1 = sourceConfigurations[0];

                    AddAudioSourceConfiguration(newProfile.token, sc1.token);

                    AddAudioEncoderConfiguration(token, configuration1.token);

                    AudioEncoderConfiguration newConfiguration1 = GetAudioEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");


                    AddAudioEncoderConfiguration(token, configuration1.token);

                    newConfiguration1 = GetAudioEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount+1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount+1, newConfiguration1.UseCount),
                        "Check UseCount value after adding the same configuration to a profile twice");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (ADD DIFFERENT AUDIO ENCODER CONFIGURATIONS IN PROFILE)",
            Path = PATH_3_3,
            Order = "03.03.07",
            Id = "3-3-7",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioEncoderConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                    BeginStep("Check if the DUT returned audio encoder configurations");
                    
                    if (configurations == null || configurations.Length == 0)
                    {
                        throw new AssertException("DUT did not return any audio encoder configuration");
                    }
                    if (configurations.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (configurations.Length == 1)
                    {
                        return;
                    }

                    AudioSourceConfiguration[] sourceConfigurations = GetAudioSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                       "DUT did not return any audio source configuration",
                       "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);
                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioEncoderConfiguration configuration1 = configurations[0];

                    AudioEncoderConfiguration configuration2 = configurations[1];
                    
                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        RemoveAudioEncoderConfiguration(newProfile.token);
                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                        // fix for UseCount
                        if (configuration2.token == newProfile.AudioEncoderConfiguration.token)
                        {
                            configuration2.UseCount = configuration2.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    AudioSourceConfiguration sc1 = sourceConfigurations[0];

                    AddAudioSourceConfiguration(newProfile.token, sc1.token);

                    AddAudioEncoderConfiguration(token, configuration1.token);

                    AudioEncoderConfiguration newConfiguration1 = GetAudioEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");

                    AddAudioEncoderConfiguration(token, configuration2.token);

                    newConfiguration1 = GetAudioEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    AudioEncoderConfiguration newConfiguration2 = GetAudioEncoderConfiguration(configuration2.token);

                    Assert(newConfiguration2.UseCount == configuration2.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration2.UseCount + 1, newConfiguration2.UseCount),
                        "Check UseCount value after adding configuration to a profile (for added configuration)");



                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (REMOVE AUDIO ENCODER CONFIGURATION)",
            Path = PATH_3_3,
            Order = "03.03.08",
            Id = "3-3-8",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioEncoderConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");
                    
                    AudioSourceConfiguration[] sourceConfigurations = GetAudioSourceConfigurations();
                    
                    Assert(configurations != null && configurations.Length > 0,
                       "DUT did not return any audio source configuration",
                       "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioEncoderConfiguration configuration1 = configurations[0];

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        RemoveAudioEncoderConfiguration(newProfile.token);
                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    AudioSourceConfiguration sc1 = sourceConfigurations[0];

                    AddAudioSourceConfiguration(newProfile.token, sc1.token);

                    AddAudioEncoderConfiguration(token, configuration1.token);

                    RemoveAudioEncoderConfiguration(token);

                    AudioEncoderConfiguration newConfiguration1 = GetAudioEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after removing configuration from a profile");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (DELETION PROFILE WITH AUDIO ENCODER CONFIGURATION)",
            Path = PATH_3_3,
            Order = "03.03.09",
            Id = "3-3-9",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Audio },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void AudioEncoderConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");

                    AudioSourceConfiguration[] sourceConfigurations = GetAudioSourceConfigurations();

                    Assert(sourceConfigurations != null && sourceConfigurations.Length > 0,
                       "DUT did not return any audio source configuration",
                       "Check if the DUT returned audio source configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    AudioEncoderConfiguration configuration = configurations[0];

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        RemoveAudioEncoderConfiguration(newProfile.token);
                        // fix for UseCount
                        if (configuration.token == newProfile.AudioEncoderConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AudioSourceConfiguration sc1 = sourceConfigurations[0];

                    AddAudioSourceConfiguration(newProfile.token, sc1.token);

                    AddAudioEncoderConfiguration(token, configuration.token);

                    DeleteProfile(newProfile.token);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    AudioEncoderConfiguration newConfiguration = GetAudioEncoderConfiguration(configuration.token);

                    Assert(newConfiguration.UseCount == configuration.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration.UseCount, newConfiguration.UseCount),
                        "Check UseCount value after deleting profile with configuration");

                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            RestoreProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "AUDIO ENCODER CONFIGURATION USE COUNT (SET AUDIO ENCODER CONFIGURATION)",
            Order = "03.03.10",
            Id = "3-3-10",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Audio })]
        public void AudioEncoderConfigurationChangeUseCountTest()
        {
            RunTest(() =>
            {
                // Audio source configurations 

                AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                AudioEncoderConfiguration configuration = configurations[0];

                int useCount = configuration.UseCount;
                configuration.UseCount = useCount + 1;
                SetAudioEncoderConfiguration(configuration, true);

                AudioEncoderConfiguration actualConfiguration = GetAudioEncoderConfiguration(configuration.token);

                Assert(actualConfiguration.UseCount == useCount,
                string.Format("Use count value is invalid. Expected: {0}, actual: {1}", useCount, actualConfiguration.UseCount),
                "Check UseCount after setting new value via SetAudioEncoderConfiguration");
            });
        }
        
        #endregion

        #region VideoSourceConfiguration

        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (CURRENT STATE)",
            Path = PATH_2_2,
            Order = "02.02.06",
            Id = "2-2-6",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoSourceConfigurationUseCountTest()
        {
            RunTest(
                () =>
                {
                    VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                        "DUT did not return any configuration",
                        "Check if the DUT returned configurations");

                    Profile[] profiles = GetProfiles();

                    Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                    foreach (VideoSourceConfiguration configuration in configurations)
                    {
                        int useCount = configuration.UseCount;

                        int cnt = 0;
                        List<string> invalidProfiles = new List<string>();

                        foreach (Profile profile in profiles)
                        {
                            if (profile.VideoSourceConfiguration != null)
                            {
                                if (profile.VideoSourceConfiguration.token == configuration.token)
                                {
                                    cnt++;
                                    if (profile.VideoSourceConfiguration.UseCount != useCount)
                                    {
                                        invalidProfiles.Add(profile.token);
                                    }
                                }
                            }
                        }

                        Assert(cnt <= useCount,
                            string.Format("UseCount value for configuration with token '{0}' is invalid. Value in configuration: {1}, configuration is used in {2} profile(s)",
                            configuration.token, useCount, cnt));

                        if (invalidProfiles.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder("UseCount value is different when configuration is referenced from the following profiles: ");
                            bool first = true;
                            foreach (string token in invalidProfiles)
                            {
                                sb.Append(first ? token : string.Format(", {0}", token));
                                first = false;
                            }

                            Assert(false, sb.ToString(), string.Format("Check UseCount value for configuration with token '{0}' referenced from profiles", configuration.token));
                        }

                        VideoSourceConfiguration config = GetVideoSourceConfiguration(configuration.token);

                        Assert(config.UseCount == configuration.UseCount,
                            string.Format("UseCount is different when getting single configuration with token '{0}' and list of all configuration.", configuration.token),
                            "Check UseCount value");


                    }

                });
        }
        
        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (ADD SAME VIDEO SOURCE CONFIGURATION TO PROFILE TWICE)",
            Path = PATH_2_2,
            Order = "02.02.07",
            Id = "2-2-7",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoSourceConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    VideoSourceConfiguration configuration1 = configurations[0];

                    if (newProfile.VideoSourceConfiguration != null)
                    {
                        RemoveVideoSourceConfigurationFromProfile(newProfile);

                        if (configuration1.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    AddVideoSourceConfiguration(token, configuration1.token);

                    VideoSourceConfiguration newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");
                    
                    AddVideoSourceConfiguration(token, configuration1.token);

                    newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount+1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount+1, newConfiguration1.UseCount),
                        "Check UseCount value after adding the same configuration to a profile twice");

                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (ADD DIFFERENT VIDEO SOURCE CONFIGURATIONS IN PROFILE)",
            Path = PATH_2_2,
            Order = "02.02.08",
            Id = "2-2-8",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoSourceConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                    BeginStep("Check if the DUT returned video source configurations");

                    if (configurations == null || configurations.Length == 0)
                    {
                        throw new AssertException("DUT did not return any video source configuration");
                    }
                    if (configurations.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (configurations.Length == 1)
                    {
                        return;
                    }

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    VideoSourceConfiguration configuration1 = configurations[0];
                    
                    VideoSourceConfiguration configuration2 = configurations[1];

                    if (newProfile.VideoSourceConfiguration != null)
                    {
                        RemoveVideoSourceConfigurationFromProfile(newProfile);

                        // fix for UseCount
                        if (configuration1.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
                        if (configuration2.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration2.UseCount = configuration2.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddVideoSourceConfiguration(token, configuration1.token);
                    
                    VideoSourceConfiguration newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");

                    AddVideoSourceConfiguration(token, configuration2.token);

                    newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    VideoSourceConfiguration newConfiguration2 = GetVideoSourceConfiguration(configuration2.token);

                    Assert(newConfiguration2.UseCount == configuration2.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration2.UseCount + 1, newConfiguration2.UseCount),
                        "Check UseCount value after adding configuration to a profile (for added configuration)");
                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (REMOVE VIDEO SOURCE CONFIGURATION)",
            Path = PATH_2_2,
            Order = "02.02.09",
            Id = "2-2-9",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoSourceConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");

                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    VideoSourceConfiguration configuration1 = configurations[0];

                    if (newProfile.VideoSourceConfiguration != null)
                    {
                        RemoveVideoSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration1.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddVideoSourceConfiguration(token, configuration1.token);

                    RemoveVideoSourceConfiguration(token);

                    VideoSourceConfiguration newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after removing configuration from a profile");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (DELETION PROFILE WITH VIDEO SOURCE CONFIGURATION)",
            Path = PATH_2_2,
            Order = "02.02.10",
            Id = "2-2-10",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoSourceConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any configuration",
                           "Check if the DUT returned configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    VideoSourceConfiguration configuration = configurations[0];

                    if (newProfile.VideoSourceConfiguration != null)
                    {
                        RemoveVideoSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    AddVideoSourceConfiguration(token, configuration.token);

                    DeleteProfile(newProfile.token);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    VideoSourceConfiguration newConfiguration = GetVideoSourceConfiguration(configuration.token);

                    Assert(newConfiguration.UseCount == configuration.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration.UseCount, newConfiguration.UseCount),
                        "Check UseCount value after deleting profile with configuration");
                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            RestoreProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (SET VIDEO SOURCE CONFIGURATION)",
            Order = "02.02.11",
            Id = "2-2-11",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoSourceConfigurationChangeUseCountTest()
        {
            RunTest(() =>
            {
                // Video source configurations 

                VideoSourceConfiguration[] configurations = GetVideoSourceConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                VideoSourceConfiguration configuration = configurations[0];

                int useCount = configuration.UseCount;
                configuration.UseCount = useCount + 1;
                SetVideoSourceConfiguration(configuration, true);

                VideoSourceConfiguration actualConfiguration = GetVideoSourceConfiguration(configuration.token);

                Assert(actualConfiguration.UseCount == useCount,
                string.Format("Use count value is invalid. Expected: {0}, actual: {1}", useCount, actualConfiguration.UseCount),
                "Check UseCount after setting new value via SetVideoSourceConfiguration");
            });
        }

        #endregion

        #region VideoEncoderConfiguration

        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (CURRENT STATE)",
            Path = PATH_2_3,
            Order = "02.03.05",
            Id = "2-3-5",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoEncoderConfigurationUseCountTest()
        {
            RunTest(
                () =>
                {
                    VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                        "DUT did not return any configuration",
                        "Check if the DUT returned configurations");

                    Profile[] profiles = GetProfiles();

                    Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                    foreach (VideoEncoderConfiguration configuration in configurations)
                    {
                        int useCount = configuration.UseCount;

                        int cnt = 0;
                        List<string> invalidProfiles = new List<string>();

                        foreach (Profile profile in profiles)
                        {
                            if (profile.VideoEncoderConfiguration != null)
                            {
                                if (profile.VideoEncoderConfiguration.token == configuration.token)
                                {
                                    cnt++;
                                    if (profile.VideoEncoderConfiguration.UseCount != useCount)
                                    {
                                        invalidProfiles.Add(profile.token);
                                    }
                                }
                            }
                        }

                        Assert(cnt <= useCount,
                            string.Format("UseCount value for configuration with token '{0}' is invalid. Value in configuration: {1}, configuration is used in {2} profile(s)",
                            configuration.token, useCount, cnt));

                        if (invalidProfiles.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder("UseCount value is different when configuration is referenced from the following profiles: ");
                            bool first = true;
                            foreach (string token in invalidProfiles)
                            {
                                sb.Append(first ? token : string.Format(", {0}", token));
                                first = false;
                            }

                            Assert(false, sb.ToString(), string.Format("Check UseCount value for configuration with token '{0}' referenced from profiles", configuration.token));
                        }

                        VideoEncoderConfiguration config = GetVideoEncoderConfiguration(configuration.token);

                        Assert(config.UseCount == configuration.UseCount,
                            string.Format("UseCount is different when getting single configuration with token '{0}' and list of all configuration.", configuration.token),
                            "Check UseCount value");
                    }
                });
        }
        
        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (ADD SAME VIDEO ENCODER CONFIGURATION TO PROFILE TWICE)",
            Path = PATH_2_3,
            Order = "02.03.06",
            Id = "2-3-6",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoEncoderConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();

                    Assert(sourceConfigurations != null && sourceConfigurations.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }
  
                    VideoEncoderConfiguration configuration1 = configurations[0];

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        if (configuration1.token == newProfile.VideoEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    VideoSourceConfiguration vsc1 = sourceConfigurations[0];

                    AddVideoSourceConfiguration(token, vsc1.token);

                    AddVideoEncoderConfiguration(token, configuration1.token);

                    VideoEncoderConfiguration newConfiguration1 = GetVideoEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");
                    
                    AddVideoEncoderConfiguration(token, configuration1.token);

                    newConfiguration1 = GetVideoEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding the same configuration to a profile twice");
                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (ADD DIFFERENT VIDEO ENCODER CONFIGURATIONS IN PROFILE)",
            Path = PATH_2_3,
            Order = "02.03.07",
            Id = "2-3-7",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoEncoderConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                    BeginStep("Check if the DUT returned video encoder configurations");

                    if (configurations == null || configurations.Length == 0)
                    {
                        throw new AssertException("DUT did not return any video encoder configuration");
                    }
                    if (configurations.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (configurations.Length == 1)
                    {
                        return;
                    }

                    VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();

                    Assert(sourceConfigurations != null && sourceConfigurations.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }
                    
                    VideoEncoderConfiguration configuration1 = configurations[0];

                    VideoEncoderConfiguration configuration2 = configurations[1];

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        RemoveVideoEncoderConfiguration(newProfile.token);


                        if (configuration1.token == newProfile.VideoEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                        if (configuration2.token == newProfile.VideoEncoderConfiguration.token)
                        {
                            configuration2.UseCount = configuration2.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    VideoSourceConfiguration vsc1 = sourceConfigurations[0];

                    AddVideoSourceConfiguration(token, vsc1.token);

                    AddVideoEncoderConfiguration(token, configuration1.token);

                    VideoEncoderConfiguration newConfiguration1 = GetVideoEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");

                    AddVideoEncoderConfiguration(token, configuration2.token);

                    newConfiguration1 = GetVideoEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    VideoEncoderConfiguration newConfiguration2 = GetVideoEncoderConfiguration(configuration2.token);

                    Assert(newConfiguration2.UseCount == configuration2.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration2.UseCount + 1, newConfiguration2.UseCount),
                        "Check UseCount value after adding configuration to a profile (for added configuration)");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (REMOVE VIDEO ENCODER CONFIGURATION)",
            Path = PATH_2_3,
            Order = "02.03.08",
            Id = "2-3-8",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoEncoderConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();

                    Assert(sourceConfigurations != null && sourceConfigurations.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }
  
                    VideoEncoderConfiguration configuration1 = configurations[0];

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        RemoveVideoEncoderConfiguration(newProfile.token);
                        // fix for UseCount
                        if (configuration1.token == newProfile.VideoEncoderConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

                    }

                    string token = newProfile.token;

                    VideoSourceConfiguration vsc1 = sourceConfigurations[0];

                    AddVideoSourceConfiguration(token, vsc1.token);

                    AddVideoEncoderConfiguration(token, configuration1.token);

                    RemoveVideoEncoderConfiguration(token);

                    VideoEncoderConfiguration newConfiguration1 = GetVideoEncoderConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount, newConfiguration1.UseCount),
                        "Check UseCount value after removing configuration from a profile");


                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            ResetProfile(newProfile);
                        }
                    }
                });
        }

        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (PROFILE DELETION WITH VIDEO ENCODER CONFIGURATION)",
            Path = PATH_2_3,
            Order = "02.03.09",
            Id = "2-3-9",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.Video },
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void VideoEncoderConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                    Assert(configurations != null && configurations.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] sourceConfigurations = GetVideoSourceConfigurations();

                    Assert(sourceConfigurations != null && sourceConfigurations.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");
                    
                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }
  
                    VideoEncoderConfiguration configuration = configurations[0];

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        // fix for UseCount
                        if (configuration.token == newProfile.VideoEncoderConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }
                    }

                    string token = newProfile.token;

                    VideoSourceConfiguration vsc1 = sourceConfigurations[0];

                    AddVideoSourceConfiguration(token, vsc1.token);

                    AddVideoEncoderConfiguration(token, configuration.token);

                    DeleteProfile(newProfile.token);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    VideoEncoderConfiguration newConfiguration = GetVideoEncoderConfiguration(configuration.token);

                    Assert(newConfiguration.UseCount == configuration.UseCount,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration.UseCount, newConfiguration.UseCount),
                        "Check UseCount value after deleting profile with configuration");
                },
                () =>
                {
                    if (newProfile != null)
                    {
                        if (!existing)
                        {
                            DeleteProfile(newProfile.token);
                        }
                        else
                        {
                            RestoreProfile(newProfile);
                        }
                    }
                });
        }
        
        [Test(Name = "VIDEO ENCODER CONFIGURATION USE COUNT (SET VIDEO ENCODER CONFIGURATION)",
            Order = "02.03.10",
            Id = "2-3-10",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.ConditionalMust,
            RequiredFeatures = new Feature[] { Feature.Video })]
        public void VideoEncoderConfigurationChangeUseCountTest()
        {
            RunTest(() =>
            {
                // Video encoder configurations 

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                VideoEncoderConfiguration configuration = configurations[0];

                int useCount = configuration.UseCount;
                configuration.UseCount = useCount + 1;
                SetVideoEncoderConfiguration(configuration, true);

                VideoEncoderConfiguration actualConfiguration = GetVideoEncoderConfiguration(configuration.token);

                Assert(actualConfiguration.UseCount == useCount,
                string.Format("Use count value is invalid. Expected: {0}, actual: {1}", useCount, actualConfiguration.UseCount),
                "Check UseCount after setting new value via SetVideoEncoderConfiguration");
            });
        }

        #endregion

        #region Utils
        
        void RemoveVideoSourceConfigurationFromProfile(Profile profile)
        {
            if (profile.VideoEncoderConfiguration != null)
            {
                RemoveVideoEncoderConfiguration(profile.token);
            }

            RemoveVideoSourceConfiguration(profile.token);
        }

        void RemoveAudioSourceConfigurationFromProfile(Profile profile)
        {
            if (profile.AudioEncoderConfiguration != null)
            {
                RemoveAudioEncoderConfiguration(profile.token);
            }

            RemoveAudioSourceConfiguration(profile.token);
        }

        #endregion

    }

}
