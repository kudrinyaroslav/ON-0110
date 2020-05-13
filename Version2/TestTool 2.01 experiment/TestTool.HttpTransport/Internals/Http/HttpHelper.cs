///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Text;
using System.IO;
using TestTool.HttpTransport.Exceptions;
using System.Xml;

namespace TestTool.HttpTransport.Internals.Http
{
    /// <summary>
    /// Contains static methods for working with HTTP packets.
    /// </summary>
    class HttpHelper
    {
        public const string CONTENTLENGTH = "Content-Length";
        public const string CONTENTTYPE = "Content-Type";
        private const string APPLICATIONSOAPXML = "application/soap+xml";
        private const string CHARSET = "charset";
        private const string UTF8 = "utf-8";

        private const string STATUSLINEPATTERN = "POST {0} HTTP/1.1\r\n";
        private const string HOSTLINEPATTERN = "Host: {0}\r\n";

        /// <summary>
        /// Checks if whole packet has been received (accordingly to Content-Length).
        /// </summary>
        /// <param name="responseStream">Response stream with data.</param>
        /// <param name="header">Structure with header information to be filled.</param>
        /// <returns>True, if we still have to wait for answer; false otherwise.</returns>
        public static bool ContinueReading(MemoryStream responseStream, out HttpPacket header)
        {
            //System.Diagnostics.Debug.WriteLine("check if we still have to wait for message.");
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

                //System.Diagnostics.Debug.WriteLine(nextLine);

                bodyOffset += (2 + nextLine.Length);

                if (nextLine == null)
                {
                    //System.Diagnostics.Debug.WriteLine("NULL instead of line");
                    break;
                }

                if (!string.IsNullOrEmpty(nextLine) && string.IsNullOrEmpty(nextLine.Trim('\0')))
                {
                    //System.Diagnostics.Debug.WriteLine("\\0\\0\\0... instead of line");
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
                        int descriptionStart = statusParts[0].Length + statusParts[1].Length + 2;
                        header.StatusDescription = nextLine.Substring(descriptionStart);

                        if (code == "204" || code == "304" || code.StartsWith("1"))
                        {
                            //System.Diagnostics.Debug.WriteLine("No body supposed");
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
                            //System.Diagnostics.Debug.WriteLine(string.Format("Content-Length found: {0}", headerValue));

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
                            //System.Diagnostics.Debug.WriteLine("Empty line - end of headers");
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
            header.NoBodySupposed = noBodySupposed;

            // empty line found - body can be checked
            if (bFound)
            {
                // message must not have a body;
                if (noBodySupposed)
                {
                    //System.Diagnostics.Debug.WriteLine("Stop read");
                    return false;
                }
                else
                {
                    if (contentLength <= responseStream.Length - bodyOffset)
                    {
                        //System.Diagnostics.Debug.WriteLine("Content length OK - stop read");
                        return false;
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("Content length NOT OK, continue reading");
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Create HTTP headers for a SOAP message.
        /// </summary>
        /// <param name="size">SOAP message size.</param>
        /// <param name="path">Address for the first line.</param>
        /// <param name="address">Hostname.</param>
        /// <returns>UTF-8 encoded string.</returns>
        public static byte[] CreateHttpHeaders(long size, string path, string address)
        {

            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            httpRequest.AppendFormat(STATUSLINEPATTERN, path);
            httpRequest.AppendFormat(HOSTLINEPATTERN, address);
            httpRequest.AppendFormat("{0}: {1}; {2}={3}\r\n", CONTENTTYPE, APPLICATIONSOAPXML, CHARSET, "utf-8");
            httpRequest.Append(CONTENTLENGTH + ": " + (size).ToString() + "\r\n");
            httpRequest.Append("\r\n");

            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString()));
        }
    
        /// <summary>
        /// Performs basic headers validation.
        /// </summary>
        /// <param name="packet">HTTP packet information.</param>
        /// <returns></returns>
        /// <remarks>The following headers are validated:
        /// * Content-Type, as it it mandatory for SOAP 1.2 (presence + value)
        /// 
        /// </remarks>
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
                if (StringComparer.InvariantCultureIgnoreCase.Compare(contentType, APPLICATIONSOAPXML) != 0)
                {
                    throw new HttpProtocolException(string.Format("Content-Type mismatch; expected: application/soap+xml, actual: {0}", contentType));
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


        public static string GetFormattedMessage(byte[] messageBytes, 
            int bodyOffset)
        {
            try
            {
                int count = messageBytes.Length - bodyOffset;
                byte[] soapPacket = new byte[count];

                // extract message body
                Array.Copy(messageBytes, bodyOffset, soapPacket, 0, count);
                string sp = Encoding.UTF8.GetString(soapPacket);

                // load XML. If an error occurs, raw message will be returned from catch block.
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.LoadXml(sp);

                // write HTTP headers
                MemoryStream formattedStream = new MemoryStream();
                formattedStream.Write(messageBytes, 0, bodyOffset);

                string str = Encoding.UTF8.GetString(formattedStream.GetBuffer());

                // save formatted XML
                MemoryStream xmlStream = new MemoryStream();
                doc.Save(xmlStream);

                int offset = 0;
                byte[] bytes = xmlStream.GetBuffer();
                while (bytes[offset] != (byte)'<')
                {
                    offset++;
                }

                //System.Diagnostics.Debug.WriteLine(string.Format("BOM: {0}", (offset == 0) ? "NOT PRESENT" : "PRESENT"));

                formattedStream.Write(bytes, offset, (int)xmlStream.Length - offset);

                string message = Encoding.UTF8.GetString(formattedStream.GetBuffer());

                xmlStream.Close();
                formattedStream.Close();

                return message;
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetString(messageBytes);
            }
        }

    }
}
