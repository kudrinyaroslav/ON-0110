using System;
using System.Text;
using System.IO;

namespace HttpTransport
{
    class HttpHelper
    {
        private const string CONTENTLENGTH = "Content-Length";
        private const string CONTENTTYPE = "Content-Type";
        private const string APPLICATIONSOAPXML = "application/soap+xml";
        private const string CHARSET = "charset";
        private const string UTF8 = "utf-8";

        private const string STATUSLINEPATTERN = "POST {0} HTTP/1.1\r\n";
        private const string HOSTLINEPATTERN = "Host: {0}\r\n";

        public static bool ContinueReading(MemoryStream responseStream, out HttpPacket header)
        {
            System.Diagnostics.Debug.WriteLine("check if we still have to wait for message.");
            MemoryStream streamCopy = new MemoryStream(responseStream.GetBuffer());
            StreamReader rdr = new StreamReader(streamCopy);

            header = new HttpPacket();

            int contentLength = 0;
            int bodyOffset = 0;

            bool bFound = false;
            bool bFirst = true;

            bool noBodySupposed = false;

            while (!rdr.EndOfStream)
            {
                string nextLine = rdr.ReadLine();

                System.Diagnostics.Debug.WriteLine(nextLine);

                bodyOffset += (2 + nextLine.Length);

                if (nextLine == null)
                {
                    System.Diagnostics.Debug.WriteLine("NULL instead of line");
                    break;
                }

                if (bFirst)
                {
                    // Status-Line = HTTP-Version SP Status-Code SP Reason-Phrase CRLF

                    string[] statusParts = nextLine.Split(' ');

                    if (statusParts.Length > 2)
                    {
                        string code = statusParts[1].Trim();
                        header.HttpVersion = statusParts[0];
                        header.StatusCode = int.Parse(code);
                        header.StatusDescription = statusParts[2];

                        if (code == "204" || code == "304" || code.StartsWith("1"))
                        {
                            System.Diagnostics.Debug.WriteLine("No body supposed");
                            noBodySupposed = true;
                        }
                    }
                    else
                    {
                        throw new HttpProtocolException(string.Format("The first line of a Response message is incorrect: {0}", nextLine));
                    }
                    bFirst = false;
                }
                else
                {
                    // header:        header-field   = field-name ":" OWS [ field-value ] OWS
                    // field-name     = token
                    // field-value    = *( field-content / OWS )
                    // field-content  = *( WSP / VCHAR / obs-text )
                    
                    int colonPos = nextLine.IndexOf(':');
                    
                    if (colonPos > 0)
                    {
                        string headerName = nextLine.Substring(0, colonPos).Trim();
                        string headerValue = nextLine.Substring(colonPos + 1).Trim();

                        header.Headers.Add(headerName, headerValue);

                        if (StringComparer.InvariantCultureIgnoreCase.Compare(headerName, CONTENTLENGTH) == 0)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("Content-Length found: {0}", headerValue));

                            if (!int.TryParse(headerValue, out contentLength))
                            {
                                throw new HttpProtocolException(string.Format("Content-Length incorrect: {0}, integer expected", headerValue));
                            }

                            header.ContentLength = contentLength;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(nextLine))
                        {
                            System.Diagnostics.Debug.WriteLine("Empty line - end of headers");
                            bFound = true;
                            break;
                        }
                        else
                        {
                            throw new HttpProtocolException(string.Format("Unexpected header format: {0}", nextLine));
                        }                        
                    }
                }
            }

            header.BodyOffset = bodyOffset;

            // empty line found - body can be checked
            if (bFound)
            {
                // message must not have a body;
                if (noBodySupposed)
                {
                    System.Diagnostics.Debug.WriteLine("Stop read");
                    return false;
                }
                else
                {
                    if (contentLength <= responseStream.Length - bodyOffset)
                    {
                        System.Diagnostics.Debug.WriteLine("Content length OK - stop read");
                        return false;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Content length NOT OK, continue reading");
                    }
                }
            }
            return true;
        }

        public static byte[] CreateHttpHeaders(long size, string path, string address)
        {
            string xmlDirective = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            httpRequest.AppendFormat(STATUSLINEPATTERN, path);
            httpRequest.AppendFormat(HOSTLINEPATTERN, address);
            httpRequest.AppendFormat("{0}: {1}; {2}={3}\r\n", CONTENTTYPE, APPLICATIONSOAPXML, CHARSET, "utf-8");
            httpRequest.Append(CONTENTLENGTH + ": " + (size + xmlDirective.Length).ToString() + "\r\n");
            httpRequest.Append("\r\n");

            httpRequest.Append(xmlDirective);
            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString()));
        }
    
        public static bool ValidateHttpHeaders(HttpPacket packet)
        {
            if (!packet.Headers.ContainsKey(CONTENTTYPE))
            {
                throw new HttpProtocolException("Mandatory Content-Type header not found.");
            }

            string contentTypeHeader = packet.Headers[CONTENTTYPE];
            string[] contentTypeDefinitions = contentTypeHeader.Split(';');
            if (contentTypeDefinitions.Length == 0)
            {
                throw new HttpProtocolException("Content-Type not defined");
            }
            else
            {
                string contentType = contentTypeDefinitions[0];
                // todo: remove this?
                if (StringComparer.InvariantCultureIgnoreCase.Compare(contentType, APPLICATIONSOAPXML) != 0)
                {
                    //throw new HttpProtocolException(string.Format("Content-Type mismatch; expected: application/soap+xml, actual: {0}", contentType));
                }
                packet.ContentType = contentType;
                for (int i = 1; i < contentTypeDefinitions.Length; i++)
                {
                    string[] parameters = contentTypeDefinitions[i].Trim().Split('=');
                    if (parameters.Length > 1)
                    {
                        if (StringComparer.InvariantCultureIgnoreCase.Compare(parameters[0], "charset") == 0)
                        {
                            packet.Encoding = parameters[1].Trim();

                            if (StringComparer.InvariantCultureIgnoreCase.Compare(packet.Encoding, UTF8) != 0)
                            {
                                throw new HttpProtocolException(string.Format("Charset mismatch. Expected: utf-8, actual: {0}", packet.Encoding));
                            }
                        }
                    }
                }
            }

            return true;
        }

    }
}
