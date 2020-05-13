///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
namespace TestTool.Tests.TestCases.Utils.Events
{
    /// <summary>
    /// Constants for ONVIF-specific notification elements.
    /// </summary>
    class OnvifMessage
    {
        /// <summary>
        /// ONVIF schema namespace.
        /// </summary>
        public const string ONVIF = "http://www.onvif.org/ver10/schema";
        /// <summary>
        /// Prefix for ONVIF schema namespace
        /// </summary>
        public const string ONVIFPREFIX = "tt";
        /// <summary>
        /// MessageDescription element in TopicSet.
        /// </summary>
        public const string MESSAGEDESCRIPTION = "MessageDescription";

        /// <summary>
        /// SimpleItem message element
        /// </summary>
        public const string SIMPLEITEM = "SimpleItem";
        /// <summary>
        /// ElementItem message element
        /// </summary>
        public const string ELEMENTITEM = "ElementItem";
        /// <summary>
        /// SimpleItem message element
        /// </summary>
        public const string SIMPLEITEMDESCRIPTION = "SimpleItemDescription";
        /// <summary>
        /// ElementItem message element
        /// </summary>
        public const string ELEMENTITEMDESCRIPTION = "ElementItemDescription";
        /// <summary>
        /// Name attribute of ElementItem and SimpleItem
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// Value attribute of SimpleItem
        /// </summary>
        public const string VALUE = "Value";
        /// <summary>
        /// Extension message element.
        /// </summary>
        public const string ITEMLISTEXTENSION = "Extension";

        /// <summary>
        /// UTCTime message attribute
        /// </summary>
        public const string UTCTIMEATTRIBUTE = "UtcTime";
        /// <summary>
        /// PropertyOperation message attribute
        /// </summary>
        public const string PROPERTYOPERATIONTYPE = "PropertyOperation";

        /// <summary>
        /// IsProperty attribute of MessageDescription
        /// </summary>
        public const string ISPROPERTY = "IsProperty";

        /// <summary>
        /// "Initialized" property operation value.
        /// </summary>
        public const string INITIALIZED = "Initialized";
        /// <summary>
        /// "Deleted" property operation value.
        /// </summary>
        public const string DELETED = "Deleted";
        /// <summary>
        /// "Changed" property operation value.
        /// </summary>
        public const string CHANGED = "Changed";

        /// <summary>
        /// Source message element.
        /// </summary>
        public const string SOURCE = "Source";
        /// <summary>
        /// Key message element
        /// </summary>
        public const string KEY = "Key";
        /// <summary>
        /// Data message element
        /// </summary>
        public const string DATA = "Data";
        /// <summary>
        /// Extension message element.
        /// </summary>
        public const string EXTENSION = "Extension";
        /// <summary>
        /// Concrete topic identifier.
        /// </summary>
        public const string CONCRETETOPIC = "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
        /// <summary>
        /// "Concrete Set" topic identifier.
        /// </summary>
        public const string CONCRETESETTOPIC = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";

    }

}
