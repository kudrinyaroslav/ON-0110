using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using TestTool.Tests.Engine;
using TestTool.Tests.Definitions.Data;
using TestTool.GUI.Controllers;
using TestTool.Tests.Common.Discovery;
using TestTool.GUI.Data;
using EnvironmentSettings=TestTool.Tests.Common.TestEngine.EnvironmentSettings;

namespace TestTool.GUI
{
    [XmlRoot("Parameters")]
    public class SerializableTestingParameters
    {
        public DeviceParameters Device { get; set; }
        public TestParameters TestParameters { get; set; }
        public Output Output { get; set; }
        public SessionInfo SessionInfo { get; set; }

        [XmlArrayItem(ElementName = "Value")]
        public XmlElement[] Advanced { get; set; }
        
        public TestSuiteParameters GetTestSuiteParameters()
        {
            TestSuiteParameters parameters = new TestSuiteParameters();

            if (Device != null)
            {
                parameters.Address = Device.DeviceServiceAddress;

                //parameters.CameraUUID
            }

            if (string.IsNullOrEmpty(parameters.Address))
            {
                Console.WriteLine("Mandatory parameters (Device address) not defined!");
                return null;
            }

            Timeouts defTimeouts = new Timeouts();
            TestSettings defSettings = new TestSettings();

            if (TestParameters != null)
            {
                parameters.MessageTimeout = (0 != TestParameters.MessageTimeout)
                                                ? TestParameters.MessageTimeout
                                                : defTimeouts.Message;
                parameters.RebootTimeout = (0 != TestParameters.RebootTimeout)
                                               ? TestParameters.RebootTimeout
                                               : defTimeouts.Reboot;
                parameters.RecoveryDelay = TestParameters.TimeBetweenRequests;
                parameters.TimeBetweenTests = TestParameters.TimeBetweenTests;
                parameters.OperationDelay = (0 != TestParameters.OperationDelay)
                                                ? TestParameters.OperationDelay
                                                : defSettings.OperationDelay;

                parameters.UserName = TestParameters.UserName;
                parameters.Password = TestParameters.Password;

                parameters.EnvironmentSettings = new EnvironmentSettings();
                parameters.EnvironmentSettings.DefaultGateway = TestParameters.DefaultGatewayIpv4;
                parameters.EnvironmentSettings.DefaultGatewayIpv6 = TestParameters.DefaultGatewayIpv6;
                parameters.EnvironmentSettings.DnsIpv4 = TestParameters.DnsIpv4;
                parameters.EnvironmentSettings.DnsIpv6 = TestParameters.DnsIpv6;
                parameters.EnvironmentSettings.NtpIpv4 = TestParameters.NtpIpv4;
                parameters.EnvironmentSettings.NtpIpv6 = TestParameters.NtpIpv6;

                if (!string.IsNullOrEmpty(TestParameters.Address))
                {
                    // get all "own" addresses;

                    List<NetworkInterfaceDescription> nics = DiscoveryHelper.GetNetworkInterfaces();
                    
                    // select required address (compare strings)
                    foreach (NetworkInterfaceDescription nic in nics)
                    {
                        if (nic.IP.ToString() == TestParameters.Address)
                        {
                            parameters.NetworkInterfaceController = nic;
                            break;
                        }
                    }

                    if (parameters.NetworkInterfaceController != null)
                    {
                        // define device IP
                        bool ipv6 = (parameters.NetworkInterfaceController.IP != null) &&
                            (parameters.NetworkInterfaceController.IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
                        parameters.CameraIP =  DiscoveryUtils.GetIP(Device.DeviceIP, ipv6);
                    }

                }     
       
                parameters.UseEmbeddedPassword = TestParameters.UseEmbeddedPassword;
                parameters.Password1 = TestParameters.Password1;
                parameters.Password2 = TestParameters.Password2;
                parameters.SecureMethod = !string.IsNullOrEmpty(TestParameters.SecureMethod)
                                              ? TestParameters.SecureMethod
                                              : defSettings.SecureMethod;
                parameters.PTZNodeToken = TestParameters.PTZNodeToken;
                parameters.VideoSourceToken = TestParameters.VideoSourceToken;
                parameters.EventTopic = TestParameters.EventTopic;
                parameters.SubscriptionTimeout = (0 != TestParameters.SubscriptionTimeout)
                                                     ? TestParameters.SubscriptionTimeout
                                                     : defSettings.SubscriptionTimeout;
                parameters.TopicNamespaces = TestParameters.TopicNamespaces;
                parameters.RelayOutputDelayTimeMonostable = (0 != TestParameters.RelayOutputDelayTime)
                                                                ? TestParameters.RelayOutputDelayTime
                                                                : defSettings.RelayOutputDelayTimeMonostable;


            }

            //
            
            return parameters;
        }
        
    }


    public class DeviceParameters
    {
        public string DeviceServiceAddress { get; set; }
        public string Model { get; set; }
        public string DeviceIP { get; set; }
    }

    public class TestParameters
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public int MessageTimeout { get; set;}
        public int RebootTimeout { get; set;}
        public int TimeBetweenRequests { get; set;}
        public int TimeBetweenTests { get; set;}
        public int OperationDelay { get; set; }

        public string DnsIpv4 { get; set; }
        public string NtpIpv4 { get; set; }
        public string DnsIpv6 { get; set; }
        public string NtpIpv6 { get; set; }
        public string DefaultGatewayIpv4 { get; set; }
        public string DefaultGatewayIpv6 { get; set; }
        public string Address { get; set; } 

        public bool UseEmbeddedPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string SecureMethod { get; set; }
        public string PTZNodeToken { get; set; }
        public string VideoSourceToken { get; set; }
        public string EventTopic { get; set; }
        public int SubscriptionTimeout { get; set; }
        public string TopicNamespaces { get; set; }
        public int RelayOutputDelayTime { get; set; }

        public int SearchTimeout { get; set; }
        public string RecordingToken { get; set; }
        public string MetadataFilter { get; set; }
    }
    
    public class Output
    {
        // default: current
        public string Directory { get; set; }
        public bool CreateNestedFolder { get; set; }
        // default: Report-Device-Date.pdf
        public string Report { get; set; }
        // default: DoC-Device.pdf
        public string DeclarationOfConformance { get; set; }
        // if empty - no log
        public string TestLog { get; set; }
        // if empty - no log
        public string FeatureDefinitionLog { get; set; }

    }

    public class SessionInfo
    {
        public TesterInfo TesterInfo { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public string OtherInformation { get; set; }
    }

}
