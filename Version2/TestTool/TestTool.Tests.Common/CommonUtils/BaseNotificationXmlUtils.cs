using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestTool.Tests.Common.CommonUtils
{
    public class BaseNotificationXmlUtils
    {
        /// <summary>
        /// Gets "raw" response from HTTP packet
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static XmlDocument GetRawResponse(string response)
        {
            System.IO.StringReader rdr = new System.IO.StringReader(response);

            string nextLine;
            do
            {
                nextLine = rdr.ReadLine();
            } while (!string.IsNullOrEmpty(nextLine));

            string rawSoapPacket = rdr.ReadToEnd();

            // fix for #976
            rawSoapPacket = RemoveInvalidXmlChars(rawSoapPacket);

            int offset = 0;
            while (rawSoapPacket[offset] != '<' && offset < rawSoapPacket.Length)
            {
                offset++;
            }
            if (offset > 0)
            {
                rawSoapPacket = rawSoapPacket.Substring(offset);
            }

            XmlDocument soapResponse = new XmlDocument();
            soapResponse.LoadXml(rawSoapPacket);
            return soapResponse;
        }

        public static string RemoveInvalidXmlChars(string rawSoapPacket)
        {
            return new string(rawSoapPacket.Where(XmlConvert.IsXmlChar).ToArray());
        }

    }
}
