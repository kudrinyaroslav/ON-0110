///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace TestTool.Tests.Common.TestEngine
{
    /// <summary>
    /// Provides functionality to add errors in SOAP packets.
    /// Currently its possibly to replace nodes specified.
    /// </summary>
    public class MessageSpoiler : HttpTransport.ISoapMessageMutator
    {
        private Dictionary<string, string> _namespaces;
        /// <summary>
        /// Namespace to be used.
        /// </summary>
        public Dictionary<string, string> Namespaces
        {
            get { return _namespaces; }
            set { _namespaces = value; }
        }

        private Dictionary<string, string> _nodesToReplace;
        /// <summary>
        /// Dictionary of nodes to be corrected (X-Path) and values to be substituted.
        /// </summary>
        public Dictionary<string, string> NodesToReplace
        {
            get { return _nodesToReplace; }
            set { _nodesToReplace = value; }
        }

        /// <summary>
        /// List of nodes to be deleted (X-Path)
        /// </summary>
        public List<string> NodesToDelete { get; set; }

        public Dictionary<string, string> AttributesToDelete { get; set; }

        #region ISoapMessageMutator Members

        /// <summary>
        /// Processes message.
        /// </summary>
        /// <param name="original">Original message.</param>
        /// <returns>Message corrected in accordance with NodesToReplace data.</returns>
        public byte[] ProcessMessage(byte[] original)
        {
            string content = Encoding.UTF8.GetString(original);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            System.Diagnostics.Debug.WriteLine("Content before spoiling: ");
            System.Diagnostics.Debug.WriteLine(content);

            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (string key in Namespaces.Keys)
            {
                manager.AddNamespace(key, Namespaces[key]);
            }
            if (NodesToReplace != null)
            {
                foreach (string nodePath in NodesToReplace.Keys)
                {
                    XmlNode node = xmlDoc.SelectSingleNode(nodePath, manager);
                    if (node != null)
                    {
                        node.InnerText = NodesToReplace[nodePath];
                    }
                }
            }
            if (NodesToDelete != null)
            {
                foreach (string nodePath in NodesToDelete)
                {
                    XmlNode node = xmlDoc.SelectSingleNode(nodePath, manager);
                    node.ParentNode.RemoveChild(node);
                }
            }
            if(AttributesToDelete != null)
            {
                foreach (string nodePath in AttributesToDelete.Keys)
                {
                    XmlNode node = xmlDoc.SelectSingleNode(nodePath, manager);
                    XmlAttribute attr = node.Attributes[AttributesToDelete[nodePath]];
                    node.Attributes.Remove(attr);
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
