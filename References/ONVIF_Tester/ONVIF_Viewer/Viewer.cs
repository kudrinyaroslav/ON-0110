/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ONVIF_TestCases;
using System.Net;

using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.WindowsForms;


namespace ONVIF_Viewer
{
    public partial class Viewer : Form
    {
        #region Delegates

        delegate void ActionCallBack();
        
        #endregion

        #region Structures
        
        public struct DiscoveredDevices_Type
        {
            public RemoteDiscovery.ProbeMatchesType MatchesType;
            public string IP;
        }

        #endregion

        #region Constants

        private const string ONVIF_Spec_MajorVersion = "1"; // # of cherictors based on ONVIF version
        private const string ONVIF_Spec_MinorVersion = "01"; // # of cherictors based on ONVIF version

        private const string Test_Tool_MajorVersion = "0";  // 1 charictor
        private const string Test_Tool_MinorVersion = "04"; // 2 charictors

        private const string ONVIF_SpecVersion = ONVIF_Spec_MajorVersion + "." + ONVIF_Spec_MinorVersion;
        private const string Version = Test_Tool_MajorVersion + "." + Test_Tool_MinorVersion;

        private const string ToolVersion = "ONVIF Test Tool version " + ONVIF_SpecVersion + "." + Version;
        private const string TestVersion = "ONVIF Test Specification version 1.01, September 2009";
        private const string CoreVersion = "ONVIF Core Specification version 1.01, July 2009";

        #endregion

        #region Variables

        private ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface = new ONVIF_NetworkInterface.NetworkInterface_Class();
        private DiscoveredDevices_Type[] DiscoveredDevices = new DiscoveredDevices_Type[1];
        private int DiscoveredDevices_Count = 0;
        private ONVIF_TestCases.TestMessages TestMessage = new TestMessages();
        private ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters = new TestCases_Class.TestParameters_Type();

        private bool TargetAddressTextChange = false;

        // video stream viewer        
        private DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl vlcPlayerControl;

        private Media.Profile[] VideoProfiles;

        #endregion

        #region Form Initialization

        /// <summary>
        /// Form initilization
        /// </summary>
        public Viewer()
        {
            InitializeComponent();

#if !DEBUG
            gb_DeviceInfo.Visible = false;
#endif
            

            
        }


        /// <summary>
        /// Form "On Load" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewer_Load(object sender, EventArgs e)
        {          

            this.Text += " " + ONVIF_SpecVersion + "." + Version;
        }

        /// <summary>
        /// Form "on Closing" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseVidStream();

        }


        /// <summary>
        /// Form "Shown" event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewer_Shown(object sender, EventArgs e)
        {
            DisplaySplashImage();

            // setup the video streaming control
            SetupVideoWindow();

            vlcPlayerControl.Initialize();

            UpdateTestParameters(false);

            TestMessage.InitSchemaCollection();

            HideSplashImage();
        }

        /// <summary>
        /// Ths function allows the running thread to setup the video display and perform
        /// the neccissary form size adjustments
        /// </summary>
        private void SetupVideoWindow()
        {
            
 
            // 
            // vlcPlayerControl
            // 
            this.vlcPlayerControl = new VlcPlayerControl();

            this.vlcPlayerControl.Location = gb_VideoBox.Location;
            this.vlcPlayerControl.Name = "vlcPlayerControl";
            this.vlcPlayerControl.Position = 0;
            this.vlcPlayerControl.Size = gb_VideoBox.Size;
            this.vlcPlayerControl.TabIndex = 9;
            this.vlcPlayerControl.Time = System.TimeSpan.Parse("00:00:00");
            this.vlcPlayerControl.Volume = 50;

            this.Controls.Add(this.vlcPlayerControl);


            gb_VideoBox.Visible = false;
        }

        /// <summary>
        /// Display the Splash image
        /// </summary>
        private void DisplaySplashImage()
        {
            pnl_SplashPanel.MinimumSize = new Size(this.Size.Width, this.Size.Height);
            pnl_SplashPanel.MaximumSize = new Size(this.Size.Width, this.Size.Height);
            pnl_SplashPanel.Size = new Size(this.Size.Width, this.Size.Height);

            pnl_SplashPanel.Location = new Point(0, 0);
            pnl_SplashPanel.Visible = true;
            pnl_SplashPanel.Update();

        }

        /// <summary>
        /// Hide the Splash image
        /// </summary>
        private void HideSplashImage()
        {
            pnl_SplashPanel.Visible = false;
            pnl_SplashPanel.Update();
            pnl_SplashPanel.Location = new Point(0, this.Size.Height + 20);
            

        }

        /// <summary>
        /// Stop the video stream tool and cleanup
        /// </summary>
        private void CloseVidStream()
        {

            stopPlayer();
            vlcPlayerControl.Dispose();
        }

        /// <summary>
        /// Stop the video player
        /// </summary>
        void stopPlayer()
        {
            try
            {
                if (vlcPlayerControl.State != VlcPlayerControlState.IDLE)
                {
                    vlcPlayerControl.Stop();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            btn_LiveVideo.Text = "Connect";

        }
        #endregion


        #region Form Events

        private void btn_discover_Click(object sender, EventArgs e)
        {
            string messageSent, messageReceived;
            bool msgReceived = true;
            RemoteDiscovery.ProbeMatchesType PMT;
            RemoteDiscovery.ScopesType Scope;
            int msgErrorCount = 0;
            string tmpTxt = btn_discover.Text;
            IPEndPoint receivedFrom;

            btn_discover.Text = "Please wait";
            btn_discover.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            // clear the old results
            ClearDiscoveredDevices();


            try
            {
                // setup the network interface
                NetworkInterface.UDP_ConnectAnyMulticast("239.255.255.250", 3702, 1);

                // build the probe request
                Scope = new RemoteDiscovery.ScopesType();
                Scope.Text = new string[] { "" }; //"onvif://www.onvif.org/type/super" , " onvif://www.onvif.org/type/analytics ", " onvif://www.onvif.org/type/video", " onvif://www.onvif.org/name/Bosch", " onvif://www.onvif.org/location/city/Nuernberg", " onvif://www.onvif.org/hardware/Dinion-IP-NWC" };
                messageSent = TestMessage.Build_ProbeRequest("", Scope);
            }
            catch (Exception error)
            {
                System.Windows.Forms.MessageBox.Show("Unexpected error - " + error.Message + Environment.NewLine + "Please validate network connections", "Error", MessageBoxButtons.OK);
                Cursor.Current = Cursors.Default;
                return;
            }



            // check to see if any single device responds
            try
            {
                //NetworkInterface.Send(messageSent);
                NetworkInterface.SendMulticast(messageSent);
            }
            catch (Exception error1)
            {
                // there was a problem sending, don't bother listening
                msgReceived = false;
                Console.WriteLine(error1.Message);

            }

            // now listen for any others
            while (msgReceived)
            {

                try
                {
                    // check for more messages
                    messageReceived = NetworkInterface.UDP_Listen(500, out receivedFrom);
                    //messageReceived = NetworkInterface.Receive(500, out receivedFrom);
                                      
                    PMT = (RemoteDiscovery.ProbeMatchesType)TestMessage.Parse_SoapMessage(messageReceived, typeof(RemoteDiscovery.ProbeMatchesType));
                    AddDiscoveredDevice(PMT, receivedFrom);
                    msgErrorCount = 0;
                }
                catch (Exception error2)
                {
                    // it this was a timeout then stop
                    if (error2.GetType() == typeof(ONVIF_NetworkInterface.NetworkInterface_TimeoutException))
                        msgReceived = false;
                    else // there are other mutlicast responses so ignore them, as long as receiving messages keep trying
                    {

                        if (msgErrorCount++ < 10)
                            msgReceived = true;
                        else
                            msgReceived = false;
                    }
                }
            }

            NetworkInterface.UDP_Close();

            btn_discover.Text = tmpTxt;
            btn_discover.Refresh();
            Cursor.Current = Cursors.Default;
        }
        
        private void AddDiscoveredDevice(RemoteDiscovery.ProbeMatchesType newDev, IPEndPoint receivedFrom)
        {
            DiscoveredDevices_Type[] tmpDev;
            int x;

            if ((newDev.ProbeMatch == null) ||
                (newDev.ProbeMatch[0].XAddrs == null))
            {
                // this is a problem since this is a required field
                // tell the user there was an erro on the device but don't
                // add it to the list

                return;
            }

            if (newDev.ProbeMatch[0].Types != OnvifTests.DEFAULT_DEVICE_TYPE)
                return;

            // first make sure this device hasn't already been added
            for (x = 0; (x < DiscoveredDevices_Count) && (x < DiscoveredDevices.Length); x++)
            {
                if (DiscoveredDevices[x].IP == receivedFrom.Address.ToString())
                    return;

                if ((DiscoveredDevices[x].MatchesType != null) &&
                    (DiscoveredDevices[x].MatchesType.ProbeMatch[0] != null) &&
                    (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference != null) &&
                    (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address != null) &&
                    (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address.Value != null) &&
                    (newDev.ProbeMatch[0].EndpointReference != null) &&
                    (newDev.ProbeMatch[0].EndpointReference.Address != null) &&
                    (newDev.ProbeMatch[0].EndpointReference.Address.Value != null))
                {
                    if (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address.Value == newDev.ProbeMatch[0].EndpointReference.Address.Value)
                        return;
                }

                //if (DiscoveredDevices[x].ProbeMatch[0].EndpointReference.Address.Value == newDev.ProbeMatch[0].EndpointReference.Address.Value)
                //     return;

            }

            DiscoveredDevices_Count++;

            if (DiscoveredDevices_Count > DiscoveredDevices.Length)
            {
                tmpDev = new DiscoveredDevices_Type[DiscoveredDevices.Length];

                Array.Copy(DiscoveredDevices, tmpDev, DiscoveredDevices.Length);

                DiscoveredDevices = new DiscoveredDevices_Type[DiscoveredDevices.Length + 1];

                Array.Copy(tmpDev, DiscoveredDevices, tmpDev.Length);
            }

            DiscoveredDevices[DiscoveredDevices_Count - 1].MatchesType = newDev;

            // update the device list
            if (newDev.ProbeMatch[0].EndpointReference != null)
                lb_DiscoverdDevices.Items.Add(newDev.ProbeMatch[0].EndpointReference.Address.Value);
            else
                lb_DiscoverdDevices.Items.Add("Device - " + DiscoveredDevices_Count.ToString());

            
        }

        private void lb_DiscoverdDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x = lb_DiscoverdDevices.SelectedIndex;
            string tmpstring;

            if (x < 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            // clear the old items            
            ClearTestParameters();

            if (DiscoveredDevices[x].MatchesType.ProbeMatch == null)
            {
                MessageBox.Show("This device does not have a probe match, unable to select", "ERROR", MessageBoxButtons.OK);
                return;
            }

            if ((DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference != null) &&
                (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address != null) &&
                (DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address.Value != null))
                tb_TargetEndPointAddress.Text = DiscoveredDevices[x].MatchesType.ProbeMatch[0].EndpointReference.Address.Value;

            if (DiscoveredDevices[x].MatchesType.ProbeMatch[0].Types != null)
                tb_TargetType.Text = DiscoveredDevices[x].MatchesType.ProbeMatch[0].Types;

            if ((DiscoveredDevices[x].MatchesType.ProbeMatch[0].Scopes != null) &&
                (DiscoveredDevices[x].MatchesType.ProbeMatch[0].Scopes.Text != null))
            {
                foreach (string scope in DiscoveredDevices[x].MatchesType.ProbeMatch[0].Scopes.Text)
                    tb_TargetScopes.Text = scope + Environment.NewLine;
            }

            // a device may have several XAddreses, parse them out and display them seperatly
            if (DiscoveredDevices[x].MatchesType.ProbeMatch[0].XAddrs != null)
            {
                tmpstring = DiscoveredDevices[x].MatchesType.ProbeMatch[0].XAddrs;


                string[] Addresses = tmpstring.Split(new char[] { ' ' });
                foreach (string address in Addresses)
                {
                    cb_TargetAddress.Items.Add(address);
                }
            }

            tb_TargetMDversion.Text = DiscoveredDevices[x].MatchesType.ProbeMatch[0].MetadataVersion.ToString();

            if (cb_TargetAddress.Items.Count > 0)
            {
                //cb_TargetAddress.SelectedIndex = 0;
                cb_TargetAddress_SelectedIndexChanged(null, null);
            }


            // update the test
            UpdateTestParameters(false);
            //RetreiveDeviceInformation();
            Cursor.Current = Cursors.Default;

        }

        private void cb_TargetAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = "";
            Cursor.Current = Cursors.WaitCursor;
            int x = 0;

            // stop the player
            stopPlayer();

            try
            {

                if (cb_TargetAddress.SelectedIndex >= 0)
                {
                    tmpstring = cb_TargetAddress.SelectedItem.ToString();
                    // parse the IP address out of the URL
                    tb_TargetIPAddress.Text = TestMessage.ParseIpAddress(tmpstring);
                    UpdateTestParameters(false);
                    if (UpdateProfiles(Parameters.URL, true))
                        PlayVideo();
                }
                else
                {
                    // find a valid profile
                    foreach (object aItem in cb_TargetAddress.Items)
                    {
                        tmpstring = aItem.ToString();
                        Parameters.URL = tmpstring;
                        if (UpdateProfiles(tmpstring, false))
                        {                            
                            cb_TargetAddress.SelectedIndexChanged -= new System.EventHandler(this.cb_TargetAddress_SelectedIndexChanged);
                            cb_TargetAddress.SelectedIndex = x;
                            cb_TargetAddress.SelectedIndexChanged += new System.EventHandler(this.cb_TargetAddress_SelectedIndexChanged);
                            PlayVideo();
                            break;
                        }
                        else
                            x++;
                    }
                }

            }
            catch (Exception err)
            {
                Console.WriteLine("error - " + err.Message);
                tb_TargetIPAddress.Text = "";
            }

            Cursor.Current = Cursors.Default;
        }

        private void cb_TargetAddress_TextUpdate(object sender, EventArgs e)
        {
            TargetAddressTextChange = true;
        }

        private void cb_TargetAddress_Leave(object sender, EventArgs e)
        {
            string tmpString;
            // if the user made a change handle it
            if (TargetAddressTextChange)
            {
                tmpString = cb_TargetAddress.Text;

                // if the address already exists don't add it to the list
                if (!cb_TargetAddress.Items.Contains(tmpString))
                {
                    cb_TargetAddress.Items.Add(tmpString);
                    cb_TargetAddress.SelectedItem = tmpString;

                }

                // update the IP address
                tb_TargetIPAddress.Text = TestMessage.ParseIpAddress(tmpString);

                // update the parameters
                UpdateTestParameters(false);
                UpdateProfiles(Parameters.Media_ServiceAddress, true);
            }
        }

        private void btn_LiveVideo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (vlcPlayerControl.State == VlcPlayerControlState.IDLE)
            {

                PlayVideo();
                    
                    
            }
            else
            {
                stopPlayer();
                
            }

            Cursor.Current = Cursors.Default;
        }

        private void cb_VideoStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (vlcPlayerControl.State != VlcPlayerControlState.IDLE)
            {
                btn_LiveVideo.Text = "Connecting";
                btn_LiveVideo.Update();
                if (PlayVideo())
                    btn_LiveVideo.Text = "Stop";
                else
                    btn_LiveVideo.Text = "Connect";
            }
            
            Cursor.Current = Cursors.Default;
        }

        private void lb_DiscoverdDevices_MouseUp(object sender, MouseEventArgs e)
        {
            string deviceString;
            int x;


            if (e.Button == MouseButtons.Right)
            {
                x = lb_DiscoverdDevices.IndexFromPoint(e.Location);
                if (x < 0)
                    return;

                Cursor.Current = Cursors.WaitCursor;

                deviceString = BuildDeviceInfoString(x);
                MessageBox.Show(deviceString, "Device Information", MessageBoxButtons.OK);

                Cursor.Current = Cursors.Default;

            }
        }

        #endregion

        private bool OpenVideoStream(string URI)
        {
            try
            {
                MediaInput input = new MediaInput(MediaInputType.NetworkStream, URI);
                vlcPlayerControl.Play(input);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        private bool PlayVideo()
        {
            Media.StreamSetup streamSetup;
            string msg, returnString, errorMessage = "";
            string URI, soapFault;

            stopPlayer();
            Cursor.Current = Cursors.WaitCursor;

            btn_LiveVideo.Text = "Connecting";
            btn_LiveVideo.Update();

            // get the URI for the selected profile
            if (VideoProfiles == null)
            {
                btn_LiveVideo.Text = "Connect";
                return false;
            }


            // if the selected index is greater then the known profiles length return
            if (cb_VideoStreams.SelectedIndex > VideoProfiles.Length)
            {
                btn_LiveVideo.Text = "Connect";
                return false;
            }

            if ((Parameters.Media_ServiceAddress == "") && !(PollMediaConfigurationAddress(true)))
            {
                System.Windows.Forms.MessageBox.Show("Unable to retrieve Media Service Address.  Unable to view video stream.", "Error", MessageBoxButtons.OK);
                btn_LiveVideo.Text = "Connect";
                return false;
            }

            // configure the video stream

            SetVideoProfileType(VideoProfiles[cb_VideoStreams.SelectedIndex].VideoEncoderConfiguration.Encoding, VideoProfiles[cb_VideoStreams.SelectedIndex].VideoEncoderConfiguration);


            // if the user wishes to see the video stream from the selected profile set it up

            streamSetup = new Media.StreamSetup();
            streamSetup.Stream = new Media.StreamType();
            streamSetup.Transport = new Media.Transport();

            streamSetup.Transport.Protocol = Media.TransportProtocol.UDP;
            streamSetup.Stream = Media.StreamType.RTPUnicast;

            msg = TestMessage.Build_Media_GetStreamUriRequest(VideoProfiles[cb_VideoStreams.SelectedIndex].token, streamSetup);


            try
            {
                returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, msg, Parameters.UserName, Parameters.Password);

                if (TestMessage.Check_SoapFault(returnString, out soapFault))
                {
                    MessageBox.Show("SOAP Fault, unable to connect." + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
                    btn_LiveVideo.Text = "Connect";
                    return false;
                }

                Media.GetStreamUriResponse GSR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetStreamUriResponse));

                if (GSR.MediaUri == null)
                {
                    MessageBox.Show("Unable to connect, Get Stream URI Response MediaUri is NULL" + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
                    btn_LiveVideo.Text = "Connect";
                    return false;
                }

                if (GSR.MediaUri.Uri == null)
                {
                    MessageBox.Show("Unable to connect, Get Stream URI Response does not contain a Media URI" + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
                    btn_LiveVideo.Text = "Connect";
                    return false;
                }

                URI = GSR.MediaUri.Uri;

                if (URI.Contains("&line=0"))
                    URI = URI.Replace("&line=0", "");

                //sc_VideoStream.StreamUri = URI;
                //sc_VideoStream.Play();

                OpenVideoStream(URI);

            }
            catch (Exception err)
            {
                errorMessage = err.Message;
                btn_LiveVideo.Text = "Connect";
            }

            if (errorMessage != "")
            {
                MessageBox.Show("Unable to connect - " + errorMessage, "Error", MessageBoxButtons.OK);
                btn_LiveVideo.Text = "Connect";
            }
            Cursor.Current = Cursors.Default;
            btn_LiveVideo.Text = "Stop";
            return true;
        }

        private void ClearDiscoveredDevices()
        {
            DiscoveredDevices_Count = 0;
            DiscoveredDevices = new DiscoveredDevices_Type[1];

            lb_DiscoverdDevices.Items.Clear();

        }

        /// <summary>
        /// Update the test Parameters object with the DUT information
        /// </summary>
        private void UpdateTestParameters(bool pollDevice)
        {

            Parameters.Multicast_IP = "239.255.255.250";
            Parameters.Port = 3702;
            Parameters.TTL = 1;
            Parameters.URL = cb_TargetAddress.Text;
            Parameters.Target_IP = tb_TargetIPAddress.Text;

            Parameters.TestTimeout = 2000;
            Parameters.RebootTime = 60000;

            Parameters.UserName = "";
            Parameters.Password = "";

            Parameters.Media_ServiceAddress = "";


        }

        /// <summary>
        /// Clear all associated test parameters
        /// </summary>
        private void ClearTestParameters()
        {
            tb_TargetEndPointAddress.Text = "";
            tb_TargetType.Text = "";
            tb_TargetScopes.Text = "";
            //tb_TargetXaddr.Text = "";
            tb_TargetMDversion.Text = "";
            tb_TargetIPAddress.Text = "";
            
            cb_TargetAddress.Items.Clear();
            cb_TargetAddress.Text = "";

            cb_VideoStreams.Items.Clear();
            cb_VideoStreams.Text = "";
        }

        /// <summary>
        /// Poll the device under test for the Media Configuration, fill the parameters obejct with 
        /// the information found.
        /// </summary>
        /// <param name="showErrors">True to display error box on error event</param>
        /// <returns></returns>
        private bool PollMediaConfigurationAddress(bool showErrors)
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
                if(showErrors)
                    System.Windows.Forms.MessageBox.Show("POST of GetCapabilitiesRequest message failed, error = " + e.Message + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
                return false;
            }


            // check to make sure there wasn't an error
            if (TestMessage.Check_SoapFault(MessagesReceived, out soapFault))
            {
                // tell the user the NVC was unable to communicate with the device so no media service address was found
                if (showErrors)
                    System.Windows.Forms.MessageBox.Show("GetCapabilitiesResponse returned SOAP error = " + soapFault + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
                return false;
            }

            // otherwise verify the response
            try
            {
                if (!TestMessage.Verify_GetCapabilitiesResponse(MessagesReceived, ref errorMessages))
                {
                    // tell the user the NVC was unable to communicate with the device so no media service address was found
                    if (showErrors)
                        System.Windows.Forms.MessageBox.Show("GetCapabilitiesResponse message failed failed validation, error = " + errorMessages + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
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
                        if (showErrors)
                            System.Windows.Forms.MessageBox.Show("Required capabilities not found, GetCapabilitiesResponse Capabilities = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
                        return false;
                    }
                    else
                    {
                        if (GCR.Capabilities.Media == null)
                        {
                            // tell the user the NVC was unable to communicate with the device so no media service address was found
                            if (showErrors)
                                System.Windows.Forms.MessageBox.Show("Required capabilities not found, GetCapabilitiesResponse Media = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
                            return false;
                        }
                        else
                        {
                            if (GCR.Capabilities.Media.XAddr == null)
                            {
                                // tell the user the NVC was unable to communicate with the device so no media service address was found
                                if (showErrors)
                                    System.Windows.Forms.MessageBox.Show("Required capabilities not found, GetCapabilitiesResponse Media service address = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
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
                if (showErrors)
                    System.Windows.Forms.MessageBox.Show("GetCapabilitiesResponse message failed failed validation, error = " + err.Message + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to display video.", "ERROR", MessageBoxButtons.OK);
                Parameters.Media_ServiceAddress = "";
                return false;
            }      
        }
        
        
        /// <summary>
        /// Parse the profile list and add the profiles to the Video Streams combo box
        /// </summary>
        /// <param name="ProfilesFound">Array of Media Profile types</param>
        private void FillProfileReport(Media.Profile[] ProfilesFound)
        {
            if (ProfilesFound != null)
            {


                foreach (Media.Profile Profile in ProfilesFound)
                {
                    cb_VideoStreams.Items.Add("Profile " + Profile.Name + " - Token = " + Profile.token);
                }
                cb_VideoStreams.Update();
            }


        }

       
        private bool UpdateProfiles(string URL, bool showErrors)
        {
            string msg, returnString;


            cb_VideoStreams.Items.Clear();
            cb_VideoStreams.Text = "";
            cb_VideoStreams.Update();

            VideoProfiles = null;

            if ((Parameters.Media_ServiceAddress == "") && !(PollMediaConfigurationAddress(showErrors)))
            {
                if(showErrors)
                    System.Windows.Forms.MessageBox.Show("Unable to retrieve Media Service Address.  Unable to open video stream.", "Error", MessageBoxButtons.OK);
                return false;
            }

            cb_VideoStreams.Visible = true;
            msg = TestMessage.Build_Media_GetProfilesRequest();

            try
            {
                returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, msg, Parameters.UserName, Parameters.Password);

                Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));

                if (GPsR.Profiles != null)
                {
                    VideoProfiles = GPsR.Profiles;
                    FillProfileReport(GPsR.Profiles);
                    // auto select the first stream
                    cb_VideoStreams.SelectedIndex = 0;
                }
                else
                {
                    if (showErrors)
                        System.Windows.Forms.MessageBox.Show("No video profiles found on this device.  Unable to open video stream.", "Error", MessageBoxButtons.OK);
                    return false;
                }

            }
            catch (Exception error)
            {

                Console.WriteLine(error.Message);

                return false;
            }

            return true;
        }

        private DeviceManagement.GetDeviceInformationResponse GetDeviceInfo(out string errorMessage)
        {
            return GetDeviceInfo(Parameters.URL, out errorMessage);

        }

        private DeviceManagement.GetDeviceInformationResponse GetDeviceInfo(string URL, out string errorMessage)
        {
            DeviceManagement.GetDeviceInformationResponse GDIR;
            string message, messageReceived, soapFault, errormsg;

            message = TestMessage.Build_GetDeviceInformationRequest();

            try
            {
                messageReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, URL, message, Parameters.UserName, Parameters.Password);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return null;
            }

            if (TestMessage.Check_SoapFault(messageReceived, out soapFault))
            {
                errorMessage = "Soap Fault received - " + soapFault;
                return null;
            }

            errormsg = "";
            if (!TestMessage.Verify_GetDeviceInformationResponse(messageReceived, ref errormsg))
            {
                errorMessage = "Unable to verify device information response - " + errormsg;
                return null;
            }
            errorMessage = ""; // no error
            GDIR = (DeviceManagement.GetDeviceInformationResponse)TestMessage.Parse_SoapMessage(messageReceived, typeof(DeviceManagement.GetDeviceInformationResponse));

            return GDIR;
        }

        private bool RetreiveDeviceInformation(string URL, out string deviceInfo)
        {
            string errorMessage = "";
            DeviceManagement.GetDeviceInformationResponse GDIR;

            string Device_FW = "";
            string Device_HW = "";
            string Device_MFG = "";
            string Device_Model = "";
            string Device_SerialNumber = "";

            deviceInfo = "";

            // send a discovery message and display the information to the user so they can verify
            try
            {
                GDIR = GetDeviceInfo(URL, out errorMessage);
            }
            catch (Exception e)
            {
                return false;
            }
            if (GDIR != null)
            {

                if (GDIR.FirmwareVersion != null)
                    Device_FW = GDIR.FirmwareVersion;


                if (GDIR.HardwareId != null)
                    Device_HW = GDIR.HardwareId;

                if (GDIR.Manufacturer != null)
                    Device_MFG = GDIR.Manufacturer;

                if (GDIR.Model != null)
                    Device_Model = GDIR.Model;

                if (GDIR.SerialNumber != null)
                    Device_SerialNumber = GDIR.SerialNumber;
            }
            else
                return false;

            deviceInfo = "Firmware - " + Device_FW + System.Environment.NewLine +
                         "Hardware - " + Device_HW + System.Environment.NewLine +
                         "Manufacturer - " + Device_MFG + System.Environment.NewLine +
                         "Model - " + Device_Model + System.Environment.NewLine +
                         "Serial Number - " + Device_SerialNumber + System.Environment.NewLine;

            return true;
        }

        private string BuildDeviceInfoString(int index)
        {
            string deviceInfo = "";
            string tmpstring = "";
            string TargetEndPointAddress = "";
            string TargetType = "";
            string TargetScopes = "";
            string TargetMDversion = "";
            string TargetDeviceInfo = "";
            string AddressString = "";
            string[] Addresses;
            string[] Scopes;

            if((index > DiscoveredDevices.Length) || (DiscoveredDevices[index].MatchesType.ProbeMatch == null))
            {
                return "";
            }

            if ((DiscoveredDevices[index].MatchesType.ProbeMatch[0].EndpointReference != null) &&
                (DiscoveredDevices[index].MatchesType.ProbeMatch[0].EndpointReference.Address != null) &&
                (DiscoveredDevices[index].MatchesType.ProbeMatch[0].EndpointReference.Address.Value != null))
                TargetEndPointAddress = DiscoveredDevices[index].MatchesType.ProbeMatch[0].EndpointReference.Address.Value;

            if (DiscoveredDevices[index].MatchesType.ProbeMatch[0].Types != null)
                TargetType = DiscoveredDevices[index].MatchesType.ProbeMatch[0].Types;

            if ((DiscoveredDevices[index].MatchesType.ProbeMatch[0].Scopes != null) &&
                (DiscoveredDevices[index].MatchesType.ProbeMatch[0].Scopes.Text != null))
            {
                foreach (string scope in DiscoveredDevices[index].MatchesType.ProbeMatch[0].Scopes.Text)
                {
                    Scopes = scope.Split(new char[] { ' ' });

                    foreach (string aScope in Scopes)
                    {
                        if (aScope != "")
                            TargetScopes += System.Environment.NewLine + "     " + aScope;
                    }

                }
            }

            // a device may have several XAddreses, parse them out and display them seperatly
            if (DiscoveredDevices[index].MatchesType.ProbeMatch[0].XAddrs != null)
            {
                tmpstring = DiscoveredDevices[index].MatchesType.ProbeMatch[0].XAddrs;


                Addresses = tmpstring.Split(new char[] { ' ' });

                if (Addresses.Length > 0)
                {
                    foreach (string address in Addresses)
                        AddressString += System.Environment.NewLine + "     " + address;


                    foreach (string address in Addresses)
                    {
                        if (RetreiveDeviceInformation(address, out TargetDeviceInfo))
                            break;
                    }
                }
            }

            TargetMDversion = DiscoveredDevices[index].MatchesType.ProbeMatch[0].MetadataVersion.ToString();



            deviceInfo = "Target End Point Address - " + TargetEndPointAddress + System.Environment.NewLine +
                         "Target Type - " + TargetType + System.Environment.NewLine +
                         "Target Scopes - " + TargetScopes + System.Environment.NewLine +                         
                         "Target MD version - " + TargetMDversion + System.Environment.NewLine +
                         "Target Addresses - " + AddressString + System.Environment.NewLine +
                         TargetDeviceInfo;
                 




            return deviceInfo;
        }

        private bool SetVideoProfileType(Media.VideoEncoding encoding, Media.VideoEncoderConfiguration config)
        {
            string msgSent, msgRecv;
            Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
            SVEC.ForcePersistence = false;

            SVEC.Configuration = config;
            SVEC.Configuration.Encoding = encoding;


            msgSent = TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(SVEC);

            try
            {

                msgRecv = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, msgSent, Parameters.UserName, Parameters.Password);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        
    }
}
