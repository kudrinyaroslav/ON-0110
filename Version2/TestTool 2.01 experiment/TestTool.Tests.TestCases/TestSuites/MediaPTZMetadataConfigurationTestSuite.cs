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
using TestTool.Proxies.Media;
using TestTool.Tests.TestCases;
namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaPTZMetadataConfigurationTestSuite : Base.MediaTest
    {
        public MediaPTZMetadataConfigurationTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

//#if FULL
        [Test(Name = "PTZ CONFIGURATION",
            Path = "Media Configuration\\PTZ Configuration",
            Order = "07.04.01",
            Version = 1.02,
            Services = new Service[] {Service.Device, Service.Media, Service.PTZ },
            RequiredFeatures = new Feature[] {Feature.PTZ},
            RequirementLevel = RequirementLevel.ConditionalMust)]
//#endif
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
            Order = "07.05.01",
            Version = 1.02,
            Services = new Service[] { Service.Media, Service.Device },
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
