using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTool.Device
{
    class HttpPacketHelper
    {

        public string GetBody(MemoryStream responseStream, out string httpStatusLine)
        {
            string responseBody = string.Empty;
            httpStatusLine = string.Empty;

            responseStream.Seek(0, SeekOrigin.Begin);
            int bytesRead = 0;

            TextReader rdr = new StreamReader(responseStream);

            string statusLine = rdr.ReadLine();
            bytesRead = statusLine.Length + 2;

            System.Diagnostics.Debug.WriteLine("First status line: " + statusLine);

            string[] statusLineParts = statusLine.Split(' ');
            if (statusLineParts.Length > 1)
            {
                httpStatusLine = statusLine;
                int status = int.Parse(statusLineParts[1]);
                
                if (status == 100)
                {
                    System.Diagnostics.Debug.WriteLine("Skip lines to next HTTP packet");
                    while (true)
                    {
                        string nextLine = rdr.ReadLine();
                        bytesRead += (nextLine.Length + 2);

                        System.Diagnostics.Debug.WriteLine("Next line in packet: " + nextLine);

                        if (string.IsNullOrEmpty(nextLine))
                        {
                            System.Diagnostics.Debug.WriteLine("Empty string found");
                            // responseStream is positioned OK - at the beginning of "real" packet;
                            break;
                        }
                    }

                    statusLine = rdr.ReadLine();
                    httpStatusLine = statusLine;
                }
               
                {
                        
                    int length = 0;
                    while (true)
                    {
                        string nextLine = rdr.ReadLine();
                        bytesRead += (nextLine.Length + 2);

                        System.Diagnostics.Debug.WriteLine("Next line in packet: " + nextLine);
                        
                        if (string.IsNullOrEmpty(nextLine))
                        {
                            System.Diagnostics.Debug.WriteLine("Empty string found");
                            long bytesLeft = responseStream.Length - bytesRead;
                           
                            responseBody = rdr.ReadToEnd();
                            if (bytesLeft < length)
                            {
                                responseBody = string.Empty;
                            }
                            break;
                        }
                        else
                        {
                            string[] headerParts = nextLine.Split(':');
                            if (headerParts.Length > 1)
                            {
                                if (StringComparer.InvariantCultureIgnoreCase.Compare(headerParts[0], "Content-Length") == 0)
                                {
                                    length = int.Parse(headerParts[1]);
                                    System.Diagnostics.Debug.WriteLine("Content-Length: " + length);
                                }
                            }                               
                        }
                    }
                    
                }
            }
            else
            {
                // whole packet not received, event status line not received...
            }

            if (string.IsNullOrEmpty(responseBody))
            {
                responseStream.Seek(0, SeekOrigin.End);
            }

            return responseBody;
        }
    }
}
