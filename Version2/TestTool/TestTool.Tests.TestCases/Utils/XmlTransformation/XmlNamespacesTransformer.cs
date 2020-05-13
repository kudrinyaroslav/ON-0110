using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Reflection;
using TestTool.HttpTransport.Interfaces;

namespace TestTool.Tests.CommonUtils.XmlTransformation
{
    public enum XmlTransformation
    {
        EachTag,
        ParentTag,
        NotStandardPrefixes,
        DifferentPrefixes,
        SamePrefixes
    }

    public class XmlNamespacesTransformer : ISoapMessageMutator
    {
        private XmlTransformation _transformation;
        private bool _omitXmlDeclaration = true;

        public XmlNamespacesTransformer(XmlTransformation transformation)
        {
            _transformation = transformation;
        }

        public XmlNamespacesTransformer(XmlTransformation transformation, bool omitXmlDeclaration)
            : this(transformation)
        {
            _omitXmlDeclaration = omitXmlDeclaration;
        }

        private void Transform(XmlDocument doc)
        {
            switch (_transformation)
            {
                case XmlTransformation.EachTag:
                    {
                        XmlRemoveUnusedNamespaces(doc);
                        XmlDocument docEmpty = new XmlDocument();
                        int prefixAttributesCounter = 1;
                        doc.InnerXml = GetOuterXml(doc.DocumentElement, docEmpty,
                        (node) =>
                        {
                            node.Prefix = string.Empty;
                            RemoveNotDefaultNamespaces(node);
                            SetDifferentAttributePrefixes(node, ref prefixAttributesCounter);
                        });
                    }
                    break;
                case XmlTransformation.ParentTag:
                    {
                        int prefixAttributesCounter = 1;
                        UpdateNamespacePrefix(doc.DocumentElement,
                        (node) =>
                        {
                            node.Prefix = string.Empty;
                            SetDifferentAttributePrefixes(node, ref prefixAttributesCounter);
                        });
                        XmlRemoveUnusedNamespaces(doc);
                    }
                    break;
                case XmlTransformation.NotStandardPrefixes:
                    {
                        Dictionary<string, string> namespaces = new Dictionary<string, string>();
                        Dictionary<string, string> namespacesForAttributes = new Dictionary<string, string>();
                        int prefixCounter = 1;
                        XmlDocument docEmpty = new XmlDocument();
                        doc.InnerXml = GetOuterXml(doc.DocumentElement, docEmpty,
                        (node) =>
                        {
                            if (!namespaces.ContainsKey(node.NamespaceURI))
                            {
                                node.Prefix = "ns" + prefixCounter++;
                                namespaces.Add(node.NamespaceURI, node.Prefix);
                            }
                            else
                            {
                                node.Prefix = namespaces[node.NamespaceURI];
                            }
                            RemoveNotDefaultNamespaces(node);
                            // change prefixes of attributes: the same prefixes for atrribute from the same namespace
                            foreach (XmlAttribute attr in node.Attributes)
                            {
                                if (!string.IsNullOrEmpty(attr.NamespaceURI)
                                    && (attr.Name.IndexOf("xmlns") != 0))
                                {
                                    if (!namespacesForAttributes.ContainsKey(attr.NamespaceURI))
                                    {
                                        attr.Prefix = "ns" + prefixCounter++;
                                        namespacesForAttributes.Add(attr.NamespaceURI, attr.Prefix);
                                    }
                                    else
                                    {
                                        attr.Prefix = namespacesForAttributes[attr.NamespaceURI];
                                    }
                                }
                            }
                        });
                        XmlMoveNamespacesToRoot(doc);
                    }
                    break;
                case XmlTransformation.DifferentPrefixes:
                    {
                        XmlRemoveUnusedNamespaces(doc);
                        XmlDocument docEmpty = new XmlDocument();
                        int prefixCounter = 1;
                        int prefixAttributesCounter = 1;
                        doc.InnerXml = GetOuterXml(doc.DocumentElement, docEmpty,
                        (node) =>
                        {
                            node.Prefix = "ns" + prefixCounter++;
                            RemoveNotDefaultNamespaces(node);
                            SetDifferentAttributePrefixes(node, ref prefixAttributesCounter);
                        });
                    }
                    break;
                case XmlTransformation.SamePrefixes:
                    {
                        int prefixAttributesCounter = 1;
                        UpdateNamespacePrefix(doc.DocumentElement,
                        (node) =>
                        {
                            node.Prefix = "ns";
                            SetDifferentAttributePrefixes(node, ref prefixAttributesCounter);
                        });
                        XmlRemoveUnusedNamespaces(doc);
                    }
                    break;
            }
        }

        #region ISoapMessageMutator Members

        public byte[] ProcessMessage(byte[] original)
        {
            string content = Encoding.UTF8.GetString(original);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            Transform(xmlDoc);

            MemoryStream stream = new MemoryStream();

            Encoding encoding = new UTF8Encoding(false);

            XmlWriter writer = new XmlTextWriter(stream, encoding);
            xmlDoc.WriteTo(writer);
            writer.Close();

            byte[] messageBytes = stream.ToArray();
            return messageBytes;
        }

        #endregion

        void XmlApplyTransformation(Stream xslStream, XmlDocument xmlDocument)
        {
            XslCompiledTransform transform = new XslCompiledTransform();

            XmlReader xmlReader = new XmlTextReader(xslStream);

            transform.Load(xmlReader);

            XmlWriterSettings settings = new XmlWriterSettings();

            settings.Indent = false;
            settings.NewLineHandling = NewLineHandling.None;
            settings.NewLineOnAttributes = false;
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.OmitXmlDeclaration = _omitXmlDeclaration;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                    {
                        transform.Transform(new XmlNodeReader(xmlDocument), writer);

                        memoryStream.Position = 0;

                        xmlDocument.LoadXml(reader.ReadToEnd());
                        //[04.09.2013] AKS: It isn't necessary forcibly close local reader that uses local stream.
                        //Moreover, this reader will be closed by using-statement
                        //reader.Close();
                    }
                }
            }
        }

        void XmlRemoveUnusedNamespaces(XmlDocument xmlDocument)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.TestCases.Utils.XmlTransformation.XmlTransformation.RemoveUnusedNamespaces.xslt");
            XmlApplyTransformation(stream, xmlDocument);
        }

        void XmlMoveNamespacesToRoot(XmlDocument xmlDocument)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestTool.Tests.TestCases.Utils.XmlTransformation.XmlTransformation.MoveNamespacesToRoot.xslt");
            XmlApplyTransformation(stream, xmlDocument);
        }

        void RemoveNotDefaultNamespaces(XmlNode node)
        {
            List<XmlAttribute> attributes = new List<XmlAttribute>();
            foreach (XmlAttribute attr in node.Attributes)
            {
                if ((attr.Prefix == "xmlns" && attr.LocalName != node.Prefix)
                    || (attr.Name == "xmlns" && node.Prefix.Length > 0))
                {
                    attributes.Add(attr);
                }
            }
            foreach (XmlAttribute attr in attributes)
            {
                node.Attributes.Remove(attr);
            }
        }

        string GetOuterXml(XmlNode element, XmlDocument docImport, Action<XmlNode> changePrefix)
        {
            bool hasChildNodes = element.HasChildNodes;
            if (hasChildNodes)
            {
                if ((element.ChildNodes.Count == 1)
                    && (element.FirstChild.NodeType == XmlNodeType.Text))
                {
                    hasChildNodes = false;
                }
            }

            XmlNode nodeNew = docImport.ImportNode(element, !hasChildNodes);
            changePrefix(nodeNew);

            if (!hasChildNodes)
            {
                return nodeNew.OuterXml;
            }
            else
            {
                // collect inner content of child nodes
                string inner = string.Empty;
                foreach (XmlNode node in element.ChildNodes)
                {
                    XmlElement child = node as XmlElement;
                    if (child != null)
                    {
                        inner += GetOuterXml(child, docImport, changePrefix);
                    }
                }

                // insert inner manually to prevent loosing namespaces
                string text = nodeNew.InnerText;
                string fakeInner = "REPLACE_ME";
                nodeNew.InnerText = fakeInner;

                return nodeNew.OuterXml.Replace(fakeInner, inner);
            }
        }

        void UpdateNamespacePrefix(XmlElement element, Action<XmlNode> changePrefix)
        {
            changePrefix(element);

            foreach (XmlNode node in element.ChildNodes)
            {
                XmlElement child = node as XmlElement;
                if (child != null)
                {
                    UpdateNamespacePrefix(child, changePrefix);
                }
            }
        }

        void SetDifferentAttributePrefixes(XmlNode node, ref int counter)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (!string.IsNullOrEmpty(attr.NamespaceURI)
                    && (attr.Name.IndexOf("xmlns") != 0))
                {
                    attr.Prefix = "ans" + counter++;
                }
            }
        }
    }
}
