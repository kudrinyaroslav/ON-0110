///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.ServiceModel;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.Exceptions;
using TestTool.Tests.Common.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Proxies.Device;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    public class DeviceManagementCapabilitiesTestSuite : Base.DeviceManagementTest
    {
        public DeviceManagementCapabilitiesTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Device Management\\Capabilities";

        [Test(Name = "GET WSDL URL",
            Order =  "06.01.01",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{  Service.Device},
            RequirementLevel = RequirementLevel.Must)]
        public void GetWsdlTest()
        {
            RunTest( () => 
            {                 
                string wsdlUrl = GetWsdlUrl();

                string assertName = string.Format("Validate URL returned ({0})", wsdlUrl);
                Assert(wsdlUrl.IsValidUrl(), "URL returned is not valid URL", assertName);
            });
        }

        [Test(Name = "ALL CAPABILITIES",
            Order = "06.01.02",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device, Service.Media, Service.Events},
            RequirementLevel = RequirementLevel.Must)]
        public void GetCapabilitiesTest()
        {
            RunTest( () => 
            {                 
                Capabilities capabilities = GetCapabilities(new [] {CapabilityCategory.All});

                Assert(capabilities != null, "Сapabilities not found", "Check capabilities");

                Assert(capabilities.Device !=null, "Device capabilities not found", "Check that DUT returned Device capabilities");
                Assert(capabilities.Media != null, "Media capabilities not found", "Check that DUT returned Media capabilities");
                Assert(capabilities.Events != null, "Events capabilities not found", "Check that DUT returned Events capabilities");

                // CapabilityCategory[0] and null result in the same SOAP packet
                capabilities = GetCapabilities(new CapabilityCategory[0]);

                Assert(capabilities != null, "Сapabilities not found", "Check capabilities");

                Assert(capabilities.Device != null, "Device capabilities not found", "Check that DUT returned Device capabilities");
                Assert(capabilities.Media != null, "Media capabilities not found", "Check that DUT returned Media capabilities");
                Assert(capabilities.Events != null, "Events capabilities not found", "Check that DUT returned Events capabilities");
            });
        }

        [Test(Name = "DEVICE CAPABILITIES",
            Order = "06.01.03",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device},
            RequirementLevel = RequirementLevel.Must)]
        public void DeviceCapabilitiesTest()
        {
            RunTest( () => {                
                Capabilities capabilities = GetCapabilities(new [] {CapabilityCategory.Device});

                Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                Assert(capabilities.Device != null, "Device capabilities not found", "Check that DUT returned device capabilities");
                Assert(capabilities.Device.XAddr.IsValidUrl(), "Device service address is not valid URL", string.Format("Validate device address ({0})", capabilities.Device.XAddr));
                Assert(capabilities.Device.Network != null, "Network capabilities not found", "Check that DUT returned network capabilities");
                Assert(capabilities.Device.System != null, "System capabilities not found", "Check that DUT returned system capabilities");

                Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");

                bool supportedVersionsOk = true;
                string supportedVersions = "Device supports ONVIF versions: ";
                if (capabilities.Device.System.SupportedVersions == null)
                {
                    supportedVersionsOk = false;
                }
                else
                {
                    bool bFirst = true;
                    supportedVersionsOk = capabilities.Device.System.SupportedVersions.Length > 0;

                    foreach (OnvifVersion version in capabilities.Device.System.SupportedVersions)
                    {
                        string versionInfo = string.Empty;
                        if (!bFirst)
                        {
                            versionInfo = ", ";
                        }
                        else
                        {
                            bFirst = false;
                        }
                        versionInfo += string.Format("{0}.{1}", version.Major, version.Minor.ToString("00"));
                        supportedVersions += versionInfo;
                    }
                }

                if (!supportedVersionsOk)
                {
                    supportedVersions = null;
                }

                Assert(supportedVersionsOk, "Supported ONVIF versions not found", "Check supported ONVIF versions", supportedVersions);

                Assert(capabilities.Device.IO != null, "IO capabilities not found", "Check that DUT returned IO capabilities");
                Assert(capabilities.Device.Security != null, "Security capabilities not found", "Check that DUT returned security capabilities");
            });
            
        }
        
        [Test(Name = "MEDIA CAPABILITIES",
            Order = "06.01.04",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device, Service.Media},
            RequirementLevel = RequirementLevel.Must)]
        public void MediaCapabilitiesTest()
        {
            RunTest( () => 
            {                 
                Capabilities capabilities = GetCapabilities(new [] { CapabilityCategory.Media  });

                Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                Assert(capabilities.Media != null, "Media capabilities not found", "Check that DUT returned media capabilities");

                Assert(capabilities.Media.XAddr.IsValidUrl(), "Media address is not a valid URL", string.Format("Validate media address ({0})", capabilities.Media.XAddr));

                Assert(capabilities.Media.StreamingCapabilities != null, "Streaming capabilities not found", "Check that DUT returned streaming capabilities");

                Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");


                //Assert(capabilities.Media.StreamingCapabilities.);
            });

        }

//#if FULL
        [Test(Name = "EVENT CAPABILITIES",
             Path = PATH,
             Order = "06.01.05",
             Version = 1.02,
             Services = new Service[]{ Service.Device, Service.Events},
             RequirementLevel = RequirementLevel.Must)]
//#endif
        public void EventCapabilitiesTest()
        {
            RunTest( () => 
            {
                Capabilities capabilities = GetCapabilities(new [] { CapabilityCategory.Events });

                Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                Assert(capabilities.Events != null, "Events capabilities not found", "Check that DUT returned events capabilities");

                Assert(capabilities.Events.XAddr.IsValidUrl(), "Events address is not a valid URL", string.Format("Validate events address ({0})", capabilities.Events.XAddr));

                Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");
                
                //no assertion on this
                //Assert(capabilities.Events.WSSubscriptionPolicySupport, "Subscription policies not supported by the DUT", "Check if subscription policies are supported by the DUT");

                if (capabilities.Events.WSSubscriptionPolicySupport)
                {
                    // ToDo ...
                }

            });

        }

//#if FULL
        [Test(Name = "PTZ CAPABILITIES",
             Order = "06.01.06",
             Path = PATH,
             Version = 1.02,
             Services = new Service[]{ Service.Device},
             RequirementLevel = RequirementLevel.Must)]
//#endif
        public void PTZCapabilitiesTest()
        {
            RunTest( () => 
            {
                Capabilities capabilities = null;

                bool fault = false;
                bool correctFault = false;
                string reason = string.Empty;

                try
                {
                    capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.PTZ });
                    Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                    Assert(capabilities.PTZ != null, "PTZ capabilities not found", "Check that DUT returned PTZ capabilities");
                    
                    Assert(capabilities.PTZ.XAddr.IsValidUrl(), "PTZ address is not a valid URL", string.Format("Validate PTZ service address ({0})", capabilities.PTZ.XAddr));

                    Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                    Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                    Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                    Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                    Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");

                }
                catch (FaultException exc)
                {
                    fault = true;

                    string faultDump;
                    correctFault = exc.IsValidOnvifFault("Receiver/ActionNotSupported/NoSuchService", out faultDump);
                    if (!correctFault)
                    {
                        reason = string.Format("The DUT returned unexpected SOAP FAULT: {0}", faultDump);
                    }
                    correctFault = true;
                    SaveStepFault(exc);
                    StepPassed();
                }

                if (fault)
                {
                    Assert(correctFault, reason, "Verify that correct SOAP FAULT is returned", reason);
                }

            });
        }

        [Test(Name = "SERVICE CATEGORY CAPABILITIES",
            Order="06.01.07",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device},
            RequirementLevel = RequirementLevel.Must)]
        public void ServiceCategoryCapabilitiesTest()
        {
            RunTest( () => 
            {

                bool fault = false;
                bool correctFault = false;
                string reason = string.Empty;

                try
                {
                    Capabilities capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Analytics }, "Get Analytics capabilities");

                    Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                    Assert(capabilities.Analytics != null, "Analytics capabilities not found", "Check that DUT returned Analytics capabilities");

                    Assert(capabilities.Analytics.XAddr.IsValidUrl(), "Analytics address is not a valid URL",
                           string.Format("Validate analytics address ({0})", capabilities.Analytics.XAddr));

                    Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                    Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                    Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                    Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                    Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");

                }
                catch (FaultException exc)
                {
                    fault = true;

                    string faultDump;
                    correctFault = exc.IsValidOnvifFault("Receiver/ActionNotSupported/NoSuchService", out faultDump);
                    if (!correctFault)
                    {
                        reason = string.Format("The DUT returned unexpected SOAP FAULT: {0}", faultDump);
                    }
                    correctFault = true;
                    SaveStepFault(exc);
                    StepPassed();
                }

                if (fault)
                {
                    Assert(correctFault, reason, "Verify that correct SOAP FAULT is returned", reason);
                }

                fault = false;
                correctFault = false;
                reason = string.Empty;

                try
                {
                    Capabilities capabilities = GetCapabilities(new CapabilityCategory[] { CapabilityCategory.Imaging }, "Get Imaging capabilities");

                    Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                    Assert(capabilities.Imaging != null, "Imaging capabilities not found", "Check that DUT returned Imaging capabilities");

                    Assert(capabilities.Imaging.XAddr.IsValidUrl(), "Imaging address is not a valid URL",
                           string.Format("Validate imaging address ({0})", capabilities.Imaging.XAddr));

                    Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                    Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                    Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                    Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                    Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");

                }
                catch (FaultException exc)
                {
                    fault = true;

                    string faultDump;
                    correctFault = exc.IsValidOnvifFault("Receiver/ActionNotSupported/NoSuchService", out faultDump);
                    if (!correctFault)
                    {
                        reason = string.Format("The DUT returned unexpected SOAP FAULT: {0}", faultDump);
                    }
                    correctFault = true;
                    SaveStepFault(exc);
                    StepPassed();
                }

                if (fault)
                {
                    Assert(correctFault, reason, "Verify that correct SOAP FAULT is returned", reason);
                }
            
            });


        }

        [Test(Name = "SOAP FAULT MESSAGE",
            Order = "06.01.08",
            Path = PATH,
            Version = 1.02,
            Services = new Service[]{ Service.Device},
            RequirementLevel = RequirementLevel.Should)]
        public void CapabilitiesFaultTest()
        {
            RunTest( () =>
            {
                MessageSpoiler spoiler = new MessageSpoiler();
                
                Dictionary<string, string> namespaces = new Dictionary<string, string>();
                namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                namespaces.Add("onvif", "http://www.onvif.org/ver10/device/wsdl");

                Dictionary<string,string> replacements = new Dictionary<string, string>();
                replacements.Add("/s:Envelope/s:Body/onvif:GetCapabilities/onvif:Category", "XYZ");

                spoiler.Namespaces = namespaces;
                spoiler.NodesToReplace = replacements;
                
                SetBreakingBehaviour(spoiler);

                bool fault = false;
                string reason = "No SOAP fault returned from the DUT";
                try
                {
                    GetCapabilities(new CapabilityCategory[] {CapabilityCategory.All});
                }
                catch (FaultException exception)
                {
                    fault = exception.IsValidOnvifFault();
                    reason = "A SOAP 1.2 fault message is invalid";
                    SaveStepFault(exception);
                    if (!fault)
                    {
                        AssertException ex = new AssertException(reason);
                        StepFailed(ex);
                        throw ex;
                    }
                    else
                    {
                        StepPassed();
                    }
                }                             
            });

        }



        /*
        [Test(Name ="Some test", 
            Path="Device Management", 
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must)]
        public void SomeTest()
        {
            RunTest( () => 
            {                 



            });

        }
         */
    }
}
