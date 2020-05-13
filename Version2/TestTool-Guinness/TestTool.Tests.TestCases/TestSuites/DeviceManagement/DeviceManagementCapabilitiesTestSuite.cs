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
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
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
                
                try
                {
                    GetCapabilities(new CapabilityCategory[] {CapabilityCategory.All});
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
        
        /*[Test(Name = "GET SERVICES",
            Order = "01.01.15",
            Id = "1-1-15",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.1,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetServices}, 
            RequiredFeatures = new Feature[] { Feature.GetServices },
            RequirementLevel = RequirementLevel.Must)]*/
        public void GetServicesTest()
        {
            RunTest(() =>
            {
                Service[] servicesList = GetServices(false);

                Action<Service[]> check = new Action<Service[]>( 
                    (services) =>
                        {
                            Service deviceService = services.FindService(Definitions.Onvif.OnvifService.DEVICE);
                            Service eventsService = services.FindService(Definitions.Onvif.OnvifService.EVENTS);

                            Assert(deviceService != null, "Device service not found", "Check that DUT returned Device service address");
                            Assert(eventsService != null, "Events service not found", "Check that DUT returned Events service address");
                            
                            // Media, DeviceIO, PTZ, Imaging, and Analytics 
                            if (Features.Contains(Feature.MediaService))
                            {
                                Service service = services.FindService(Definitions.Onvif.OnvifService.MEDIA);
                                Assert(service != null, "Media service not found",
                                       "Check that DUT returned Media service address");
                            }

                            if (Features.Contains(Feature.DeviceIoService))
                            {
                                Service service = services.FindService(Definitions.Onvif.OnvifService.IO);
                                Assert(service != null,
                                    "Device IO service not found",
                                    "Check that DUT returned Device IO service");
                            }

                            if (Features.Contains(Feature.PTZService))
                            {
                                Service service = services.FindService(Definitions.Onvif.OnvifService.PTZ);
                                Assert(service != null, "PTZ service not found",
                                       "Check that DUT returned PTZ service address");
                            }

                            if (Features.Contains(Feature.ImagingService))
                            {
                                Service service = services.FindService(Definitions.Onvif.OnvifService.IMAGING);

                                Assert(service != null, "Imaging service not found",
                                       "Check that DUT returned Imaging service");
                            }

                            if (Features.Contains(Feature.AnalyticsService))
                            {
                                Service service = services.FindService(Definitions.Onvif.OnvifService.ANALYTICS);

                                Assert(service != null, "Analytics service not found",
                                       "Check that DUT returned Analytics service address");
                            }                            

                        });

                check(servicesList);

                // check that no capabilities returned
                StringBuilder sb = new StringBuilder();
                bool notFound = true;
                foreach (Service service in servicesList)
                {
                    if (service.Capabilities != null)
                    {
                        notFound = false;
                        string version = service.Version == null
                                             ? "empty field"
                                             : string.Format("{0}.{1}", service.Version.Major, service.Version.Minor);
                        sb.AppendFormat("Capabilities are included in entry for service with namespace {0}, version {1}",
                                        service.Namespace, version);
                    }
                }

                Assert(notFound, sb.ToString(), "Check that no Capabilities returned");
                
                servicesList = GetServices(true);
                check(servicesList);

                Dictionary<string, Service> latestServices =
                    new Dictionary<string, Service>(StringComparer.InvariantCultureIgnoreCase);
                
                foreach (Service service in servicesList)
                {
                    if (latestServices.ContainsKey(service.Namespace))
                    {
                        // if entry has no versin info - ignore it
                        if (service.Version != null)
                        {
                            if (latestServices[service.Namespace].Version != null)
                            {
                                if (latestServices[service.Namespace].Version.Major >= service.Version.Major)
                                {
                                    if (latestServices[service.Namespace].Version.Minor > service.Version.Minor)
                                    {
                                        latestServices[service.Namespace] = service;
                                    }
                                }
                            }
                            else
                            {
                                // replace "unknown" with "known"
                                latestServices[service.Namespace] = service;
                            }
                        }
                    }
                    else
                    {
                        latestServices.Add(service.Namespace, service);
                    }
                }
                

                // check Capabilities
                
                // validator 
                XmlElementValidator validator = null; 

                sb = new StringBuilder();
                bool hasErrors = false;

                foreach (string ns in latestServices.Keys)
                {
                    Service service = latestServices[ns];

                    if (service.Capabilities != null)
                    {
                        if (service.Capabilities.LocalName != "Capabilities" || 
                            service.Capabilities.NamespaceURI.ToLower() != ns.ToLower() )
                        {
                            hasErrors = true;

                            string version = service.Version == null
                                                 ? "empty field"
                                                 : string.Format("{0}.{1}", service.Version.Major, service.Version.Minor);

                            sb.AppendFormat("Capabilities element included in entry for service with namespace {0}, version {1} is incorrect: child element must be 'Capabilities' from namespace {0} {2}",
                                            ns, version, Environment.NewLine);
                        }

                        // schema validation will be performed automatically only for Device service
                        if (validator == null)
                        {
                            BaseSchemaSet schemaSet = TypesSchemaSet.GetInstance();
                            validator = new XmlElementValidator(schemaSet);
                        }

                        //validate
                        XmlElement capabilities = service.Capabilities;
                        string error = string.Empty;
                        try
                        {
                            validator.Validate(capabilities);
                        }
                        catch (Exception exc)
                        {
                            hasErrors = true;
                            string version = service.Version == null
                                                 ? "empty field"
                                                 : string.Format("{0}.{1}", service.Version.Major, service.Version.Minor);
                            error = exc.Message;

                            sb.AppendFormat("Capabilities element included in entry for service with namespace {0}, version {1} is incorrect: {2} {3}",
                                            service.Namespace, version, error, Environment.NewLine);
                        }
                    }
                }

                string errDump = sb.ToStringTrimNewLine();
                Assert(!hasErrors, errDump, "Check that Capabilities elements are correct");

            });
        }

        /*
        static Dictionary<string, string> GetCapabilitiesNamespaces()
        {
            Dictionary<string, string> capabilitiesNamespace = new Dictionary<string, string>();

            capabilitiesNamespace.Add("http://www.onvif.org/ver20/analytics/wsdl", "http://www.onvif.org/ver20/analytics/wsdl");
            capabilitiesNamespace.Add("http://www.onvif.org/ver20/ptz/wsdl", "http://www.onvif.org/ver20/ptz/wsdl");
            capabilitiesNamespace.Add("http://www.onvif.org/ver10/media/wsdl", "http://www.onvif.org/ver10/media/wsdl");
            capabilitiesNamespace.Add("http://www.onvif.org/ver20/imaging/wsdl", "http://www.onvif.org/ver20/imaging/wsdl");
            capabilitiesNamespace.Add("http://www.onvif.org/ver10/device/wsdl", "http://www.onvif.org/ver10/device/wsdl");
            capabilitiesNamespace.Add("http://www.onvif.org/ver10/deviceIO/wsdl", "http://www.onvif.org/ver10/deviceIO/wsdl");

            return capabilitiesNamespace;
        }

        static Dictionary<string, string> GetCapabilitiesElements()
        {
            Dictionary<string, string> capabilitiesElements = new Dictionary<string, string>();

            capabilitiesElements.Add("http://www.onvif.org/ver20/analytics/wsdl", "Capabilities");
            capabilitiesElements.Add("http://www.onvif.org/ver20/ptz/wsdl", "Capabilities");
            capabilitiesElements.Add("http://www.onvif.org/ver10/media/wsdl", "Capabilities");
            capabilitiesElements.Add("http://www.onvif.org/ver20/imaging/wsdl", "Capabilities");
            capabilitiesElements.Add("http://www.onvif.org/ver10/device/wsdl", "Capabilities");
            capabilitiesElements.Add("http://www.onvif.org/ver10/deviceIO/wsdl", "Capabilities");

            return capabilitiesElements;
        }
        */

        /*[Test(Name = "DEVICE SERVICE CAPABILITIES",
            Order = "01.01.13",
            Id = "1-1-13",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceServiceCapabilities })]*/
        public void DeviceServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                DeviceServiceCapabilities capabilities = GetServiceCapabilities();
                
                // validate ?..

            });
        }

        /*[Test(Name = "GET SERVICES AND DEVICE SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.14",
            Id = "1-1-14",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetDeviceServiceCapabilities, Functionality.GetServices })]*/
        public void DeviceServiceCapabilitiesConsistencyTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);

                Service deviceService = services.FindService(Definitions.Onvif.OnvifService.DEVICE);
                
                Assert(deviceService != null, "No device service information returned", "Check that the DUT returned Device service information");

                if (deviceService.Capabilities == null)
                {
                    LogTestEvent("No Capabilities information included, skip the test");
                }
                else
                {
                    DeviceServiceCapabilities capabilities = GetServiceCapabilities();
                    
                    DeviceServiceCapabilities serviceCapabilities = ExtractCapabilities(deviceService.Capabilities);

                    CompareCapabilities(serviceCapabilities, capabilities);
                    
                }
            });
        }

        protected DeviceServiceCapabilities ExtractCapabilities(XmlElement element)
        {
            BeginStep("Parse Capabilities element");
            
            System.Xml.Serialization.XmlRootAttribute xRoot = new System.Xml.Serialization.XmlRootAttribute();
            xRoot.ElementName = "Capabilities";
            xRoot.IsNullable = true;
            xRoot.Namespace = Definitions.Onvif.OnvifService.DEVICE;
            
            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(DeviceServiceCapabilities), xRoot);

            XmlReader reader = new XmlNodeReader(element);

            DeviceServiceCapabilities capabilities = (DeviceServiceCapabilities)serializer.Deserialize(reader);
            StepPassed();
            return capabilities;
        }

        protected void CompareCapabilities(DeviceServiceCapabilities fromGetServices, DeviceServiceCapabilities fromGetCapabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            bool check = TestUtils.BothNotNull(out local,
                                     "System", "got via GetServices", "got via GetServiceCapabilities", 
                                     fromGetServices.System ,
                                     fromGetCapabilities.System, dump);

            equal = local;
            if (check)
            {
                SystemCapabilities system1 = fromGetServices.System;
                SystemCapabilities system2 = fromGetCapabilities.System;

                List<TestUtils.CheckSettings<SystemCapabilities>> batch = new List<TestUtils.CheckSettings<SystemCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "DiscoveryBye", SpecifiedSelector = (S) => S.DiscoveryByeSpecified, ValueSelector = (S) => S.DiscoveryBye });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "DiscoveryResolve", SpecifiedSelector = (S) => S.DiscoveryResolveSpecified, ValueSelector = (S) => S.DiscoveryResolve });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "FirmwareUpgrade", SpecifiedSelector = (S) => S.FirmwareUpgradeSpecified, ValueSelector = (S) => S.FirmwareUpgrade });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "HttpFirmwareUpgrade", SpecifiedSelector = (S) => S.HttpFirmwareUpgradeSpecified, ValueSelector = (S) => S.HttpFirmwareUpgrade });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "HttpSupportInformation", SpecifiedSelector = (S) => S.HttpSupportInformationSpecified, ValueSelector = (S) => S.HttpSupportInformation });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "HttpSystemBackup", SpecifiedSelector = (S) => S.HttpSystemBackupSpecified, ValueSelector = (S) => S.HttpSystemBackup });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "HttpSystemLogging", SpecifiedSelector = (S) => S.HttpSystemLoggingSpecified, ValueSelector = (S) => S.HttpSystemLogging });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "RemoteDiscovery", SpecifiedSelector = (S) => S.RemoteDiscoverySpecified, ValueSelector = (S) => S.RemoteDiscovery });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "SystemBackup", SpecifiedSelector = (S) => S.SystemBackupSpecified, ValueSelector = (S) => S.SystemBackup });
                batch.Add(new TestUtils.CheckSettings<SystemCapabilities>() 
                { FieldName = "SystemLogging", SpecifiedSelector = (S) => S.SystemLoggingSpecified, ValueSelector = (S) => S.SystemLogging });

                local = TestUtils.BatchCheckBoolean(system1, system2, "GetService", "GetServiceCapabilities", batch, "System", dump);
                equal = equal && local;

            }

            check = TestUtils.BothNotNull(out local,
                                     "Network", "got via GetServices", "got via GetServiceCapabilities", 
                                     fromGetServices.Network,
                                     fromGetCapabilities.Network, dump);

            equal = equal && local;
            if (check)
            {
                NetworkCapabilities network1 = fromGetServices.Network;
                NetworkCapabilities network2 = fromGetCapabilities.Network;

                List<TestUtils.CheckSettings<NetworkCapabilities>> batch = new List<TestUtils.CheckSettings<NetworkCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "Dot11Configuration", SpecifiedSelector = (S) => S.Dot11ConfigurationSpecified, ValueSelector = (S) => S.Dot11Configuration });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "DynDNS", SpecifiedSelector = (S) => S.DynDNSSpecified, ValueSelector = (S) => S.DynDNS });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "HostnameFromDHCP", SpecifiedSelector = (S) => S.HostnameFromDHCPSpecified, ValueSelector = (S) => S.HostnameFromDHCP });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "IPFilter", SpecifiedSelector = (S) => S.IPFilterSpecified, ValueSelector = (S) => S.IPFilter });
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "IPVersion6", SpecifiedSelector = (S) => S.IPVersion6Specified, ValueSelector = (S) => S.IPVersion6 });

                local = TestUtils.BatchCheckBoolean(network1, network2, "GetServices", "GetServiceCapabilities", batch, "Network", dump);
                equal = equal && local;

                local = TestUtils.CheckIntField<NetworkCapabilities>(network1, network2, N => N.NTP, N => N.NTPSpecified,
                                                           "Network.NTP", "GetServices", "GetServiceCapabilities", dump);
                equal = equal && local;

                
                batch.Clear();
                batch.Add(new TestUtils.CheckSettings<NetworkCapabilities>() { FieldName = "ZeroConfiguration", SpecifiedSelector = (S) => S.ZeroConfigurationSpecified, ValueSelector = (S) => S.ZeroConfiguration });

                local = TestUtils.BatchCheckBoolean(network1, network2, "GetServices", "GetServiceCapabilities", batch, "Network", dump);
                equal = equal && local;

            }

            check = TestUtils.BothNotNull(out local,
                                     "Security", "got via GetServices", "got via GetServiceCapabilities", 
                                     fromGetServices.Security,
                                     fromGetCapabilities.Security, dump);

            equal = equal && equal;
            if (check)
            {
                SecurityCapabilities s1 = fromGetServices.Security;
                SecurityCapabilities s2 = fromGetCapabilities.Security;

                List<TestUtils.CheckSettings<SecurityCapabilities>> batch = new List<TestUtils.CheckSettings<SecurityCapabilities>>();

                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS10", SpecifiedSelector = (S) => S.TLS10Specified, ValueSelector = (S) => S.TLS10 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS11", SpecifiedSelector = (S) => S.TLS11Specified, ValueSelector = (S) => S.TLS11 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "TLS12", SpecifiedSelector = (S) => S.TLS12Specified, ValueSelector = (S) => S.TLS12 });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "OnboardKeyGeneration", SpecifiedSelector = (S) => S.OnboardKeyGenerationSpecified, ValueSelector = (S) => S.OnboardKeyGeneration });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "AccessPolicyConfig", SpecifiedSelector = (S) => S.AccessPolicyConfigSpecified, ValueSelector = (S) => S.AccessPolicyConfig });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "Dot1X", SpecifiedSelector = (S) => S.Dot1XSpecified, ValueSelector = (S) => S.Dot1X });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "RemoteUserHandling", SpecifiedSelector = (S) => S.RemoteUserHandlingSpecified, ValueSelector = (S) => S.RemoteUserHandling });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "X509Token", SpecifiedSelector = (S) => S.X509TokenSpecified, ValueSelector = (S) => S.X509Token });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "SAMLToken", SpecifiedSelector = (S) => S.SAMLTokenSpecified, ValueSelector = (S) => S.SAMLToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "KerberosToken", SpecifiedSelector = (S) => S.KerberosTokenSpecified, ValueSelector = (S) => S.KerberosToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "UsernameToken", SpecifiedSelector = (S) => S.UsernameTokenSpecified, ValueSelector = (S) => S.UsernameToken });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "HttpDigest", SpecifiedSelector = (S) => S.HttpDigestSpecified, ValueSelector = (S) => S.HttpDigest });
                batch.Add(new TestUtils.CheckSettings<SecurityCapabilities>() { FieldName = "RELToken", SpecifiedSelector = (S) => S.RELTokenSpecified, ValueSelector = (S) => S.RELToken });

                local = TestUtils.BatchCheckBoolean(s1, s2, "GetServices", "GetServiceCapabilities", batch, "Security", dump);
                equal = equal && local;


                check = TestUtils.BothNotNull(out local, "SupportedEAPMethod",
                                    "got via GetServices", "got via GetServiceCapabilities", s1.SupportedEAPMethods,
                                    s2.SupportedEAPMethods, dump);

                equal = equal && local;

                if (check)
                {
                    local = CheckSupportedEAPMethods(s1.SupportedEAPMethods, s2.SupportedEAPMethods, 
                        "GetServices", "GetServiceCapabilities", dump);
                }
            }
            
            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
        }

        [Test(Name = "CAPABILITIES AND DEVICE SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.12",
            Id = "1-1-12",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Optional,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.GetCapabilities },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetCapabilities, Functionality.GetServices })]
        public void CapabilitiesAndDeviceServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Capabilities capabilities = GetCapabilities(new CapabilityCategory[] {CapabilityCategory.Device});
                
                Assert(capabilities.Device!=null, "Device field is empty", "Check that the DUT returned Device capabilities");

                DeviceServiceCapabilities serviceCapabilities = GetServiceCapabilities();
                
                // compare...
                CompareCapabilities(serviceCapabilities, capabilities);

            });
        }

        void CompareCapabilities(DeviceServiceCapabilities serviceCapabilities, Capabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;

            bool local;

            TestUtils.BothNotNull(out local, "Network", "got via GetServiceCapabilities", "got via GetCapabilities",
                                               serviceCapabilities.Network, capabilities.Device.Network, dump);

            if (serviceCapabilities.Network != null || capabilities.Device.Network != null)
            {
                NetworkCapabilities scNetwork = serviceCapabilities.Network;
                NetworkCapabilities1 cNetwork = capabilities.Device.Network;

                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.IPFilterSpecified : false,
                                             scNetwork != null ? scNetwork.IPFilter : false,
                                             cNetwork != null ? cNetwork.IPFilterSpecified : false,
                                             cNetwork != null ? cNetwork.IPFilter : false,
                                             "IPFilter", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.ZeroConfigurationSpecified : false,
                                             scNetwork != null ? scNetwork.ZeroConfiguration : false,
                                             cNetwork != null ? cNetwork.ZeroConfigurationSpecified : false,
                                             cNetwork != null ? cNetwork.ZeroConfiguration : false,
                                             "ZeroConfiguration", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.IPVersion6Specified : false,
                                             scNetwork != null ? scNetwork.IPVersion6 : false,
                                             cNetwork != null ? cNetwork.IPVersion6Specified : false,
                                             cNetwork != null ? cNetwork.IPVersion6 : false,
                                             "IPVersion6", dump);
                equal = equal && local;
                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.DynDNSSpecified : false,
                                             scNetwork != null ? scNetwork.DynDNS : false,
                                             cNetwork != null ? cNetwork.DynDNSSpecified : false,
                                             cNetwork != null ? cNetwork.DynDNS : false,
                                             "DynDNS", dump);
                equal = equal && local;

                bool dot11specified = false;
                bool dot11 = false;
                if (cNetwork != null && cNetwork.Extension != null)
                {
                    dot11specified = cNetwork.Extension.Dot11ConfigurationSpecified;
                    dot11 = cNetwork.Extension.Dot11Configuration;
                }

                local = CheckCapabilitiesField(scNetwork != null ? scNetwork.Dot11ConfigurationSpecified : false,
                                             scNetwork != null ? scNetwork.Dot11Configuration : false,
                                             dot11specified,
                                             dot11,
                                             "Dot11Configuration", dump);
                equal = equal && local;
            }

            /* System */

            TestUtils.BothNotNull(out local, "System", "got via GetServiceCapabilities", "got via GetCapabilities",
                                               serviceCapabilities.System, capabilities.Device.System, dump);

            if (serviceCapabilities.System != null || capabilities.Device.System != null)
            {
                SystemCapabilities scSystem = serviceCapabilities.System;
                SystemCapabilities1 cSystem = capabilities.Device.System;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.DiscoveryResolveSpecified : false,
                             scSystem != null ? scSystem.DiscoveryResolve : false,
                             cSystem != null,
                             cSystem != null ? cSystem.DiscoveryResolve : false,
                             "DiscoveryResolve", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.DiscoveryByeSpecified : false,
                             scSystem != null ? scSystem.DiscoveryBye : false,
                             cSystem != null,
                             cSystem != null ? cSystem.DiscoveryBye : false,
                             "DiscoveryBye", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.RemoteDiscoverySpecified : false,
                             scSystem != null ? scSystem.RemoteDiscovery : false,
                             cSystem != null,
                             cSystem != null ? cSystem.RemoteDiscovery : false,
                             "RemoteDiscovery", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.SystemBackupSpecified : false,
                             scSystem != null ? scSystem.SystemBackup : false,
                             cSystem != null,
                             cSystem != null ? cSystem.SystemBackup : false,
                             "SystemBackup", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.SystemLoggingSpecified : false,
                             scSystem != null ? scSystem.SystemLogging : false,
                             cSystem != null,
                             cSystem != null ? cSystem.SystemLogging : false,
                             "SystemLogging", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSystem != null ? scSystem.FirmwareUpgradeSpecified : false,
                             scSystem != null ? scSystem.FirmwareUpgrade : false,
                             cSystem != null,
                             cSystem != null ? cSystem.FirmwareUpgrade : false,
                             "FirmwareUpgrade", dump);
                equal = equal && local;

                bool specified = false;
                bool value = false;
                if(cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpSystemBackupSpecified;
                    value = cSystem.Extension.HttpSystemBackup;
                }
                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpSystemBackupSpecified : false,
                             scSystem != null ? scSystem.HttpSystemBackup : false,
                             specified,
                             value,
                             "HttpSystemBackup", dump);
                equal = equal && local;

                specified = false;
                value = false;
                if(cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpSystemLoggingSpecified;
                    value = cSystem.Extension.HttpSystemLogging;
                }

                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpSystemLoggingSpecified : false,
                             scSystem != null ? scSystem.HttpSystemLogging : false,
                             specified,
                             value,
                             "HttpSystemLogging", dump);
                equal = equal && local;

                specified = false;
                value = false;
                if (cSystem != null && cSystem.Extension != null)
                {
                    specified = cSystem.Extension.HttpFirmwareUpgradeSpecified;
                    value = cSystem.Extension.HttpFirmwareUpgrade;
                }

                local = CheckCapabilitiesField(scSystem != null ? scSystem.HttpFirmwareUpgradeSpecified : false,
                             scSystem != null ? scSystem.HttpFirmwareUpgrade : false,
                             specified,
                             value,
                             "HttpFirmwareUpgrade", dump);
                equal = equal && local;
            }
            
            /* Security */
            TestUtils.BothNotNull(out local, "Security", "got via GetServiceCapabilities", "got via GetCapabilities",
                                               serviceCapabilities.Security, capabilities.Device.Security, dump);

            if (serviceCapabilities.Security != null || capabilities.Device.Security != null )
            {
                SecurityCapabilities scSecurity = serviceCapabilities.Security;
                SecurityCapabilities1 cSecurity = capabilities.Device.Security;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS11Specified : false,
                                 scSecurity != null ? scSecurity.TLS11 : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.TLS11 : false,
                                 "TLS11", dump);
                equal = equal && local;
                
                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS12Specified : false,
                                 scSecurity != null ? scSecurity.TLS12 : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.TLS12 : false,
                                 "TLS12", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.OnboardKeyGenerationSpecified : false,
                                 scSecurity != null ? scSecurity.OnboardKeyGeneration : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.OnboardKeyGeneration : false,
                                 "OnBoardKeyGeneration", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.AccessPolicyConfigSpecified : false,
                                 scSecurity != null ? scSecurity.AccessPolicyConfig : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.AccessPolicyConfig : false,
                                 "AccessPolicyconfig", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.X509TokenSpecified : false,
                                 scSecurity != null ? scSecurity.X509Token : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.X509Token : false,
                                 "X509Token", dump);
                equal = equal && local;
                
                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.SAMLTokenSpecified : false,
                                 scSecurity != null ? scSecurity.SAMLToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.SAMLToken : false,
                                 "SAMLToken", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.KerberosTokenSpecified : false,
                                 scSecurity != null ? scSecurity.KerberosToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.KerberosToken : false,
                                 "KerberosToken", dump);
                equal = equal && local;

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.RELTokenSpecified : false,
                                 scSecurity != null ? scSecurity.RELToken : false,
                                 cSecurity != null,
                                 cSecurity != null ? cSecurity.RELToken : false,
                                 "RELToken", dump);
                equal = equal && local;
                
                bool specified = false;
                bool value = false;
                if (cSecurity != null && cSecurity.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.TLS10 ;
                }
                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.TLS10Specified : false,
                             scSecurity != null ? scSecurity.TLS10 : false,
                             specified,
                             value,
                             "TLS10", dump);
                equal = equal && local;
                
                /********************************************************/
                specified = false;
                value = false;
                if (cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.Extension.Dot1X;
                }

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.Dot1XSpecified : false,
                             scSecurity != null ? scSecurity.Dot1X : false,
                             specified,
                             value,
                             "Dot1X", dump);
                equal = equal && local;

                int[] methods1 = null;
                
                if (scSecurity != null)
                {
                    methods1 = scSecurity.SupportedEAPMethods;
                }

                int[] methods2 = null;
                if (cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    methods2 = cSecurity.Extension.Extension.SupportedEAPMethod;
                }
                
                 TestUtils.BothNotNull(out local, "SupportedEAPMethod",
                    "got via GetServiceCapabilities", "got via GetCapabilities", 
                    methods1, methods2, dump);
                
                if (methods1 != null && methods2 != null)
                {
                    local = CheckSupportedEAPMethods(
                        methods1, 
                        methods2,
                        "GetServiceCapabilities", "GetCapabilities", dump);

                    equal = equal && local;
                }


                specified = false;
                value = false;
                if(cSecurity != null && cSecurity.Extension != null && cSecurity.Extension.Extension != null)
                {
                    specified = true;
                    value = cSecurity.Extension.Extension.RemoteUserHandling ;
                }

                local = CheckCapabilitiesField(scSecurity != null ? scSecurity.RemoteUserHandlingSpecified : false,
                             scSecurity != null ? scSecurity.RemoteUserHandling : false,
                             specified,
                             value,
                             "RemoteUserHandling", dump);
                equal = equal && local;
                
            }


            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }

            StepPassed();
            
        }

        bool CheckCapabilitiesField(bool specified1, bool field1,
            bool specified2, bool field2,
            string fieldName, StringBuilder dump)
        {
            return TestUtils.CheckField(specified1,
                             field1,
                             specified2,
                             field2,
                             fieldName, "GetServiceCapabilities", "GetCapabilities", dump);
        }

        bool CheckSupportedEAPMethods(int[] arr1, int[] arr2, string descr1, string descr2, StringBuilder dump)
        {
            bool equal = true;

            List<int> l1 = new List<int>();
            if (arr1 != null)
            {
                l1.AddRange(arr1);
            }

            List<int> l2 = new List<int>();
            if (arr2 != null)
            {
                l2.AddRange(arr2);
            }

            List<int> notFound1 = new List<int>();
            List<int> notFound2 = new List<int>();

            Action<List<int>, List<int>, List<int>> select =
                new Action<List<int>, List<int>, List<int>>(
                    (list1, list2, notFound) =>
                        {
                            foreach (int val in list1)
                            {
                                if (!list2.Contains(val) && !notFound.Contains(val))
                                {
                                    notFound.Add(val);
                                }
                            }                                

                        });

            select(l1, l2, notFound1);
            select(l2, l1, notFound2);

            Action <List<int>, string> dumpNotFound = new Action<List<int>, string>(
                (list, fieldName) =>
                    {
                        if (list.Count > 0)
                        {
                            equal = false;

                            StringBuilder lst = new StringBuilder("Value(s) ");
                            bool first = true;
                            foreach (int val in list)
                            {
                                if (first)
                                {
                                    lst.AppendFormat("{0}", val);
                                    first = false;
                                }
                                else
                                {
                                    lst.AppendFormat(", {0}", val);
                                }
                            }

                            lst.AppendFormat(" not found in SupportedEAPMethods in structure got via {0}{1}",
                                             fieldName, Environment.NewLine);
                        
                            dump.Append(lst);
                        }
                    });

            dumpNotFound(notFound1, descr2);
            dumpNotFound(notFound2, descr1);

            return equal;
        
        }

    }
}
