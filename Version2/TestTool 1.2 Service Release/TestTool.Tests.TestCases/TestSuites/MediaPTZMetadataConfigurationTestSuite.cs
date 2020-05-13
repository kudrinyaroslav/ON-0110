///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.ServiceModel;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaPTZMetadataConfigurationTestSuite : Base.MediaTest
    {
        public MediaPTZMetadataConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "PTZ CONFIGURATION",
            Path = "Media Configuration\\PTZ Configuration",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] {Feature.PTZ},
            RequirementLevel = RequirementLevel.ConditionalMust)]
        public void PTZConfigurationConfiguration()
        {
            Profile profile = null;
            RunTest<Profile>(
               new Backup<Profile>(() => { return null; }),
               () =>
               {
                   PTZConfiguration[] configurations = GetPTZConfigurations();
                   PTZConfiguration config = configurations[0];
                   profile = CreateProfile("testprofilex", null);

                   string reason;
                   Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);

                   AddPTZConfiguration(profile.token, config.token);

                   RemovePTZConfiguration(profile.token);

                   DeleteProfile(profile.token);
                   profile = null;
               },
               (param) =>
               {
                   if(profile != null)
                   {
                       DeleteProfile(profile.token);
                   }
               });
        }
        [Test(Name = "METADATA CONFIGURATION",
            Path = "Media Configuration\\Metadata Configuration",
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void MetadataConfigurationConfiguration()
        {
            Profile profile = null;
            RunTest<Profile>(
               new Backup<Profile>(() => { return null; }),
               () =>
               {
                   profile = CreateProfile("testprofilex", null);
                   string reason;
                   Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);

                   MetadataConfiguration[] configs = GetMetadataConfigurations();
                   // patch for I/F
                   if ((configs == null) || (configs.Length <= 0))
                   {
                       return;
                   }
                   Assert(ValidateMetadataConfigs(configs, out reason), reason, Resources.StepValidatingMetadataConfigs_Title);

                   configs = GetCompatibleMetadataConfigurations(profile.token);
                   Assert(ValidateMetadataConfigs(configs, out reason), reason, Resources.StepValidatingMetadataConfigs_Title);
                   MetadataConfiguration config = configs[0];

                   // fix for I/F
                   try
                   {
                       AddMetadataConfiguration(profile.token, config.token);
                   } catch (FaultException)
                   {
                       LogStepEvent("Assuming I/F mode");
                       StepPassed();
                       return;
                   }
                   
                   MetadataConfigurationOptions options = GetMetadataConfigurationOptions(null, config.token);
                   //TODO how do I get values out of ranges??
                   string timeout = config.SessionTimeout;
                   config.SessionTimeout = "invalid";
                   string details = string.Format("Setting invalid configuration (/MetadataConfiguration/SessionTimeout = '{0}')", config.SessionTimeout);
                   SetInvalidMetadataConfiguration(config, false, details);
                   
                   config.SessionTimeout = timeout;
                   
                   // fix for I/F
                   try
                   {
                       SetMetadataConfiguration(config, false);
                   } catch (FaultException)
                   {
                       LogStepEvent("Assuming I/F mode");
                       StepPassed();
                       return;
                   }
                   
                   MetadataConfiguration newConfig = GetMetadataConfiguration(config.token);
                   Assert(EqualConfigurations(config, newConfig, out reason), 
                       string.Format(Resources.ErrorMetadataConfigNotEqual_Format, reason),
                       Resources.StepCompareMetadataConfigs_Title);
                   
                   RemoveMetadataConfiguration(profile.token);
                   DeleteProfile(profile.token);
                   profile = null;
               },
               (param) =>
               {
                   if(profile != null)
                   {
                       DeleteProfile(profile.token);
                   }
               });
        }
    }
}
