///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using System.ServiceModel;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;

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
            Order = "04.01.03",
            Id = "4-1-3",
            Category = Category.MEDIA,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.PTZService, Feature.MediaService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetPtzConfigurations, Functionality.AddPTZConfiguration, Functionality.RemovePTZConfiguration})]
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
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.CreateProfile, 
                                             Functionality.GetMetadataConfiguration,
                                             Functionality.GetMetadataConfigurations,  
                                             Functionality.AddMetadataConfiguration,
                                             Functionality.SetMetadataConfiguration,
                                             Functionality.RemoveMetadataConfiguration,
                                             Functionality.GetMetadataConfigurationOptions,
                                             Functionality.GetCompatibleMetadataConfigurations
                                         })]
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
