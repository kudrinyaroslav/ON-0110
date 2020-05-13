///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.Tests.TestCases
{
    /// <summary>
    /// Constants for Base Notification XML-element names.
    /// </summary>
    class BaseNotification
    {
        /// <summary>
        /// Dialect attribute
        /// </summary>
        public const string DIALECT = "Dialect";
        /// <summary>
        /// MessageContent element (for filter)
        /// </summary>
        public const string MESSAGECONTENT = "MessageContent";
        /// <summary>
        /// TopicExpression element (for filter)
        /// </summary>
        public const string TOPICEXPRESSION = "TopicExpression";
        /// <summary>
        /// ConcreteSet dialect URI
        /// </summary>
        public const string CONCRETESETDIALECT = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
        /// <summary>
        /// ItemFilter dialect URI
        /// </summary>
        public const string ITEMFILTERDIALECT = "http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";
        /// <summary>
        /// ONVIF topics namespace
        /// </summary>
        public const string TOPICSNAMESPACE = "http://www.onvif.org/ver10/topics";
        /// <summary>
        /// WSNT namespace (where TopicExpression and MessageContent elements are defined).
        /// </summary>
        public const string WSNT = "http://docs.oasis-open.org/wsn/b-2";
        /// <summary>
        /// Profix for WSNT namespace
        /// </summary>
        public const string WSNTPREFIX = "wsnt";

        /// <summary>
        /// t-1 namespace (where Topic element is declared).
        /// </summary>
        public const string T1 = "http://docs.oasis-open.org/wsn/t-1";
        /// <summary>
        /// Topic element
        /// </summary>
        public const string TOPIC = "topic";

        /// <summary>
        /// Timestamp element
        /// </summary>
        public const string TIMESTAMP = "Timestamp";
        /// <summary>
        /// WSRF-BF namespace
        /// </summary>
        public const string WSRFBFNAMESPACE = "http://docs.oasis-open.org/wsrf/bf-2";
    }
    

}
