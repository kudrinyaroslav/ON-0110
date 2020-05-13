///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System;
using System.Xml;
using System.Linq;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.TestCases.Utils.Events;
using DateTime = System.DateTime;

namespace TestTool.Tests.TestCases.Base
{
    /// <summary>
    /// Wrapper for SearchPortClient to be used as base class for Search tests
    /// </summary>
    public class SearchTest : BaseServiceTest<SearchPort, SearchPortClient>
    {
        /// <summary>
        /// Recording for test
        /// </summary>
        protected string _recordingToken;
        /// <summary>
        /// Search timeout
        /// </summary>
        protected int _searchTimeout;

        protected string _metadataFilter;

        protected string _metadataRecordingToken;

        public SearchTest(TestLaunchParam param)
            : base(param)
        {
            _recordingToken = param.RecordingToken;
            _searchTimeout = param.SearchTimeout;
            _metadataFilter = param.MetadataFilter;
            _metadataRecordingToken = param.RecordingToken;
        }

        protected override SearchPortClient CreateClient()
        {
            string address = GetSearchServiceAddress();

            BeginStep("Connect to Search service");
            LogStepEvent(string.Format("Search service address: {0}", address));
            if (string.IsNullOrEmpty(address))
            {
                throw new AssertException("Search service not supported");
            }
            else
            {
                if (!address.IsValidUrl())
                {
                    throw new AssertException("Search service address is invalid");
                }
            }
            Binding binding = CreateBinding(false,
                    new IChannelController[] { new SoapValidator(SearchSchemasSet.GetInstance()) });
            SearchPortClient client = new SearchPortClient(binding, new EndpointAddress(address));
            StepPassed();
            return client;
        }

        /// <summary>
        /// Returns DUT's search service address
        /// </summary>
        /// <returns>Search service url</returns>
        protected string GetSearchServiceAddress()
        {
            string address = string.Empty;
            RunStep(() =>
            {
                DeviceClient device = DeviceClient;
                address = device.GetSearchServiceAddress(Features);

                if (string.IsNullOrEmpty(address))
                {
                    throw new AssertException("The DUT did not return Search service address");
                }

            }, "Get Search Service address");
            DoRequestDelay();
            return address;
        }

        private string _recordingServiceAddress;
        private RecordingPortClient _recordingClient;

        protected RecordingPortClient RecordingClient
        {
            get
            {
                if (_recordingClient == null)
                {
                    bool found = false;
                    if (string.IsNullOrEmpty(_recordingServiceAddress))
                    {
                        RunStep(() =>
                        {
                            Proxies.Onvif.DeviceClient deviceClient = DeviceClient;
                            string address = deviceClient.GetRecordingsServiceAddress(Features);
                            if (string.IsNullOrEmpty(address))
                            {
                                throw new AssertException("Recording service not found");
                            }
                            else
                            {
                                _recordingServiceAddress = address;
                                found = true;
                                LogStepEvent(_recordingServiceAddress);
                            }
                        }, "Get Recording service address", OnvifFaults.NoSuchService, true, true);
                        DoRequestDelay();
                    }

                    Assert(found, "Recording service address not found", "Check that the DUT returned Recording service address");

                    if (found)
                    {
                        EndpointController controller = new EndpointController(new EndpointAddress(_recordingServiceAddress));

                        Binding binding = CreateBinding(
                            false,
                            new IChannelController[] { new SoapValidator(RecordingSchemasSet.GetInstance()), controller });

                        _recordingClient = new RecordingPortClient(binding, new EndpointAddress(_recordingServiceAddress));

                        AttachSecurity(_recordingClient.Endpoint);
                        SetupChannel(_recordingClient.InnerChannel);
                    }
                }
                return _recordingClient;
            }
        }

        private DeviceClient _deviceClient;

        protected DeviceClient DeviceClient
        {
            get
            {
                if (_deviceClient == null)
                {
                    Binding binding =
                        CreateBinding(false,
                        new IChannelController[] { new SoapValidator(DeviceManagementSchemasSet.GetInstance()) });

                    _deviceClient =
                        new DeviceClient(binding, new EndpointAddress(_cameraAddress));

                    AttachSecurity(_deviceClient.Endpoint);
                    SetupChannel(_deviceClient.InnerChannel);

                }

                return _deviceClient;
            }
        }

        protected override void Release()
        {
            if (_deviceClient != null)
            {
                _deviceClient.Close();
            }
            if (_recordingClient != null)
            {
                _recordingClient.Close();
            }
            base.Release();
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories)
        {
            return CommonMethodsProvider.GetCapabilities(this, DeviceClient, categories);
        }

        protected FindRecordingResultList GetRecordingSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime)
        {
            return GetRecordingSearchResults(searchToken, minResults, maxResults, waitTime, "Get Recordings Search results");
        }

        protected FindRecordingResultList GetRecordingSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime, string stepName)
        {
            FindRecordingResultList response = null;

            GetRecordingSearchResults request = new GetRecordingSearchResults();
            request.SearchToken = searchToken;
            request.WaitTime = waitTime;
            request.MaxResultsSpecified = maxResults.HasValue;
            request.MaxResults = maxResults.GetValueOrDefault();
            request.MinResultsSpecified = minResults.HasValue;
            request.MinResults = minResults.GetValueOrDefault();
            RunStep(() => { response = Client.GetRecordingSearchResults(request).ResultList; }, stepName);
            DoRequestDelay();
            return response;
        }

        protected List<RecordingInformation> GetAllRecordingsSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime, out SearchState state)
        {
            DateTime requestSent = DateTime.MinValue;
            DateTime responseReceived = DateTime.MinValue;

            Action<string> logRequest = new Action<string>((s) => { requestSent = DateTime.Now; });
            Action<string> logResponse = new Action<string>((s) => { responseReceived = DateTime.Now; });

            _trafficListener.RequestSent += logRequest;
            _trafficListener.ResponseReceived += logResponse;

            TimeSpan ts = new TimeSpan(0);
            if (!string.IsNullOrEmpty(waitTime))
            {
                ts = XmlConvert.ToTimeSpan(waitTime);
                ts = ts.Add(new TimeSpan(0, 0, 1));
            }

            try
            {
                List<RecordingInformation> recordingsList = new List<RecordingInformation>();

                FindRecordingResultList response = null;
                GetRecordingSearchResults request = new GetRecordingSearchResults();
                request.SearchToken = searchToken;
                request.WaitTime = waitTime;
                request.MaxResultsSpecified = maxResults.HasValue;
                request.MaxResults = maxResults.GetValueOrDefault();
                request.MinResultsSpecified = minResults.HasValue;
                request.MinResults = minResults.GetValueOrDefault();

                DateTime started = DateTime.Now;
                DateTime dueTo = started.AddSeconds(_searchTimeout);
                DateTime lastResponse = DateTime.Now;

                bool completed = true;

                LogTestEvent(string.Format("All results should be received by {0}{1}",
                    dueTo.StdTimeToString(), Environment.NewLine));

                do
                {
                    RunStep(
                        () =>
                        {
                            response = Client.GetRecordingSearchResults(request).ResultList;
                        }, "Get Recording Search results");
                    lastResponse = DateTime.Now;
                    // no request delay here! 
                    if (response.RecordingInformation != null)
                    {
                        recordingsList.AddRange(response.RecordingInformation);
                    }

                    if (lastResponse > dueTo)
                    {
                        completed = false;
                        break;
                    }

                    if (maxResults.HasValue)
                    {
                        int count = 0;
                        if (response.RecordingInformation != null)
                        {
                            count = response.RecordingInformation.Length;
                        }
                        state = response.SearchState;
                        Assert(count <= maxResults.Value, string.Format("Number of recordings received ({0}) is more than maxResults ({1})", count, maxResults.Value), "Check that maxResults parameter is not exceeded");
                    }

                    if (!string.IsNullOrEmpty(waitTime))
                    {
                        TimeSpan duration = responseReceived - requestSent;
                        Assert(duration < ts,
                            string.Format("Response received {0}.{1} seconds after request is sent (waitTime is {2})", duration.Seconds, duration.Milliseconds.ToString("000"), waitTime),
                            "Check that waitTime is taken into account");
                    }

                } while (response.SearchState != SearchState.Completed);

                state = response.SearchState;

                Assert(completed, string.Format("Completed state has not been achieved (last response received at {0}, State: {1})", lastResponse.StdTimeToString(), response.SearchState), "Check that search has been completed in due time");

                return recordingsList;

            }
            finally
            {
                _trafficListener.RequestSent -= logRequest;
                _trafficListener.ResponseReceived -= logResponse;
            }
        }

        protected string FindRecordings(SearchScope scope, int? maxMatches, string keepAliveTime)
        {
            return FindRecordings(scope, maxMatches, keepAliveTime, "Find Recordings");
        }

        protected string FindRecordings(SearchScope scope, int? maxMatches, string keepAliveTime, string stepName)
        {
            string response = null;
            FindRecordings request = new FindRecordings();
            request.KeepAliveTime = keepAliveTime;
            request.MaxMatchesSpecified = maxMatches.HasValue;
            request.MaxMatches = maxMatches.GetValueOrDefault();
            request.Scope = scope;
            RunStep(() => { response = Client.FindRecordings(request).SearchToken; }, stepName);
            DoRequestDelay();
            return response;
        }

        protected void EndSearch(string searchToken)
        {
            EndSearch(searchToken, string.Format("End search [token={0}]", searchToken));
        }

        protected void EndSearch(string searchToken, string stepName)
        {
            RunStep(() => { Client.EndSearch(searchToken); }, stepName);
        }

        
        protected string FindEvents(SearchScope scope, EventFilter searchFilter,
            DateTime startPoint, DateTime? endPoint, bool includeStartState,
            int? maxMatches, string keepAliveTime)
        {
            return FindEvents(scope, searchFilter, startPoint, endPoint, includeStartState, maxMatches, keepAliveTime, "Find Events");
        }

        protected string FindEvents(SearchScope scope, EventFilter searchFilter,
            DateTime startPoint, DateTime? endPoint, bool includeStartState,
            int? maxMatches, string keepAliveTime, string stepName)
        {

            string response = null;
            FindEvents request = new FindEvents();
            request.Scope = scope;
            request.SearchFilter = searchFilter;
            request.StartPoint = startPoint;
            request.EndPointSpecified = endPoint.HasValue;
            request.EndPoint = endPoint.GetValueOrDefault();
            request.IncludeStartState = includeStartState;
            request.KeepAliveTime = keepAliveTime;
            request.MaxMatchesSpecified = maxMatches.HasValue;
            request.MaxMatches = maxMatches.GetValueOrDefault();
            RunStep(() => { response = Client.FindEvents(request).SearchToken; }, stepName);
            DoRequestDelay();
            return response;
        }

        protected FindEventResultList GetEventSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime)
        {
            return GetEventSearchResults(searchToken, minResults, maxResults,
                                         string.Format("Get events search result with token({0})", searchToken));
        }

        protected FindEventResultList GetEventSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime, string stepName)
        {
            GetEventSearchResultsResponse response = null;

            GetEventSearchResults request = new GetEventSearchResults();
            request.SearchToken = searchToken;
            request.WaitTime = waitTime;
            request.MaxResultsSpecified = maxResults.HasValue;
            request.MaxResults = maxResults.GetValueOrDefault();
            request.MinResultsSpecified = minResults.HasValue;
            request.MinResults = minResults.GetValueOrDefault();

            RunStep(
                () =>
                {
                    response = Client.GetEventSearchResults(request);
                }, stepName);

            return response.ResultList;
        }

        protected List<FindEventResult> GetAllEventsSearchResults(
            string searchToken,
            int? minResults,
            int? maxResults,
            string waitTime, out SearchState state)
        {
            return GetAllEventsSearchResults(searchToken, minResults, maxResults, waitTime, null, out state);
        }

        protected List<FindEventResult> GetAllEventsSearchResults(string searchToken,
                                                                  int? minResults,
                                                                  int? maxResults,
                                                                  string waitTime,
                                                                  Dictionary<FindEventResult, XmlElement> rawResults,
                                                                  out SearchState state)
        {

            List<FindEventResult> eventsList = new List<FindEventResult>();

            FindEventResultList response = null;
            GetEventSearchResults request = new GetEventSearchResults();
            request.SearchToken = searchToken;
            request.WaitTime = waitTime;
            request.MaxResultsSpecified = maxResults.HasValue;
            request.MaxResults = maxResults.GetValueOrDefault();
            request.MinResultsSpecified = minResults.HasValue;
            request.MinResults = minResults.GetValueOrDefault();

            string dump = string.Empty;
            Action<string> logAction = new Action<string>(str => { dump = str; });
            _trafficListener.ResponseReceived += logAction;

            DateTime started = DateTime.Now;
            DateTime dueTo = started.AddSeconds(_searchTimeout);
            bool completed = true;
            DateTime lastResponse = DateTime.Now;

            LogTestEvent(string.Format("All results should be received by {0}{1}", dueTo.StdTimeToString(), Environment.NewLine));

            do
            {
                RunStep(() => { response = Client.GetEventSearchResults(request).ResultList; }, "Get Events Search results");
                lastResponse = DateTime.Now;

                var events = response.Result ?? new FindEventResult[0];

                // no request delay here! 
                var onvifEvents = events.Where(OnvifMessage.IsOnvifMessage);

                if (events.Count() != onvifEvents.Count())
                    LogStepEvent("WARNING: there is a message from non-ONVIF namespace");

                //if (null != rawResults)
                {
                    var rdr = new System.IO.StringReader(dump);
                    string nextLine;
                    do
                    {
                        nextLine = rdr.ReadLine();
                    } while (!string.IsNullOrEmpty(nextLine));
                    string rawSoapPacket = rdr.ReadToEnd();
                    rawSoapPacket = rawSoapPacket.Replace("\r\n", "");
                    var doc = new XmlDocument();
                    doc.LoadXml(rawSoapPacket);

                    var messagePath = "/s:Envelope/s:Body/search:GetEventSearchResultsResponse/search:ResultList/onvif:Result/onvif:Event";
                    var manager = new XmlNamespaceManager(doc.NameTable);
                    manager.AddNamespace("s", "http://www.w3.org/2003/05/soap-envelope");
                    manager.AddNamespace("search", "http://www.onvif.org/ver10/search/wsdl");
                    manager.AddNamespace("onvif", "http://www.onvif.org/ver10/schema");
                    manager.AddNamespace("b2", "http://docs.oasis-open.org/wsn/b-2");

                    XmlNodeList responseNodeList = doc.SelectNodes(messagePath, manager);

                    for (int i = 0; i < events.Count(); i++)
                    {
                        var e = events[i];
                        if (OnvifMessage.IsOnvifMessage(e))
                        {
                            eventsList.Add(e);
                            if (null != rawResults) rawResults.Add(e, responseNodeList[i] as XmlElement);
                        }
                    }
                }

                if (lastResponse > dueTo)
                {
                    completed = false;
                    break;
                }

            } while (response.SearchState != SearchState.Completed);

            state = response.SearchState;
            _trafficListener.ResponseReceived -= logAction;

            Assert(completed, string.Format("Completed state has not been achieved (last response received at {0}, State: {1})", lastResponse.StdTimeToString(), response.SearchState), "Check that search has been completed in due time");

            return eventsList;

        }


        protected RecordingSummary GetRecordingSummary()
        {
            return GetRecordingSummary("Get Recording Summary");
        }

        protected RecordingSummary GetRecordingSummary(string stepName)
        {
            RecordingSummary response = null;
            RunStep(() => { response = Client.GetRecordingSummary(); }, stepName);
            return response;
        }

        /// <summary>
        /// Releases search (via unsubscribe or via timeout)
        /// </summary>
        protected void ReleaseSearch(string searchToken, int timeout)
        {
            BeginStep("Stop search");
            bool stopByRequest = false;
            try
            {
                if (Client.InnerChannel.State == CommunicationState.Faulted)
                {
                    LogStepEvent("Connection is faulted");
                }
                else
                {
                    LogStepEvent("Send EndSearch request");
                    Client.EndSearch(searchToken);
                    stopByRequest = true;
                }
            }
            catch (FaultException exc)
            {
                LogFault(exc);
                LogStepEvent("Failed to end search through request.");
            }
            catch (System.Net.Sockets.SocketException exc)
            {
                LogStepEvent(string.Format("Failed to end search through request. Error received: {0}", exc.Message));
            }
            catch (Exception exc)
            {
                LogStepEvent(string.Format("Failed to end search through request. Error received: {0}", exc.Message));
            }

            if (!stopByRequest)
            {
                LogStepEvent("Wait until search is ended by timeout");
                Sleep(timeout);
            }
            StepPassed();
        }

        protected RecordingInformation GetRecordingInformation(string token)
        {
            RecordingInformation response = null;
            RunStep(() => { response = Client.GetRecordingInformation(token); },
                "Get Recording Information");
            return response;
        }

        protected FindMetadataResponse FindMetadata(DateTime start, DateTime end,
            string recordingToken, int? maxMatches)
        {
            var findMetaData = new FindMetadata()
            {
                StartPoint = start,
                EndPoint = end,
                EndPointSpecified = true,
                Scope = new SearchScope() { IncludedRecordings = new[] { recordingToken } },
                MetadataFilter = new MetadataFilter { MetadataStreamFilter = _metadataFilter },
                KeepAliveTime = "PT10S",
                MaxMatches = maxMatches ?? 0,
                MaxMatchesSpecified = maxMatches == null ? false : true
            };
            
            FindMetadataResponse response = null;
            RunStep(() => { response = Client.FindMetadata(findMetaData); },
                string.Format("Find Metadata of recording {0}", recordingToken));
            return response;
        }

        protected FindMetadataResponse FindMetadataInvalidFilter(DateTime start, DateTime end,
            string recordingToken, int? maxMatches)
        {
            var findMetaData = new FindMetadata()
            {
                StartPoint = start,
                EndPoint = end,
                EndPointSpecified = true,
                Scope = new SearchScope() { IncludedRecordings = new[] { recordingToken } },
                MetadataFilter = new MetadataFilter
                {
                    MetadataStreamFilter = string.Format("boolean(//Track[TrackToken = \"{0}\"])", Guid.NewGuid().ToString().Substring(0, 8))

                },
                KeepAliveTime = "PT10S",
                MaxMatches = maxMatches ?? 0,
                MaxMatchesSpecified = maxMatches == null ? false : true
            };

            FindMetadataResponse response = null;
            RunStep(() => { response = Client.FindMetadata(findMetaData); },
                string.Format("Find Metadata of recording {0}", recordingToken));
            return response;
        }

        protected List<FindMetadataResult> GetAllMetadataSearchResults(
                                                        string searchToken,
                                                        int? minResults,
                                                        int? maxResults,
                                                        string waitTime,
                                                        out SearchState state)
        {

            List<FindMetadataResult> metadataList = new List<FindMetadataResult>();

            FindMetadataResultList response = null;

            DateTime started = DateTime.Now;
            DateTime dueTo = started.AddSeconds(_searchTimeout);
            bool completed = true;
            DateTime lastResponse = DateTime.Now;

            LogTestEvent(string.Format("All results should be received by {0}{1}",
                dueTo.StdTimeToString(), Environment.NewLine));

            do
            {
                RunStep(() =>
                {
                    response = Client.GetMetadataSearchResults(new GetMetadataSearchResults()
                    {
                        SearchToken = searchToken,
                        WaitTime = waitTime,
                        MinResults = minResults ?? 0,
                        MaxResults = maxResults ?? 0,
                        MinResultsSpecified = minResults == null ? false : true,
                        MaxResultsSpecified = maxResults == null ? false : true
                    }).ResultList;
                }, "Get Metadata Search results");
                lastResponse = DateTime.Now;

                if (response.Result != null)
                {
                    metadataList.AddRange(response.Result);
                }
                if (lastResponse > dueTo)
                {
                    completed = false;
                    break;
                }

            } while (response.SearchState != SearchState.Completed);

            state = response.SearchState;

            Assert(completed,
                string.Format("Completed state has not been achieved (last response received at {0}, State: {1})", lastResponse.StdTimeToString(), response.SearchState),
                "Check that search has been completed in due time");

            return metadataList;

        }

        protected string FindPTZPosition(DateTime start, DateTime? end, SearchScope scope,
            PTZPositionFilter filter, int? maxMatxhes, string keepAliveTime)
        {
            return FindPTZPosition(start, end, scope, filter, maxMatxhes, keepAliveTime, "Find PTZPostion");
        }
        protected string FindPTZPosition(DateTime start, DateTime? end, SearchScope scope, PTZPositionFilter filter, int? maxMathes, string keepAliveTime, string stepName)
        {
            string token = string.Empty;

            RunStep(() => { token = Client.FindPTZPosition(start, end, scope, filter, maxMathes, keepAliveTime).SearchToken; }, stepName);
            DoRequestDelay();
            return token;
        }

        protected FindPTZPositionResultList GetPTZPositionSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime)
        {
            return GetPTZPositionSearchResults(searchToken, minResults, maxResults, waitTime, string.Format("Get PTZ Position Search Results with token({0})", searchToken));
        }
        protected FindPTZPositionResultList GetPTZPositionSearchResults(string searchToken, int? minResults, int? maxResults, string waitTime, string stepName)
        {
            FindPTZPositionResultList response = null;

            RunStep(() => { response = Client.GetPTZPositionSearchResults(searchToken, minResults, maxResults, waitTime).ResultList; }, stepName);

            return response;
        }

        protected void ValidatePTZPositionSearchResults(PTZPositionFilter filter, IEnumerable<FindPTZPositionResult> results)
        {
            Func<FindPTZPositionResult, bool> outOfRequestedRangePredicate =
                (e) =>
                {
                    if (null == e.Position || null == e.Position.PanTilt)
                        return false;

                    return filter.MinPosition.PanTilt.x > e.Position.PanTilt.x || e.Position.PanTilt.x > filter.MaxPosition.PanTilt.x
                           || filter.MinPosition.PanTilt.y > e.Position.PanTilt.y || e.Position.PanTilt.y > filter.MaxPosition.PanTilt.y;
                };

            if (null != results && results.Any(outOfRequestedRangePredicate))
            {
                LogStepEvent("WARNING: The GetPTZPositionSearchResultsResponse contains responses with Position.PanTilt out of requested range");
                LogStepEvent("");
            }            
        }

        protected List<FindPTZPositionResult> GetAllPtzSearchResults(string searchToken,
                                                                     PTZPositionFilter filter,
                                                                     int? minResults,
                                                                     int? maxResults,
                                                                     string waitTime,
                                                                     out SearchState state)
        {

            List<FindPTZPositionResult> ptsPositionsList = new List<FindPTZPositionResult>();

            FindPTZPositionResultList response = null;

            DateTime started = DateTime.Now;
            DateTime dueTo = started.AddSeconds(_searchTimeout);
            bool completed = true;
            DateTime lastResponse = DateTime.Now;

            LogTestEvent(string.Format("All results should be received by {0}{1}", dueTo.StdTimeToString(), Environment.NewLine));

            do
            {
                response = GetPTZPositionSearchResults(searchToken, minResults, maxResults, waitTime, "Get PTZ Search results");
                lastResponse = DateTime.Now;

                // no request delay here! 
                if (response.Result != null)
                {
                    ptsPositionsList.AddRange(response.Result);
                }
                if (lastResponse > dueTo)
                {
                    completed = false;
                    break;
                }

            } while (response.SearchState != SearchState.Completed);

            ValidatePTZPositionSearchResults(filter, ptsPositionsList);

            state = response.SearchState;

            Assert(completed,
                   string.Format("Completed state has not been achieved (last response received at {0}, State: {1})", lastResponse.StdTimeToString(), response.SearchState),
                   "Check that search has been completed in due time");

            return ptsPositionsList;

        }
                
        protected class SearchRange
        {
            public SearchRange()
            {

            }

            public SearchRange(DateTime start, DateTime end)
            {
                Start = start;
                End = end;
            }

            public SearchRange(DateTime start, DateTime end, DateTime recordingStart, DateTime recordingEnd)
            {
                Start = start;
                End = end;
                Earliest = recordingStart;
                Latest = recordingEnd;
            }

            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public DateTime Earliest { get; set; }
            public DateTime Latest { get; set; }
        }

        protected SearchRange DefineSearchRange(RecordingInformation recording)
        {
            DateTime start;
            DateTime end;

            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;

            foreach (TrackInformation trackInformation in recording.Track)
            {
                if (trackInformation.DataFrom < minDate)
                {
                    minDate = trackInformation.DataFrom;
                }
                if (trackInformation.DataTo > maxDate)
                {
                    maxDate = trackInformation.DataTo;
                }
            }

            start = recording.EarliestRecordingSpecified ? recording.EarliestRecording : minDate;
            end = recording.LatestRecordingSpecified ? recording.LatestRecording : maxDate;

            return new SearchRange(start, end, start, end);
        }

        protected SearchRange DefineSearchRangeInside(RecordingInformation recording)
        {
            SearchRange range = DefineSearchRange(recording);

            TimeSpan timeSpan = (range.End - range.Start);
            double delta = (int)timeSpan.TotalMilliseconds / 10;

            range.Start = range.Start.AddMilliseconds(delta);
            range.End = range.End.AddMilliseconds(-delta);

            range.Earliest = range.Start;
            range.Latest = range.End;
            return range;
        }

        protected SearchRange DefineSearchRangeOutside(RecordingInformation recording)
        {
            SearchRange range = DefineSearchRange(recording);

            TimeSpan timeSpan = (range.End - range.Start);
            int delta = 1 + (int)timeSpan.TotalSeconds / 10;

            range.Start = range.Start.AddSeconds(-delta);
            range.End = range.End.AddSeconds(delta);

            return range;
        }
        
        protected GetRecordingsResponseItem[] GetRecordings()
        {
            RecordingPortClient recordingPortClient = RecordingClient;
            return CommonMethodsProvider.GetRecordings(this, recordingPortClient);
        }

        protected RecordingInformation FindRecordingForTest()
        {
            RecordingInformation recording = null;
            RunStep(() => { recording = Client.GetRecordingInformation(_recordingToken); }, "Get Recording Information");
            DoRequestDelay();

            Assert(recording != null, "Recording not found", "Check that recording for test found");

            Assert(recording.Track != null && recording.Track.Length > 0, "Recording has no tracks", "Check that recording has tracks");

            return recording;
        }
        
        protected void ValidateOrder<T>(IList<T> results,
            Func<T, System.DateTime> timeSelector,
            System.DateTime start, System.DateTime end)
        {
            bool ok;
            bool ascending = start < end;
            ok = true;
            for (int i = 1; i < results.Count; i++)
            {
                if (timeSelector(results[i - 1]) == timeSelector(results[i]))
                {
                    continue;
                }
                if (timeSelector(results[i - 1]) <= timeSelector(results[i]) != ascending)
                {
                    ok = false;
                }
            }

            Assert(ok, "Results order is broken", string.Format("Check that results are ordered {0}", ascending ? "ascending" : "descending"));
        }

        protected MediaAttributes[] GetMediaAttributes(string token, System.DateTime T)
        {
            MediaAttributes[] r = null;
            RunStep(() => r = Client.GetMediaAttributes(new[] { token }, T),
                    string.Format("Get Media Attributes with RecordingTokens = '{0}' and Time = '{1}'", token, T));

            return r ?? new MediaAttributes[0];
        }

    }
}
