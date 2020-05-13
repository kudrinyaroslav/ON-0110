/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Onvif
{
    public class RtspResponse
    {
        private RtspResponse()
        {

        }

        public class MediaDef
        {
            public MediaDef(string media, int port, string protocol, int payload)
            {
                Media = media;
                Port = port;
                Protocol = protocol;
                Payload = payload;
            }
            public string Media;
            public int Port;
            public string Protocol;
            public int Payload;
            public Hashtable Attributes = new Hashtable();
        }

        public class Sdp
        {
            public string Origin;
            public int Version;
            public string Subject;
            public string Key;
            public List<MediaDef> MediaDefs = new List<MediaDef>();
            public Hashtable Attributes = new Hashtable();
        }

        public int Status;
        public int CSeq;
        public string Session;
        public int SessionTimeout = 30;
        public string AuthorizationType;

        public Hashtable HeaderFields = new Hashtable();
        public string Content;
        public Hashtable TransportFields;
        public Sdp SdpFields;
        private MediaDef _currentMediaDef;


        public static RtspResponse Parse(string rawResponse)
        {
            RtspResponse response = new RtspResponse();

            if (rawResponse.Length == 0) return response;

            rawResponse = rawResponse.Replace("\r\n", "\n");

            string firstLine = rawResponse.Substring(0, rawResponse.IndexOf("\n"));

            string[] firstLineParts = firstLine.Split(' ');

            response.Status = Convert.ToInt32(firstLineParts[1]);

            rawResponse = rawResponse.Substring(firstLine.Length + 1);

            int posEndOfHeader = rawResponse.IndexOf("\n\n");

            if (posEndOfHeader == -1) return response;  //not valid

            string header = rawResponse.Substring(0, posEndOfHeader);

            response.Content = rawResponse.Substring(posEndOfHeader + 2);


            string[] responseLines = header.Split('\n');

            foreach (string responseLine in responseLines)
            {
                if (responseLine.Length < 1) continue;

                string[] headerLineParts = responseLine.Split(':');

                if (headerLineParts.Length < 2)
                {
                    continue;
                }

                switch (headerLineParts[0].ToUpper())
                {
                    case "CSEQ":
                        response.CSeq = Convert.ToInt32(headerLineParts[1]);
                        break;
                    case "AUTHORIZATION":
                        response.AuthorizationType= headerLineParts[1];
                        break;
                    case "TRANSPORT":
                        string[] transportFields = headerLineParts[1].Trim().Split(';');
                        if (transportFields.Length > 0) response.TransportFields = new Hashtable();
                        foreach (string p in transportFields)
                        {
                            string[] transportFieldParts = p.Split('=');
                            if (transportFieldParts.Length > 1)
                                response.TransportFields[transportFieldParts[0].ToUpper()] = transportFieldParts[1];
                            else
                                response.TransportFields[transportFieldParts[0].ToUpper()] = null;
                        }
                        break;
                    case "SESSION":
                        string[] sessionLineParts = headerLineParts[1].Trim().Split(';');
                        response.Session = sessionLineParts[0].Trim();
                        if (sessionLineParts.Length > 1 && sessionLineParts[1].ToUpper().Contains("TIMEOUT"))
                        {
                            response.SessionTimeout = Convert.ToInt32(sessionLineParts[1].Substring(sessionLineParts[1].IndexOf('=') + 1));
                        }
                        break;
                    default:
                        string key = headerLineParts[0].ToUpper();
                        if (!response.HeaderFields.ContainsKey(key))
                        {
                            response.HeaderFields[key] = headerLineParts[1].Trim();
                        }
                        break;
                }

            }

            if (response.HeaderFields.ContainsKey("CONTENT-TYPE") && response.HeaderFields["CONTENT-TYPE"].ToString() == "application/sdp")
            {
                response.SdpFields = new Sdp();

                //parse sdp
                string[] sdpFields = response.Content.Split('\n');
                foreach (string field in sdpFields)
                {
                    if (field.Length < 3) continue;
                    char code = field.ToUpper()[0];
                    string value = field.Substring(2);

                    switch (code)
                    {
                        case 'O':
                            response.SdpFields.Origin = value;
                            break;
                        case 'V':
                            response.SdpFields.Version = Convert.ToInt32(value);
                            break;
                        case 'S':
                            response.SdpFields.Subject = value;
                            break;
                        case 'K':
                            {
                                string[] sa = value.Split(':');
                                if (sa.Length == 2)
                                {
                                    switch (sa[0])
                                    {
                                        case "base64":
                                            byte[] b = Convert.FromBase64String(sa[1]);
                                            response.SdpFields.Key = System.Text.UTF8Encoding.UTF8.GetString(b);
                                            break;
                                    }
                                }
                            }
                            break;
                        case 'M':
                            string[] mediaDefParts = value.Split(' ');
                            if (mediaDefParts.Length == 4)
                            {
                                response._currentMediaDef = new MediaDef(mediaDefParts[0], Convert.ToInt32(mediaDefParts[1]), mediaDefParts[2], Convert.ToInt32(mediaDefParts[3]));
                                response.SdpFields.MediaDefs.Add(response._currentMediaDef);
                            }
                            break;
                        case 'A':
                            int pos = value.IndexOf(':');
                            
                            if (pos > 0)
                            {
                                string[] attParts = new string[2];
                                attParts[0] = value.Substring(0, pos);
                                attParts[1] = value.Substring(pos+1);

                                if (response._currentMediaDef != null)
                                {
                                    response._currentMediaDef.Attributes[attParts[0].ToUpper()] = attParts[1];
                                }
                                else
                                {
                                    response.SdpFields.Attributes[attParts[0].ToUpper()] = attParts[1];
                                }


                            }
                            break;
                    }
                }
            }


            return response;
        }
    }
}
