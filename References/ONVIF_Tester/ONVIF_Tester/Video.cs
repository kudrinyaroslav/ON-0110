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

namespace ONVIF_Tester
{
    public partial class Video : Form
    {

        #region Delegates
        delegate void ControlGenericEvent_CallBack(object sender, EventArgs e);
        delegate void SetTextCallback(string text);
        delegate void ActionCallBack();
        delegate void SetBoolCallBack(bool value);

        delegate bool SetVidStreamCallback(string text);

        delegate bool HTTP_VidPost(ref Onvif.StreamerControl sc, out string results, out string packetData);
        delegate bool HTTP_VidGet(ref Onvif.StreamerControl sc, out string results, out string response);

        delegate bool QueryRTSPOptions(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response);
        delegate bool QueryRTSPDescribe(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response);
        delegate bool QueryRTSPSetup(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response, string connection, string type);
        delegate bool QueryRTSPPlay(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response);
        delegate bool QueryRTSPSetParameter(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response);
        delegate bool QueryRTSPTeardown(ref Onvif.StreamerControl sc, out string results, out Onvif.RtspResponse response);

        delegate bool SetVidStreamCredentials(ref Onvif.StreamerControl sc, string uName, string pWD);

        #endregion

        // video stream viewer
        private Onvif.StreamerControl sc_VideoStream;

        private string Device_URL = "";
        private string Device_IP = "";

        private ONVIF_TestCases.TestMessages TestMessage;
        private ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface;

        private void Video_FormClosing(object sender, FormClosingEventArgs e)
        {
            // handing closing events
            CloseVidStream();
        }

        private Media.Profile[] Profiles;

        public bool VideoFormRunning = false;

        /// <summary>
        /// Stop the video stream tool and cleanup
        /// </summary>
        private void CloseVidStream()
        {
            if (this.InvokeRequired)
            {
                ActionCallBack d = new ActionCallBack(CloseVidStream);
                this.Invoke(d);
            }
            else
            {
                try
                {
                    sc_VideoStream.Pause();
                    sc_VideoStream.Dispose();
                }
                catch { }
            }
        }

        /// <summary>
        /// Pulblic call to close the form
        /// </summary>
        public void CloseVideoWindow()
        {
            ActionCallBack d = new ActionCallBack(Form_Close);
            this.Invoke(d);
        }

        /// <summary>
        /// Call Form.Close()
        /// </summary>
        private void Form_Close()
        {
            this.Close();
        }

        /// <summary>
        /// Public function to setup Video Form
        /// </summary>
        public Video()
        {      
            // initilize the form
            InitializeComponent();
            // setup the video streaming control
            SetupVideoWindow();
            // setup window objects
            SetupWindowObjects();

        }

        /// <summary>
        /// Ths function allows the running thread to setup the video display and perform
        /// the neccissary form size adjustments
        /// </summary>
        private void SetupVideoWindow()
        {
            initVideoDisplayMode(false);

            sc_VideoStream = new Onvif.StreamerControl();

            this.sc_VideoStream.BackColor = System.Drawing.Color.Black;
            this.sc_VideoStream.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.sc_VideoStream.Location = new System.Drawing.Point(10, 10);
            this.sc_VideoStream.Name = "streamerControl1";
            this.sc_VideoStream.Password = "";
            this.sc_VideoStream.RtpReceivePort = 40000;
            this.sc_VideoStream.RtspTimeout = 5000;
            this.sc_VideoStream.Size = new System.Drawing.Size(390, 314);
            this.sc_VideoStream.StreamUri = "";
            this.sc_VideoStream.TabIndex = 9;
            this.sc_VideoStream.UserName = null;

            HideGroupBox();

            this.Size = new Size(415, 390);
            this.MaximumSize = new Size(415, 390);
            this.MinimumSize = new Size(415, 390);

            this.Controls.Add(this.sc_VideoStream);

        }

        /// <summary>
        /// Initilize/Create form objects
        /// </summary>
        private void SetupWindowObjects()
        {
            TestMessage = new ONVIF_TestCases.TestMessages();
            NetworkInterface = new ONVIF_NetworkInterface.NetworkInterface_Class();

        }

        /// <summary>
        /// This function is used for debugging to display messages on the Video Form
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        public void VideoMessage(string message)
        {
            lbl_videoLabel.Text = message;   
        }

        /// <summary>
        /// Asynchronous function handler for thread communication
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        private void SetMessageText(string text)
        {
            if (lbl_videoLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetMessageText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbl_videoLabel.Text = text;
            }
        }

        /// <summary>
        /// Threadsafe Asychronisis call to hide the VidSimulator Group box
        /// </summary>
        private void HideGroupBox()
        {
            if (gb_VidSimulator.InvokeRequired)
            {
                ActionCallBack d = new ActionCallBack(HideGroupBox);
                this.Invoke(d);
            }
            else
            {
                gb_VidSimulator.Visible = false;
            }
        }
              
        /// <summary>
        /// Public function to set the video URL for testing
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="IP"></param>
        public void SetDeviceURL(string URL, string IP)
        {
            Device_URL = URL;
            Device_IP = IP;
            SetMessageText(IP);
        }

 
        /// <summary>
        /// Public function to call Delagate for video stream init
        /// </summary>
        /// <param name="URI">Stream URI</param>
        /// <returns></returns>
        public bool InitVideoStream(string URI)
        {
            SetVidStreamCallback d = new SetVidStreamCallback(initVideoStream);
            return (bool)this.Invoke(d, new object[] { URI });

        }
        
        /// <summary>
        /// Init video stream
        /// </summary>
        /// <param name="URI">URI of video stream</param>
        /// <returns></returns>
        private bool initVideoStream(string URI)
        {
            return sc_VideoStream.Init(URI);
        }

        /// <summary>
        /// Open video stream using provided URI and play video
        /// </summary>
        /// <param name="URI">Stream URI</param>
        /// <returns>False if exception occurs when trying to play</returns>
        public bool OpenVideoStream(string URI)
        {
            try
            {
                sc_VideoStream.StreamUri = URI;
                sc_VideoStream.Play();
            }
            catch
            {
                return false;
            }

            return true;
        }


        /***********************************************************************************
         * 
         *                      Video Stream Delegates
         *  
         *  The video stream delegates are public functions that give access the the video
         *  stream tools inner workings.  Since the ONVIF specification requires the tool to
         *  be capible of stopping at any point, as well as providing feedback during use 
         *  the orginal design of having the video tool peform all the video tests was not 
         *  going to work.  To get around this problem hooks were added to the video tool to
         *  call the inner functions directly and to return Media Profile information.  This 
         *  change allowed the tool to provide feedback at each step and allows the user to 
         *  terminate the test at any time.
         *  
         *  Each of the functions are relitivly self describing and call one function within
         *  the OnvifStreamerControl.
         * 
         * 
         * 
         * *********************************************************************************/
        #region Video Stream Delegates

        public bool Query_RTSP_Options(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPOptions QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_Options(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool Query_RTSP_Describe(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPDescribe QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_Describe(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool Query_RTSP_Setup(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPOptions QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_Setup(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool Query_RTSP_Play(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPPlay QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_Play(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool Query_RTSP_SetParameter(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPSetParameter QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_SetParameter(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool Query_RTSP_Teardown(out string results, out Onvif.RtspResponse response)
        {

            QueryRTSPTeardown QrtspOpt = delegate(ref Onvif.StreamerControl sc, out string res, out Onvif.RtspResponse resp)
            {
                return sc.RTSP_Teardown(out res, out resp);
            };

            return QrtspOpt.Invoke(ref sc_VideoStream, out results, out response);
        }

        public bool HTTP_Send_POST(out string results, out string packetData)
        {
            HTTP_VidPost HTTPpost = delegate(ref Onvif.StreamerControl sc, out string res, out string packet)
            {
                return sc.HTTP_Post(out res, out packet);
            };

            return HTTPpost.Invoke(ref sc_VideoStream, out results, out packetData);
        }

        public bool HTTP_Request_GET(out string results, out string response)
        {
            HTTP_VidGet HTTPget = delegate(ref Onvif.StreamerControl sc, out string res, out string resp)
            {
                return sc.HTTP_Get(out res, out resp);
            };

            return HTTPget.Invoke(ref sc_VideoStream, out results, out response);
        }


        public bool Video_SetCredentials(string userName, string password)
        {
            SetVidStreamCredentials VidCredentials = delegate(ref Onvif.StreamerControl sc, string uName, string pwd)
            {
                try
                {
                    sc.UserName = uName;
                    sc.Password = pwd;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            };
            return VidCredentials.Invoke(ref sc_VideoStream, userName, password);

        }

        #endregion


        #region TEST FUNCTIONS

        // these functions were for testing and early development.  They are no longer in use
        // and are kept for reference only.  Users do not have access to the hidden controls 
        // these functions use.

        /// <summary>
        /// Public function to call Delagate to set video display mode
        /// </summary>
        /// <param name="test">Test Mode</param>
        public void SetDisplayMode_Test(bool test)
        {
            SetBoolCallBack d = new SetBoolCallBack(initVideoDisplayMode);
            this.Invoke(d, new object[] { test });

        }
 
        /// <summary>
        /// Set video form mode display
        /// </summary>
        /// <param name="TestMode">Test mode</param>
        private void initVideoDisplayMode(bool TestMode)
        {
            //  This function was originally designed to allow the calling form to set
            //  enable test mode.  This allows the tester to see more buttons and 
            //  features normally hidden.  I found it was no longer needed after a while
            //  and removed the functionality

            if (TestMode)
            {
                this.Size = new Size(415, 390);
                this.MaximumSize = new Size(415, 390);
                this.MinimumSize = new Size(415, 390);

            }
            else
            {
                this.Size = new Size(415, 390);
                this.MaximumSize = new Size(415, 390);
                this.MinimumSize = new Size(415, 390);

                //this.Size = new Size(800, 390);
                //this.MaximumSize = new Size(800, 390);
                //this.MinimumSize = new Size(800, 390);
            }

            this.Refresh();
        }
            

        private void FillProfileReport(Media.Profile[] ProfilesFound)
        {
            if (ProfilesFound != null)
            {
                

                foreach (Media.Profile Profile in ProfilesFound)
                {
                    lb_DeviceProfiles.Items.Add("Profile " + Profile.Name + " - Token = " + Profile.token);
                }
                lb_DeviceProfiles.Refresh();
            }


        }

        private void Video_Load(object sender, EventArgs e)
        {            
            VideoFormRunning = true;
        }
   
        //private void btn_GetVidStream_Click(object sender, EventArgs e)
        //{
        //    string msg, returnString;
        //    string btnString = btn_GetVidStream.Text;

        //    // send the Get Profiles Request
        //    btn_GetVidStream.Text = "Please Wait";
        //    btn_GetVidStream.Refresh();

        //    lb_DeviceProfiles.Items.Clear();
        //    Profiles = null;

        //    msg = TestMessage.Build_GetProfilesRequest();

        //    returnString = NetworkInterface.POST_Message(Device_URL, msg, Parameters);

        //    try
        //    {               

        //        Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));

        //        if (GPsR.Profiles != null)
        //        {
        //            Profiles = GPsR.Profiles;
        //            FillProfileReport(GPsR.Profiles);
        //            // auto select the first stream
        //            lb_DeviceProfiles.SelectedIndex = 0;
        //        }

        //    }
        //    catch (Exception error)
        //    {
        //        lb_DeviceProfiles.Items.Add(error.Message);
        //    }

        //    btn_GetVidStream.Text = btnString;
        //}

        //private void lb_DeviceProfiles_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DialogResult aDialog;
        //    Media.StreamSetup streamSetup;
        //    string msg, returnString, errorMessage = "";
        //    string URI, soapFault;

        //    // get the URI for the selected profile
        //    if (Profiles == null)
        //        return;

        //    if(lb_DeviceProfiles.SelectedIndex > Profiles.Length)
        //        return;

        //    aDialog = MessageBox.Show("Do you wish to view profile \"" + Profiles[lb_DeviceProfiles.SelectedIndex].Name + "\"", "Connect", MessageBoxButtons.OKCancel);

        //    if (aDialog == DialogResult.OK)
        //    {
        //        streamSetup = new Media.StreamSetup();
        //        streamSetup.Stream = new Media.StreamType();
        //        streamSetup.Transport = new Media.Transport();

        //        msg = TestMessage.Build_GetStreamUriRequest(Profiles[lb_DeviceProfiles.SelectedIndex].token, streamSetup);


        //        try
        //        {
        //            returnString = NetworkInterface.POST_Message(Device_URL, msg, Parameters.UserName, Parameters.Password);

        //            if (TestMessage.Check_SoapFault(returnString, out soapFault))
        //            {
        //                MessageBox.Show("SOAP Fault, unable to connect." + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
        //                return;
        //            }

        //            Media.GetStreamUriResponse GSR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetStreamUriResponse));

        //            URI = GSR.MediaUri.Uri;

        //            if (URI.Contains("&line=0"))
        //                URI = URI.Replace("&line=0", "");

        //            OpenVideoStream(URI);

        //        }
        //        catch (Exception err)
        //        {
        //            errorMessage = err.Message;
                    
        //        }

        //        if (errorMessage != "")
        //            MessageBox.Show("Unable to connect - " + errorMessage, "Error", MessageBoxButtons.OK);

        //    }


        //}

        
        #endregion




    }
}
