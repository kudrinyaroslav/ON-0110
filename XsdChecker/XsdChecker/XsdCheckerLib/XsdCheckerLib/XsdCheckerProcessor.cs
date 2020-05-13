using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace XsdCheckerLib
{
    public static class XsdCheckerProcessor
    {
        public static XsdCheckResult[] CheckXsdFilesMinOccursAtrribute(string filesname)
        {
            List<string> files = new List<string>();

            if (IsFile(filesname))
            {
                files.Add(filesname);
            }
            else
            {
                files.AddRange(Directory.GetFiles(filesname, "*.xsd"));
                files.AddRange(Directory.GetFiles(filesname, "*.wsdl"));
            }

            return CheckFiles(files).ToArray();
        }

        private static bool IsFile(string filesname)
        {
            return new FileInfo(filesname).Exists;
        }

        private static IEnumerable<XsdCheckResult> CheckFiles(IEnumerable<string> files)
        {
            foreach (var filename in files)
            {
                yield return CheckXsdFileMinOccursAtrribute(filename);
            }
        }

        private static XmlNamespaceManager GetXmlNamespaceManager(XmlDocument doc)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);

            nsmgr.AddNamespace("tt", "http://www.onvif.org/ver10/schema");
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsmgr.AddNamespace("xmime", "http://www.w3.org/2005/05/xmlmime");
            nsmgr.AddNamespace("wsnt", "http://docs.oasis-open.org/wsn/b-2");
            nsmgr.AddNamespace("xop", "http://www.w3.org/2004/08/xop/include");
            nsmgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");

            return nsmgr;
        }

        private static XsdCheckResult CheckXsdFileMinOccursAtrribute(string xsdfilename)
        {
            XsdCheckResult res = new XsdCheckResult()
            {
                FilePath = xsdfilename
            };

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xsdfilename);

                XmlNode root = doc.DocumentElement;

                var wrongMinOccursXpath = "//xs:element[@name='Extension' and (not(@minOccurs) or @minOccurs!='0')]";

                XmlNodeList nodeList = root.SelectNodes(wrongMinOccursXpath, GetXmlNamespaceManager(doc));

                foreach (XmlNode node in nodeList)
                {
                    res.ReportError(node.OuterXml);
                }
            }
            catch (Exception ex)
            {
                res.ReportError(ex.Message);
            }
            return res;

        }

        private static void ReportError(this XsdCheckResult res, string errorDescription)
        {
            res.ErrorMessage += errorDescription + "\n\n";
            res.ErrorCount++;
        }
    }
}
