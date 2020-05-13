using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace CameraWebService.Search
{
    /// <summary>
    /// Summary description for SearchService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SearchService : SearchBinding 
    {
        
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetServiceCapabilities", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Capabilities")]
        public override Capabilities GetServiceCapabilities()
        {
            return new Capabilities() {MetadataSearch = false, MetadataSearchSpecified = true};
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingSummary", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Summary")]
        public override RecordingSummary GetRecordingSummary()
        {
            RecordingSummary result = new RecordingSummary();
            result.NumberRecordings = 3;

            return result;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingInformation", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("RecordingInformation")]
        public override RecordingInformation GetRecordingInformation(string RecordingToken)
        {
            RecordingInformation info =
                SearchStorage.Instance.Recordings.Where(R => R.RecordingToken == RecordingToken).FirstOrDefault();
            return info;
        }

        public override MediaAttributes[] GetMediaAttributes(string[] RecordingTokens, System.DateTime Time)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindRecordings", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindRecordings(SearchScope Scope, int MaxMatches, bool MaxMatchesSpecified, string KeepAliveTime)
        {
            string token = SearchSessionManager.Instance.GetNextToken();

            int keepAliveTime = 0;
            string strKeepAlive = KeepAliveTime.Substring(2, KeepAliveTime.Length - 3);
            keepAliveTime = int.Parse(strKeepAlive);
            if (KeepAliveTime.EndsWith("M"))
            {
                keepAliveTime = keepAliveTime*60;
            }

            RecordingSearchSession session = new RecordingSearchSession()
                                        {
                                            KeepAlive = keepAliveTime, 
                                            Token = token, 
                                            Started = System.DateTime.Now,
                                            LastRequest = System.DateTime.Now
                                        };

            //SearchStorage.Instance.ClearRecordings();
            //SearchStorage.Instance.ChangeRecordingsGeneration();

            List<RecordingInformation> filtered;
            if (Scope != null)
            {
                string filter = Scope.RecordingInformationFilter;

                if (filter.Contains("Video"))
                {
                    filtered =
                        SearchStorage.Instance.Recordings.Where(
                            R => R.Track.Where(T => T.TrackType == Search.TrackType.Video).Count() > 0).ToList();

                }
                else if (filter.Contains("Audio"))
                {
                    filtered =
                        SearchStorage.Instance.Recordings.Where(
                            R => R.Track.Where(T => T.TrackType == Search.TrackType.Audio).Count() > 0).ToList();

                }
                else if (filter.Contains("Metadata"))
                {
                    filtered =
                        SearchStorage.Instance.Recordings.Where(
                            R => R.Track.Where(T => T.TrackType == Search.TrackType.Metadata).Count() > 0).ToList();

                }
                else
                {
                    filtered = SearchStorage.Instance.Recordings;
                }
            }
            else
            {
                filtered = SearchStorage.Instance.Recordings;
            }


            List<RecordingInformation> list;
            if (MaxMatchesSpecified)
            {
                list = new List<RecordingInformation>();
                for (int i = 0; i < Math.Min(MaxMatches, filtered.Count); i++)
                {
                    list.Add(SearchStorage.Instance.Recordings[i]);
                }
            }
            else
            {
                list = new List<RecordingInformation>(filtered);
            }
            session.Data = list;

            SearchSessionManager.Instance.Sessions.Add(session);
            return token;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetRecordingSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindRecordingResultList GetRecordingSearchResults(string SearchToken, 
            int MinResults,
            bool MinResultsSpecified, int MaxResults, bool MaxResultsSpecified, string WaitTime)
        {
            
            RecordingSearchSession session =
                SearchSessionManager.Instance.GetSession(SearchToken) as RecordingSearchSession;
            if (session != null)
            {
                IList<RecordingInformation> data = (IList<RecordingInformation>) session.Data;
                
                // all data received
                if (data.Count == 0)
                {
                    CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
                    return null;
                }

                FindRecordingResultList list = new FindRecordingResultList();

                Random rnd = new Random();
                int cnt = Math.Min(rnd.Next(1, 4), data.Count);

                rnd = new Random();
                int sleep = rnd.Next(1, 3);
                System.Threading.Thread.Sleep(400 * sleep + 450);

                list.RecordingInformation = new RecordingInformation[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    list.RecordingInformation[i] = data[0];
                    data.RemoveAt(0);
                }
                session.ResultsSent = session.ResultsSent + cnt;
                list.SearchState = data.Count > 0 ? SearchState.Searching : SearchState.Completed;

                if (data.Count == 0)
                {
                    SearchSessionManager.Instance.Sessions.Remove(session);
                }
                return list;
            }
            else
            {
                CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
                return null;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindEvents", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindEvents(System.DateTime StartPoint, 
            System.DateTime EndPoint, 
            bool EndPointSpecified, 
            SearchScope Scope, 
            EventFilter SearchFilter, 
            bool IncludeStartState, int MaxMatches, bool MaxMatchesSpecified, string KeepAliveTime)
        {
            string token = SearchSessionManager.Instance.GetNextToken();

            int keepAliveTime = 0;
            string strKeepAlive = KeepAliveTime.Substring(2, KeepAliveTime.Length - 3);
            keepAliveTime = int.Parse(strKeepAlive);
            if (KeepAliveTime.EndsWith("M"))
            {
                keepAliveTime = keepAliveTime * 60;
            }

            EventsSearchSession session = new EventsSearchSession()
                                        {
                                            KeepAlive = keepAliveTime,
                                            Token = token,
                                            Started = System.DateTime.Now,
                                            LastRequest = System.DateTime.Now,
                                            StartPoint = StartPoint,
                                            EndPoint = EndPointSpecified ? new Nullable<System.DateTime>(EndPoint) : null

                                        };

            if (MaxMatchesSpecified)
            {
                session.MaxMatches = MaxMatches;
            }

            if (Scope != null && Scope.IncludedRecordings != null & Scope.IncludedRecordings.Length > 0)
            {
                session.RecordingToken = Scope.IncludedRecordings[0];
            }

            List<FindEventResult> resultsList= null;

            bool ascending = true;
            if (session.EndPoint.HasValue)
            {
                if (session.EndPoint.Value < session.StartPoint.Value)
                {
                    ascending = false;
                }
            }
            
            // to get different events from one search operation to another
            //SearchStorage.Instance.ClearEvents();
            //SearchStorage.Instance.ChangeEventsGeneration();

            IEnumerable<FindEventResult> filtered =
                SearchStorage.Instance.Events.Where(
                    E =>
                    E.RecordingToken == session.RecordingToken);
                    

            if (ascending)
            {
                resultsList =
                    filtered.Where(E => (E.Time >= session.StartPoint &&
                    E.Time <= session.EndPoint)).OrderBy(E => E.Time).ToList();
            }
            else
            {
                resultsList =
                    filtered.Where(E => (E.Time <= session.StartPoint &&
                    E.Time >= session.EndPoint)).OrderByDescending(E => E.Time).ToList();
            }

            if (session.MaxMatches.HasValue)
            {
                int cnt = Math.Min(session.MaxMatches.Value, resultsList.Count);
                List<FindEventResult> corrected = new List<FindEventResult>();
                for (int i = 0; i< cnt; i++)
                {
                    corrected.Add(resultsList[i]);
                }
                session.Data = corrected;
            }
            else
            {
                session.Data = resultsList;
            }

            SearchSessionManager.Instance.Sessions.Add(session);
            return token;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetEventSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindEventResultList GetEventSearchResults(string SearchToken, int MinResults, bool MinResultsSpecified, int MaxResults, bool MaxResultsSpecified, string WaitTime)
        {
            EventsSearchSession session = SearchSessionManager.Instance.GetSession(SearchToken) as EventsSearchSession;

            if (session != null)
            {

                List<FindEventResult> data = (List<FindEventResult>) session.Data;
               
                FindEventResultList list = new FindEventResultList();

                Random rnd= new Random();
                int cnt = Math.Min(rnd.Next(1,4), data.Count);
                
                rnd = new Random();
                int sleep = rnd.Next(1, 3);
                System.Threading.Thread.Sleep(400 * sleep + 450);

                list.Result = new FindEventResult[cnt];
                for (int i = 0; i< cnt; i++)
                {
                    list.Result[i] = data[0];
                    data.RemoveAt(0);
                }
                session.ResultsSent = session.ResultsSent + cnt;
                list.SearchState = data.Count > 0 ? SearchState.Searching : SearchState.Completed;
                
                if (data.Count == 0)
                {
                    SearchSessionManager.Instance.Sessions.Remove(session);
                }
                return list;
            }
            else
            {
                CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
                return null;
            }
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindPTZPosition", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindPTZPosition(System.DateTime StartPoint, System.DateTime EndPoint, bool EndPointSpecified, SearchScope Scope, PTZPositionFilter SearchFilter, int MaxMatches, bool MaxMatchesSpecified, string KeepAliveTime)
        {
            string token = SearchSessionManager.Instance.GetNextToken();

            int keepAliveTime = 0;
            string strKeepAlive = KeepAliveTime.Substring(2, KeepAliveTime.Length - 3);
            keepAliveTime = int.Parse(strKeepAlive);
            if (KeepAliveTime.EndsWith("M"))
            {
                keepAliveTime = keepAliveTime * 60;
            }

            SearchSession session = new SearchSession()
            {
                KeepAlive = keepAliveTime,
                Token = token,
                Started = System.DateTime.Now,
                LastRequest = System.DateTime.Now
            };

            bool ascending = true;
            if (EndPointSpecified)
            {
                if (EndPoint < StartPoint)
                {
                    ascending = false;
                }
            }

            List<FindPTZPositionResult> filtered = SearchStorage.Instance.PtzPositions;

            if (ascending)
            {
                filtered =
                    filtered.Where(E => (E.Time >= StartPoint &&
                    (E.Time <= EndPoint || !EndPointSpecified ))).OrderBy(E => E.Time).ToList();
            }
            else
            {
                filtered =
                    filtered.Where(E => (E.Time <= StartPoint &&
                    (E.Time >= EndPoint || !EndPointSpecified))).OrderByDescending(E => E.Time).ToList();
            }

            List<FindPTZPositionResult> list;
            if (MaxMatchesSpecified)
            {
                list = new List<FindPTZPositionResult>();
                for (int i = 0; i < Math.Min(MaxMatches, filtered.Count); i++)
                {
                    list.Add(filtered[i]);
                }
            }
            else
            {
                list = new List<FindPTZPositionResult>(filtered);
            }


            session.Data = list;

            SearchSessionManager.Instance.Sessions.Add(session);
            return token;
        }




        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetPTZPositionSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindPTZPositionResultList GetPTZPositionSearchResults(string SearchToken, int MinResults, bool MinResultsSpecified, int MaxResults, bool MaxResultsSpecified, string WaitTime)
        {
            SearchSession session = SearchSessionManager.Instance.GetSession(SearchToken);

            if (session != null)
            {

                List<FindPTZPositionResult> data = (List<FindPTZPositionResult>)session.Data;

                FindPTZPositionResultList list = new FindPTZPositionResultList();

                Random rnd = new Random();
                int cnt = Math.Min(rnd.Next(1, 4), data.Count);

                rnd = new Random();
                int sleep = rnd.Next(1, 3);
                System.Threading.Thread.Sleep(400 * sleep + 450);

                list.Result = new FindPTZPositionResult[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    list.Result[i] = data[0];
                    data.RemoveAt(0);
                }
                session.ResultsSent = session.ResultsSent + cnt;
                list.SearchState = data.Count > 0 ? SearchState.Searching : SearchState.Completed;

                if (data.Count == 0)
                {
                    SearchSessionManager.Instance.Sessions.Remove(session);
                }
                return list;
            }
            else
            {
                CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
                return null;
            }
        }

        public override SearchState GetSearchState(string SearchToken)
        {
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/EndSearch", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Endpoint")]
        public override System.DateTime EndSearch(string SearchToken)
        {
            bool ended = SearchSessionManager.Instance.EndSearch(SearchToken);
            if (!ended)
            {
                System.Diagnostics.Debug.WriteLine("Search not ended");
                CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
            }
            return System.DateTime.Now;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/FindMetadata", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SearchToken")]
        public override string FindMetadata(System.DateTime StartPoint, 
            System.DateTime EndPoint, 
            [System.Xml.Serialization.XmlIgnoreAttribute()] bool EndPointSpecified, 
            SearchScope Scope, 
            MetadataFilter MetadataFilter, 
            int MaxMatches, 
            [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxMatchesSpecified, 
            [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string KeepAliveTime)
        {
            string token = SearchSessionManager.Instance.GetNextToken();

            int keepAliveTime = 0;
            string strKeepAlive = KeepAliveTime.Substring(2, KeepAliveTime.Length - 3);
            keepAliveTime = int.Parse(strKeepAlive);
            if (KeepAliveTime.EndsWith("M"))
            {
                keepAliveTime = keepAliveTime * 60;
            }

            RecordingEntitiesSearchSession session = new RecordingEntitiesSearchSession()
            {
                KeepAlive = keepAliveTime,
                Token = token,
                Started = System.DateTime.Now,
                LastRequest = System.DateTime.Now,
            };

            if (MaxMatchesSpecified)
            {
                session.MaxMatches = MaxMatches;
            }

            if (Scope != null && Scope.IncludedRecordings != null & Scope.IncludedRecordings.Length > 0)
            {
                session.RecordingToken = Scope.IncludedRecordings[0];
            }

            List<FindMetadataResult> resultsList = null;

            bool ascending = true;
            if (EndPointSpecified)
            {
                if (EndPoint < StartPoint)
                {
                    ascending = false;
                }
            }

            // to get different results from one search operation to another
            //SearchStorage.Instance.ClearMetadata();
            //SearchStorage.Instance.ChangeMetadataGeneration();

            IEnumerable<FindMetadataResult> filtered =
                SearchStorage.Instance.Metadata.Where(
                    E =>
                    E.RecordingToken == session.RecordingToken || string.IsNullOrEmpty(session.RecordingToken));


            if (ascending)
            {
                resultsList =
                    filtered.Where(E => (E.Time >= StartPoint &&
                    (E.Time <= EndPoint || !EndPointSpecified ))).OrderBy(E => E.Time).ToList();
            }
            else
            {
                resultsList =
                    filtered.Where(E => (E.Time <= StartPoint &&
                    (E.Time >= EndPoint || !EndPointSpecified))).OrderByDescending(E => E.Time).ToList();
            }

            if (session.MaxMatches.HasValue)
            {
                int cnt = Math.Min(session.MaxMatches.Value, resultsList.Count);
                List<FindMetadataResult> corrected = new List<FindMetadataResult>();
                for (int i = 0; i < cnt; i++)
                {
                    corrected.Add(resultsList[i]);
                }
                session.Data = corrected;
            }
            else
            {
                session.Data = resultsList;
            }

            SearchSessionManager.Instance.Sessions.Add(session);
            return token;
        }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.onvif.org/ver10/search/wsdl/GetMetadataSearchResults", RequestNamespace = "http://www.onvif.org/ver10/search/wsdl", ResponseNamespace = "http://www.onvif.org/ver10/search/wsdl", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResultList")]
        public override FindMetadataResultList GetMetadataSearchResults(string SearchToken, int MinResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MinResultsSpecified, int MaxResults, [System.Xml.Serialization.XmlIgnoreAttribute()] bool MaxResultsSpecified, [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")] string WaitTime)
        {
            RecordingEntitiesSearchSession session = SearchSessionManager.Instance.GetSession(SearchToken) as RecordingEntitiesSearchSession;

            if (session != null)
            {

                List<FindMetadataResult> data = (List<FindMetadataResult>)session.Data;

                FindMetadataResultList list = new FindMetadataResultList();

                Random rnd = new Random();
                int cnt = Math.Min(rnd.Next(1, 4), data.Count);

                rnd = new Random();
                int sleep = rnd.Next(1, 3);
                System.Threading.Thread.Sleep(400 * sleep + 450);

                list.Result = new FindMetadataResult[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    list.Result[i] = data[0];
                    data.RemoveAt(0);
                }
                session.ResultsSent = session.ResultsSent + cnt;
                list.SearchState = data.Count > 0 ? SearchState.Searching : SearchState.Completed;

                if (data.Count == 0)
                {
                    SearchSessionManager.Instance.Sessions.Remove(session);
                }
                return list;
            }
            else
            {
                CommonUtils.ReturnFault(new string[] { "Sender", "InvalidArgVal", "InvalidToken" });
                return null;
            }
        }


    }
}
