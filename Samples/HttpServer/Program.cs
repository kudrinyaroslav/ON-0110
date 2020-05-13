using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using TestTool.Proxies.Event;
using TestTool.Tests.Common.Discovery;
using Events = TestTool.Tests.Common.NotificationConsumer;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            /*using (Events.NotificationConsumer consumer = new Events.NotificationConsumer("http://192.168.10.79:7070/onvif_notify_server/"))
            {
                consumer.Start();
                consumer.OnNotify += OnMessageNotify;
                consumer.OnError += OnErrorReceived;
                Console.ReadLine();
            }*/
            using (HttpSoapServer server = new HttpSoapServer("http://192.168.10.79:8080/events/"))
            {
                server.Start();
                server.OnMessage += OnMessageReceived;
                server.OnError += OnErrorReceived;
                Console.WriteLine("Server started. Press Enter to exit");
                Console.ReadLine();
            }
        }
        static void OnErrorReceived(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        static void OnMessageReceived(byte[] data)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }
        static void OnMessageNotify(SoapMessage<Notify> notify)
        {

        }
    }
}
