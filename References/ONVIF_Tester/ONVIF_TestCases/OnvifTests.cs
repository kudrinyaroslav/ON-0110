/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;


namespace ONVIF_TestCases
{
    /// <summary>
    /// ONVIF Test class.  Performs ONVIF test v1.0 tests
    /// </summary>
    public class OnvifTests
    {
        #region Form Constants

        public const string STEP_SPACING = "     ";
        public const string STEP_MSG_SPACING = "       ";
        public const string ERROR_MSG_PREFIX = "Error - ";

        public const string DEFAULT_DEVICE_TYPE = "dn:NetworkVideoTransmitter";

        private const int SINGLE_RECEIVE_TIMEOUT = 45000;

        private const string TMP_SCOPE_STRING = "TEMP_SCOPE_VALUE";

        private const string RTSP_RTP_UDP_CONNECTION = "RTP/AVP/UDP";
        private const string RTSP_RTP_TCP_CONNECTION = "RTP/AVP/TCP";

        private const string RTSP_CONNECTION_UNICAST = "unicast";

        private const string TIMEZONE_STRING = "PST8PDT,M3.2.0,M11.1.0"; // the /2 local time switch was removed as a requriement in test ver 1.01
        // PST = designation for standard time when daylight saving is not in force
        // 8 = offset in hours = 5 hours west of Greenwich meridian (i.e. behind UTC)
        // PDT = designation when daylight saving is in force (if omitted there is no daylight saving)
        // , = no offset number between code and comma, so default to one hour ahead for daylight saving
        // M3.2.0 = when daylight saving starts = the 0th day (Sunday) in the second week of month 3 (March)
        // /2, = the local time when the switch occurs = 2 a.m. in this case
        // M11.1.0 = when daylight saving ends = the 0th day (Sunday) in the first week of month 11 (November). No time is given here so the switch occurs at 02:00 local 


        private const int CameraBootWaitTime = 2000;

        #endregion

        #region Form Variables

        

        //private ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface = new ONVIF_NetworkInterface.NetworkInterface_Class();
        private ONVIF_TestCases.TestMessages TestMessage = new ONVIF_TestCases.TestMessages();



        #endregion

        public void InitTestMessanger()
        {
            TestMessage.InitSchemaCollection();
        }

        private string GetHelloResponse(int timeout, 
                                        ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters,
                                        ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            int messageCount = 1000;
            string messageReceived = "";
            IPEndPoint receivedFrom;
            string[] uuid1, uuid2;
            int totalTimeout = 0;
            // listen for Hello messages untill the correct one is found or no more messages are received

            System.DateTime endTime = System.DateTime.Now.AddMilliseconds(timeout);

            while (messageCount-- > 0)
            {
                if(System.DateTime.Now > endTime)
                    throw new ONVIF_NetworkInterface.NetworkInterface_TimeoutException("Receive message timeout");

                totalTimeout = (endTime - System.DateTime.Now).Milliseconds + ((endTime - System.DateTime.Now).Seconds * 1000) + ((endTime - System.DateTime.Now).Minutes * 60000);

                messageReceived = NetworkInterface.UDP_Listen(totalTimeout, out receivedFrom);

                // if this is from the target IP address, or one of the other IP address on the device
                //  OR the UUID of the device matches, it should be from the target

                try
                {
                    RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(messageReceived, typeof(RemoteDiscovery.HelloType));

                    // first check to see if it was from the target
                    if (receivedFrom.Address.ToString() == Parameters.Target_IP)
                        break;

                    if ((Parameters.EPR != null) &&
                         (Parameters.EPR.Address != null) &&
                         (Parameters.EPR.Address.Value != null) &&
                         (HT.EndpointReference != null) &&
                         (HT.EndpointReference.Address != null) &&
                         (HT.EndpointReference.Address.Value != null))
                    {
                        uuid1 = HT.EndpointReference.Address.Value.Split(new char[] { '-' });
                        uuid2 = Parameters.EPR.Address.Value.Split(new char[] { '-' });

                        if ((uuid1.Length >= 5) && (uuid2.Length >= 5) &&
                            (uuid1[4] == uuid2[4]))
                        //if (HT.EndpointReference.Address.Value == Parameters.EPR.Address.Value)
                        {
                            // so the IP address doesn't match but the Endpoint Reference UUID does, this SHOULD be the same device but ask the user 
                            // to make sure
                            string msgString = "Hello Message received from device " + HT.EndpointReference.Address.Value + Environment.NewLine;
                            msgString += "IP address is different then expected, received from " + receivedFrom.Address.ToString() + " instead of " + Parameters.Target_IP + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                            msgString += "Is this the NVT being tested and if so do you wish to use this new IP address for the rest of the tests?";
                            ExceptionMessageBox mbox = new ExceptionMessageBox(msgString, "IP Address conflict", ExceptionMessageBoxButtons.Custom);

                            mbox.SetButtonText("Use " + Parameters.Target_IP, "Use " + receivedFrom.Address.ToString(), "This message not from NVT");

                            mbox.Show(null);

                            if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button1)  // use the original
                            {
                                // break and return
                                break;
                            }
                            if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button2) // use the new IP address
                            {
                                Parameters.UpdateIPaddress(Parameters.Target_IP, receivedFrom.Address.ToString());
                                break;
                            }
                            if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button3) // not from the NVT, keep searching
                            {
                                // continue searching
                            }

                            break;
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            if (messageCount == 0)
                throw new ONVIF_NetworkInterface.NetworkInterface_TimeoutException("Hello Message not found");


            return messageReceived;
        }

        /// <summary>
        /// Poll the selected device and store the ProbeMatches type information in the
        /// parameters
        /// </summary>
        /// <param name="IP"></param>
        private void ProbeDevice(ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters,
                                ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string messageSent, messageReceived;
            RemoteDiscovery.ProbeMatchesType PMT;

            if (Parameters.Target_IP == "")
                return;

            try
            {
                NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                Scope.Text = new string[] { "" };
                messageSent = TestMessage.Build_ProbeRequest("", Scope);

                // send the message
                NetworkInterface.UDP_Send(messageSent);

                messageReceived = NetworkInterface.UDP_Listen(1000, Parameters.Target_IP);

                PMT = (RemoteDiscovery.ProbeMatchesType)TestMessage.Parse_SoapMessage(messageReceived, typeof(RemoteDiscovery.ProbeMatchesType));

                Parameters.EPR = PMT.ProbeMatch[0].EndpointReference;
                
            }
            catch (Exception e)
            {
                Parameters.EPR = null;
            }
            finally
            {
                NetworkInterface.UDP_Close();
            }

        }


        private bool Get_DeviceMediaServiceAddress(ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters,
                                                     ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string MessagesSent = "";
            string MessagesReceived = "";
            string soapFault = "";
            string errorMessages = "";

            

            MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Media });

            // send the message
            try
            {
                MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, MessagesSent, Parameters.UserName, Parameters.Password);
            }
            catch (Exception e)
            {
                // tell the user the NVC was unable to communicate with the device so no media service address was found
                ExceptionMessageBox mbox = new ExceptionMessageBox("POST of GetCapabilitiesRequest message failed, error = " + e.Message + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                mbox.Show(null);
                Parameters.Media_ServiceAddress = "";
                return false;
            }


            // check to make sure there wasn't an error
            if (TestMessage.Check_SoapFault(MessagesReceived, out soapFault))
            {
                // tell the user the NVC was unable to communicate with the device so no media service address was found
                ExceptionMessageBox mbox = new ExceptionMessageBox("GetCapabilitiesResponse returned SOAP error = " + soapFault + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                mbox.Show(null);
                Parameters.Media_ServiceAddress = "";
                return false;
            }

            // otherwise verify the response
            try
            {
                if (!TestMessage.Verify_GetCapabilitiesResponse(MessagesReceived, ref errorMessages))
                {
                    // tell the user the NVC was unable to communicate with the device so no media service address was found
                    ExceptionMessageBox mbox = new ExceptionMessageBox("GetCapabilitiesResponse message failed failed validation, error = " + errorMessages + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                    mbox.Show(null);
                    Parameters.Media_ServiceAddress = "";
                    return false;
                }
                else
                {
                    // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                    // this request was only for media
                    DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                    if (GCR.Capabilities == null)
                    {
                        // tell the user the NVC was unable to communicate with the device so no media service address was found
                        ExceptionMessageBox mbox = new ExceptionMessageBox("Required capabilities not found, GetCapabilitiesResponse Capabilities = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                        mbox.Show(null);
                        Parameters.Media_ServiceAddress = "";
                        return false;
                    }
                    else
                    {
                        if (GCR.Capabilities.Media == null)
                        {
                            // tell the user the NVC was unable to communicate with the device so no media service address was found
                            ExceptionMessageBox mbox = new ExceptionMessageBox("Required capabilities not found, GetCapabilitiesResponse Media = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                            mbox.Show(null);
                            Parameters.Media_ServiceAddress = "";
                            return false;
                        }
                        else
                        {
                            if (GCR.Capabilities.Media.XAddr == null)
                            {
                                // tell the user the NVC was unable to communicate with the device so no media service address was found
                                ExceptionMessageBox mbox = new ExceptionMessageBox("Required capabilities not found, GetCapabilitiesResponse Media service address = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                                mbox.Show(null);
                                Parameters.Media_ServiceAddress = "";
                                return false;
                            }
                            else
                            {
                                Parameters.Media_ServiceAddress = GCR.Capabilities.Media.XAddr;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                // tell the user the NVC was unable to communicate with the device so no media service address was found
                ExceptionMessageBox mbox = new ExceptionMessageBox("GetCapabilitiesResponse message failed failed validation, error = " + err.Message + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                mbox.Show(null);
                Parameters.Media_ServiceAddress = "";
                return false;
            }

            Parameters.Media_ServiceAddress = "";
            return false;
        }


        #region Discovery Tests

        /// <summary>
        /// Perform Discovery Multicast Hello test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_MULTICAST_HELLO(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string errorMessages = "";
            string soapFault; 
            string results = "";
            
            
            // this test requires the target IP, multicast, port and TTL
            if((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if(Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            // probe the device
            if(test.CurrentStep == 0)
                ProbeDevice(ref Parameters, ref NetworkInterface);

            
            switch (test.CurrentStep) // TODO: PERFORM TEST HERE 
            {

                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SystemReboot message" + Environment.NewLine;
                    try
                    {
                        // setup the network interface for the buy message
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        // build the message
                        test.MessagesSent = TestMessage.Build_SystemRebootRequest();

                        // setup the network interface
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // send the message
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SystemRebootResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SystemRebootResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Response Message validated" + Environment.NewLine;
                            stepPassed = true;

                            // Print out the reboot message
                            DeviceManagement.SystemRebootResponse SRR = (DeviceManagement.SystemRebootResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SystemRebootResponse));
                            results += STEP_MSG_SPACING + "Response Message received - " + SRR.Message + Environment.NewLine;

                            // no other validatation required


                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Receive multicast HELLO Message" + Environment.NewLine;

                    // setup the network interface
                    NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                    // removed in test spec 1.01 to reduce user interaction
                    //ExceptionMessageBox mbox = new ExceptionMessageBox("This will check that the NVT transmitts a multicast \"Hello\" message on startup, please reboot NVT.", "powercycle NVT", ExceptionMessageBoxButtons.Custom);
                    //mbox.SetButtonText("NVT Rebooting", "Skip");

                    //mbox.Show(null);

                    //if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button2)  // stop the test
                    //{
                    //    test.StepComplete(true);
                    //    test.TestComplete = true;
                    //    test.TestSkipped = true;
                    //    test.Action = TestCases_Class.TestActions.Skip;
                    //    break;
                    //}

                    try
                    {
                        
                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.RebootTime + Parameters.TestTimeout, IPAddress.Any.ToString());
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    //This test is only verifying that the unit sent a "hello" message
                    try
                    {
                        test.TestObject = TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));
                        results += STEP_MSG_SPACING + "Multicast Hello Message received" + Environment.NewLine;
                        test.StepComplete(true);
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        test.StepComplete(false);
                    }

                    break;

         
                default:
                    test.TestComplete = true;
                    break;
            }

            // if the test is complete return the Passed/failed status
            if (test.TestComplete)
            {
                if (test.Action == TestCases_Class.TestActions.Skip)
                    results += "Test SKIPPED" + Environment.NewLine;
                else
                {
                    if (test.TestPassed)
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;
                    else
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;
                }

                // close the connection
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
                System.Threading.Thread.Sleep(CameraBootWaitTime); // let the unit boot up
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Multicast Hello validate test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_MULTICAST_HELLO_VALIDATE(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            bool stepPassed = true;
            string soapFault;            
            string errorMessages = "";
            

            // this test requires the target IP, multicast, port and TTL
            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            // probe the device
            if (test.CurrentStep == 0)
                ProbeDevice(ref Parameters, ref NetworkInterface);

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SystemReboot message" + Environment.NewLine;
                    try
                    {
                        // setup the network interface for the buy message
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        // build the message
                        test.MessagesSent = TestMessage.Build_SystemRebootRequest();

                        // setup the network interface
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // send the message
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SystemRebootResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SystemRebootResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Response Message validated" + Environment.NewLine;
                            stepPassed = true;

                            // Print out the reboot message
                            DeviceManagement.SystemRebootResponse SRR = (DeviceManagement.SystemRebootResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SystemRebootResponse));
                            results += STEP_MSG_SPACING + "Response Message received - " + SRR.Message + Environment.NewLine;

                            // no other validatation required


                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Receive multicast HELLO message" + Environment.NewLine;

                    // setup the network interface
                    NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                    // open a dialog box and tell the users to start the NVT
                    // removed in test spec 1.01 to reduce user interaction
                    //ExceptionMessageBox mbox = new ExceptionMessageBox("This will check that the NVT transmitts a valid multicast \"Hello\" message on startup, please reboot NVT.", "powercycle NVT", ExceptionMessageBoxButtons.Custom);
                    //mbox.SetButtonText("NVT Rebooting", "Skip");

                    //mbox.Show(null);

                    //if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button2){ // stop the test
                    //    test.StepComplete(true);
                    //    test.TestComplete = true;
                    //    test.TestSkipped = true;
                    //    test.Action = TestCases_Class.TestActions.Skip;
                    //    break;
                    //}

                    try
                    {


                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    //This test verifying that the unit sent a "hello" message
                    try
                    {
                        test.TestObject = TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));
                        results += STEP_MSG_SPACING + "Multicast Hello Message received" + Environment.NewLine;
                        stepPassed = true;                        
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;                        
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Verify HELLO message" + Environment.NewLine;

                    
                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);                        
                        break;
                    }

                    // take the object received and validate it
                    try
                    {
                        // now verify the message received in step one
                        if (!TestMessage.Verify_HelloResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message validated" + Environment.NewLine;
                            
                            // perform any test specific validation (beyond the XML validation)
                            // now make sure the scopes are present
                            RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));

                            // according to Annex A of the ONVIF test spec 1.0
                            RemoteDiscovery.HelloType Hello = new RemoteDiscovery.HelloType();
                            Hello.Types = DEFAULT_DEVICE_TYPE;
                            Hello.Scopes = new RemoteDiscovery.ScopesType();
                            Hello.Scopes.Text = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Location, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Type};


                            if (TestMessage.Compare_RemoteDiscovery_ScopesType(Hello.Scopes, HT.Scopes, ref errorMessages))
                                stepPassed = true;
                            else
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message did not contain neccissary objects.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }

                            
                        }
                       
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.Action == TestCases_Class.TestActions.Skip)
                    results += "Test SKIPPED" + Environment.NewLine;
                else
                {
                    if (test.TestPassed)
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;
                    else
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;
                }

                // close the connection
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
                System.Threading.Thread.Sleep(CameraBootWaitTime); // let the unit boot up
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Multicast Scope Search test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_MULTICAST_SCOPE_SEARCH(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;
            bool scopeFound;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetScopesRequest message" + Environment.NewLine;

                    // build the message
                    test.MessagesSent = TestMessage.Build_GetScopesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch(Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }

                    test.StepComplete(stepPassed);                    
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetScopesResponse message" + Environment.NewLine;

                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }
                    
                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetScopesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                            test.TestComplete = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;
                            stepPassed = true;
                            
                            // perform any test specific validation (beyond the XML validation)

                            // now make sure the scopes are present
                            DeviceManagement.GetScopesResponse GSR = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));
                            
                            if (TestMessage.Compare_DeviceManagement_GetScopesResponse(null, GSR, ref errorMessages))
                            {
                                // remember these scopes
                                DeviceManagement.GetScopesResponse ScopesResponse = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));
                                Parameters.Scopes = ScopesResponse.Scopes;

                                foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                                {
                                    results += STEP_MSG_SPACING + "Device Scope found - " + scope.ScopeItem + Environment.NewLine;
                                }

                                stepPassed = true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message did not contain neccissary objects.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }

                            

                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    x = 0;

                    if (Parameters.Scopes == null)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Scopes found" + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }
                    

                    foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                    {
                        x++;
                        results += STEP_SPACING + "Step 3." + x.ToString() + " - Transmit multicast PROBE message" + Environment.NewLine;
                        results += STEP_SPACING + "Scope Item = " + scope.ScopeItem + Environment.NewLine;
                        scopeFound = false;
                        
                        try
                        {
                            // setup the network interface
                            NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                            //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                            RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                            Scope.Text = new string[] { scope.ScopeItem };
                            test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                            // send the message
                            // according to the test spec the timeout for this step is 500 ms
                            test.MessagesReceived = NetworkInterface.UDP_SendMulticast(Parameters.TestTimeout, test.MessagesSent, Parameters.Target_IP);

                            //NetworkInterface.Send(test.MessagesSent);
                            //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, IPAddress.Any.ToString());


                            scopeFound = true;
                        }
                        catch (Exception e)
                        {
                            //NetworkInterface.Disconnect();
                            NetworkInterface.UDP_Close();

                            if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                            {
                                results += STEP_MSG_SPACING + "Scope Item = " + scope.ScopeItem + " failed to respond within timeout period, test case timeout" + Environment.NewLine;
                                stepPassed &= false;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                                stepPassed &= false;
                                break;
                            }
                        }

                        if (scopeFound)
                        {

                            results += STEP_SPACING + "Step 4." + x.ToString() + " - Receive PROBE MATCH message" + Environment.NewLine;

                            // check to make sure there wasn't an error
                            if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                            {
                                test.SoapErrors += soapFault;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                                stepPassed &= false;
                                break;
                            }

                            // otherwise verify the response
                            try
                            {

                                if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                                {
                                    results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                                    errorMessages = "";
                                    stepPassed &= false;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                                    // perform any test specific validation (beyond the XML validation)
                                    // the ONVIF test spec only required that the mandatory XML elements be present to pass this test

                                    stepPassed &= true;
                                }
                            }
                            catch (Exception err)
                            {
                                // if it failes to parse the return message it is not a valid message
                                test.XML_Errors = err.Message;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                                stepPassed &= false;
                            }
                        }

                        if (!stepPassed)
                            break;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed); 
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;
                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Multicast Scope Search, with device information ommited test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_MULTICAST_SCOPE_SEARCH_OMITTED_DEVICE(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit multicast PROBE message" + Environment.NewLine;
                    results += STEP_SPACING + "Device and scope types are empty" + Environment.NewLine;

                    // setup the network interface
                    NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                    try
                    {
                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "" };
                        test.MessagesSent = TestMessage.Build_ProbeRequest("", Scope);

                        // send the message
                        // according to the test spec the timeout is supposed to be 500 milliseconds
                        test.MessagesReceived = NetworkInterface.UDP_SendMulticast(Parameters.TestTimeout, test.MessagesSent, Parameters.Target_IP);

                        //NetworkInterface.Send(test.MessagesSent);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, IPAddress.Any.ToString());
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive unicast PROBE MATCH message" + Environment.NewLine;
                    // the response has already been received, now validate it.
                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response

                    //since each vender seems to change the probe response from 

                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                            
                            stepPassed = true;
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);  
                    
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Multicast Scope Seach with invalid parmaters test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_MULTICAST_SCOPE_SEARCH_INVALID(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            bool stepPassed = true;
            string soapFault;
            string errorMessages = "";

            // this test requires the target URL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            switch (test.CurrentStep) 
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit multicast PROBE message with invalid device and scope types" + Environment.NewLine;
                    // setup the network interface
                    NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                    try
                    {
                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "blah" };
                        test.MessagesSent = TestMessage.Build_ProbeRequest("wrongTYPE", Scope);

                        // send the message
                        test.MessagesReceived = NetworkInterface.UDP_SendMulticast(Parameters.TestTimeout, test.MessagesSent, Parameters.Target_IP);

                        //NetworkInterface.Send(test.MessagesSent);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                            
                            test.StepComplete(true);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    // check to see if this was a probe matches response before failing the device
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        results += STEP_MSG_SPACING + "to invalid Probe message" + Environment.NewLine;
                        test.StepComplete(true);
                        break;
                    }

                    // otherwise verify the response

                    //since each vender seems to change the probe response from 
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            // this wasn't a probe matches response so the unit still passed
                            results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            // if the device responded then this is a test failure
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit responded to invalid Probe message" + Environment.NewLine;
                            stepPassed = false;
                        }
                    }
                    catch (Exception err)
                    {
                        // this wasn't a probe matches response so the unit still passed
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                        stepPassed = true;
                    }

                    test.StepComplete(stepPassed); 
                    break;
                                  

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Unicast Scope Search test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_UNICAST_SCOPE_SEARCH(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetScopesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetScopesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetScopesResponse message" + Environment.NewLine;

                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetScopesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                            test.TestComplete = true; // end of test
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;
                            stepPassed = true;

                            // now make sure the scopes are present
                            DeviceManagement.GetScopesResponse GSR = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));

                            if (TestMessage.Compare_DeviceManagement_GetScopesResponse(null, GSR, ref errorMessages))
                            {
                                // remember these scopes
                                DeviceManagement.GetScopesResponse ScopesResponse = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));
                                Parameters.Scopes = ScopesResponse.Scopes;

                                foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                                {
                                    results += STEP_MSG_SPACING + "Device Scope found - " + scope.ScopeItem + Environment.NewLine;
                                }

                                stepPassed = true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                                stepPassed = false;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    if (Parameters.Scopes == null)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Scopes found" + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    x = 0;
                    foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                    {
                        x++;
                        results += STEP_SPACING + "Step 3." + x.ToString() + " - Transmit unicast PROBE message" + Environment.NewLine;
                        results += STEP_SPACING + "Scope Item = " + scope.ScopeItem + Environment.NewLine;

                        // setup the network interface
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        try
                        {
                            RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                            Scope.Text = new string[] { scope.ScopeItem };
                            test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                            // send the message
                            NetworkInterface.UDP_Send(test.MessagesSent);
                            //NetworkInterface.Send(test.MessagesSent);
                        }
                        catch (Exception e)
                        {
                            if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                                test.TestComplete = true;
                                test.StepComplete(false);
                                break;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                                test.StepComplete(false);
                                break;
                            }
                        }



                        results += STEP_SPACING + "Step 4." + x.ToString() + " - Receive PROBE MATCH message" + Environment.NewLine;
                        try
                        {
                            test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                            //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                        }
                        catch (Exception e)
                        {
                            if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                                test.TestComplete = true;
                                test.StepComplete(false);
                                break;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                                test.StepComplete(false);
                                break;
                            }
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // otherwise verify the response
                        try
                        {
                            if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                            {
                                results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                                errorMessages = "";
                                stepPassed = false;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                                // perform any test specific validation (beyond the XML validation)
                                // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                            
                                
                                stepPassed = true;
                            }
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                            stepPassed = false;
                        }
                        // each test will subbimit its passed/failed status, if any fail the whole set fail
                        test.StepComplete(stepPassed);
                    }
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Unicast Scope Search, deice information omitted, test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_UNICAST_SCOPE_SEARCH_OMITTED_DEVICE(ref ONVIF_TestCases.TestCases_Class.Test_Type test, ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit unicast PROBE message" + Environment.NewLine;
                    results += STEP_SPACING + "Device and scope types are empty" + Environment.NewLine;

                    // setup the network interface
                    NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);


                    try
                    {
                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "" };
                        test.MessagesSent = TestMessage.Build_ProbeRequest("", Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                        
                        System.Threading.Thread.Sleep(200);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive unicast PROBE MATCH message" + Environment.NewLine;
                    // get the response
                    try
                    {

                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);

                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;

                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation"+ Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                            
                            stepPassed = true;
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Unicast Scope Search, invalid deice information, test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_UNICAST_SCOPE_SEARCH_INVALID(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            bool stepPassed = true;
            string soapFault;
            string errorMessages = "";

            // this test requires the target URL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit unicast PROBE message with invalid device and scope types" + Environment.NewLine;
                    // setup the network interface
                    NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                    try
                    {
                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "blah" };
                        test.MessagesSent = TestMessage.Build_ProbeRequest("wrongTYPE", Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;

                            test.StepComplete(false);
                            test.TestComplete = true;
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Verify device does not send PROBE MATCH message" + Environment.NewLine;

                    try
                    {
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                            test.StepComplete(true);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to see if this was a probe matches response before failing the device
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        results += STEP_MSG_SPACING + "to invalid Probe message" + Environment.NewLine;
                        test.StepComplete(true);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            // this wasn't a probe matches response so the unit still passed
                            results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            // if the device responded then this is a test failure
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit responded to invalid Probe message" + Environment.NewLine;
                            stepPassed = false;
                        }
                    }
                    catch (Exception err)
                    {
                        // this wasn't a probe matches response so the unit still passed
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + "Unit did not respond to invalid Probe message" + Environment.NewLine;
                        stepPassed = true;
                    }

                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Device, Scopes Configuration test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_DEVICE_SCOPES_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            RemoteDiscovery.HelloType Hello;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
                        
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            // probe the device
            if (test.CurrentStep == 0)
                ProbeDevice(ref Parameters, ref NetworkInterface);

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetScopesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetScopesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetScopesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetScopesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            test.StepComplete(false); 
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            // this test is done
                            test.TestComplete = true;
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;
                            stepPassed = true;
   
                            // now make sure the scopes are present
                            DeviceManagement.GetScopesResponse GSR = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));

                            if (TestMessage.Compare_DeviceManagement_GetScopesResponse(null, GSR, ref errorMessages))
                            {
                                // remember these scopes
                                DeviceManagement.GetScopesResponse ScopesResponse = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetScopesResponse));
                                Parameters.Scopes = ScopesResponse.Scopes;

                                foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                                {
                                    results += STEP_MSG_SPACING + "Device Scope found - " + scope.ScopeItem + Environment.NewLine;
                                }

                                stepPassed = true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                                stepPassed = false;
                            }

                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit SetScopesRequest message of a fixed scope type" + Environment.NewLine;

                    // this test is to make sure the fixed type is really fixed, this is done by taking
                    // the scope list received and modifying the fixed values

                    if (Parameters.Scopes == null)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Scopes found" + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // create a new local copy of the scopes
                    string[] newScopes = new string[Parameters.Scopes.Length];
                                       

                    // first modify the scopes
                    x = 0;
                    foreach (DeviceManagement.Scope scope in Parameters.Scopes)
                    {
                        if (scope.ScopeDef == DeviceManagement.ScopeDefinition.Fixed)
                        {
                            newScopes[x] = scope.ScopeItem;
                            x++;
                        }
                    }

                    if (x == 0)
                    {
                        // no fixed scope type was found
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Fixed Scopes found" + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // now build the message
                    test.MessagesSent = TestMessage.Build_SetScopesRequest(newScopes);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        // this test is done
                        test.TestComplete = true;
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive SOAP 1.2 fault response, Operation Prohibited/Scope Overwrite" + Environment.NewLine;
                    // the message has already been received

                    // check to make sure there was a SOAP error returned
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:OperationProhibited\\ter:ScopeOverwrite"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                        }
                            

                        test.StepComplete(stepPassed);
                        break;
                    }

                    // we were expecting a soap error
                    results += STEP_MSG_SPACING + "SOAP error NOT returned" + Environment.NewLine;
                    test.StepComplete(false);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit AddScopesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "List of new scopes types" + Environment.NewLine;

                    // create a new scope               
                    Parameters.Temporary_String = RemoteDiscovery.Constants.ScopeTypePrefix_Name + TMP_SCOPE_STRING;

                    results += STEP_SPACING + "Adding new scope - " + Parameters.Temporary_String + Environment.NewLine;

                    // build the message
                    test.MessagesSent = TestMessage.Build_AddScopesRequest(new string[] { Parameters.Temporary_String } );

                    // send the message
                    try
                    {
                        // get ready for the multicast hello message, setup the network interface
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                                                
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive AddScopesResponse message" + Environment.NewLine;
                                       

                    // check to make sure there was a SOAP error returned
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        // end test
                        test.TestCompleted();

                        break;
                    }

                    // otherwise validate
                    try
                    {
                        if (!TestMessage.Verify_AddScopesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Add Scopes Response Message failed validation" + Environment.NewLine;
                            test.StepComplete(false);
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            // this test is done
                            test.TestComplete = true;
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Add Scopes Response Message validated" + Environment.NewLine;

                            // Perform any test specific validation
                            

                            stepPassed = true;                                                    
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;
                    
                case 6:
                    results += STEP_SPACING + "Step 7 - Receive multicast HELLO message" + Environment.NewLine;
                    try
                    {

                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // now we need to verify that the new scope has been added
                    // take the object received and validate it
                    try
                    {
                        // now verify the message received in step one
                        if (!TestMessage.Verify_HelloResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // now make sure the scopes are present
                            RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));

                            // according to Annex A of the ONVIF test spec 1.0
                            Hello = new RemoteDiscovery.HelloType();
                            Hello.Types = DEFAULT_DEVICE_TYPE;
                            Hello.Scopes = new RemoteDiscovery.ScopesType();
                            Hello.Scopes.Text = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Location, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Type, 
                                                      Parameters.Temporary_String };


                            if (TestMessage.Compare_RemoteDiscovery_ScopesType(Hello.Scopes, HT.Scopes, ref errorMessages))
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message contained new scope - " + Parameters.Temporary_String + Environment.NewLine;                                
                                stepPassed = true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message did not contain the new scope.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }
                        }

                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                case 7:
                    results += STEP_SPACING + "Step 8 - Transmit unicast PROBE message" + Environment.NewLine;
                    results += STEP_SPACING + "New scopes types = " + Parameters.Temporary_String + Environment.NewLine;

                    try
                    {
                        // setup the network interface
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { Parameters.Temporary_String };
                        test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    test.StepComplete(true);
                    break;

                case 8:
                    results += STEP_SPACING + "Step 9 - Receive unicast PROBE MATCH message" + Environment.NewLine;
                    try
                    {
                        // according to the test spec the timeout for the probe match response is 500 ms
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                            
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                case 9:
                    results += STEP_SPACING + "Step 10 - Transmit DeleteScopesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "New scopes types = " + Parameters.Temporary_String + Environment.NewLine;

                    try
                    {
                        // get ready for the multicast hello message, setup the network interface
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // build the message
                        test.MessagesSent = TestMessage.Build_DeleteScopesRequest(new string[] { Parameters.Temporary_String });

                        // send the message

                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 10:
                    results += STEP_SPACING + "Step 11 - Receive DeleteScopesResponse message" + Environment.NewLine;
                    results += STEP_SPACING + "List of scopes types deleted" + Environment.NewLine;
                    // check to make sure there was a SOAP error returned
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        // end test
                        test.TestCompleted();
                        break;
                    }

                    // otherwise validate
                    try
                    {
                        DeviceManagement.RemoveScopesResponse RSR = new DeviceManagement.RemoveScopesResponse();
                        RSR.ScopeItem = new string[] { Parameters.Temporary_String };

                        if (!TestMessage.Verify_RemoveScopesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Add Scopes Response Message failed validation" + Environment.NewLine;
                            test.StepComplete(false);
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            // this test is done
                            test.TestComplete = true;
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Remove Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // this is an empty message, nothing more to validate
                            
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 11:
                    results += STEP_SPACING + "Step 12 - Receive multicast HELLO message" + Environment.NewLine;
                    try
                    {


                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }


                    try
                    {
                        

                        // now verify the message received in step one
                        if (!TestMessage.Verify_HelloResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Multicast Hello Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // now make sure the scopes are present
                            RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));

                            // according to Annex A of the ONVIF test spec 1.0
                            Hello = new RemoteDiscovery.HelloType();
                            Hello.Types = DEFAULT_DEVICE_TYPE;
                            Hello.Scopes = new RemoteDiscovery.ScopesType();
                            Hello.Scopes.Text = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Location, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Type};


                            if (TestMessage.Compare_RemoteDiscovery_ScopesType(Hello.Scopes, HT.Scopes, ref errorMessages))
                                stepPassed = true;
                            else
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message did not contain neccissary objects.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }
                        }

                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 12:
                    results += STEP_SPACING + "Step 13 - Transmit unicast PROBE message" + Environment.NewLine;
                    results += STEP_SPACING + "New scopes types = " + Parameters.Temporary_String + Environment.NewLine;

                    try
                    {
                        // setup the network interface
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { Parameters.Temporary_String };
                        test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    test.StepComplete(true);
                    break;

                case 13:
                    results += STEP_SPACING + "Step 14 - Verify device does not send PROBE MATCH message" + Environment.NewLine;

                    try
                    {
                        stepPassed = false;
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + "Unit did not send PROBE MATCH message." + Environment.NewLine;
                            results += STEP_MSG_SPACING + "Temporary scope has been deleted sucessfully" + Environment.NewLine;
                            test.StepComplete(true); // no since going any further, test is done
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false); // no since going any further, test is done
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            stepPassed = true;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;
                            results += STEP_MSG_SPACING + "Device responded (incorrectly) to probe request" + Environment.NewLine;
                            stepPassed = false;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = true;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Device, Bye Message test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_BYE_MESSAGE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = ""; 
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SystemReboot message" + Environment.NewLine;
                    try
                    {
                        // setup the network interface for the buy message
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        // build the message
                        test.MessagesSent = TestMessage.Build_SystemRebootRequest();

                        // setup the network interface
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // send the message
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SystemRebootResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SystemRebootResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Response Message validated" + Environment.NewLine;
                            stepPassed = true;

                            // Print out the reboot message
                            DeviceManagement.SystemRebootResponse SRR = (DeviceManagement.SystemRebootResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SystemRebootResponse));
                            results += STEP_MSG_SPACING + "Response Message received - " + SRR.Message + Environment.NewLine;
                            
                            // no other validatation required
                            

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Receive multicast BYE message" + Environment.NewLine;

                    try
                    {
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            //test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ByeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + "Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Response Message validated" + Environment.NewLine;
                            
                            // no validation required
                            
                            stepPassed = true;


                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                case 3:
                    //results += STEP_SPACING + "Step 4 - Wait " + Parameters.RebootTime.ToString() + " Milliseconds for unit to reset" + Environment.NewLine;
                    results += STEP_SPACING + "Prepare to pause for \"User defined boot time\" " + Parameters.RebootTime.ToString() + " ms" + Environment.NewLine;
                    test.StepComplete(true);
                    break;

                case 4:
                    results += STEP_SPACING + "Pause done, resuming test" + Environment.NewLine;
                    System.Threading.Thread.Sleep(Parameters.RebootTime);
                    test.StepComplete(true);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                //ExceptionMessageBox mbox = new ExceptionMessageBox("Is the device resetting?  If so please wait until the device is done booting before clicking \"Continue\".", "Stop", ExceptionMessageBoxButtons.Custom);
                //mbox.SetButtonText("Continue");

                //mbox.Show(null);
            }

            return results;
        }

        /// <summary>
        /// Perform Discovery Device, Soap Fault Detection test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DISCOVERY_SOAP_FAULT_MESSAGE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit unicast PROBE message" + Environment.NewLine;
                    try
                    {
                        // setup the network interface
                        NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "" };
                        Scope.MatchBy = "BADTYPE";
                        test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault response" + Environment.NewLine;
                    //results += STEP_SPACING + "Matching rule not supported" + Environment.NewLine;
                    try
                    {
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX  + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure the was a soap error returned
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "SOAP-ENV:Sender\\d:MatchingRuleNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                        }
                            
                     
                    }
                    else
                    {

                        // otherwise verify the response
                        try
                        {
                            if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                            {
                                results += STEP_MSG_SPACING + "Get Scopes Response Message failed validation" + Environment.NewLine;
                                stepPassed = false;
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                                errorMessages = "";
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                                // perform any test specific validation (beyond the XML validation)
                                // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                                                            
                                stepPassed = false;
                            }
                        }
                        catch(Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                            stepPassed = false;
                        }
                        // each test will subbimit its passed/failed status, if any fail the whole set fail
                    }
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }
        #endregion

        #region Device Tests

        /// <summary>
        /// Perform Device, WSDL URL Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_WSDL_URL(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetWsdlUrlRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetWsdlUrlRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetWsdlUrlResponse message" + Environment.NewLine;                    
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error, " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetWsdlUrlResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;
                             
                            DeviceManagement.GetWsdlUrlResponse WSDL_URL = (DeviceManagement.GetWsdlUrlResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetWsdlUrlResponse));
                            results += STEP_SPACING + "WSDL URL = " + WSDL_URL.WsdlUrl + Environment.NewLine;

                            // is the WSDL correctly formed?

                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }
                   

            return results;
        }

        /// <summary>
        /// Perform Device, All Capabilities Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_ALL_CAPABILITIES(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"ALL\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest( new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.All } );

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetCapabilitiesResponse message" + Environment.NewLine;
                    
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error, " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;
                            

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Required capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                if (GCR.Capabilities.Media == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Media capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                if (GCR.Capabilities.Device == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                if (stepPassed)
                                {
                                    results += STEP_MSG_SPACING + "All required capabilities found" + Environment.NewLine;
                                    stepPassed &= true;
                                }
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            

            return results;
        }

        /// <summary>
        /// Perform Device, Device Capabilities Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_DEVICE_CAPABILITIES(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"DEVICE\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Device });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetCapabilitiesResponse message" + Environment.NewLine;
                    results += STEP_SPACING + "Device capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error, " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;
                            
                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for device
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Required capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {                                
                                if (GCR.Capabilities.Device == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Device capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                try
                                {
                                    DeviceManagement.OnvifVersion[] versions = GCR.Capabilities.Device.System.SupportedVersions;
                                    for (int x = 0; x < versions.Length; x++)
                                    {
                                        results += STEP_MSG_SPACING + "Device supports ONVIF version " + versions[x].Major.ToString() + "." + versions[x].Minor.ToString() + Environment.NewLine;
                                    }
                                }
                                catch
                                {
                                    // this is a problem the version field is supposed to be filled.
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Supported ONVIF versions not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (stepPassed)
                                {                         


                                    results += STEP_MSG_SPACING + "All required capabilities found" + Environment.NewLine;
                                    stepPassed &= true;
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;


                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }
         


            return results;
        }

        /// <summary>
        /// Perform Device, Media Capabilities Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_MEDIA_CAPABILITIES(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"MEDIA\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Media });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetCapabilitiesResponse message" + Environment.NewLine;
                    results += STEP_SPACING + "Media capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error, " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for media
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Required capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                if (GCR.Capabilities.Media == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Media capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                if (stepPassed)
                                {
                                    results += STEP_MSG_SPACING + "All required capabilities found" + Environment.NewLine;
                                    stepPassed &= true;
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;


                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }
        

            return results;
        }

        /// <summary>
        /// Perform Device, Service Capabilities Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SERVICE_CATEGORY_CAPABILITIES(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"ANALYTICS\"" + Environment.NewLine;
                    
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Analytics });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetCapabilitiesResponse or SOAP 1.2 fault response" + Environment.NewLine;
                    results += STEP_SPACING + "Depending on device capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error, it is OK for this test
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported\\ter:NoSuchService"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"ANALYTICS\" capabilities no supported" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for media
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                // if a soap error wasn't returned the objects must not be null
                                if (GCR.Capabilities.Analytics == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Analytics not capabilities found" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + "Analytics capabilities found" + Environment.NewLine;                                    
                                    stepPassed &= true;
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"EVENTS\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Events });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetCapabilitiesResponse or SOAP 1.2 fault response" + Environment.NewLine;
                    results += STEP_SPACING + "Depending on device capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error, it is OK for this test
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported\\ter:NoSuchService"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"EVENTS\" capabilities no supported" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for media
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                // if a soap error wasn't returned the objects must not be null
                                if (GCR.Capabilities.Events != null)
                                {
                                    results += STEP_MSG_SPACING + "Events capabilities found" + Environment.NewLine;
                                    stepPassed &= true;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Events capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"IMAGING\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Imaging });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive GetCapabilitiesResponse or SOAP 1.2 fault response" + Environment.NewLine;
                    results += STEP_SPACING + "Depending on device capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported\\ter:NoSuchService"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"IMAGING\" capabilities no supported" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for media
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                if (GCR.Capabilities.Imaging != null)
                                {
                                    results += STEP_MSG_SPACING + "Imaging capabilities found" + Environment.NewLine;
                                    stepPassed &= true;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Imaging capabilities not found" + Environment.NewLine;
                                    stepPassed &= false;  
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step 7 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"PTZ\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.PTZ });

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 7:
                    results += STEP_SPACING + "Step 8 - Receive GetCapabilitiesResponse or SOAP 1.2 fault response" + Environment.NewLine;
                    results += STEP_SPACING + "Depending on device capabilities" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported\\ter:NoSuchService"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"PTZ\" capabilities no supported" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                            
                        }
                        test.StepComplete(stepPassed);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetCapabilitiesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the DUT MUST support device and media capiblities
                            // this request was only for media
                            DeviceManagement.GetCapabilitiesResponse GCR = (DeviceManagement.GetCapabilitiesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetCapabilitiesResponse));

                            if (GCR.Capabilities == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "capabilities not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                // no PTZ object in capibilties??
                                stepPassed = true;
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Soap Fault Detection test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SOAP_FAULT_MESSAGE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = ""; 
            string soapFault;
            bool stepPassed = true;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetCapabilitiesRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "CapabilityCategory = \"XYZ\"" + Environment.NewLine;
                     // build the message
                    test.MessagesSent = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.PTZ });

                    // replace >PTZ< with >XYZ<, since this is an illegal value it must be done manuall or the xml parser will throw
                    // an error
                    test.MessagesSent = test.MessagesSent.Replace(">PTZ<", ">XYZ<");

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault response" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure the device returned a SOAP error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        //results += STEP_MSG_SPACING + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        //results += STEP_MSG_SPACING + "as required" + Environment.NewLine;

                        // ERRATA #1 CHANGE
                        // according to If a service category is not understood by the DUT, it must generate a SOAP Error, specifically the generic fault response ‘InvalidArgVal’. See Test Case 8.2.6.
                        // ERRATA #2 CHANGE
                        // Change all occurrences of (ActionNotSupported/NoSuchService) to (InvalidArgVal).
                        //if (!TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported\\ter:NoSuchService"))

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                        }
                            

                        test.StepComplete(stepPassed);
                        break;
                    }

                    // this should not have happened so this test is a failure
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT return a SOAP error" + Environment.NewLine;
                    test.StepComplete(false);
                    break;


                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device Hostname Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_HOSTNAME_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = ""; 
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetHostnameRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetHostnameRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetHostnameResponse message" + Environment.NewLine;
                    results += STEP_SPACING + "FromDHCP = true/false, Name = <hostname>" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetHostnameResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetHostnameResponse GHR = (DeviceManagement.GetHostnameResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetHostnameResponse));

                            if (GHR.HostnameInformation != null)
                            {
                                results += STEP_MSG_SPACING + "HostnameInformation, from DHCP = " + GHR.HostnameInformation.FromDHCP.ToString() + Environment.NewLine;
                                results += STEP_MSG_SPACING + "HostnameInformation, Name = " + GHR.HostnameInformation.Name + Environment.NewLine;
                                stepPassed &= true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HostnameInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device Hostname Setup test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_HOSTNAME_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");


            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetHostnameRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "Set Hostname =\"Test\"" + Environment.NewLine;
                    // build the message
                    //System.Random rand = new Random((int)System.DateTime.Now.Ticks);
                    //Parameters.Temporary_String = "Test_" + (rand.Next() % 100).ToString();
                    Parameters.Temporary_String = "Test";
                    test.MessagesSent = TestMessage.Build_SetHostnameRequest(Parameters.Temporary_String);
                    

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetHostnameResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetHostnameResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the SetHostnameResponse just needs to be returned
                            DeviceManagement.SetHostnameResponse SHR = (DeviceManagement.SetHostnameResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SetHostnameResponse));
                                                       

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetHostnameRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetHostnameRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetHostnameResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetHostnameResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetHostnameResponse GHR = (DeviceManagement.GetHostnameResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetHostnameResponse));

                            if (GHR.HostnameInformation != null)
                            {
                                results += STEP_MSG_SPACING + "HostnameInformation, from DHCP = " + GHR.HostnameInformation.FromDHCP.ToString() + Environment.NewLine;
                                if (GHR.HostnameInformation.Name.Equals(Parameters.Temporary_String))
                                {
                                    results += STEP_MSG_SPACING + "HostnameInformation, Name set to = " + GHR.HostnameInformation.Name + Environment.NewLine;
                                    stepPassed &= true;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HostnameInformation, Name = " + GHR.HostnameInformation.Name + Environment.NewLine;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Hostname set failed" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HostnameInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device Hostname Setup with invalid name test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_INVALID_HOSTNAME_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");



            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetHostnameRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "Name=\"Test#$%\"" + Environment.NewLine;
                    // build the message
                    Parameters.Temporary_String = "Test#$%";
                    test.MessagesSent = TestMessage.Build_SetHostnameRequest(Parameters.Temporary_String);


                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidHostname"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                            
                        }
                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT returned a SOAP error" + Environment.NewLine;
                        stepPassed = false;
                    }                    
                    
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetHostnameRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetHostnameRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetHostnameResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetHostnameResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetHostnameResponse GHR = (DeviceManagement.GetHostnameResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetHostnameResponse));

                            if (GHR.HostnameInformation != null)
                            {
                                results += STEP_MSG_SPACING + "HostnameInformation, from DHCP = " + GHR.HostnameInformation.FromDHCP.ToString() + Environment.NewLine;
                                results += STEP_MSG_SPACING + "HostnameInformation, Name = " + GHR.HostnameInformation.Name + Environment.NewLine;
                                stepPassed &= true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "HostnameInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device DNS Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_DNS_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetDNSRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetDNSRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetDNSResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetDNSResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetDNSResponse GDR = (DeviceManagement.GetDNSResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetDNSResponse));

                            if (GDR.DNSInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "DNSInformation received" + Environment.NewLine;                                
                                
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DNSInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device DNS Setup test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_DNS_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetDNSRequest message" + Environment.NewLine;
                    //results += STEP_SPACING + "DNSManual=IPv4 DNS Server Address" + Environment.NewLine;
                    // build the message
                    DeviceManagement.IPAddress[] DNS_Server = new DeviceManagement.IPAddress[1];
                    DNS_Server[0] = new DeviceManagement.IPAddress();
                    Parameters.Temporary_String = "10.1.1.1";
                    DNS_Server[0].IPv4Address = Parameters.Temporary_String;
                    DNS_Server[0].Type = DeviceManagement.IPType.IPv4;
                    test.MessagesSent = TestMessage.Build_SetDNSRequest(DNS_Server, null);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetDNSResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetDNSResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // empty message, passed
                            stepPassed &= true;
                            results += STEP_MSG_SPACING + "SetDNSResponse received" + Environment.NewLine;

                            
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetDNSRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetDNSRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetDNSResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetDNSResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetDNSResponse GDR = (DeviceManagement.GetDNSResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetDNSResponse));

                            if (GDR.DNSInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "DNSInformation received" + Environment.NewLine;

                                // validate the DNS settings
                                if (GDR.DNSInformation.FromDHCP == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "FromDHCP is null" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GDR.DNSInformation.DNSManual == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DNSManual is null" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    // find the manual DNS info sent
                                    bool foundEntry = false;
                                    for (x = 0; x < GDR.DNSInformation.DNSManual.Length; x++)
                                    {
                                        if ((GDR.DNSInformation.DNSManual[x].IPv4Address.Equals(Parameters.Temporary_String)) &&
                                            (GDR.DNSInformation.DNSManual[x].Type == DeviceManagement.IPType.IPv4))
                                            foundEntry = true;
                                    }

                                    stepPassed &= foundEntry;
                                }

                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DNSInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device DNS Setup with invalid DNS test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_INVALID_DNS_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = ""; 
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");


            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetDNSRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "DNSManual=Invalid Server Address" + Environment.NewLine;
                    // build the message
                    DeviceManagement.IPAddress[] DNS_Server = new DeviceManagement.IPAddress[1];
                    DNS_Server[0] = new DeviceManagement.IPAddress();
                    Parameters.Temporary_String = "10.1.1.255";
                    DNS_Server[0].IPv4Address = Parameters.Temporary_String;
                    DNS_Server[0].Type = DeviceManagement.IPType.IPv4;
                    test.MessagesSent = TestMessage.Build_SetDNSRequest(DNS_Server, null);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidIPv4Address"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                    }
                    else
                    {
                        // A soap fault was not returned so thats bad
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "A SOAP error was NOT returned as expected " + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetDNSRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetDNSRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetDNSResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetDNSResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetDNSResponse GDR = (DeviceManagement.GetDNSResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetDNSResponse));

                            if (GDR.DNSInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "DNSInformation received" + Environment.NewLine;

                                if ((GDR.DNSInformation.DNSManual != null))
                                {
                                    // find the manual DNS info sent
                                    bool foundEntry = true;
                                    for (x = 0; x < GDR.DNSInformation.DNSManual.Length; x++)
                                    {
                                        if (GDR.DNSInformation.DNSManual[x].IPv4Address.Equals(Parameters.Temporary_String))
                                        {                        
                                            // this is bad, the invalid DNS address got saved
                                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Invalid DNS was saved" + Environment.NewLine;
                                            foundEntry = false;  

                                        }
                                    }

                                    stepPassed &= foundEntry;
                                }

                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DNSInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device NTP Inquery test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_NTP_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetNTPRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetNTPRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        
                        // MUST IF SUPPORTED
                        // if the device does not support the Get NTP Response request it must respond with a ActionNotSupported Soap error
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"GetNTPResponse\" command no supported" + Environment.NewLine;
                            stepPassed = true;

                            // stop the test
                            test.TestCompleted();
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);
                                                
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetNTPResponse GNTPR = (DeviceManagement.GetNTPResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetNTPResponse));

                            if (GNTPR.NTPInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "NTPInformation received" + Environment.NewLine;

                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device NTP Setup test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_NTP_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            DeviceManagement.NetworkHost[] NTP;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");


            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetNTPRequest message, Type = IPv4" + Environment.NewLine;
                    // build the message
                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.IPv4;
                    NTP[0].IPv4Address = "10.1.1.1";
                    //NTP[0].DNSname = "test";  removed in test version 1.01

                    Parameters.Temporary_Object = NTP;

                    test.MessagesSent = TestMessage.Build_SetNTPRequest(false, NTP);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;

                        // MUST IF SUPPORTED
                        // if the device does not support the Get NTP Response request it must respond with a ActionNotSupported Soap error
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"SetNTPRequest\" command no supported" + Environment.NewLine;
                            stepPassed = true;

                            // stop the test
                            test.TestCompleted();
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);

                        
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // empty message, passed
                            stepPassed &= true;
                            results += STEP_MSG_SPACING + "SetNTPResponse received" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetNTPRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetNTPRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetNTPResponse GNTPR = (DeviceManagement.GetNTPResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetNTPResponse));

                            if (GNTPR.NTPInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "NTPInformation received" + Environment.NewLine;

                                NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;

                                // verify the settings were saved
                                if (GNTPR.NTPInformation.FromDHCP != false)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "FromDHCP not set" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GNTPR.NTPInformation.NTPManual == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPManual not correct" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    // find the manual NTP info sent
                                    bool foundEntry = false;
                                    for (x = 0; x < GNTPR.NTPInformation.NTPManual.Length; x++)
                                    {
                                        if ((GNTPR.NTPInformation.NTPManual[x].Type == null) || (GNTPR.NTPInformation.NTPManual[x].Type != DeviceManagement.NetworkHostType.IPv4))
                                        {
                                            // type cannot be null
                                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation Type not set" + Environment.NewLine;
                                            stepPassed &= false;
                                            break;
                                        }

                                        
                                        if (GNTPR.NTPInformation.NTPManual[x].IPv4Address == NTP[0].IPv4Address)
                                            foundEntry = true;
                                        
                                    }

                                    if(!foundEntry)
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Correct NTP entry not found" + Environment.NewLine;


                                    stepPassed &= foundEntry;
                                }
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;



                /*
                 *              REMOVED AS PER CR_DNS_NTP1.doc
                 * 
                 *              ITEM #1 - Remove DNS tests from NTP test cases
                 * 
                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit SetNTPRequest message, Type = DNS" + Environment.NewLine;
                    // build the message
                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.DNS;                    
                    NTP[0].DNSname = "test";  

                    Parameters.Temporary_Object = NTP;

                    test.MessagesSent = TestMessage.Build_SetNTPRequest(false, NTP);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive SetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        // MUST IF SUPPORTED
                        // if the device does not support the Get NTP Response request it must respond with a ActionNotSupported Soap error
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"SetNTPRequest\" command no supported" + Environment.NewLine;
                            stepPassed = true;

                            // stop the test
                            test.TestCompleted();
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;                            
                        }
                        test.StepComplete(stepPassed);

                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // empty message, passed
                            stepPassed &= true;
                            results += STEP_MSG_SPACING + "SetNTPResponse received" + Environment.NewLine;
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step 7 - Transmit GetNTPRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetNTPRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 7:
                    results += STEP_SPACING + "Step 8 - Receive GetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetNTPResponse GNTPR = (DeviceManagement.GetNTPResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetNTPResponse));

                            if (GNTPR.NTPInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "NTPInformation received" + Environment.NewLine;

                                NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;

                                // verify the settings were saved
                                if (GNTPR.NTPInformation.FromDHCP != false)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "FromDHCP not set" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GNTPR.NTPInformation.NTPManual == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPManual not correct" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    // find the manual NTP info sent
                                    bool foundEntry = false;
                                    for (x = 0; x < GNTPR.NTPInformation.NTPManual.Length; x++)
                                    {
                                        if( (GNTPR.NTPInformation.NTPManual[x].Type == null) || (GNTPR.NTPInformation.NTPManual[x].Type != DeviceManagement.NetworkHostType.DNS))
                                        {
                                            // type cannot be null
                                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation Type not set" + Environment.NewLine;
                                            stepPassed &= false;
                                            break;
                                        }


                                        if (GNTPR.NTPInformation.NTPManual[x].DNSname == NTP[0].DNSname)
                                            foundEntry = true;

                                    }

                                    if (!foundEntry)
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Correct NTP entry not found" + Environment.NewLine;


                                    stepPassed &= foundEntry;
                                }
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;
                 */






                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device NTP Setup with invalid IP address test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_INVALID_IP_NTP_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            DeviceManagement.NetworkHost[] NTP;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetNTPRequest message" + Environment.NewLine;
                    // build the message
                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.IPv4;
                    NTP[0].IPv4Address = "10.1.1.255";
                    //NTP[0].DNSname = "test"; //removed in test version 1.01

                    Parameters.Temporary_Object = NTP;

                    test.MessagesSent = TestMessage.Build_SetNTPRequest(false, NTP);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        
                        // MUST IF SUPPORTED
                        // if the device does not support the Get NTP Response request it must respond with a ActionNotSupported Soap error
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"SetNTPRequest\" command no supported" + Environment.NewLine;
                            stepPassed = true;

                            // stop the test
                            test.TestCompleted();
                        }
                        else if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidIPv4Address"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;
                            
                        }
                        //test.StepComplete(stepPassed);                                               
                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT returned a SOAP error" + Environment.NewLine;
                        stepPassed = false;
                    }

                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetNTPRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetNTPRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetNTPResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetNTPResponse GNTPR = (DeviceManagement.GetNTPResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetNTPResponse));

                            if (GNTPR.NTPInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "NTPInformation received" + Environment.NewLine;

                                // make sure the NTP information is correct and the invalid IP wasn't saved
                                if (GNTPR.NTPInformation == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not complete" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    if (GNTPR.NTPInformation.NTPManual == null)
                                    {
                                        results += STEP_MSG_SPACING + "No NTP servers manually set - OK" + Environment.NewLine;
                                        stepPassed &= true;
                                    }
                                    else
                                    {
                                        NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;

                                        // find the manual NTP info sent                                        
                                        for (x = 0; x < GNTPR.NTPInformation.NTPManual.Length; x++)
                                        {
                                            try
                                            {
                                                if ((GNTPR.NTPInformation.NTPManual[x].IPv4Address != null) && (GNTPR.NTPInformation.NTPManual[x].IPv4Address.Equals(NTP[0].IPv4Address)))
                                                {
                                                    // problem
                                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation IPv4Address incorrectly set" + Environment.NewLine;
                                                    stepPassed &= false;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not correct - " + e.Message + Environment.NewLine;

                                                if (GNTPR.NTPInformation.NTPManual[x].IPv4Address == null)
                                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation IPv4Address not set" + Environment.NewLine;

                                                stepPassed &= false;
                                            }
                                        }

                                        if(stepPassed)
                                            results += STEP_MSG_SPACING + "NTPInformation IPv4Address was NOT incorrectly set" + Environment.NewLine;
                                                    
                                    }
                                }
                                
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device NTP Setup with invalid Name test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_INVALID_NAME_NTP_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            DeviceManagement.NetworkHost[] NTP;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetNTPRequest message" + Environment.NewLine;
                    // build the message
                    NTP = new DeviceManagement.NetworkHost[1];
                    NTP[0] = new DeviceManagement.NetworkHost();
                    NTP[0].Type = DeviceManagement.NetworkHostType.DNS;
                    //NTP[0].IPv4Address = "10.1.1.1"; // removed in test version 1.01
                    NTP[0].DNSname = "test#$%";

                    Parameters.Temporary_Object = NTP;

                    test.MessagesSent = TestMessage.Build_SetNTPRequest(false, NTP);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;

                        // MUST IF SUPPORTED
                        // if the device does not support the Get NTP Response request it must respond with a ActionNotSupported Soap error
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Receiver\\ter:ActionNotSupported"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error, " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "\"SetNTPRequest\" command no supported" + Environment.NewLine;
                            stepPassed = true;

                            // stop the test
                            test.TestCompleted();
                        }
                        else if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidDnsName"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed = false;

                        }
                        //test.StepComplete(stepPassed);
                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT returned a SOAP error" + Environment.NewLine;
                        stepPassed = false;
                    }

                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetNTPRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetNTPRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetDNSResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetNTPResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetNTPResponse GNTPR = (DeviceManagement.GetNTPResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetNTPResponse));

                            if (GNTPR.NTPInformation != null)
                            {
                                stepPassed &= true;
                                results += STEP_MSG_SPACING + "NTPInformation received" + Environment.NewLine;

                                // make sure the NTP information is correct and the invalid IP wasn't saved
                                if (GNTPR.NTPInformation == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not complete" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                else
                                {
                                    if (GNTPR.NTPInformation.NTPManual == null)
                                    {
                                        results += STEP_MSG_SPACING + "No NTP servers manually set - OK" + Environment.NewLine;
                                        stepPassed &= true;
                                    }
                                    else
                                    {
                                        NTP = (DeviceManagement.NetworkHost[])Parameters.Temporary_Object;

                                        // find the manual NTP info sent                                        
                                        for (x = 0; x < GNTPR.NTPInformation.NTPManual.Length; x++)
                                        {
                                            try
                                            {
                                                // the DNS name is not a required object so make sure it isn't null before 
                                                // testing against it
                                                if ((GNTPR.NTPInformation.NTPManual[x].DNSname != null) &&
                                                    (GNTPR.NTPInformation.NTPManual[x].DNSname.Equals(NTP[0].DNSname)))
                                                {
                                                    // problem
                                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation IPv4Address incorrectly set" + Environment.NewLine;
                                                    stepPassed &= false;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not correct - " + e.Message + Environment.NewLine;                                          

                                                stepPassed &= false;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "NTPInformation not returned" + Environment.NewLine;
                                stepPassed &= false;
                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device Information request test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_DEVICE_INFORMATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetDeviceInformationRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetDeviceInformationRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);                    
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetDeviceInformationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetDeviceInformationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetDeviceInformationResponse GDIR = (DeviceManagement.GetDeviceInformationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetDeviceInformationResponse));

                            if (GDIR.Manufacturer == null)
                            {
                                stepPassed &= false;
                                results += STEP_MSG_SPACING + "Manufacturer information not received" + Environment.NewLine;
                            } 
                            else
                                results += STEP_MSG_SPACING + "Manufacturer - " + GDIR.Manufacturer + Environment.NewLine;

                            if (GDIR.Model == null)
                            {
                                stepPassed &= false;
                                results += STEP_MSG_SPACING + "Model information not received" + Environment.NewLine;
                            } 
                            else
                                results += STEP_MSG_SPACING + "Model - " + GDIR.Model + Environment.NewLine;

                            if (GDIR.FirmwareVersion == null)
                            {
                                stepPassed &= false;
                                results += STEP_MSG_SPACING + "FirmwareVersion information not received" + Environment.NewLine;
                            }
                            else
                                results += STEP_MSG_SPACING + "FirmwareVersion - " + GDIR.FirmwareVersion + Environment.NewLine;

                            if (GDIR.SerialNumber == null)
                            {
                                stepPassed &= false;
                                results += STEP_MSG_SPACING + "SerialNumber information not received" + Environment.NewLine;
                            }
                            else
                                results += STEP_MSG_SPACING + "SerialNumber - " + GDIR.SerialNumber + Environment.NewLine;

                            if (GDIR.HardwareId == null)
                            {
                                stepPassed &= false;
                                results += STEP_MSG_SPACING + "HardwareId information not received" + Environment.NewLine;
                            }
                            else
                                results += STEP_MSG_SPACING + "HardwareId - " + GDIR.HardwareId + Environment.NewLine;

                            if(stepPassed)
                                results += STEP_MSG_SPACING + "Device Information Valid" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// 8.2.11 Perform Device, Device system date and time request test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SYSTEM_DATE_AND_TIME(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetSystemDateAndTimeRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetSystemDateAndTimeRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetSystemDateAndTimeResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetSystemDateAndTimeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetSystemDateAndTimeResponse GSDTR = (DeviceManagement.GetSystemDateAndTimeResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetSystemDateAndTimeResponse));

                            if (GSDTR.SystemDateAndTime == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "System Date and Time not received" + Environment.NewLine;
                                stepPassed &= false;
                            }
                            else
                            {
                                if (GSDTR.SystemDateAndTime.DateTimeType == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GSDTR.SystemDateAndTime.DaylightSavings == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DaylightSavings not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if ((GSDTR.SystemDateAndTime.DateTimeType == DeviceManagement.SetDateTimeType.Manual) && ((GSDTR.SystemDateAndTime.LocalDateTime == null) && (GSDTR.SystemDateAndTime.UTCDateTime == null)) )
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType is Manual but neither UTCDateTime or LocalDateTime set" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                            }

                            if (stepPassed)
                                results += STEP_MSG_SPACING + "System Date And Time Valid" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// 8.2.11.1 Perform Device, Device system date and time setup test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SYSTEM_DATE_AND_TIME_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetSystemDateAndTimeRequest message" + Environment.NewLine;
                    // build the message
                    DeviceManagement.SetSystemDateAndTime SetSystemDateAndTimeRequest = new DeviceManagement.SetSystemDateAndTime();
                    

                    DeviceManagement.TimeZone TimeZone = new DeviceManagement.TimeZone();

                    TimeZone.TZ = TIMEZONE_STRING; 
                    
                    DeviceManagement.DateTime Datetime = new DeviceManagement.DateTime();
                    Datetime.Date = new DeviceManagement.Date();
                    Datetime.Time = new DeviceManagement.Time();

                    Datetime.Date.Day = System.DateTime.Now.Day;
                    Datetime.Date.Month = System.DateTime.Now.Month;
                    Datetime.Date.Year = System.DateTime.Now.Year;

                    Datetime.Time.Hour = System.DateTime.Now.Hour;
                    Datetime.Time.Minute = System.DateTime.Now.Minute;
                    Datetime.Time.Second = System.DateTime.Now.Second;

                    SetSystemDateAndTimeRequest.DateTimeType = DeviceManagement.SetDateTimeType.Manual;
                    SetSystemDateAndTimeRequest.DaylightSavings = true;
                    SetSystemDateAndTimeRequest.UTCDateTime = Datetime;
                    SetSystemDateAndTimeRequest.TimeZone = TimeZone;

                    Parameters.Temporary_Object = SetSystemDateAndTimeRequest;

                    test.MessagesSent = TestMessage.Build_SetSystemDateAndTimeRequest(SetSystemDateAndTimeRequest);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetSystemDateAndTimeResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetSystemDateAndTimeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.SetSystemDateAndTimeResponse SSDTR = (DeviceManagement.SetSystemDateAndTimeResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SetSystemDateAndTimeResponse));

                            // emtpy message

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetSystemDateAndTimeRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetSystemDateAndTimeRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetSystemDateAndTimeResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetSystemDateAndTimeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetSystemDateAndTimeResponse GSDTR = (DeviceManagement.GetSystemDateAndTimeResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetSystemDateAndTimeResponse));
                            
                            SetSystemDateAndTimeRequest = (DeviceManagement.SetSystemDateAndTime)Parameters.Temporary_Object;

                            if (GSDTR.SystemDateAndTime == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "System Date and Time not received" + Environment.NewLine;
                                stepPassed &= false;
                            }
                            else
                            {
                                
                                if ( (GSDTR.SystemDateAndTime.DateTimeType == null) || 
                                     (GSDTR.SystemDateAndTime.DateTimeType != SetSystemDateAndTimeRequest.DateTimeType) )
                                {
                                    if (GSDTR.SystemDateAndTime.DateTimeType == null)
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType not set" + Environment.NewLine;
                                    else
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType " + GSDTR.SystemDateAndTime.DateTimeType.ToString() + " found, expected " + SetSystemDateAndTimeRequest.DateTimeType.ToString() + Environment.NewLine;
                                    
                                    stepPassed &= false;
                                }

                                if ( (GSDTR.SystemDateAndTime.DaylightSavings == null) ||
                                     (GSDTR.SystemDateAndTime.DaylightSavings != SetSystemDateAndTimeRequest.DaylightSavings) )
                                {
                                    if (GSDTR.SystemDateAndTime.DaylightSavings == null)
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DaylightSavings not set" + Environment.NewLine;
                                    else 
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DaylightSavings " + GSDTR.SystemDateAndTime.DaylightSavings.ToString() + " found, expected " + SetSystemDateAndTimeRequest.DaylightSavings.ToString() + Environment.NewLine;

                                    stepPassed &= false;
                                }

                                if ( (GSDTR.SystemDateAndTime.TimeZone == null) ||
                                     (GSDTR.SystemDateAndTime.TimeZone.TZ != SetSystemDateAndTimeRequest.TimeZone.TZ) )
                                {
                                    if(GSDTR.SystemDateAndTime.TimeZone == null)
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "TimeZone not set" + Environment.NewLine;
                                    else
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "TimeZone " + GSDTR.SystemDateAndTime.TimeZone.TZ + " found, expected " + SetSystemDateAndTimeRequest.TimeZone.TZ + Environment.NewLine;

                                    stepPassed &= false;
                                }

                                if ((GSDTR.SystemDateAndTime.LocalDateTime == null) && (GSDTR.SystemDateAndTime.UTCDateTime == null))
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType is Manual but neither UTCDateTime or LocalDateTime set" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                                                          

                            }

                            if (stepPassed)
                                results += STEP_MSG_SPACING + "System Date was set corectly" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// 8.2.11.2 Perform Device, Device system date and time set with invalid timezone test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SYSTEM_DATE_AND_TIME_INVALID_TIMEZONE_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetSystemDateAndTimeRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "DateTimeType=\"Manual\", DayLightSavings=true, Timezone=INVALID," + Environment.NewLine;
                    results += STEP_SPACING + "UTCDateTime=Hour:Min:Sec, Year:Month:Day" + Environment.NewLine;
                    // build the message
                    DeviceManagement.SetSystemDateAndTime SetSystemDateAndTimeRequest = new DeviceManagement.SetSystemDateAndTime();


                    DeviceManagement.TimeZone TimeZone = new DeviceManagement.TimeZone();

                    TimeZone.TZ = "BAD_TIMEZONE";
                    // PST = designation for standard time when daylight saving is not in force
                    // 8 = offset in hours = 5 hours west of Greenwich meridian (i.e. behind UTC)
                    // PDT = designation when daylight saving is in force (if omitted there is no daylight saving)
                    // , = no offset number between code and comma, so default to one hour ahead for daylight saving
                    // M3.2.0 = when daylight saving starts = the 0th day (Sunday) in the second week of month 3 (March)
                    // /2, = the local time when the switch occurs = 2 a.m. in this case
                    // M11.1.0 = when daylight saving ends = the 0th day (Sunday) in the first week of month 11 (November). No time is given here so the switch occurs at 02:00 local 

                    DeviceManagement.DateTime Datetime = new DeviceManagement.DateTime();
                    Datetime.Date = new DeviceManagement.Date();
                    Datetime.Time = new DeviceManagement.Time();

                    Datetime.Date.Day = System.DateTime.Now.Day;
                    Datetime.Date.Month = System.DateTime.Now.Month;
                    Datetime.Date.Year = System.DateTime.Now.Year;

                    Datetime.Time.Hour = System.DateTime.Now.Hour;
                    Datetime.Time.Minute = System.DateTime.Now.Minute;
                    Datetime.Time.Second = System.DateTime.Now.Second;

                    SetSystemDateAndTimeRequest.DateTimeType = DeviceManagement.SetDateTimeType.Manual;
                    SetSystemDateAndTimeRequest.DaylightSavings = true;
                    SetSystemDateAndTimeRequest.UTCDateTime = Datetime;
                    SetSystemDateAndTimeRequest.TimeZone = TimeZone;

                    Parameters.Temporary_Object = SetSystemDateAndTimeRequest;

                    test.MessagesSent = TestMessage.Build_SetSystemDateAndTimeRequest(SetSystemDateAndTimeRequest);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidTimeZone"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed &= true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed &= false;
                            
                        }
                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT returned a SOAP error" + Environment.NewLine;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Step Failed" + Environment.NewLine;
                        stepPassed &= false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetSystemDateAndTimeRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetSystemDateAndTimeRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetSystemDateAndTimeResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetSystemDateAndTimeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetSystemDateAndTimeResponse GSDTR = (DeviceManagement.GetSystemDateAndTimeResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetSystemDateAndTimeResponse));

                            if (GSDTR.SystemDateAndTime == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "System Date and Time not received" + Environment.NewLine;
                                stepPassed &= false;
                            }
                            else
                            {
                                if (GSDTR.SystemDateAndTime.DateTimeType == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GSDTR.SystemDateAndTime.DaylightSavings == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DaylightSavings not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GSDTR.SystemDateAndTime.TimeZone == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "TimeZone not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if ((GSDTR.SystemDateAndTime.DateTimeType == DeviceManagement.SetDateTimeType.Manual) && ((GSDTR.SystemDateAndTime.LocalDateTime == null) && (GSDTR.SystemDateAndTime.UTCDateTime == null)))
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType is Manual but neither UTCDateTime or LocalDateTime set" + Environment.NewLine;
                                    stepPassed &= false;
                                }
                            }

                            if (stepPassed)
                                results += STEP_MSG_SPACING + "System Date And Time Valid" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// 8.2.11.3 Perform Device, Device system date and time setup with invalid date test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_SYSTEM_DATE_AND_TIME_INVALID_DATE_TEST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetSystemDateAndTimeRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "DateTimeType=\"Manual\", DayLightSavings=true, Timezone=POSIX 1003.1," + Environment.NewLine;
                    results += STEP_SPACING + "UTCDateTime=INVALID" + Environment.NewLine;
                    // build the message
                    DeviceManagement.SetSystemDateAndTime SetSystemDateAndTimeRequest = new DeviceManagement.SetSystemDateAndTime();


                    DeviceManagement.TimeZone TimeZone = new DeviceManagement.TimeZone();

                    TimeZone.TZ = TIMEZONE_STRING;
                   
                    DeviceManagement.DateTime Datetime = new DeviceManagement.DateTime();
                    Datetime.Date = new DeviceManagement.Date();
                    Datetime.Time = new DeviceManagement.Time();

                    Datetime.Date.Day = 32;
                    Datetime.Date.Month = 13;
                    Datetime.Date.Year = System.DateTime.Now.Year;

                    Datetime.Time.Hour = 26;
                    Datetime.Time.Minute = 71;
                    Datetime.Time.Second = 130;

                    SetSystemDateAndTimeRequest.DateTimeType = DeviceManagement.SetDateTimeType.Manual;
                    SetSystemDateAndTimeRequest.DaylightSavings = true;
                    SetSystemDateAndTimeRequest.UTCDateTime = Datetime;
                    SetSystemDateAndTimeRequest.TimeZone = TimeZone;

                    Parameters.Temporary_Object = SetSystemDateAndTimeRequest;

                    test.MessagesSent = TestMessage.Build_SetSystemDateAndTimeRequest(SetSystemDateAndTimeRequest);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidDateTime"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed &= true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed &= false;                            
                        }
                    }
                    else
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT returned a SOAP error" + Environment.NewLine;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Step Failed" + Environment.NewLine;
                        stepPassed &= false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetSystemDateAndTimeRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_GetSystemDateAndTimeRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetSystemDateAndTimeResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetSystemDateAndTimeResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.GetSystemDateAndTimeResponse GSDTR = (DeviceManagement.GetSystemDateAndTimeResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.GetSystemDateAndTimeResponse));

                            if (GSDTR.SystemDateAndTime == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "System Date and Time not received" + Environment.NewLine;
                                stepPassed &= false;
                            }
                            else
                            {
                                if (GSDTR.SystemDateAndTime.DateTimeType == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GSDTR.SystemDateAndTime.DaylightSavings == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DaylightSavings not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if (GSDTR.SystemDateAndTime.TimeZone == null)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "TimeZone not received" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                                if ((GSDTR.SystemDateAndTime.DateTimeType == DeviceManagement.SetDateTimeType.Manual) && ((GSDTR.SystemDateAndTime.LocalDateTime == null) && (GSDTR.SystemDateAndTime.UTCDateTime == null)))
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "DateTimeType is Manual but neither UTCDateTime or LocalDateTime set" + Environment.NewLine;
                                    stepPassed &= false;
                                }

                            }

                            if (stepPassed)
                                results += STEP_MSG_SPACING + "System Date And Time Valid" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Restore device to factory default test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_FACTORY_DEFAULT(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string deviceInfo = "";
            bool helloFound;
            DialogResult aDialog;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            // probe the device
            if (test.CurrentStep == 0)
                ProbeDevice(ref Parameters, ref NetworkInterface);

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetSystemFactoryDefaultRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "FactoryDefaultType = \"Hard\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_SetSystemFactoryDefaultRequest(DeviceManagement.FactoryDefaultType.Hard);

                    // warn the user about what is going to happen
                    // open a dialog box and tell the users to start the NVT
                    //ExceptionMessageBox mbox = new ExceptionMessageBox("This test will reset the camera back to defaults, including the IP address, do you wish to continue?", "WARNING!!", ExceptionMessageBoxButtons.Custom);
                    //mbox.SetButtonText("Yes", "Skip");

                    //mbox.Show(null);


                    //aDialog = System.Windows.Forms.MessageBox.Show("This test will reset the camera back to defaults, including the IP address, do you wish to continue?", "WARNING!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.);
                    //aDialog = DialogResult.Cancel;
                    //if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button2)  // stop the test
                    //{                        
                    //    test.StepComplete(true);
                    //    test.TestComplete = true;
                    //    test.TestSkipped = true;
                    //    test.Action = TestCases_Class.TestActions.Skip;
                    //    break;
                    //}

                    // send the message
                    try
                    {
                        // becuase this is a factory reset, the IP address will also be reset
                        // so setup the multicast listener to listen to any multicast.
                        // setup the multicast listern for the hello message after the reset.
                        //NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, IPAddress.Any.ToString(), Parameters.Port, (short)Parameters.TTL);
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // send the message
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetSystemFactoryDefaultResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetSystemFactoryDefaultResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.SetSystemFactoryDefaultResponse SSFDR = (DeviceManagement.SetSystemFactoryDefaultResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SetSystemFactoryDefaultResponse));


                            // nothing to validate, just an empty message
                            stepPassed = true;
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Receive multicast HELLO message" + Environment.NewLine;
                    //  get the hello message
                    helloFound = false;

                    try
                    {

                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.RebootTime + Parameters.TestTimeout, IPAddress.Any.ToString());
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                                               
                    }                    
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    
                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_HelloResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));

                            // according to Annex A of the ONVIF test spec 1.0
                            RemoteDiscovery.HelloType Hello = new RemoteDiscovery.HelloType();
                            Hello.Types = DEFAULT_DEVICE_TYPE;
                            Hello.Scopes = new RemoteDiscovery.ScopesType();
                            Hello.Scopes.Text = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Location, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Type};


                            if (TestMessage.Compare_RemoteDiscovery_ScopesType(Hello.Scopes, HT.Scopes, ref errorMessages))
                                stepPassed = true;
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Multicast Hello Message did not contain neccissary objects.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }

                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;


                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();

                if (test.Action == TestCases_Class.TestActions.Skip)
                    results += "Test SKIPPED" + Environment.NewLine;
                else
                {
                    if (test.TestPassed)
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;
                    else
                        results += STEP_SPACING + "Test complete" + Environment.NewLine;

                    // create a bogus exception and store the results string in the message field so it isn't lost
                    //Exception resultsExcpt = new Exception(results);
                    

                    // throw the network reset error so the test will stop and the user can reconfigure the device
                    //throw new TestCase_StopTest("Target Device has done a hard reset, the IP address may need to be reset to continue", resultsExcpt);
                }
                
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Restore device to factory default (soft) test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_FACTORY_DEFAULT_SOFT(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;
            int x;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");


            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SetSystemFactoryDefaultRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "FactoryDefaultType = \"Soft\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_SetSystemFactoryDefaultRequest(DeviceManagement.FactoryDefaultType.Soft);

                    // send the message
                    try
                    {                        
                        // send the message
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SetSystemFactoryDefaultResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetSystemFactoryDefaultResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            DeviceManagement.SetSystemFactoryDefaultResponse SSFDR = (DeviceManagement.SetSystemFactoryDefaultResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SetSystemFactoryDefaultResponse));


                            // nothing to validate, just an empty message
                            stepPassed = true;
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;
                
                case 2:
                    results += STEP_SPACING + "Step 3 - Pause for \"User defined boot time\"" + Environment.NewLine;
                    System.Threading.Thread.Sleep(Parameters.RebootTime);
                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Transmit unicast PROBE message" + Environment.NewLine;
                    results += STEP_SPACING + "Retransmit 10 times a second until a response is received or timeout" + Environment.NewLine;

                    // setup the network interface
                    NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                    System.DateTime endTime = System.DateTime.Now.AddMilliseconds(Parameters.RebootTime);
                    
                    x = 0;

                    while(System.DateTime.Now < endTime)
                    {                      

                        try
                        {
                            RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                            Scope.Text = new string[] { "" };
                            test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                            // send the message
                            stepPassed = false;
                            NetworkInterface.UDP_Send(test.MessagesSent);
                            stepPassed = true;
                            //NetworkInterface.Send(test.MessagesSent);

                            results += STEP_MSG_SPACING + "Sending Probe Request " + (x++ + 1).ToString() + " ";
                                  
                        }
                        catch (Exception e)
                        {
                            if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                            {
                                //results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                                //stepPassed = false;
                            }
                            else
                            {
                                results += "Send socket failed - " + e.Message + Environment.NewLine;
                                stepPassed = false;
                                
                            }
                            
                        }

                        if (stepPassed)
                        {
                            // see if we get a probe response back
                            try
                            {
                                test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);                          

                                results += "- Received response" + Environment.NewLine;
                                stepPassed = true;
                                

                            }
                            catch (Exception e)
                            {
                                if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                                {
                                    results += "- No response" + Environment.NewLine;

                                    // reconnect
                                    NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                                }
                                else
                                {
                                    results += "- Listen socket failed - " + e.Message + Environment.NewLine;
                                    //test.StepComplete(false);
                                    break;
                                }
                            }

                            try
                            {
                                // double check to make sure this is a probe response before contiuing
                                TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages);
                                break;
                            }
                            catch (Exception e)
                            {
                                results += STEP_MSG_SPACING + "Message received not Probe Matches Response - " + e.Message + Environment.NewLine;
                            }
                        }
                        else
                            System.Threading.Thread.Sleep(100);
                    }

                    if (!stepPassed)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                        test.TestComplete = true;        
                    }

                    test.StepComplete(stepPassed);                                
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Receive PROBE MATCH message" + Environment.NewLine;
                    
                    // THE MESSAGE HAS ALREADY BEEN RECIEVED, JUST VERIFY IT
                
                    // lisen for the response
                    //try
                    //{
                    //    test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                    //    //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    //}
                    //catch (Exception e)
                    //{
                    //    if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                    //    {
                    //        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                    //        test.TestComplete = true;
                    //        test.StepComplete(false);
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                    //        test.StepComplete(false);
                    //        break;
                    //    }
                    //}

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // the ONVIF test spec only required that the mandatory XML elements be present to pass this test
                        
                            
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Device, Device reset test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string DEVICE_RESET(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool helloFound;
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            // probe the device
            if (test.CurrentStep == 0)
                ProbeDevice(ref Parameters, ref NetworkInterface);

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit SystemReboot message" + Environment.NewLine;
                    try
                    {
                        // setup the network interface for the hello message
                        NetworkInterface.UDP_ConnectMulticast(Parameters.Multicast_IP, Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                        //NetworkInterface.Connect(Parameters.Multicast_IP, Parameters.Port, (short)Parameters.TTL);

                        // build the message
                        test.MessagesSent = TestMessage.Build_SystemRebootRequest();

                        // send the message

                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SystemRebootResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SystemRebootResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Response Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Response Message validated" + Environment.NewLine;
                            stepPassed = true;

                            // Print out the reboot message
                            DeviceManagement.SystemRebootResponse SRR = (DeviceManagement.SystemRebootResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(DeviceManagement.SystemRebootResponse));
                            results += STEP_MSG_SPACING + "Response Message received - " + SRR.Message + Environment.NewLine;
                            // no other validatation required
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Receive multicast HELLO message" + Environment.NewLine;
                    //  get the hello message


                    try
                    {

                        test.MessagesReceived = GetHelloResponse(Parameters.RebootTime + Parameters.TestTimeout, ref Parameters, ref NetworkInterface);
                        //test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.RebootTime + Parameters.TestTimeout, Parameters.Target_IP);
                                              
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
               

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_HelloResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            // According to the ONVIF test spec 1.0 the GetHostnameResponse just needs to be returned
                            RemoteDiscovery.HelloType HT = (RemoteDiscovery.HelloType)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(RemoteDiscovery.HelloType));

                            // according to Annex A of the ONVIF test spec 1.0
                            RemoteDiscovery.HelloType Hello = new RemoteDiscovery.HelloType();
                            Hello.Types = DEFAULT_DEVICE_TYPE;
                            Hello.Scopes = new RemoteDiscovery.ScopesType();
                            Hello.Scopes.Text = new string[] {RemoteDiscovery.Constants.ScopeTypePrefix_Hardware, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Location, 
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Name,
                                                      RemoteDiscovery.Constants.ScopeTypePrefix_Type};


                            if (TestMessage.Compare_RemoteDiscovery_ScopesType(Hello.Scopes, HT.Scopes, ref errorMessages))
                                stepPassed = true;
                            else
                            {
                                results += STEP_MSG_SPACING + "Multicast Hello Message did not contain neccissary objects.  " + errorMessages + Environment.NewLine;
                                errorMessages = "";
                                stepPassed = false;
                            }

                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }                    
                    test.StepComplete(stepPassed);
                    //NetworkInterface.Disconnect();
                    NetworkInterface.UDP_Close();
                    break;


                case 3:
                    results += STEP_SPACING + "Step 3 - Transmit unicast PROBE message" + Environment.NewLine;
                    // setup the network interface
                    NetworkInterface.UDP_Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);
                    //NetworkInterface.Connect(Parameters.Target_IP, Parameters.Port, (short)Parameters.TTL);

                    try
                    {
                        RemoteDiscovery.ScopesType Scope = new RemoteDiscovery.ScopesType();
                        Scope.Text = new string[] { "" };
                        test.MessagesSent = TestMessage.Build_ProbeRequest(DEFAULT_DEVICE_TYPE, Scope);

                        // send the message
                        NetworkInterface.UDP_Send(test.MessagesSent);
                        //NetworkInterface.Send(test.MessagesSent);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }
                    test.StepComplete(true);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 4 - Receive PROBE MATCH message" + Environment.NewLine;
                    // listen for the probe response and validate
                    try
                    {
                        // according to the test spec the probe match must be recieved in 500ms or less
                        test.MessagesReceived = NetworkInterface.UDP_Listen(Parameters.TestTimeout, Parameters.Target_IP);
                        //test.MessagesReceived = NetworkInterface.Receive(Parameters.TestTimeout, Parameters.Target_IP);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unit failed to respond within timeout period, test case timeout" + Environment.NewLine;
                            test.TestComplete = true;
                            test.StepComplete(false);
                            break;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Listen socket failed - " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_ProbeMatchesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Scopes Response Message failed validation" + Environment.NewLine;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                            stepPassed = false;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Get Scopes Response Message validated" + Environment.NewLine;

                            // perform any test specific validation (beyond the XML validation)
                            // the ONVIF test spec only required that the mandatory XML elements be present to pass this test


                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    // each test will subbimit its passed/failed status, if any fail the whole set fail
                    test.StepComplete(stepPassed);
                    break;


                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                if (!test.TestPassed)
                {
                    ExceptionMessageBox mbox = new ExceptionMessageBox("Is the device resetting?  If so please wait until the device is done booting before clicking \"Continue\".", "Stop", ExceptionMessageBoxButtons.Custom);
                    mbox.SetButtonText("Continue");
                    mbox.Show(null);
                }
            }

            return results;
        }
        #endregion

        #region Media Tests

        /// <summary>
        /// Perform Media, Profile configuration test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_PROFILE_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetProfilesResponse message" + Environment.NewLine;                    
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));

                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.Name != null)
                                        results += STEP_SPACING + "Profile found = " + Profile.Name + Environment.NewLine;

                                    if (Profile.token != null)
                                        results += STEP_SPACING + "Token = " + Profile.token + Environment.NewLine;
                                }

                            }

                            // is the WSDL correctly formed?

                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, Dynamic Profile configuration test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_DYNAMIC_MEDIA_PROFILE_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            bool videoSourceFound = false;
            bool videoEncoderFound = false;
            Media.Profile tmpProfile;
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();
                    //TestMessage.Build_GetVideoEncoderConfigurationRequest

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));

                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.Name != null)
                                        results += STEP_SPACING + "Profile found = " + Profile.Name + Environment.NewLine;

                                    if ((Profile.VideoSourceConfiguration != null) &&
                                       (Profile.VideoSourceConfiguration.token != null))
                                    {
                                        results += STEP_SPACING + "Video Source Configuration Token found = " + Profile.VideoSourceConfiguration.token + Environment.NewLine;
                                        videoSourceFound = true;
                                    }
                                    else
                                        videoSourceFound = false;

                                    if ((Profile.VideoEncoderConfiguration != null) &&
                                        (Profile.VideoEncoderConfiguration.token != null))
                                    {
                                        results += STEP_SPACING + "Video Encoder Configuration Token found = " + Profile.VideoSourceConfiguration.token + Environment.NewLine;
                                        videoEncoderFound = true;
                                    }
                                    else
                                        videoEncoderFound = false;

                                    if (videoSourceFound && videoEncoderFound)
                                    {
                                        Parameters.Temporary_Object = Profile;
                                        break;
                                    }
                                }

                                if (!videoSourceFound && !videoEncoderFound)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Profile found with a Video Source and Video Encoder" + Environment.NewLine;
                                    stepPassed = false;
                                } else
                                    stepPassed = true;


                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Media Profiles found" + Environment.NewLine;                        
                                stepPassed = false;
                            }
                         
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit CreateProfilesRequest message" + Environment.NewLine;

                    System.Random rand = new Random((int)System.DateTime.Now.Ticks);
                    Parameters.Temporary_String = "testprofile" + (rand.Next() % 100).ToString();
                    
                    results += STEP_SPACING + "Name = \"" + Parameters.Temporary_String + "\"" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_CreateProfileRequest(Parameters.Temporary_String, Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive CreateProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_CreateProfileResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.CreateProfileResponse CPR = (Media.CreateProfileResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.CreateProfileResponse));

                            if (CPR.Profile != null)
                            {
                                results += STEP_SPACING + "Create profile response" + Environment.NewLine;

                                if(CPR.Profile.Name != null)
                                    results += STEP_SPACING + "Profile " + CPR.Profile.Name + Environment.NewLine;
                                if(CPR.Profile.token != null)
                                    results += STEP_SPACING + "Profile token " + CPR.Profile.token + Environment.NewLine;

                                if (CPR.Profile.token == Parameters.Temporary_String)
                                {
                                    stepPassed = true;
                                    results += STEP_SPACING + "Profile token correct" + Environment.NewLine;
                                }
                                else
                                {
                                    stepPassed = false;
                                    results += STEP_SPACING + "Profile token not as expected" + Environment.NewLine;
                                }
                            }
                            else
                            {
                                stepPassed = false;
                                results += STEP_SPACING + "Create profile response is null, test fail" + Environment.NewLine;
                            }
                            
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit AddVideoSourceConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    tmpProfile = (Media.Profile )Parameters.Temporary_Object;

                    test.MessagesSent = TestMessage.Build_Media_AddVideoSourceConfigurationRequest(tmpProfile.VideoSourceConfiguration.token, Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive AddVideoSourceConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_AddVideoSourceConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.AddVideoSourceConfigurationResponse AVSCR = (Media.AddVideoSourceConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.AddVideoSourceConfigurationResponse));

                            // empty message nothing to do
                            stepPassed = true;
                                                       
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step 7 - Transmit AddVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    tmpProfile = (Media.Profile)Parameters.Temporary_Object;
                    test.MessagesSent = TestMessage.Build_Media_AddVideoEncoderConfigurationRequest(tmpProfile.VideoEncoderConfiguration.token, Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 7:
                    results += STEP_SPACING + "Step 8 - Receive AddVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_AddVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.AddVideoEncoderConfigurationResponse AVECR = (Media.AddVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.AddVideoEncoderConfigurationResponse));
                                                     

                            // empty message nothing to do
                            stepPassed = true;

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 8:
                    results += STEP_SPACING + "Step 9 - Transmit GetProfileRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfileRequest(Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 9:
                    results += STEP_SPACING + "Step 10 - Receive GetProfileResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfileResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfileResponse GPR = (Media.GetProfileResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfileResponse));

                            if (GPR.Profile == null)
                                stepPassed = false;
                            else
                            {
                                if (GPR.Profile.token == Parameters.Temporary_String)
                                {
                                    stepPassed = true;
                                    results += STEP_SPACING + "Temporary profile found" + Environment.NewLine;
                                }
                                else
                                {
                                    stepPassed = false;
                                    results += STEP_SPACING + "Profile token incorrect" + Environment.NewLine;
                                }

                            }

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 10:
                    results += STEP_SPACING + "Step 11 - Transmit RemoveVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_RemoveVideoEncoderConfigurationRequest(Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 11:
                    results += STEP_SPACING + "Step 12 - Receive RemoveVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_RemoveVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.RemoveVideoEncoderConfigurationResponse RVECR = (Media.RemoveVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.RemoveVideoEncoderConfigurationResponse));
                            
                            // empty message nothing to do
                            stepPassed = true;

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 12:
                    results += STEP_SPACING + "Step 13 - Transmit RemoveVideoSourceConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_RemoveVideoSourceConfigurationRequest (Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 13:
                    results += STEP_SPACING + "Step 14 - Receive RemoveVideoSourceConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_RemoveVideoSourceConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.RemoveVideoSourceConfigurationResponse RVSCR = (Media.RemoveVideoSourceConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.RemoveVideoSourceConfigurationResponse));
                            
                            // empty message nothing to do
                            stepPassed = true;

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 14:
                    results += STEP_SPACING + "Step 15 - Transmit DeleteProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_DeleteProfileRequest(Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 15:
                    results += STEP_SPACING + "Step 16 - Receive DeleteProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_DeleteProfileResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.DeleteProfileResponse DPR = (Media.DeleteProfileResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.DeleteProfileResponse));
                            
                            // empty message nothing to do
                            stepPassed = true;

                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 16:
                    results += STEP_SPACING + "Step 17 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfileRequest(Parameters.Temporary_String);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 17:
                    results += STEP_SPACING + "Step 18 - Receive SOAP 1.2 fault message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:NoProfile"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed &= true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed &= false;
                        }
                            

                        test.StepComplete(stepPassed);
                        break;
                    }
                    
                    // this only happenes if soap error not found
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT return SOAP error as required " + Environment.NewLine;
                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "test failed" + Environment.NewLine;
                    test.StepComplete(false);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, JPEG video encoder configuration test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_JPEG_VIDEO_ENCODER_CONFIGURATION(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));
                            
                            // assume no profiles found (error)
                            Parameters.Profile_Token = "";
                            stepPassed = false;
                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.VideoEncoderConfiguration != null)
                                    {

                                        if (Profile.VideoEncoderConfiguration.Name != null)
                                        {
                                            results += STEP_SPACING + "Profile found = " + Profile.VideoEncoderConfiguration.Name + Environment.NewLine;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Temporary_String = Profile.VideoEncoderConfiguration.Name;
                                        }

                                        if (Profile.VideoEncoderConfiguration.token != null)
                                        {
                                            results += STEP_SPACING + "Token = " + Profile.VideoEncoderConfiguration.token + Environment.NewLine;
                                            stepPassed = true;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Profile_Token = Profile.VideoEncoderConfiguration.token;
                                        }
                                    }
                                }
                            } else {
                                stepPassed = false;
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profiles found" + Environment.NewLine;
                                Parameters.Temporary_String = "";
                            }

                            if(!stepPassed)
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profile Token found" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit GetVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetVideoEncoderConfigurationRequest(Parameters.Profile_Token);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive GetVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetVideoEncoderConfigurationResponse GVECR = (Media.GetVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetVideoEncoderConfigurationResponse));
                            
                            if (GVECR.Configuration != null)
                            {
                                Parameters.Temporary_Object = GVECR.Configuration;

                                if(GVECR.Configuration.Name != null)
                                    results += STEP_MSG_SPACING + "Video Encoder Configuration - " + GVECR.Configuration.Name + Environment.NewLine;

                                if (GVECR.Configuration.token != null)
                                    results += STEP_MSG_SPACING + "Configuration token - " + GVECR.Configuration.token + Environment.NewLine;

                                if (GVECR.Configuration.Encoding != null)
                                    results += STEP_MSG_SPACING + "Configuration encoding - " + GVECR.Configuration.Encoding.ToString() + Environment.NewLine;
                                
                                stepPassed = true;
                            }
                            else
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No configuration found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit SetVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "JPEG Video Encoder Configuration, force persistence = false" + Environment.NewLine;
                    // build the message
                    // lots of required pieces in this object
                    Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                    
                    SVEC.ForcePersistence = false;
                    SVEC.Configuration = (Media.VideoEncoderConfiguration)Parameters.Temporary_Object;

                    SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                    Parameters.Temporary_String = SVEC.Configuration.token;

                    test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive SetVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.SetVideoEncoderConfigurationResponse SVECR = (Media.SetVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.SetVideoEncoderConfigurationResponse));

                            // empty message, nothing to do here
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step 7 - Transmit GetVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetVideoEncoderConfigurationRequest(Parameters.Temporary_String);
                  
                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 7:
                    results += STEP_SPACING + "Step 8 - Receive GetVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetVideoEncoderConfigurationResponse GVECR = (Media.GetVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetVideoEncoderConfigurationResponse));

                            if (GVECR.Configuration == null)
                            {
                                results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Encoder configuration not found" + Environment.NewLine;
                                stepPassed = false;
                            }
                            else
                            {
                                Media.VideoEncoderConfiguration VEC = (Media.VideoEncoderConfiguration)Parameters.Temporary_Object;

                                // assume step passed
                                stepPassed = true;

                                // compare what we sent to what we got back
                                if ((GVECR.Configuration.Encoding == null) ||
                                    (GVECR.Configuration.Encoding != VEC.Encoding))
                                {
                                    stepPassed = false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video encoding not set correctly" + Environment.NewLine;
                                }


                                if ((GVECR.Configuration.Resolution == null) ||
                                    (GVECR.Configuration.Resolution.Height != VEC.Resolution.Height) ||
                                    (GVECR.Configuration.Resolution.Width != VEC.Resolution.Width))
                                {
                                    stepPassed = false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video resolution not set correctly" + Environment.NewLine;
                                }


                                if ((GVECR.Configuration.Quality == null) ||
                                    (GVECR.Configuration.Quality != VEC.Quality))
                                {
                                    stepPassed = false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video quality not set correctly" + Environment.NewLine;
                                }

                                if ((GVECR.Configuration.SessionTimeout == null) ||
                                    (GVECR.Configuration.SessionTimeout != VEC.SessionTimeout))
                                {
                                    stepPassed = false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video quality not set correctly" + Environment.NewLine;
                                }

                                // the test spec says to validate force persistance but the return string doesn't
                                // contain that value??
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, media stream URI configuration RTP UDP Unicast test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_STREAM_URI__RTP_UDP_UNICAST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));

                            // assume no profiles found (error)
                            stepPassed = false;
                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.VideoEncoderConfiguration != null)
                                    {
                                        Parameters.Temporary_Object = Profile;

                                        if (Profile.VideoEncoderConfiguration.Name != null)
                                        {
                                            results += STEP_SPACING + "Profile found = " + Profile.VideoEncoderConfiguration.Name + Environment.NewLine;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Temporary_String = Profile.VideoEncoderConfiguration.Name;
                                        }

                                        if (Profile.VideoEncoderConfiguration.token != null)
                                        {
                                            results += STEP_SPACING + "Token = " + Profile.VideoEncoderConfiguration.token + Environment.NewLine;
                                            stepPassed = true;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Profile_Token = Profile.VideoEncoderConfiguration.token;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                stepPassed = false;
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profiles found" + Environment.NewLine;
                                Parameters.Temporary_String = "";
                            }

                            if (!stepPassed)
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profile Token found" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit SetVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "JPEG Video Encoder Configuration, force persistence = false" + Environment.NewLine;
                    // build the message
                    // lots of required pieces in this object
                    Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                    SVEC.ForcePersistence = false;

                    SVEC.Configuration = ((Media.Profile)Parameters.Temporary_Object).VideoEncoderConfiguration;

                    SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                    Parameters.Temporary_String = ((Media.Profile)Parameters.Temporary_Object).Name;
                    Parameters.Profile_Token = ((Media.Profile)Parameters.Temporary_Object).token;

                    test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive SetVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.SetVideoEncoderConfigurationResponse SVECR = (Media.SetVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.SetVideoEncoderConfigurationResponse));

                            // empty message, nothing to do here
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit GetStreamUriRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "Profile token, RTP-Unicast, UDP" + Environment.NewLine;
                    // build the message
                    Media.GetStreamUri GSUri = new Media.GetStreamUri();
                    
                    GSUri.ProfileToken = Parameters.Profile_Token;

                    GSUri.StreamSetup = new Media.StreamSetup();
                    GSUri.StreamSetup.Transport = new Media.Transport();
                    GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.UDP;
                    GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;

                    test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive GetStreamUriResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetStreamUriResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetStreamUriResponse GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));

                            if (GSUriR.MediaUri == null)
                                stepPassed = false;
                            else
                            {
                                if (GSUriR.MediaUri.Uri == null)
                                    stepPassed = false;
                                else
                                    results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;

                                // make sure none of the required pieces are null
                                if (GSUriR.MediaUri.InvalidAfterConnect == null)
                                    stepPassed &= false;
                                
                                if(GSUriR.MediaUri.InvalidAfterReboot == null)
                                    stepPassed &= false;

                                if((GSUriR.MediaUri.Timeout == null ) || GSUriR.MediaUri.Timeout.Equals(""))
                                    stepPassed &= false;

                                if (!stepPassed)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response message did not contain all required objects" + Environment.NewLine;
                                }
                                else
                                {
                                    // make sure the timeout is a valid number 

                                    TimeSpan TS = System.Xml.XmlConvert.ToTimeSpan(GSUriR.MediaUri.Timeout);

                                    // verify the life time is correct
                                    if (((GSUriR.MediaUri.InvalidAfterConnect  && !GSUriR.MediaUri.InvalidAfterReboot) || // timeout can be zero or non zero so as long as it isn't null it passed
                                        (!GSUriR.MediaUri.InvalidAfterConnect  && GSUriR.MediaUri.InvalidAfterReboot && TS.Ticks == 0)) ||
                                        (!GSUriR.MediaUri.InvalidAfterConnect  && !GSUriR.MediaUri.InvalidAfterReboot && (TS.Ticks != 0)))
                                        stepPassed = true;
                                    else
                                    {
                                        results += STEP_MSG_SPACING + "Stream lifetime combination is invalid" + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Invalid After Connect = " + GSUriR.MediaUri.InvalidAfterConnect.ToString() + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Invalid After Reboot" + GSUriR.MediaUri.InvalidAfterReboot.ToString() + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Timeout = " + GSUriR.MediaUri.Timeout + Environment.NewLine;
                                        stepPassed = false;
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, media stream URI configuration RTP RTSP HTTP test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_STREAM_URI__RTP_RTSP_HTTP(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            TimeSpan TS = new TimeSpan();
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetProfilesRequest message" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive GetProfilesResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));

                            // assume no profiles found (error)
                            stepPassed = false;
                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.VideoEncoderConfiguration != null)
                                    {
                                        Parameters.Temporary_Object = Profile;

                                        if (Profile.VideoEncoderConfiguration.Name != null)
                                        {
                                            results += STEP_SPACING + "Profile found = " + Profile.VideoEncoderConfiguration.Name + Environment.NewLine;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Temporary_String = Profile.VideoEncoderConfiguration.Name;
                                        }

                                        if (Profile.VideoEncoderConfiguration.token != null)
                                        {
                                            results += STEP_SPACING + "Token = " + Profile.VideoEncoderConfiguration.token + Environment.NewLine;
                                            stepPassed = true;
                                            if (Parameters.Profile_Token == "")
                                                Parameters.Profile_Token = Profile.VideoEncoderConfiguration.token;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                stepPassed = false;
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profiles found" + Environment.NewLine;
                                Parameters.Temporary_String = "";
                            }

                            if (!stepPassed)
                                results += STEP_SPACING + ERROR_MSG_PREFIX + "No Profile Token found" + Environment.NewLine;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 3 - Transmit SetVideoEncoderConfigurationRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "JPEG Video Encoder Configuration, force persistence = false" + Environment.NewLine;
                    // build the message
                    // lots of required pieces in this object
                    Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                    SVEC.ForcePersistence = false;

                    SVEC.Configuration = ((Media.Profile)Parameters.Temporary_Object).VideoEncoderConfiguration;

                    SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                    Parameters.Temporary_String = ((Media.Profile)Parameters.Temporary_Object).Name;
                    Parameters.Profile_Token = ((Media.Profile)Parameters.Temporary_Object).token;

                    test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 3:
                    results += STEP_SPACING + "Step 4 - Receive SetVideoEncoderConfigurationResponse message" + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_SetVideoEncoderConfigurationResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.SetVideoEncoderConfigurationResponse SVECR = (Media.SetVideoEncoderConfigurationResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.SetVideoEncoderConfigurationResponse));

                            // empty message, nothing to do here
                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step 5 - Transmit GetStreamUriRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "Profile token, RTP-Unicast, HTTP" + Environment.NewLine;
                    // build the message
                    Media.GetStreamUri GSUri = new Media.GetStreamUri();

                    GSUri.ProfileToken = Parameters.Profile_Token;

                    GSUri.StreamSetup = new Media.StreamSetup();
                    GSUri.StreamSetup.Transport = new Media.Transport();
                    GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.HTTP;
                    GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;

                    test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 5:
                    results += STEP_SPACING + "Step 6 - Receive GetStreamUriResponse message " + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetStreamUriResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetStreamUriResponse GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));

                            if (GSUriR.MediaUri == null)
                                stepPassed = false;
                            else
                            {
                                if (GSUriR.MediaUri.Uri == null)
                                {
                                    stepPassed = false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Media URI not found" + Environment.NewLine;
                                }
                                else
                                {
                                    results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;
                                    if (!(GSUriR.MediaUri.Uri.StartsWith("http://")))
                                    {
                                        stepPassed &= false;
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Stream URI does not start with \"http://\"" + Environment.NewLine;
                                    }                                    
                                }

                                // make sure none of the required pieces are null
                                if (GSUriR.MediaUri.InvalidAfterConnect == null)
                                {
                                    stepPassed &= false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Invalid After Connect is NULL" + Environment.NewLine;
                                }

                                if (GSUriR.MediaUri.InvalidAfterReboot == null)
                                {
                                    stepPassed &= false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Invalid After Reboot is NULL" + Environment.NewLine;
                                }

                                if ((GSUriR.MediaUri.Timeout == null) || GSUriR.MediaUri.Timeout.Equals(""))
                                {
                                    stepPassed &= false;
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Timeout not set" + Environment.NewLine;
                                }
                                else
                                {

                                    // make sure the timeout is a valid number 
                                    try
                                    {
                                        TS = System.Xml.XmlConvert.ToTimeSpan(GSUriR.MediaUri.Timeout);
                                    }
                                    catch
                                    {
                                        stepPassed &= false;
                                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to convert " + GSUriR.MediaUri.Timeout + " to Time Span Type" + Environment.NewLine;
                                    }
                                }


                                if (!stepPassed)
                                {
                                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response message failed validation" + Environment.NewLine;
                                }
                                else
                                {
                                    

                                    // verify the life time is correct
                                    if (((GSUriR.MediaUri.InvalidAfterConnect  && !GSUriR.MediaUri.InvalidAfterReboot) || // timeout can be zero or non zero so as long as it isn't null it passed
                                        (!GSUriR.MediaUri.InvalidAfterConnect  && GSUriR.MediaUri.InvalidAfterReboot && TS.Ticks == 0)) ||
                                        (!GSUriR.MediaUri.InvalidAfterConnect  && !GSUriR.MediaUri.InvalidAfterReboot && (TS.Ticks != 0)))
                                        stepPassed = true;
                                    else
                                    {
                                        results += STEP_MSG_SPACING + "Stream lifetime combination is invalid" + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Valid Until Connect = " + GSUriR.MediaUri.InvalidAfterConnect .ToString() + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Valid Until Reboot" + GSUriR.MediaUri.InvalidAfterReboot.ToString() + Environment.NewLine;
                                        results += STEP_MSG_SPACING + "Timeout = " + GSUriR.MediaUri.Timeout + Environment.NewLine;
                                        stepPassed = false;
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, media stream URI invlaid configuration test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_SOAP_FAULT_MESSAGE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            bool stepPassed = true;
            string soapFault;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 1 - Transmit GetStreamUriRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "INVALID PROFILE, RTP-Unicast, UDP" + Environment.NewLine;
                    // build the message
                    Media.GetStreamUri GSUri = new Media.GetStreamUri();

                    GSUri.ProfileToken = "INVALID_PROFILE";

                    GSUri.StreamSetup = new Media.StreamSetup();
                    GSUri.StreamSetup.Transport = new Media.Transport();
                    GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.HTTP;
                    GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;

                    test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    test.StepComplete(true);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message " + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        // this should have been InvalidArgs/NoProfile, but was changed to InvalidArgVal in test version 1.01
                        //  if (!TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:NoProfile")) 
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed = true;
                        }                        
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed &= false;
                            
                        }

                        test.StepComplete(stepPassed);
                        break;
                    }

                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT retur a SOAP error as expected" + Environment.NewLine;
                    test.StepComplete(false);    

                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }

        /// <summary>
        /// Perform Media, media stream invalid transport test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string MEDIA_INVALID_TRANSPORT_SOAP_FAULT_MESSAGE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            string results = "";
            bool stepPassed = true;
            string soapFault;
            string errorMessages = "";

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step 0 - (Pre test) Send GetProfilesRequest message for valid Profile Token" + Environment.NewLine;
                    // build the message
                    test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }

                    // otherwise verify the response
                    try
                    {
                        if (!TestMessage.Verify_GetProfilesResponse(test.MessagesReceived, ref errorMessages))
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Message failed validation" + Environment.NewLine;
                            stepPassed = false;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + errorMessages;
                            errorMessages = "";
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + "Message validated" + Environment.NewLine;

                            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));

                            if (GPsR.Profiles != null)
                            {
                                results += STEP_SPACING + "Existing media profiles" + Environment.NewLine;

                                foreach (Media.Profile Profile in GPsR.Profiles)
                                {
                                    if (Profile.Name != null)
                                        results += STEP_SPACING + "Profile found = " + Profile.Name + Environment.NewLine;

                                    if (Profile.token != null)
                                    {
                                        results += STEP_SPACING + "Token = " + Profile.token + Environment.NewLine;
                                        Parameters.Profile_Token = Profile.token;
                                    }
                                }

                            }

                            // is the WSDL correctly formed?

                            stepPassed = true;
                        }
                    }
                    catch(Exception err)
                    {
                        // if it failes to parse the return message it is not a valid message
                        test.XML_Errors = err.Message;
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to validate message received - " + err.Message + Environment.NewLine;
                        stepPassed = false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 1:
                    results += STEP_SPACING + "Step 1 - Transmit GetStreamUriRequest message" + Environment.NewLine;
                    results += STEP_SPACING + "Profile token, RTP-Unicast, RTP" + Environment.NewLine;
                    // build the message
                    Media.GetStreamUri GSUri = new Media.GetStreamUri();

                    GSUri.ProfileToken = "";

                    if (Parameters.Profile_Token == null || Parameters.Profile_Token.Equals(""))
                    {
                        //results += STEP_SPACING + "Profile token not set, perform GetProfilesRequest for valid profiles" + Environment.NewLine;
                        GSUri.ProfileToken = "0";
                    } else
                        GSUri.ProfileToken = Parameters.Profile_Token;

                    
                    GSUri.StreamSetup = new Media.StreamSetup();
                    GSUri.StreamSetup.Transport = new Media.Transport();
                    GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.HTTP; // this will be changed later
                    GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;
                    

                    test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                    // because we cannot set the protocol to an invalid value while using the .net object do it here
                    if (test.MessagesSent.Contains("<tt:Protocol>HTTP</tt:Protocol>"))
                        test.MessagesSent = test.MessagesSent.Replace("<tt:Protocol>HTTP</tt:Protocol>", "<tt:Protocol>RTP</tt:Protocol>");

                    // send the message
                    try
                    {
                        test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                        test.StepComplete(false);
                        break;
                    }
                                        
                    test.StepComplete(true);
                    break;

                case 2:
                    results += STEP_SPACING + "Step 2 - Receive SOAP 1.2 fault message " + Environment.NewLine;
                    // the response has already been received, now validate it.

                    // check to make sure there wasn't an error
                    if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                    {
                        test.SoapErrors += soapFault;
                        

                        // ERRATA #3 CHANGE
                        // Change all occurrences of ‘(InvalidArgs/InvalidStreamSetup)’ to ‘(InvalidArgVal)’
                        //if (!TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal\\ter:InvalidStreamSetup"))
                        if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender\\ter:InvalidArgVal"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed &= true;
                        }
                        else if (TestMessage.Verify_SoapErrorType(test.MessagesReceived, out soapFault, "env:Sender"))
                        {
                            results += STEP_MSG_SPACING + "POST returned a SOAP error - " + test.SoapErrors + Environment.NewLine;
                            results += STEP_MSG_SPACING + "as required" + Environment.NewLine;
                            stepPassed &= true;
                        }
                        else
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "SOAP Error returned not as expected, " + soapFault + Environment.NewLine;
                            stepPassed &= false;                            
                        }

                        test.StepComplete(stepPassed);
                        break;
                    }

                    results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST did NOT retur a SOAP error as expected" + Environment.NewLine;
                    test.StepComplete(false);   
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
            }

            return results;
        }
        #endregion

        #region RTSP tests

        /// <summary>
        /// Real Time Streaming Protocol, RTSP TCP test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string REAL_TIME_STREAMING_RTSP_TCP(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            DialogResult aDialog;
            int profile, count;
            bool itemFound;
            string soapFault = "";
            Media.GetProfilesResponse GPsR = new Media.GetProfilesResponse();
            Media.GetStreamUriResponse GSUriR = new Media.GetStreamUriResponse();

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Setup NVT Media Stream URI - RTP/UDP Unicast transport" + Environment.NewLine;
                    
                    // perform test 8.3.4
                    try
                    {
                        // Get Profiles
                        test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Profiles Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Profiles Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Set Video Encoder Configuration
                        Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                        SVEC.ForcePersistence = false;
                        profile = 0;
                        count = 0;
                        itemFound = false;
                        foreach (Media.Profile aProfile in GPsR.Profiles)
                        {
                            if (aProfile.VideoEncoderConfiguration != null)
                            {
                                profile = count;
                                itemFound = true;
                                break;
                            }
                            count++;
                        }

                        // if no profile was found stop the test, this is a problem
                        if (!itemFound)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Video Encoder Configuration found in profiles recieved" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GPsR.Profiles[profile].VideoEncoderConfiguration == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video Profile[" + profile.ToString() + "] does not contain a valid Video Encoder Configuration, unable to continue" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        results += STEP_MSG_SPACING + "Video Encoder Configuration found" + Environment.NewLine;

                        SVEC.Configuration = GPsR.Profiles[profile].VideoEncoderConfiguration;
                        SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                        results += STEP_MSG_SPACING + "Using profile \"" + GPsR.Profiles[profile].Name + "\" token = \"" + GPsR.Profiles[profile].token + "\"" + Environment.NewLine;

                        Parameters.Temporary_String = GPsR.Profiles[profile].Name;
                        Parameters.Profile_Token = GPsR.Profiles[profile].token;

                        test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Set Video Encoder Configuration Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Get Stream URI Request
                        Media.GetStreamUri GSUri = new Media.GetStreamUri();

                        GSUri.ProfileToken = GPsR.Profiles[profile].token;

                        GSUri.StreamSetup = new Media.StreamSetup();
                        GSUri.StreamSetup.Transport = new Media.Transport();
                        GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.UDP;
                        GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;


                        test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Stream Uri Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Stream Uri Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GSUriR.MediaUri.Uri == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response, Media URI is null" + GSUriR.MediaUri.Uri + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                        else
                            results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;

                        results += STEP_MSG_SPACING + "Opening the video window" + Environment.NewLine;
                        Parameters.Temporary_String = GSUriR.MediaUri.Uri;
                        Parameters.Video_OpenWindow.Invoke();
                        System.Threading.Thread.Sleep(200);
                        Parameters.Video_SetMode.Invoke(true);
                        System.Threading.Thread.Sleep(200);
                        Parameters.RTSPInitVidStream.Invoke(Parameters.Temporary_String);
                        System.Threading.Thread.Sleep(200);

                        // set the credentials
                        Parameters.Video_SetCredentials.Invoke(Parameters.UserName, Parameters.Password);

                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to get Control Stream URI - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }

                    test.StepComplete(stepPassed);
                    break;

                

                case 1:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP OPTIONS" + Environment.NewLine;
                    try
                    {
                        stepPassed &= Parameters.RTSPOptions.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Options - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP OPTIONS response" + Environment.NewLine;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 3:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP DESCRIBE" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPDescribe.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Describe - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP DESCRIBE response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 5:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP SETUP" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPSetup.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Setup - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP SETUP response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 7:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP PLAY" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPPlay.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Play - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 8:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP PLAY response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 9:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Live Video, receive RTP packets" + Environment.NewLine;

                    aDialog = MessageBox.Show("Is live video present?", "Video running", MessageBoxButtons.YesNo);

                    if (aDialog == DialogResult.Yes)
                        stepPassed &= true;
                    else
                        stepPassed &= false;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 10:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP TEARDOWN" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPTeardown.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Teardown - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 11:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP TEARDOWN response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                Parameters.Video_CloseWindow.Invoke();
            }

            return results;
        }

        /// <summary>
        /// Real Time Streaming Protocol, RTP UDP Unicast test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string REAL_TIME_STREAMING_RTP_UDP_UNICAST(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            DialogResult aDialog;
            int profile, count;
            bool itemFound;
            string soapFault = "";
            Media.GetProfilesResponse GPsR = new Media.GetProfilesResponse();
            Media.GetStreamUriResponse GSUriR = new Media.GetStreamUriResponse();


            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Setup NVT Media Stream URI - RTP/UDP Unicast transport" + Environment.NewLine;

                    // perform test 8.3.4
                    try
                    {
                        // Get Profiles
                        test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Profiles Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Profiles Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Set Video Encoder Configuration
                        Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                        SVEC.ForcePersistence = false;
                        profile = 0;
                        count = 0;
                        itemFound = false;
                        foreach (Media.Profile aProfile in GPsR.Profiles)
                        {
                            if (aProfile.VideoEncoderConfiguration != null)
                            {
                                profile = count;
                                itemFound = true;
                                break;
                            }
                            count++;
                        }

                        // if no profile was found stop the test, this is a problem
                        if (!itemFound)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Video Encoder Configuration found in profiles recieved" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GPsR.Profiles[profile].VideoEncoderConfiguration == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video Profile[" + profile.ToString() + "] does not contain a valid Video Encoder Configuration, unable to continue" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        results += STEP_MSG_SPACING + "Video Encoder Configuration found" + Environment.NewLine;

                        SVEC.Configuration = GPsR.Profiles[profile].VideoEncoderConfiguration;
                        SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                        results += STEP_MSG_SPACING + "Using profile \"" + GPsR.Profiles[profile].Name + "\" token = \"" + GPsR.Profiles[profile].token + "\"" + Environment.NewLine;

                        Parameters.Temporary_String = GPsR.Profiles[profile].Name;
                        Parameters.Profile_Token = GPsR.Profiles[profile].token;

                        test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Set Video Encoder Configuration Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Get Stream URI Request
                        Media.GetStreamUri GSUri = new Media.GetStreamUri();

                        GSUri.ProfileToken = GPsR.Profiles[profile].token;

                        GSUri.StreamSetup = new Media.StreamSetup();
                        GSUri.StreamSetup.Transport = new Media.Transport();
                        GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.UDP;
                        GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;


                        test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Stream Uri Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Stream Uri Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GSUriR.MediaUri.Uri == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response, Media URI is null" + GSUriR.MediaUri.Uri + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                        else
                            results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;

                        // Open Video stream
                        results += STEP_MSG_SPACING + "Opening the video window" + Environment.NewLine;
                        Parameters.Temporary_String = GSUriR.MediaUri.Uri;
                        Parameters.Video_OpenWindow.Invoke();
                        System.Threading.Thread.Sleep(200);
                        Parameters.Video_SetMode.Invoke(true);
                        System.Threading.Thread.Sleep(200);
                        Parameters.RTSPInitVidStream.Invoke(Parameters.Temporary_String);
                        System.Threading.Thread.Sleep(200);

                        // set the credentials
                        Parameters.Video_SetCredentials.Invoke(Parameters.UserName, Parameters.Password);

                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to get Control Stream URI - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 1:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP DESCRIBE" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPDescribe.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Describe - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP DESCRIBE response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 3:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP SETUP" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPSetup.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Setup - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP SETUP response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 5:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP PLAY" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPPlay.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Play - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP PLAY response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 7:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Live Video, receive RTP packets" + Environment.NewLine;

                    aDialog = MessageBox.Show("Is live video present?", "Video running", MessageBoxButtons.YesNo);

                    if (aDialog == DialogResult.Yes)
                        stepPassed &= true;
                    else
                        stepPassed &= false;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 8:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP TEARDOWN" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPTeardown.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Teardown - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 9:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP TEARDOWN response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }
            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                Parameters.Video_CloseWindow.Invoke();
            }

            return results;
        }

        /// <summary>
        /// Real Time Streaming Protocol, RTP RTSP HTTP test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string REAL_TIME_STREAMING_RTP_RTSP_HTTP(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            DialogResult aDialog;
            int profile, count;
            string soapFault = "";
            Media.GetStreamUri GSUri;
            Media.GetStreamUriResponse GSUriR;
            Media.GetProfilesResponse GPsR = new Media.GetProfilesResponse();
            string tmpMessage = "";
            bool itemFound;

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Setup NVT Media Stream URI - RTP/RTSP/HTTP transport" + Environment.NewLine;
                    // Get Profiles
                    try
                    {
                        test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Profiles Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Profiles Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Set Video Encoder Configuration
                        Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                        SVEC.ForcePersistence = false;

                        profile = 0;
                        count = 0;
                        itemFound = false;
                        foreach (Media.Profile aProfile in GPsR.Profiles)
                        {
                            if (aProfile.VideoEncoderConfiguration != null)
                            {
                                profile = count;
                                itemFound = true;
                                break;
                            }
                            count++;
                        }

                        // if no profile was found stop the test, this is a problem
                        if (!itemFound)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Video Encoder Configuration found in profiles recieved" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GPsR.Profiles[profile].VideoEncoderConfiguration == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video Profile[" + profile.ToString() + "] does not contain a valid Video Encoder Configuration, unable to continue" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        SVEC.Configuration = GPsR.Profiles[profile].VideoEncoderConfiguration;
                        SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;


                        Parameters.Temporary_String = GPsR.Profiles[profile].Name;
                        Parameters.Profile_Token = GPsR.Profiles[profile].token;

                        test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Set Video Encoder Configuration Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }


                        // Get Stream URI
                        GSUri = new Media.GetStreamUri();

                        GSUri.ProfileToken = GPsR.Profiles[profile].token;

                        GSUri.StreamSetup = new Media.StreamSetup();
                        GSUri.StreamSetup.Transport = new Media.Transport();
                        GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.HTTP;
                        GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;

                        test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Stream Uri Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Profiles Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GSUriR.MediaUri.Uri == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response, Media URI is null" + GSUriR.MediaUri.Uri + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                        else
                            results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;

                        // check the URI, it should start with HTTP, if not fix it THIS IS A BUG IN THE CAMERA
                        if (!GSUriR.MediaUri.Uri.StartsWith("http://"))
                        {
                            //GSUriR.MediaUri.Uri = GSUriR.MediaUri.Uri.Replace("rtsp://", "http://");
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response, URI does not start with \"http://\"" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }


                        results += STEP_MSG_SPACING + "Opening the video window" + Environment.NewLine;
                        Parameters.Temporary_String = GSUriR.MediaUri.Uri;
                        Parameters.Video_OpenWindow.Invoke();
                        System.Threading.Thread.Sleep(200);
                        Parameters.Video_SetMode.Invoke(true);
                        System.Threading.Thread.Sleep(200);
                        Parameters.RTSPInitVidStream.Invoke(Parameters.Temporary_String);
                        System.Threading.Thread.Sleep(200);

                        // set the credentials
                        Parameters.Video_SetCredentials.Invoke(Parameters.UserName, Parameters.Password);

                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to get Control Stream URI - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    test.StepComplete(stepPassed);
                    break;

                case 1:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send HTTP Get request" + Environment.NewLine;

                    //test.MessagesReceived = NetworkInterface.GET(Parameters.Temporary_String);
                    stepPassed &= Parameters.HTTP_GET.Invoke(out errorMessages, out tmpMessage);
                    test.MessagesSent = tmpMessage;
                    results += errorMessages;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate HTTP Get response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 3:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send HTTP POST request" + Environment.NewLine;

                    stepPassed &= Parameters.HTTP_POST.Invoke(out errorMessages, out tmpMessage);
                    test.MessagesReceived = tmpMessage;
                    results += errorMessages;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP DESCRIBE" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPDescribe.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Describe - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 5:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP DESCRIBE response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP SETUP" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPSetup.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Setup - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 7:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP SETUP response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 8:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP PLAY" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPPlay.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Play - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 9:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP PLAY response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 10:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Live Video, receive HTTP packets" + Environment.NewLine;
                    aDialog = MessageBox.Show("Is live video present?", "Video running", MessageBoxButtons.YesNo);

                    if (aDialog == DialogResult.Yes)
                        stepPassed &= true;
                    else
                        stepPassed &= false;

                    test.StepComplete(stepPassed);
                    break;

                case 11:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP TEARDOWN" + Environment.NewLine;

                    try
                    {
                        Parameters.RTSPTeardown.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        // do not expect a response during an HTTP test, not required for the test to pass according to the ONVIF Test Spec 1.0
                        stepPassed &= true;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Teardown - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }

                    test.StepComplete(stepPassed);
                    break;
                                   
                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                Parameters.Video_CloseWindow.Invoke();
            }

            return results;
        }

        /// <summary>
        /// Real Time Streaming Protocol, keepalive test
        /// </summary>
        /// <param name="test">Referance to Test Case</param>
        /// <returns>Test step results</returns>
        public string REAL_TIME_STREAMING_RTSP_KEEPALIVE(ref ONVIF_TestCases.TestCases_Class.Test_Type test,ref ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters, ref ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface)
        {
            bool stepPassed = true;
            string results = "";
            string errorMessages = "";
            DialogResult aDialog;
            int profile, count;
            bool itemFound;
            string soapFault = "";
            Media.GetProfilesResponse GPsR = new Media.GetProfilesResponse();
            Media.GetStreamUriResponse GSUriR = new Media.GetStreamUriResponse();

            // this test requires the target URL, IP, multicast, port and TTL
            if ((Parameters.URL == null) || (Parameters.URL.Equals("")))
                throw new TestCase_ExecuteException("Target URL not set");

            if ((Parameters.Target_IP == null) || (Parameters.Target_IP.Equals("")))
                throw new TestCase_ExecuteException("Target IP not set");

            if ((Parameters.Multicast_IP == null) || (Parameters.Multicast_IP.Equals("")))
                throw new TestCase_ExecuteException("Multicast IP not set");

            if (Parameters.Port == 0)
                throw new TestCase_ExecuteException("Port not set");

            if ((Parameters.Media_ServiceAddress == "") && !(Get_DeviceMediaServiceAddress(ref Parameters, ref NetworkInterface)))
                throw new TestCase_ExecuteException("Unable to retrieve Media Service Address");

            switch (test.CurrentStep) // TODO: PERFORM TEST HERE
            {
                case 0:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Setup NVT Media Stream URI - RTP/UDP Unicast transport" + Environment.NewLine;

                    // perform test 8.3.4
                    try
                    {
                        // Get Profiles
                        test.MessagesSent = TestMessage.Build_Media_GetProfilesRequest();

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Profiles Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetProfilesResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Profiles Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Set Video Encoder Configuration
                        Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                        SVEC.ForcePersistence = false;
                        profile = 0;
                        count = 0;
                        itemFound = false;
                        foreach (Media.Profile aProfile in GPsR.Profiles)
                        {
                            if (aProfile.VideoEncoderConfiguration != null)
                            {
                                profile = count;
                                itemFound = true;
                                break;
                            }
                            count++;
                        }

                        // if no profile was found stop the test, this is a problem
                        if (!itemFound)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "No Video Encoder Configuration found in profiles recieved" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GPsR.Profiles[profile].VideoEncoderConfiguration == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Video Profile[" + profile.ToString() + "] does not contain a valid Video Encoder Configuration, unable to continue" + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        results += STEP_MSG_SPACING + "Video Encoder Configuration found" + Environment.NewLine;

                        SVEC.Configuration = GPsR.Profiles[profile].VideoEncoderConfiguration;
                        SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                        results += STEP_MSG_SPACING + "Using profile \"" + GPsR.Profiles[profile].Name + "\" token = \"" + GPsR.Profiles[profile].token + "\"" + Environment.NewLine;

                        Parameters.Temporary_String = GPsR.Profiles[profile].Name;
                        Parameters.Profile_Token = GPsR.Profiles[profile].token;

                        test.MessagesSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Set Video Encoder Configuration Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // Get Stream URI Request
                        Media.GetStreamUri GSUri = new Media.GetStreamUri();

                        GSUri.ProfileToken = GPsR.Profiles[profile].token;

                        GSUri.StreamSetup = new Media.StreamSetup();
                        GSUri.StreamSetup.Transport = new Media.Transport();
                        GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.UDP;
                        GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;


                        test.MessagesSent = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                        try
                        {
                            results += STEP_MSG_SPACING + "Send \"Get Stream Uri Request\"" + Environment.NewLine;
                            test.MessagesReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, test.MessagesSent, Parameters.UserName, Parameters.Password);
                        }
                        catch (Exception e)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST failed, " + e.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        // check to make sure there wasn't an error
                        if (TestMessage.Check_SoapFault(test.MessagesReceived, out soapFault))
                        {
                            test.SoapErrors += soapFault;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "POST returned a SOAP error - " + soapFault + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        try
                        {
                            GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(test.MessagesReceived, typeof(Media.GetStreamUriResponse));
                        }
                        catch (Exception err)
                        {
                            // if it failes to parse the return message it is not a valid message
                            test.XML_Errors = err.Message;
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to parse Get Stream Uri Response message received - " + err.Message + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }

                        if (GSUriR.MediaUri.Uri == null)
                        {
                            results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Get Stream Uri Response, Media URI is null" + GSUriR.MediaUri.Uri + Environment.NewLine;
                            test.StepComplete(false);
                            break;
                        }
                        else
                            results += STEP_MSG_SPACING + "Stream URI = " + GSUriR.MediaUri.Uri + Environment.NewLine;

                        // Open Video stream
                        results += STEP_MSG_SPACING + "Opening the video window" + Environment.NewLine;
                        Parameters.Temporary_String = GSUriR.MediaUri.Uri;
                        Parameters.Video_OpenWindow.Invoke();
                        System.Threading.Thread.Sleep(200);
                        Parameters.Video_SetMode.Invoke(true);
                        System.Threading.Thread.Sleep(200);
                        Parameters.RTSPInitVidStream.Invoke(Parameters.Temporary_String);
                        System.Threading.Thread.Sleep(200);

                        // set the credentials
                        Parameters.Video_SetCredentials.Invoke(Parameters.UserName, Parameters.Password);
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to get Control Stream URI - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 1:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP DESCRIBE" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPDescribe.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Describe - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 2:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP DESCRIBE response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 3:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP SETUP" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPSetup.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Setup - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 4:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP SETUP response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 5:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP PLAY" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPPlay.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Play - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 6:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP PLAY response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 7:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP SET_PARAMETER \"Timeout\"" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPSetParameter.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Set Parameter - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 8:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP SET_PARAMETER response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                case 9:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Live Video, receive RTP packets" + Environment.NewLine;

                    aDialog = MessageBox.Show("Is live video present?", "Video running", MessageBoxButtons.YesNo);

                    if (aDialog == DialogResult.Yes)
                        stepPassed &= true;
                    else
                        stepPassed &= false;
                    
                    test.StepComplete(stepPassed);
                    break;

                case 10:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Send RTSP TEARDOWN" + Environment.NewLine;

                    try
                    {
                        stepPassed &= Parameters.RTSPTeardown.Invoke(out errorMessages, out Parameters.RTSP_Response);
                        results += errorMessages;
                    }
                    catch (Exception e)
                    {
                        results += STEP_MSG_SPACING + ERROR_MSG_PREFIX + "Unable to call RTSP Teardown - " + e.Message + Environment.NewLine;
                        stepPassed &= false;
                    }
                    
                    test.StepComplete(stepPassed);
                    break;

                case 11:
                    results += STEP_SPACING + "Step " + (test.CurrentStep + 1).ToString() + " - Validate RTSP TEARDOWN response" + Environment.NewLine;
                    test.StepComplete(stepPassed);
                    break;

                default:
                    test.TestComplete = true;
                    break;
            }

            if (test.TestComplete)
            {
                if (test.TestPassed)
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;
                else
                    results += STEP_SPACING + "Test complete" + Environment.NewLine;

                Parameters.Video_CloseWindow.Invoke();
            }

            return results;
        }

        #endregion


    }

}
