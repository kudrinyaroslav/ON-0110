using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Event;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;

namespace TestTool.Tests.TestCases.TestSuites
{
    delegate bool ValidateMessageFunction(NotificationMessageHolderType notification, MessageCheckSettings settings, StringBuilder logger);
    
    /// <summary>
    /// Message settings to pass to the validating method
    /// </summary>
    class MessageCheckSettings
    {
        public Dictionary<NotificationMessageHolderType, XmlElement> RawMessageElements { get; set; }
        public XmlNamespaceManager NamespaceManager { get; set; }
        public TopicInfo ExpectedTopic { get; set; }
        public string ExpectedPropertyOperation { get; set; }
        public object Data { get; set; }
    }
}
