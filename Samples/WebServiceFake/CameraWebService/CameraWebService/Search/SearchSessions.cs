using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CameraWebService.Search
{
    public class SearchSession
    {
        public string Token { get; set; }
        public System.DateTime Started { get; set; }
        public int KeepAlive { get; set; }
        public System.DateTime LastRequest { get; set; }
        public object Data { get; set; }
        public int? MaxMatches { get; set; }
        public int ResultsSent { get; set; }
    }

    public class RecordingEntitiesSearchSession : SearchSession
    { 
        public string RecordingToken { get; set; }
    
    }

    public class RecordingSearchSession : SearchSession
    {
        public int MinResults { get; set; }
        public int MaxResults { get; set; }
    }

    public class EventsSearchSession : RecordingEntitiesSearchSession
    {
        public System.DateTime? StartPoint { get; set; }
        public System.DateTime? EndPoint { get; set; }
    }



    public class SearchSessionManager
    {
        private static SearchSessionManager _instance;
        public static SearchSessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SearchSessionManager();
                }
                return _instance;
            }
        }

        private int _lastId = 0;
        List<SearchSession> _sessions = new List<SearchSession>();

        public List<SearchSession> Sessions
        {
            get { return _sessions; }
        }

        public SearchSession GetSession(string token)
        {
            SearchSession session = Sessions.Where(S => S.Token == token).FirstOrDefault();
            if (session != null)
            {
                // expired
                if (session.LastRequest.AddSeconds(session.KeepAlive) < System.DateTime.Now)
                {
                    Sessions.Remove(session);
                    return null;
                }
                
                session.LastRequest = System.DateTime.Now;
            }
            return session;
        }

        public bool EndSearch(string token)
        {
            SearchSession session = GetSession(token);
            if (session != null)
            {
                Sessions.Remove(session);
            }
            return session != null;
        }

        public string GetNextToken()
        {
            _lastId++;
            return _lastId.ToString("000000");
        }


    }

}