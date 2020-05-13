///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.CommonUtils.MessageModification;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;

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
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetWsdlUrl})]
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
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[]{ Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void GetCapabilitiesTest()
        {
            RunTest( () => 
            {                 
               
                Action<Capabilities> check = new Action<Capabilities>(
                    (capabilities) =>
                        {
                            Assert(capabilities != null, "Сapabilities not found", "Check capabilities");
                            Assert(capabilities.Device != null, "Device capabilities not found",
                                   "Check that DUT returned Device capabilities");
                            Assert(capabilities.Events != null, "Events capabilities not found",
                                   "Check that DUT returned Events capabilities");
                            // Media, DeviceIO, PTZ, Imaging, and Analytics 
                            if (Features.Contains(Feature.MediaService))
                            {
                                Assert(capabilities.Media != null, "Media capabilities not found",
                                       "Check that DUT returned Media capabilities");
                            }

                            if (Features.Contains(Feature.DeviceIoService))
                            {
                                Assert(capabilities.Extension != null && capabilities.Extension.DeviceIO != null, 
                                    "DeviceIO capabilities not found",
                                    "Check that DUT returned DeviceIO capabilities");
                            }

                            if (Features.Contains(Feature.PTZService))
                            {
                                Assert(capabilities.PTZ != null, "PTZ capabilities not found",
                                       "Check that DUT returned PTZ capabilities");
                            }

                            if (Features.Contains(Feature.ImagingService))
                            {
                                Assert(capabilities.Imaging != null, "Imaging capabilities not found",
                                       "Check that DUT returned Imaging capabilities");
                            }

                            if (Features.Contains(Feature.AnalyticsService))
                            {
                                Assert(capabilities.Analytics != null, "Analytics capabilities not found",
                                       "Check that DUT returned Analytics capabilities");
                            }
                        });


                Capabilities dutCapabilities = GetCapabilities(new[] { CapabilityCategory.All });
                check(dutCapabilities);

                // CapabilityCategory[0] and null result in the same SOAP packet
                dutCapabilities = GetCapabilities(new CapabilityCategory[0]);
                check(dutCapabilities);
            });
        }

        [Test(Name = "DEVICE CAPABILITIES",
            Order = "01.01.03",
            Id = "1-1-3",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[]{Functionality.GetCapabilities})]
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
            Order = "01.01.04",
            Id = "1-1-4",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void MediaCapabilitiesTest()
        {
            RunTest( () => 
            {
                Capabilities capabilities = null;
                CapabilityCategory[] category = new[] {CapabilityCategory.Media};
                
                if (Features.Contains(Feature.MediaService))
                {

                    capabilities = GetCapabilities(category);

                    Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                    Assert(capabilities.Media != null, "Media capabilities not found", "Check that DUT returned media capabilities");

                    Assert(capabilities.Media.XAddr.IsValidUrl(), "Media address is not a valid URL", string.Format("Validate media address ({0})", capabilities.Media.XAddr));

                    Assert(capabilities.Media.StreamingCapabilities != null, "Streaming capabilities not found", "Check that DUT returned streaming capabilities");

                    Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                    Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                    Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                    Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                    Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");
                                    
                }
                else
                {
                    RunStep( 
                        new Action( () => { capabilities = Client.GetCapabilities(category);}), 
                        "Get Media Capabilities - negative test",
                        OnvifFaults.NoSuchService);

                }

            });

        }

        [Test(Name = "EVENT CAPABILITIES",
             Path = PATH,
             Order = "01.01.05",
             Id = "1-1-5",
             Category = Category.DEVICE,
             Version = 1.02,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void EventCapabilitiesTest()
        {
            RunTest( () => 
            {
                Capabilities capabilities = null;

                capabilities = GetCapabilities(new[] {CapabilityCategory.Events});
                Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                Assert(capabilities.Events != null, "Events capabilities not found", "Check that DUT returned events capabilities");

                Assert(capabilities.Events.XAddr.IsValidUrl(), "Events address is not a valid URL", string.Format("Validate events address ({0})", capabilities.Events.XAddr));

                Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                Assert(capabilities.PTZ == null, "PTZ capabilities returned from the DUT", "Check that DUT did not return PTZ capabilities");



                

            });

        }

        [Test(Name = "PTZ CAPABILITIES",
             Order = "01.01.06",
             Id = "1-1-6",
             Category = Category.DEVICE,
             Path = PATH,
             Version = 1.02,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void PTZCapabilitiesTest()
        {
            RunTest( () => 
            {
                Capabilities capabilities = null;
                CapabilityCategory[] category = new[] { CapabilityCategory.PTZ };

                if (Features.Contains(Feature.PTZService))
                {
                    capabilities = GetCapabilities(category);

                    Assert(capabilities != null, "Сapabilities not found", "Check that DUT returned capabilities");

                    Assert(capabilities.PTZ != null, "PTZ capabilities not found", "Check that DUT returned PTZ capabilities");

                    Assert(capabilities.PTZ.XAddr.IsValidUrl(), "PTZ address is not a valid URL", string.Format("Validate PTZ service address ({0})", capabilities.PTZ.XAddr));

                    Assert(capabilities.Device == null, "Device capabilities returned from the DUT", "Check that DUT did not return device capabilities");
                    Assert(capabilities.Analytics == null, "Analytics capabilities  returned from the DUT", "Check that DUT did not return analytics capabilities");
                    Assert(capabilities.Events == null, "Events capabilities  returned from the DUT", "Check that DUT did not return events capabilities");
                    Assert(capabilities.Imaging == null, "Imaging capabilities  returned from the DUT", "Check that DUT did not return imaging capabilities");
                    Assert(capabilities.Media == null, "Media capabilities  returned from the DUT", "Check that DUT did not return media capabilities");
                }
                else
                {
                    RunStep(
                        new Action(() => { capabilities = Client.GetCapabilities(category); }),
                        "Get PTZ Capabilities - negative test",
                        OnvifFaults.NoSuchService);

                }
            });
        }

/* In 2.01 version splitted to 1-1-10 and 1-1-11 */
/*
        [Test(Name = "SERVICE CATEGORY CAPABILITIES",
            Order = "01.01.07",
            Id = "1-1-7",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
*/
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
                    correctFault = exc.IsValidOnvifFault(OnvifFaults.NoSuchService, out faultDump);
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
                    correctFault = exc.IsValidOnvifFault(OnvifFaults.NoSuchService, out faultDump);
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
            Order = "01.01.09",
            Id = "1-1-9",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            RequirementLevel = RequirementLevel.Must)]
        public void CapabilitiesFaultTest()
        {
            RunTest(() =>
            {
                MessageSpoiler spoiler = new MessageSpoiler();

                Dictionary<string, string> namespaces = new Dictionary<string, string>();
                namespaces.Add("s", "http://www.w3.org/2003/05/soap-envelope");
                namespaces.Add("onvif", "http://www.onvif.org/ver10/device/wsdl");

                Dictionary<string, string> replacements = new Dictionary<string, string>();
                replacements.Add("/s:Envelope/s:Body/onvif:GetCapabilities/onvif:Category", "XYZ");

                spoiler.Namespaces = namespaces;
                spoiler.NodesToReplace = replacements;

                SetBreakingBehaviour(spoiler);

                bool fault = false;

                try
                {
                    GetCapabilities(new CapabilityCategory[] { CapabilityCategory.All });
                }
                catch (FaultException exception)
                {
                    fault = true;
                    SaveStepFault(exception);
                    StepPassed();
                }

                if (!fault)
                {
                    string reason = "No SOAP fault returned from the DUT";
                    AssertException ex = new AssertException(reason);
                    StepFailed(ex);
                    throw ex;
                }
            });
        }
        
        [Test(Name = "IMAGING CAPABILITIES",
             Order = "01.01.10",
             Id = "1-1-10",
             Category = Category.DEVICE,
             Path = PATH,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void ImagingCapabilitiesTest()
        {
            RunTest(() =>
            {
                Capabilities capabilities = null;
                CapabilityCategory[] category = new[] { CapabilityCategory.Imaging };

                if (Features.Contains(Feature.ImagingService))
                {
                    capabilities = GetCapabilities(category);

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
                else
                {
                    RunStep(
                        new Action(() => { capabilities = Client.GetCapabilities(category); }),
                        "Get Imaging Capabilities - negative test",
                        OnvifFaults.NoSuchService);
                }
            });
        }
        
        [Test(Name = "ANALYTICS CAPABILITIES",
             Order = "01.01.11",
             Id = "1-1-11",
             Category = Category.DEVICE,
             Path = PATH,
             Version = 2.1,
             RequirementLevel = RequirementLevel.Must,
             RequiredFeatures = new Feature[] { Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities })]
        public void AnalyticsCapabilitiesTest()
        {
            RunTest(() =>
            {
                Capabilities capabilities = null;
                CapabilityCategory[] category = new[] { CapabilityCategory.Analytics };

                if (Features.Contains(Feature.AnalyticsService))
                {
                    capabilities = GetCapabilities(category);

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
                else
                {
                    RunStep(
                        new Action(() => { capabilities = Client.GetCapabilities(category); }),
                        "Get Analytics Capabilities - negative test",
                        OnvifFaults.NoSuchService);
                }
            });
        }

        // 1-1-12 moved to ServiceCapabilitiesTestSuite
        
    }
}
