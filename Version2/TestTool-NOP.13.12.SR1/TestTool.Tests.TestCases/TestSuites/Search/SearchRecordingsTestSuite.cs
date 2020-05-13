using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.TestCases.TestSuites;

namespace TestTool.Tests.TestCases
{
    [TestClass]
    public partial class SearchRecordingsTestSuite : Base.SearchTest
    {

        public SearchRecordingsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        private int _searchKeepAlive = 10; // in seconds

        private const string PATH = "Search\\Recordings Search";

        [Test(Name = "GET RECORDING SEARCH RESULTS AND GET RECORDINGS CONSISTENCY",
           Path = PATH,
           Order = "02.01.01",
           Id = "2-1-1",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService, Feature.RecordingControlService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings, Functionality.GetRecordings, Functionality.GetRecordings })]
        public void GetRecordingAndSearchResultsConsistencyTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    GetRecordingsResponseItem[] recordings = GetAllRecordings();
                    // GetAllRecordings() includes validation of recordings list (not empty, tokens unique)

                    string keepAlive = string.Format("PT{0}S", testKeepAlive);
                    SearchScope scope = new SearchScope();
                    searchToken = FindRecordings(scope, null, keepAlive);

                    List<RecordingInformation> foundRecordings = GetAllRecordingsSearchResults(searchToken, 1, 1, "PT5S", out state);

                    // list not empty;
                    // tokens unique;
                    // DataFrom/DataTo and track information consistent
                    ValidateSearchResult(foundRecordings);

                    Assert(foundRecordings.Count == recordings.Length, "Number of recordings for GetRecordings and for Search is different", "Check that number of recordings is the same");

                    bool ok = true;
                    StringBuilder logger = new StringBuilder();

                    // check that all tokens from full list are present in list of found recordings
                    foreach (GetRecordingsResponseItem item in recordings)
                    {
                        string token = item.RecordingToken;
                        RecordingInformation[] foundInfos =
                            foundRecordings.Where(RI => RI.RecordingToken == token).ToArray();

                        if (foundInfos.Length == 0)
                        {
                            logger.AppendFormat(
                                    "Recording with token {0} not found within recordings got via GetRecordingSearchResults{1}",
                                    item.RecordingToken, Environment.NewLine);
                            ok = false;
                            //break;
                        }
                    }

                    Assert(ok, logger.ToStringTrimNewLine(), "Check that all recordings are returned");

                    //
                    // tokens
                    //
                    CheckInclusion(recordings, foundRecordings);

                    // Check recordings. Tokens and inclusion are already checked.
                    bool localOk = CheckRecordings(recordings, foundRecordings, logger);
                    ok = ok && localOk;

                    Assert(ok, logger.ToStringTrimNewLine(), "Validate records found");

                },
                () =>
                {
                    if (state != SearchState.Completed)
                    {
                        // 10000 is 10S from request.
                        // if chaged to "Get from UI", change it in both places
                        ReleaseSearch(searchToken, testKeepAlive * 1000);
                    }

                });
        }

        [Test(Name = "GET RECORDING SEARCH RESULTS WITH MINRESULTS",
           Path = PATH,
           Order = "02.01.03",
           Id = "2-1-3",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings, Functionality.GetRecordingSearchResults })]
        public void FindRecordingWithMinResultsTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    List<RecordingInformation> currentInfos = null;

                    Action<int?> checkAction =
                    new Action<int?>(
                        (minResults) =>
                        {
                            SearchScope searchScope = new SearchScope();
                            searchToken = FindRecordings(searchScope, null, keepAlive);

                            List<RecordingInformation> recordings = GetAllRecordingsSearchResults(searchToken, minResults, null, null, out state);

                            ValidateSearchResult(recordings);

                            currentInfos.AddRange(recordings);

                        });

                    // Get total number of recordings;
                    RecordingSummary summary = GetRecordingSummary();
                    int N1 = summary.NumberRecordings;

                    List<RecordingInformation> recordings1 = new List<RecordingInformation>();
                    currentInfos = recordings1;
                    checkAction(N1);

                    List<RecordingInformation> recordings2 = new List<RecordingInformation>();
                    currentInfos = recordings2;
                    checkAction(N1 + 1);

                    // compare lists of recordings

                    Assert(recordings1.Count == recordings2.Count,
                        "Number of recordings is different for different minResults parameter",
                        "Check that number of recordings is the same");

                    CompareLists(recordings1, recordings2);

                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       // 10000 is 10S from request.
                       // if chaged to "Get from UI", change it in both places
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }

               });
        }

        [Test(Name = "GET RECORDING SEARCH RESULTS WITH MAXRESULTS",
           Path = PATH,
           Order = "02.01.04",
           Id = "2-1-4",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMaxResultsTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    // total number
                    RecordingSummary summary = GetRecordingSummary();
                    int N1 = summary.NumberRecordings;

                    Action<int?> checkAction =
                        new Action<int?>(
                            (maxResults) =>
                            {
                                SearchScope searchScope = new SearchScope();
                                searchToken = FindRecordings(searchScope, null, keepAlive);
                                List<RecordingInformation> recordings = GetAllRecordingsSearchResults(searchToken, null, maxResults, null, out state);

                                ValidateSearchResult(recordings);

                            });

                    checkAction(1);
                    checkAction(N1);
                    checkAction(N1 + 1);

                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }
               });
        }

        [Test(Name = "GET RECORDING SEARCH RESULTS WITH WAITTIME",
           Path = PATH,
           Order = "02.01.05",
           Id = "2-1-5",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithWaitTimeTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);


                    Action<string> checkAction =
                        new Action<string>(
                            (waitTime) =>
                            {
                                SearchScope scope = new SearchScope();
                                searchToken = FindRecordings(scope, null, keepAlive);
                                List<RecordingInformation> recordings = GetAllRecordingsSearchResults(searchToken, null, null, waitTime, out state);

                                ValidateSearchResult(recordings);

                            });

                    checkAction("PT0.5S");
                    checkAction("PT1S");

                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       // 10000 is 10S from request.
                       // if chaged to "Get from UI", change it in both places
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }

               });
        }


        [Test(Name = "FIND RECORDINGS WITH MAXMATCHES",
           Path = PATH,
           Order = "02.01.07",
           Id = "2-1-7",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMaxMatchesTest()
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    RecordingSummary summary = GetRecordingSummary();
                    int N1 = summary.NumberRecordings;

                    Action<int> checkAction =
                        new Action<int>(
                            (maxMatches) =>
                            {
                                SearchScope searchScope = new SearchScope();

                                searchToken = FindRecordings(searchScope, maxMatches, keepAlive);
                                List<RecordingInformation> recordings =
                                    GetAllRecordingsSearchResults(searchToken, null, null, null, out state);

                                ValidateSearchResult(recordings);

                                Assert(maxMatches >= recordings.Count,
                                    string.Format("Number of recordings found ({0}) is greater than maxMatches search parameter ({1})", recordings.Count, maxMatches),
                                    "Check that number of recordings returned is not greater than maxMatches parameter passsed");

                            });

                    checkAction(1);
                    if (N1 > 1)
                    {
                        if (N1 > 2)
                        {
                            checkAction(N1 - 1);
                        }
                        checkAction(N1);
                    }

                },
               () =>
               {
                   if (state != SearchState.Completed)
                   {
                       // 10000 is 10S from request.
                       // if chaged to "Get from UI", change it in both places
                       ReleaseSearch(searchToken, testKeepAlive * 1000);
                   }

               });
        }


        [Test(Name = "FIND RECORDINGS WITH RECORDING INFORMATION FILTER (ONLY VIDEO)",
           Path = PATH,
           Order = "02.01.08",
           Id = "2-1-8",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMFilterTest()
        {
            FindRecordingWithFilterTest("boolean(//Track[TrackType = \"Video\"])",
                new[] { TrackType.Video });
        }


        [Test(Name = "GET RECORDING SEARCH RESULTS WITH INVALID SEARCHTOKEN",
               Path = PATH,
               Order = "02.01.10",
               Id = "2-1-10",
               Category = Category.SEARCH,
               Version = 2.0,
               RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
               RequirementLevel = RequirementLevel.Optional,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void GetRecordingSearchResultsInvalidTokenTest()
        {
            RunTest(
                () =>
                {
                    string searchToken = Guid.NewGuid().ToString().Substring(0, 8);

                    RunStep(
                        () =>
                        {
                            GetRecordingSearchResults request = new GetRecordingSearchResults();
                            request.SearchToken = searchToken;
                            Client.GetRecordingSearchResults(request);
                        }, string.Format("Get recordings search result with invalid token ({0})", searchToken),
                            Definitions.Onvif.OnvifFaults.InvalidToken,
                            true);

                });
        }


        [Test(Name = "END SEARCH WITH INVALID SEARCHTOKEN",
               Path = PATH,
               Order = "02.01.11",
               Id = "2-1-11",
               Category = Category.SEARCH,
               Version = 2.0,
               RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
               RequirementLevel = RequirementLevel.Optional,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void EndSearchResultsInvalidTokenTest()
        {
            RunTest(() =>
                        {
                            string testToken = Guid.NewGuid().ToString().Substring(0, 8);
                            RunStep(
                                () =>
                                {
                                    Client.EndSearch(testToken);
                                }, string.Format("End search with invalid token ({0})", testToken),
                                    TestTool.Tests.Definitions.Onvif.OnvifFaults.InvalidToken,
                                    true);

                        });
        }


        [Test(Name = "GET RECORDING SEARCH RESULTS AFTER END OF SEARCH (ENDSEARCH COMMAND WAS INVOKED)",
               Path = PATH,
               Order = "02.01.12",
               Id = "2-1-12",
               Category = Category.SEARCH,
               Version = 2.0,
               RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
               RequirementLevel = RequirementLevel.Optional,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void GetRecordingSearchResultsAfterEndSearchTest()
        {
            int testKeepAlive = _searchKeepAlive;
            bool searchEnded = true;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);
                    SearchScope scope = new SearchScope();

                    searchToken = FindRecordings(scope, null, keepAlive);
                    searchEnded = false;

                    EndSearch(searchToken);
                    searchEnded = true;

                    RunStep(
                        () =>
                        {
                            GetRecordingSearchResults request = new GetRecordingSearchResults();
                            request.SearchToken = searchToken;
                            Client.GetRecordingSearchResults(request);
                        },
                            "Get recordings search result",
                            Definitions.Onvif.OnvifFaults.InvalidToken,
                            true);

                },
                () =>
                {
                    if (!searchEnded)
                    {
                        ReleaseSearch(searchToken, testKeepAlive * 1000);
                    }

                });
        }


        [Test(Name = "GET RECORDING SUMMARY",
           Path = "Search\\Recordings Summary",
           Order = "04.01.01",
           Id = "4-1-1",
           Category = Category.SEARCH,
           Version = 2.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.GetRecordingSummary })]
        public void GetRecordingSummaryTest()
        {
            RunTest(
                () =>
                {

                    RecordingSummary summary = GetRecordingSummary();

                    Assert(summary.DataUntil >= summary.DataFrom,
                        string.Format("DataUntil ({0}) less than DataFrom ({1})", summary.DataUntil.StdDateTimeToString(), summary.DataFrom.StdDateTimeToString()),
                        "Validate RecordingSummary structure received");
                });
        }

        [Test(Name = "FIND RECORDINGS WITH RECORDING INFORMATION FILTER (ONLY AUDIO)",
           Path = PATH,
           Order = "02.01.13",
           Id = "2-1-13",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMFilterTestAudio()
        {
            FindRecordingWithFilterTest("boolean(//Track[TrackType = \"Audio\"])",
                new[] { TrackType.Audio });
        }

        [Test(Name = "FIND RECORDINGS WITH RECORDING INFORMATION FILTER (ONLY METADATA)",
           Path = PATH,
           Order = "02.01.14",
           Id = "2-1-14",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMFilterTestMetadata()
        {
            FindRecordingWithFilterTest("boolean(//Track[TrackType = \"Metadata\"])",
                new[] { TrackType.Metadata });
        }

        [Test(Name = "FIND RECORDINGS WITH RECORDING INFORMATION FILTER (VIDEO AND AUDIO)",
           Path = PATH,
           Order = "02.01.15",
           Id = "2-1-15",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMFilterTestVideoAndAudio()
        {
            FindRecordingWithFilterTest("boolean(//Track[TrackType = \"Video\"]) and boolean(//Track[TrackType = \"Audio\"])",
                new[] { TrackType.Video, TrackType.Audio });
        }

        [Test(Name = "FIND RECORDINGS WITH RECORDING INFORMATION FILTER (VIDEO AND METADATA)",
           Path = PATH,
           Order = "02.01.16",
           Id = "2-1-16",
           Category = Category.SEARCH,
           Version = 1.0,
           RequiredFeatures = new Feature[] { Feature.RecordingSearchService },
           RequirementLevel = RequirementLevel.Must,
           FunctionalityUnderTest = new Functionality[] { Functionality.FindRecordings })]
        public void FindRecordingWithMFilterTestVideoAndMetadata()
        {
            FindRecordingWithFilterTest("boolean(//Track[TrackType = \"Video\"]) and boolean(//Track[TrackType = \"Metadata\"])",
                new[] { TrackType.Video, TrackType.Metadata });
        }

        private bool HasAllTrackTypes(Dictionary<TrackType, bool> hasTrackTypeDictionary)
        {
            bool hasAllTrackTypes = true;
            foreach (var hasTrackType in hasTrackTypeDictionary)
            {
                hasAllTrackTypes &= hasTrackType.Value;
            }
            return hasAllTrackTypes;
        }

        private void FindRecordingWithFilterTest(string recordingInformationFilter,
            TrackType[] trackTypes)
        {
            int testKeepAlive = _searchKeepAlive;
            SearchState state = SearchState.Completed;
            string searchToken = string.Empty;
            RunTest(
                () =>
                {
                    string keepAlive = string.Format("PT{0}S", testKeepAlive);

                    searchToken = FindRecordings(
                        new SearchScope() { RecordingInformationFilter = recordingInformationFilter },
                        null,
                        keepAlive);
                    List<RecordingInformation> recordings = GetAllRecordingsSearchResults(searchToken, 1, null, "PT5S", out state);
                    //recordings = new List<RecordingInformation>();
                    StringBuilder logger = new StringBuilder();
                    bool ok = true;

                    bool emptyListAllowed = false;
                    if (trackTypes.Contains(TrackType.Audio) && !Features.ContainsFeature(Feature.AudioRecording))
                    {
                        emptyListAllowed = true;
                    }
                    if (trackTypes.Contains(TrackType.Metadata) && !Features.ContainsFeature(Feature.MetadataRecording))
                    {
                        emptyListAllowed = true;
                    }

                    if (recordings.Count == 0 && emptyListAllowed)
                    {
                        BeginStep("Check that recording list is not empty");
                        LogStepEvent("Recording list is empty");
                        StepPassed();
                    }
                    else
                    {
                        ValidateSearchResult(recordings);

                        foreach (RecordingInformation recording in recordings)
                        {
                            bool localOk = true;
                            if (recording.Track != null)
                            {
                                bool found = false;
                                var hasTrackTypeDictionary = trackTypes.ToDictionary(trackType => trackType, value => false);
                                var trackTokenList = new List<string>();
                                foreach (TrackInformation info in recording.Track)
                                {
                                    if (trackTokenList.FirstOrDefault(t => t == info.TrackToken) == null)
                                    {
                                        var trackTokenSameCount =
                                            recording.Track.Where(t => t.TrackToken == info.TrackToken).Count();
                                        if (trackTokenSameCount > 1)
                                        {
                                            trackTokenList.Add(info.TrackToken);
                                            ok = false;
                                            logger.AppendFormat("Recording '{0}' has {1} tracks with trackToken '{2}'{3}",
                                                                recording.RecordingToken, trackTokenSameCount,
                                                                info.TrackToken, Environment.NewLine);
                                        }
                                    }
                                    foreach (var trackType in trackTypes)
                                    {
                                        if (info.TrackType == trackType)
                                        {
                                            hasTrackTypeDictionary[trackType] = true;
                                        }
                                    }

                                    if (HasAllTrackTypes(hasTrackTypeDictionary))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    localOk = false;
                                }
                            }
                            else
                            {
                                localOk = false;
                            }
                            if (!localOk)
                            {
                                logger.AppendFormat("Recording {0} does not contain tracks of type", recording.RecordingToken);
                                foreach (var trackType in trackTypes)
                                {
                                    logger.AppendFormat(" {0} and", trackType);
                                }
                                logger.Remove(logger.Length - 4, 4);
                                logger.AppendFormat("{0}", Environment.NewLine);
                                ok = false;
                            }
                        }

                        StringBuilder strBuilder = new StringBuilder("Check that all recordings have");
                        foreach (var trackType in trackTypes)
                        {
                            strBuilder.AppendFormat(" {0} and", trackType);
                        }
                        strBuilder.Remove(strBuilder.Length - 4, 4);
                        strBuilder.Append(" tracks");
                        Assert(ok, logger.ToStringTrimNewLine(), strBuilder.ToString());
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
    }
}
