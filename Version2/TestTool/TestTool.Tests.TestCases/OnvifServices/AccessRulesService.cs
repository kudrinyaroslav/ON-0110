using System.Collections.Generic;
using System.Linq;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.OnvifServices
{
    public interface IAccessRulesService: IBaseOnvifService2<AccessRulesPort, AccessRulesPortClient>
    {}

    public static class AccessRulesServiceExtensions
    {
        private static void InitializeGuard(this IAccessRulesService s)
        {
            if (null == s.ServiceClient.Port)
                s.Test.Assert(false,
                              "Can't connect to Access Rules Service",
                              "Check that Access Rules Service is accessible");
        }

        public static string GetAccessRulesServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.GetServiceAddress(OnvifService.ACCESS_RULES_SERVICE);
        }

        public static AccessRulesServiceCapabilities GetServiceCapabilities(this IAccessRulesService s)
        {
            s.InitializeGuard();

            AccessRulesServiceCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Access Rules)");

            return r;
        }

        public static AccessProfileInfo[] GetAccessProfileInfo(this IAccessRulesService s, string[] Token)
        {
            s.InitializeGuard();

            AccessProfileInfo[] r = null;
            
            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessProfileInfo(Token), "Get Access Profile Info");

            return r ?? new AccessProfileInfo[0];
        }

        public static string GetAccessProfileInfoList(this IAccessRulesService s, int? Limit, string StartReference, out AccessProfileInfo[] AccessProfileInfo)
        {
            s.InitializeGuard();

            string r = null;
            AccessProfileInfo[] localAccessProfileInfo = null;
            
            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessProfileInfoList(Limit, StartReference, out localAccessProfileInfo), "Get Access Profile Info List");

            AccessProfileInfo = localAccessProfileInfo ?? new AccessProfileInfo[0];

            return r;
        }

        public static AccessProfile[] GetAccessProfiles(this IAccessRulesService s, string[] Token)
        {
            s.InitializeGuard();

            AccessProfile[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessProfiles(Token), "Get Access Profiles");

            return r ?? new AccessProfile[0];
        }

        public static string GetAccessProfileList(this IAccessRulesService s, int? Limit, string StartReference, out AccessProfile[] AccessProfile)
        {
            s.InitializeGuard();

            string r = null;
            AccessProfile[] localAccessProfile = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessProfileList(Limit, StartReference, out localAccessProfile), "Get Access Profile List");

            AccessProfile = localAccessProfile ?? new AccessProfile[0];
            
            return r;
        }

        public static string CreateAccessProfile(this IAccessRulesService s, AccessProfile AccessProfile)
        {
            s.InitializeGuard();

            string r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.CreateAccessProfile(AccessProfile), "Create Access Profile");
            
            return r;
        }

        public static void ModifyAccessProfile(this IAccessRulesService s, AccessProfile AccessProfile)
        {
            s.InitializeGuard();

            //s.Test.RunStep(() => s.ServiceClient.Port.ModifyAccessProfile(Token, AccessProfile), "Modify Access Profile");
            s.Test.RunStep(() => s.ServiceClient.Port.ModifyAccessProfile(AccessProfile), "Modify Access Profile");
        }

        public static void DeleteAccessProfile(this IAccessRulesService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DeleteAccessProfile(Token), "Delete Access Profile");
        }

        public static AccessRulesServiceCapabilities ExtractAccessRulesCapabilities(this IAccessRulesService s, Service service)
        {
            return s.ExtractCapabilities<AccessRulesServiceCapabilities, AccessRulesPort, AccessRulesPortClient>(service, "Access Rules");
        }

        public static List<AccessProfileInfo> GetFullAccessProfilesInfoListA1(this IAccessRulesService s)
        {
            var r = new List<AccessProfileInfo>();

            string nextReference = null;
            do
            {
                AccessProfileInfo[] dst = null;
                nextReference = s.GetAccessProfileInfoList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        public static List<AccessProfile> GetFullAccessProfilesListA3(this IAccessRulesService s)
        {
            var r = new List<AccessProfile>();

            string nextReference = null;
            do
            {
                AccessProfile[] dst = null;
                nextReference = s.GetAccessProfileList(null, nextReference, out dst);
                r.AddRange(dst);
            } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());

            return r;
        }

        // Annex A.7: Helper procedure to check free storage for additional credential
        public static AccessProfile CheckFreeStorageForAdditionalAccessProfileA7(this IAccessRulesService s, IEnumerable<AccessProfile> fullAccessProfileList)
        {
            var cap = s.GetServiceCapabilities();

            if (fullAccessProfileList.Count() < cap.MaxAccessProfiles) 
                return null;
            else if (fullAccessProfileList.Count() == cap.MaxAccessProfiles)
            {
                var r = s.GetAccessProfiles(new string[] {fullAccessProfileList.First().token});

                s.DeleteAccessProfile(r.First().token);

                return r.First();
            }
            else
            {
                //throw new AssertException("The DUT has more Access Profile items than specified by ServiceCapabilities.MaxAccessProfiles");
                s.Test.Assert(false,
                              "The DUT has more Access Profile items than specified by ServiceCapabilities.MaxAccessProfiles",
                              "Check Access Rules Service Capabilities");

                return null;
            }
        }

        public static string CreateAccessProfileA11(this IAccessRulesService s, out AccessProfile accessProfile, IEnumerable<Schedule> fullSchedulesList, IEnumerable<AccessPointInfo> fullAccessPointList, string token = "")
        {
            AccessPolicy accessPolicy = null;
            if (fullAccessPointList.Any() && fullSchedulesList.Any())
                accessPolicy = new AccessPolicy()
                               {
                                   ScheduleToken = fullSchedulesList.First().token,
                                   Entity = fullAccessPointList.First().token,
                                   EntityType = null,
                                   Extension = null
                               };

            var newAccessProfile = new AccessProfile()
                                   {
                                       token = token,
                                       Name = "Test Access Profile",
                                       Description = "Test Description",
                                       Extension = null,
                                       AccessPolicy = (null != accessPolicy ? new [] { accessPolicy } : null)
                                   };
            
            var r = s.CreateAccessProfile(accessProfile = newAccessProfile);
            accessProfile.token = r;
            return r;
        }
    }
}
