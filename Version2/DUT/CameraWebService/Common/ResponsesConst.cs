using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.CameraWebService.Common
{
    public static class ResponsesConst
    {
        #region Invalid responses (XMLScxema validation testing)

        #region Device Management

        public const string GetServices_InvalidXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"\r\n" +
"<Service>\r\n" +
"<Namespace1>http://www.onvif.org/ver10/accessrules/wsdl</Namespace1>\r\n" +
"<XAddr>http://localhost:17934/ServiceAccessRules10/AccessRulesService.asmx</XAddr>\r\n" +
"<Capabilities>\r\n" +
"<tar:Capabilities xmlns:tar=\"http://www.onvif.org/ver10/accessrules/wsdl\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" MultipleSchedulesPerAccessPointSupported=\"true\"></tar:Capabilities></Capabilities>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>";

        #endregion

        #region Advanced Security

        public const string UploadCertificateWithPrivateKeyInPKCS12_InvalidXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<UploadCertificateWithPrivateKeyInPKCS12Response xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\">\r\n" +
"<CertificationPathID1>certPathID1</CertificationPathID1>\r\n" +
"<KeyID>keyID1</KeyID></UploadCertificateWithPrivateKeyInPKCS12Response></soap:Body></soap:Envelope>";


        #endregion

        #endregion

        #region Honeywell
        public const string GetEventPropertiesHoneywell = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:xenc=\"http://www.w3.org/2001/04/xmlenc#\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:ns2=\"http://www.onvif.org/ver10/pacs\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:ns1=\"http://www.onvif.org/ver10/accesscontrol/wsdl\" xmlns:ns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:ns4=\"http://www.onvif.org/ver10/accessrules/wsdl\" xmlns:ns5=\"http://www.onvif.org/ver10/credential/wsdl\" xmlns:ns6=\"http://www.onvif.org/ver10/schedule/wsdl\" xmlns:tdlb=\"http://www.onvif.org/ver10/network/wsdl/DiscoveryLookupBinding\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tevcppb=\"http://www.onvif.org/ver10/events/wsdl/CreatePullPointBinding\" xmlns:teveb=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tevncb=\"http://www.onvif.org/ver10/events/wsdl/NotificationConsumerBinding\" xmlns:tevnpb=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tevppb=\"http://www.onvif.org/ver10/events/wsdl/PullPointBinding\" xmlns:tevpps=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tevpsmb=\"http://www.onvif.org/ver10/events/wsdl/PausableSubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tevsmb=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:trdb=\"http://www.onvif.org/ver10/network/wsdl/RemoteDiscoveryBinding\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:wsd=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\"><SOAP-ENV:Body><tev:GetEventPropertiesResponse><tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation><wsnt:FixedTopicSet>true</wsnt:FixedTopicSet><wstop:TopicSet><tns1:AccessPoint><State><Enabled wstop:topic=\"true\"><tt:MessageDescription IsProperty=\"true\"><tt:Source><tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Enabled></State></tns1:AccessPoint><tns1:AccessControl><AccessGranted><Credential wstop:topic=\"true\"><tt:MessageDescription><tt:Source><tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription><tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential></AccessGranted></tns1:AccessControl><tns1:AccessControl><Denied><Credential wstop:topic=\"true\"><tt:MessageDescription><tt:Source><tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription><tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription><tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential></Denied></tns1:AccessControl><tns1:AccessControl><Denied><CredentialNotFound wstop:topic=\"true\"><Card wstop:topic=\"true\"><tt:MessageDescription><tt:Source><tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription><tt:SimpleItemDescription Name=\"CardNr\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Card></CredentialNotFound></Denied></tns1:AccessControl><tns1:Configuration><AccessProfile><Removed wstop:topic=\"true\"><tt:MessageDescription IsProperty=\"false\"><tt:Source><tt:SimpleItemDescription Name=\"AccessProfileToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Removed></AccessProfile></tns1:Configuration><tns1:Configuration><AccessProfile><Changed wstop:topic=\"true\"><tt:MessageDescription IsProperty=\"false\"><tt:Source><tt:SimpleItemDescription Name=\"AccessProfileToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Changed></AccessProfile></tns1:Configuration><tns1:Door><State><DoorMode wstop:topic=\"true\"><tt:MessageDescription IsProperty=\"true\"><tt:Source><tt:SimpleItemDescription Name=\"DoorToken\" Type=\"ns2:ReferenceToken\"></tt:SimpleItemDescription></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"State\" Type=\"ns3:DoorMode\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorMode></State></tns1:Door></wstop:TopicSet><wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect><wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect><tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect><tev:MessageContentSchemaLocation>http://www.onvif.org/onvif/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #416

        public const string ResponseTicket416 =
"<soapenv:Envelope xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" " +
"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" " +
"xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" " +
"xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" " +
"xmlns:xmime5=\"http://www.w3.org/2005/05/xmlmime\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" " +
"xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" " +
"xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:tan1=\"http://www.onvif.org/ver10/analytics/wsdl/RuleEngineBinding\" " +
"xmlns:tan=\"http://www.onvif.org/ver10/analytics/wsdl\" xmlns:tan2=\"http://www.onvif.org/ver10/analytics/wsdl/AnalyticsEngineBinding\" " +
"xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" " +
"xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" " +
"xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" " +
"xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" " +
"xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" " +
"xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" " +
"xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" " +
"xmlns:tnsionodes=\"http://www.ionodes.com/ver10/event/topics\" xmlns:evtionodes=\"http://www.ionodes.com/ver10/event\">\n" +
"<soapenv:Header>\n" +
"<wsa5:Action soapenv:mustunderstand=\"true\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa5:Action>\n</soapenv:Header>\n" +
"<soapenv:Body>\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\"><CurrentTime>2013-08-01T19:42:58.711125Z</CurrentTime><TerminationTime>2013-08-01T19:43:58.711125Z</TerminationTime><NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\"><Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:VideoSource/MotionAlarm</Topic><Message><tt:Message UtcTime=\"2013-08-01T19:42:58.624Z\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><tt:Source><tt:SimpleItem Name=\"VideoSourceToken\" Value=\"videoinput_1:0\" /></tt:Source><tt:Data><tt:SimpleItem Name=\"State\" Value=\"on\" /><tt:SimpleItem Name=\"Level\" Value=\"99\" /><tt:SimpleItem Name=\"ROI\" Value=\"1\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse>\n</soapenv:Body>\n</soapenv:Envelope>\n\r\n";

        #endregion

        #region Ticket #343

        public const string ResponseTicket343_GetEventPropertiesResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<GetEventPropertiesResponse xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<DoorMode wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"<DoorPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorPhysicalState\" /></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
"<DoubleLockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
"<LockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
"<DoorTamper wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorTamperState\" /></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
"<DoorAlarm wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorAlarmState\" /></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
"<DoorFault wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tns4=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorFaultState\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns4:string\" /></tt:Data></tt:MessageDescription></DoorFault></State></tns1:Door>  \r\n" +
"\r\n" +
"\r\n" +
"</TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        public const string ResponseTicket343_TopicSet = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
        "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
        "<soap:Header>\r\n" +
        "<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
        "<soap:Body xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
        "<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
        "<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
        "<TopicSet xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
        "<State xmlns=\"\">\r\n" +
        "<DoorMode wstop:topic=\"true\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
        "<DoorPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorPhysicalState\" /></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
        "<DoubleLockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
        "<LockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
        "<DoorTamper wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorTamperState\" /></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
        "<DoorAlarm wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorAlarmState\" /></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
        "<DoorFault wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tns4=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
        "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
        "<tt:Source>\r\n" +
        "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
        "<tt:Data>\r\n" +
        "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorFaultState\" />\r\n" +
        "<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns4:string\" /></tt:Data></tt:MessageDescription></DoorFault></State></tns1:Door>  \r\n" +
        "\r\n" +
        "\r\n" +
        "</TopicSet>\r\n" +
        "<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
        "<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
        "<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
        "<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        public const string ResponseTicket343_RootTopic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<DoorMode wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"<DoorPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorPhysicalState\" /></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
"<DoubleLockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
"<LockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
"<DoorTamper wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorTamperState\" /></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
"<DoorAlarm wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorAlarmState\" /></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
"<DoorFault wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tns4=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorFaultState\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns4:string\" /></tt:Data></tt:MessageDescription></DoorFault></State></tns1:Door>  \r\n" +
"\r\n" +
"\r\n" +
"</TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        public const string ResponseTicket343_StateTopic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\">\r\n" +
"<DoorMode wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"<DoorPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorPhysicalState\" /></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
"<DoubleLockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
"<LockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
"<DoorTamper wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorTamperState\" /></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
"<DoorAlarm wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorAlarmState\" /></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
"<DoorFault wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tns4=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorFaultState\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns4:string\" /></tt:Data></tt:MessageDescription></DoorFault></State></tns1:Door>  \r\n" +
"\r\n" +
"\r\n" +
"</TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        public const string ResponseTicket343_DoorMode = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<DoorMode wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"<DoorPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorPhysicalState\" /></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
"<DoubleLockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
"<LockPhysicalState wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:LockPhysicalState\" /></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
"<DoorTamper wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorTamperState\" /></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
"<DoorAlarm wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorAlarmState\" /></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
"<DoorFault wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tns4=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorFaultState\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns4:string\" /></tt:Data></tt:MessageDescription></DoorFault></State></tns1:Door>\r\n" +
"<tns1:AccessControl xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<AccessGranted xmlns=\"\">\r\n" +
"<Anonymous wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"External\" Type=\"tns3:boolean\" /></tt:Data></tt:MessageDescription></Anonymous>\r\n" +
"<Credential wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" />\r\n" +
"<tt:SimpleItemDescription Name=\"External\" Type=\"tns3:boolean\" /></tt:Data></tt:MessageDescription></Credential></AccessGranted>\r\n" +
"<AccessTaken xmlns=\"\">\r\n" +
"<Anonymous wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Anonymous>\r\n" +
"<Credential wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" /></tt:Data></tt:MessageDescription></Credential></AccessTaken>\r\n" +
"<AccessNotTaken xmlns=\"\">\r\n" +
"<Anonymous wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Anonymous>\r\n" +
"<Credential wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" /></tt:Data></tt:MessageDescription></Credential></AccessNotTaken>\r\n" +
"<Denied xmlns=\"\">\r\n" +
"<Credential wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns3:string\" />\r\n" +
"<tt:SimpleItemDescription Name=\"External\" Type=\"tns3:boolean\" /></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Anonymous wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns3:string\" />\r\n" +
"<tt:SimpleItemDescription Name=\"External\" Type=\"tns3:boolean\" /></tt:Data></tt:MessageDescription></Anonymous>\r\n" +
"<CredentialNotFound>\r\n" +
"<Card wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"tns3:string\" /></tt:Data></tt:MessageDescription></Card></CredentialNotFound></Denied>\r\n" +
"<Duress wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" />\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"tns3:string\" /></tt:Data></tt:MessageDescription></Duress>\r\n" +
"<Request xmlns=\"\">\r\n" +
"<Anonymous wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Anonymous>\r\n" +
"<Credential wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"tns2:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"tns3:string\" /></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Timeout wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Timeout></Request></tns1:AccessControl>\r\n" +
"<tns1:AccessPoint xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<Enabled wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:boolean\" /></tt:Data></tt:MessageDescription></Enabled></State></tns1:AccessPoint>\r\n" +
"<tns1:Configuration xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<AccessPoint xmlns=\"\">\r\n" +
"<Changed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Removed></AccessPoint>\r\n" +
"<Door xmlns=\"\">\r\n" +
"<Changed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Removed></Door>\r\n" +
"<Area xmlns=\"\">\r\n" +
"<Changed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AreaToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AreaToken\" Type=\"tns2:ReferenceToken\" /></tt:Source></tt:MessageDescription></Removed></Area></tns1:Configuration></TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        public const string ResponseTicket343_MessageDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<DoorMode wstop:topic=\"true\"  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription xmlns:tns2=\"http://www.onvif.org/ver10/pacs\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"\r\n" +
"\r\n" +
"</State></tns1:Door>            \r\n" +
"</TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        public const string ResponseTicket343_SourceData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<State xmlns=\"\">\r\n" +
"<DoorMode wstop:topic=\"true\"  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source xmlns:tns2=\"http://www.onvif.org/ver10/pacs\">\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" /></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"\r\n" +
"\r\n" +
"</State></tns1:Door>            \r\n" +
"</TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        public const string ResponseTicket343_SimpleItemDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
 "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
 "<soap:Header>\r\n" +
 "<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
 "<soap:Body>\r\n" +
 "<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
 "<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
 "<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
 "<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
 "<tns1:Door xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
 "<State xmlns=\"\">\r\n" +
 "<DoorMode wstop:topic=\"true\"  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
 "<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
 "<tt:Source>\r\n" +
 "<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"tns2:ReferenceToken\" xmlns:tns2=\"http://www.onvif.org/ver10/pacs\"/></tt:Source>\r\n" +
 "<tt:Data>\r\n" +
 "<tt:SimpleItemDescription Name=\"State\" Type=\"tns3:DoorMode\" xmlns:tns3=\"http://www.onvif.org/ver10/doorcontrol/wsdl\"/></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
 "\r\n" +
 "\r\n" +
 "</State></tns1:Door>            \r\n" +
 "</TopicSet>\r\n" +
 "<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
 "<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
 "<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
 "<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #452
        public const string ResponseTicket452_GetEventPropertiesResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<env:Header>\r\n" +
"<wsa:MessageID>urn:uuid:78d6c3d2-6a21-13b2-927f-00804559666f</wsa:MessageID>\r\n" +
"<wsa:RelatesTo>urn:uuid:2b3fc227-7b51-4b3f-8e1e-f99817a4c84e</wsa:RelatesTo>\r\n" +
"<wsa:To env:mustUnderstand=\"1\">http://www.w3.org/2005/08/addressing/anonymous</wsa:To>\r\n" +
"<wsa:Action env:mustUnderstand=\"1\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</wsa:Action></env:Header>\r\n" +
"<env:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnspana1=\"http://panasonic.co.jp/sn/psn/2010/event/topics\">\r\n" +
"<tns1:UserAlarm>\r\n" +
"<tnspana1:AlarmDetector>\r\n" +
"<IO1>\r\n" +
"<SignalDetected wstop:topic=\"true\">\r\n" +
"	                <tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"AlarmDetectNumber\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Source>\r\n" +
"	                  <tt:Data>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"Type\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Data>\r\n" +
"	                </tt:MessageDescription>\r\n" +
"	              </SignalDetected>\r\n" +
"	            </IO1>\r\n" +
"	          </tnspana1:AlarmDetector>\r\n" +
"	        </tns1:UserAlarm>\r\n" +
"	        <tns1:UserAlarm>\r\n" +
"	          <tnspana1:AlarmDetector>\r\n" +
"	            <IO2>\r\n" +
"	              <SignalDetected wstop:topic=\"true\">\r\n" +
"	                <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	                  <tt:Source>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"AlarmDetectNumber\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Source>\r\n" +
"	                  <tt:Data>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"Type\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Data>\r\n" +
"	                </tt:MessageDescription>\r\n" +
"	              </SignalDetected>\r\n" +
"	            </IO2>\r\n" +
"	          </tnspana1:AlarmDetector>\r\n" +
"	        </tns1:UserAlarm>\r\n" +
"	        <tns1:UserAlarm>\r\n" +
"	          <tnspana1:AlarmDetector>\r\n" +
"	            <IO3>\r\n" +
"	              <SignalDetected wstop:topic=\"true\">\r\n" +
"	                <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	                  <tt:Source>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"AlarmDetectNumber\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Source>\r\n" +
"	                  <tt:Data>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"Type\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Data>\r\n" +
"	                </tt:MessageDescription>\r\n" +
"	              </SignalDetected>\r\n" +
"	            </IO3>\r\n" +
"	          </tnspana1:AlarmDetector>\r\n" +
"	        </tns1:UserAlarm>\r\n" +
"	        <tns1:VideoAnalytics>\r\n" +
"	          <tnspana1:MotionDetector>\r\n" +
"	            <FigureChanged wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	                <tt:Source>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"VideoAnalytics\" Type=\"xsd:string\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"Type\" Type=\"xsd:string\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </FigureChanged>\r\n" +
"	          </tnspana1:MotionDetector>\r\n" +
"	        </tns1:VideoAnalytics>\r\n" +
"	        <tns1:UserAlarm>\r\n" +
"	          <tnspana1:Command>\r\n" +
"	            <Received wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	                <tt:Source>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"Alarm\" Type=\"xsd:string\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"Type\" Type=\"xsd:string\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </Received>\r\n" +
"	          </tnspana1:Command>\r\n" +
"	        </tns1:UserAlarm>\r\n" +
"	        <tns1:Device>\r\n" +
"	          <tnspana1:SD>\r\n" +
"	            <Capacity>\r\n" +
"	              <Decreased wstop:topic=\"true\">\r\n" +
"	                <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	                  <tt:Source>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"Memory\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Source>\r\n" +
"	                  <tt:Data>\r\n" +
"	                    <tt:SimpleItemDescription Name=\"Capacity\" Type=\"xsd:string\" />\r\n" +
"	                  </tt:Data>\r\n" +
"	                </tt:MessageDescription>\r\n" +
"	              </Decreased>\r\n" +
"	            </Capacity>\r\n" +
"	          </tnspana1:SD>\r\n" +
"	          <Trigger>\r\n" +
"	            <DigitalInput wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"true\">\r\n" +
"	                <tt:Source>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"InputToken\" Type=\"tt:ReferenceToken\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"LogicalState\" Type=\"xsd:boolean\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </DigitalInput>\r\n" +
"	            <Relay wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"true\">\r\n" +
"	                <tt:Source>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"RelayToken\" Type=\"tt:ReferenceToken\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"LogicalState\" Type=\"tt:RelayLogicalState\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </Relay>\r\n" +
"	          </Trigger>\r\n" +
"	        </tns1:Device>\r\n" +
"	        <tns1:RecordingHistory>\r\n" +
"	          <Recording>\r\n" +
"	            <State wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"true\">\r\n" +
"	                <tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingToken\" Type=\"tt:ReferenceToken\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"IsRecording\" Type=\"xsd:boolean\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </State>\r\n" +
"	          </Recording>\r\n" +
"	          <Track>\r\n" +
"	            <State wstop:topic=\"true\">\r\n" +
"	              <tt:MessageDescription IsProperty=\"true\">\r\n" +
"	                <tt:Source>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"RecordingToken\" Type=\"tt:ReferenceToken\" />\r\n" +
"	                  <tt:SimpleItemDescription Name=\"Track\" Type=\"tt:ReferenceToken\" />\r\n" +
"	                </tt:Source>\r\n" +
"	                <tt:Data>\r\n" +
"	                  <tt:SimpleItemDescription Name=\"IsDataPresent\" Type=\"xsd:boolean\" />\r\n" +
"	                </tt:Data>\r\n" +
"	              </tt:MessageDescription>\r\n" +
"	            </State>\r\n" +
"	          </Track>\r\n" +
"	        </tns1:RecordingHistory>\r\n" +
"	        <tns1:RecordingConfig>\r\n" +
"	          <JobState wstop:topic=\"true\">\r\n" +
"	            <tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" />\r\n" +
"	              </tt:Source>\r\n" +
"	              <tt:Data>\r\n" +
"	                <tt:SimpleItemDescription Name=\"State\" Type=\"xsd:string\" />\r\n" +
"	                <tt:ElementItemDescription Name=\"Information\" Type=\"tt:RecordingJobStateInformation\" />\r\n" +
"	              </tt:Data>\r\n" +
"	            </tt:MessageDescription>\r\n" +
"	          </JobState>\r\n" +
"	          <RecordingConfiguration wstop:topic=\"true\">\r\n" +
"	            <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	              <tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingToken\" Type=\"tt:RecordingReference\" />\r\n" +
"	              </tt:Source>\r\n" +
"	              <tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingConfiguration\" />\r\n" +
"	              </tt:Data>\r\n" +
"	            </tt:MessageDescription>\r\n" +
"	          </RecordingConfiguration>\r\n" +
"	          <TrackConfiguration wstop:topic=\"true\">\r\n" +
"	            <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	              <tt:Source>\r\n" +
"	                <tt:SimpleItemDescription Name=\"RecordingToken\" Type=\"tt:RecordingReference\" />\r\n" +
"	                <tt:SimpleItemDescription Name=\"TrackToken\" Type=\"tt:TrackReference\" />\r\n" +
"	              </tt:Source>\r\n" +
"	              <tt:Data>\r\n" +
"	                <tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:TrackConfiguration\" />\r\n" +
"	              </tt:Data>\r\n" +
"	            </tt:MessageDescription>\r\n" +
"	          </TrackConfiguration>\r\n" +
"	          <RecordingJobConfiguration wstop:topic=\"true\">\r\n" +
"	            <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	              <tt:Source>\r\n" +
"	                <tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" />\r\n" +
"	              </tt:Source>\r\n" +
"	              <tt:Data>\r\n" +
"	                <tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" />\r\n" +
"	              </tt:Data>\r\n" +
"	            </tt:MessageDescription>\r\n" +
"	          </RecordingJobConfiguration>\r\n" +
"	        </tns1:RecordingConfig>\r\n" +
"	        <tns1:VideoSource>\r\n" +
"	          <MotionAlarm wstop:topic=\"true\">\r\n" +
"	            <tt:MessageDescription IsProperty=\"false\">\r\n" +
"	              <tt:Source>\r\n" +
"	                <tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" />\r\n" +
"	              </tt:Source>\r\n" +
"	              <tt:Data>\r\n" +
"	                <tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" />\r\n" +
"	              </tt:Data>\r\n" +
"	            </tt:MessageDescription>\r\n" +
"	          </MotionAlarm>\r\n" +
"	        </tns1:VideoSource>\r\n" +
"	      </wstop:TopicSet>\r\n" +
"	      <wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"	      <wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"	      <MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"	      <MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation>\r\n" +
"	    </GetEventPropertiesResponse>\r\n" +
"	  </env:Body>\r\n" +
"	</env:Envelope>";

        #endregion

        #region Ticket #605
        //GetEventPropertiesResponse: xs is defined in Envelope
        public const string ResponseTicket605_Env = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in Body
        public const string ResponseTicket605_Body = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in GetEventPropertiesResponse
        public const string ResponseTicket605_GEPResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in TopicSet
        public const string ResponseTicket605_TopicSet = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in root Topic
        public const string ResponseTicket605_RootTopic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription  Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";


        //GetEventPropertiesResponse: xs is defined in Topic
        public const string ResponseTicket605_Topic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in MessageDescription
        public const string ResponseTicket605_MessageDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in Data
        public const string ResponseTicket605_Data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<tt:SimpleItemDescription Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //GetEventPropertiesResponse: xs is defined in SimpleItemDescription
        public const string ResponseTicket605_SimpleItemDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>true</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:RecordingConfig xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<RecordingJobConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RecordingJobToken\" Type=\"tt:RecordingJobReference\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Configuration\" Type=\"tt:RecordingJobConfiguration\" /></tt:Data></tt:MessageDescription></RecordingJobConfiguration></tns1:RecordingConfig>\r\n" +
"<tns1:Monitoring xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<ProcessorUsage wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" Name=\"Value\" Type=\"xs:float\" /></tt:Data></tt:MessageDescription></ProcessorUsage></tns1:Monitoring></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        //PM: topic prefix is defined in the Envelop
        public const string ResponseTicket605_PMEnv = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";

        //PM: topic prefix is defined in the Body
        public const string ResponseTicket605_PMBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        //PM: topic prefix is defined in the PM response
        public const string ResponseTicket605_PMResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        //PM: topic prefix is defined in the NotificationMessage
        public const string ResponseTicket605_PMNotificationMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";

        //PM: topic prefix is defined in the Topic
        public const string ResponseTicket605_PMTopic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope  xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic  xmlns:tns1=\"http://www.onvif.org/ver10/topics\" Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        //PM: no topic prefix is defined
        public const string ResponseTicket605_PMnoPrefix = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-29T08:32:54.5807062Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-29T08:33:34.5807062Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        //PM: prefix tns1 is redefined within PM
        public const string ResponseTicket605_PMredefined_tns1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2014-09-30T11:23:52.4751419Z</CurrentTime>\r\n" +
"<TerminationTime>2014-09-30T11:33:52.4751419Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns:tns3=\"http://www.onvif.org/ver10/topics3\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic xmlns:tns2=\"http://www.onvif.org/ver10/topics\" Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns2:Monitoring/ProcessorUsage</Topic>\r\n" +
"<ProducerReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:3246/Events/PullPointServiceFake.asmx?param=value</Address></ProducerReference>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2012-12-01T07:18:40.321\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Token\" Value=\"Token1\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"Value\" Value=\"0.5\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #717
        public const string GetServiceCapabilities_with_missed_attribute = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\">\r\n" +
"<Capabilities MaxAccessProfiles=\"1\" MaxAccessPoliciesPerAccessProfile=\"2\" MultipleSchedulesPerAccessPointSupported=\"true\" anyAttribute=\"any\">\r\n" +
"\r\n" +
"</Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";

        public const string GetServiceCapabilities_with_missed_boolean_attribute = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"1\" MaxAccessProfiles=\"1\" MaxAccessPoliciesPerAccessProfile=\"2\"></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";

        public const string GetServiceCapabilities_with_two_tags = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"10\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" MultipleSchedulesPerAccessPointSupported=\"true\"></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";

        public const string GetServices_with_missed_attribute = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/accessrules/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceAccessRules10/AccessRulesService.asmx</XAddr>\r\n" +
"<Capabilities>\r\n" +
"<tar:Capabilities xmlns:tar=\"http://www.onvif.org/ver10/accessrules/wsdl\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" MultipleSchedulesPerAccessPointSupported=\"true\"></tar:Capabilities></Capabilities>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>";

        public const string GetServices_with_one_tag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/accessrules/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceAccessRules10/AccessRulesService.asmx</XAddr>\r\n" +
"<Capabilities>\r\n" +
"<tar:Capabilities xmlns:tar=\"http://www.onvif.org/ver10/accessrules/wsdl\" MaxLimit=\"10\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" MultipleSchedulesPerAccessPointSupported=\"true\"/>\r\n" +
"\r\n" +
"</Capabilities>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #711
        public const string ResponseTicket711_GetServiceCapabilities_with_missed_attribute = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"3\" CredentialAccessProfileValiditySupported=\"true\" MaxCredentials=\"10\" MaxAccessProfilesPerCredential=\"5\" ResetAntipassbackSupported=\"true\">\r\n" +
"<SupportedIdentifierType>ONVIFCard</SupportedIdentifierType>\r\n" +
"\r\n" +
"</Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";

        #endregion

        #region Ticket #719
        public const string ResponseTicket719_GetAccessProfileList_invalid_EntityType = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetAccessProfileListResponse xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\">\r\n" +
"<AccessProfile token=\"accessprofile1\">\r\n" +
"<Name>Access Profile 1</Name>\r\n" +
"<Description>Access Profile Description 1</Description>\r\n" +
"<AccessPolicy>\r\n" +
"<ScheduleToken>schedule1</ScheduleToken>\r\n" +
"<Entity>tokenAccessPoint1</Entity>\r\n" +
"<EntityType>q1:AccessPoint</EntityType></AccessPolicy>        \r\n" +
"</AccessProfile>     \r\n" +
"</GetAccessProfileListResponse></soap:Body></soap:Envelope>";


        #endregion

         #region Ticket #713
        public const string ResponseTicket713_GetCredentialList_invalid_CredentialIdentifierValue = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetCredentialListResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\">\r\n" +
"<NextStartReference />\r\n" +
"<Credential token=\"Credential1\" attribut=\"any\">\r\n" +
"<Description>Access Profile 1 Description</Description>\r\n" +
"<CredentialHolderReference>CredentialHolderReference</CredentialHolderReference>\r\n" +
"<ValidFrom>2014-01-01T19:00:00+04:00</ValidFrom>\r\n" +
"<ValidTo>2015-01-01T18:00:00+03:00</ValidTo>\r\n" +
"<CredentialIdentifier>\r\n" +
"<Type>\r\n" +
"<Name>ONVIFCard</Name></Type>\r\n" +
"<ExemptedFromAuthentication>true</ExemptedFromAuthentication>\r\n" +
"<CredentialIdentifierValue>\r\n" +
"<Value>====</Value></CredentialIdentifierValue></CredentialIdentifier></Credential></GetCredentialListResponse></soap:Body></soap:Envelope>";
#endregion

        #region CREDENTIAL-8-1-3
        public const string GetEventProperties_namespase_in_TopicSet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Header><a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header><soap:Body><GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\"><TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation><FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet><TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:pt=\"http://www.onvif.org/ver10/pacs\"><tns1:Configuration xmlns:tns1=\"http://www.onvif.org/ver10/topics\"><Credential xmlns=\"\"><Removed wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\"><tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><tt:Source><tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\" /></tt:Source></tt:MessageDescription></Removed></Credential></tns1:Configuration></TopicSet><TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect><TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect><MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect><MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        #endregion

        #region CREDENTIAL-8-1-4
        public const string Credential_814_GetEventProperties_namespase_in_TopicSet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Header><a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header><soap:Body><GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\"><TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation><FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet><TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\"><tns1:Credential xmlns:tns1=\"http://www.onvif.org/ver10/topics\"><State xmlns=\"\"><Enabled wstop:topic=\"true\" xmlns:pt=\"http://www.onvif.org/ver10/pacs\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" ><tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"><tt:Source><tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\" /></tt:Source><tt:Data><tt:SimpleItemDescription Name=\"State\" Type=\"xs:boolean\" /><tt:SimpleItemDescription Name=\"Reason\" Type=\"xs:string\" /></tt:Data></tt:MessageDescription></Enabled></State></tns1:Credential></TopicSet><TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect><TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect><MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect><MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";

        #endregion

        #region Ticket #879 (CREDENTIAL-1-1-1)
        public const string Cr_111_GetServiceCapabilities_ValiditySupportsTimeValueisTrue = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\"><Capabilities MaxLimit=\"3\" CredentialValiditySupported=\"true\" CredentialAccessProfileValiditySupported=\"true\" ValiditySupportsTimeValue=\"True\" MaxCredentials=\"10\" MaxAccessProfilesPerCredential=\"5\" ResetAntipassbackSupported=\"true\"><SupportedIdentifierType>pt:Card</SupportedIdentifierType><SupportedIdentifierType>pt:PIN</SupportedIdentifierType><SupportedIdentifierType>pt:Fingerprint</SupportedIdentifierType><SupportedIdentifierType>pt:Face</SupportedIdentifierType><SupportedIdentifierType>pt:Iris</SupportedIdentifierType><SupportedIdentifierType>pt:Vein</SupportedIdentifierType></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #879 (CREDENTIAL-1-1-1) 2
        public const string Cr_111_GetServiceCapabilities_ValiditySupportsTimeValueSkipped = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\"><Capabilities MaxLimit=\"3\" CredentialValiditySupported=\"true\" CredentialAccessProfileValiditySupported=\"true\" MaxCredentials=\"10\" MaxAccessProfilesPerCredential=\"5\" ResetAntipassbackSupported=\"true\"><SupportedIdentifierType>pt:Card</SupportedIdentifierType><SupportedIdentifierType>pt:PIN</SupportedIdentifierType><SupportedIdentifierType>pt:Fingerprint</SupportedIdentifierType><SupportedIdentifierType>pt:Face</SupportedIdentifierType><SupportedIdentifierType>pt:Iris</SupportedIdentifierType><SupportedIdentifierType>pt:Vein</SupportedIdentifierType></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region SCHEDULE-1-1-1 
        public const string Sch111_GetSvcCap_MaxLimitSkipped = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\"><Capabilities MaxSchedules=\"3\" MaxTimePeriodsPerDay=\"10\" MaxSpecialDayGroups=\"2\" MaxSpecialDaysInSpecialDayGroup=\"1\" MaxSpecialDaysSchedules=\"1\" ExtendedRecurrenceSupported=\"false\" SpecialDaysSupported=\"true\" StateReportingSupported=\"false\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region SCHEDULE-1-1-1
        public const string Sch111_GetSvcCap_MaxLimitMinusOne = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\"><Capabilities MaxLimit=\"-1\" MaxSchedules=\"3\" MaxTimePeriodsPerDay=\"10\" MaxSpecialDayGroups=\"2\" MaxSpecialDaysInSpecialDayGroup=\"1\" MaxSpecialDaysSchedules=\"1\" ExtendedRecurrenceSupported=\"false\" SpecialDaysSupported=\"true\" StateReportingSupported=\"false\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Features discovery: Credential
        public const string CredentialValiditySkipped = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\"><Capabilities MaxLimit=\"3\" CredentialAccessProfileValiditySupported=\"false\" ValiditySupportsTimeValue=\"false\" MaxCredentials=\"10\" MaxAccessProfilesPerCredential=\"5\" ResetAntipassbackSupported=\"false\"><SupportedIdentifierType>pt:Card</SupportedIdentifierType><SupportedIdentifierType>pt:PIN</SupportedIdentifierType><SupportedIdentifierType>pt:Fingerprint</SupportedIdentifierType><SupportedIdentifierType>pt:Face</SupportedIdentifierType><SupportedIdentifierType>pt:Iris</SupportedIdentifierType></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Features discovery: Credential
        public const string CredentialAccessProfileValidityInvalid = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/credential/wsdl\"><Capabilities MaxLimit=\"3\" CredentialValiditySupported=\"false\" CredentialAccessProfileValiditySupported=\"TRUE\" ValiditySupportsTimeValue=\"false\" MaxCredentials=\"10\" MaxAccessProfilesPerCredential=\"5\" ResetAntipassbackSupported=\"false\"><SupportedIdentifierType>pt:Card</SupportedIdentifierType><SupportedIdentifierType>pt:PIN</SupportedIdentifierType><SupportedIdentifierType>pt:Fingerprint</SupportedIdentifierType><SupportedIdentifierType>pt:Face</SupportedIdentifierType><SupportedIdentifierType>pt:Iris</SupportedIdentifierType></Capabilities></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Features discovery: Access Rules
        public const string MultipleSkipped = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\"><Capabilities MaxLimit=\"10\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Namespase prefix: Schedule
        public const string SchedulePrefixInCapabilities = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><Service><Namespace>http://www.onvif.org/ver10/device/wsdl</Namespace><XAddr>http://localhost:17934/ServiceDevice10/DeviceServiceFake.asmx</XAddr><Version><Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major><Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service><Service><Namespace>http://www.onvif.org/ver10/schedule/wsdl</Namespace><XAddr>http://localhost:17934/ServiceSchedule10/ServiceSchedule.asmx</XAddr><Capabilities xmlns:new=\"http://www.onvif.org/ver10/schedule/wsdl\"><new:Capabilities MaxLimit=\"3\" MaxSchedules=\"3\" MaxTimePeriodsPerDay=\"10\" MaxSpecialDayGroups=\"2\" MaxSpecialDaysInSpecialDayGroup=\"1\" MaxSpecialDaysSchedules=\"1\" ExtendedRecurrenceSupported=\"false\" SpecialDaysSupported=\"true\" StateReportingSupported=\"false\"></new:Capabilities></Capabilities><Version><Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major><Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region SCHEDULE-6-1-1
        public const string ActiveSkipped = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetScheduleStateResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\"><ScheduleState><SpecialDay>true</SpecialDay></ScheduleState></GetScheduleStateResponse></soap:Body></soap:Envelope>";
        #endregion

        #region SCHEDULE-6-1-2
        public const string ActiveNotBool = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetScheduleStateResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\"><ScheduleState><Active>True</Active><SpecialDay>false</SpecialDay></ScheduleState></GetScheduleStateResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Hikvision RECORDING-5-1-4
        public const string PullMessage_Hikvision_001 = "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:trec=\"http://www.onvif.org/ver10/recording/wsdl\">\r\n" +
"<soap:Header>\r\n" +
"<wsa:Action>http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<tev:PullMessagesResponse>\r\n" +
"<tev:CurrentTime>2015-07-06T02:11:31Z</tev:CurrentTime>\r\n" +
"<tev:TerminationTime>2015-07-06T02:21:50Z</tev:TerminationTime>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingConfig/TrackConfiguration</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2015-07-06T02:11:30Z\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"RecordingToken001\" />\r\n" +
"<tt:SimpleItem Name=\"TrackToken\" Value=\"VIDEO001\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItem Name=\"Configuration\">\r\n" +
"<tt:TrackConfiguration>\r\n" +
"<tt:TrackType>Video</tt:TrackType>\r\n" +
"<tt:Description>A</tt:Description></tt:TrackConfiguration></tt:ElementItem></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region FW

        public const string GetDeviceInformation_NamespaceTest1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<Envelope xmlns=\"http://www.w3.org/2003/05/soap-envelope\"\r\n" +
"xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\"\r\n" +
"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n" +
"xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"\r\n" +
"xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\"\r\n" +
"xmlns:xop=\"http://www.w3.org/2004/08/xop/include\"\r\n" +
"xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\"\r\n" +
"xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Body>\r\n" +
"<GetDeviceInformationResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"<Manufacturer>Panasonic</Manufacturer>\r\n" +
"<Model>WV-SW155</Model>\r\n" +
"<tds:FirmwareVersion>1.66</tds:FirmwareVersion>\r\n" +
"<tds:SerialNumber>KIV32062</tds:SerialNumber>\r\n" +
"<tds:HardwareId>00</tds:HardwareId></GetDeviceInformationResponse></Body></Envelope>";

        #endregion

        #region #1119

        public const string GetCapabilities_1119_ticket = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"<Capabilities>\r\n" +
"<Events xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<XAddr>http://localhost:17934/ServiceEvents10/EventService.asmx</XAddr>\r\n" +
"<WSSubscriptionPolicySupport>false</WSSubscriptionPolicySupport>\r\n" +
"<WSPullPointSupport>false</WSPullPointSupport>\r\n" +
"<WSPausableSubscriptionManagerInterfaceSupport>false</WSPausableSubscriptionManagerInterfaceSupport></Events>\r\n" +
"<Extension xmlns=\"http://www.onvif.org/ver10/schema\">         \r\n" +
"<Receiver>\r\n" +
"<XAddr>http://localhost:17934/ServiceReceiver10/ReceiverService.asmx</XAddr>\r\n" +
"<RTP_Multicast>true</RTP_Multicast>\r\n" +
"<RTP_TCP>true</RTP_TCP>\r\n" +
"<RTP_RTSP_TCP>true</RTP_RTSP_TCP>\r\n" +
"<SupportedReceivers>17</SupportedReceivers>\r\n" +
"<MaximumRTSPURILength>1024</MaximumRTSPURILength></Receiver>\r\n" +
"<Display>\r\n" +
"<XAddr>http://192.168.250.160:8884/onvif/display_service</XAddr>\r\n" +
"<FixedLayout>false</FixedLayout></Display></Extension></Capabilities></GetCapabilitiesResponse></soap:Body></soap:Envelope>";

        #endregion

        #region Ticket #1299
        public const string ResponseTicket1299_InvalidGetVideoSourceConfigurationOptionsResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetVideoSourceConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Options>\r\n" +
"<BoundsRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<XRange>\r\n" +
"<Min>1</Min>\r\n" +
"<Max>1920</Max></XRange>\r\n" +
"<YRange>\r\n" +
"<Min>1</Min>\r\n" +
"<Max>1080</Max></YRange>\r\n" +
"<WidthRange>\r\n" +
"<Min>192</Min>\r\n" +
"<Max>1280</Max></WidthRange>\r\n" +
"<HeightRange>\r\n" +
"<Min>96</Min>\r\n" +
"<Max>1024</Max></HeightRange></BoundsRange>\r\n" +
"<VideoSourceTokensAvailable xmlns=\"http://www.onvif.org/ver10/schema\">VSC1</VideoSourceTokensAvailable>\r\n" +
"<Extension xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Rotate>\r\n" +
"<Mode>ON</Mode>\r\n" +
"<DegreeList>\r\n" +
"<Items>11</Items>\r\n" +
"<Items>10</Items></DegreeList>\r\n" +
"<Extension>\r\n" +
"<tt:BoundsRange xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:XRange>\r\n" +
"<tt:Min>1</tt:Min>\r\n" +
"<tt:Max>1920</tt:Max></tt:XRange>\r\n" +
"<tt:YRange>\r\n" +
"<tt:Min>1</tt:Min>\r\n" +
"<tt:Max>1080</tt:Max></tt:YRange>\r\n" +
"<tt:WidthRange>\r\n" +
"<tt:Min>192</tt:Min>\r\n" +
"<tt:Max>1280</tt:Max></tt:WidthRange>\r\n" +
"<tt:HeightRange>\r\n" +
"<tt:Min>96</tt:Min>\r\n" +
"<tt:Max>1024</tt:Max></tt:HeightRange></tt:BoundsRange></Extension></Rotate>\r\n" +
"<Extension>\r\n" +
"<SceneOrientationMode>MANUAL</SceneOrientationMode>\r\n" +
"<SceneOrientationMode>invalid value</SceneOrientationMode>\r\n" +
"<tt:Rotate xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Mode>ON</tt:Mode>\r\n" +
"<tt:DegreeList>\r\n" +
"<tt:Items>11</tt:Items>\r\n" +
"<tt:Items>10</tt:Items></tt:DegreeList>\r\n" +
"<tt:Extension>\r\n" +
"<tt:BoundsRange>\r\n" +
"<tt:XRange>\r\n" +
"<tt:Min>1</tt:Min>\r\n" +
"<tt:Max>1920</tt:Max></tt:XRange>\r\n" +
"<tt:YRange>\r\n" +
"<tt:Min>1</tt:Min>\r\n" +
"<tt:Max>1080</tt:Max></tt:YRange>\r\n" +
"<tt:WidthRange>\r\n" +
"<tt:Min>192</tt:Min>\r\n" +
"<tt:Max>1280</tt:Max></tt:WidthRange>\r\n" +
"<tt:HeightRange>\r\n" +
"<tt:Min>96</tt:Min>\r\n" +
"<tt:Max>1024</tt:Max></tt:HeightRange></tt:BoundsRange></tt:Extension></tt:Rotate></Extension></Extension></Options></GetVideoSourceConfigurationOptionsResponse></soap:Body></soap:Envelope>";

        #endregion

        #region Ticket #1307
        public const string ResponseTicket1307_ProfileChangedEvent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:chan=\"http://schemas.microsoft.com/ws/2005/02/duplex\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:xenc=\"http://www.w3.org/2001/04/xmlenc#\" xmlns:wsc=\"http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:xmime=\"http://www.w3.org/2005/05/xmlmime\" xmlns:tev4=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:tev6=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:CC=\"http://www.canon.com/ns/networkcamera/onvif/va/schema\" xmlns:tev7=\"http://www.onvif.org/ver10/schema\" xmlns:tev8=\"http://www.w3.org/2004/08/xop/include\" xmlns:tev3=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:tev10=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev11=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:tev12=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev13=\"http://www.onvif.org/ver10/events/wsdl/NotificationConsumerBinding\" xmlns:tev14=\"http://www.onvif.org/ver10/events/wsdl/PullPointBinding\" xmlns:tev15=\"http://www.onvif.org/ver10/events/wsdl/CreatePullPointBinding\" xmlns:tev16=\"http://www.onvif.org/ver10/events/wsdl/PausableSubscriptionManagerBinding\" xmlns:tev5=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev9=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:CT=\"http://www.canon.com/ns/networkcamera/onvif/va/topic/schema\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\"><SOAP-ENV:Header><wsa5:RelatesTo>urn:uuid:842f4015-d1ff-4ad6-a185-1b113fff9d86</wsa5:RelatesTo><wsa5:Action>http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa5:Action></SOAP-ENV:Header><SOAP-ENV:Body><tev:PullMessagesResponse><tev:CurrentTime>2017-01-24T01:38:11.000000Z</tev:CurrentTime><tev:TerminationTime>2017-01-24T02:38:11.000000Z</tev:TerminationTime><tev5:NotificationMessage><tev5:SubscriptionReference><wsa5:Address>http://10.255.185.228:80/onvif/event_service?id=0bmqbh</wsa5:Address></tev5:SubscriptionReference><tev5:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Media/ProfileChanged</tev5:Topic><tev5:ProducerReference><wsa5:Address>http://10.255.185.228:80/onvif/event_service</wsa5:Address></tev5:ProducerReference><tev5:Message><tev7:Message PropertyOperation=\"Initialized\" UtcTime=\"2017-01-24T01:38:11.313726Z\"><tev7:Source><tev7:SimpleItem Value=\"profile0\" Name=\"Token\"></tev7:SimpleItem></tev7:Source><tev7:Key></tev7:Key><tev7:Data></tev7:Data></tev7:Message></tev5:Message></tev5:NotificationMessage></tev:PullMessagesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1358
        public const string ResponseTicket1356_ScheduleFromSimulator = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"+
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n"+
"<soap:Body>\r\n"+
"<GetSpecialDayGroupInfoListResponse xmlns=\"http://www.onvif.org/ver10/schedule/wsdl\">\r\n"+
"<SpecialDayGroupInfo token=\"specialdaygroup1\">\r\n"+
"<Name>special day group days name</Name>\r\n"+
"<Description>special day group days description</Description>\r\n"+
"<Days>BEGIN:VCALENDAR</Days></SpecialDayGroupInfo>\r\n"+
"<SpecialDayGroupInfo token=\"specialdaygroup2\">\r\n"+
"<Name>special day group days name</Name>\r\n"+
"<Description>special day group days description</Description>\r\n"+
"<Days>BEGIN:VCALENDAR</Days></SpecialDayGroupInfo></GetSpecialDayGroupInfoListResponse></soap:Body></soap:Envelope> ";
        #endregion

        #region Ticket #1358
        public const string ResponseTicket1356_ScheduleFromTicket = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"+
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:tsc=\"http://www.onvif.org/ver10/schedule/wsdl\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\">\r\n"+
"<env:Header />\r\n"+
"<env:Body>\r\n"+
"<tsc:GetSpecialDayGroupInfoListResponse>\r\n"+
"<tsc:SpecialDayGroupInfo token=\"1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"tsc:SpecialDayGroup\">\r\n"+
"<tsc:Name>Holidays</tsc:Name>\r\n"+
"<tsc:Description>Swedish holidays</tsc:Description>\r\n"+
"<tsc:Days>BEGIN:VCALENDAR END:VCALENDAR</tsc:Days></tsc:SpecialDayGroupInfo>\r\n"+
"<tsc:SpecialDayGroupInfo token=\"2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"tsc:SpecialDayGroup\">\r\n"+
"<tsc:Name>Halfdays</tsc:Name>\r\n"+
"<tsc:Description>Swedish half-working days</tsc:Description>\r\n"+
"<tsc:Days>BEGIN:VCALENDAR</tsc:Days></tsc:SpecialDayGroupInfo></tsc:GetSpecialDayGroupInfoListResponse></env:Body></env:Envelope>";
        #endregion

         #region Ticket #1228
        public const string ResponseTicket1228_FrameRatesSupported_Empty = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetVideoEncoderConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Options GovLengthRange=\"0 10\" FrameRatesSupported=\"\" ProfilesSupported=\"Simple\" ConstantBitRateSupported=\"true\">\r\n" +
"<Encoding xmlns=\"http://www.onvif.org/ver10/schema\">H264</Encoding>\r\n" +
"<QualityRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></QualityRange>\r\n" +
"<ResolutionsAvailable xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Width>40</Width>\r\n" +
"<Height>80</Height></ResolutionsAvailable>\r\n" +
"<BitrateRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></BitrateRange></Options></GetVideoEncoderConfigurationOptionsResponse></soap:Body></soap:Envelope>";
       
        public const string ResponseTicket1228_ConstantBitRateSupported_Empty = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetVideoEncoderConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Options GovLengthRange=\"0 10\" FrameRatesSupported=\"10\" ProfilesSupported=\"Simple\" ConstantBitRateSupported=\"\">\r\n" +
"<Encoding xmlns=\"http://www.onvif.org/ver10/schema\">H264</Encoding>\r\n" +
"<QualityRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></QualityRange>\r\n" +
"<ResolutionsAvailable xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Width>40</Width>\r\n" +
"<Height>80</Height></ResolutionsAvailable>\r\n" +
"<BitrateRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></BitrateRange></Options></GetVideoEncoderConfigurationOptionsResponse></soap:Body></soap:Envelope>";
      

        public const string ResponseTicket1228_GovLengthRange_Empty = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetVideoEncoderConfigurationOptionsResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Options GovLengthRange=\"\" FrameRatesSupported=\"10\" ProfilesSupported=\"Simple\" ConstantBitRateSupported=\"true\">\r\n" +
"<Encoding xmlns=\"http://www.onvif.org/ver10/schema\">H264</Encoding>\r\n" +
"<QualityRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></QualityRange>\r\n" +
"<ResolutionsAvailable xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Width>40</Width>\r\n" +
"<Height>80</Height></ResolutionsAvailable>\r\n" +
"<BitrateRange xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Min>0</Min>\r\n" +
"<Max>100</Max></BitrateRange></Options></GetVideoEncoderConfigurationOptionsResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1342
        public const string ResponseTicket1342_SessionTimeoutRange_Empty = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/replay/wsdl\">\r\n" +
"<Capabilities SessionTimeoutRange=\"\" RTP_RTSP_TCP=\"false\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket 1341 GetDynamicDNS
        public const string ResponseTicket1341_DynamicDNSInformation_Empty = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+
            "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "+
            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetDynamicDNSResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">"+
            "<DynamicDNSInformation></DynamicDNSInformation></GetDynamicDNSResponse>"+
            "</soap:Body></soap:Envelope>";
        public const string ResponseTicket1341_DynamicDNSInformation_NoType = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetDynamicDNSResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">" +
            "<DynamicDNSInformation><Name xmlns=\"http://www.onvif.org/ver10/schema\">name</Name><TTL xmlns=\"http://www.onvif.org/ver10/schema\">15</TTL></DynamicDNSInformation></GetDynamicDNSResponse>" +
            "</soap:Body></soap:Envelope>";
        public const string ResponseTicket1341_DynamicDNSInformation_WrongType = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><GetDynamicDNSResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">" +
            "<DynamicDNSInformation><Type xmlns=\"http://www.onvif.org/ver10/schema\">TESTTEST</Type><Name xmlns=\"http://www.onvif.org/ver10/schema\">name</Name><TTL xmlns=\"http://www.onvif.org/ver10/schema\">15</TTL></DynamicDNSInformation></GetDynamicDNSResponse>" +
            "</soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1185 xsiType
        public const string ResponseTicket1185_xsiType = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:chan=\"http://schemas.microsoft.com/ws/2005/02/duplex\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:xenc=\"http://www.w3.org/2001/04/xmlenc#\" xmlns:wsc=\"http://schemas.xmlsoap.org/ws/2005/02/sc\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:oncam=\"http://www.oncamgrandeye.com/2015/schema\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsdd=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tae=\"http://www.onvif.org/ver10/actionengine/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:ons1=\"http://www.oncamgrandeye.com/2015/topics\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tan:GetRuleOptionsResponse>\r\n" +
"<tan:RuleOptions Type=\"axt:MotionRegionConfigOptions\" Name=\"MotionRegion\" RuleType=\"axt:MotionRegionConfig\">\r\n" +
"<axt:MotionRegionConfigOptions xsi:type=\"axt:MotionRegionConfigOptions\">\r\n" +
"<axt:MaxRegions>16</axt:MaxRegions>\r\n" +
"<axt:DisarmSupport>false</axt:DisarmSupport>\r\n" +
"<axt:PolygonSupport>true</axt:PolygonSupport>\r\n" +
"<axt:PolygonLimits>\r\n" +
"<tt:Min>3</tt:Min>\r\n" +
"<tt:Max>19</tt:Max></axt:PolygonLimits>\r\n" +
"<axt:SingleSensitivitySupport>true</axt:SingleSensitivitySupport>\r\n" +
"<axt:RuleNotification>true</axt:RuleNotification></axt:MotionRegionConfigOptions></tan:RuleOptions></tan:GetRuleOptionsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1405 InvalidRemoveConfigurationResponse
        public const string ResponseTicket1405_RemoveConfigurationResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<RemoveResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesOneLine
        public const string ResponseTicket1405_Media2CapabilitiesOneLine = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\"><env:Body> <tr2:GetServiceCapabilitiesResponse> <tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">   <tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>        <tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities>      </tr2:Capabilities>    </tr2:GetServiceCapabilitiesResponse> </env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesNamespaceInResponseTag
        public const string ResponseTicket1405_Media2CapabilitiesNamespaceInResponseTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<tr2:GetServiceCapabilitiesResponse xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\r\n" +
"<tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>\r\n" +
"<tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tr2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesNamespaceInBodyTag
        public const string ResponseTicket1405_Media2CapabilitiesNamespaceInBodyTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<tr2:GetServiceCapabilitiesResponse>\r\n" +
"<tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\r\n" +
"<tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>\r\n" +
"<tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tr2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesCustomNamespacePrefix
        public const string ResponseTicket1405_Media2CapabilitiesCustomNamespacePrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body xmlns:m2=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<m2:GetServiceCapabilitiesResponse>\r\n" +
"<m2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\r\n" +
"<m2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></m2:ProfileCapabilities>\r\n" +
"<m2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></m2:StreamingCapabilities></m2:Capabilities></m2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesNoNamespacePrefix
        public const string ResponseTicket1405_Media2CapabilitiesNoNamespacePrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesNullSimbol
        public const string ResponseTicket1405_Media2CapabilitiesNullSimbol = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\0" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_Media2CapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<GetServiceCapabilitiesResponse2 xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse2></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesLineFeedSimbol
        public const string ResponseTicket1405_Media2CapabilitiesLineFeedSimbol = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\n" +
"<env:Body xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\">\n" +
"<tr2:GetServiceCapabilitiesResponse>\n" +
"<tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\n" +
"<tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>\n" +
"<tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tr2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesLineTabulationSimbol
        public const string ResponseTicket1405_Media2CapabilitiesLineTabulationSimbol = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\v" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\v" +
"<env:Body xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\">\v" +
"<tr2:GetServiceCapabilitiesResponse>\v" +
"<tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\v" +
"<tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>\v" +
"<tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tr2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesCarriageReturnSimbol
        public const string ResponseTicket1405_Media2CapabilitiesCarriageReturnSimbol = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r" +
"<env:Body xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\">\r" +
"<tr2:GetServiceCapabilitiesResponse>\r" +
"<tr2:Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\">\r" +
"<tr2:ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\"></tr2:ProfileCapabilities>\r" +
"<tr2:StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tr2:GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesBackspaceSimbol
        public const string ResponseTicket1405_Media2CapabilitiesBackspaceSimbol = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\b" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\b" +
"<env:Body>\b" +
"<GetServiceCapabilitiesResponse2 xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\b" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse2></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesIncorrectNamepace
        public const string ResponseTicket1405_Media2CapabilitiesIncorrectNamepace = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\">\r\n" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 Media2CapabilitiesNamepaceNotDeclarated
        public const string ResponseTicket1405_Media2CapabilitiesNamepaceNotDeclarated = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\">\r\n" +
"<env:Body>\r\n" +
"<GetServiceCapabilitiesResponse>\r\n" +
"<Capabilities SnapshotUri=\"true\" Rotation=\"false\" VideoSourceMode=\"true\" OSD=\"true\" Mask=\"true\" SourceMask=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<ProfileCapabilities MaximumNumberOfProfiles=\"10\" ConfigurationsSupported=\"VideoSource VideoEncoder AudioSource AudioEncoder AudioOutput AudioDecoder Metadata Analytics PTZ\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></ProfileCapabilities>\r\n" +
"<StreamingCapabilities RTSPStreaming=\"true\" RTPMulticast=\"true\" RTP_RTSP_TCP=\"true\" NonAggregateControl=\"false\" AutoStartMulticast=\"true\" xmlns=\"http://www.onvif.org/ver20/media/wsdl\"></StreamingCapabilities></Capabilities></GetServiceCapabilitiesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1405 DeviceCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_DeviceCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:acert=\"http://www.axis.com/vapix/ws/cert\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:acertificates=\"http://www.axis.com/vapix/ws/certificates\" xmlns:aentry=\"http://www.axis.com/vapix/ws/entry\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:aeva=\"http://www.axis.com/vapix/ws/embeddedvideoanalytics1\" xmlns:ali1=\"http://www.axis.com/vapix/ws/light/CommonBinding\" xmlns:ali2=\"http://www.axis.com/vapix/ws/light/IntensityBinding\" xmlns:ali3=\"http://www.axis.com/vapix/ws/light/AngleOfIlluminationBinding\" xmlns:ali4=\"http://www.axis.com/vapix/ws/light/DayNightSynchronizeBinding\" xmlns:ali=\"http://www.axis.com/vapix/ws/light\" xmlns:apc=\"http://www.axis.com/vapix/ws/panopsiscalibration1\" xmlns:arth=\"http://www.axis.com/vapix/ws/recordedtour1\" xmlns:ascm=\"http://www.axis.com/vapix/ws/siblingcameramonitor1\" xmlns:asd=\"http://www.axis.com/vapix/ws/shockdetection\" xmlns:aweb=\"http://www.axis.com/vapix/ws/webserver\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tds:GetServicesResponse>\r\n" +
"<tds:Capabilities>\r\n" +
"<tds:Network DHCPv6=\"true\" NTP=\"1\" DynDNS=\"true\" IPVersion6=\"true\" ZeroConfiguration=\"true\" IPFilter=\"true\"></tds:Network>\r\n" +
"<tds:Security HttpDigest=\"true\" UsernameToken=\"true\" DefaultAccessPolicy=\"true\" AccessPolicyConfig=\"true\" OnboardKeyGeneration=\"true\" TLS1.2=\"true\" TLS1.1=\"true\" TLS1.0=\"true\"></tds:Security>\r\n" +
"<tds:System SystemLogging=\"true\" DiscoveryBye=\"true\" DiscoveryResolve=\"true\"></tds:System></tds:Capabilities></tds:GetServicesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1405 DeviceCapabilitiesGetServicesResponseInsteadOfGetServiceCapabilitiesResponse
        public const string ResponseTicket1405_DeviceCapabilitiesGetServicesResponseInsteadOfGetServiceCapabilitiesResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServicesResponse xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/device/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceDevice10/DeviceServiceFake.asmx</XAddr>\r\n" +
"<Capabilities>\r\n" +
"<tds:Capabilities xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"<tds:Network IPFilter=\"true\" ZeroConfiguration=\"true\" IPVersion6=\"true\" DynDNS=\"true\" Dot11Configuration=\"true\" HostnameFromDHCP=\"true\" NTP=\"1\" />\r\n" +
"<tds:Security TLS1.0=\"true\" TLS1.1=\"true\" TLS1.2=\"true\" OnboardKeyGeneration=\"true\" AccessPolicyConfig=\"true\" DefaultAccessPolicy=\"true\" Dot1X=\"true\" RemoteUserHandling=\"true\" X.509Token=\"true\" SAMLToken=\"true\" KerberosToken=\"true\" UsernameToken=\"true\" HttpDigest=\"true\" RELToken=\"true\" SupportedEAPMethods=\"1 2 3\" />\r\n" +
"<tds:System DiscoveryResolve=\"true\" DiscoveryBye=\"true\" RemoteDiscovery=\"true\" SystemBackup=\"true\" SystemLogging=\"true\" FirmwareUpgrade=\"true\" HttpFirmwareUpgrade=\"true\" HttpSystemBackup=\"true\" HttpSystemLogging=\"true\" HttpSupportInformation=\"true\" />\r\n" +
"<tds:Misc AuxiliaryCommands=\"\" /></tds:Capabilities></Capabilities>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service>\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/media/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceMedia10/MediaService.asmx</XAddr>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service>\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/events/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceEvents10/EventService.asmx</XAddr>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service>\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver20/ptz/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServicePTZ20/PtzService.asmx</XAddr>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service>\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver20/imaging/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost:17934/ServiceImaging20/ImagingService20.asmx</XAddr>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service>\r\n" +
"<Service>\r\n" +
"<Namespace>http://www.onvif.org/ver10/ver20/analytics/wsdl</Namespace>\r\n" +
"<XAddr>http://localhost/onvif/ServiceDevice10/DeviceServiceFake.asmx</XAddr>\r\n" +
"<Version>\r\n" +
"<Major xmlns=\"http://www.onvif.org/ver10/schema\">2</Major>\r\n" +
"<Minor xmlns=\"http://www.onvif.org/ver10/schema\">1</Minor></Version></Service></GetServicesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 DeviceService_SetNetworkDefaultGatewayResponseInsteadOfSetHostnameResponse
        public const string ResponseTicket1405_DeviceService_SetNetworkDefaultGatewayResponseInsteadOfSetHostnameResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:acert=\"http://www.axis.com/vapix/ws/cert\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:aa=\"http://www.axis.com/vapix/ws/action1\" xmlns:acertificates=\"http://www.axis.com/vapix/ws/certificates\" xmlns:aentry=\"http://www.axis.com/vapix/ws/entry\" xmlns:aev=\"http://www.axis.com/vapix/ws/event1\" xmlns:aeva=\"http://www.axis.com/vapix/ws/embeddedvideoanalytics1\" xmlns:ali1=\"http://www.axis.com/vapix/ws/light/CommonBinding\" xmlns:ali2=\"http://www.axis.com/vapix/ws/light/IntensityBinding\" xmlns:ali3=\"http://www.axis.com/vapix/ws/light/AngleOfIlluminationBinding\" xmlns:ali4=\"http://www.axis.com/vapix/ws/light/DayNightSynchronizeBinding\" xmlns:ali=\"http://www.axis.com/vapix/ws/light\" xmlns:apc=\"http://www.axis.com/vapix/ws/panopsiscalibration1\" xmlns:arth=\"http://www.axis.com/vapix/ws/recordedtour1\" xmlns:ascm=\"http://www.axis.com/vapix/ws/siblingcameramonitor1\" xmlns:asd=\"http://www.axis.com/vapix/ws/shockdetection\" xmlns:aweb=\"http://www.axis.com/vapix/ws/webserver\" xmlns:tan1=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan2=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tds:SetNetworkDefaultGatewayResponse></tds:SetNetworkDefaultGatewayResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1405 MediaCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_MediaCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<trt:GetServiceCapabilities2Response>\r\n" +
"<trt:Capabilities Rotation=\"true\" SnapshotUri=\"true\">\r\n" +
"<trt:ProfileCapabilities MaximumNumberOfProfiles=\"32\"></trt:ProfileCapabilities>\r\n" +
"<trt:StreamingCapabilities RTP_RTSP_TCP=\"true\" RTP_TCP=\"true\" RTPMulticast=\"true\"></trt:StreamingCapabilities></trt:Capabilities></trt:GetServiceCapabilities2Response></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1405 PTZCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_PTZCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilities2Response xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">\r\n" +
"<Capabilities EFlip=\"true\" Reverse=\"false\" GetCompatibleConfigurations=\"true\" MoveStatus=\"true\" StatusPosition=\"true\" /></GetServiceCapabilities2Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 ImagingCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_ImagingCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilities1Response xmlns=\"http://www.onvif.org/ver20/imaging/wsdl\">\r\n" +
"<Capabilities ImageStabilization=\"true\" /></GetServiceCapabilities1Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 SearchCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_SearchCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/search1/wsdl\">\r\n" +
"<Capabilities MetadataSearch=\"true\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 ReplaySetReplayConfigurationResponseIncorrectResponseTag
        public const string ResponseTicket1405_ReplaySetReplayConfigurationResponseIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<SetConfigurationResponse xmlns=\"http://www.onvif.org/ver10/replay/wsdl\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 ReceiverCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_ReceiverCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse xmlns=\"http://www.onvif.org/ver10/replay/wsdl\">\r\n" +
"<Capabilities RTP_Multicast=\"true\" RTP_RTSP_TCP=\"true\" SupportedReceivers=\"2\" MaximumRTSPURILength=\"256\" /></GetServiceCapabilitiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 RecordingCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_RecordingCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse2 xmlns=\"http://www.onvif.org/ver10/recording/wsdl\">\r\n" +
"<Capabilities DynamicRecordings=\"true\" DynamicTracks=\"true\" Encoding=\"tt:Jpeg\" MaxRate=\"8\" MaxTotalRate=\"8\" MaxRecordings=\"10\" /></GetServiceCapabilitiesResponse2></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 AccessControlCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_AccessControlCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilities1Response xmlns=\"http://www.onvif.org/ver10/accesscontrol/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"2\" /></GetServiceCapabilities1Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 DoorControlCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_DoorControlCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceResponse xmlns=\"http://www.onvif.org/ver10/doorcontrol/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"3\" /></GetServiceResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 AdvancedSecurityCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_AdvancedSecurityCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilities2Response xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\">\r\n" +
"<Capabilities>\r\n" +
"<KeystoreCapabilities MaximumNumberOfKeys=\"12\" MaximumNumberOfCertificates=\"12\" MaximumNumberOfCertificationPaths=\"12\" RSAKeyPairGeneration=\"true\" RSAKeyLengths=\"1 2\" PKCS10ExternalCertificationWithRSA=\"true\" SelfSignedCertificateCreationWithRSA=\"true\" X509Versions=\"1 3 4 5\">\r\n" +
"<SignatureAlgorithms>\r\n" +
"<algorithm>9</algorithm></SignatureAlgorithms></KeystoreCapabilities>\r\n" +
"<TLSServerCapabilities TLSServerSupported=\"1.0 2\" MaximumNumberOfTLSCertificationPaths=\"12\" /></Capabilities></GetServiceCapabilities2Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 AdvancedSecurity_DeleteKeyIncorrectResponseTag
        public const string ResponseTicket1405_AdvancedSecurity_DeleteKeyIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<DeleteResponse xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 AdvancedSecurity_AddServerCertificateAssignmentResponseTag
        public const string ResponseTicket1405_AdvancedSecurity_AddServerCertificateAssignmentResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\">\r\n" +
"<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<AddServerCertificateAssignment xmlns=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\">\r\n" +
"<CertificationPathID>newPathID</CertificationPathID></AddServerCertificateAssignment></s:Body></s:Envelope>";
        #endregion

        #region Ticket #1405 AccessRulesCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_AccessRulesCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<Response xmlns=\"http://www.onvif.org/ver10/accessrules/wsdl\">\r\n" +
"<Capabilities MaxLimit=\"10\" MaxAccessProfiles=\"8\" MaxAccessPoliciesPerAccessProfile=\"12\" MultipleSchedulesPerAccessPointSupported=\"true\" /></Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 DeviceIO_GetRelayOutputsResponseIncorrectResponseTag
        public const string ResponseTicket1405_DeviceIO_GetRelayOutputsResponseIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetRelayOutputs1Response xmlns=\"http://www.onvif.org/ver10/device/wsdl\">\r\n" +
"<RelayOutputs token=\"Relay1\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Bistable</Mode>\r\n" +
"<DelayTime>PT1S</DelayTime>\r\n" +
"<IdleState>open</IdleState></Properties></RelayOutputs>\r\n" +
"<RelayOutputs token=\"Relay2\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Monostable</Mode>\r\n" +
"<DelayTime>PT10S</DelayTime>\r\n" +
"<IdleState>closed</IdleState></Properties></RelayOutputs>\r\n" +
"<RelayOutputs token=\"Relay3\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Monostable</Mode>\r\n" +
"<DelayTime>PT10S</DelayTime>\r\n" +
"<IdleState>closed</IdleState></Properties></RelayOutputs></GetRelayOutputs1Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 DeviceIO_GetRelayOutputsResponseIncorrectNamespace
        public const string ResponseTicket1405_DeviceIO_GetRelayOutputsResponseIncorrectNamespace = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetRelayOutputsResponse xmlns=\"http://www.onvif.org/ver10/deviceio/wsdl\">\r\n" +
"<RelayOutputs token=\"Relay1\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Bistable</Mode>\r\n" +
"<DelayTime>PT1S</DelayTime>\r\n" +
"<IdleState>open</IdleState></Properties></RelayOutputs>\r\n" +
"<RelayOutputs token=\"Relay2\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Monostable</Mode>\r\n" +
"<DelayTime>PT10S</DelayTime>\r\n" +
"<IdleState>closed</IdleState></Properties></RelayOutputs>\r\n" +
"<RelayOutputs token=\"Relay3\">\r\n" +
"<Properties xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<Mode>Monostable</Mode>\r\n" +
"<DelayTime>PT10S</DelayTime>\r\n" +
"<IdleState>closed</IdleState></Properties></RelayOutputs></GetRelayOutputsResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 DeviceIO_GetVideoSourcesResponseIncorrectTag
        public const string ResponseTicket1405_DeviceIO_GetVideoSourcesResponseIncorrectTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetAudioSourcesResponse xmlns=\"http://www.onvif.org/ver10/deviceIO/wsdl\">\r\n" +
"\r\n" +
"</GetAudioSourcesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 AnalyticsCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_AnalyticsCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilities1Response xmlns=\"http://www.onvif.org/ver20/analytics/wsdl\">\r\n" +
"<Capabilities RuleSupport=\"true\" AnalyticsModuleSupport=\"true\" CellBasedSceneDescriptionSupported=\"true\" RuleOptionsSupported=\"true\" AnalyticsModuleOptionsSupported=\"true\" /></GetServiceCapabilities1Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Analytics_DeleteRulesIncorrectResponseTag
        public const string ResponseTicket1405_Analytics_DeleteRulesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<DeleteResponse xmlns=\"http://www.onvif.org/ver20/analytics/wsdl\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 EventCapabilitiesIncorrectResponseTag
        public const string ResponseTicket1405_EventCapabilitiesIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetServiceCapabilities</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetServiceCapabilitiesResponse1 xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<Capabilities WSSubscriptionPolicySupport=\"true\" WSPullPointSupport=\"true\" WSPausableSubscriptionManagerInterfaceSupport=\"true\" /></GetServiceCapabilitiesResponse1></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_SubscribeResponseIncorrectNamespace
        public const string ResponseTicket1405_Event_SubscribeResponseIncorrectNamespace = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://docs.oasis-open.org/wsn/bw-2/NotificationProducer/SubscribeResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<SubscribeResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<SubscriptionReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:17934/ServiceEvents10/SubscriptionManagerService.asmx</Address></SubscriptionReference>\r\n" +
"<CurrentTime>2018-02-13T14:03:06.1284693Z</CurrentTime>\r\n" +
"<TerminationTime>2018-02-13T14:04:06.1284693Z</TerminationTime></SubscribeResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_SubscribeIncorrectResponseTag
        public const string ResponseTicket1405_Event_SubscribeIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://docs.oasis-open.org/wsn/bw-2/NotificationProducer/SubscribeResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<Subscribe1Response xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<SubscriptionReference>\r\n" +
"<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://localhost:17934/ServiceEvents10/SubscriptionManagerService.asmx</Address></SubscriptionReference>\r\n" +
"<CurrentTime>2018-02-13T14:12:47.7989714Z</CurrentTime>\r\n" +
"<TerminationTime>2018-02-13T14:13:47.7989714Z</TerminationTime></Subscribe1Response></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_UnsubscribeResponseIncorrectNamespace
        public const string ResponseTicket1405_Event_UnsubscribeResponseIncorrectNamespace = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<UnsubscribeResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_UnsubscribeResponseIncorrectResponseTag
        public const string ResponseTicket1405_Event_UnsubscribeResponseIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://docs.oasis-open.org/wsn/bw-2/SubscriptionManager/UnsubscribeResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<Unsubscribe1Response xmlns=\"http://docs.oasis-open.org/wsn/b-2\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_SetSynchronizationPointResponseIncorrectNamespace
        public const string ResponseTicket1405_Event_SetSynchronizationPointResponseIncorrectNamespace = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/SetSynchronizationPointResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<SetSynchronizationPointResponse xmlns=\"http://docs.oasis-open.org/wsn/b-2\" /></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_PullMessagesResponseIncorrectResponseTag
        public const string ResponseTicket1405_Event_PullMessagesResponseIncorrectResponseTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2018-02-14T07:33:32.5011597Z</CurrentTime>\r\n" +
"<TerminationTime>2018-02-14T07:34:02.5011597Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:VideoSource/SignalLoss</Topic>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"1\" />\r\n" +
"<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"ObjectId\" Value=\"15\" /></tt:Data></tt:Message></Message></NotificationMessage>\r\n" +
"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:VideoSource</Topic>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"ObjectId\" Value=\"15\" /></tt:Data></tt:Message></Message></NotificationMessage></GetMessagesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1405 Event_PullMessagesResponseIncorrectNamespace
        public const string ResponseTicket1405_Event_PullMessagesResponseIncorrectNamespace = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events1/wsdl\">\r\n" +
"<CurrentTime>2018-02-14T07:33:32.5011597Z</CurrentTime>\r\n" +
"<TerminationTime>2018-02-14T07:34:02.5011597Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:VideoSource/SignalLoss</Topic>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"1\" />\r\n" +
"<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"ObjectId\" Value=\"15\" /></tt:Data></tt:Message></Message></NotificationMessage>\r\n" +
"<NotificationMessage xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:VideoSource</Topic>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\" />\r\n" +
"<tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"ObjectId\" Value=\"15\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #Forum1 PMResponse
        public const string ResponseForum1_PMResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsdd=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:tpa=\"http://www.onvif.org/ver10/pacs\" xmlns:xmime=\"http://tempuri.org/xmime.xsd\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:tac=\"http://www.onvif.org/ver10/accesscontrol/wsdl\" xmlns:tad=\"http://www.onvif.org/ver10/analyticsdevice/wsdl\" xmlns:tae=\"http://www.onvif.org/ver10/actionengine/wsdl\" xmlns:tana=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:tanr=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tar=\"http://www.onvif.org/ver10/accessrules/wsdl\" xmlns:tasa=\"http://www.onvif.org/ver10/advancedsecurity/wsdl/AdvancedSecurityServiceBinding\" xmlns:tasd=\"http://www.onvif.org/ver10/advancedsecurity/wsdl/Dot1XBinding\" xmlns:task=\"http://www.onvif.org/ver10/advancedsecurity/wsdl/KeystoreBinding\" xmlns:tast=\"http://www.onvif.org/ver10/advancedsecurity/wsdl/TLSServerBinding\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tec=\"http://www.onvif.org/ver10/events/wsdl/CreatePullPointBinding\" xmlns:tee=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tenc=\"http://www.onvif.org/ver10/events/wsdl/NotificationConsumerBinding\" xmlns:tenp=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tep=\"http://www.onvif.org/ver10/events/wsdl/PullPointBinding\" xmlns:tepa=\"http://www.onvif.org/ver10/events/wsdl/PausableSubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tepu=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tesm=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tls=\"http://www.onvif.org/ver10/display/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tndl=\"http://www.onvif.org/ver10/network/wsdl/DiscoveryLookupBinding\" xmlns:tdn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tnrd=\"http://www.onvif.org/ver10/network/wsdl/RemoteDiscoveryBinding\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:trt2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:trv=\"http://www.onvif.org/ver10/receiver/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:tss=\"http://www.onvif.org/ver10/schedule/wsdl\" xmlns:tth=\"http://www.onvif.org/ver10/thermal/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><SOAP-ENV:Header><wsa5:MessageID>urn:uuid:3a3c03f1-015b-42c3-9803-ff83ed11b97a</wsa5:MessageID><wsa5:ReplyTo SOAP-ENV:mustUnderstand=\"true\"><wsa5:Address>http://www.w3.org/2005/08/addressing/anonymous</wsa5:Address></wsa5:ReplyTo><wsa5:To SOAP-ENV:mustUnderstand=\"true\">http://192.168.18.36/onvif/event/subsription_14</wsa5:To><wsa5:Action SOAP-ENV:mustUnderstand=\"true\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa5:Action></SOAP-ENV:Header><SOAP-ENV:Body><tev:PullMessagesResponse><tev:CurrentTime>2018-01-23T00:52:58Z</tev:CurrentTime><tev:TerminationTime>2018-01-23T00:53:58Z</tev:TerminationTime><wsnt:NotificationMessage><wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:Device/Trigger/DigitalInput</wsnt:Topic><wsnt:Message><tt:Message  UtcTime=\"2018-01-23T00:52:58Z\" PropertyOperation=\"Initialized\"><tt:Source><tt:SimpleItem Name=\"InputToken\" Value=\"1\"/></tt:Source><tt:Data><tt:SimpleItem Name=\"LogicalState\" Value=\"false\"/></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>\r\n" +
"";
        #endregion

        #region Ticket #1616 GetProfilesResponse
        public const string ResponseTicket1616_GetProfilesResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetProfilesResponse xmlns=\"http://www.onvif.org/ver20/media/wsdl\">\r\n" +
"<Profiles token=\"media_profile4\">\r\n" +
"<Name>media2_profile1</Name>    \r\n" +
"</Profiles></GetProfilesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1614 PullMessagesResponse_prefix_defined_in_Topic
        public const string ResponseTicket1614_PullMessagesResponse_prefix_defined_in_Topic = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<PullMessagesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<CurrentTime>2018-04-13T11:27:05.4105137Z</CurrentTime>\r\n" +
"<TerminationTime>2018-04-13T11:28:10.4105137Z</TerminationTime>\r\n" +
"<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">\r\n" +
"<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\">tns1:Device/Trigger/DigitalInput</Topic>\r\n" +
"<Message>\r\n" +
"<tt:Message UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RelayToken\" Value=\"Relay\" /></tt:Source>\r\n" +
"<tt:Key>\r\n" +
"<tt:SimpleItem Name=\"LogicalState\" Value=\"Open\" /></tt:Key>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"LogicalState\" Value=\"inactive\" /></tt:Data></tt:Message></Message></NotificationMessage></PullMessagesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1614 Notify_prefix_defined_in_Topic
        public const string ResponseTicket1614_Notify_prefix_defined_in_Topic = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\">\r\n" +
"<SOAP-ENV:Header>\r\n" +
"<wsa5:To SOAP-ENV:mustUnderstand=\"true\">http://192.168.10.129:8086/onvif_notify_server/</wsa5:To>\r\n" +
"<wsa5:Action SOAP-ENV:mustUnderstand=\"true\">http://docs.oasis-open.org/wsn/bw-2/NotificationConsumer/Notify</wsa5:Action></SOAP-ENV:Header>\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<wsnt:Notify>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:Topic xmlns:tns1=\"http://www.onvif.org/ver10/topics\" Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Simple\">tns1:Device/Trigger/DigitalInput</wsnt:Topic>\r\n" +
"<wsnt:ProducerReference>\r\n" +
"<wsa5:Address>uri://1e43462b-0b0f-4262-a56a-2cb8aaf46f5a/ProducerReference</wsa5:Address></wsnt:ProducerReference>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2018-04-18T08:37:58Z\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"InputToken\" Value=\"0\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Key></tt:Key>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"LogicalState\" Value=\"1\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></wsnt:Notify></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1614 PanasonicPullMessagesResponse_MotionAlarmEvent
        public const string ResponseTicket1614_PanasonicPullMessagesResponse_MotionAlarmEvent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<env:Header>\r\n" +
"<wsa:MessageID>urn:uuid:93b04580-5ce1-11e8-bcb9-0080450d0003</wsa:MessageID>\r\n" +
"<wsa:RelatesTo>urn:uuid:c7866c40-5cd4-4bf0-ad9f-5e8586ad3f3a</wsa:RelatesTo>\r\n" +
"<wsa:To env:mustUnderstand=\"1\">http://www.w3.org/2005/08/addressing/anonymous</wsa:To>\r\n" +
"<wsa:Action env:mustUnderstand=\"1\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action></env:Header>\r\n" +
"<env:Body>\r\n" +
"<tev:PullMessagesResponse>\r\n" +
"<tev:CurrentTime>2018-05-21T10:27:35Z</tev:CurrentTime>\r\n" +
"<tev:TerminationTime>2018-05-28T10:27:35Z</tev:TerminationTime>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa:Address>http://192.168.0.76/onvif/Subscription?Idx=60183</wsa:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnspana1=\"http://panasonic.co.jp/sn/psn/2010/event/topics\">tns1:VideoSource/MotionAlarm</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2018-05-21T10:27:35Z\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Source\" Value=\"VideoSource\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"State\" Value=\"false\" /></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1614 PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_Envelope
        public const string ResponseTicket1614_PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_Envelope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<env:Header>\r\n" +
"<wsa:MessageID>urn:uuid:93b04580-5ce1-11e8-bcb9-0080450d0003</wsa:MessageID>\r\n" +
"<wsa:RelatesTo>urn:uuid:c7866c40-5cd4-4bf0-ad9f-5e8586ad3f3a</wsa:RelatesTo>\r\n" +
"<wsa:To env:mustUnderstand=\"1\">http://www.w3.org/2005/08/addressing/anonymous</wsa:To>\r\n" +
"<wsa:Action env:mustUnderstand=\"1\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action></env:Header>\r\n" +
"<env:Body>\r\n" +
"<tev:PullMessagesResponse>\r\n" +
"<tev:CurrentTime>2018-05-21T10:27:35Z</tev:CurrentTime>\r\n" +
"<tev:TerminationTime>2018-05-28T10:27:35Z</tev:TerminationTime>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa:Address>http://192.168.0.76/onvif/Subscription?Idx=60183</wsa:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:tnspana1=\"http://panasonic.co.jp/sn/psn/2010/event/topics\">tns1:VideoSource/MotionAlarm</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2018-05-21T10:27:35Z\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Source\" Value=\"VideoSource\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"State\" Value=\"false\" /></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1614 PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_Body
        public const string ResponseTicket1614_PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_Body = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<env:Header>\r\n" +
"<wsa:MessageID>urn:uuid:93b04580-5ce1-11e8-bcb9-0080450d0003</wsa:MessageID>\r\n" +
"<wsa:RelatesTo>urn:uuid:c7866c40-5cd4-4bf0-ad9f-5e8586ad3f3a</wsa:RelatesTo>\r\n" +
"<wsa:To env:mustUnderstand=\"1\">http://www.w3.org/2005/08/addressing/anonymous</wsa:To>\r\n" +
"<wsa:Action env:mustUnderstand=\"1\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action></env:Header>\r\n" +
"<env:Body xmlns:tns1=\"http://www.onvif.org/ver10/topics\" >\r\n" +
"<tev:PullMessagesResponse>\r\n" +
"<tev:CurrentTime>2018-05-21T10:27:35Z</tev:CurrentTime>\r\n" +
"<tev:TerminationTime>2018-05-28T10:27:35Z</tev:TerminationTime>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa:Address>http://192.168.0.76/onvif/Subscription?Idx=60183</wsa:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:tnspana1=\"http://panasonic.co.jp/sn/psn/2010/event/topics\">tns1:VideoSource/MotionAlarm</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2018-05-21T10:27:35Z\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Source\" Value=\"VideoSource\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"State\" Value=\"false\" /></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1614 PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_PullMessagesResponse
        public const string ResponseTicket1614_PullMessagesResponse_MotionAlarmEvent_tns1_declarated_in_PullMessagesResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:enc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:rpc=\"http://www.w3.org/2003/05/soap-rpc\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<env:Header>\r\n" +
"<wsa:MessageID>urn:uuid:93b04580-5ce1-11e8-bcb9-0080450d0003</wsa:MessageID>\r\n" +
"<wsa:RelatesTo>urn:uuid:c7866c40-5cd4-4bf0-ad9f-5e8586ad3f3a</wsa:RelatesTo>\r\n" +
"<wsa:To env:mustUnderstand=\"1\">http://www.w3.org/2005/08/addressing/anonymous</wsa:To>\r\n" +
"<wsa:Action env:mustUnderstand=\"1\">http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse</wsa:Action></env:Header>\r\n" +
"<env:Body>\r\n" +
"<tev:PullMessagesResponse xmlns:tns1=\"http://www.onvif.org/ver10/topics\" >\r\n" +
"<tev:CurrentTime>2018-05-21T10:27:35Z</tev:CurrentTime>\r\n" +
"<tev:TerminationTime>2018-05-28T10:27:35Z</tev:TerminationTime>\r\n" +
"<wsnt:NotificationMessage>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa:Address>http://192.168.0.76/onvif/Subscription?Idx=60183</wsa:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:tnspana1=\"http://panasonic.co.jp/sn/psn/2010/event/topics\">tns1:VideoSource/MotionAlarm</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2018-05-21T10:27:35Z\" PropertyOperation=\"Initialized\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"Source\" Value=\"VideoSource\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"State\" Value=\"false\" /></tt:Data></tt:Message></wsnt:Message></wsnt:NotificationMessage></tev:PullMessagesResponse></env:Body></env:Envelope>";
        #endregion

        #region PlugFestBugInPTZFeatures AxisGetMedia2ProfilesResponse
        public const string AxisGetMedia2ProfilesResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tr2:GetProfilesResponse>\r\n" +
"<tr2:Profiles fixed=\"false\" token=\"profile_1_h264\">\r\n" +
"<tr2:Name>profile_1 h264</tr2:Name>\r\n" +
"<tr2:Configurations>\r\n" +
"<tr2:VideoSource token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:SourceToken>0</tt:SourceToken>\r\n" +
"<tt:Bounds height=\"2160\" width=\"3840\" y=\"0\" x=\"0\"></tt:Bounds>\r\n" +
"<tt:Extension>\r\n" +
"<tt:Rotate>\r\n" +
"<tt:Mode>ON</tt:Mode>\r\n" +
"<tt:Degree>0</tt:Degree></tt:Rotate></tt:Extension></tr2:VideoSource>\r\n" +
"<tr2:PTZ token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:NodeToken>1</tt:NodeToken>\r\n" +
"<tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace>\r\n" +
"<tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace>\r\n" +
"<tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace>\r\n" +
"<tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace>\r\n" +
"<tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace>\r\n" +
"<tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace>\r\n" +
"<tt:DefaultPTZSpeed>\r\n" +
"<tt:PanTilt space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace\" y=\"1\" x=\"1\"></tt:PanTilt>\r\n" +
"<tt:Zoom space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace\" x=\"1\"></tt:Zoom></tt:DefaultPTZSpeed>\r\n" +
"<tt:DefaultPTZTimeout>PT2147S</tt:DefaultPTZTimeout>\r\n" +
"<tt:ZoomLimits>\r\n" +
"<tt:Range>\r\n" +
"<tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI>\r\n" +
"<tt:XRange>\r\n" +
"<tt:Min>0</tt:Min>\r\n" +
"<tt:Max>1</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tr2:PTZ></tr2:Configurations></tr2:Profiles>\r\n" +
"<tr2:Profiles fixed=\"false\" token=\"profile_1_jpeg\">\r\n" +
"<tr2:Name>profile_1 jpeg</tr2:Name>\r\n" +
"<tr2:Configurations>\r\n" +
"<tr2:VideoSource token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:SourceToken>0</tt:SourceToken>\r\n" +
"<tt:Bounds height=\"2160\" width=\"3840\" y=\"0\" x=\"0\"></tt:Bounds>\r\n" +
"<tt:Extension>\r\n" +
"<tt:Rotate>\r\n" +
"<tt:Mode>ON</tt:Mode>\r\n" +
"<tt:Degree>0</tt:Degree></tt:Rotate></tt:Extension></tr2:VideoSource>\r\n" +
"<tr2:PTZ token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:NodeToken>1</tt:NodeToken>\r\n" +
"<tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace>\r\n" +
"<tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace>\r\n" +
"<tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace>\r\n" +
"<tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace>\r\n" +
"<tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace>\r\n" +
"<tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace>\r\n" +
"<tt:DefaultPTZSpeed>\r\n" +
"<tt:PanTilt space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace\" y=\"1\" x=\"1\"></tt:PanTilt>\r\n" +
"<tt:Zoom space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace\" x=\"1\"></tt:Zoom></tt:DefaultPTZSpeed>\r\n" +
"<tt:DefaultPTZTimeout>PT2147S</tt:DefaultPTZTimeout>\r\n" +
"<tt:ZoomLimits>\r\n" +
"<tt:Range>\r\n" +
"<tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI>\r\n" +
"<tt:XRange>\r\n" +
"<tt:Min>0</tt:Min>\r\n" +
"<tt:Max>1</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tr2:PTZ></tr2:Configurations></tr2:Profiles>\r\n" +
"<tr2:Profiles fixed=\"false\" token=\"profile0\">\r\n" +
"<tr2:Name>New profile</tr2:Name>\r\n" +
"<tr2:Configurations>\r\n" +
"<tr2:VideoSource token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:SourceToken>0</tt:SourceToken>\r\n" +
"<tt:Bounds height=\"2160\" width=\"3840\" y=\"0\" x=\"0\"></tt:Bounds>\r\n" +
"<tt:Extension>\r\n" +
"<tt:Rotate>\r\n" +
"<tt:Mode>ON</tt:Mode>\r\n" +
"<tt:Degree>0</tt:Degree></tt:Rotate></tt:Extension></tr2:VideoSource>\r\n" +
"<tr2:PTZ token=\"0\">\r\n" +
"<tt:Name>user0</tt:Name>\r\n" +
"<tt:UseCount>3</tt:UseCount>\r\n" +
"<tt:NodeToken>1</tt:NodeToken>\r\n" +
"<tt:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tt:DefaultAbsolutePantTiltPositionSpace>\r\n" +
"<tt:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:DefaultAbsoluteZoomPositionSpace>\r\n" +
"<tt:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tt:DefaultRelativePanTiltTranslationSpace>\r\n" +
"<tt:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tt:DefaultRelativeZoomTranslationSpace>\r\n" +
"<tt:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tt:DefaultContinuousPanTiltVelocitySpace>\r\n" +
"<tt:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tt:DefaultContinuousZoomVelocitySpace>\r\n" +
"<tt:DefaultPTZSpeed>\r\n" +
"<tt:PanTilt space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace\" y=\"1\" x=\"1\"></tt:PanTilt>\r\n" +
"<tt:Zoom space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace\" x=\"1\"></tt:Zoom></tt:DefaultPTZSpeed>\r\n" +
"<tt:DefaultPTZTimeout>PT2147S</tt:DefaultPTZTimeout>\r\n" +
"<tt:ZoomLimits>\r\n" +
"<tt:Range>\r\n" +
"<tt:URI>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tt:URI>\r\n" +
"<tt:XRange>\r\n" +
"<tt:Min>0</tt:Min>\r\n" +
"<tt:Max>1</tt:Max></tt:XRange></tt:Range></tt:ZoomLimits></tr2:PTZ></tr2:Configurations></tr2:Profiles></tr2:GetProfilesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1730 GetAnalyticsConfigurationsResponse
        public const string Ticket1730_GetAnalyticsConfigurationsResponse = "<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:soapenc=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tst=\"http://www.onvif.org/ver10/storage/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl\" xmlns:wsoap12=\"http://schemas.xmlsoap.org/wsdl/soap12\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:wsadis=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wsntw=\"http://docs.oasis-open.org/wsn/bw-2\" xmlns:wsrf-rw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:tnshik=\"http://www.hikvision.com/2011/event/topics\" xmlns:hikwsd=\"http://www.onvifext.com/onvif/ext/ver10/wsdl\" xmlns:hikxsd=\"http://www.onvifext.com/onvif/ext/ver10/schema\" xmlns:tas=\"http://www.onvif.org/ver10/advancedsecurity/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\"><env:Body><tr2:GetAnalyticsConfigurationsResponse><tr2:Configurations token=\"VideoAnalyticsToken\"><tt:Name>VideoAnalyticsName</tt:Name>\r\n" +
"<tt:UseCount>4</tt:UseCount>\r\n" +
"<tt:AnalyticsEngineConfiguration><tt:AnalyticsModule Name=\"MyCellMotionModule\" Type=\"tt:CellMotionEngine\"><tt:Parameters><tt:SimpleItem Name=\"Sensitivity\" Value=\"0\"/>\r\n" +
"<tt:ElementItem Name=\"Layout\"><tt:CellLayout Columns=\"22\" Rows=\"18\"><tt:Transformation><tt:Translate x=\"-1.000000\" y=\"-1.000000\"/>\r\n" +
"<tt:Scale x=\"0.090909\" y=\"0.111111\"/></tt:Transformation></tt:CellLayout></tt:ElementItem></tt:Parameters></tt:AnalyticsModule>\r\n" +
"<tt:AnalyticsModule Name=\"MyLineDetectorModule\" Type=\"tt:LineDetectorEngine\"><tt:Parameters><tt:SimpleItem Name=\"Sensitivity\" Value=\"50\"/>\r\n" +
"<tt:ElementItem Name=\"Layout\"><tt:Transformation><tt:Translate x=\"-1.000000\" y=\"-1.000000\"/>\r\n" +
"<tt:Scale x=\"0.002000\" y=\"0.002000\"/></tt:Transformation></tt:ElementItem>\r\n" +
"<tt:ElementItem Name=\"Field\"><tt:PolygonConfiguration><tt:Polygon><tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"1000\"/>\r\n" +
"<tt:Point x=\"1000\" y=\"1000\"/>\r\n" +
"<tt:Point x=\"1000\" y=\"0\"/></tt:Polygon></tt:PolygonConfiguration></tt:ElementItem></tt:Parameters></tt:AnalyticsModule>\r\n" +
"<tt:AnalyticsModule Name=\"MyFieldDetectorModule\" Type=\"tt:FieldDetectorEngine\"><tt:Parameters><tt:SimpleItem Name=\"Sensitivity\" Value=\"50\"/>\r\n" +
"<tt:ElementItem Name=\"Layout\"><tt:Transformation><tt:Translate x=\"-1.000000\" y=\"-1.000000\"/>\r\n" +
"<tt:Scale x=\"0.002000\" y=\"0.002000\"/></tt:Transformation></tt:ElementItem>\r\n" +
"<tt:ElementItem Name=\"Field\"><tt:PolygonConfiguration><tt:Polygon><tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"1000\"/>\r\n" +
"<tt:Point x=\"1000\" y=\"1000\"/>\r\n" +
"<tt:Point x=\"1000\" y=\"0\"/></tt:Polygon></tt:PolygonConfiguration></tt:ElementItem></tt:Parameters></tt:AnalyticsModule>\r\n" +
"<tt:AnalyticsModule Name=\"MyTamperDetecModule\" Type=\"hikxsd:TamperEngine\"><tt:Parameters><tt:SimpleItem Name=\"Sensitivity\" Value=\"0\"/>\r\n" +
"<tt:ElementItem Name=\"Transformation\"><tt:Transformation><tt:Translate x=\"-1.000000\" y=\"-1.000000\"/>\r\n" +
"<tt:Scale x=\"0.002841\" y=\"0.003472\"/></tt:Transformation></tt:ElementItem>\r\n" +
"<tt:ElementItem Name=\"Field\"><tt:PolygonConfiguration><tt:Polygon><tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"576\"/>\r\n" +
"<tt:Point x=\"704\" y=\"576\"/>\r\n" +
"<tt:Point x=\"704\" y=\"0\"/></tt:Polygon></tt:PolygonConfiguration></tt:ElementItem></tt:Parameters></tt:AnalyticsModule></tt:AnalyticsEngineConfiguration>\r\n" +
"<tt:RuleEngineConfiguration><tt:Rule Name=\"MyMotionDetectorRule\" Type=\"tt:CellMotionDetector\"><tt:Parameters><tt:SimpleItem Name=\"MinCount\" Value=\"5\"/>\r\n" +
"<tt:SimpleItem Name=\"AlarmOnDelay\" Value=\"1000\"/>\r\n" +
"<tt:SimpleItem Name=\"AlarmOffDelay\" Value=\"1000\"/>\r\n" +
"<tt:SimpleItem Name=\"ActiveCells\" Value=\"zwA=\"/></tt:Parameters></tt:Rule>\r\n" +
"<tt:Rule Name=\"MyTamperDetectorRule\" Type=\"hikxsd:TamperDetector\"><tt:Parameters><tt:ElementItem Name=\"Field\"><tt:PolygonConfiguration><tt:Polygon><tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"0\"/>\r\n" +
"<tt:Point x=\"0\" y=\"0\"/></tt:Polygon></tt:PolygonConfiguration></tt:ElementItem></tt:Parameters></tt:Rule></tt:RuleEngineConfiguration></tr2:Configurations></tr2:GetAnalyticsConfigurationsResponse></env:Body></env:Envelope>";
        #endregion

        #region Ticket #1730 GetAnalyticsConfigurationsResponse
        public const string WhiteSpace_GetAnalyticsConfigurationsResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\" xmlns:ter=\"http://www.onvif.org/ver10/error\"><SOAP-ENV:Body><tr2:GetAnalyticsConfigurationsResponse><tr2:Configurations token=\"0\"><tt:Name>analytics0</tt:Name><tt:UseCount>0</tt:UseCount><tt:AnalyticsEngineConfiguration></tt:AnalyticsEngineConfiguration><tt:RuleEngineConfiguration><tt:Rule Type=\"tt:FaceRecognition\" Name=\"Profile 1\"><Parameters xmlns=\"http://www.onvif.org/ver10/schema\">\r\n" +
"			<tt:SimpleItem Name=\"IncludeImage\" Value=\"Embedded\">\r\n" +
"			</tt:SimpleItem>\r\n" +
"		</Parameters></tt:Rule></tt:RuleEngineConfiguration></tr2:Configurations></tr2:GetAnalyticsConfigurationsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Hanwa GetServicesWithCapabilitiesResponse
        public const string Hanwa_GetServicesWithCapabilitiesResponse = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:xenc=\"http://www.w3.org/2001/04/xmlenc#\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrf-bf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsrf-r=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:wsaw=\"http://www.w3.org/2006/05/addressing/wsdl\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\" xmlns:axt3=\"http://www.onvif.org/ver20/analytics/wsdl/RuleEngineBinding\" xmlns:axt4=\"http://www.onvif.org/ver20/analytics/wsdl/AnalyticsEngineBinding\" xmlns:axt2=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tev5=\"http://www.onvif.org/ver10/events/wsdl/NotificationConsumerBinding\" xmlns:tev6=\"http://www.onvif.org/ver10/events/wsdl/PullPointBinding\" xmlns:tev7=\"http://www.onvif.org/ver10/events/wsdl/CreatePullPointBinding\" xmlns:tev8=\"http://www.onvif.org/ver10/events/wsdl/PausableSubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:tpv=\"http://www.onvif.org/ver10/provisioning/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:trc=\"http://www.onvif.org/ver10/recording/wsdl\" xmlns:trp=\"http://www.onvif.org/ver10/replay/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:tth=\"http://www.onvif.org/ver10/thermal/wsdl\" xmlns:xmime=\"http://www.w3.org/2005/05/xmlmime\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:env=\"http://www.w3.org/2001/12/soap-envelope\" xmlns:tnssamsung=\"http://www.samsungcctv.com/2011/event/topics\">\r\n" +
"	<SOAP-ENV:Body>\r\n" +
"		<tds:GetServicesResponse>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/device/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/device_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<tds:Capabilities>\r\n" +
"						<tds:Network NTP=\"1\" HostnameFromDHCP=\"false\" Dot11Configuration=\"false\" DynDNS=\"true\" IPVersion6=\"true\" ZeroConfiguration=\"true\" IPFilter=\"true\"/>\r\n" +
"						<tds:Security RELToken=\"false\" HttpDigest=\"true\" UsernameToken=\"true\" KerberosToken=\"false\" SAMLToken=\"false\" X.509Token=\"false\" RemoteUserHandling=\"false\" Dot1X=\"false\" AccessPolicyConfig=\"false\" OnboardKeyGeneration=\"false\" TLS1.2=\"false\" TLS1.1=\"true\"/>\r\n" +
"						<tds:System HttpSupportInformation=\"false\" HttpSystemLogging=\"false\" HttpSystemBackup=\"false\" HttpFirmwareUpgrade=\"true\" FirmwareUpgrade=\"false\" SystemLogging=\"true\" SystemBackup=\"false\" RemoteDiscovery=\"false\" DiscoveryBye=\"true\" DiscoveryResolve=\"true\"/>\r\n" +
"					</tds:Capabilities>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>17</tt:Major>\r\n" +
"					<tt:Minor>6</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/media/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/media_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<trt:Capabilities OSD=\"false\" VideoSourceMode=\"true\" Rotation=\"false\" SnapshotUri=\"true\">\r\n" +
"						<trt:ProfileCapabilities MaximumNumberOfProfiles=\"10\"/>\r\n" +
"						<trt:StreamingCapabilities RTP_RTSP_TCP=\"true\" RTP_TCP=\"true\" RTPMulticast=\"true\"/>\r\n" +
"					</trt:Capabilities>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>17</tt:Major>\r\n" +
"					<tt:Minor>6</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver20/media/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/media_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<tr2:Capabilities TemporaryOSDText=\"false\" OSD=\"true\" VideoSourceMode=\"true\" Rotation=\"false\" SnapshotUri=\"true\">\r\n" +
"						<tr2:ProfileCapabilities ConfigurationsSupported=\"VideoEncoder VideoSource Metadata Analytics AudioEncoder AudioSource AudioDecoder AudioOutput\" MaximumNumberOfProfiles=\"10\"/>\r\n" +
"						<tr2:StreamingCapabilities RTSPWebSocketUri=\"ws:/onvifstreamingserver\" RTP_RTSP_TCP=\"true\" RTP_TCP=\"true\" RTPMulticast=\"true\" RTSPStreaming=\"true\"/>\r\n" +
"					</tr2:Capabilities>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>18</tt:Major>\r\n" +
"					<tt:Minor>5</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/events/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/event_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<tev:Capabilities MaxPullPoints=\"15\" MaxNotificationProducers=\"15\" WSPausableSubscriptionManagerInterfaceSupport=\"false\" WSPullPointSupport=\"true\" WSSubscriptionPolicySupport=\"true\" PersistentNotificationStorage=\"false\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>18</tt:Major>\r\n" +
"					<tt:Minor>5</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver20/imaging/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/imaging_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<timg:Capabilities/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>16</tt:Major>\r\n" +
"					<tt:Minor>9</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/deviceIO/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/deviceio_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<tmd:Capabilities DigitalInputOptions=\"true\" DigitalInputs=\"1\" RelayOutputs=\"1\" AudioOutputs=\"1\" AudioSources=\"1\" VideoOutputs=\"1\" VideoSources=\"1\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>17</tt:Major>\r\n" +
"					<tt:Minor>12</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver20/analytics/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/analytics_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<axt:Capabilities RuleEngine=\"true\" RuleOptionsSupported=\"true\" RuleSupport=\"true\" AnalyticsModuleSupport=\"false\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>18</tt:Major>\r\n" +
"					<tt:Minor>5</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/recording/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/recording_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<trc:Capabilities Options=\"true\" MaxRecordingJobs=\"2\" MaxRecordings=\"1\" MaxTotalRate=\"6144\" MaxRate=\"6144\" Encoding=\"JPEG H264 AAC G711\" 						DynamicTracks=\"false\" DynamicRecordings=\"false\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>18</tt:Major>\r\n" +
"					<tt:Minor>5</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/search/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/search_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<tse:Capabilities GeneralStartEvents=\"false\" MetadataSearch=\"false\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>2</tt:Major>\r\n" +
"					<tt:Minor>42</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"			<tds:Service>\r\n" +
"				<tds:Namespace>http://www.onvif.org/ver10/replay/wsdl</tds:Namespace>\r\n" +
"				<tds:XAddr>http://125.132.148.201:8051/onvif/replay_service</tds:XAddr>\r\n" +
"				<tds:Capabilities>\r\n" +
"					<trp:Capabilities RTP_RTSP_TCP=\"true\" SessionTimeoutRange=\"10 60\" ReversePlayback=\"true\"/>\r\n" +
"				</tds:Capabilities>\r\n" +
"				<tds:Version>\r\n" +
"					<tt:Major>18</tt:Major>\r\n" +
"					<tt:Minor>5</tt:Minor>\r\n" +
"				</tds:Version>\r\n" +
"			</tds:Service>\r\n" +
"		</tds:GetServicesResponse>\r\n" +
"	</SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region Ticket #1566 GetEventPropertiesResponse
        public const string Ticket1566_GetEventPropertiesResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<my:TopicSet xmlns:my=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<my:documentation>\r\n" +
"<tns1:Device xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:pt=\"http://www.onvif.org/ver10/pacs\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<tns1:Trigger>\r\n" +
"<tns1:DigitalInput wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"IOInputToken\" Type=\"tt:ReferenceToken\" /></tt:Source></tt:MessageDescription></tns1:DigitalInput></tns1:Trigger></tns1:Device></my:documentation>\r\n" +
"<tns1:VideoSource xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<SignalLoss wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></SignalLoss>\r\n" +
"<SignalTooBlurry wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></SignalTooBlurry>\r\n" +
"<SignalTooNoisy wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></SignalTooNoisy>\r\n" +
"<SignalTooDark wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></SignalTooDark>\r\n" +
"<SignalTooBright wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></SignalTooBright>\r\n" +
"<CameraRedirected wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></CameraRedirected>\r\n" +
"<MotionAlarm wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSourceToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></MotionAlarm></tns1:VideoSource>\r\n" +
"<tns1:Device xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<Trigger xmlns=\"\">\r\n" +
"<Relay wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"RelayToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"LogicalState\" Type=\"tt:RelayLogicalState\" /></tt:Data></tt:MessageDescription></Relay>\r\n" +
"<DigitalInput wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"InputToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"LogicalState\" Type=\"xsd:boolean\" /></tt:Data></tt:MessageDescription></DigitalInput></Trigger></tns1:Device>\r\n" +
"<tns1:MediaConfiguration xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<Profile wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ProfileToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Config\" Type=\"tt:Profile\" /></tt:Data></tt:MessageDescription></Profile>\r\n" +
"<VideoEncoderConfiguration wstop:topic=\"true\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\" xmlns:tt=\"http://www.onvif.org/ver10/schema\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoEncoderConfigurationToken\" Type=\"tt:ReferenceToken\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:ElementItemDescription Name=\"Config\" Type=\"tt:VideoEncoderConfiguration\" /></tt:Data></tt:MessageDescription></VideoEncoderConfiguration></tns1:MediaConfiguration></my:TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1758 GetEventPropertiesResponse
        public const string Ticket1758_GetEventPropertiesResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:tev1=\"http://www.onvif.org/ver10/events/wsdl/NotificationProducerBinding\" xmlns:tev2=\"http://www.onvif.org/ver10/events/wsdl/EventBinding\" xmlns:tev3=\"http://www.onvif.org/ver10/events/wsdl/SubscriptionManagerBinding\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tev4=\"http://www.onvif.org/ver10/events/wsdl/PullPointSubscriptionBinding\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tnsaxis=\"http://www.axis.com/2009/event/topics\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:pt=\"http://www.onvif.org/ver10/pacs\" xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:tac=\"http://www.onvif.org/ver10/accesscontrol/wsdl\">\r\n" +
"<SOAP-ENV:Header>\r\n" +
"<wsa5:RelatesTo>urn:uuid:50a2fc7a-a7d5-4b43-ab0c-ccb0d531e3a1</wsa5:RelatesTo>\r\n" +
"<wsa5:To SOAP-ENV:mustUnderstand=\"true\">http://www.w3.org/2005/08/addressing/anonymous</wsa5:To>\r\n" +
"<wsa5:Action SOAP-ENV:mustUnderstand=\"true\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</wsa5:Action></SOAP-ENV:Header>\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tev:GetEventPropertiesResponse>\r\n" +
"<tev:TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</tev:TopicNamespaceLocation>\r\n" +
"<wsnt:FixedTopicSet>false</wsnt:FixedTopicSet>\r\n" +
"<wstop:TopicSet>\r\n" +
"<tns1:Device>\r\n" +
"<tnsaxis:IO>\r\n" +
"<VirtualInput wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"port\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></VirtualInput>\r\n" +
"<VirtualPort wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"port\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"state\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></VirtualPort>\r\n" +
"<Port wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"port\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"state\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Port></tnsaxis:IO>\r\n" +
"<tnsaxis:SystemMessage>\r\n" +
"<ActionFailed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"description\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></ActionFailed></tnsaxis:SystemMessage>\r\n" +
"<tnsaxis:Status>\r\n" +
"<SystemReady wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ready\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></SystemReady></tnsaxis:Status>\r\n" +
"<tnsaxis:Casing>\r\n" +
"<Open wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Open\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Open></tnsaxis:Casing>\r\n" +
"<tnsaxis:Network>\r\n" +
"<Lost wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"interface\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"lost\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Lost></tnsaxis:Network>\r\n" +
"<tnsaxis:PeerConnection wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Status\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Peer\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:PeerConnection></tns1:Device>\r\n" +
"<tnsaxis:EventLogger>\r\n" +
"<Alarm wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Device Source\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"UtcTime\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Alarm>\r\n" +
"<DroppedEvents wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Device Source\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"DroppedEvents\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DroppedEvents>\r\n" +
"<DroppedAlarms wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Device Source\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"DroppedAlarms\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DroppedAlarms></tnsaxis:EventLogger>\r\n" +
"<tns1:EventBuffer>\r\n" +
"<Begin wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Device Source\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Begin></tns1:EventBuffer>\r\n" +
"<tns1:Configuration>\r\n" +
"<Door>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Removed></Door>\r\n" +
"<tnsaxis:IdPoint>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Removed></tnsaxis:IdPoint>\r\n" +
"<AccessPoint>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Removed></AccessPoint>\r\n" +
"<Area>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AreaToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AreaToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Removed></Area>\r\n" +
"<Credential>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Removed>\r\n" +
"<tnsaxis:ThirdPartyCredentialCreated wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderEmail\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:ThirdPartyCredentialCreated>\r\n" +
"<tnsaxis:ThirdPartyCredentialCreatedFailed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderEmail\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:ThirdPartyCredentialCreatedFailed>\r\n" +
"<tnsaxis:ThirdPartyCredentialRemoved wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderEmail\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:ThirdPartyCredentialRemoved>\r\n" +
"<tnsaxis:ThirdPartyCredentialRemovedFailed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderEmail\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:ThirdPartyCredentialRemovedFailed>\r\n" +
"<tnsaxis:ThirdPartyCredentialEnabled wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderEmail\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialLastName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialCardNumber\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialFirstName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:ThirdPartyCredentialEnabled></Credential>\r\n" +
"<AccessProfile>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessProfileToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessProfileToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"NbrTokens\" Type=\"xsd:int\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Removed></AccessProfile>\r\n" +
"<Schedule>\r\n" +
"<Changed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ScheduleToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Changed>\r\n" +
"<Removed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ScheduleToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Removed></Schedule></tns1:Configuration>\r\n" +
"<tns1:Door>\r\n" +
"<State>\r\n" +
"<DoorPhysicalState wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:DoorPhysicalState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorPhysicalState>\r\n" +
"<DoubleLockPhysicalState wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:LockPhysicalState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoubleLockPhysicalState>\r\n" +
"<LockPhysicalState wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:LockPhysicalState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></LockPhysicalState>\r\n" +
"<DoorMode wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:DoorMode\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorMode>\r\n" +
"<DoorTamper wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:DoorTamperState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorTamper>\r\n" +
"<DoorAlarm wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:DoorAlarmState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorAlarm>\r\n" +
"<DoorWarning wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"tdc:DoorWarningState\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></DoorWarning></State>\r\n" +
"<tnsaxis:Status>\r\n" +
"<BatteryAlarm wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ID\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></BatteryAlarm>\r\n" +
"<RadioDisturbance wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ID\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></RadioDisturbance>\r\n" +
"<LockJammed wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"DoorToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></LockJammed></tnsaxis:Status></tns1:Door>\r\n" +
"<tns1:IdPoint>\r\n" +
"<tnsaxis:Activity wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Description\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:Activity>\r\n" +
"<tnsaxis:Timeout wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:Timeout>\r\n" +
"<tnsaxis:Status>\r\n" +
"<WhitelistSync>\r\n" +
"<Error wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Error>\r\n" +
"<Ongoing wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Ongoing\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Ongoing></WhitelistSync>\r\n" +
"<BatteryAlarm wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"ID\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></BatteryAlarm>\r\n" +
"<RadioDisturbance wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"ID\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></RadioDisturbance>\r\n" +
"<SecureChannel wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></SecureChannel>\r\n" +
"<Device wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Device></tnsaxis:Status>\r\n" +
"<tnsaxis:Tampering wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:Tampering>\r\n" +
"<tnsaxis:Request>\r\n" +
"<PIN wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Action\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"PIN\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Description\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></PIN>\r\n" +
"<REX wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Action\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"REX\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Description\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></REX>\r\n" +
"<IdData wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Action\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"BitCount\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Description\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></IdData></tnsaxis:Request>\r\n" +
"<tnsaxis:PreAuthorization>\r\n" +
"<IdData wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Category\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"IdPointToken\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Location\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Area\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Action\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"BitCount\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Granted\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Description\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></IdData></tnsaxis:PreAuthorization></tns1:IdPoint>\r\n" +
"<tns1:AccessControl>\r\n" +
"<AccessGranted>\r\n" +
"<Credential wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Anonymous wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Anonymous></AccessGranted>\r\n" +
"<AccessTaken>\r\n" +
"<Credential wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Anonymous wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Anonymous></AccessTaken>\r\n" +
"<AccessNotTaken>\r\n" +
"<Credential wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Anonymous wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></Anonymous></AccessNotTaken>\r\n" +
"<Denied>\r\n" +
"<Authentication>\r\n" +
"<InvalidPIN wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></InvalidPIN></Authentication>\r\n" +
"<CredentialNotFound wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"REX\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"BitCount\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CardNr\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription>\r\n" +
"<Card wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"PIN\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"BitCount\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CardNr\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"REX\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Card>\r\n" +
"<Pin wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"PIN\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Card\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"BitCount\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CardNr\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Pin></CredentialNotFound>\r\n" +
"<Credential wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Credential>\r\n" +
"<Anonymous wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Anonymous></Denied>\r\n" +
"<Duress wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"xsd:string\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Duress></tns1:AccessControl>\r\n" +
"<tns1:Credential>\r\n" +
"<State>\r\n" +
"<ApbViolation wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialHolderName\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"ClientUpdated\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"ApbViolation\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialType\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></ApbViolation>\r\n" +
"<Enabled wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"CredentialToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"ClientUpdated\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Reason\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Enabled></State></tns1:Credential>\r\n" +
"<tns1:AccessPoint>\r\n" +
"<State>\r\n" +
"<Enabled wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"AccessPointToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Enabled></State></tns1:AccessPoint>\r\n" +
"<tns1:Schedule>\r\n" +
"<tnsaxis:Interval wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ScheduleToken\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Exception\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></tnsaxis:Interval>\r\n" +
"<tnsaxis:Pulse wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"false\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ScheduleToken\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source></tt:MessageDescription></tnsaxis:Pulse>\r\n" +
"<State>\r\n" +
"<Active wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ScheduleToken\" Type=\"pt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"Name\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Active\" Type=\"xsd:boolean\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"SpecialDay\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Active></State></tns1:Schedule>\r\n" +
"<tnsaxis:RemoteDevice>\r\n" +
"<Connection>\r\n" +
"<Status wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"Token\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Connected\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></Status></Connection></tnsaxis:RemoteDevice>\r\n" +
"<tnsaxis:ThirdPartyCredential>\r\n" +
"<Status>\r\n" +
"<ProviderConnection wstop:topic=\"true\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"ProviderName\" Type=\"xsd:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"Connected\" Type=\"xsd:boolean\"></tt:SimpleItemDescription></tt:Data></tt:MessageDescription></ProviderConnection></Status></tnsaxis:ThirdPartyCredential></wstop:TopicSet>\r\n" +
"<wsnt:TopicExpressionDialect>http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</wsnt:TopicExpressionDialect>\r\n" +
"<wsnt:TopicExpressionDialect>http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</wsnt:TopicExpressionDialect>\r\n" +
"<tev:MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</tev:MessageContentFilterDialect>\r\n" +
"<tev:MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</tev:MessageContentSchemaLocation></tev:GetEventPropertiesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion

        #region ANALYTICS-2-1-1 GetEventPropertiesResponse
        public const string MotionRegionDetectorTopic_GetEventPropertiesResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tdc=\"http://www.onvif.org/ver10/doorcontrol/wsdl\" xmlns:pt=\"http://www.onvif.org/ver10/pacs\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<soap:Header>\r\n" +
"<a:Action soap:mustUnderstand=\"1\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">http://www.onvif.org/ver10/events/wsdl/EventPortType/GetEventPropertiesResponse</a:Action></soap:Header>\r\n" +
"<soap:Body>\r\n" +
"<GetEventPropertiesResponse xmlns=\"http://www.onvif.org/ver10/events/wsdl\">\r\n" +
"<TopicNamespaceLocation>http://www.onvif.org/onvif/ver10/topics/topicns.xml</TopicNamespaceLocation>\r\n" +
"<FixedTopicSet xmlns=\"http://docs.oasis-open.org/wsn/b-2\">true</FixedTopicSet>\r\n" +
"<TopicSet xmlns=\"http://docs.oasis-open.org/wsn/t-1\">        \r\n" +
"<tns1:RuleEngine>\r\n" +
"<MotionRegionDetector xmlns=\"\">\r\n" +
"<Motion wstop:topic=\"true\" xmlns=\"\">\r\n" +
"<tt:MessageDescription IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSource\" Type=\"tt:ReferenceToken\" />\r\n" +
"<tt:SimpleItemDescription Name=\"RuleName\" Type=\"xs:string\" /></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xs:boolean\" /></tt:Data></tt:MessageDescription></Motion></MotionRegionDetector></tns1:RuleEngine></TopicSet>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet</TopicExpressionDialect>\r\n" +
"<TopicExpressionDialect xmlns=\"http://docs.oasis-open.org/wsn/b-2\">http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete</TopicExpressionDialect>\r\n" +
"<MessageContentFilterDialect>http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter</MessageContentFilterDialect>\r\n" +
"<MessageContentSchemaLocation>http://www.onvif.org/ver10/schema/onvif.xsd</MessageContentSchemaLocation></GetEventPropertiesResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #1767 FormattingOfResponse
        public const string ResponseTicket1767_FormattingOfResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+ "\x0a" + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:xmime=\"http://www.w3.org/2005/05/xmlmime\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wsrfr=\"http://docs.oasis-open.org/wsrf/r-2\" xmlns:wsrfrw=\"http://docs.oasis-open.org/wsrf/rw-2\" xmlns:wsrfbf=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\" xmlns:tev=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:timg=\"http://www.onvif.org/ver20/imaging/wsdl\" xmlns:tptz=\"http://www.onvif.org/ver20/ptz/wsdl\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\" xmlns:tr2=\"http://www.onvif.org/ver20/media/wsdl\" xmlns:tmd=\"http://www.onvif.org/ver10/deviceIO/wsdl\" xmlns:tth=\"http://www.onvif.org/ver10/thermal/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:tavg=\"http://www.avigilon.com/onvif/ver10/avigilon-types\" xmlns:tnsavg=\"http://www.avigilon.com/onvif/ver10/topics\" xmlns:avg=\"http://www.avigilon.com/onvif/ver10/avigilon/wsdl\" xmlns:avgb=\"http://www.avigilon.com/onvif/ver10/avigilon-base/wsdl\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\"><SOAP-ENV:Body><tds:GetServicesResponse><tds:Service><tds:Namespace>http://www.onvif.org/ver20/analytics/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/analytics_service</tds:XAddr><tds:Capabilities><tan:Capabilities ChipStreamingSupport=\"true\" AnalyticsModuleSupport=\"true\" RuleSupport=\"true\"></tan:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver10/device/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/device_service</tds:XAddr><tds:Capabilities><tds:Capabilities><tds:Network DynDNS=\"false\" IPVersion6=\"true\" ZeroConfiguration=\"true\" IPFilter=\"false\"></tds:Network><tds:Security RELToken=\"false\" HttpDigest=\"true\" UsernameToken=\"true\" KerberosToken=\"false\" SAMLToken=\"false\" X.509Token=\"true\" RemoteUserHandling=\"false\" Dot1X=\"false\" AccessPolicyConfig=\"false\" OnboardKeyGeneration=\"true\" TLS1.2=\"false\" TLS1.1=\"false\" TLS1.0=\"true\"></tds:Security><tds:System FirmwareUpgrade=\"true\" SystemLogging=\"true\" SystemBackup=\"false\" RemoteDiscovery=\"false\" DiscoveryBye=\"true\" DiscoveryResolve=\"true\"></tds:System></tds:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>20</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver10/deviceIO/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/deviceio_service</tds:XAddr><tds:Capabilities><tmd:Capabilities DigitalInputOptions=\"false\" DigitalInputs=\"0\" SerialPorts=\"0\" RelayOutputs=\"1\" AudioOutputs=\"1\" AudioSources=\"1\" VideoOutputs=\"0\" VideoSources=\"1\"></tmd:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver10/events/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/event_service</tds:XAddr><tds:Capabilities><tev:Capabilities MaxPullPoints=\"30\" WSPausableSubscriptionManagerInterfaceSupport=\"true\" WSPullPointSupport=\"false\" WSSubscriptionPolicySupport=\"false\"></tev:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver20/imaging/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/imaging_service</tds:XAddr><tds:Capabilities><timg:Capabilities></timg:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver10/media/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/media_service</tds:XAddr><tds:Capabilities><trt:Capabilities OSD=\"true\" SnapshotUri=\"true\"><trt:ProfileCapabilities MaximumNumberOfProfiles=\"30\"></trt:ProfileCapabilities><trt:StreamingCapabilities RTP_RTSP_TCP=\"true\" RTP_TCP=\"false\" RTPMulticast=\"true\"></trt:StreamingCapabilities></trt:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver20/media/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/media_service</tds:XAddr><tds:Capabilities><tr2:Capabilities OSD=\"true\" SnapshotUri=\"true\"><tr2:ProfileCapabilities ConfigurationsSupported=\"Metadata VideoSource VideoEncoder AudioSource AudioEncoder AudioDecoder AudioOutput Analytics PTZ\" MaximumNumberOfProfiles=\"30\"></tr2:ProfileCapabilities><tr2:StreamingCapabilities RTSPStreaming=\"true\" RTP_RTSP_TCP=\"true\" RTPMulticast=\"true\"></tr2:StreamingCapabilities></tr2:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.onvif.org/ver20/ptz/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/ptz_service</tds:XAddr><tds:Capabilities><tptz:Capabilities></tptz:Capabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major><tt:Minor>2</tt:Minor></tds:Version></tds:Service><tds:Service><tds:Namespace>http://www.avigilon.com/onvif/ver10/avigilon/wsdl</tds:Namespace><tds:XAddr>http://172.32.4.104/onvif/avigilon</tds:XAddr><tds:Capabilities><tavg:AvigilonCapabilities><tavg:XAddr>http://172.32.4.104/onvif/avigilon</tavg:XAddr><tavg:SingleShotAutoFocusSupport>true</tavg:SingleShotAutoFocusSupport><tavg:SerialPortSupport>true</tavg:SerialPortSupport><tavg:PtzOverSerialPortSupport>true</tavg:PtzOverSerialPortSupport><tavg:StreamUriNtpSupport>true</tavg:StreamUriNtpSupport><tavg:SyncedStreamModeSupport>true</tavg:SyncedStreamModeSupport><tavg:LowLatencyStreamingSupport>true</tavg:LowLatencyStreamingSupport><tavg:StorageSupport>true</tavg:StorageSupport></tavg:AvigilonCapabilities></tds:Capabilities><tds:Version><tt:Major>2</tt:Major>" + "<tt:Minor>2</tt:Minor></tds:Version></tds:Service></tds:GetServicesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>" + "\x0d\x0a";
        #endregion

        #region MediaService
        public const string GetMetadataConfigurationsResponseEmptyList = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetMetadataConfigurationsResponse xmlns=\"http://www.onvif.org/ver10/media/wsdl\"></GetMetadataConfigurationsResponse></soap:Body></soap:Envelope>";
        #endregion

        #region GetEventSearchResultsResponse
        public const string GetEventSearchResultsResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
"<soap:Body>\r\n" +
"<GetEventSearchResultsResponse xmlns=\"http://www.onvif.org/ver10/search/wsdl\">\r\n" +
"<tse:ResultList xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\">\r\n" +
"<tt:SearchState>Completed</tt:SearchState>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>VIDEO001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54.0000Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Recording/State</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"IsRecording\" Value=\"true\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>VIDEO001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Track/State</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"Track\" Value=\"VIDEO001\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"IsDataPresent\" Value=\"true\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>AUDIO001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Track/State</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"Track\" Value=\"AUDIO001\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"IsDataPresent\" Value=\"true\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>META001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Track/State</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"Track\" Value=\"META001\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"IsDataPresent\" Value=\"true\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>VIDEO001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Track/VideoParameters</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"Track\" Value=\"VIDEO001\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"VideoEncoding\" Value=\"JPEG\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"VideoWidth\" Value=\"600\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"VideoHeight\" Value=\"800\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"VideoRateControl\" Value=\"12\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result>\r\n" +
"<tt:Result>\r\n" +
"<tt:RecordingToken>cam1idx1</tt:RecordingToken>\r\n" +
"<tt:TrackToken>AUDIO001</tt:TrackToken>\r\n" +
"<tt:Time>2012-05-28T09:59:54Z</tt:Time>\r\n" +
"<tt:Event>\r\n" +
"<wsnt:SubscriptionReference>\r\n" +
"<wsa5:Address>http://192.168.10.209/search_service</wsa5:Address></wsnt:SubscriptionReference>\r\n" +
"<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">tns1:RecordingHistory/Track/AudioParameters</wsnt:Topic>\r\n" +
"<wsnt:Message>\r\n" +
"<tt:Message UtcTime=\"2012-05-28T09:59:54\" PropertyOperation=\"Changed\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItem Name=\"RecordingToken\" Value=\"cam1idx1\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"Track\" Value=\"AUDIO001\"></tt:SimpleItem></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItem Name=\"AudioEncoding\" Value=\"AAC\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"AudioSampleRate\" Value=\"600\"></tt:SimpleItem>\r\n" +
"<tt:SimpleItem Name=\"AudioBitrate\" Value=\"12\"></tt:SimpleItem></tt:Data></tt:Message></wsnt:Message></tt:Event>\r\n" +
"<tt:StartStateEvent>true</tt:StartStateEvent></tt:Result></tse:ResultList></GetEventSearchResultsResponse></soap:Body></soap:Envelope>";
        #endregion

        #region Ticket #2047 GetSupportedRulesResponse
        public const string ResponseTicket2047_GetSupportedRulesResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:tt=\"http://www.onvif.org/ver10/schema\" xmlns:tan=\"http://www.onvif.org/ver20/analytics/wsdl\" xmlns:xs=\"http://www.w3.org/2005/05/xmlmime\" xmlns:axt=\"http://www.onvif.org/ver20/analytics\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\">\r\n" +
"<SOAP-ENV:Body>\r\n" +
"<tan:GetSupportedRulesResponse>\r\n" +
"<tan:SupportedRules>\r\n" +
"<tt:RuleDescription Name=\"tt:MotionRegionDetector\" maxInstances=\"32\">\r\n" +
"<tt:Parameters>\r\n" +
"<tt:ElementItemDescription Name=\"MotionRegion\" Type=\"axt:MotionRegionConfig\"></tt:ElementItemDescription></tt:Parameters>\r\n" +
"<tt:Messages IsProperty=\"true\">\r\n" +
"<tt:Source>\r\n" +
"<tt:SimpleItemDescription Name=\"VideoSource\" Type=\"tt:ReferenceToken\"></tt:SimpleItemDescription>\r\n" +
"<tt:SimpleItemDescription Name=\"RuleName\" Type=\"xs:string\"></tt:SimpleItemDescription></tt:Source>\r\n" +
"<tt:Data>\r\n" +
"<tt:SimpleItemDescription Name=\"State\" Type=\"xs:boolean\"></tt:SimpleItemDescription></tt:Data>\r\n" +
"<tt:ParentTopic>tns1:RuleEngine/MotionRegionDetector/Motion</tt:ParentTopic></tt:Messages></tt:RuleDescription></tan:SupportedRules></tan:GetSupportedRulesResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        #endregion
    }
}