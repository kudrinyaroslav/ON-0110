using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CamerasLogsToXlsx
{
    public class XmlDoc
    {
        private String xmlName;
        private bool append = false;
        private XmlDocument xmlResult;
        public XmlNodeList TestResults;
        
        public XmlDoc(String path)
        {
            xmlName = path.Substring(path.LastIndexOf(@"\")+1);
            xmlName = xmlName.Remove(xmlName.IndexOf("xml") - 1);
            if (xmlName.Contains("[a]"))
                {
                    append = true;
                    xmlName = xmlName.Remove(xmlName.IndexOf("[a]"));
                }
            xmlResult = new XmlDocument();
            xmlResult.Load(path);

           TestResults = xmlResult.SelectNodes("/LogFile/test/LogTest");
        }

        public bool isAppend()
        {
            return append;
        }

        public String GetXmlName()
        {
            return xmlName;
        }

        public String InnerText(XmlNode node, String xpath)
        {
            return node.SelectSingleNode(xpath).InnerText;
        }


        internal string InnerText(object TestResult, string p)
        {
            return InnerText((XmlNode)TestResult, p);
        }
    }
}
