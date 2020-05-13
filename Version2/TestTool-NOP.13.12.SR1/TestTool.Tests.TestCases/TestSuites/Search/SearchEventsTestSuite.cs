using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Definitions.Trace;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.TestCases.Base;
using DateTime=System.DateTime;

namespace TestTool.Tests.TestCases
{
    [TestClass]
    public partial class SearchEventsTestSuite : Base.SearchTest
    {
        private ServiceHolder<EventPortTypeClient, EventPortType> _eventPortTypeClient;
        private EventPortTypeClient eventService
        {
            get
            {
                if (null == _eventPortTypeClient)
                {
                    _eventPortTypeClient = new ServiceHolder<EventPortTypeClient, EventPortType>((features) => DeviceClient.GetServiceAddress(OnvifService.EVENTS),
                                                                                                 (binding, address) => new EventPortTypeClient(binding, address),
                                                                                                 "Event Service");


                    InitServiceClient(_eventPortTypeClient, 
                                      new IChannelController[]
                                      { new SoapValidator(EventsSchemasSet.GetInstance()) });
                }

                return _eventPortTypeClient.Client;
            }
        }

        private const string PATH = "Search\\Events Search";

        private int _searchKeepAlive = 10; // in seconds

        public SearchEventsTestSuite(TestLaunchParam param)
            : base(param)
        {
            //eventServiceProvider = new EventServiceProvider(param);
        }

        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS EQUAL TO RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.01",
           Id = "3-1-1",
           Category = Category.SEARCH,
           Version = 2.0 ,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents, Functionality.GetEventSearchResults })]
        public void FindEventsForwardSearchTest()
        {
            CommonForwardBackwardSearchEventsTest(DefineSearchRange, false, true);
        }

        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS OUTSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.02",
           Id = "3-1-2",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents, Functionality.GetEventSearchResults })]
        public void FindEventsForwardSearchExtendedPeriodTest()
        {
            CommonForwardBackwardSearchEventsTest(DefineSearchRangeOutside, false, true);
        }
        
        [Test(Name = "FIND EVENTS – FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS INSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "03.01.11",
           Id = "3-1-11",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents, Functionality.GetEventSearchResults })]
        public void FindEventsForwardSearchNarrowedPeriodTest()
        {
            //CommonForwardBackwardSearchEventsTest(DefineSearchRangeInside, true, false);
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    List<EventsTopicInfo> availableEvents = GetTopics();

                    RecordingInformation recording = FindRecordingForTest();

                    SearchRange range = DefineSearchRangeInside(recording);

                    DateTime start = range.Start;
                    DateTime end = range.End;

                    if (0 != start.Millisecond)
                        start = start.AddTicks(-(start.Ticks % TimeSpan.TicksPerSecond));

                    if (0 != end.Millisecond)
                    {
                        end = end.AddTicks(-(end.Ticks % TimeSpan.TicksPerSecond));
                        end = end.AddSeconds(1);
                    }

                    List<FindEventResult> results = new List<FindEventResult>();

                    Dictionary<FindEventResult, XmlElement> elements1 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements2 = new Dictionary<FindEventResult, XmlElement>();
                    Dictionary<FindEventResult, XmlElement> elements = elements1;

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };

                    Action<DateTime, DateTime> validateAction =
                        (startTime, endTime) =>
                            {
                                state = SearchState.Completed;

                                searchToken = FindEvents(scope, filter, startTime, endTime, true, null, keepAlive);

                                Dictionary<FindEventResult, XmlDocument> rawResults = new Dictionary<FindEventResult, XmlDocument>();

                                results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", elements, out state);

                                Assert(results != null && results.Count > 0, "No events found", "Check that events list is not empty");

                                ValidateMessages(results);

                                //GetMessageElements(results, rawResults, elements);

                                ValidateRecordingEvents(recording, results, elements, false, (startTime < endTime));

                                ValidateOrder(results, startTime, endTime);
                            };

                    Action<List<FindEventResult>, Dictionary<FindEventResult, XmlElement>, DateTime, string> validateVirtualEventsAction =
                        (events, xmlElements, time, pointType) =>
                            {
                                var virtualEvents = events.Where(e => e.StartStateEvent && e.Time == time);

                                var token = recording.RecordingToken;
                                Func<FindEventResult, string, bool> checkEventTopic = (e, s) => CheckEventTopic(e, elements[e], s, TNS1NAMESPACE);
                                var recStateEventName = "tns1:RecordingHistory/Recording/State";
                                Assert(virtualEvents.Any(e => token == e.RecordingToken && checkEventTopic(e, recStateEventName)),
                                                                  string.Format("No virtual events with topic {0} are received at {1}", recStateEventName, pointType),
                                                                  string.Format("Check that virtual events with topic {0} are received at {1}", recStateEventName, pointType));

                                var trackStateEventName = "tns1:RecordingHistory/Track/State";
                                var virtualTrackEvents = virtualEvents.Where(e => token == e.RecordingToken && checkEventTopic(e, trackStateEventName));
                                Assert(virtualTrackEvents.Any(),
                                       string.Format("No virtual events with topic {0} are received at {1}", trackStateEventName, pointType),
                                       string.Format("Check that virtual events with topic {0} are received at {1}", trackStateEventName, pointType));

                                Assert(recording.Track.All(t => virtualTrackEvents.Any(e => e.TrackToken == t.TrackToken)),
                                       string.Format("There is a track(s) for that event {0} is not received at {1}", trackStateEventName, pointType),
                                       string.Format("Check that for all tracks event {0} is received", trackStateEventName));

                                
                                var videoParametersEventName = "tns1:RecordingHistory/Track/VideoParameters";
                                if (availableEvents.Any(e => e.Topic == videoParametersEventName))
                                {
                                    var virtualVideoEvents = virtualEvents.Where(e => token == e.RecordingToken && checkEventTopic(e, videoParametersEventName));
                                    Assert(virtualVideoEvents.Any(),
                                           string.Format("No virtual events with topic {0} are received at {1}", videoParametersEventName, pointType),
                                           string.Format("Check that virtual events with topic {0} are received at {1}", videoParametersEventName, pointType));

                                    Assert(recording.Track.Where(t=> TrackType.Video == t.TrackType).All(t => virtualVideoEvents.Any(e => e.TrackToken == t.TrackToken)),
                                           string.Format("There is a video track(s) for that event {0} is not received at {1}", videoParametersEventName, pointType),
                                           string.Format("Check that for all video tracks event {0} is received at {1}", videoParametersEventName, pointType));
                                }

                                var audioParametersEventName = "tns1:RecordingHistory/Track/AudioParameters";
                                if (availableEvents.Any(e => e.Topic == audioParametersEventName))
                                {
                                    var virtualAudioEvents = virtualEvents.Where(e => token == e.RecordingToken && checkEventTopic(e, audioParametersEventName));
                                    Assert(virtualAudioEvents.Any(),
                                           string.Format("No virtual events with topic {0} are received at {1}", audioParametersEventName, pointType),
                                           string.Format("Check that virtual events with topic {0} are received at {1}", audioParametersEventName, pointType));

                                    Assert(recording.Track.Where(t => TrackType.Audio == t.TrackType).All(t => virtualAudioEvents.Any(e => e.TrackToken == t.TrackToken)),
                                           string.Format("There is a audio track(s) for that event {0} is not received at {1}", audioParametersEventName, pointType),
                                           string.Format("Check that for all audio tracks event {0} is received at {1}", audioParametersEventName, pointType));
                                }
                            };


                    validateAction(start, end);
                    validateVirtualEventsAction(results, elements, start, "start point");
                    List<FindEventResult> results1 = new List<FindEventResult>();
                    results1.AddRange(results);

                    elements = elements2;
                    validateAction(end, start);
                    validateVirtualEventsAction(results, elements, start, "start point");
                    validateVirtualEventsAction(results, elements, end, "end point");
                    List<FindEventResult> results2 = new List<FindEventResult>();
                    results2.AddRange(results);

                    // compare lists

                    CompareLists(results1, results2, elements1, elements2, "list received for StartPoint < EndPoint", "list received for StartPoint > EndPoint");

                },
                        () =>
                {
                    if (state != SearchState.Completed)
                    {
                        ReleaseSearch(searchToken, _searchTimeout);
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
           FunctionalityUnderTest = new Functionality[] { Functionality.FindEvents, Functionality.GetEventSearchResults })]
        public void FindEventsSearchWithMaxMatchesTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    SearchRange range = DefineSearchRange(recording);

                    DateTime start = range.Start;
                    DateTime end = range.End;

                    List<FindEventResult> results;

                    EventFilter filter = new EventFilter();

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };
                    searchToken = FindEvents(scope, filter, start, end, false, 1, keepAlive);
                    results = GetAllEventsSearchResults(searchToken, null, null, "PT5S", out state);

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
