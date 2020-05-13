using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Onvif;
using System.Linq;
using System.Text;
using TestTool.Tests.CommonUtils;
using TestTool.Tests.TestCases.Profiles;

namespace TestTool.Tests.TestCases.TestSuites.Search
{
    [TestClass]
    public partial class PTZSearchTestSuite : Base.SearchTest
    {
        private const string PATH = "Search\\PTZ Search";
        private const string MEDIA_ATTRIBUTES = "Search\\Media Attributes";

        private int _searchKeepAlive = 10; // in seconds

        public PTZSearchTestSuite(TestLaunchParam param)
            : base(param)
        {

        }


        [Test(Name = "FIND PTZ POSITION - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS EQUAL TO RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "06.01.08",
           Id = "6-1-8",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void PTZSearchEndpointsEqualToRecordingEndpointsTest()
        {
            CommonForwardBackwardPtzPositionSearchTest(DefineSearchRange); 
        }

        [Test(Name = "FIND PTZ POSITION - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS OUTSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "06.01.09",
           Id = "6-1-9",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void PTZSearchEndpointsOutsideRecordingEndpointsTest()
        {
            CommonForwardBackwardPtzPositionSearchTest(DefineSearchRangeOutside);
        }

        [Test(Name = "FIND PTZ POSITION - FORWARD AND BACKWARD SEARCH (SEARCH ENDPOINTS INSIDE RECORDING ENDPOINTS)",
           Path = PATH,
           Order = "06.01.10",
           Id = "6-1-10",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void PTZSearchEndpointsInsideRecordingEndpointsTest()
        {
            CommonForwardBackwardPtzPositionSearchTest(DefineSearchRangeInside);
        }

        [Test(Name = "FIND PTZ POSITION (MAXMATCHES = 1)",
           Path = PATH,
           Order = "06.01.11",
           Id = "6-1-11",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void FindPtzPositionsSearchWithMaxMatchesTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", _searchKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    SearchRange range = DefineSearchRange(recording);

                    System.DateTime start = range.Start;
                    System.DateTime end = range.End;

                    List<FindPTZPositionResult> results;

                    PTZPositionFilter filter = new PTZPositionFilter();
                    filter.MinPosition = new PTZVector();
                    filter.MinPosition.PanTilt = new Vector2D() { x = -1, y = -1 };
                    filter.MaxPosition = new PTZVector();
                    filter.MaxPosition.PanTilt = new Vector2D() { x = 1, y = 1 };

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };
                    searchToken = FindPTZPosition(start, end, scope, filter, 1, keepAlive);
                    results = GetAllPtzSearchResults(searchToken, filter, null, null, "PT5S", out state);

                    Assert(results != null && results.Count > 0, "No PTZ positions found",
                           "Check that PTZ positions list is not empty");

                    Assert(results.Count == 1, string.Format("{0} PTZ positions found", results.Count),
                           "Check that maxMatches parameter is not exceeded");

                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        ReleaseSearch(searchToken, _searchTimeout);
                    }
                });
        }


        [Test(Name = "FIND PTZ POSITIONS USING RECORDING INFORMATION FILTER",
           Path = PATH,
           Order = "06.01.12",
           Id = "6-1-12",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void PTZSearchWithRecordingFilterTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;
            int testKeepAlive = _searchKeepAlive;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    System.DateTime start = System.DateTime.MinValue;

                    PTZPositionFilter filter = new PTZPositionFilter();
                    filter.MinPosition = new PTZVector();
                    filter.MinPosition.PanTilt = new Vector2D() { x = -1, y = -1 };
                    filter.MaxPosition = new PTZVector();
                    filter.MaxPosition.PanTilt = new Vector2D() { x = 1, y = 1 };

                    SearchScope scope = new SearchScope();
                    scope.RecordingInformationFilter = "boolean(//Track[TrackType = \"Audio\"])";

                    List<FindPTZPositionResult> results = new List<FindPTZPositionResult>();

                    state = SearchState.Completed;
                    searchToken = FindPTZPosition(start, null, scope, filter, 20, keepAlive);

                    results = GetAllPtzSearchResults(searchToken, filter, 1, null, "PT5S", out state);

                    if (results != null)
                    {
                        List<string> recordingTokens = new List<string>();

                        foreach (FindPTZPositionResult result in results)
                        {
                            string recordingToken = result.RecordingToken;
                            if (!recordingTokens.Contains(recordingToken))
                            {
                                recordingTokens.Add(recordingToken);
                            }
                        }

                        List<string> correctRecordings = new List<string>();
                        Dictionary<string, RecordingInformation> recordingInfos = new Dictionary<string, RecordingInformation>();

                        foreach (string token in recordingTokens)
                        {
                            RecordingInformation info = GetRecordingInformation(token);
                            recordingInfos.Add(token, info);
                            bool hasAudio = false;
                            if (info.Track != null)
                            {
                                hasAudio = info.Track.Where(T => T.TrackType == TrackType.Audio).FirstOrDefault() != null;
                            }

                            if (hasAudio)
                            {
                                correctRecordings.Add(token);
                            }
                        }

                        {
                            StringBuilder sb = new StringBuilder();
                            bool ok = true;
                            foreach (FindPTZPositionResult result in results)
                            {
                                string recordingToken = result.RecordingToken;

                                if (!correctRecordings.Contains(recordingToken))
                                {
                                    ok = false;
                                    sb.AppendLine(
                                        string.Format("Result for time='{0}' belongs to recording '{1}' which does not have tracks of type Audio",
                                        result.Time.StdDateTimeToString(), recordingToken));
                                }
                                else
                                {
                                    RecordingInformation recording = recordingInfos[recordingToken];

                                    if (!string.IsNullOrEmpty(result.TrackToken) && recording.Track != null)
                                    {
                                        if (recording.Track.Where(T => T.TrackToken == result.TrackToken).FirstOrDefault() == null)
                                        {
                                            ok = false;
                                            sb.AppendFormat("Track with token '{0}', specified in result for time {1} does not belong to recording {2}{3}",
                                                result.TrackToken, result.Time.StdDateTimeToString(), recordingToken, Environment.NewLine);
                                        }
                                    }

                                    SearchRange range = DefineSearchRange(recording);
                                    if (result.Time < range.Start || result.Time > range.End)
                                    {
                                        ok = false;
                                        sb.AppendFormat("Result with time {0} is out of corresponding recording interval ({1}; {2}){3}",
                                            result.Time.StdDateTimeToString(), 
                                            range.Start.StdDateTimeToString(), 
                                            range.End.StdDateTimeToString(), Environment.NewLine);
                                    }

                                    if (result.Position != null)
                                    {
                                        bool positionOk = true;
                                        if (result.Position.PanTilt != null)
                                        {
                                            positionOk = result.Position.PanTilt.x >= filter.MinPosition.PanTilt.x &&
                                                result.Position.PanTilt.x <= filter.MaxPosition.PanTilt.x &&
                                                result.Position.PanTilt.y >= filter.MinPosition.PanTilt.y &&
                                                result.Position.PanTilt.y <= filter.MaxPosition.PanTilt.y;
                                        }
                                        else
                                        {
                                            positionOk = false;
                                        }
                                        if (!positionOk)
                                        {
                                            ok = false;
                                                                                        
                                            sb.AppendFormat("PTZ Pozition specified in result for time {0} is out of search interval{1}",
                                                result.Time.StdDateTimeToString(), Environment.NewLine);
                                        }
                                    }
                                    else
                                    {
                                        ok = false;
                                        string.Format("Result for time='{0}' does not contain PTZ position", result.Time.StdDateTimeToString());
                                    }

                                }
                            }
                            Assert(ok, sb.ToStringTrimNewLine(), "Validate search results");
                        }
                    }
                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        ReleaseSearch(searchToken, testKeepAlive * 1000);
                    }
                });        
        }

        [Test(Name = "FIND PTZ POSITION - SEARCHING IN A CERTAIN POSITION",
           Path = PATH,
           Order = "06.01.13",
           Id = "6-1-13",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindPTZPosition, Functionality.GetPTZPositionSearchResults })]
        public void PTZSearchForCertainPositionTest()
        {
            string searchToken = string.Empty;
            SearchState state = SearchState.Completed;
            int testKeepAlive = _searchKeepAlive;

            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    RecordingInformation recording = FindRecordingForTest();

                    SearchRange range = DefineSearchRange(recording);

                    System.DateTime start = range.Start;
                    System.DateTime end = range.End;

                    PTZPositionFilter filter = new PTZPositionFilter();
                    filter.MinPosition = new PTZVector();
                    filter.MinPosition.PanTilt = new Vector2D() { x = -1, y = -1 };
                    filter.MaxPosition = new PTZVector();
                    filter.MaxPosition.PanTilt = new Vector2D() { x = 1, y = 1 };

                    SearchScope scope = new SearchScope();
                    scope.IncludedRecordings = new string[] { recording.RecordingToken };

                    List<FindPTZPositionResult> results = new List<FindPTZPositionResult>();

                    state = SearchState.Completed;
                    searchToken = FindPTZPosition(end, start, scope, filter, null, keepAlive);

                    results = GetAllPtzSearchResults(searchToken, filter, 1, null, "PT5S", out state);

                    Assert(results != null && results.Count > 0, "No PTZ positions found",
                           "Check that PTZ positions list is not empty");

                    /// check that results belong to specified recording?
                    ValidatePTZPositionResults(results, recording, start, end);

                    FindPTZPositionResult result = results.FirstOrDefault(R => R.Position != null);

                    Assert(result != null, "No search results with PTZ position returned", "Select PTZ position for test");

                    filter.MinPosition = result.Position;
                    filter.MaxPosition = result.Position;
                    filter.EnterOrExit = false;

                    state = SearchState.Completed;
                    searchToken = FindPTZPosition(end, start, scope, filter, null, keepAlive, "Find specified PTZ position");

                    results = GetAllPtzSearchResults(searchToken, filter, 1, null, "PT5S", out state);

                    Assert(results != null && results.Count > 0, "No PTZ positions found",
                           "Check that PTZ positions list is not empty");

                    /// check that results belong to specified recording?
                    ValidatePTZPositionResults(results, recording, start, end);
                    
                    bool ok = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (FindPTZPositionResult r in results)
                    {
                        // compare positions - ?
                        if (r.Position == null)
                        {
                            ok = false;
                            sb.AppendLine(
                                string.Format("Result for {0} contains no PTZ position",
                                r.Time.StdDateTimeToString()));
                        }
                        else 
                        {
                            if (r.Position.PanTilt == null)
                            {
                                ok = false;
                                sb.AppendLine(
                                    string.Format("Result for {0} contains no PanTilt in PTZ position",
                                    r.Time.StdDateTimeToString()));
                            }
                            else
                            {
                                ok = (r.Position.PanTilt.x == result.Position.PanTilt.x &&
                                    r.Position.PanTilt.y == result.Position.PanTilt.y &&
                                    r.Position.PanTilt.space == result.Position.PanTilt.space);
                                if (!ok)
                                {
                                    sb.AppendLine(
                                        string.Format("Position in result for {0} is not as expected: PanTilt.x={1}, PanTilt.y={2}, PanTilt.space='{3}'",
                                        r.Time.StdDateTimeToString(), r.Position.PanTilt.x, r.Position.PanTilt.y, r.Position.PanTilt.space));
                                
                                }
                            }
                        }
                    }

                    Assert(ok, sb.ToStringTrimNewLine(), "Check PTZ Positions returned");

                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        ReleaseSearch(searchToken, testKeepAlive * 1000);
                    }
                });
        }



        [Test(Name = "GET PTZ POSITION SEARCH RESULTS WITH INVALID SEARCHTOKEN",
           Path = PATH,
           Order = "06.01.07",
           Id = "6-1-7",
           Category = Category.SEARCH,
           Version = 2.1,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.GetPTZPositionSearchResults })]
        public void SearchPTZPositionInvalidSearchTokenTest()
        {
            RunTest(() => this.InvalidTokenTestBody((s) => Client.GetPTZPositionSearchResults(s, null, null, null), 
                                                    RunStep, "PTZ Position search results", OnvifFaults.InvalidToken));
        }

        [Test(Name = "GET MEDIA ATTRIBUTES – PTZ MEDIA ATTRIBUTES",
           Path = MEDIA_ATTRIBUTES,
           Order = "07.01.01",
           Id = "7-1-1",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.PTZPositionSearch },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.GetMediaAttributes })]
        public void GetMediaAttributesPTZMediaAttributesTest()
        {
            RunTest(() =>
                    {
                        var recordingInformation = FindRecordingForTest();
                        var recordingToken = recordingInformation.RecordingToken;

                        var T1 = recordingInformation.EarliestRecordingSpecified ? recordingInformation.EarliestRecording : recordingInformation.Track.Select(e => e.DataFrom).Min();
                        var T2 = recordingInformation.LatestRecordingSpecified ? recordingInformation.LatestRecording : recordingInformation.Track.Select(e => e.DataTo).Max();

                        var filter = new PTZPositionFilter()
                            {
                                MinPosition = new PTZVector(){ PanTilt = new Vector2D(){ x = -1, y = -1 } },
                                MaxPosition = new PTZVector(){ PanTilt = new Vector2D(){ x = 1, y = 1 } },
                                EnterOrExit = false
                            };
                        var searchScope = new SearchScope() { IncludedRecordings = new []{ recordingToken } };

                        var searchToken = FindPTZPosition(T1, T2, searchScope, filter, null, "PT10S");


                        FindPTZPositionResultList latestResponse;
                        do
                        {
                            latestResponse = GetPTZPositionSearchResults(searchToken, null, null, "PT5S");
                        } while (!(SearchState.Completed == latestResponse.SearchState || (null != latestResponse.Result && latestResponse.Result.Any())));

                        var results = latestResponse.Result;
                        
                        ValidatePTZPositionSearchResults(filter, results);

                        if (SearchState.Completed == latestResponse.SearchState)
                            Assert(null != results && results.Any(), "The search state 'Completed' has been reached but no result is recieved", "Checking recieved result");
                        else
                            EndSearch(searchToken);

                        var selected = results.First();

                        var mediaAttributes = GetMediaAttributes(recordingToken, selected.Time);
                        
                        Assert(mediaAttributes.All(e => e.RecordingToken == recordingToken),
                               string.Format("There is an Media Attribute with 'RecordingToken' other than {0}", recordingToken),
                               "Checking field 'RecordingToken' of recieved Media Attributes");

                        Assert(mediaAttributes.All(e => null == e.TrackAttributes || e.TrackAttributes.All(et => null == et.MetadataAttributes || et.MetadataAttributes.CanContainPTZ)),
                               "There is an Media Attribute with 'TrackAttributes.MetadataAttributes.CanContainPTZ' is equal to false",
                               "Checking field 'TrackAttributes.MetadataAttributes.CanContainPTZ' of recieved Media Attributes");

                        if (DeclaredScopes.Contains(typeof(StorageProfile).GetProfileScope()))
                        {
                            if (null != selected.Position.PanTilt)
                            {
                                const string panTiltPositionGenericNamespace = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace";
                                var flag = mediaAttributes.All(e => null == e.TrackAttributes
                                                                    || e.TrackAttributes.Where(a => null != a.TrackInformation && selected.TrackToken == a.TrackInformation.TrackToken)
                                                                                        .All(a => null != a.MetadataAttributes && null != a.MetadataAttributes.PtzSpaces
                                                                                                  && a.MetadataAttributes.PtzSpaces.Contains(panTiltPositionGenericNamespace)));

                                Assert(flag,
                                       "There is a TrackAttribute for selected TrackToken without specified PanTilt Position Generic namespace while in selected Search Result the field 'Position.PanTilt' is present", 
                                       "Checking PanTilt Position Generic namespace presence");
                            }

                            if (null != selected.Position.Zoom)
                            {
                                const string zoomPositionGenericNamespace = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace";
                                var flag = mediaAttributes.All(e => null == e.TrackAttributes
                                                                    || e.TrackAttributes.Where(a => null != a.TrackInformation && selected.TrackToken == a.TrackInformation.TrackToken)
                                                                                        .All(a => null != a.MetadataAttributes && null != a.MetadataAttributes.PtzSpaces
                                                                                                  && a.MetadataAttributes.PtzSpaces.Contains(zoomPositionGenericNamespace)));

                                Assert(flag,
                                       "There is a TrackAttribute for selected TrackToken without specified Zoom Position Generic namespace while in selected Search Result the field 'Position.Zoom' is present", 
                                       "Checking Zoom Position Generic namespace presence");
                            }
                        }
                        else
                        {
                            LogStepEvent("WARNING: Profile G is not declared as supported. Checking of MediaAttributes.TrackAttributes.MetadataAttributes.PtzSpaces won't be performed.");
                            LogStepEvent("");
                        }
                    });
        }
    }
}
