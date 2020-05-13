///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System;
using System.Xml;
using TestTool.HttpTransport.Interfaces;
using TestTool.Tests.CommonUtils.SoapValidation;
using System.ServiceModel;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Engine.Base.TestBase;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Definitions.Onvif;
using DateTime=System.DateTime;

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


        public SearchTest(TestLaunchParam param)
            : base(param)
        {
            _recordingToken = param.RecordingToken;
            _searchTimeout = param.SearchTimeout;
        }
        
        protected override SearchPortClient CreateClient()
        {
            string address = GetSearchServiceAddress();

            BeginStep("Connect to Search service");
            LogStepEvent(string.Format("Search service address: {0}", address));
            if (!address.IsValidUrl())
            {
                throw new AssertException("Search service address is invalid");
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

        protected Capabilities GetCapabilities(CapabilityCategory[] categories, string stepName)
        {
            Capabilities capabilities = null;
            RunStep(() => { capabilities = DeviceClient.GetCapabilities(categories); }, stepName);
            DoRequestDelay();
            return capabilities;
        }

        protected Capabilities GetCapabilities(CapabilityCategory[] categories)
        {
            return GetCapabilities(categories, "Get capabilities");
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

            Action<string> logRequest = new Action<string>((s) => { requestSent = DateTime.Now;} );
            Action<string> logResponse =  new Action<string>((s) => { responseReceived = DateTime.Now; });

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
                            "Check that waitTime account is taken into account");
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
            RunStep(() => { response = Client.FindRecordings(request).SearchToken;}, stepName);
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

        protected GetRecordingsResponseItem[] GetRecordings()
        {
            return GetRecordings("Get recordings");
        }

        protected GetRecordingsResponseItem[] GetRecordings(string stepName)
        {
            GetRecordingsResponseItem[] recordingsResponseItems = null;
            RecordingPortClient recordingPortClient = RecordingClient;
            RunStep(
                () => { recordingsResponseItems = recordingPortClient.GetRecordings(new GetRecordings()); }, 
                stepName);

            return recordingsResponseItems;
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

        protected List<FindEventResult> GetAllEventsSearchResults(
            string searchToken, 
            int? minResults, 
            int? maxResults, 
            string waitTime,
            Dictionary<FindEventResult, XmlDocument> rawResults, 
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
            Action<string> logAction = new Action<string>( str => {dump=str;});
            _trafficListener.ResponseReceived += logAction;
            
            DateTime started = DateTime.Now;
            DateTime dueTo = started.AddSeconds(_searchTimeout);
            bool completed = true;
            DateTime lastResponse = DateTime.Now;

            LogTestEvent(string.Format("All results should be received by {0}{1}",
                dueTo.StdTimeToString(), Environment.NewLine));

            do
            {
                RunStep(() => { response = Client.GetEventSearchResults(request).ResultList; }, "Get Events Search results");
                lastResponse = DateTime.Now;

                // no request delay here! 
                if (response.Result != null)
                {
                    eventsList.AddRange(response.Result);

                    if (rawResults != null)
                    {
                        System.IO.StringReader rdr = new System.IO.StringReader(dump);
                        string nextLine;
                        do
                        {
                            nextLine = rdr.ReadLine();
                        } while (!string.IsNullOrEmpty(nextLine));
                        string rawSoapPacket = rdr.ReadToEnd();
                        rawSoapPacket = rawSoapPacket.Replace("\r\n", "");
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(rawSoapPacket);
                        foreach (FindEventResult result in response.Result)
                        {
                            rawResults.Add(result, doc);
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


    }
}
