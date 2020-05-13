using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TestTool.Proxies.Onvif;
using TestTool.Proxies.WSDiscovery;
using TestTool.Tests.Common.Soap;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Onvif;
using TestTool.Tests.Engine.Base.Definitions;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.TestCases.Utils.Comparison;



namespace TestTool.Tests.TestCases.TestSuites
{
    public partial class ReceiverGeneralTestSuit : ReceiverTest
    {
        private Receiver CreateReceiverAnnexA2(out Receiver[] receivers)
        {
            Receiver receiver = null;
            receivers = GetReceivers();
            if (receivers == null || receivers.Length == 0)
            {
                var config = GetReceiverConfiguration();
                receiver = CreateReceiver(config);
                CheckReceiverConfiguration(receiver, config);
            }
            return receiver;
        }
        private void ValidateReceiverChanging<T>(string token, Action<Receiver, T> action, T parameter)
        {
            var receivers = GetReceivers();
            Assert(receivers != null, "No receivers returned", "Check that receivers list is not empty");
            var receiver = receivers.FirstOrDefault(r => r.Token == token);
            Assert(receiver != null, "Receiver list doesn't contain receiver",
                        string.Format("Check that receiver list contains receiver (token = '{0}')", token));
            action(receiver, parameter);

            receiver = GetReceiver(token);
            Assert(receiver != null, "Receiver wasn't returned", "Check that receiver was returned");
            action(receiver, parameter);
        }

        private void CheckReceiverMode(Receiver receiver, ReceiverMode receiverMode)
        {
            Assert(receiver.Configuration.Mode == receiverMode,
                    string.Format("Mode is {0}", receiver.Configuration.Mode),
                        string.Format("Validate of setting {0} mode", receiverMode));
        }

        public void CheckReceiverConfiguration(Receiver receiver, ReceiverConfiguration config, string responseType)
        {
            var receiverConfiguration = receiver.Configuration;
            bool ok = true;
            StringBuilder logger = new StringBuilder();
            //bool local = true;
            //if (receiverConfiguration.Mode != config.Mode)
            //{
            //    logger.AppendFormat("");
            //}
            ok &= receiverConfiguration.Mode == config.Mode;
            ok &= receiverConfiguration.MediaUri == config.MediaUri;
            ok &= receiverConfiguration.StreamSetup.Transport.Protocol == config.StreamSetup.Transport.Protocol;
            ok &= receiverConfiguration.StreamSetup.Stream == config.StreamSetup.Stream;

            Assert(ok,
                string.Format("Receiver parameters of receiver in {0} is not coincide with specified parameters", responseType),
                    "Check parameters of receiver created");
        }

        private void CheckReceiverConfiguration(Receiver receiver, ReceiverConfiguration config)
        {
            var receiverConfiguration = receiver.Configuration;
            bool ok = true;
            StringBuilder logger = new StringBuilder();

            bool local = CompareParameteres(receiverConfiguration.Mode,
                config.Mode, (param1, param2) => param1 == param2, logger, "Mode");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.MediaUri,
                config.MediaUri, (param1, param2) => param1 == param2, logger, "MediaUri");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.StreamSetup.Stream,
                config.StreamSetup.Stream, (param1, param2) => param1 == param2, logger, "Stream");
            ok &= local;
            local = CompareParameteres(receiverConfiguration.StreamSetup.Transport.Protocol,
                config.StreamSetup.Transport.Protocol, (param1, param2) => param1 == param2, logger,
                "Protocol");
            ok &= local;
            Assert(ok, logger.ToStringTrimNewLine(),
                    "Check receiver configuration");
        }

        private bool CompareParameteres<T>(T param1, T param2, Func<T, T, bool> compareOp,
            StringBuilder logger, string paramName)
        {
            bool ok = compareOp(param1, param2);
            if (!ok)
            {
                logger.AppendFormat("{0} is '{1}' but must be '{2}'{3}", paramName,
                    param1, param2, Environment.NewLine);
            }
            return ok;
        }

        private Receiver ConfigureReceiver(bool isPersistance, out bool isReceiverCreated)
        {
            Receiver[] receivers = null;
            isReceiverCreated = false;
            var receiver = CreateReceiverAnnexA2(out receivers);
            if (receiver == null)
            {
                receiver = receivers[0];
            }
            else
            {
                isReceiverCreated = true;
            }
            string token = receiver.Token;

            var config = CreateNewReceiverConfiguration(receiver);
            ConfigureReceiver(token, config);

            if (isPersistance)
                SystemReboot();
                //Reboot();

            ValidateReceiverChanging<ReceiverConfiguration>(token, CheckReceiverConfiguration, config);
            return receiver;
        }

        private ReceiverConfiguration CreateNewReceiverConfiguration(Receiver receiver)
        {
            var capabilities = GetServiceCapabilities();

            var config = new ReceiverConfiguration
                {
                    MediaUri = receiver.Configuration.MediaUri == "http://localhost/Valid/URI"
                               ? 
                               "http://localhost/Valid/NewURI" : "http://localhost/Valid/URI",
                    Mode = receiver.Configuration.Mode == ReceiverMode.AutoConnect
                           ? 
                           ReceiverMode.NeverConnect : ReceiverMode.AutoConnect,
                    StreamSetup = new StreamSetup
                        {
                            Transport = new Transport()
                                {
                                    Protocol = receiver.Configuration.StreamSetup.Transport.Protocol == TransportProtocol.HTTP
                                               ? 
                                               TransportProtocol.RTSP : TransportProtocol.HTTP
                                }
                        }
                };

            if (capabilities.RTP_MulticastSpecified && capabilities.RTP_Multicast)
                config.StreamSetup.Stream = receiver.Configuration.StreamSetup.Stream == StreamType.RTPMulticast
                                            ?
                                            StreamType.RTPUnicast : StreamType.RTPMulticast;
            else
                config.StreamSetup.Stream = receiver.Configuration.StreamSetup.Stream;

            return config;
        }

        private void SystemReboot()
        {
            Reboot();
            SoapMessage<HelloType> hello = ReceiveHelloMessage();
            string reason = null;
            Assert(ValidateHelloMessage(hello, null, out reason), reason, "Validating hello message");
        }

        private Receiver CreateReceiverWithValidation(bool isPersistance)
        {
            Receiver receiver = null;


            receiver = CreateReceiver();
            var config = GetReceiverConfiguration();
            string token = receiver.Token;

            if (isPersistance)
                SystemReboot();
                //Reboot(); 

            ValidateReceiverChanging<ReceiverConfiguration>(token, CheckReceiverConfiguration, config);
            return receiver;
        }

        private Receiver[] GetReceiversCountToCreate(out int count)
        {
            ReceiverServiceCapabilities capabilities = GetServiceCapabilities();

            Assert(capabilities != null, "No capabilities returned", "Check that capabilities were returned");

            var receivers = GetReceivers();

            Assert(receivers != null, "No receivers returned", "Check that receivers list is not empty");

            count = capabilities.SupportedReceivers - receivers.Length;

            return receivers;
        }

        private ReceiverConfiguration GetReceiverConfiguration()
        {
            var receiverConfiguration = new ReceiverConfiguration()
            {
                MediaUri = "http://localhost/Valid/URI",
                Mode = ReceiverMode.NeverConnect,
                StreamSetup = new StreamSetup()
                {
                    Stream = StreamType.RTPUnicast,
                    Transport = new Transport()
                    {
                        Protocol = TransportProtocol.UDP
                    }
                }
            };
            return receiverConfiguration;
        }

        private Receiver CreateReceiver()
        {
            Receiver receiver = null;

            int receiverCountToCreate;
            Receiver[] receivers = GetReceiversCountToCreate(out receiverCountToCreate);

            var receiverConfiguration = GetReceiverConfiguration();

            string token;

            if (receiverCountToCreate == 0)
            {
                bool isDelete = false;
                foreach (var item in receivers)
                {
                    token = item.Token;
                    try
                    {
                        DeleteReceiver(token);
                        isDelete = true;
                        break;
                    }
                    catch (FaultException exc)
                    {
                        if (exc.IsValidOnvifFault("Receiver/Action/CannotDeleteReceiver"))
                        {
                            LogStepEvent(string.Format("Can't delete receiver (token = {0})", token));
                            StepPassed();
                        }
                        else
                        {
                            throw exc;
                        }
                    }
                }
                Assert(isDelete, "No receivers delete",
                    "Check that receiver was deleted");
            }

            receiver = CreateReceiver(receiverConfiguration);

            Assert(receiver != null, "Receiver is not returned",
                    "Check that receiver was returned in CreateReceiverResponse");

            CheckReceiverConfiguration(receiver, receiverConfiguration, "CreateReceiverResponse");

            return receiver;
        }

        private void ValidateFullReceiversList(Receiver[] fullList)
        {
            // check that tokens are unique
            bool tokensOk = true;
            StringBuilder logger = new StringBuilder();

            List<string> logged = new List<string>();

            foreach (var item in fullList)
            {
                string token = item.Token;
                int count = fullList.Where(R => R.Token == token).Count();
                if (count != 1)
                {
                    tokensOk = false;
                    if (!logged.Contains(token))
                    {
                        logged.Add(token);
                        logger.AppendFormat("Token '{0}' is not unique{1}", token, Environment.NewLine);
                    }
                    //break; // ?
                }

                if (item.Configuration == null)
                {
                    logger.AppendFormat("Configuration element is missing for item with token '{0}'{1}", token, Environment.NewLine);
                }
            }

            Assert(tokensOk, logger.ToStringTrimNewLine(),
                   "Validate receiver list got from GetReceivers", null);
        }

        private void CompareReceivers(Receiver receiverFromList, Receiver receiverByToken, AssertDelegate assert)
        {
            bool ok = true;
            StringBuilder logger = new StringBuilder();
            string token = receiverByToken.Token;

            if (receiverFromList.Token != receiverByToken.Token)
            {
                ok &= false;
                logger.Append(string.Format("Token of receiver in GetReceiverResponse is {0} " +
                    "but in GetReceivesResponse is {1}{2}",
                        receiverByToken.Token, receiverFromList.Token, Environment.NewLine));
            }

            if (receiverFromList.Configuration.Mode != receiverByToken.Configuration.Mode)
            {
                ok &= false;
                logger.Append(string.Format("Mode of receiver in GetReceiverResponse is {0} " +
                    "but in GetReceivesResponse is {1}{2}",
                        receiverByToken.Configuration.Mode, receiverFromList.Configuration.Mode, Environment.NewLine));
            }

            if (receiverFromList.Configuration.MediaUri != receiverByToken.Configuration.MediaUri)
            {
                ok &= false;
                logger.Append(string.Format("MediaUri of receiver in GetReceiverResponse is {0} " +
                    "but in GetReceivesResponse is {1}{2}",
                        receiverByToken.Configuration.MediaUri, receiverFromList.Configuration.MediaUri, Environment.NewLine));
            }

            if (receiverFromList.Configuration.StreamSetup.Stream != receiverByToken.Configuration.StreamSetup.Stream)
            {
                ok &= false;
                logger.Append(string.Format("StreamSetup.Stream of receiver in GetReceiverResponse is {0} " +
                    "but in GetReceivesResponse is {1}{2}",
                        receiverByToken.Configuration.StreamSetup.Stream, receiverFromList.Configuration.StreamSetup.Stream, Environment.NewLine));
            }

            string parameter = "Transport";
            CompareTransports(receiverFromList.Configuration.StreamSetup.Transport,
                receiverByToken.Configuration.StreamSetup.Transport, logger,
                    receiverByToken.Token, ref ok, parameter);

            assert(ok, logger.ToStringTrimNewLine(),
                   string.Format("Compare receivers with token {0} in GetReceiversResponse and in GetReceiverResponse", token), null);
        }

        private void CompareTransports(Transport transport1, Transport transport2,
            StringBuilder logger, string token, ref bool ok, string parameter)
        {
            if (transport1 == null && transport2 == null)
            {
                return;
            }
            else if (transport1 == null)
            {
                ok &= false;
                logger.Append(string.Format("{0} of receiver (token='{1}') in GetReceiversResponse " +
                    "wasn't returned but transport of it in GetReceiverResponse was returned{2}",
                        token, Environment.NewLine));
                return;
            }
            else if (transport2 == null)
            {
                ok &= false;
                logger.Append(string.Format("{0} of receiver (token='{1}') in GetReceiverResponse " +
                    "wasn't returned but transport of it in GetReceiversResponse was returned{2}",
                        parameter, token, Environment.NewLine));
                return;
            }

            if (transport1.Protocol != transport2.Protocol)
            {
                ok &= false;
                logger.Append(string.Format("{0} Protocol of receiver (token='{1}') in GetReceiverResponse " +
                    "and in GetReceivesResponse{2} are different", parameter, token, Environment.NewLine));
            }
            parameter += ".Tunnel";
            CompareTransports(transport1.Tunnel, transport2.Tunnel, logger, token, ref ok, parameter);
        }
    }
}