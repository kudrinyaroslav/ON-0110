using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.OnvifServices
{
    public interface IAccessControlService : IBaseOnvifService2<PACSPort, PACSPortClient>
    { }

    public static class AccessControlServiceExtensions
    {
        private static void InitializeGuard(this IAccessControlService s)
        {
            if (null == s.ServiceClient.Port)
                s.Test.Assert(false,
                              "Can't connect to Access Control Service",
                              "Check that Access Control Service is accessible");
        }

        public static string GetAccessControlServiceAddress(this IDeviceService s, FeaturesList featureList)
        {
            return s.GetServiceAddress(OnvifService.ACCESSCONTROL);
        }

        public static AccessControlServiceCapabilities GetServiceCapabilities(this IAccessControlService s)
        {
            s.InitializeGuard();

            AccessControlServiceCapabilities r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetServiceCapabilities(), "Get Service Capabilities(Access Control)");

            return r;
        }

        public static string GetAccessPointInfoList(this IAccessControlService s,
                                                    int? Limit,
                                                    string StartReference,
                                                    out AccessPointInfo[] AccessPointInfo)
        {
            s.InitializeGuard();

            string r = null;
            AccessPointInfo[] lAccessPointInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessPointInfoList(Limit, StartReference, out lAccessPointInfo), "Get Access Point Info List");

            AccessPointInfo = lAccessPointInfo ?? new AccessPointInfo[0];

            return r;

        }

        public static AccessPointInfo[] GetAccessPointInfo(this IAccessControlService s, string[] Token)
        {
            s.InitializeGuard();

            AccessPointInfo[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessPointInfo(Token), "Get Access Point Info");

            return r ?? new AccessPointInfo[0];
        }

        public static AccessPointState GetAccessPointState(this IAccessControlService s, string Token)
        {
            s.InitializeGuard();

            AccessPointState r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAccessPointState(Token), "Get Access Point State");

            return r;
        }

        public static void EnableAccessPoint(this IAccessControlService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.EnableAccessPoint(Token), "Enable Access Point");
        }

        public static void DisableAccessPoint(this IAccessControlService s, string Token)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.DisableAccessPoint(Token), "Disable Access Point");
        }

        public static void ExternalAuthorization(this IAccessControlService s,
                                                 string AccessPointToken,
                                                 string CredentialToken,
                                                 string Reason,
                                                 Decision Decision)
        {
            s.InitializeGuard();

            s.Test.RunStep(() => s.ServiceClient.Port.ExternalAuthorization(AccessPointToken, CredentialToken, Reason, Decision), "Get Service Capabilities(Access Control)");
        }

        public static string GetAreaInfoList(this IAccessControlService s,
                                             int? Limit,
                                             string StartReference,
                                             out AreaInfo[] AreaInfo)
        {
            s.InitializeGuard();

            string r = null;
            AreaInfo[] lAreaInfo = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAreaInfoList(Limit, StartReference, out lAreaInfo), "Get AreaInfo List");

            AreaInfo = lAreaInfo ?? new AreaInfo[0];

            return r;
        }

        public static AreaInfo[] GetAreaInfo(this IAccessControlService s, string[] Token)
        {
            s.InitializeGuard();

            AreaInfo[] r = null;

            s.Test.RunStep(() => r = s.ServiceClient.Port.GetAreaInfo(Token), "Get AreaInfo");

            return r ?? new AreaInfo[0];
        }

        public static List<AccessPointInfo> GetFullAccessPointInfoListA1(this IAccessControlService s)
        {
            var r = new List<AccessPointInfo>();
            if (s != null)
            {
                string nextReference = null;
                do
                {
                    AccessPointInfo[] dst = null;
                    nextReference = s.GetAccessPointInfoList(null, nextReference, out dst);
                    r.AddRange(dst);
                } while (!string.IsNullOrEmpty(nextReference) && !s.Test.StopRequested());
            }

            return r;
        }

        public static bool IsAccessControlSupported(this IAccessControlService s, Service[] services)
        {
            return services.Any(e=> e.Namespace != null && e.Namespace == OnvifService.ACCESSCONTROL);
        }
    }
}
