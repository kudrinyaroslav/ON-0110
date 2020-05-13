using System;
using System.Collections.Generic;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using DateTime=System.DateTime;
using System.Xml;

namespace TestTool.Tests.TestCases
{
    [TestClass]
    public partial class SearchEventsTestSuite : Base.SearchTest
    {
        private const string PATH = "Search\\Events Search";

        private int _searchKeepAlive = 10; // in seconds

        public SearchEventsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS EQUAL TO RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.01",
           Id = "3-1-1",
           Category = Category.SEARCH,
           Version = 2.0 ,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents })]
        public void FindEventsForwardSearchTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();
                    
                    DateTime start;
                    DateTime end;
                    DefineEventsSearchRange(recording, out start, out end);

                    List<FindEventResult> results = new List<FindEventResult>();

                    Dictionary<FindEventResult, XmlElement> elements1 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements2 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements = elements1;

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[]{ recording.RecordingToken};

                    Action<DateTime, DateTime> validateAction = 
                        new Action<DateTime, DateTime>(
                            (startTime, endTime) =>
                                {
                                    state = SearchState.Completed;
                                    searchToken = FindEvents(scope, filter, startTime, endTime, false, null, keepAlive);
                                    
                                    Dictionary<FindEventResult, XmlDocument> rawResults= new Dictionary<FindEventResult, XmlDocument>();

                                    results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", rawResults, out state);

                                    Assert(results != null && results.Count > 0, "No events found",
                                           "Check that events list is not empty");
                                    
                                    ValidateMessages(results);

                                    GetMessageElements(results, rawResults, elements);

                                    ValidateRecordingEvents(recording, results, elements);

                                    ValidateOrder(results, startTime, endTime);

                                });

                    validateAction(start, end);
                    List<FindEventResult> results1 = new List<FindEventResult>();
                    results1.AddRange(results);

                    elements = elements2;
                    validateAction(end, start);
                    List<FindEventResult> results2 = new List<FindEventResult>();
                    results2.AddRange(results);

                    // compare lists

                    CompareLists(results1, results2, elements1, elements2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");

                },
                () =>
                {
                    if ( state != SearchState.Completed)
                    {
                        EndSearch(searchToken);
                    }
                });
        }

        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS OUTSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.02",
           Id = "3-1-2",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents })]
        public void FindEventsForwardSearchExtendedPeriodTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    DateTime start;
                    DateTime end;
                    DefineEventsSearchRange(recording, out start, out end);

                    TimeSpan timeSpan = (end -start);
                    int delta = 1 + (int)timeSpan.TotalSeconds / 10;
                    
                    start = start.AddSeconds(-delta);
                    end = end.AddSeconds(delta);
                    
                    List<FindEventResult> results = new List<FindEventResult>();

                    Dictionary<FindEventResult, XmlElement> elements1 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements2 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements = elements1;

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };

                    Action<DateTime, DateTime> validateAction =
                        new Action<DateTime, DateTime>(
                            (startTime, endTime) =>
                            {
                                state = SearchState.Completed;
                                searchToken = FindEvents(scope, filter, startTime, endTime, false, null, keepAlive);

                                Dictionary<FindEventResult, XmlDocument> rawResults = new Dictionary<FindEventResult, XmlDocument>();

                                results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", rawResults, out state);
                                
                                Assert(results != null && results.Count > 0, "No events found",
                                       "Check that events list is not empty");


                                ValidateMessages(results);

                                GetMessageElements(results, rawResults, elements);

                                ValidateRecordingEvents(recording, results, elements);

                                ValidateOrder(results, startTime, endTime);

                            });

                    validateAction(start, end);
                    List<FindEventResult> results1 = new List<FindEventResult>();
                    results1.AddRange(results);

                    elements = elements2;
                    validateAction(end, start);
                    List<FindEventResult> results2 = new List<FindEventResult>();
                    results2.AddRange(results);

                    // compare lists
                    CompareLists(results1, results2, elements1, elements2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");
                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        EndSearch(searchToken);
                    }
                });
        }
        
        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS INSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.04",
           Id = "3-1-4",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents })]
        public void FindEventsForwardSearchNarrowedPeriodTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    DateTime start;
                    DateTime end;

                    DefineEventsSearchRange(recording, out start, out end);

                    TimeSpan timeSpan = (end - start);
                    double delta = (int)timeSpan.TotalMilliseconds/10;

                    start = start.AddMilliseconds(delta);
                    end = end.AddMilliseconds(-delta);

                    List<FindEventResult> results = new List<FindEventResult>();

                    Dictionary<FindEventResult, XmlElement> elements1 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements2 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements = elements1;

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };

                    Action<DateTime, DateTime> validateAction =
                        new Action<DateTime, DateTime>(
                            (startTime, endTime) =>
                            {
                                state = SearchState.Completed;

                                searchToken = FindEvents(scope, filter, startTime, endTime, true, null, keepAlive);

                                Dictionary<FindEventResult, XmlDocument> rawResults = new Dictionary<FindEventResult, XmlDocument>();

                                results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", rawResults, out state);

                                Assert(results != null && results.Count > 0, "No events found",
                                       "Check that events list is not empty");

                                ValidateMessages(results);

                                GetMessageElements(results, rawResults, elements);

                                ValidateRecordingEvents(recording, results, elements, false);

                                ValidateOrder(results, startTime, endTime);

                            });

                    validateAction(start, end);
                    List<FindEventResult> results1 = new List<FindEventResult>();
                    results1.AddRange(results);

                    elements = elements2;
                    validateAction(end, start);
                    List<FindEventResult> results2 = new List<FindEventResult>();
                    results2.AddRange(results);

                    // compare lists
                    CompareLists(results1, results2, elements1, elements2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");
                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        EndSearch(searchToken);
                    }
                });
        }
        
        [Test(Name = "FIND EVENTS (MAXMATCHES = 1)",
           Path = PATH,
           Order = "03.01.05",
           Id = "3-1-5",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents })]
        public void FindEventsSearchWithMaxMatchesTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    DateTime start;
                    DateTime end;

                    DefineEventsSearchRange(recording, out start, out end);

                    List<FindEventResult> results = new List<FindEventResult>();

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };
                    searchToken = FindEvents(scope, filter, start, end, false, 1, keepAlive);
                    results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", null, out state);

                    Assert(results != null && results.Count > 0, "No events found",
                           "Check that events list is not empty");

                    Assert(results.Count == 1, string.Format("{0} events found", results.Count),
                        "Check that maxMatches parameter is not exceeded");
                    
                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        EndSearch(searchToken);
                    }
                });
        }
        
        [Test(Name = "GET EVENT SEARCH RESULTS WITH INVALID SEARCHTOKEN",
           Path = PATH,
           Order = "03.01.10",
           Id = "3-1-10",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Optional,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents })]
        public void GetEventsSearchResultInvalidTokenTest()
        {
            RunTest( 
                () =>
                    {
                        GetEventSearchResults request = new GetEventSearchResults();
                        request.SearchToken = Guid.NewGuid().ToString().Substring(0,8);
                        request.WaitTime = "PT5S";
                        RunStep(
                            () =>
                                {
                                    Client.GetEventSearchResults(request);
                                }, "Get events search results with invalid token",
                            Definitions.Onvif.OnvifFaults.InvalidToken, true, false);
                    });

        }

    }
}
