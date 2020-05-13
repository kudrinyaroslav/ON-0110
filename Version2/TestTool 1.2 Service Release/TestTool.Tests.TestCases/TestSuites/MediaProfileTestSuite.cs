///////////////////////////////////////////////////////////////////////////
//!  @author        Ivan Vagunin
////
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Onvif;

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
            RequirementLevel = RequirementLevel.Must)]
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
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.MEDIA,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void DynamicProfileConfiguration()
        {
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

                   profile = CreateProfile("testprofilex", null);
                   Assert(IsEmptyProfile(profile, out reason), reason, Resources.StepValidatingNewProfile_Title);

                   AddVideoSourceConfiguration(profile.token, validProfile.VideoSourceConfiguration.token);
                   AddVideoEncoderConfiguration(profile.token, validProfile.VideoEncoderConfiguration.token);

                   profile = GetProfile(profile.token);
                   bool valid = ValidateProfile(profile, validProfile.VideoSourceConfiguration.token, validProfile.VideoEncoderConfiguration.token, out reason);
                   Assert(valid, reason, Resources.StepValidatingNewProfile_Title);

                   RemoveVideoEncoderConfiguration(profile.token);
                   RemoveVideoSourceConfiguration(profile.token);
                   DeleteProfile(profile.token);
                   string token = profile.token;
                   profile = null;
                   GetInvalidProfile(token);
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
