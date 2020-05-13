using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HttpTransport;

namespace CameraWebService.Notification
{
    public class NotificationProvider
    {

        public void Notify(string address)
        {
            /*
            string request = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.w3.org/2003/05/soap-envelope\">" +
                            "<Body> " +
                                "<Notify xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">" +
                                    "<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\">"+
                                        "<SubscriptionReference>"+
                                             "<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address>"+
                                             "<ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" />"+
                                              "<Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" />"+
                                        "</SubscriptionReference>"+
                                        "<Topic Dialect=\"TestDialect\" >SomeTopic</Topic>"+
                                        "<Message xmlns=\"http://docs.oasis-open.org/wsn/b-2\" > " +
                                        "<Message xmlns=\"http://www.onvif.org/ver10/schema\" UtcTime=\"2008-10-10T12:24:57.321\"> " +
                                        "<Source xmlns=\"http://www.onvif.org/ver10/schema\"> " +
                                        "<SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoSourceConfigurationToken\" Value=\"1\"/>  " +
                                        "<SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/>  " +
                                        "<SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Value=\"MyImportantFence1\" Name=\"Rule\"/>   " +
                                        "</Source >   " +
                                        "<Data xmlns=\"http://www.onvif.org/ver10/schema\">   " +
                                        "<SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"ObjectId\" Value=\"15\" />   " +
                                        "</Data>   " +
                                        "</Message>  " +
                                        "</Message>   " +
                                        "<ProducerReference>   "+
                                              "<Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address>   "+
                                              "<ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" />   "+
                                              "<Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" />   "+
                                        "</ProducerReference>"+
                                  "</NotificationMessage>"+
                            "</Notify>"+
                            "</Body>"+
                            "</Envelope>";*/
            
            string request =    "<?xml version=\"1.0\" encoding=\"UTF-8\"?> " + 
                                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsa=\"http://www.w3.org/2005/08/addressing\" xmlns:wstop=\"http://docs.oasis-open.org/wsn/t-1\" xmlns:wsnt=\"http://docs.oasis-open.org/wsn/b-2\" xmlns:tet=\"http://www.onvif.org/ver10/events/wsdl\" xmlns:tns1=\"http://www.onvif.org/ver10/topics\" xmlns:tt=\"http://www.onvif.org/ver10/schema\"> " + 
                                "<SOAP-ENV:Header>"+
                                "<wsa:Action>urn:#Notify</wsa:Action>"+
                                "<wsa:ReplyTo><wsa:Address>http://example.com/business/client1</wsa:Address></wsa:ReplyTo>" +
                                "<wsa:MessageID>http://example.com/6B29FC40-CA47-1067-B31D-00DD010662DA  http://example.com/6B29FC40-CA47-1067-B31D-00DD010662DA</wsa:MessageID>" +
                                "</SOAP-ENV:Header>" +
                                "<SOAP-ENV:Body>" +
                                "<wsnt:Notify>" +
                                "<wsnt:NotificationMessage>" +
                                "<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">" +
                                "tns1:RuleEngine/LineDetector/Crossed" +
                                "</wsnt:Topic>" +
                                "<wsnt:Message>" +
                                "<tt:Message UtcTime=\"2008-10-10T12:24:57.321\">" +
                                "<tt:Source>" +
                                "<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"1\"/>" +
                                "<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/>" +
                                "<tt:SimpleItem Value=\"MyImportantFence1\" Name=\"Rule\"/>" +
                                "</tt:Source>" +
                                "<tt:Data>" +
                                "<tt:SimpleItem Name=\"ObjectId\" Value=\"15\" />" +
                                "</tt:Data>" +
                                "</tt:Message>" +
                                "</wsnt:Message>" +
                                "</wsnt:NotificationMessage>" +
                                "<wsnt:NotificationMessage>" +
                                "<wsnt:Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\">" +
                                "tns1:RuleEngine/LineDetector/Crossed" +
                                "</wsnt:Topic>" +
                                "<wsnt:Message>" +
                                "<tt:Message UtcTime=\"2008-10-10T12:24:57.789\">" +
                                "<tt:Source>" +
                                "<tt:SimpleItem Name=\"VideoSourceConfigurationToken\" Value=\"1\"/>" +
                                "<tt:SimpleItem Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/>" +
                                "<tt:SimpleItem Value=\"MyImportantFence2\" Name=\"Rule\"/>" +
                                "</tt:Source>" +
                                "<tt:Data>" +
                                "<tt:SimpleItem Name=\"ObjectId\" Value=\"19\"/>" +
                                "</tt:Data>" +
                                "</tt:Message>" +
                                "</wsnt:Message>" +
                                "</wsnt:NotificationMessage>" +
                                "<tet:CurrentTime>2008-10-10T12:24:58</tet:CurrentTime>" +
                                "<tet:TerminationTime>2008-10-10T12:25:58</tet:TerminationTime>" +
                                "</wsnt:Notify>" +
                                "</SOAP-ENV:Body>" +
                                "</SOAP-ENV:Envelope>";
            /*
            string request = "<?xml version=\"1.0\" encoding=\"utf-8\"?> "+
" <Envelope "+
 " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "+
 " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" "+
 " xmlns=\"http://www.w3.org/2003/05/soap-envelope\"> "+
  " <Body> "+
    " <Notify "+
      " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "+
      " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" "+
      " xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> "+
      " <NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> "+
        " <SubscriptionReference> "+
          " <Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address> "+
          " <ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" /> "+
          " <Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" /> "+
        " </SubscriptionReference> "+
        " <Topic Dialect=\"TestDialect\" >SomeTopic</Topic> "+
        " <Message xmlns=\"http://docs.oasis-open.org/wsn/b-2\" > "+
          " <Message xmlns=\"http://www.onvif.org/ver10/schema\" UtcTime=\"2008-10-10T12:24:57.321\"> "+
            " <Source xmlns=\"http://www.onvif.org/ver10/schema\"> "+
              " <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoSourceConfigurationToken\" Value=\"1\"/> "+
              " <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/> "+
              " <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Value=\"MyImportantFence1\" Name=\"Rule\"/> "+
            " </Source> "+
            " <Data xmlns=\"http://www.onvif.org/ver10/schema\"> "+
              " <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"ObjectId\" Value=\"15\" /> "+
            " </Data> "+
          " </Message> "+
        " </Message> "+
        " <ProducerReference> "+
          " <Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address> "+
          " <ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" /> "+
          " <Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" /> "+
        " </ProducerReference> "+
      " </NotificationMessage> "+
    " </Notify> "+
  " </Body> "+
" </Envelope>";*/

            /*
            string request = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
            "<Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.w3.org/2003/05/soap-envelope\"> "+
            "<Body><Notify xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">" + 
            "<NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> <SubscriptionReference> <Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address> <ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" /> <Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" /> </SubscriptionReference> "+
            "<Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:ax=\"http://www.axis.com/2009/event/topics\">ax:Tampering</Topic> " +
            "<ProducerReference><Address xmlns=\"http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address><ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" /><Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" /></ProducerReference>" +
            "<Message xmlns=\"http://docs.oasis-open.org/wsn/b-2\" ><Message xmlns=\"http://www.onvif.org/ver10/schema\" UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\"><Source xmlns=\"http://www.onvif.org/ver10/schema\"><SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoSourceConfigurationToken\" Value=\"1\"/><SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/><SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Value=\"MyImportantFence1\" Name=\"Rule\"/></Source><Data xmlns=\"http://www.onvif.org/ver10/schema\"><SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"ObjectId\" Value=\"15\" /></Data></Message></Message>" +
            "</NotificationMessage></Notify></Body></Envelope>";
            */
            
            /*
            string request = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<Envelope " +
                " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                " xmlns=\"http://www.w3.org/2003/05/soap-envelope\"> " +
                "  <Body> " +
                "    <Notify " +
                "     xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "      xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "      xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> " +
                "      <NotificationMessage xmlns=\"http://docs.oasis-open.org/wsn/b-2\"> " +
                "        <Topic Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns:ax=\"http://www.axis.com/2009/event/topics\">ax:Tampering</Topic> " +
                "        <Message xmlns=\"http://docs.oasis-open.org/wsn/b-2\" > " +
                "          <Message xmlns=\"http://www.onvif.org/ver10/schema\" UtcTime=\"2008-10-10T12:24:57.321\" PropertyOperation=\"Initialized\"> " +
                "            <Source xmlns=\"http://www.onvif.org/ver10/schema\"> " +
                "             <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoSourceConfigurationToken\" Value=\"1\"/> " +
                "              <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"VideoAnalyticsConfigurationToken\" Value=\"2\"/> " +
                "              <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Value=\"MyImportantFence1\" Name=\"Rule\"/> " +
                "            </Source> " +
                "            <Data xmlns=\"http://www.onvif.org/ver10/schema\"> " +
                "              <SimpleItem xmlns=\"http://www.onvif.org/ver10/schema\" Name=\"ObjectId\" Value=\"15\" /> " +
                "            </Data> " +
                "          </Message> " +
                "        </Message> " +
                //"        <ProducerReference> " +
                //"          <Address xmlns=\"http://192.168.10.79http://www.w3.org/2005/08/addressing\">http://192.168.10.79</Address> " +
                //"          <ReferenceParameters xmlns=\"http://www.w3.org/2005/08/addressing\" /> " +
                //"          <Metadata xmlns=\"http://www.w3.org/2005/08/addressing\" /> " +
                //"        </ProducerReference> " +
                "      </NotificationMessage> " +
                "    </Notify> " +
                "  </Body> " +
                "</Envelope>";*/

            Notify(address, request);
        }

        public void Notify(string address, string request)
        {
            try
            {
                HttpTransport.HttpClient client = new HttpClient(address, 30000);
                client.SendSoapMessage(request);
            }
            catch (Exception exc)
            {
                // Do nothing
            }

        }

    }
}
