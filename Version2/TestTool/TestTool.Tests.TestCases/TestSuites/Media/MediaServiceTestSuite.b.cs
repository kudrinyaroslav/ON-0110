#define ERRATA

///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using System;


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
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioSourceConfigurations, Functionality.GetAudioSourceConfiguration })]
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
            Order = "03.02.12",
            Id = "3-2-12",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.GetAudioSourceConfigurations, 
                                             Functionality.AddAudioSourceConfiguration, 
                                             Functionality.GetAudioSourceConfiguration
                                         })]
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
#if ERRATA
                        RemoveAudioSourceConfigurationFromProfile(newProfile);

                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
#else
                        RemoveAudioSourceConfiguration(newProfile.token);
#endif
                    }

                    string token = newProfile.token;

                    AddAudioSourceConfiguration(token, configuration1.token);

                    AudioSourceConfiguration newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");


                    AddAudioSourceConfiguration(token, configuration1.token);

                    newConfiguration1 = GetAudioSourceConfiguration(configuration1.token);

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

        [Test(Name = "AUDIO SOURCE CONFIGURATION USE COUNT (ADD DIFFERENT AUDIO SOURCE CONFIGURATIONS IN PROFILE)",
            Path = PATH_3_2,
            Order = "03.02.13",
            Id = "3-2-13",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.AddAudioSourceConfiguration, Functionality.GetAudioSourceConfiguration })]
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
#if ERRATA
                        RemoveAudioSourceConfigurationFromProfile(newProfile);

                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
                        if (configuration2.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration2.UseCount = configuration2.UseCount - 1;
                        }
#else
                        RemoveAudioSourceConfiguration(newProfile.token);
#endif
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
            Order = "03.02.14",
            Id = "3-2-14",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveAudioSourceConfiguration })]
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
#if ERRATA
                        RemoveAudioSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration1.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
#else
                        RemoveAudioSourceConfiguration(newProfile.token);
#endif
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
            Order = "03.02.15",
            Id = "3-2-15",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteProfile })]
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
#if ERRATA
                        RemoveAudioSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration.token == newProfile.AudioSourceConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }
#else
                        RemoveAudioSourceConfiguration(newProfile.token);
#endif
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
            Order = "03.02.16",
            Id = "3-2-16",
            Category = Category.MEDIA,
            Path = PATH_3_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioSourceConfiguration })]
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
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.AddAudioEncoderConfiguration })]
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
            Order = "03.03.12",
            Id = "3-3-12",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.AddAudioEncoderConfiguration })]
        public void AudioEncoderConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {
                    AudioEncoderConfiguration[] AECs = GetAudioEncoderConfigurations();

                    Assert(AECs != null && AECs.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");

                    AudioSourceConfiguration[] ASCs = GetAudioSourceConfigurations();

                    Assert(ASCs != null && ASCs.Length > 0,
                           "DUT did not return any audio source configuration",
                           "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        // in existing profile remove AEC
                        string removedAecToken = newProfile.AudioEncoderConfiguration.token;
                        RemoveAudioEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveAudioEncoderConfiguration
                        int index = Array.FindIndex(AECs, aec => aec.token == removedAecToken);
                        AECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    AudioSourceConfiguration[] AscCompatible = GetCompatibleAudioSourceConfigurations(profileToken);
                    AudioEncoderConfiguration[] AecCompatible = null;

                    // if there wasn't found any compatible ASC then pass the test
                    if (AscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible audio source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var asc in AscCompatible)
                    {
                        // add ASC
                        AddAudioSourceConfiguration(profileToken, asc.token);

                        // get compatible AEC
                        AecCompatible = GetCompatibleAudioEncoderConfigurations(profileToken);

                        // if there is no compatible AEC, take next ASC
                        if (AecCompatible == null) continue;

                        // add compatible AEC
                        AddAudioEncoderConfiguration(profileToken, AecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible AEC then pass the test
                    if (AecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible audio encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // get added AEC
                    AudioEncoderConfiguration AecAfterAdding = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // find array index of added AEC
                    int index2 = Array.FindIndex(AECs, aec => aec.token == AecAfterAdding.token);

                    // UseCount value should be increased
                    Assert(AecAfterAdding.UseCount == AECs[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount + 1, AecAfterAdding.UseCount),
                           "Check UseCount value after adding configuration to a profile");

                    // add AEC once again
                    AddAudioEncoderConfiguration(profileToken, AecCompatible[0].token);

                    // get added twice AEC
                    AudioEncoderConfiguration AecAfterTwiceAdding = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // after twice adding AEC to profile, UseCount value should stay constant since previous adding
                    Assert(AecAfterTwiceAdding.UseCount == AECs[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount + 1, AecAfterTwiceAdding.UseCount),
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
            Order = "03.03.13",
            Id = "3-3-13",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.AddAudioEncoderConfiguration })]
        public void AudioEncoderConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    AudioEncoderConfiguration[] AECs = GetAudioEncoderConfigurations();

                    BeginStep("Check if the DUT returned audio encoder configurations");

                    if (AECs == null || AECs.Length == 0)
                    {
                        throw new AssertException("DUT did not return any audio encoder configuration");
                    }
                    if (AECs.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (AECs.Length == 1)
                    {
                        return;
                    }

                    AudioSourceConfiguration[] ASCs = GetAudioSourceConfigurations();

                    Assert(ASCs != null && ASCs.Length > 0,
                           "DUT did not return any audio source configuration",
                           "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        // in existing profile remove AEC
                        string removedAecToken = newProfile.AudioEncoderConfiguration.token;
                        RemoveAudioEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveAudioEncoderConfiguration
                        int index = Array.FindIndex(AECs, aec => aec.token == removedAecToken);
                        AECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    AudioSourceConfiguration[] AscCompatible = GetCompatibleAudioSourceConfigurations(profileToken);
                    AudioEncoderConfiguration[] AecCompatible = null;

                    // if there wasn't found any compatible ASC then pass the test
                    if (AscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible audio source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var asc in AscCompatible)
                    {
                        // add ASC
                        AddAudioSourceConfiguration(profileToken, asc.token);

                        // get compatible AEC
                        AecCompatible = GetCompatibleAudioEncoderConfigurations(profileToken);

                        // if there are less than 2 compatible AECs, take next ASC
                        if (AecCompatible == null || AecCompatible.Length < 2) continue;

                        // add compatible AEC
                        AddAudioEncoderConfiguration(profileToken, AecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible AEC then pass the test
                    if (AecCompatible == null || AecCompatible.Length < 2)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible audio encoder configurations or their count is less than 2 {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // get added Aec1
                    AudioEncoderConfiguration Aec1 = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // find array index of the Aec1
                    int index2 = Array.FindIndex(AECs, aec => aec.token == Aec1.token);

                    // UseCount value for Aec1 should be increased
                    Assert(Aec1.UseCount == AECs[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount + 1, Aec1.UseCount),
                           "Check UseCount value after adding configuration to a profile");

                    // add compatible Aec2
                    AddAudioEncoderConfiguration(profileToken, AecCompatible[1].token);

                    // get removed Aec1
                    Aec1 = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // UseCount value for Aec1 should be decreased
                    Assert(Aec1.UseCount == AECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount, Aec1.UseCount),
                           "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    // get added Aec2
                    AudioEncoderConfiguration Aec2 = GetAudioEncoderConfiguration(AecCompatible[1].token);

                    // find array index of the Aec2
                    int index3 = Array.FindIndex(AECs, aec => aec.token == Aec2.token);

                    // UseCount value for Aec2 should be increased
                    Assert(Aec2.UseCount == AECs[index3].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index3].UseCount + 1, Aec2.UseCount),
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
            Order = "03.03.14",
            Id = "3-3-14",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveAudioEncoderConfiguration })]
        public void AudioEncoderConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {
                    AudioEncoderConfiguration[] AECs = GetAudioEncoderConfigurations();

                    Assert(AECs != null && AECs.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");

                    AudioSourceConfiguration[] ASCs = GetAudioSourceConfigurations();

                    Assert(ASCs != null && ASCs.Length > 0,
                           "DUT did not return any audio source configuration",
                           "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        // in existing profile remove AEC
                        string removedAecToken = newProfile.AudioEncoderConfiguration.token;
                        RemoveAudioEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveAudioEncoderConfiguration
                        int index = Array.FindIndex(AECs, aec => aec.token == removedAecToken);
                        AECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    AudioSourceConfiguration[] AscCompatible = GetCompatibleAudioSourceConfigurations(profileToken);
                    AudioEncoderConfiguration[] AecCompatible = null;

                    // if there wasn't found any compatible ASC then pass the test
                    if (AscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible audio source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var asc in AscCompatible)
                    {
                        // add ASC
                        AddAudioSourceConfiguration(profileToken, asc.token);

                        // get compatible AEC
                        AecCompatible = GetCompatibleAudioEncoderConfigurations(profileToken);

                        // if there are no compatible AECs, take next ASC
                        if (AecCompatible == null) continue;

                        // add compatible AEC
                        AddAudioEncoderConfiguration(profileToken, AecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible AEC then pass the test
                    if (AecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible audio encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // remove AEC
                    RemoveAudioEncoderConfiguration(profileToken);

                    // get added and removed Aec
                    AudioEncoderConfiguration Aec = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // find array index of the Aec
                    int index2 = Array.FindIndex(AECs, aec => aec.token == Aec.token);

                    // UseCount value for Aec should be the same as in the beginning of the test
                    Assert(Aec.UseCount == AECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount, Aec.UseCount),
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
            Order = "03.03.15",
            Id = "3-3-15",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteProfile })]
        public void AudioEncoderConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {
                    AudioEncoderConfiguration[] AECs = GetAudioEncoderConfigurations();

                    Assert(AECs != null && AECs.Length > 0,
                           "DUT did not return any audio encoder configuration",
                           "Check if the DUT returned audio encoder configurations");

                    AudioSourceConfiguration[] ASCs = GetAudioSourceConfigurations();

                    Assert(ASCs != null && ASCs.Length > 0,
                           "DUT did not return any audio source configuration",
                           "Check if the DUT returned audio source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.AudioEncoderConfiguration != null)
                    {
                        // in existing profile remove AEC
                        string removedAecToken = newProfile.AudioEncoderConfiguration.token;
                        RemoveAudioEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveAudioEncoderConfiguration
                        int index = Array.FindIndex(AECs, aec => aec.token == removedAecToken);
                        AECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    AudioSourceConfiguration[] AscCompatible = GetCompatibleAudioSourceConfigurations(profileToken);
                    AudioEncoderConfiguration[] AecCompatible = null;

                    // if there wasn't found any compatible ASC then pass the test
                    if (AscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible audio source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var asc in AscCompatible)
                    {
                        // add ASC
                        AddAudioSourceConfiguration(profileToken, asc.token);

                        // get compatible AEC
                        AecCompatible = GetCompatibleAudioEncoderConfigurations(profileToken);

                        // if there are no compatible AECs, take next ASC
                        if (AecCompatible == null) continue;

                        // add compatible AEC
                        AddAudioEncoderConfiguration(profileToken, AecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible AEC then pass the test
                    if (AecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible audio encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    DeleteProfile(profileToken);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    // get Aec from deleted profile
                    AudioEncoderConfiguration Aec = GetAudioEncoderConfiguration(AecCompatible[0].token);

                    // find array index of the Aec
                    int index2 = Array.FindIndex(AECs, aec => aec.token == Aec.token);

                    // UseCount value for Aec should be the same as in the beginning of the test
                    Assert(Aec.UseCount == AECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           AECs[index2].UseCount, Aec.UseCount),
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
            Order = "03.03.16",
            Id = "3-3-16",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioEncoderConfiguration })]
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

        // Heineken scope test
        [Test(Name = "AUDIO ENCODER CONFIGURATION OPTIONS AND AUDIO ENCODER CONFIGURATIONS CONSISTENCY (BITRATE AND SAMPLERATE)",
            Order = "03.03.11",
            Id = "3-3-11",
            Category = Category.MEDIA,
            Path = PATH_3_3,
            Version = 2.2,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.Audio },
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioEncoderConfiguration })]
        public void AudioEncoderConfigurationOptionsRangesTest()
        {
            RunTest(() =>
            {
                int BITRATEMAX = 10000;
                int SAMPLERATEMAX = 1000;

                //3.	ONVIF Client invokes GetAudioEncoderConfigurationOptionsRequest message 
                // to retrieve list of Audio Encoder Configuration Options from device.
                //4.	Verify the GetAudioEncoderConfigurationOptionsResponse message from the DUT. 
                // Check that all values of Options.BitrateList.Items are less than 10000. 
                // Check that all values of Options.SampleRateList.Items are less than 1000.
                AudioEncoderConfigurationOptions commonOptions = GetAudioEncoderConfigurationOptions(null, null);

                bool ok = true;

                StringBuilder dump = null;

                Func<AudioEncoderConfigurationOptions, bool> check =
                    new Func<AudioEncoderConfigurationOptions, bool>(
                        (opt) =>
                        {
                            bool optionsOk = true;
                            dump = new StringBuilder();
                            foreach (AudioEncoderConfigurationOption option in opt.Options)
                            {
                                List<int> incorrect = new List<int>();
                                bool localOk = CheckRanges(option.BitrateList, BITRATEMAX, incorrect);
                                if (!localOk)
                                {
                                    optionsOk = false;
                                    dump.AppendLine(
                                        string.Format("Value(s) {0} are incorrect for Bitrate", MediaTestUtils.DumpList(incorrect)));
                                }
                                incorrect = new List<int>();
                                localOk = CheckRanges(option.SampleRateList, SAMPLERATEMAX, incorrect);
                                if (!localOk)
                                {
                                    optionsOk = false;
                                    dump.AppendLine(
                                        string.Format("Value(s) {0} are incorrect for SampleRate", MediaTestUtils.DumpList(incorrect)));
                                }
                            }
                            return optionsOk;
                        });

                ok = check(commonOptions);

                Assert(ok, dump.ToStringTrimNewLine(), "Check Bitrate and Samplerate for all options");

                //5.	ONVIF Client invokes GetAudioEncoderConfigurationsRequest message to retrieve list of Audio Encoder Configurations from device.
                //6.	Verify the GetAudioEncoderConfigurationsResponse message from the DUT.

                AudioEncoderConfiguration[] configurations = GetAudioEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                foreach (AudioEncoderConfiguration config in configurations)
                {
                    //7.	ONVIF Client invokes GetAudioEncoderConfigurationOptionsRequest message 
                    // (ConfigurationToken = ‘token1’ where “Token1” is a first audio encoder 
                    // configuration token from GetAudioEncoderConfigurationsResponse message) 
                    // to retrieve list of Audio Encoder Configuration Options for specified 
                    // Audio Encoder Configuration from device.
                    AudioEncoderConfigurationOptions options = GetAudioEncoderConfigurationOptions(config.token, null);

                    //8.	Verify the GetAudioEncoderConfigurationOptionsResponse message from the DUT. 
                    // Check that all values of Options.BitrateList.Items are less than 10000 and 
                    // listed in Options.BitrateList of GetAudioEncoderConfigurationOptionsResponse 
                    // message from step 4. Check that all values of Options.SampleRateList.Items are 
                    // less than 1000 listed in Options.SampleRateList of 
                    // GetAudioEncoderConfigurationOptionsResponse message from step 4.

                    ok = check(options);

                    Assert(ok, dump.ToStringTrimNewLine(), "Check Bitrate and Samplerate for all options");

                    StringBuilder sb = new StringBuilder();
                    ok = true;
                    if (options.Options != null && commonOptions != null)
                    {
                        // each options for current configuration
                        foreach (AudioEncoderConfigurationOption opt in options.Options)
                        {
                            bool found = false;

                            // empty lists will not pass schema validation
                            string bitratesDescription = string.Format("all Bitrates from {{{0}}}", MediaTestUtils.DumpList(opt.BitrateList));

                            string sampleRatesDescription = string.Format("all SampleRates from {{{0}}}", MediaTestUtils.DumpList(opt.SampleRateList));

                            if (commonOptions.Options != null)
                            {
                                // find options with encoding matching;
                                foreach (AudioEncoderConfigurationOption globalOpt in commonOptions.Options)
                                {
                                    if (globalOpt.Encoding == opt.Encoding)
                                    {
                                        bool bitrateFound = false;
                                        bool sampleRateFound = false;

                                        // Encoding matches.
                                        // Check other parameters
                                        if (opt.BitrateList == null)
                                        {
                                            bitrateFound = true;
                                        }
                                        else
                                        {
                                            if (globalOpt.BitrateList != null)
                                            {
                                                bitrateFound = true;
                                                foreach (int bitrate in opt.BitrateList)
                                                {
                                                    if (!globalOpt.BitrateList.Contains(bitrate))
                                                    {
                                                        bitrateFound = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (opt.SampleRateList == null)
                                        {
                                            sampleRateFound = true;
                                        }
                                        else
                                        {
                                            if (globalOpt.SampleRateList != null)
                                            {
                                                sampleRateFound = true;
                                                foreach (int sampleRate in opt.SampleRateList)
                                                {
                                                    if (!globalOpt.SampleRateList.Contains(sampleRate))
                                                    {
                                                        sampleRateFound = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        // Encoding matches, bitrate found, samplerate found
                                        found = bitrateFound && sampleRateFound;
                                    }
                                }
                            }

                            if (!found)
                            {
                                sb.AppendLine(string.Format("Options with Encoding={0}, {1} and {2} not found in total options list",
                                    opt.Encoding, bitratesDescription, sampleRatesDescription));

                                ok = false;
                            }
                        }
                    }

                    Assert(ok,
                        sb.ToStringTrimNewLine(),
                        string.Format("Check that options received for configuration with token '{0}' are valid", config.token));

                }
                //9.	Repeat steps 7-8 for the rest Audio Encoder Configurations supported by the DUT.

            });
        }

        bool CheckRanges(int[] rates, int max, List<int> incorrect)
        {
            if (rates != null)
            {
                foreach (int rate in rates)
                {
                    if (rate > max)
                    {
                        incorrect.Add(rate);
                    }
                }
            }
            return incorrect.Count == 0;
        }

        #endregion

        #region VideoSourceConfiguration

        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (CURRENT STATE)",
            Path = PATH_2_2,
            Order = "02.02.06",
            Id = "2-2-6",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles, Functionality.GetVideoSourceConfiguration })]
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
            Order = "02.02.12",
            Id = "2-2-12",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.AddVideoSourceConfiguration })]
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
#if ERRATA
                        RemoveVideoSourceConfigurationFromProfile(newProfile);

                        // fix for UseCount
                        if (configuration1.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }
#else
                        RemoveVideoSourceConfiguration(newProfile.token);
#endif
                    }

                    string token = newProfile.token;

                    AddVideoSourceConfiguration(token, configuration1.token);

                    VideoSourceConfiguration newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

                    Assert(newConfiguration1.UseCount == configuration1.UseCount + 1,
                        string.Format("Use count value is invalid. Expected: {0}, actual: {1}", configuration1.UseCount + 1, newConfiguration1.UseCount),
                        "Check UseCount value after adding configuration to a profile");

                    AddVideoSourceConfiguration(token, configuration1.token);

                    newConfiguration1 = GetVideoSourceConfiguration(configuration1.token);

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

        [Test(Name = "VIDEO SOURCE CONFIGURATION USE COUNT (ADD DIFFERENT VIDEO SOURCE CONFIGURATIONS IN PROFILE)",
            Path = PATH_2_2,
            Order = "02.02.13",
            Id = "2-2-13",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.AddVideoSourceConfiguration })]
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
#if ERRATA
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

#else
                        RemoveVideoSourceConfiguration(newProfile.token);
#endif
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
            Order = "02.02.14",
            Id = "2-2-14",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveVideoSourceConfiguration })]
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
#if ERRATA
                        RemoveVideoSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration1.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration1.UseCount = configuration1.UseCount - 1;
                        }

#else
                        RemoveVideoSourceConfiguration(newProfile.token);
#endif
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
            Order = "02.02.15",
            Id = "2-2-15",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteProfile })]
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
#if ERRATA
                        RemoveVideoSourceConfigurationFromProfile(newProfile);
                        // fix for UseCount
                        if (configuration.token == newProfile.VideoSourceConfiguration.token)
                        {
                            configuration.UseCount = configuration.UseCount - 1;
                        }

#else
                        RemoveVideoSourceConfiguration(newProfile.token);
#endif
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
            Order = "02.02.16",
            Id = "2-2-16",
            Category = Category.MEDIA,
            Path = PATH_2_2,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetVideoEncoderConfiguration })]
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
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles, Functionality.GetVideoEncoderConfigurations })]
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
            Order = "02.03.13",
            Id = "2-3-13",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.AddVideoEncoderConfiguration })]
        public void VideoEncoderConfigurationUseCountAddTheSameConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] Vec = GetVideoEncoderConfigurations();

                    Assert(Vec != null && Vec.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] Vsc = GetVideoSourceConfigurations();

                    Assert(Vsc != null && Vsc.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        // in existing profile remove VEC
                        string removedVecToken = newProfile.VideoEncoderConfiguration.token;
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveVideoEncoderConfiguration
                        int index = Array.FindIndex(Vec, vec => vec.token == removedVecToken);
                        Vec[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    VideoSourceConfiguration[] VscCompatible = GetCompatibleVideoSourceConfigurations(profileToken);
                    VideoEncoderConfiguration[] VecCompatible = null;

                    // if there wasn't found any compatible VSC then pass the test
                    if (VscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible video source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var vsc in VscCompatible)
                    {
                        // add VSC
                        AddVideoSourceConfiguration(profileToken, vsc.token);

                        // get compatible VEC
                        VecCompatible = GetCompatibleVideoEncoderConfigurations(profileToken);

                        // if there is no compatible VEC, take next VSC
                        if (VecCompatible == null) continue;

                        // add compatible VEC
                        AddVideoEncoderConfiguration(profileToken, VecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible VEC then pass the test
                    if (VecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible video encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // get added VEC
                    VideoEncoderConfiguration VecAfterAdding = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // find array index of the same VECs
                    int index2 = Array.FindIndex(Vec, vec => vec.token == VecAfterAdding.token);

                    // UseCount value should be increased
                    Assert(VecAfterAdding.UseCount == Vec[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           Vec[index2].UseCount + 1, VecAfterAdding.UseCount),
                           "Check UseCount value after adding configuration to a profile");

                    // add VEC once again
                    AddVideoEncoderConfiguration(profileToken, VecCompatible[0].token);

                    // get added twice VEC
                    VideoEncoderConfiguration VecAfterTwiceAdding = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // after twice adding VEC to profile, UseCount value should stay constant since previous adding
                    Assert(VecAfterTwiceAdding.UseCount == Vec[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           Vec[index2].UseCount + 1, VecAfterTwiceAdding.UseCount),
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
            Order = "02.03.14",
            Id = "2-3-14",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.AddVideoEncoderConfiguration })]
        public void VideoEncoderConfigurationUseCountAddNewConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] VECs = GetVideoEncoderConfigurations();

                    BeginStep("Check if the DUT returned video encoder configurations");

                    if (VECs == null || VECs.Length == 0)
                    {
                        throw new AssertException("DUT did not return any video encoder configuration");
                    }
                    if (VECs.Length == 1)
                    {
                        LogStepEvent("There is only one configuration - no possibility to continue test");
                    }
                    StepPassed();

                    if (VECs.Length == 1)
                    {
                        return;
                    }

                    VideoSourceConfiguration[] VSCs = GetVideoSourceConfigurations();

                    Assert(VSCs != null && VSCs.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        // in existing profile remove VEC
                        string removedVecToken = newProfile.VideoEncoderConfiguration.token;
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveVideoEncoderConfiguration
                        int index = Array.FindIndex(VECs, vec => vec.token == removedVecToken);
                        VECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    VideoSourceConfiguration[] VscCompatible = GetCompatibleVideoSourceConfigurations(profileToken);
                    VideoEncoderConfiguration[] VecCompatible = null;

                    // if there wasn't found any compatible VSC then pass the test
                    if (VscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible video source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var vsc in VscCompatible)
                    {
                        // add VSC
                        AddVideoSourceConfiguration(profileToken, vsc.token);

                        // get compatible VEC
                        VecCompatible = GetCompatibleVideoEncoderConfigurations(profileToken);

                        // if there are less than 2 compatible VECs, take next VSC
                        if (VecCompatible == null || VecCompatible.Length < 2) continue;

                        // add compatible VEC
                        AddVideoEncoderConfiguration(profileToken, VecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible VEC then pass the test
                    if (VecCompatible == null || VecCompatible.Length < 2)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible video encoder configurations or their number is less than 2 {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // get added Vec1
                    VideoEncoderConfiguration Vec1 = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // find array index of the Vec1
                    int index2 = Array.FindIndex(VECs, vec => vec.token == Vec1.token);

                    // UseCount value for Vec1 should be increased
                    Assert(Vec1.UseCount == VECs[index2].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           VECs[index2].UseCount + 1, Vec1.UseCount),
                           "Check UseCount value after adding configuration to a profile");

                    // add compatible Vec2
                    AddVideoEncoderConfiguration(profileToken, VecCompatible[1].token);

                    // get removed Vec1
                    Vec1 = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // UseCount value for Vec1 should be decreased
                    Assert(Vec1.UseCount == VECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           VECs[index2].UseCount, Vec1.UseCount),
                           "Check UseCount value after replacing configuration in a profile (for replaced configuration)");

                    // get added Vec2
                    VideoEncoderConfiguration Vec2 = GetVideoEncoderConfiguration(VecCompatible[1].token);

                    // find array index of the Vec1
                    int index3 = Array.FindIndex(VECs, vec => vec.token == Vec2.token);

                    // UseCount value for Vec2 should be increased
                    Assert(Vec2.UseCount == VECs[index3].UseCount + 1,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           VECs[index3].UseCount + 1, Vec2.UseCount),
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
            Order = "02.03.15",
            Id = "2-3-15",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.RemoveVideoEncoderConfiguration })]
        public void VideoEncoderConfigurationUseCountRemoveConfigurationTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] VECs = GetVideoEncoderConfigurations();

                    Assert(VECs != null && VECs.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] VSCs = GetVideoSourceConfigurations();

                    Assert(VSCs != null && VSCs.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        // in existing profile remove VEC
                        string removedVecToken = newProfile.VideoEncoderConfiguration.token;
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveVideoEncoderConfiguration
                        int index = Array.FindIndex(VECs, vec => vec.token == removedVecToken);
                        VECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    VideoSourceConfiguration[] VscCompatible = GetCompatibleVideoSourceConfigurations(profileToken);
                    VideoEncoderConfiguration[] VecCompatible = null;

                    // if there wasn't found any compatible VSC then pass the test
                    if (VscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible video source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var vsc in VscCompatible)
                    {
                        // add VSC
                        AddVideoSourceConfiguration(profileToken, vsc.token);

                        // get compatible VEC
                        VecCompatible = GetCompatibleVideoEncoderConfigurations(profileToken);

                        // if there are no compatible VECs, take next VSC
                        if (VecCompatible == null) continue;

                        // add compatible VEC
                        AddVideoEncoderConfiguration(profileToken, VecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible VEC then pass the test
                    if (VecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible video encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    // remove VEC
                    RemoveVideoEncoderConfiguration(profileToken);

                    // get added and removed Vec
                    VideoEncoderConfiguration Vec = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // find array index of the Vec
                    int index2 = Array.FindIndex(VECs, vec => vec.token == Vec.token);

                    // UseCount value for Vec should be the same as in the beginning of the test
                    Assert(Vec.UseCount == VECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           VECs[index2].UseCount, Vec.UseCount),
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
            Order = "02.03.16",
            Id = "2-3-16",
            Category = Category.MEDIA,
            Version = 2.0,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteProfile })]
        public void VideoEncoderConfigurationUseCountDeleteProfileTest()
        {
            Profile newProfile = null;
            bool existing = false;

            RunTest(
                () =>
                {

                    VideoEncoderConfiguration[] VECs = GetVideoEncoderConfigurations();

                    Assert(VECs != null && VECs.Length > 0,
                           "DUT did not return any video encoder configuration",
                           "Check if the DUT returned video encoder configurations");

                    VideoSourceConfiguration[] VSCs = GetVideoSourceConfigurations();

                    Assert(VSCs != null && VSCs.Length > 0,
                           "DUT did not return any video source configuration",
                           "Check if the DUT returned video source configurations");

                    newProfile = CreateTestProfile(out existing);

                    if (newProfile == null)
                    {
                        return;
                    }

                    if (newProfile.VideoEncoderConfiguration != null)
                    {
                        // in existing profile remove VEC
                        string removedVecToken = newProfile.VideoEncoderConfiguration.token;
                        RemoveVideoEncoderConfiguration(newProfile.token);

                        // we need to decrement UseCount value after RemoveVideoEncoderConfiguration
                        int index = Array.FindIndex(VECs, vec => vec.token == removedVecToken);
                        VECs[index].UseCount -= 1;
                    }

                    string profileToken = newProfile.token;

                    VideoSourceConfiguration[] VscCompatible = GetCompatibleVideoSourceConfigurations(profileToken);
                    VideoEncoderConfiguration[] VecCompatible = null;

                    // if there wasn't found any compatible VSC then pass the test
                    if (VscCompatible == null)
                    {
                        LogTestEvent(String.Format(
                                     "DUT did not return any compatible video source configurations {0}",
                                     System.Environment.NewLine));
                        return;
                    }

                    foreach (var vsc in VscCompatible)
                    {
                        // add VSC
                        AddVideoSourceConfiguration(profileToken, vsc.token);

                        // get compatible VEC
                        VecCompatible = GetCompatibleVideoEncoderConfigurations(profileToken);

                        // if there are no compatible VECs, take next VSC
                        if (VecCompatible == null) continue;

                        // add compatible VEC
                        AddVideoEncoderConfiguration(profileToken, VecCompatible[0].token);
                        break;
                    }

                    // if there wasn't found any compatible VEC then pass the test
                    if (VecCompatible == null)
                    {
                        LogTestEvent(String.Format(
                            "DUT did not return any compatible video encoder configurations {0}",
                            System.Environment.NewLine));
                        return;
                    }

                    DeleteProfile(profileToken);

                    if (!existing)
                    {
                        newProfile = null;
                    }

                    // get Vec from deleted profile
                    VideoEncoderConfiguration Vec = GetVideoEncoderConfiguration(VecCompatible[0].token);

                    // find array index of the Vec
                    int index2 = Array.FindIndex(VECs, vec => vec.token == Vec.token);

                    // UseCount value for Vec should be the same as in the beginning of the test
                    Assert(Vec.UseCount == VECs[index2].UseCount,
                           string.Format("Use count value is invalid. Expected: {0}, actual: {1}",
                           VECs[index2].UseCount, Vec.UseCount),
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
            Order = "02.03.17",
            Id = "2-3-17",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.0,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.SetVideoEncoderConfiguration })]
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

        // Heineken project - December 2012
        [Test(Name = "VIDEO ENCODER CONFIGURATIONS – ALL SUPPORTED VIDEO ENCODINGS (ALL VIDEO ENCODER CONFIGURATION)",
            Order = "02.03.18",
            Id = "2-3-18",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetVideoEncoderConfigurationOptions, Functionality.SetVideoEncoderConfiguration })]
        public void VideoEncoderConfigurationAllSupportedVideoEncodingAllVideoEncoderConfigurationTest()
        {
            List<VideoEncoderConfiguration> backups = new List<VideoEncoderConfiguration>();

            RunTest(() =>
            {

                string reason;

                //3.	ONVIF Client will invoke GetVideoEncoderConfigurationsRequest message to retrieve 
                // all DUT video encoder configurations.
                //4.	Verify the GetVideoEncoderConfigurationsResponse message from the DUT.

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null,
                    "DUT did not return any configuration",
                    "Check if the DUT returned configurations");

                //List<VideoEncoderConfiguration> selectedConfigs = MediaTestUtils.SelectConfigurations(configurations);

                foreach (VideoEncoderConfiguration config in configurations)
                {

                    //5.	ONVIF Client will invoke GetVideoEncoderConfigurationOptionsRequest message 
                    // (ConfigurationToken = “Token1”, where “Token1” is a first video encoder configuration 
                    // token from GetVideoEncoderConfigurationsResponse message, no ProfileToken) to retrieve 
                    // supported video encoder configuration options.
                    //6.	Verify the GetVideoEncoderConfigurationOptionsResponse message from the DUT.
                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(config.token, null);

                    // config is null before changes are made.
                    // i.e. if an error occurs in GetVideoEncoderConfigurationOptions - no configurations need to be rolled back
                    VideoEncoderConfiguration currentConfig = Utils.CopyMaker.CreateCopy(config);
                    backups.Add(currentConfig);

                    if (options.JPEG != null)
                    {
                        VideoResolution resolution = options.JPEG.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.JPEG;
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl = new VideoRateControl();
                        config.RateControl.FrameRateLimit = options.JPEG.FrameRateRange.Max;
                        config.RateControl.BitrateLimit = 64000;
                        config.RateControl.EncodingInterval = options.JPEG.EncodingIntervalRange.Min;
                        config.H264 = null;
                        config.MPEG4 = null;

                        //7.	ONVIF Client will invoke SetVideoEncoderConfigurationRequest message 
                        // (Configuration.token = “Token1”, where “Token1” is a first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message, 
                        // Configuration.Encoding = “codec1”, where “codec1” is a first supported codec from 
                        // GetVideoEncoderConfigurationOptionsResponse message, other Configuration parameters 
                        // that are applicable for selected Encoding) to change video encoder configuration settings.
                        //8.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.
                        SetVideoEncoderConfiguration(config, false, false);

                        //9.	ONVIF Client will invoke GetVideoEncoderConfigurationRequest message 
                        // (ConfigurationToken = “Token1”, where “Token1” is a first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message) to retrieve DUT video encoder 
                        // configuration for specified token.
                        //10.	Verify the GetVideoEncoderConfigurationResponse message from the DUT. Check that 
                        // video encoder setting was applied.
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        bool ok = ConfigurationValid(newConfig, VideoEncoding.JPEG, resolution, out reason);
                        Assert(ok, reason, "Check that the DUT accepted values passed");
                    }

                    //11.	Repeat steps 7-10 for the rest Video Encodings supported by selected configuration.
                    if (options.MPEG4 != null)
                    {
                        VideoResolution resolution = options.MPEG4.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.MPEG4;
                        config.MPEG4 = new Mpeg4Configuration();
                        config.MPEG4.Mpeg4Profile = options.MPEG4.Mpeg4ProfilesSupported.Contains(Mpeg4Profile.SP)
                                ? Mpeg4Profile.SP
                                : Mpeg4Profile.ASP;
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl = new VideoRateControl();
                        config.RateControl.FrameRateLimit = options.MPEG4.FrameRateRange.Max;
                        config.RateControl.BitrateLimit = 64000;
                        config.RateControl.EncodingInterval = options.MPEG4.EncodingIntervalRange.Min;
                        config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Min;
                        config.H264 = null;

                        SetVideoEncoderConfiguration(config, false, false);
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        bool ok = ConfigurationValid(newConfig, VideoEncoding.MPEG4, resolution, out reason);

                        string error = string.Empty;
                        if (newConfig.MPEG4 == null)
                        {
                            ok = false;
                            error = "MPEG4 configuration not found";
                        }
                        else
                        {
                            if (newConfig.MPEG4.Mpeg4Profile != config.MPEG4.Mpeg4Profile)
                            {
                                ok = false;
                                error = string.Format("Mpeg4Profile is incorrect. Expected: {0}, actual: {1} ",
                                                             config.MPEG4.Mpeg4Profile, newConfig.MPEG4.Mpeg4Profile);

                            }
                        }

                        if (!string.IsNullOrEmpty(error))
                        {
                            if (string.IsNullOrEmpty(reason))
                            {
                                reason = error;
                            }
                            else
                            {
                                reason += System.Environment.NewLine;
                                reason += error;
                            }
                        }

                        Assert(ok, reason, "Check that the DUT accepted values passed");
                    }

                    if (options.H264 != null)
                    {

                        VideoResolution resolution = options.H264.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.H264;
                        config.H264 = new H264Configuration();

                        // max

                        H264Profile h264Profile = H264Profile.High;
                        if (options.H264.H264ProfilesSupported.Contains(H264Profile.Baseline))
                        {
                            h264Profile = H264Profile.Baseline;
                        }
                        else if (options.H264.H264ProfilesSupported.Contains(H264Profile.Main))
                        {
                            h264Profile = H264Profile.Main;
                        }
                        else if (options.H264.H264ProfilesSupported.Contains(H264Profile.Extended))
                        {
                            h264Profile = H264Profile.Extended;
                        }

                        config.H264.H264Profile = h264Profile;
                        config.RateControl = new VideoRateControl();
                        config.RateControl.BitrateLimit = 64000;
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Max;
                        config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Min;
                        config.H264.GovLength = options.H264.GovLengthRange.Min;
                        config.MPEG4 = null;

                        SetVideoEncoderConfiguration(config, false, false);
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        {
                            StringBuilder dump = new StringBuilder();

                            bool ok = ConfigurationValid(newConfig, VideoEncoding.H264, resolution, out reason);
                            if (!ok)
                            {
                                dump.AppendLine(reason);
                            }
                            if (newConfig.H264 != null)
                            {
                                if (newConfig.H264.H264Profile != config.H264.H264Profile)
                                {
                                    ok = false;
                                    reason = string.Format("H264Profile is incorrect. Expected: {0}, actual: {1} ",
                                                                 config.H264.H264Profile, newConfig.H264.H264Profile);

                                    dump.AppendLine(reason);
                                }
                            }
                            Assert(ok, dump.ToStringTrimNewLine(), "Check that the DUT accepted values passed");
                        }
                    }

                    // rollback changes somehow;

                    //SetVideoEncoderConfiguration(currentConfig,
                    //    false,
                    //    currentConfig.Multicast != null,
                    //    string.Format("SetVideoEncoderConfiguration - rollback changes made in configuration '{0}'", config.token));

                    //currentConfig = null;
                }
                //12.	Repeat steps 5-11 for the rest Video Encoder Configurations supported by the DUT.

            },
            () =>
            {
                // rollback...

                foreach (VideoEncoderConfiguration config in backups)
                {
                    SetVideoEncoderConfiguration(config,
                        false,
                        config.Multicast != null,
                        string.Format("SetVideoEncoderConfiguration - rollback changes made in configuration '{0}'", config.token));
                }

            });
        }

        // Heineken project - December 2012
        [Test(Name = "VIDEO ENCODER CONFIGURATIONS – ALL SUPPORTED VIDEO ENCODINGS",
            Order = "02.03.12",
            Id = "2-3-12",
            Category = Category.MEDIA,
            Path = PATH_2_3,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetVideoEncoderConfigurationOptions, Functionality.SetVideoEncoderConfiguration })]
        public void VideoEncoderConfigurationAllSupportedVideoEncodingTest()
        {
            List<VideoEncoderConfiguration> backups = new List<VideoEncoderConfiguration>();

            RunTest(() =>
            {

                string reason;

                //3.	ONVIF Client will invoke GetVideoEncoderConfigurationsRequest message to retrieve 
                // all DUT video encoder configurations.
                //4.	Verify the GetVideoEncoderConfigurationsResponse message from the DUT.

                VideoEncoderConfiguration[] configurations = GetVideoEncoderConfigurations();

                Assert(configurations != null, "DUT did not return any configuration", "Check if the DUT returned configurations");

                var selectedConfigs = configurations.Count() <= 2 ?
                                        configurations
                                        :
                                        new[] { configurations.First(), configurations[configurations.Count() / 2], configurations.Last() };
                foreach (VideoEncoderConfiguration config in selectedConfigs)
                {

                    //5.	ONVIF Client will invoke GetVideoEncoderConfigurationOptionsRequest message 
                    // (ConfigurationToken = “Token1”, where “Token1” is a first video encoder configuration 
                    // token from GetVideoEncoderConfigurationsResponse message, no ProfileToken) to retrieve 
                    // supported video encoder configuration options.
                    //6.	Verify the GetVideoEncoderConfigurationOptionsResponse message from the DUT.
                    VideoEncoderConfigurationOptions options = GetVideoEncoderConfigurationOptions(config.token, null);

                    // config is null before changes are made.
                    // i.e. if an error occurs in GetVideoEncoderConfigurationOptions - no configurations need to be rolled back
                    VideoEncoderConfiguration currentConfig = Utils.CopyMaker.CreateCopy(config);
                    backups.Add(currentConfig);

                    if (options.JPEG != null)
                    {
                        VideoResolution resolution = options.JPEG.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.JPEG;
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl = new VideoRateControl
                            {
                                FrameRateLimit = options.JPEG.FrameRateRange.Max,
                                BitrateLimit = 64000,
                                EncodingInterval = options.JPEG.EncodingIntervalRange.Min
                            };
                        config.H264 = null;
                        config.MPEG4 = null;

                        //7.	ONVIF Client will invoke SetVideoEncoderConfigurationRequest message 
                        // (Configuration.token = “Token1”, where “Token1” is a first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message, 
                        // Configuration.Encoding = “codec1”, where “codec1” is a first supported codec from 
                        // GetVideoEncoderConfigurationOptionsResponse message, other Configuration parameters 
                        // that are applicable for selected Encoding) to change video encoder configuration settings.
                        //8.	Verify the SetVideoEncoderConfigurationResponse message from the DUT.
                        SetVideoEncoderConfiguration(config, false, false);

                        //9.	ONVIF Client will invoke GetVideoEncoderConfigurationRequest message 
                        // (ConfigurationToken = “Token1”, where “Token1” is a first video encoder configuration 
                        // token from GetVideoEncoderConfigurationsResponse message) to retrieve DUT video encoder 
                        // configuration for specified token.
                        //10.	Verify the GetVideoEncoderConfigurationResponse message from the DUT. Check that 
                        // video encoder setting was applied.
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        bool ok = ConfigurationValid(newConfig, VideoEncoding.JPEG, resolution, out reason);
                        Assert(ok, reason, "Check that the DUT accepted values passed");
                    }

                    //11.	Repeat steps 7-10 for the rest Video Encodings supported by selected configuration.
                    if (options.MPEG4 != null)
                    {
                        VideoResolution resolution = options.MPEG4.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.MPEG4;
                        config.MPEG4 = new Mpeg4Configuration();
                        config.MPEG4.Mpeg4Profile = options.MPEG4.Mpeg4ProfilesSupported.Contains(Mpeg4Profile.SP)
                                ? Mpeg4Profile.SP
                                : Mpeg4Profile.ASP;
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl = new VideoRateControl
                            {
                                FrameRateLimit = options.MPEG4.FrameRateRange.Max,
                                BitrateLimit = 64000,
                                EncodingInterval = options.MPEG4.EncodingIntervalRange.Min
                            };
                        config.MPEG4.GovLength = options.MPEG4.GovLengthRange.Min;
                        config.H264 = null;

                        SetVideoEncoderConfiguration(config, false, false);
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        bool ok = ConfigurationValid(newConfig, VideoEncoding.MPEG4, resolution, out reason);

                        string error = string.Empty;
                        if (newConfig.MPEG4 == null)
                        {
                            ok = false;
                            error = "MPEG4 configuration not found";
                        }
                        else
                        {
                            if (newConfig.MPEG4.Mpeg4Profile != config.MPEG4.Mpeg4Profile)
                            {
                                ok = false;
                                error = string.Format("Mpeg4Profile is incorrect. Expected: {0}, actual: {1} ", config.MPEG4.Mpeg4Profile, newConfig.MPEG4.Mpeg4Profile);

                            }
                        }

                        if (!string.IsNullOrEmpty(error))
                        {
                            if (string.IsNullOrEmpty(reason))
                            {
                                reason = error;
                            }
                            else
                            {
                                reason += System.Environment.NewLine;
                                reason += error;
                            }
                        }

                        Assert(ok, reason, "Check that the DUT accepted values passed");
                    }

                    if (options.H264 != null)
                    {

                        VideoResolution resolution = options.H264.ResolutionsAvailable.OrderBy(R => R.Height * R.Width).Last();

                        config.Encoding = VideoEncoding.H264;
                        config.H264 = new H264Configuration();

                        // max

                        H264Profile h264Profile = H264Profile.High;
                        if (options.H264.H264ProfilesSupported.Contains(H264Profile.Baseline))
                        {
                            h264Profile = H264Profile.Baseline;
                        }
                        else if (options.H264.H264ProfilesSupported.Contains(H264Profile.Main))
                        {
                            h264Profile = H264Profile.Main;
                        }
                        else if (options.H264.H264ProfilesSupported.Contains(H264Profile.Extended))
                        {
                            h264Profile = H264Profile.Extended;
                        }

                        config.H264.H264Profile = h264Profile;
                        config.RateControl = new VideoRateControl { BitrateLimit = 64000 };
                        config.Resolution = resolution;
                        config.Quality = options.QualityRange.Max;
                        config.RateControl.FrameRateLimit = options.H264.FrameRateRange.Max;
                        config.RateControl.EncodingInterval = options.H264.EncodingIntervalRange.Min;
                        config.H264.GovLength = options.H264.GovLengthRange.Min;
                        config.MPEG4 = null;

                        SetVideoEncoderConfiguration(config, false, false);
                        VideoEncoderConfiguration newConfig = GetVideoEncoderConfiguration(config.token);

                        {
                            StringBuilder dump = new StringBuilder();

                            bool ok = ConfigurationValid(newConfig, VideoEncoding.H264, resolution, out reason);
                            if (!ok)
                            {
                                dump.AppendLine(reason);
                            }
                            if (newConfig.H264 != null)
                            {
                                if (newConfig.H264.H264Profile != config.H264.H264Profile)
                                {
                                    ok = false;
                                    reason = string.Format("H264Profile is incorrect. Expected: {0}, actual: {1} ", config.H264.H264Profile, newConfig.H264.H264Profile);

                                    dump.AppendLine(reason);
                                }
                            }
                            Assert(ok, dump.ToStringTrimNewLine(), "Check that the DUT accepted values passed");
                        }
                    }

                    // rollback changes somehow;

                    //SetVideoEncoderConfiguration(currentConfig,
                    //    false,
                    //    currentConfig.Multicast != null,
                    //    string.Format("SetVideoEncoderConfiguration - rollback changes made in configuration '{0}'", config.token));

                    //currentConfig = null;
                }
                //12.	Repeat steps 5-11 for the rest Video Encoder Configurations supported by the DUT.

            },
            () =>
            {
                // rollback...

                foreach (VideoEncoderConfiguration config in backups)
                {
                    SetVideoEncoderConfiguration(config,
                                                 false,
                                                 config.Multicast != null,
                                                 string.Format("SetVideoEncoderConfiguration - rollback changes made in configuration '{0}'", config.token));
                }
            });
        }


        #endregion

        #region Audio Output

        [Test(Name = "SET AUDIO OUTPUT CONFIGURATION",
            Path = PATH_3_4,
            Order = "03.04.04",
            Id = "3-4-4",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioOutputConfiguration },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Must)]
        public void SetAudioOutputConfigurationTest()
        {
            List<AudioOutputConfiguration> backup = new List<AudioOutputConfiguration>();

            RunTest(
                () =>
                {
                    AudioOutputConfiguration[] configurations = GetAudioOutputConfigurations();

                    Assert(configurations != null, "No audio output configurations returned from the DUT", "Check that DUT returned audio output configurations.");

                    var configurationNames = configurations.Select(e => e.Name).ToList();

                    foreach (AudioOutputConfiguration configuration in configurations)
                    {

                        AudioOutputConfiguration expected = new AudioOutputConfiguration();

                        expected.token = configuration.token;
                        expected.Name = configurationNames.GetNonMatchingString();
                        configurationNames.Add(expected.Name);

                        expected.OutputLevel = configuration.OutputLevel;
                        expected.OutputToken = configuration.OutputToken;
                        expected.SendPrimacy = configuration.SendPrimacy;

                        AudioOutputConfigurationOptions options = GetAudioOutputConfigurationOptions(configuration.token, null);


                        //OutputToken=OT1, where OT1 is one of OutputTokensAvailable from GetAudioOutputConfigurationResponse, 

                        if (options.OutputTokensAvailable != null)
                        {
                            // Change to something new.
                            // If no different values, leave as is.
                            foreach (string token in options.OutputTokensAvailable)
                            {
                                if (token != configuration.OutputToken)
                                {
                                    expected.OutputToken = token;
                                    break;
                                }
                            }
                        }

                        //SendPrimacy=SendPrimacyMode1, where SendPrimacyMode1 is one of modes from SendPrimacyOptions from Response on step 6, 
                        if (options.SendPrimacyOptions != null)
                        {
                            foreach (string mode in options.SendPrimacyOptions)
                            {
                                if (mode != configuration.SendPrimacy)
                                {
                                    expected.SendPrimacy = mode;
                                    break;
                                }
                            }
                        }

                        //OutputLevel=OL1, where OL1 is between OutputRange.min and OutputRange.max from response on step 6, 

                        int level = configuration.OutputLevel;
                        if (options.OutputLevelRange != null)
                        {
                            expected.OutputLevel = options.OutputLevelRange.Average();
                        }
                        if (level == expected.OutputLevel)
                        {
                            expected.OutputLevel = (options.OutputLevelRange.Max + level) / 2;
                        }

                        SetAudioOutputConfiguration(expected, false);
                        backup.Add(configuration);

                        AudioOutputConfiguration actual = GetAudioOutputConfiguration(configuration.token);

                        CheckConfiguration(expected, actual, "Check that configuration has been changed correctly");
                    }
                },
                () =>
                {
                    foreach (AudioOutputConfiguration configuration in backup)
                    {
                        SetAudioOutputConfiguration(configuration, false);
                    }
                });
        }


        [Test(Name = "SET AUDIO OUTPUT CONFIGURATION – INVALID CONFIGURATION",
            Path = PATH_3_4,
            Id = "3-4-7",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioOutputConfiguration },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            //LastChangedIn = "v14.12",
            RequirementLevel = RequirementLevel.Must)]
        public void SetAudioOutputConfigurationInvalidTokenTest()
        {
            RunTest(() =>
                    {
                        var configurations = GetAudioOutputConfigurations();

                        Assert(configurations != null && configurations.Any(),
                               "No audio output configurations returned from the DUT",
                               "Check that DUT returned audio output configurations.");

                        string token = configurations.Select(C => C.token).GetNonMatchingString();
                        //string outputToken = configurations.Select(C => C.OutputToken).GetNonMatchingString();

                        //var expected = new AudioOutputConfiguration { token = token, OutputToken = outputToken };
                        var expected = configurations.First();
                        expected.token = token;

                        RunStep(() => Client.SetAudioOutputConfiguration(expected, false),
                                "Set audio output configuration - negative test",
                                "Sender/InvalidArgVal/NoConfig",
                                true);

                        DoRequestDelay();
                    });
        }


        [Test(Name = "SET AUDIO OUTPUT CONFIGURATION – INVALID OUTPUTTOKEN",
            Path = PATH_3_4,
            Order = "03.04.06",
            Id = "3-4-6",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetAudioOutputConfiguration },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Must)]
        public void SetAudioOutputConfigurationInvalidOutputTokenTest()
        {
            RunTest(
                () =>
                {
                    AudioOutputConfiguration[] configurations = GetAudioOutputConfigurations();

                    Assert(configurations != null, "No audio output configurations returned from the DUT", "Check that DUT returned audio output configurations.");

                    foreach (AudioOutputConfiguration configuration in configurations)
                    {
                        AudioOutputConfigurationOptions options = GetAudioOutputConfigurationOptions(
                            configuration.token, null);

                        AudioOutputConfiguration testConfiguration = new AudioOutputConfiguration();
                        testConfiguration.Name = configuration.Name;
                        testConfiguration.token = configuration.token;
                        if (options.OutputTokensAvailable != null)
                        {
                            testConfiguration.OutputToken = options.OutputTokensAvailable.GetNonMatchingString();
                        }
                        else
                        {
                            testConfiguration.OutputToken = "InvlidOutputToken";
                        }
                        if (options.SendPrimacyOptions != null)
                        {
                            List<string> uris = new List<string>();
                            foreach (string uri in options.SendPrimacyOptions)
                            {
                                uris.Add(uri.ToLower().Replace("http://", ""));
                            }

                            testConfiguration.SendPrimacy = "http://" + uris.GetNonMatchingString();
                        }
                        else
                        {
                            testConfiguration.SendPrimacy = "http://someuri";
                        }
                        if (options.OutputLevelRange != null)
                        {
                            testConfiguration.OutputLevel = options.OutputLevelRange.Max + 1;
                        }

                        RunStep(
                            () => { Client.SetAudioOutputConfiguration(testConfiguration, false); },
                            "Set audio output configuration - negative test",
                            "Sender/InvalidArgVal/ConfigModify",
                            true);

                        DoRequestDelay();

                        AudioOutputConfiguration actual = GetAudioOutputConfiguration(configuration.token);

                        CheckConfiguration(configuration, actual, "Check that configuration has not been changed");

                    }
                });
        }
#if __PROFILE_A__
        [Test(Name = "GET AUDIO OUTPUT CONFIGURATION OPTIONS",
            Path = PATH_3_4,
            Order = "03.04.08",
            Id = "3-4-8",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles, Functionality.GetAudioOutputConfigurations, Functionality.GetAudioOutputConfigurationOptions },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void GetAudioOutputConfigurationOptionsTest()
        {
            RunTest(
            () =>
            {
                Profile[] Profiles = GetProfiles();
                Assert((Profiles != null) && (Profiles.Length > 0), "No profiles available", "Checking Profiles");
                AudioOutputConfiguration[] Configurations = GetAudioOutputConfigurations();
                Assert((Configurations != null) && (Configurations.Length > 0), "No AudioOutputConfigurations available", "Checking AudioOutputConfigurations");
                AudioOutputConfigurationOptions
                Options = GetAudioOutputConfigurationOptions(Configurations[0].token, Profiles[0].token);
                Options = GetAudioOutputConfigurationOptions(null, Profiles[0].token);
                Options = GetAudioOutputConfigurationOptions(Configurations[0].token, null);
                Options = GetAudioOutputConfigurationOptions(null, null);
            });
        }
        [Test(Name = "AUDIO OUTPUT CONFIGURATION",
            Path = PATH_3_4,
            Id = "3-4-9",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioOutputConfigurationOptions, Functionality.GetCompatibleAudioOutputConfigurations },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioOutputConfigurationTest()
        {
            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;
            Profile profile = null;
            bool RemoveAudio = false;

            RunTest(
            () =>
            {
                profile = CreateProfileByAnnex3("TestProfile", null, out deletedProfile, out createdProfile, out modifiedProfile);
                AudioOutput[] audioOutputsList = GetAudioOutputs();
                Assert((audioOutputsList != null) && (audioOutputsList.Length > 0), "No AudioOutputs available", "Checking AudioOutputs");
                AudioOutputConfiguration[] compatibleConfigurationsList = GetCompatibleAudioOutputConfigurations(profile.token);
                Assert((compatibleConfigurationsList != null) && (compatibleConfigurationsList.Length > 0), "No AudioOutputConfigurations available", "Checking AudioOutputConfigurations");
                string configurationToken = compatibleConfigurationsList[0].token;
                AddAudioOutputConfiguration(profile.token, configurationToken);
                RemoveAudio = true;
                AudioOutputConfigurationOptions Options = GetAudioOutputConfigurationOptions(configurationToken, null);
                AudioOutputConfiguration Configuration = new AudioOutputConfiguration();
                Configuration.Name = compatibleConfigurationsList[0].Name;
                Configuration.UseCount = compatibleConfigurationsList[0].UseCount;
                Configuration.token = compatibleConfigurationsList[0].token;
                Configuration.OutputToken = compatibleConfigurationsList[0].OutputToken;
                Configuration.SendPrimacy = compatibleConfigurationsList[0].SendPrimacy;
                Configuration.OutputLevel = Options.OutputLevelRange.Min - 1;
                RunStep(
                    () => { Client.SetAudioOutputConfiguration(Configuration, false); },
                    "Set audio output configuration - negative test",
                    "Sender/InvalidArgVal/ConfigModify",
                    false);

                Configuration.OutputLevel = Options.OutputLevelRange.Max;
                SetAudioOutputConfiguration(Configuration, false);

                AudioOutputConfiguration updatedConfiguration = GetAudioOutputConfiguration(Configuration.token);
                CheckConfiguration(Configuration, updatedConfiguration, "Check that configuration has been changed correctly");
            },
            () =>
            {
                if (RemoveAudio)
                {
                    RemoveAudioOutputConfiguration(profile.token);
                }
                RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
            }
            );
        }
#endif

        #region 3-4-10 AUDIO DECODER CONFIGURATION
        [Test(Name = "AUDIO DECODER CONFIGURATION",
            Path = PATH_3_4,
            Id = "3-4-10",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioDecoderConfigurations, Functionality.GetAudioDecoderConfiguration, Functionality.GetProfile },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioDecoderConfigurationTest()
        {
            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;
            Profile profile = null;

            bool RemoveAudioDecoder = false;
            bool RemoveAudioOutput = false;

            RunTest(
            () =>
            {
                profile = CreateProfileByAnnex3("TestProfile", null, out deletedProfile, out createdProfile, out modifiedProfile);
                var configurations = GetCompatibleAudioOutputConfigurations(profile.token);

                Assert((configurations != null) && (configurations.Length > 0),
                    "No CompatibleAudioOutputConfigurations available",
                    "Checking CompatibleAudioOutputConfigurations");

                string configurationToken = configurations[0].token;
                AddAudioOutputConfiguration(profile.token, configurationToken);
                RemoveAudioOutput = true;

                AudioDecoderConfiguration[] AudioDecoderConfigurations = GetAudioDecoderConfigurations();

                Assert((AudioDecoderConfigurations != null) && (AudioDecoderConfigurations.Length > 0),
                    "No AudioDecoderConfigurations available",
                    "Checking AudioDecoderConfigurations");

                AudioDecoderConfiguration[] CompatibleAudioDecoderConfigurations = GetCompatibleAudioDecoderConfigurations(profile.token);

                Assert((CompatibleAudioDecoderConfigurations != null) && (CompatibleAudioDecoderConfigurations.Length > 0),
                    "No CompatibleAudioDecoderConfigurations available",
                    "Checking CompatibleAudioDecoderConfigurations");

                var audioDecoderConfigToken = CompatibleAudioDecoderConfigurations[0].token;
                AddAudioDecoderConfiguration(profile.token, audioDecoderConfigToken);
                RemoveAudioDecoder = true;

                var profile2 = this.GetProfile(profile.token);

                Assert(profile2.Extension.AudioOutputConfiguration != null,
                    string.Format("Profile with token = {0} does not contain AudioOutputConfiguration element", profile2.token),
                    "Checking profile contains AudioOutputConfiguration");

                Assert(profile2.Extension.AudioOutputConfiguration.token == configurationToken,
                    string.Format("Profile with token = {0} does not contain AudioOutputConfiguration with token = {1}", profile2.token, configurationToken),
                    string.Format("Checking profile contains AudioOutputConfiguration with token {0}", configurationToken));

                Assert(profile2.Extension.AudioDecoderConfiguration != null,
                    string.Format("Profile with token = {0} does not contain AudioDecoderConfiguration element", profile2.token),
                    "Checking profile contains AudioDecoderConfiguration");

                Assert(profile2.Extension.AudioDecoderConfiguration.token == audioDecoderConfigToken,
                    string.Format("Profile with token = {0} does not contain AudioDecoderConfiguration with token = {1}", profile2.token, audioDecoderConfigToken),
                    string.Format("Checking profile contains AudioDecoderConfiguration with token {0}", audioDecoderConfigToken));


            },
            () =>
            {
                if (RemoveAudioDecoder)
                    RemoveAudioDecoderConfiguration(profile.token);

                if (RemoveAudioOutput)
                    RemoveAudioOutputConfiguration(profile.token);

                RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
            }
              );
        }
        #endregion

        #region 3-4-11 AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUT CONFIGURATION OPTIONS CONSISTENCY
        [Test(Name = "AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUT CONFIGURATION OPTIONS CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-11",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioOutputConfigurations, Functionality.GetAudioOutputConfigurationOptions },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioOutputConfigurationsAndAudioOutputConfigurationOptionsConsistencyTest()
        {
            RunTest(
            () =>
            {

                var configurations = this.GetAudioOutputConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio output configurations returned from the DUT",
                               "Check that DUT returned audio decoder configurations.");

                foreach (var configuration in configurations)
                {
                    var options = GetAudioOutputConfigurationOptions(configuration.token, null);

                    Assert(options.OutputTokensAvailable.Distinct().Count() == options.OutputTokensAvailable.Length,
                                string.Format("AudioOutputConfigurationOptions.OutputTokensAvailable list from audio output configuration with token = {0} contains at least two items with equal token", configuration.token),
                                "Check that DUT returned correct AudioOutputConfigurationOptions.OutputTokensAvailable.");

                    Assert(options.OutputTokensAvailable.Contains(configuration.OutputToken),
                                string.Format("AudioOutputConfigurationOptions.OutputTokensAvailable list does not contain AudioOutputConfiguration.OutputToken = '{0}'", configuration.OutputToken),
                                string.Format("Check that DUT returned AudioOutputConfigurationOptions.OutputTokensAvailable contains AudioOutputConfiguration.OutputToken. AudioOutputConfiguration token = {0}", configuration.token));

                    if (!string.IsNullOrEmpty(configuration.SendPrimacy))
                    {
                        Assert(options.SendPrimacyOptions != null && options.SendPrimacyOptions.Length > 0,
                                string.Format("AudioOutputConfigurationOptions.SendPrimacyOptions list from audio output configuration with token = {0} is skipped or empty", configuration.token),
                                "Check that DUT returned correct AudioOutputConfigurationOptions.SendPrimacyOptions.");

                        Assert(options.SendPrimacyOptions.Contains(configuration.SendPrimacy),
                                string.Format("AudioOutputConfigurationOptions.SendPrimacyOptions list from audio output configuration with token = {0} does not contain '{1}' value", configuration.token, configuration.SendPrimacy),
                                string.Format("Check that DUT AudioOutputConfigurationOptions.SendPrimacyOptions list contains {0}.", configuration.SendPrimacy));
                    }

                    Assert(configuration.OutputLevel >= options.OutputLevelRange.Min,
                                string.Format("AudioOutputConfiguration.OutputLevel with token = {0} less than AudioOutputConfigurationOptions.OutputLevelRange.Min", configuration.token),
                                "Check that DUT returned AudioOutputConfiguration.OutputLevel >= AudioOutputConfigurationOptions.OutputLevelRange.Min.");

                    Assert(configuration.OutputLevel <= options.OutputLevelRange.Max,
                                string.Format("AudioOutputConfiguration.OutputLevel with token = {0} more than AudioOutputConfigurationOptions.OutputLevelRange.Max", configuration.token),
                                "Check that DUT returned AudioOutputConfiguration.OutputLevel <= AudioOutputConfigurationOptions.OutputLevelRange.Max.");
                }
            }
            );
        }
        #endregion


        #region 3-4-12 PROFILES AND AUDIO OUTPUT CONFIGURATION OPTIONS CONSISTENCY
        [Test(Name = "PROFILES AND AUDIO OUTPUT CONFIGURATION OPTIONS CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-12",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles, Functionality.GetAudioOutputConfigurationOptions },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void ProfilesAndAudioOutputConfigurationOptionsConsistencyTest()
        {
            RunTest(
            () =>
            {

                var profiles = this.GetProfiles();

                Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                foreach (var profileWithAudioDecoderConfiguration in profiles.Where(e => e.Extension != null && e.Extension.AudioOutputConfiguration != null))
                {
                    var audioOutputConfiguration = profileWithAudioDecoderConfiguration.Extension.AudioOutputConfiguration;
                    var options = GetAudioOutputConfigurationOptions(audioOutputConfiguration.token, profileWithAudioDecoderConfiguration.token);

                    Assert(options.OutputTokensAvailable.Distinct().Count() == options.OutputTokensAvailable.Length,
                                string.Format("AudioOutputConfigurationOptions.OutputTokensAvailable list from audio output configuration with token = {0} contains at least two items with equal token", audioOutputConfiguration.token),
                                string.Format("Check that DUT returned correct AudioOutputConfigurationOptions.OutputTokensAvailable for profile with token = {0}.", profileWithAudioDecoderConfiguration.token));

                    Assert(options.OutputTokensAvailable.Contains(audioOutputConfiguration.OutputToken),
                                string.Format("AudioOutputConfigurationOptions.OutputTokensAvailable list does not contain AudioOutputConfiguration.OutputToken = '{0}'", audioOutputConfiguration.OutputToken, audioOutputConfiguration.token),
                                string.Format("Check that DUT returned AudioOutputConfigurationOptions.OutputTokensAvailable contains AudioOutputConfiguration.OutputToken. Profile token = {0}", profileWithAudioDecoderConfiguration.token));

                    if (!string.IsNullOrEmpty(audioOutputConfiguration.SendPrimacy))
                    {
                        Assert(options.SendPrimacyOptions != null && options.SendPrimacyOptions.Length > 0,
                                string.Format("AudioOutputConfigurationOptions.SendPrimacyOptions list from audio output configuration with token = {0} is skipped or empty", audioOutputConfiguration.token),
                                string.Format("Check that DUT returned correct AudioOutputConfigurationOptions.SendPrimacyOptions for profile with token = {0}.", profileWithAudioDecoderConfiguration.token));

                        Assert(options.SendPrimacyOptions.Contains(audioOutputConfiguration.SendPrimacy),
                                string.Format("AudioOutputConfigurationOptions.SendPrimacyOptions list from audio output configuration with token = {0} does not contain '{1}' value", audioOutputConfiguration.token, audioOutputConfiguration.SendPrimacy),
                                string.Format("Check that DUT AudioOutputConfigurationOptions.SendPrimacyOptions list contains {0} for profile with token = {1}.", audioOutputConfiguration.SendPrimacy, profileWithAudioDecoderConfiguration.token));
                    }

                    Assert(audioOutputConfiguration.OutputLevel >= options.OutputLevelRange.Min,
                                string.Format("AudioOutputConfiguration.OutputLevel with token = {0} less than AudioOutputConfigurationOptions.OutputLevelRange.Min", audioOutputConfiguration.token),
                                string.Format("Check that DUT returned AudioOutputConfiguration.OutputLevel >= AudioOutputConfigurationOptions.OutputLevelRange.Min for profile with token = {0}.", profileWithAudioDecoderConfiguration.token));

                    Assert(audioOutputConfiguration.OutputLevel <= options.OutputLevelRange.Max,
                                string.Format("AudioOutputConfiguration.OutputLevel with token = {0} more than AudioOutputConfigurationOptions.OutputLevelRange.Max", audioOutputConfiguration.token),
                                string.Format("Check that DUT returned AudioOutputConfiguration.OutputLevel <= AudioOutputConfigurationOptions.OutputLevelRange.Max for profile with token = {0}.", profileWithAudioDecoderConfiguration.token));

                }
            }
            );
        }
        #endregion

        #region 3-4-13 PROFILES AND AUDIO OUTPUT CONFIGURATIONS CONSISTENCY
        [Test(Name = "PROFILES AND AUDIO OUTPUT CONFIGURATIONS CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-13",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles, Functionality.GetAudioOutputConfigurationOptions },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void ProfilesAndAudioOutputConfigurationsConsistencyTest()
        {
            RunTest(
            () =>
            {

                var profiles = this.GetProfiles();

                Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                var configurations = this.GetAudioOutputConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio output configurations returned from the DUT",
                               "Check that DUT returned audio output configurations.");

                Assert(configurations.Select(e => e.token).Distinct().Count() == configurations.Length,
                               "Audio Output Configuration contains at least two items with equal token",
                               "Check that DUT returned correct audio output configurations.");

                foreach (var profileWithAudioOutputConfiguration in profiles.Where(e => e.Extension != null && e.Extension.AudioOutputConfiguration != null))
                {
                    var profileAudioOutputConfiguration = profileWithAudioOutputConfiguration.Extension.AudioOutputConfiguration;
                    Assert(configurations.Any(e => e.token == profileAudioOutputConfiguration.token),
                               string.Format("Audio output configurations does not contain item with token = {0}", profileAudioOutputConfiguration.token),
                               "Check that audio output configurations contains item with same token from profile Extension.AudioOutputConfiguration.");
                    StringBuilder logger = new StringBuilder();
                    var currentProfileConfiguration = configurations.Where(e => e.token == profileAudioOutputConfiguration.token).First();

                    Assert(AreEqual(profileAudioOutputConfiguration, currentProfileConfiguration, logger),
                               logger.ToStringTrimNewLine(),
                               "Check that audio output configurations fields are equal with same item from profile Extension.AudioOutputConfiguration.");
                }

            }
            );
        }
        #endregion

        #region 3-4-14 AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUT CONFIGURATION CONSISTENCY
        [Test(Name = "AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUT CONFIGURATION CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-14",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioOutputConfigurations, Functionality.GetAudioOutputConfiguration },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioOutputConfigurationsAndAudioOutputConfigurationConsistencyTest()
        {
            RunTest(
            () =>
            {

                var configurations = this.GetAudioOutputConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio output configurations returned from the DUT",
                               "Check that DUT returned audio output configurations.");

                foreach (var configuration in configurations)
                {
                    var configuration2 = GetAudioOutputConfiguration(configuration.token);

                    StringBuilder logger = new StringBuilder();
                    Assert(AreEqual(configuration, configuration2, logger),
                        logger.ToStringTrimNewLine(),
                        "Check that item from audio output configurations is equals to GetAudioOutputConfiguration item.");
                }

            }
            );
        }
        #endregion

        #region 3-4-15 AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUTS CONSISTENCY
        [Test(Name = "AUDIO OUTPUT CONFIGURATIONS AND AUDIO OUTPUTS CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-15",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioOutputConfigurations, Functionality.GetAudioOutputs },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioOutputConfigurationsAndAudioOutputsConsistencyTest()
        {
            RunTest(
            () =>
            {

                var configurations = this.GetAudioOutputConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio output configurations returned from the DUT",
                               "Check that DUT returned audio output configurations.");

                var outputs = this.GetAudioOutputs();

                Assert(outputs != null && outputs.Any(),
                               "No audio outputs returned from the DUT",
                               "Check that DUT returned audio outputs.");

                Assert(outputs.Select(e => e.token).Distinct().Count() == outputs.Length,
                               "Audio outputs contains at least two items with equal token",
                               "Check that DUT returned correct audio outputs.");

                foreach (var configuration in configurations)
                {
                    Assert(outputs.Any(e => e.token == configuration.OutputToken),
                        string.Format("Audio outputs does not contain item with token = {0}", configuration.OutputToken),
                        "Check that item from audio outputs contains token from AudioOutputConfiguration.OutputToken.");
                }

            }
            );
        }
        #endregion

        #region 3-4-16 PROFILES AND AUDIO DECODER CONFIGURATIONS CONSISTENCY
        [Test(Name = "PROFILES AND AUDIO DECODER CONFIGURATIONS CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-16",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioDecoderConfigurations, Functionality.GetProfiles },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void ProfilesAndAudioDecoderConfigurationsConsistencyTest()
        {
            RunTest(
            () =>
            {

                var profiles = this.GetProfiles();

                Assert(profiles != null && profiles.Length > 0,
                        "DUT did not return any profile",
                        "Check if the DUT returned media profiles");

                var configurations = this.GetAudioDecoderConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio decoder configurations returned from the DUT",
                               "Check that DUT returned audio decoder configurations.");

                Assert(configurations.Select(e => e.token).Distinct().Count() == configurations.Length,
                               "Audio decoder configuration contains at least two items with equal token",
                               "Check that DUT returned correct audio decoder configurations.");

                foreach (var profileWithAudioDecoderConfiguration in profiles.Where(e => e.Extension != null && e.Extension.AudioDecoderConfiguration != null))
                {
                    var profileAudioDecoderConfiguration = profileWithAudioDecoderConfiguration.Extension.AudioDecoderConfiguration;
                    Assert(configurations.Any(e => e.token == profileAudioDecoderConfiguration.token),
                               string.Format("Audio decoder configurations does not contain item with token = {0}", profileAudioDecoderConfiguration.token),
                               "Check that audio decoder configurations contains item with same token from profile Extension.AudioDecoderConfiguration.");
                    StringBuilder logger = new StringBuilder();
                    var currentProfileConfiguration = configurations.Where(e => e.token == profileAudioDecoderConfiguration.token).First();

                    Assert(AreEqual(profileAudioDecoderConfiguration, currentProfileConfiguration, logger),
                               logger.ToStringTrimNewLine(),
                               "Check that audio decoder configurations fields are equal with same item from profile Extension.AudioDecoderConfiguration.");
                }
            }
            );
        }
        #endregion

        #region 3-4-17 AUDIO DECODER CONFIGURATIONS AND AUDIO DECODER CONFIGURATION CONSISTENCY
        [Test(Name = "AUDIO DECODER CONFIGURATIONS AND AUDIO DECODER CONFIGURATION CONSISTENCY",
            Path = PATH_3_4,
            Id = "3-4-17",
            LastChangedIn = "v15.06",
            Category = Category.MEDIA,
            Version = 2.0,
            FunctionalityUnderTest = new Functionality[] { Functionality.GetAudioDecoderConfigurations, Functionality.GetAudioDecoderConfiguration },
            RequiredFeatures = new Feature[] { Feature.MediaService, Feature.AudioOutput },
            RequirementLevel = RequirementLevel.Optional)]
        public void AudioDecoderConfigurationsAndAudioDecoderConfigurationConsistencyTest()
        {
            RunTest(
            () =>
            {

                var configurations = this.GetAudioDecoderConfigurations();

                Assert(configurations != null && configurations.Any(),
                               "No audio decoder configurations returned from the DUT",
                               "Check that DUT returned audio decoder configurations.");

                foreach (var configuration in configurations)
                {
                    var configuration2 = GetAudioDecoderConfiguration(configuration.token);

                    StringBuilder logger = new StringBuilder();
                    Assert(AreEqual(configuration, configuration2, logger),
                        logger.ToStringTrimNewLine(),
                        "Check that item from audio decoder configurations is equals to GetAudioDecoderConfiguration item.");
                }

            }
            );
        }
        #endregion

        #endregion


        #region Utils

#if ERRATA

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

#endif

        #endregion

    }

}
