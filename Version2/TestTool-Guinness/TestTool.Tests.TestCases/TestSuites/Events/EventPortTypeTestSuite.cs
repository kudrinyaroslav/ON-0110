///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Linq;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Proxies.Event;
using System.Xml;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Utils.Events;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class EventPortTypeTestSuite : EventTest
    {
        public EventPortTypeTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Event Handling\\Event Properties";

        [Test(Name = "GET EVENT PROPERTIES",
            Path = PATH,
            Order = "01.01.01",
            Id = "1-1-1",
            Category = Category.EVENT,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[]{Functionality.GetEventProperties})]
        public void GetEventPropertiesTest()
        {
            RunTest(() =>
                         {
                             string[] response = null;

                             bool faultReceived = false;

                             bool fixedTopicSet = false;
                             TopicSetType topicSet = null;
                             string[] topicExpressionDialect = null;
                             string[] messageContentFilterDialect = null;
                             string[] producerPropertiesFilterDialect = null;
                             string[] messageContentSchemaLocation = null;
                             System.Xml.XmlElement[] any = null;

                             EnsureEventPortTypeClientCreated();

                             RunStep(
                             () =>
                                {
                                    response = _eventPortTypeClient.GetEventProperties(out fixedTopicSet,
                                                                          out topicSet,
                                                                          out topicExpressionDialect,
                                                                          out messageContentFilterDialect,
                                                                          out producerPropertiesFilterDialect,
                                                                          out messageContentSchemaLocation,
                                                                          out any);
                                }, 
                                "Get Event Properties",
                                new ValidateTypeFault( 
                                    (System.ServiceModel.FaultException fault, out string reason) =>
                                    {
                                        reason = string.Empty;
                                        faultReceived = (fault != null);
                                        return true; 
                                    }));

                             // NVT returns any SOAP fault - PASS
                             if (faultReceived)
                             {
                                 return;
                             }

                             bool allExceptTopicSetNull = (response == null || response.Length == 0) &&
                                                          (topicExpressionDialect == null ||
                                                           topicExpressionDialect.Length == 0) &&
                                                          (producerPropertiesFilterDialect == null ||
                                                           producerPropertiesFilterDialect.Length == 0) &&
                                                          (messageContentSchemaLocation == null ||
                                                           messageContentSchemaLocation.Length == 0) &&
                                                          (any == null || any.Length == 0) && !fixedTopicSet;


                             //The response is completely empty
                             if ((topicSet == null)&& allExceptTopicSetNull)
                             {
                                 Assert(true, "", "Validate response", "The response is empty");
                                 return;
                             }

                             // The response only contains TopicSet element where any child element is not present
                             if (allExceptTopicSetNull && topicSet != null)
                             {
                                 if ((topicSet.Any == null || topicSet.Any.Length == 0) &&
                                     topicSet.documentation == null)
                                 {
                                     Assert(true, "", "Validate response", "The response only contains TopicSet element where any child element is not present");
                                     return;
                                 }
                             }


                             // 5. Validate that the mandatory TopicExpressionDialects are supported by the NVT
                             bool bConcreteExpressionDialectFound = false;
                             bool bConcreteSetExpressionDialectFound = false;


                             Assert(topicExpressionDialect != null, "No Topic Expression Dialects returned", "Check that the DUT returned Topic Expression Dialects");

                             bConcreteExpressionDialectFound = topicExpressionDialect.Contains(OnvifMessage.CONCRETETOPIC,
                                                                                               StringComparer.InvariantCultureIgnoreCase);

                             Assert(bConcreteExpressionDialectFound,
                                 string.Format("Mandatory Topic Expression Dialect {0} not found", OnvifMessage.CONCRETETOPIC),
                                 string.Format("Check that Mandatory Topic Expression Dialect {0} is supported", OnvifMessage.CONCRETETOPIC));

                             bConcreteSetExpressionDialectFound = topicExpressionDialect.Contains(OnvifMessage.CONCRETESETTOPIC,
                                                                  StringComparer.InvariantCultureIgnoreCase);

                             Assert(bConcreteSetExpressionDialectFound,
                                 string.Format("Mandatory Topic Expression Dialect {0} not found", OnvifMessage.CONCRETESETTOPIC),
                                 string.Format("Check that Mandatory Topic Expression Dialect {0} is supported", OnvifMessage.CONCRETESETTOPIC));

                             // 6. Validate that the mandatory MessageContentFilterDialects is supported by the NVT

                             Assert(messageContentFilterDialect != null,
                                 "No Message Content Filter Dialects returned",
                                 "Check that the DUT returned Message Content Filter Dialects");

                             string mandatoryDialect = "http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";

                             bool bMandatoryDialectFound = messageContentFilterDialect.Contains(mandatoryDialect,
                                                                  StringComparer.InvariantCultureIgnoreCase);

                             Assert(bMandatoryDialectFound,
                                 string.Format("Mandatory Message Content Filter Dialect {0} not found", mandatoryDialect),
                                 string.Format("Check if the DUT supports mandatory Message Content Filter Dialect {0}", mandatoryDialect));

                             // 7. Verify that the NVT returns a valid topic namepace

                             //
                             // I think you should validate that there is at least one topic namespace 
                             // and that it is a valid string for an uri. All other testing could cause problems, 
                             // because you need not support the onvif topic namespace (unfortunally) 
                             // and you could only use your own vendor specific namespace.
                             //
                             //

                             bool bFound = false;

                             foreach (string result in response)
                             {
                                 if (result.IsValidUrl())
                                 {
                                     bFound = true;
                                     break;
                                 }
                             }

                             Assert(bFound, "No valid topic namespace found", "Check if response contains at least one topic namespace and that it is a valid string for an uri");

                             // 8. Verify that the DUT supports at least one Topic Set, validate that the TopicSet is well formed
                             // !!! response assumes exactly one or none TopicSets !!!
                             ValidateTopicSet(topicSet);

                         });

        }

        void ValidateTopicSet(TopicSetType topicSet)
        {
            Assert(topicSet != null, "TopicSet is null", "Check that the TopicSet returned is not null");
            Assert(topicSet.Any != null, "TopicSet content is null", "Check that the DUT returned not empty TopicSet");
            
            foreach (XmlElement element in topicSet.Any)
            {
                if (FindTopic(element))
                {
                    return;
                }
            }

            Assert(false, "No topics found!", "Check if response contains at least one topic in TopicSet");
        }

        bool FindTopic(XmlElement element)
        {
            if (element.HasAttribute(BaseNotification.TOPIC, BaseNotification.T1))
            {
                XmlAttribute topicAttribute = element.Attributes[BaseNotification.TOPIC, BaseNotification.T1];

                if (string.Compare(topicAttribute.Value, "true", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    // Element represents a topic.
                    return true;
                }
            }

            // If not a topic - enumerate child elements.
            foreach (XmlElement child in element.ChildNodes)
            {
                if (FindTopic(child))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
