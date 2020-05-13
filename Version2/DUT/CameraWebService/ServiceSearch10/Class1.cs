using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.CameraWebService.Search10
{
    public class Stringonst
    {

        public const string Test1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n"+
"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:c14n=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsa5=\"http://www.w3.org/2005/08/addressing\" xmlns:xmime5=\"http://www.w3.org/2005/05/xmlmime\" xmlns:xop=\"http://www.w3.org/2004/08/xop/include\" xmlns:tse2=\"http://www.onvif.org/ver10/schema\" xmlns:tse3=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tse4=\"http://docs.oasis-open.org/wsrf/bf-2\" xmlns:tse5=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:tse=\"http://www.onvif.org/ver10/search/wsdl\" xmlns:ter=\"http://www.onvif.org/ver10/error\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tev8=\"http://www.onvif.org/ver10/schema\">\r\n"+
"<SOAP-ENV:Body>\r\n"+
"<tse:GetEventSearchResultsResponse>\r\n"+
"<tse:ResultList>\r\n"+
"<tse2:SearchState>Searching</tse2:SearchState>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken></tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:16:46Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Recording/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:16:46.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsRecording\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:16:47Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:16:47.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:16:47Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:16:47.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"META001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken></tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Recording/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:50.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"false\" Name=\"IsRecording\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:51Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:51.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>AUDIO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:51Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:51.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"AUDIO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"false\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:51Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:51.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"META001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:52Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/VideoParameters</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:52.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"Recording\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"JPEG\" Name=\"VideoEncoding\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"320\" Name=\"VideoWidth\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"240\" Name=\"VideoHeight\"></tev8:SimpleItem>\r\n"+
"<tev8:ElementItem Name=\"VideoRateControl?\">\r\n"+
"<tev8:VideoRateControl>\r\n"+
"<tev8:FrameRateLimit>5</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:VideoRateControl></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>AUDIO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:52Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/AudioParameters</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:19:52.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"Recording\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"AUDIO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"G711\" Name=\"AudioEncoding\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"AudioSampleRate\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/Profile</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"profile1\" Name=\"ProfileToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"profile1\" Name=\"ProfileName\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/Profile</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"profile0\" Name=\"ProfileToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"profile0\" Name=\"ProfileName\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken></tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:54Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Recording/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:19:54.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsRecording\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoSourceConfiguration/MediaService</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"VideoSourceConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoSourceConfiguration token=\"0\">\r\n"+
"<tev8:Name>video</tev8:Name>\r\n"+
"<tev8:UseCount>2</tev8:UseCount>\r\n"+
"<tev8:SourceToken>0</tev8:SourceToken>\r\n"+
"<tev8:Bounds x=\"0\" y=\"0\" width=\"1920\" height=\"1080\" /></tev8:VideoSourceConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:55Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:19:55.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/AudioSourceConfiguration/MediaService</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"AudioSourceConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:AudioSourceConfiguration token=\"0\">\r\n"+
"<tev8:Name>audio</tev8:Name>\r\n"+
"<tev8:UseCount>2</tev8:UseCount>\r\n"+
"<tev8:SourceToken>0</tev8:SourceToken></tev8:AudioSourceConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:55Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:19:55.000000Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n"+
"<tev8:SimpleItem Value=\"META001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"264\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"264\">\r\n"+
"<tev8:Name>h264</tev8:Name>\r\n"+
"<tev8:UseCount>1</tev8:UseCount>\r\n"+
"<tev8:Encoding>H264</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>480</tev8:Width>\r\n"+
"<tev8:Height>270</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>30</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>2048</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:H264>\r\n"+
"<tev8:GovLength>15</tev8:GovLength>\r\n"+
"<tev8:H264Profile>Baseline0</tev8:H264Profile></tev8:H264>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"15\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"15\">\r\n"+
"<tev8:Name>jpeg15</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>1920</tev8:Width>\r\n"+
"<tev8:Height>1080</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>15</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"14\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"14\">\r\n"+
"<tev8:Name>jpeg14</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>1920</tev8:Width>\r\n"+
"<tev8:Height>1080</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>10</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"13\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"13\">\r\n"+
"<tev8:Name>jpeg13</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>1920</tev8:Width>\r\n"+
"<tev8:Height>1080</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>5</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"10\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"10\">\r\n"+
"<tev8:Name>jpeg10</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>960</tev8:Width>\r\n"+
"<tev8:Height>540</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>10</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"9\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"9\">\r\n"+
"<tev8:Name>jpeg9</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>960</tev8:Width>\r\n"+
"<tev8:Height>540</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>5</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"8\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"8\">\r\n"+
"<tev8:Name>jpeg8</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>480</tev8:Width>\r\n"+
"<tev8:Height>270</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>30</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"7\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"7\">\r\n"+
"<tev8:Name>jpeg7</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>480</tev8:Width>\r\n"+
"<tev8:Height>270</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>15</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n"+
"<tse3:Message>\r\n"+
"<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n"+
"<tev8:Source>\r\n"+
"<tev8:SimpleItem Value=\"6\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n"+
"<tev8:Key></tev8:Key>\r\n"+
"<tev8:Data>\r\n"+
"<tev8:ElementItem>\r\n"+
"<tev8:Name>Config</tev8:Name>\r\n"+
"<tev8:VideoEncoderConfiguration token=\"6\">\r\n"+
"<tev8:Name>jpeg6</tev8:Name>\r\n"+
"<tev8:UseCount>0</tev8:UseCount>\r\n"+
"<tev8:Encoding>JPEG</tev8:Encoding>\r\n"+
"<tev8:Resolution>\r\n"+
"<tev8:Width>480</tev8:Width>\r\n"+
"<tev8:Height>270</tev8:Height></tev8:Resolution>\r\n"+
"<tev8:Quality>3</tev8:Quality>\r\n"+
"<tev8:RateControl>\r\n"+
"<tev8:FrameRateLimit>10</tev8:FrameRateLimit>\r\n"+
"<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n"+
"<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n"+
"<tev8:Multicast>\r\n"+
"<tev8:Address>\r\n"+
"<tev8:Type>IPv4</tev8:Type>\r\n"+
"<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n"+
"<tev8:Port>0</tev8:Port>\r\n"+
"<tev8:TTL>0</tev8:TTL>\r\n"+
"<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n"+
"<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n"+
"<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n"+
"<tse2:Result>\r\n"+
"<tse2:RecordingToken>0</tse2:RecordingToken>\r\n"+
"<tse2:TrackToken>META001</tse2:TrackToken>\r\n"+
"<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n"+
"<tse2:Event>\r\n"+
"<tse3:SubscriptionReference>\r\n"+
"<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n"+
"<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n"+
"<tse3:ProducerReference>\r\n";

        public const string Test2 =
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"5\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:VideoEncoderConfiguration token=\"5\">\r\n" +
        "<tev8:Name>jpeg5</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:Encoding>JPEG</tev8:Encoding>\r\n" +
        "<tev8:Resolution>\r\n" +
        "<tev8:Width>480</tev8:Width>\r\n" +
        "<tev8:Height>270</tev8:Height></tev8:Resolution>\r\n" +
        "<tev8:Quality>3</tev8:Quality>\r\n" +
        "<tev8:RateControl>\r\n" +
        "<tev8:FrameRateLimit>5</tev8:FrameRateLimit>\r\n" +
        "<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n" +
        "<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"4\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:VideoEncoderConfiguration token=\"4\">\r\n" +
        "<tev8:Name>jpeg4</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:Encoding>JPEG</tev8:Encoding>\r\n" +
        "<tev8:Resolution>\r\n" +
        "<tev8:Width>320</tev8:Width>\r\n" +
        "<tev8:Height>240</tev8:Height></tev8:Resolution>\r\n" +
        "<tev8:Quality>3</tev8:Quality>\r\n" +
        "<tev8:RateControl>\r\n" +
        "<tev8:FrameRateLimit>30</tev8:FrameRateLimit>\r\n" +
        "<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n" +
        "<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoEncoderConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"3\" Name=\"VideoEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:VideoEncoderConfiguration token=\"3\">\r\n" +
        "<tev8:Name>jpeg3</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:Encoding>JPEG</tev8:Encoding>\r\n" +
        "<tev8:Resolution>\r\n" +
        "<tev8:Width>320</tev8:Width>\r\n" +
        "<tev8:Height>240</tev8:Height></tev8:Resolution>\r\n" +
        "<tev8:Quality>3</tev8:Quality>\r\n" +
        "<tev8:RateControl>\r\n" +
        "<tev8:FrameRateLimit>15</tev8:FrameRateLimit>\r\n" +
        "<tev8:EncodingInterval>1</tev8:EncodingInterval>\r\n" +
        "<tev8:BitrateLimit>384</tev8:BitrateLimit></tev8:RateControl>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:VideoEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/AudioEncoderConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"AudioEncoderConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:AudioEncoderConfiguration token=\"0\">\r\n" +
        "<tev8:Name>g711</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:Encoding>G711</tev8:Encoding>\r\n" +
        "<tev8:Bitrate>64</tev8:Bitrate>\r\n" +
        "<tev8:SampleRate>8</tev8:SampleRate>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:AudioEncoderConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoAnalyticsConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"VideoAnalyticsConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"va\" Name=\"VideoAnalyticsConfigurationName\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/PTZConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"PTZConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:PTZConfiguration token=\"0\">\r\n" +
        "<tev8:Name>ptz</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:NodeToken>0</tev8:NodeToken>\r\n" +
        "<tev8:DefaultAbsolutePantTiltPositionSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace</tev8:DefaultAbsolutePantTiltPositionSpace>\r\n" +
        "<tev8:DefaultAbsoluteZoomPositionSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace</tev8:DefaultAbsoluteZoomPositionSpace>\r\n" +
        "<tev8:DefaultRelativePanTiltTranslationSpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace</tev8:DefaultRelativePanTiltTranslationSpace>\r\n" +
        "<tev8:DefaultRelativeZoomTranslationSpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace</tev8:DefaultRelativeZoomTranslationSpace>\r\n" +
        "<tev8:DefaultContinuousPanTiltVelocitySpace>http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace</tev8:DefaultContinuousPanTiltVelocitySpace>\r\n" +
        "<tev8:DefaultContinuousZoomVelocitySpace>http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace</tev8:DefaultContinuousZoomVelocitySpace>\r\n" +
        "<tev8:DefaultPTZSpeed>\r\n" +
        "<tev8:PanTilt space=\"http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace\" y=\"1.000000\" x=\"1.000000\" />\r\n" +
        "<tev8:Zoom space=\"http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace\" x=\"1.000000\" /></tev8:DefaultPTZSpeed>\r\n" +
        "<tev8:DefaultPTZTimeout>PT8S</tev8:DefaultPTZTimeout></tev8:PTZConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"8\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"8\">\r\n" +
        "<tev8:Name>metadata8</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>false</tev8:Status>\r\n" +
        "<tev8:Position>false</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"7\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"7\">\r\n" +
        "<tev8:Name>metadata7</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>false</tev8:Status>\r\n" +
        "<tev8:Position>false</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"6\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"6\">\r\n" +
        "<tev8:Name>metadata6</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>false</tev8:Status>\r\n" +
        "<tev8:Position>false</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"5\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"5\">\r\n" +
        "<tev8:Name>metadata5</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>false</tev8:Status>\r\n" +
        "<tev8:Position>false</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"4\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"4\">\r\n" +
        "<tev8:Name>metadata4</tev8:Name>\r\n" +
        "<tev8:UseCount>0</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>false</tev8:Status>\r\n" +
        "<tev8:Position>false</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/MetaDataConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"1\" Name=\"MetaDataConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:MetadataConfiguration token=\"1\">\r\n" +
        "<tev8:Name>metadata1</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:PTZStatus>\r\n" +
        "<tev8:Status>true</tev8:Status>\r\n" +
        "<tev8:Position>true</tev8:Position></tev8:PTZStatus>\r\n" +
        "<tev8:Events>\r\n" +
        "<tev8:Filter></tev8:Filter></tev8:Events>\r\n" +
        "<tev8:Analytics>false</tev8:Analytics>\r\n" +
        "<tev8:Multicast>\r\n" +
        "<tev8:Address>\r\n" +
        "<tev8:Type>IPv4</tev8:Type>\r\n" +
        "<tev8:IPv4Address>0.0.0.0</tev8:IPv4Address></tev8:Address>\r\n" +
        "<tev8:Port>0</tev8:Port>\r\n" +
        "<tev8:TTL>0</tev8:TTL>\r\n" +
        "<tev8:AutoStart>false</tev8:AutoStart></tev8:Multicast>\r\n" +
        "<tev8:SessionTimeout>PT1M</tev8:SessionTimeout></tev8:MetadataConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/AudioOutputConfiguration/DeviceIOService</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"AudioOutputConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:AudioOutputConfiguration token=\"0\">\r\n" +
        "<tev8:Name>AudioOutput</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:OutputToken>0</tev8:OutputToken>\r\n" +
        "<tev8:OutputLevel>50</tev8:OutputLevel></tev8:AudioOutputConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/AudioOutputConfiguration/MediaService</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"AudioOutputConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:AudioOutputConfiguration token=\"0\">\r\n" +
        "<tev8:Name>AudioOutput</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:OutputToken>0</tev8:OutputToken>\r\n" +
        "<tev8:OutputLevel>50</tev8:OutputLevel></tev8:AudioOutputConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:50Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingConfig/JobState</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:50Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingJobToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"Idle\" Name=\"State\"></tev8:SimpleItem>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Information</tev8:Name>\r\n" +
        "<tev8:RecordingJobStateInformation>\r\n" +
        "<tev8:RecordingToken>0</tev8:RecordingToken>\r\n" +
        "<tev8:State>Idle</tev8:State>\r\n" +
        "<tev8:Sources>\r\n" +
        "<tev8:SourceToken Type=\"http://www.onvif.org/ver10/schema/Profile\">\r\n" +
        "<tev8:Token>profile0</tev8:Token></tev8:SourceToken>\r\n" +
        "<tev8:State>Idle</tev8:State>\r\n" +
        "<tev8:Tracks>\r\n" +
        "<tev8:Track>\r\n" +
        "<tev8:SourceTag>Video</tev8:SourceTag>\r\n" +
        "<tev8:Destination>VIDEO001</tev8:Destination>\r\n" +
        "<tev8:Error>No Error</tev8:Error>\r\n" +
        "<tev8:State>Idle</tev8:State></tev8:Track>\r\n" +
        "<tev8:Track>\r\n" +
        "<tev8:SourceTag>Audio</tev8:SourceTag>\r\n" +
        "<tev8:Destination>AUDIO001</tev8:Destination>\r\n" +
        "<tev8:Error>No Error</tev8:Error>\r\n" +
        "<tev8:State>Idle</tev8:State></tev8:Track>\r\n" +
        "<tev8:Track>\r\n" +
        "<tev8:SourceTag>Metadata</tev8:SourceTag>\r\n" +
        "<tev8:Destination>META001</tev8:Destination>\r\n" +
        "<tev8:Error>No Error</tev8:Error>\r\n" +
        "<tev8:State>Idle</tev8:State></tev8:Track>\r\n" +
        "<tev8:Track>\r\n" +
        "<tev8:SourceTag></tev8:SourceTag>\r\n" +
        "<tev8:Destination></tev8:Destination>\r\n" +
        "<tev8:Error></tev8:Error>\r\n" +
        "<tev8:State>Idle</tev8:State></tev8:Track></tev8:Tracks></tev8:Sources></tev8:RecordingJobStateInformation></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken></tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:20:14Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Recording/State</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:20:14.000000Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"IsRecording\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:53Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/VideoSourceConfiguration/DeviceIOService</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:53Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"VideoSourceConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:VideoSourceConfiguration token=\"0\">\r\n" +
        "<tev8:Name>video</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:SourceToken>0</tev8:SourceToken>\r\n" +
        "<tev8:Bounds x=\"0\" y=\"0\" width=\"1920\" height=\"1080\" /></tev8:VideoSourceConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:20:14Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:20:14.000000Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n" +
        "<tev8:SimpleItem Value=\"META001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:53Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Configuration/AudioSourceConfiguration/DeviceIOService</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:53Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"AudioSourceConfigurationToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Config</tev8:Name>\r\n" +
        "<tev8:AudioSourceConfiguration token=\"0\">\r\n" +
        "<tev8:Name>audio</tev8:Name>\r\n" +
        "<tev8:UseCount>2</tev8:UseCount>\r\n" +
        "<tev8:SourceToken>0</tev8:SourceToken></tev8:AudioSourceConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:20:15Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T05:20:15.000000Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n" +
        "<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:53Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Device/Trigger/DigitalInput</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:53Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"input1\" Name=\"InputToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"LogicalState\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:53Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:Device/Trigger/DigitalInput</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T08:49:53Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"input2\" Name=\"InputToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"LogicalState\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>META001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:19:54Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingConfig/RecordingJobConfiguration</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Changed\" UtcTime=\"2013-10-17T08:49:54Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingJobToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:ElementItem>\r\n" +
        "<tev8:Name>Configuration</tev8:Name>\r\n" +
        "<tev8:RecordingJobConfiguration>\r\n" +
        "<tev8:RecordingToken>0</tev8:RecordingToken>\r\n" +
        "<tev8:Mode>Idle</tev8:Mode>\r\n" +
        "<tev8:Priority>0</tev8:Priority>\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SourceToken Type=\"http://www.onvif.org/ver10/schema/Profile\">\r\n" +
        "<tev8:Token>profile0</tev8:Token></tev8:SourceToken>\r\n" +
        "<tev8:AutoCreateReceiver>0</tev8:AutoCreateReceiver>\r\n" +
        "<tev8:Tracks>\r\n" +
        "<tev8:SourceTag>Video</tev8:SourceTag>\r\n" +
        "<tev8:Destination>VIDEO001</tev8:Destination></tev8:Tracks>\r\n" +
        "<tev8:Tracks>\r\n" +
        "<tev8:SourceTag>Audio</tev8:SourceTag>\r\n" +
        "<tev8:Destination>AUDIO001</tev8:Destination></tev8:Tracks>\r\n" +
        "<tev8:Tracks>\r\n" +
        "<tev8:SourceTag>Metadata</tev8:SourceTag>\r\n" +
        "<tev8:Destination>META001</tev8:Destination></tev8:Tracks>\r\n" +
        "<tev8:Tracks>\r\n" +
        "<tev8:SourceTag></tev8:SourceTag>\r\n" +
        "<tev8:Destination></tev8:Destination></tev8:Tracks></tev8:Source>\r\n" +
        "<tev8:Extension /></tev8:RecordingJobConfiguration></tev8:ElementItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken></tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:20:23Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Recording/State</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:20:23.000000Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"false\" Name=\"IsRecording\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result>\r\n" +
        "<tse2:Result>\r\n" +
        "<tse2:RecordingToken>0</tse2:RecordingToken>\r\n" +
        "<tse2:TrackToken>VIDEO001</tse2:TrackToken>\r\n" +
        "<tse2:Time>2013-10-17T05:20:23Z</tse2:Time>\r\n" +
        "<tse2:Event>\r\n" +
        "<tse3:SubscriptionReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service?id=20</wsa5:Address></tse3:SubscriptionReference>\r\n" +
        "<tse3:Topic Dialect=\"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete\">tns1:RecordingHistory/Track/State</tse3:Topic>\r\n" +
        "<tse3:ProducerReference>\r\n" +
        "<wsa5:Address>http://10.8.30.243:80/onvif/event_service</wsa5:Address></tse3:ProducerReference>\r\n" +
        "<tse3:Message>\r\n" +
        "<tev8:Message PropertyOperation=\"Initialized\" UtcTime=\"2013-10-17T05:20:23.000000Z\">\r\n" +
        "<tev8:Source>\r\n" +
        "<tev8:SimpleItem Value=\"0\" Name=\"RecordingToken\"></tev8:SimpleItem>\r\n" +
        "<tev8:SimpleItem Value=\"VIDEO001\" Name=\"Track\"></tev8:SimpleItem></tev8:Source>\r\n" +
        "<tev8:Key></tev8:Key>\r\n" +
        "<tev8:Data>\r\n" +
        "<tev8:SimpleItem Value=\"true\" Name=\"IsDataPresent\"></tev8:SimpleItem></tev8:Data></tev8:Message></tse3:Message></tse2:Event>\r\n" +
        "<tse2:StartStateEvent>false</tse2:StartStateEvent></tse2:Result></tse:ResultList></tse:GetEventSearchResultsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";

        public const string Test3 = Test1 + Test2;
    }
}