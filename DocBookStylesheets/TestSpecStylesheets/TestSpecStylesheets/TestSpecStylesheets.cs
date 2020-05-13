using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\!PROJECTS\ONVIF CTT\Sources\onvif-ext3\Project Drambuie\ClientTestSpecs";
            string[] fileList = new string[]{
                "ONVIF_Imaging_Client_Test_Specification_v16.07.xml",
                "ONVIF_Profile_S_Client_Test_Specification_v16.07.xml"
                                    };

            foreach (string file in fileList)
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(path, file));

                XmlDocument docNew = new XmlDocument(doc.NameTable);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(docNew.NameTable);
                nsmgr.AddNamespace("def", "http://docbook.org/ns/docbook");
                nsmgr.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");

                XmlNode rootNode = docNew.CreateElement("book", "http://docbook.org/ns/docbook");
                docNew.AppendChild(rootNode);

                foreach (XmlAttribute attr in doc.DocumentElement.Attributes)
                {
                    docNew.DocumentElement.Attributes.Append((XmlAttribute)docNew.ImportNode(attr, true));
                }

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name != "article")
                    {
                        if (node is XmlComment)
                        {
                            docNew.InsertBefore(docNew.CreateComment(node.Value), docNew.DocumentElement);
                        }
                        else
                        {
                            if (node is XmlProcessingInstruction)
                            {
                                docNew.InsertBefore(docNew.CreateProcessingInstruction(((XmlProcessingInstruction)node).Target, ((XmlProcessingInstruction)node).Data), docNew.DocumentElement);
                            }
                            else
                            {
                                docNew.PrependChild(docNew.ImportNode(node, true));
                            }
                        }
                    }

                }

                docNew.InsertBefore(docNew.CreateComment("For PDF version - New ONVIF"), docNew.DocumentElement);
                docNew.InsertBefore(docNew.CreateProcessingInstruction("xml-stylesheet", "href=\"../ONVIFNew-stylesheets/onvif-specification-fo-us.xsl\" type=\"text/xsl\" "), docNew.DocumentElement);

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "section")
                    {
                        XmlNode nodeChapter = docNew.CreateElement("chapter", "http://docbook.org/ns/docbook");
                        foreach (XmlNode nodeSection in node.ChildNodes)
                        {
                            nodeChapter.AppendChild(docNew.ImportNode(nodeSection, true));
                        }
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            nodeChapter.Attributes.Append((XmlAttribute)docNew.ImportNode(attr, true));
                        }
                        docNew.DocumentElement.AppendChild(nodeChapter);
                    }
                    else
                    {
                        if (node.Name == "info")
                        {
                            XmlNode nodeInfo = docNew.DocumentElement.AppendChild(docNew.ImportNode(node, true));

                            nodeInfo.SelectSingleNode("def:title", nsmgr).InnerText = nodeInfo.SelectSingleNode("def:title", nsmgr).InnerText.Substring(6);

                            XmlNode nodeOrgname = docNew.CreateElement("orgname", "http://docbook.org/ns/docbook");
                            XmlElement nodePhrase = docNew.CreateElement("phrase", "http://docbook.org/ns/docbook");
                            XmlElement nodeTrademark = docNew.CreateElement("trademark", "http://docbook.org/ns/docbook");
                            nodeOrgname.AppendChild(nodePhrase);
                            nodePhrase.AppendChild(nodeTrademark);
                            XmlAttribute nodeClass = docNew.CreateAttribute("class");
                            nodeClass.Value = "trade";
                            nodeTrademark.Attributes.Append(nodeClass);
                            nodeTrademark.InnerText = "ONVIF";
                            nodeInfo.PrependChild(nodeOrgname);

                        }
                        else
                        {
                            docNew.DocumentElement.AppendChild(docNew.ImportNode(node, true));
                        }
                    }
                }

                // change value of Subtitle
                XmlNode nodeSubtitle;
                XmlNode element = docNew.DocumentElement;
                nodeSubtitle = element.SelectSingleNode("/*[local-name()='book'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='info'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='subtitle'][namespace-uri()='http://docbook.org/ns/docbook']");
                nodeSubtitle.InnerText = "Version 16.07";

                //add address elemet to info
                XmlNode nodeAddress = docNew.CreateElement("address", "http://docbook.org/ns/docbook");
                XmlNode nodeUri = docNew.CreateElement("uri", "http://docbook.org/ns/docbook");
                nodeUri.InnerText = "www.onvif.org";

                XmlNode nodeCopyright;
                nodeCopyright = element.SelectSingleNode("/*[local-name()='book'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='info'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='copyright'][namespace-uri()='http://docbook.org/ns/docbook']");
                nodeCopyright.ParentNode.InsertAfter(nodeAddress, nodeCopyright);
                nodeAddress.AppendChild(nodeUri);

                //add revhistory
                XmlNode nodeRevhistory = docNew.CreateElement("revhistory", "http://docbook.org/ns/docbook");
                XmlNode nodeRevision;
                XmlNode nodeRevnumber;
                XmlNode nodeRevdescription;
                XmlNode nodePara;
                XmlNode nodeDate;

                XmlNode nodeLegalnotice = element.SelectSingleNode("/*[local-name()='book'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='info'][namespace-uri()='http://docbook.org/ns/docbook']/*[local-name()='legalnotice'][namespace-uri()='http://docbook.org/ns/docbook']");
                nodeLegalnotice.ParentNode.InsertAfter(nodeRevhistory, nodeLegalnotice);


                XmlNode historyOld = docNew.SelectSingleNode("def:book/def:appendix[@xml:id='changes']", nsmgr);

                foreach (XmlNode revision in historyOld.ChildNodes)
                {
                    if (revision.Name == "para")
                    {
                        nodeRevision = docNew.CreateElement("revision", "http://docbook.org/ns/docbook");
                        nodeRevnumber = docNew.CreateElement("revnumber", "http://docbook.org/ns/docbook");
                        nodeRevdescription = docNew.CreateElement("revdescription", "http://docbook.org/ns/docbook");
                        nodePara = docNew.CreateElement("para", "http://docbook.org/ns/docbook");
                        nodeDate = docNew.CreateElement("date", "http://docbook.org/ns/docbook");

                        string versionAndDate = revision.SelectSingleNode("def:emphasis", nsmgr).InnerText;
                        int verIndex = versionAndDate.IndexOf("Version");

                        nodeDate.InnerText = versionAndDate.Substring(0, verIndex - 1);
                        nodeRevnumber.InnerText = versionAndDate.Substring(verIndex + 8);


                        nodeRevhistory.AppendChild(nodeRevision);
                        nodeRevision.AppendChild(nodeRevnumber);
                        nodeRevision.AppendChild(nodeDate);
                        nodeRevision.AppendChild(nodeRevdescription);
                        nodeRevdescription.AppendChild(revision.SelectSingleNode("def:itemizedlist", nsmgr));
                    }

                }

                historyOld.ParentNode.RemoveChild(historyOld);

                XmlNode normRef = docNew.SelectSingleNode("def:book/def:chapter[@xml:id='s.docbook']", nsmgr);
                XmlNode itemizedlist = docNew.CreateElement("itemizedlist", "http://docbook.org/ns/docbook");
                normRef.InsertAfter(itemizedlist, normRef.ChildNodes[0]);
                bool flag = true;
                XmlNode listitem = null;
                foreach (XmlNode node in normRef.SelectNodes("def:para", nsmgr))
                {
                    if (flag)
                    {
                        listitem = docNew.CreateElement("listitem", "http://docbook.org/ns/docbook");
                        itemizedlist.AppendChild(listitem);
                        listitem.AppendChild(node);
                    }
                    else
                    {
                        listitem.AppendChild(node);
                    }
                    flag = !flag;
                }

                docNew.Save(Path.Combine(path, Path.GetFileNameWithoutExtension(file) + "_Updated" + Path.GetExtension(file)));
            }
        }
    }
}
