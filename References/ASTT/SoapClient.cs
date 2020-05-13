using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using System.Xml;

namespace ONVIFTestTool
{
    /// <summary>
    /// SOAP Client (all functions in static)
    /// </summary>
    class SoapClient
    {
        #region Fields

        private static TcpClient m_client = null;
        private static NetworkStream m_netStream = null;

        #endregion //Fields

        #region Methods

        /// <summary>
        /// Open connection to host
        /// </summary>
        /// <param name="serviceInfo">Info about service</param>
        public static void OpenConnection(ServiceInfo serviceInfo)
        {
            // Create new TCP client (open TCP connection with service)
            m_client = new TcpClient(serviceInfo.Adress, serviceInfo.Port);
            m_netStream = m_client.GetStream();
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public static void CloseConnection()
        {
            if (m_netStream != null) m_netStream.Close();
            if (m_client != null) m_client.Close();
        }

        /// <summary>
        /// Send SOAP request (from file)
        /// </summary>
        /// <param name="inputFile">Path to file with request XML</param>
        /// <param name="outputFile">Path to file where result will be saved</param>
        /// <param name="pathToResponseFileRealFull">Path to file where result will be saved (with HTTP header)</param>
        /// <param name="serviceInfo">Host information</param>
        /// <param name="resultMessage">Result message (empty if we get answer)</param>
        /// <param name="time">Time between request and first response packet</param>
        /// <param name="CloseConnection">Do we need close connection after each request (true recomended)</param>
        /// <returns>SOAP Response</returns>
        public static bool SendSoapRequest(string inputFile, string outputFile, string pathToResponseFileRealFull, ServiceInfo serviceInfo, out string resultMessage, out double time, bool CloseConnection)
        {
            
            bool result = true;
            resultMessage = "";
            time = 0;

            string fileName = Path.GetFileNameWithoutExtension(inputFile);
            foreach (XmlNode service in ServiceInfo.Service.ChildNodes)
            {
                if (fileName.Contains(service.SelectSingleNode("FileName").InnerText))
                {
                    serviceInfo.Path = service.SelectSingleNode("Path").InnerText;

                }
            }

            try
            {

                // Create request text to send
                byte[] requestData = CreateSoapRequest(inputFile, serviceInfo, CloseConnection);

                // Work with network stream
                //using (NetworkStream netStream = client.GetStream())
                {

                    
                    int bytes = 0;
                    System.Diagnostics.Stopwatch counter = new System.Diagnostics.Stopwatch();

                    // Receive response
                    StringBuilder response = new StringBuilder();
                    
                    

                    m_netStream.Write(requestData, 0, requestData.Length);

                    
                    counter.Start();

                    bool isCounterStopped = false;
                    try
                    {
                        if (CloseConnection)
                        {
                            m_netStream.ReadTimeout = 60000;
                            byte[] responseBuffer = new byte[2048];
                            do
                            {
                                bytes = m_netStream.Read(responseBuffer, 0, responseBuffer.Length);
                                if (!isCounterStopped)
                                {
                                    counter.Stop();
                                    isCounterStopped = true;
                                }
                                response.AppendFormat("{0}", Encoding.UTF8.GetString(responseBuffer, 0, bytes));
                                m_netStream.ReadTimeout = 5000;

                                //System.Diagnostics.Debug.WriteLine("bytes=" + bytes.ToString());
                            }
                            while ((bytes > 0));
                        }
                        else
                        {
                            //Realy temporary variant!!!

                            m_netStream.ReadTimeout = 60000;
                            byte[] responseBuffer = new byte[2048];
                            do
                            {
                                bytes = m_netStream.Read(responseBuffer, 0, responseBuffer.Length);
                                if (!isCounterStopped)
                                {
                                    counter.Stop();
                                    isCounterStopped = true;
                                }
                                response.AppendFormat("{0}", Encoding.UTF8.GetString(responseBuffer, 0, bytes));
                                m_netStream.ReadTimeout = 5000;

                                System.Diagnostics.Debug.WriteLine("bytes=" + bytes.ToString());
                            }
                            while ((bytes == 2048));

                            do
                            {
                                bytes = m_netStream.Read(responseBuffer, 0, responseBuffer.Length);
                                if (!isCounterStopped)
                                {
                                    counter.Stop();
                                    isCounterStopped = true;
                                }
                                response.AppendFormat("{0}", Encoding.UTF8.GetString(responseBuffer, 0, bytes));
                                m_netStream.ReadTimeout = 5000;

                          //      System.Diagnostics.Debug.WriteLine("bytes=" + bytes.ToString());
                            }
                            while ((bytes == 2048));
                        }
                    }
                    catch (IOException ex) 
                    {
                      //  System.Diagnostics.Debug.WriteLine("IOEX {0}", ex.Message);
                    }

                    System.Diagnostics.Debug.WriteLine("END");
                    time = counter.ElapsedMilliseconds;

                    // Write response to file
                    if (response.ToString() != "")
                    {
                        SaveSoapResponse(outputFile, pathToResponseFileRealFull, response.ToString());
                        resultMessage = response.ToString();
                    }
                    else
                    {
                        resultMessage = "No answer from server.";
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
                result = false;
            }
            finally
            {
               
            }

            return result;
        }

        /// <summary>
        /// Send SOAP request (from string)
        /// </summary>
        /// <param name="inputRequest">String with request</param>
        /// <param name="outputFile">Path to file where result will be saved</param>
        /// <param name="pathToResponseFileRealFull">Path to file where result will be saved (with HTTP header)</param>
        /// <param name="serviceInfo">Host information</param>
        /// <param name="resultMessage">Result message (empty if we get answer)</param>
        /// <param name="time">Time between request and first response packet</param>
        /// <returns>SOAP Response</returns>
        public static bool SendSoapRequestString(string inputRequest, string outputFile, string pathToResponseFileRealFull, ServiceInfo serviceInfo, out string resultMessage, out double time)
        {
            TcpClient client = null;
            bool result = true;
            resultMessage = "";
            time = 0;

            try
            {
                // Create request text to send
                byte[] requestData = CreateSoapRequestString(inputRequest, serviceInfo);

                // Create new TCP client (open TCP connection with service)
                client = new TcpClient(serviceInfo.Adress, serviceInfo.Port);

                // Work with network stream
                using (NetworkStream netStream = client.GetStream())
                {
                    int bytes = 0;
                    System.Diagnostics.Stopwatch counter = new System.Diagnostics.Stopwatch();

                    // Receive response
                    StringBuilder response = new StringBuilder();
                    netStream.ReadTimeout = 60000;

                    netStream.Write(requestData, 0, requestData.Length);

                    counter.Start();

                    bool isCounterStopped = false;
                    try
                    {
                        byte[] responseBuffer = new byte[256];
                        do
                        {
                            bytes = netStream.Read(responseBuffer, 0, responseBuffer.Length);
                            if (!isCounterStopped)
                            {
                                counter.Stop();
                                isCounterStopped = true;
                            }
                            response.AppendFormat("{0}", Encoding.UTF8.GetString(responseBuffer, 0, bytes));
                            netStream.ReadTimeout = 1000;
                        }
                        while (bytes > 0);
                    }
                    catch (IOException) { }

                    time = counter.ElapsedMilliseconds;

                    // Write response to file
                    if (response.ToString() != "")
                    {
                        SaveSoapResponse(outputFile, pathToResponseFileRealFull, response.ToString());
                        resultMessage = response.ToString();
                        resultMessage = resultMessage.Replace("\r\n", "\n");
                        resultMessage = resultMessage.Replace("\n", "\r\n");
                        resultMessage = resultMessage.Replace("\t", "  ");
                    }
                    else
                    {
                        resultMessage = "No answer from server.";
                        result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
                result = false;
            }
            finally
            {
                if (client != null) client.Close();
            }

            return result;
        }

        #endregion //Methods

        /// <summary>
        /// Save SOAP Response to file
        /// </summary>
        /// <param name="outputFile">Path to file where result will be saved</param>
        /// <param name="pathToResponseFileRealFull">Path to file where result will be saved (with HTTP header)</param>
        /// <param name="response">Response</param>
        static private void SaveSoapResponse(string outputFile, string pathToResponseFileRealFull, string response)
        {
            if (pathToResponseFileRealFull != "")
            {
                using (StreamWriter writer = new StreamWriter(pathToResponseFileRealFull))
                {
                    writer.Write(response);
                }
            }
            
            if (response.IndexOf("<?xml") != -1)
            {
                response = response.Substring(response.IndexOf("<?xml"));
            }

            if (outputFile != "")
            {
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    writer.Write(response);
                }

            }
        }

        /// <summary>
        /// Create SOAP request text
        /// </summary>
        /// <param name="inputFile">Path to file with request XML</param>
        /// <param name="serviceInfo">Host information</param>
        /// <param name="CloseConnection">Do we need close connection after each request (true recomended)</param>
        /// <returns>HTTP request text</returns>
        static private byte[] CreateSoapRequest(string inputFile, ServiceInfo serviceInfo, bool CloseConnection)
        {
            // Read content from file
            string content = "";
            using (StreamReader reader = new StreamReader(inputFile))
            {
                content = reader.ReadToEnd();
            }

            //Not universal but it is work for now
            if (serviceInfo.User != "")
            {
                string header = "<s:Header>\r\n";
                header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                header = header + "      <wsse:UsernameToken>\r\n";
                header = header + "        <wsse:Username>" + serviceInfo.User + "</wsse:Username>\r\n";
                header = header + "        <wsse:Password>" + serviceInfo.Password + "</wsse:Password>\r\n";
                header = header + "      </wsse:UsernameToken>\r\n";
                header = header + "    </wsse:Security>\r\n";
                header = header + "  </s:Header>\r\n";
                header = header + "  <s:Body>";
                content = content.Replace("<s:Body>", header);

                header = "<soap:Header>\r\n";
                header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                header = header + "      <wsse:UsernameToken>\r\n";
                header = header + "        <wsse:Username>" + serviceInfo.User + "</wsse:Username>\r\n";
                header = header + "        <wsse:Password>" + serviceInfo.Password + "</wsse:Password>\r\n";
                header = header + "      </wsse:UsernameToken>\r\n";
                header = header + "    </wsse:Security>\r\n";
                header = header + "  </soap:Header>\r\n";
                header = header + "  <soap:Body>";
                content = content.Replace("<soap:Body>", header);

                header = "<SOAP-ENV:Header>\r\n";
                header = header + "    <wsse:Security xmlns:wsse = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n";
                header = header + "      <wsse:UsernameToken>\r\n";
                header = header + "        <wsse:Username>" + serviceInfo.User + "</wsse:Username>\r\n";
                header = header + "        <wsse:Password>" + serviceInfo.Password + "</wsse:Password>\r\n";
                header = header + "      </wsse:UsernameToken>\r\n";
                header = header + "    </wsse:Security>\r\n";
                header = header + "  </SOAP-ENV:Header>\r\n";
                header = header + "  <SOAP-ENV:Body>";
                content = content.Replace("<SOAP-ENV:Body>", header);
            }

            //content = content.Replace("\r\n", "");

            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            int size = Encoding.UTF8.GetBytes(content).Count();
            httpRequest.Append("POST " + serviceInfo.Path + " HTTP/1.1\r\n");
            httpRequest.Append("Content-Type: text/xml; charset=utf-8\r\n");
            httpRequest.Append("Content-Length: " + size.ToString() + "\r\n");
            httpRequest.Append("Accept: */*\r\n");
            if (!CloseConnection)
            {
                httpRequest.Append("Connection: Keep-Alive\r\n");
            }
            else
            {
                httpRequest.Append("Connection: Close\r\n");
            }
            httpRequest.Append("User-Agent: Mozilla/4.0 (compatible; Win32; WinHttp.WinHttpRequest.5)\r\n");
            httpRequest.Append("Host: " + serviceInfo.Adress + "\r\n");
            httpRequest.Append("\r\n");
            httpRequest.Append(content);

            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString())); 
        }

        /// <summary>
        /// Create SOAP request text
        /// </summary>
        /// <param name="inputFile">Path to file with request XML</param>
        /// <param name="serviceInfo">Host information</param>
        /// <returns>HTTP request text</returns>
        static private byte[] CreateSoapRequestString(string inputString, ServiceInfo serviceInfo)
        {
            int size = Encoding.UTF8.GetBytes(inputString).Count();
            // Create HTTP headers and add content
            StringBuilder httpRequest = new StringBuilder();
            httpRequest.Append("POST " + serviceInfo.Path + " HTTP/1.1\r\n");
            httpRequest.Append("Content-Type: text/xml; charset=utf-8\r\n");
            httpRequest.Append("Content-Length: " + size.ToString() + "\r\n");
            httpRequest.Append("Accept: */*\r\n");
            httpRequest.Append("Connection: Close\r\n");
            httpRequest.Append("User-Agent: Mozilla/4.0 (compatible; Win32; WinHttp.WinHttpRequest.5)\r\n");
            httpRequest.Append("Host: " + serviceInfo.Adress + "\r\n");
            httpRequest.Append("\r\n");
            httpRequest.Append(inputString);

            // Convert HTTP request to byte array to send
            return (Encoding.UTF8.GetBytes(httpRequest.ToString()));
        }
    }
}
