namespace SMC.Events
{
    /// <summary>
    /// Constants for Base Notification XML-element names.
    /// </summary>
    public class BaseNotification
    {
        /// <summary>
        /// Dialect attribute
        /// </summary>
        public const string DIALECT = "Dialect";
        /// <summary>
        /// TopicExpression element (for filter)
        /// </summary>
        public const string TOPICEXPRESSION = "TopicExpression";
        /// <summary>
        /// ConcreteSet dialect URI
        /// </summary>
        public const string CONCRETESETDIALECT = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
        /// <summary>
        /// WSNT namespace (where TopicExpression and MessageContent elements are defined).
        /// </summary>
        public const string WSNT = "http://docs.oasis-open.org/wsn/b-2";
        /// <summary>
        /// Profix for WSNT namespace
        /// </summary>
        public const string WSNTPREFIX = "wsnt";
    }
    

}
