using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.CommonUtils.Comparison;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class SearchServiceCapabilitiesTestSuite : DeviceManagementTest
    {
        public SearchServiceCapabilitiesTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Search\\Capabilities";

        private string _searchServiceAddress;
        private SearchPortClient _searchClient;

        protected SearchPortClient SearchClient
        {
            get
            {
                if (_searchClient == null)
                {
                    bool found = false;
                    if (string.IsNullOrEmpty(_searchServiceAddress))
                    {
                        RunStep(() =>
                        {
                            string address = Client.GetSearchServiceAddress(Features);
                            if (string.IsNullOrEmpty(address))
                            {
                                throw new AssertException("Search service not found");
                            }
                            else
                            {
                                _searchServiceAddress = address;
                                found = true;
                                LogStepEvent(_searchServiceAddress);
                            }
                        }, "Get Search service address", OnvifFaults.NoSuchService , true, true);
                        DoRequestDelay();
                    }

                    Assert(found, "Search service address not found", "Check that the DUT returned Search service address");

                    if (found)
                    {
                        EndpointController controller = new EndpointController(new EndpointAddress(_searchServiceAddress));

                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] { new SoapValidator(SearchSchemasSet.GetInstance()), controller });

                        _searchClient = new SearchPortClient(binding, new EndpointAddress(_searchServiceAddress));

                        AttachSecurity(_searchClient.Endpoint);
                        SetupChannel(_searchClient.InnerChannel);
                    }
                }
                return _searchClient;
            }
        }

        void CloseSearchClient()
        {
            if (_searchClient != null)
            {
                SearchClient.Close();
            }
        }

        [Test(Name = "RECORDING SEARCH SERVICE CAPABILITIES",
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.SEARCH,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingSearchService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetSearchServiceCapabilities  })]
        public void SearchServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                // Initialize client
                SearchPortClient client = SearchClient;

                Proxies.Onvif.SearchServiceCapabilities capabilities = GetSearchCapabilities();
            }, 
            () =>
                {
                    CloseSearchClient();
                });
        }

        [Test(Name = "GET SERVICES AND GET RECORDING SEARCH SERVICE CAPABILITIES CONSISTENCY",
            Order = "01.01.02",
            Id = "1-1-2",
            Category = Category.SEARCH,
            Path = PATH,
            Version = 2.1,
            RequirementLevel = RequirementLevel.Must,
            RequiredFeatures = new Feature[] { Feature.GetServices, Feature.RecordingSearchService },
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingServiceCapabilities, Functionality.GetServices })]
        public void CapabilitiesAndSearchServiceCapabilitiesTest()
        {
            RunTest(() =>
            {
                Service[] services = GetServices(true);
              
                Service searchService = services.FindService(Definitions.Onvif.OnvifService.SEARCH);

                Assert(searchService != null, "No Search service information returned", "Check that the DUT returned Search service information");

                Assert((searchService.Capabilities != null), "No Capabilities information included",
                       "Check that Capabilities element is included in Services element");

                SearchServiceCapabilities serviceCapabilities = ParseSearchCapabilities(searchService.Capabilities);

                SearchServiceCapabilities capabilities = GetSearchCapabilities();

                CompareCapabilities(serviceCapabilities, capabilities);

            },
            () =>
            {
                CloseSearchClient();
            });
        }
        
        #region Validation

        void CompareCapabilities(SearchServiceCapabilities serviceSapabilities, SearchServiceCapabilities capabilities)
        {
            BeginStep("Compare Capabilities");

            StringBuilder dump = new StringBuilder();
            bool equal = true;
            bool local;

            List<TestUtils.CheckSettings<SearchServiceCapabilities>> batch = new List<TestUtils.CheckSettings<SearchServiceCapabilities>>();

            batch.Add(new TestUtils.CheckSettings<SearchServiceCapabilities>()
                          {
                              FieldName = "MetadataSearch", 
                              SpecifiedSelector = (S) => S.MetadataSearchSpecified, 
                              ValueSelector = (S) => S.MetadataSearch
                          });

            local = TestUtils.BatchCheckBoolean(serviceSapabilities, capabilities, "GetServices", "GetServiceCapabilities", batch, "", dump);
            equal = equal && local;
            
            if (!equal)
            {
                LogStepEvent(dump.ToStringTrimNewLine());
                throw new AssertException("Settings don't match");
            }
            
            StepPassed();
        }
        
        protected bool CheckCapabilitiesField(bool specified1, bool field1,
            bool specified2, bool field2,
            string fieldName, StringBuilder dump)
        {
            return TestUtils.CheckField(specified1,
                             field1,
                             specified2,
                             field2,
                             fieldName, "GetServiceCapabilities", "GetCapabilities", dump);
        }

        #endregion
        
        #region Steps

        protected SearchServiceCapabilities ParseSearchCapabilities(XmlElement element)
        {
            return ExtractCapabilities<SearchServiceCapabilities>(element,
                                                                   Definitions.Onvif.OnvifService.SEARCH);
        }

        protected SearchServiceCapabilities GetSearchCapabilities()
        {
            SearchPortClient client = SearchClient;
            SearchServiceCapabilities capabilities = null;
            RunStep(() => { capabilities = client.GetServiceCapabilities(); }, "Get Service Capabilities");
            return capabilities;
        }

        #endregion

    }
}
