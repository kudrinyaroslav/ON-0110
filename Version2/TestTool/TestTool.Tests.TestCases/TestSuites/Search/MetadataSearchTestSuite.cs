using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using System.Linq;

namespace TestTool.Tests.TestCases.TestSuites.Search
{
    [TestClass]
    public partial class MetadataSearchTestSuite : Base.SearchTest
    {
        private const string PATH = "Search\\Metadata Search";

        private int _searchKeepAlive = 10; // in seconds

        public MetadataSearchTestSuite(TestLaunchParam param)
            : base(param)
        {

        }


        [Test(Name = "FIND METADATA - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS EQUAL TO RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "05.01.01",
           Id = "5-1-1",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindMetadata, Functionality.GetMetadataSearchResults })]
        public void MetadataSearchEndpointsEqualToRecordingEndpointsTest()
        {
            MetadataSearchEndpoints(DefineSearchRange, false);
        }

        [Test(Name = "FIND METADATA - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS OUTSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "05.01.02",
           Id = "5-1-2",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindMetadata, Functionality.GetMetadataSearchResults })]
        public void MetadataSearchEndpointsOutsideRecordingEndpointsTest()
        {
            MetadataSearchEndpoints(DefineSearchRangeOutside, false);
        }

        [Test(Name = "FIND METADATA - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS INSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "05.01.03",
           Id = "5-1-3",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindMetadata, Functionality.GetMetadataSearchResults })]
        public void MetadataSearchEndpointsInsideRecordingEndpointsTest()
        {
            MetadataSearchEndpoints(DefineSearchRangeInside, true);
        }

        [Test(Name = "FIND METADATA (MAXMATCHES = 1)",
           Path = PATH,
           Order = "05.01.04",
           Id = "5-1-4",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindMetadata, Functionality.GetMetadataSearchResults })]
        public void SearchMetadataMaxMatchesTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            var metadataFilter = _metadataFilter;
            string searchToken = string.Empty;
            string recordingToken = _recordingToken;
            System.DateTime T1 = System.DateTime.Now;
            System.DateTime T2 = System.DateTime.Now;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    var recordingInfo = GetRecording(recordingToken);

                    T1 = DefineSearchRange(recordingInfo).Start;
                    T2 = DefineSearchRange(recordingInfo).End;

                    var findMetadataResponse = FindMetadata(T1, T2, recordingToken, 1);
                    Assert(findMetadataResponse != null,
                        "FindMetadataResponse wasn't returned",
                            "Check that FindMetadataResponse was returned");

                    searchToken = findMetadataResponse.SearchToken;
                    var searchResults =
                        GetAllMetadataSearchResults(searchToken, null, null, "PT5S", out state);
                    Assert(searchResults != null && searchResults.Count > 0,
                        "No metadata was found",
                           "Check that metadata list is not empty");
                    Assert(searchResults.Count == 1,
                        string.Format("{0} matches was found", searchResults.Count),
                            "Check that maxMatches parameter is not exceeded");

                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }
               });
        }

        [Test(Name = "FIND METADATA (NO RESULTS)",
           Path = PATH,
           Order = "05.01.05",
           Id = "5-1-5",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindMetadata, Functionality.GetMetadataSearchResults })]
        public void SearchMetadataNoResultsTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            var metadataFilter = _metadataFilter;
            string searchToken = string.Empty;
            string recordingToken = _recordingToken;
            System.DateTime T1 = System.DateTime.Now;
            System.DateTime T2 = System.DateTime.Now;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    var recordingInfo = GetRecording(recordingToken);

                    T1 = DefineSearchRange(recordingInfo).Start;
                    T2 = DefineSearchRange(recordingInfo).End;

                    var findMetadataResponse = FindMetadataInvalidFilter(T1, T2, recordingToken, 1);
                    Assert(findMetadataResponse != null,
                        "FindMetadataResponse wasn't returned",
                            "Check that FindMetadataResponse was returned");

                    searchToken = findMetadataResponse.SearchToken;
                    var searchResults =
                        GetAllMetadataSearchResults(searchToken, null, null, "PT5S", out state);
                    Assert(searchResults == null || searchResults.Count == 0,
                        string.Format("Metadata list has {0} items", searchResults.Count),
                           "Check that metadata list is empty");
                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }
               });
        }

        [Test(Name = "GET METADATA SEARCH RESULTS WITH INVALID SEARCHTOKEN",
           Path = PATH,
           Order = "05.01.06",
           Id = "5-1-6",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.MetadataSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.GetMetadataSearchResults })]
        public void SearchMetadataInvalidSearchTokenTest()
        {
            RunTest(() =>
            {
                this.InvalidTokenTestBody<object>((s, T) =>
                                                  {
                                                      Client.GetMetadataSearchResults(new GetMetadataSearchResults()
                                                      {
                                                          SearchToken = s,
                                                          WaitTime = "PT5S"
                                                      });
                                                  },
                                                  //[14.05.2013] AKS: expected SOAP fault from OnvifFaults.NotFound -> OnvifFaults.InvalidToken
                                                  null, RunStep, "Get Metadata with invalid search token", null, OnvifFaults.InvalidToken);
            });
        }
    }
}