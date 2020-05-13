using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.Discovery;

namespace DiscoveryServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscoveryServer server = new DiscoveryServer();

            server.Start("192.168.10.79", "http://192.168.10.209/onvif/services");
            Console.WriteLine("Discovery server started, press Enter to exit");

            server.SendHello();
            server.OnProbeReceived += OnProbeReceive;
           
            Console.ReadLine();
            server.SendBye();
            server.Shutdown();
        }
        protected static void OnProbeReceive(object sender, DiscoverProbeEventArgs e)
        {
            Console.WriteLine(string.Format("Probe received from {0}", e.Sender.ToString()));
            DiscoveryServer server = sender as DiscoveryServer;
            ProbeMatchesType matches = server.BuildProbeMatches(e.Probe);
            e.Response = matches;
        }
    }
}
