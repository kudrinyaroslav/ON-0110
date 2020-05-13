using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using HttpTransport;
using System.Xml;
using System.IO;
using TestTool.HttpTransport;

namespace HttpClient
{
    class MessageSpoiler : ISoapMessageMutator
    {
        private Dictionary<string, string> _namespaces;
        public Dictionary<string, string> Namespaces
        {
            get { return _namespaces; }
            set { _namespaces = value; }
        }

        private Dictionary<string, string> _nodesToReplace;
        public Dictionary<string, string> NodesToReplace
        {
            get { return _nodesToReplace; }
            set { _nodesToReplace = value; }
        }

        #region ISoapMessageMutator Members

        public byte[] ProcessMessage(byte[] original)
        {
            string content = Encoding.UTF8.GetString(original);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            System.Diagnostics.Debug.WriteLine("Content before spoiling: ");
            System.Diagnostics.Debug.WriteLine(content);

            if (NodesToReplace != null)
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                foreach (string key in Namespaces.Keys)
                {
                    manager.AddNamespace(key, Namespaces[key]);
                }

                foreach (string nodePath in NodesToReplace.Keys)
                {
                    XmlNode node = xmlDoc.SelectSingleNode(nodePath, manager);
                    if (node != null)
                    {
                        node.InnerText = NodesToReplace[nodePath];
                    }
                }
            }

            MemoryStream stream = new MemoryStream();

            Encoding encoding = new UTF8Encoding(false);

            XmlWriter writer = new XmlTextWriter(stream, encoding);
            xmlDoc.WriteTo(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            return messageBytes;
        }

        #endregion
    }
}
