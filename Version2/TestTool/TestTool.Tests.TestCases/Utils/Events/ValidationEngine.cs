using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.TestCases.TestSuites;
using org.zorbaxquery.api;

namespace TestTool.Tests.TestCases.Utils.Events
{
    class ValidationEngine: IDisposable
    {
        #region Singleton implementation
        private static ValidationEngine m_Instance = null;
        public static ValidationEngine GetInstance()
        {
            return m_Instance ?? (m_Instance = new ValidationEngine());
        }
        #endregion

        #region Schema resource management implementation
       
        private static IEnumerable<string> resourceLines(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string script = string.Empty;

                        var line = string.Empty;
                        do
                        {
                            line = reader.ReadLine();
                            if (line != "#####")
                                script += line + Environment.NewLine;
                        }
                        while (!reader.EndOfStream && line != "#####");

                        if (!string.IsNullOrEmpty(script))
                            yield return script;
                    }
                }
            }
        }

        private string GetSchemaPathByPropertyName(string propertyName, string resourcePath)
        {
            var uri = new Uri(string.Format("{0}/{1}", System.IO.Directory.GetCurrentDirectory(), resourcePath).Replace('\\', '/'));
            return uri.AbsolutePath.Replace("(", Uri.HexEscape('(')).Replace(")", Uri.HexEscape(')'));
        }

        public string OnvifSchemaPath
        {
            get { return GetSchemaPathByPropertyName("OnvifSchemaPath",  @"zorba/schemas/onvif.xsd"); }
        }
        #endregion

        #region Resource management implementation
        private InMemoryStore m_Store = null;
        private InMemoryStore Store
        {
            get { return m_Store ?? (m_Store = org.zorbaxquery.api.InMemoryStore.getInstance()); }
        }
        
        private Zorba m_Zorba = null;
        private Zorba Zorba
        {
            get { return m_Zorba ?? (m_Zorba = org.zorbaxquery.api.Zorba.getInstance(Store)); }
        }

        public void Dispose()
        {
            Zorba.shutdown();
            InMemoryStore.shutdown(Store);
        }
        #endregion

        public bool Validate(Proxies.Event.NotificationMessageHolderType msg,
                             TopicInfo expectedTopicInfo,
                             string validationScriptPath, StringBuilder logger, Dictionary<string, string> variables)
        {
            //System.Windows.Forms.MessageBox.Show(System.IO.Directory.GetCurrentDirectory());
            try
            {
                if (!validateEventTopic(expectedTopicInfo, msg, logger))
                {
                    var actualTopicInfo = TopicInfo.ExtractTopicInfoAll(msg.Topic.Any.First().InnerText, msg.Topic.Any.First());
                    logger.AppendLine(string.Format("Invalid notification: event with topic {0} is unexpected", actualTopicInfo.GetDescription()));

                    return false;
                }

                var dst = new StringBuilder();
                var s = new XmlSerializer(typeof(NotificationMessageHolderType));
                s.Serialize(new StringWriter(dst), msg);


                //get the xml manager
                XmlDataManager lManager = Zorba.getXmlDataManager();
                //create an empty Item to store the iterator of the parsed document
                Item lDoc = Item.createEmptyItem();
                //parse the xml document into the Iterator
                var xml = dst.ToString().Replace("utf-16", "utf-8");
                //LogStepEvent(longName.Count().ToString());
                //LogStepEvent(xml);
                Iterator lDocIter = lManager.parseXML(xml);
                lDocIter.open();
                //get the root node of the document
                lDocIter.next(lDoc);
                //close the Iterator
                lDocIter.close();
                ////Even though C# is a garbage collected language the Iterator must be destroyed manually
                lDocIter.destroy();

                StaticContext stContext = Zorba.createStaticContext();
                stContext.addNamespace("wsnt", "http://docs.oasis-open.org/wsn/b-2");
                stContext.addNamespace("xs", "http://www.w3.org/2001/XMLSchema");

                foreach (var validationScript in resourceLines(validationScriptPath))
                {
                    var query = string.Format("import schema namespace tt = '{0}' at 'file://localhost/{1}';", OnvifMessage.ONVIF, OnvifSchemaPath);
                    query += Environment.NewLine + validationScript;

                    //XQuery compiledQuery = Zorba.createQuery();
                    //compiledQuery.compile(query, stContext);
                    XQuery compiledQuery = Zorba.compileQuery(query, stContext);
                    //get the Dynamic Context of a Query
                    DynamicContext dynamicContext = compiledQuery.getDynamicContext();
                    //set the Context Item of the Query
                    dynamicContext.setContextItem(lDoc);

                    if (null != variables)
                        foreach (var variable in variables)
                        { dynamicContext.setVariable(variable.Key, Zorba.getItemFactory().createString(variable.Value)); }

                    var options = new SerializationOptions();
                    options.setOmitXMLDeclaration(SerializationOptions.OmitXMLDeclaration.ZORBA_API_OMIT_XML_DECLARATION_YES);
                    var q = compiledQuery.execute(options);

                    string queryResult, log;
                    var flag = parseAnswer(q, out queryResult, out log);
                    if (flag && "false" == queryResult || !flag)
                    {
                        logger.AppendLine(log);
                        return false;
                    }
                    compiledQuery.destroy();
                }

                return true;
            }
            catch (Exception e)
            {
                logger.AppendLine(e.ToString());
                return false;
            }
            catch
            {
                logger.AppendLine("Unknown exception!");
                return false;
            }
        }

        public bool Validate(Proxies.Event.NotificationMessageHolderType msg, TopicInfo expectedTopicInfo, string validationScriptPath, StringBuilder logger)
        {
            return Validate(msg, expectedTopicInfo, validationScriptPath, logger, null);
        }

        private bool parseAnswer(string answer, out string queryResult, out string log)
        {
            queryResult = log = string.Empty;

            answer = answer.TrimStart('[', ' ').TrimEnd(']');

            if (answer.StartsWith("true"))
            {
                queryResult = "true";
                log = answer.Substring(5).Trim(' ', '\"');
            }
            else if (answer.StartsWith("false"))
            {
                queryResult = "false";
                log = answer.Substring(6).Trim(' ', '\"');
            }
            else
                return false;

            return true;
        }
        bool validateEventTopic(TopicInfo expectedTopicInfo, NotificationMessageHolderType msg, StringBuilder logger)
        {
            var actualTopicInfo = TopicInfo.ExtractTopicInfoAll(msg.Topic.Any.First().InnerText, msg.Topic.Any.First());

            //return expectedTopicInfo.GetPlainInfo().Topic == actualTopicInfo.GetPlainInfo().Topic;
            if (!fillNamespaces(actualTopicInfo, msg, logger))
                return false;

            //TO DO
            return TopicInfo.TopicsMatch(actualTopicInfo, expectedTopicInfo);
        }

        bool fillNamespaces(TopicInfo topicInfo, NotificationMessageHolderType msg, StringBuilder logger)
        {
            try
            {
                while (null != topicInfo)
                {
                    if (!string.IsNullOrEmpty(topicInfo.NamespacePrefix) && string.IsNullOrEmpty(topicInfo.Namespace))
                        topicInfo.Namespace = msg.Topic.LookupNamespace(topicInfo.NamespacePrefix);

                    topicInfo = topicInfo.ParentTopic;
                }
            }
            catch (KeyNotFoundException e)
            {
                //throw new Exception(string.Format("The namespace's prefix {0} isn't defined", topicInfo.NamespacePrefix));
                logger.AppendLine(string.Format("The namespace's prefix {0} isn't defined", topicInfo.NamespacePrefix));

                return true;
            }

            return true;
        }
    }
}
