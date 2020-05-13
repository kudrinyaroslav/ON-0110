///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using System.Linq;
using TestTool.Tests.Engine.Base;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class MediaProfileTestSuite : Base.MediaTest
    {
        public MediaProfileTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        [Test(Name = "MEDIA PROFILE CONFIGURATION",
            Path = "Media Configuration\\Media Profile",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetProfiles })]
        public void ProfileConfiguration()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();

                string reason;
                Profile validProfile;
                Assert(
                    ValidateProfiles(profiles, out reason, out validProfile), 
                    reason, 
                    Resources.StepValidatingProfiles_Title,
                    validProfile != null ?
                    string.Format(Resources.StepValidatingProfilesDetails_Format, validProfile.token) : null);
            });
        }
        
        [Test(Name = "DYNAMIC MEDIA PROFILE CONFIGURATION",
            Path = "Media Configuration\\Media Profile",
            Order = "01.01.05",
            Id = "1-1-5",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.MediaService },
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[]
                                         {
                                             Functionality.CreateProfile, 
                                             Functionality.AddVideoSourceConfiguration, 
                                             Functionality.AddVideoEncoderConfiguration, 
                                             Functionality.GetProfile,
                                             Functionality.DeleteProfile
                                         }
                                         )]
        public void DynamicProfileConfiguration()
        {
            Profile deletedProfile = null;
            Profile createdProfile = null;
            Profile modifiedProfile = null;
            Profile profile = null;
            RunTest<Profile>(
               new Backup<Profile>(() => { return null; }),
               () =>
               {
                   Profile[] profiles = GetProfiles();

                   string reason;
                   Profile validProfile;
                   Assert(
                       ValidateProfiles(profiles, out reason, out validProfile),
                       reason,
                       Resources.StepValidatingProfiles_Title,
                       validProfile != null ?
                       string.Format(Resources.StepValidatingProfilesDetails_Format, validProfile.token) : null);

                   profile = CreateProfileByAnnex3("testprofileX", null, out deletedProfile, out createdProfile, out modifiedProfile);
                   Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);
                   
                   AddVideoSourceConfiguration(profile.token, validProfile.VideoSourceConfiguration.token);
                   AddVideoEncoderConfiguration(profile.token, validProfile.VideoEncoderConfiguration.token);

                   profile = GetProfile(profile.token);
                   bool valid = ValidateProfile(profile, validProfile.VideoSourceConfiguration.token, validProfile.VideoEncoderConfiguration.token, out reason);
                   Assert(valid, reason, Resources.StepValidatingNewProfile_Title);

                   profiles = GetProfiles();
                   /*Assert(
                        ValidateProfiles(profiles, out reason, out validProfile),
                        reason,
                        Resources.StepValidatingProfiles_Title,
                        validProfile != null ?
                        string.Format(Resources.StepValidatingProfilesDetails_Format, validProfile.token) : null);*/

                   Profile newProfile = profiles.FirstOrDefault(p => p.token == profile.token);

                   Assert( newProfile != null, string.Format("Profile with token {0} not found", profile.token), "Check that newly created profile is present in the list");
                   valid = ValidateProfile(newProfile, profile.VideoSourceConfiguration.token, profile.VideoEncoderConfiguration.token, out reason);
                   Assert(valid, reason, Resources.StepValidatingNewProfile_Title);

                   if (modifiedProfile == null)
                   {
                     bool hasFixed = newProfile.fixedSpecified;
                     if (newProfile.fixedSpecified)
                     {
                       hasFixed = newProfile.@fixed;
                     }
                     Assert(hasFixed == false,
                         "Newly created profile has \"fixed\" attrbute set to true",
                         "Check that profile has no \"fixed\" attribute set to true");
                   }
                   RemoveVideoEncoderConfiguration(profile.token);
                   RemoveVideoSourceConfiguration(profile.token);
                   if (modifiedProfile == null)
                   {
                      DeleteProfile(profile.token);
                      string token = profile.token;
                      profile = null;
                      createdProfile = null;
                      GetInvalidProfile(token);
                   }
               },
               (param) =>
               {
                   RestoreProfileByAnnex3(deletedProfile, createdProfile, modifiedProfile);
               });
        }
    }
}
