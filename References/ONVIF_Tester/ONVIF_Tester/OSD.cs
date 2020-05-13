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
using ONVIF_TestSummary;
using Microsoft.Win32;
using System.Threading;
using System.IO;
using System.Collections;
using System.Net;
using Microsoft.SqlServer.MessageBox;


namespace ONVIF_Tester
{
    /// <summary>
    /// OSD Form class, main windows form for the ONVIF Compliance Tester
    /// </summary>
    public partial class OSD : Form
    {
        

        // this is the form version information.  This needs to be updated as the form is changed
        // and new releases are made.

        private const string ONVIF_Spec_MajorVersion = "1"; // # of cherictors based on ONVIF version
        private const string ONVIF_Spec_MinorVersion = "01"; // # of cherictors based on ONVIF version

        private const string Test_Tool_MajorVersion = "1";  // 1 charictor
        private const string Test_Tool_MinorVersion = "26"; // 2 charictors

        private const bool DISPLAY_USER_PWD = false;

        #region Constents

        private const string ONVIF_SpecVersion = ONVIF_Spec_MajorVersion + "." + ONVIF_Spec_MinorVersion;
        private const string Version = Test_Tool_MajorVersion + "." + Test_Tool_MinorVersion;
        private const string BuildDate = "April 4, 2009";

        private const string ToolVersion = "ONVIF Test Tool version " + ONVIF_SpecVersion + "." + Version;
        private const string TestVersion = "ONVIF Test Specification version 1.01, September 2009";
        private const string CoreVersion = "ONVIF Core Specification version 1.01, July 2009";

        private const string About_Text2 = "ONVIF Conformance Test Tool v" + ONVIF_SpecVersion + "." + Version +
                                            "\n" +
                                            "\n" +
                                            "(c) 2009 Open Network Video Interface Forum" +
                                            "\n" +
                                            "\n" +
                                            "NOT FOR REDISTRIBUTION";

        private const string About_Text = "ONVIF Conformance Test Tool v" + ONVIF_SpecVersion + "." + Version +
                                            "\n" +
                                            "\n" +
                                            "(c) 2009 Open Network Video Interface Forum" +
                                            "\n" +
                                            "www.onvif.org" +
                                            "\n" +
                                            "\n" +
                                            "\n" +
                                            "Developed by NetVideo Consulting, Inc." +
                                            "\n" +
                                            "www.netvideoconsulting.com" +
                                            "\n" +
                                            "\n" +
                                            "\n" +
                                            "This computer program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program is prohibited.\n" +
                                            "\n" +
                                            "\n" +
                                            "                                                                                                                                      ONVIF LICENSE AGREEMENT\n" +
                                            "\n" +
                                            "This is a legal agreement between you (either individual or an entity) and Open Network Video Interface Forum (hereinafter referred to as \"ONVIF\"). By installing, copying or otherwise using the SOFTWARE, you are agreeing to be bound by the terms of this agreement.\n" +
                                            "\n" +
                                            "ONVIF SOFTWARE LICENSE\n" +
                                            "1. GRANT OF LICENSE. ONVIF grants to you as ONVIF Member the right to use the SOFTWARE. The SOFTWARE is in \"use\" on a computer when it is loaded into temporary memory (i.e. RAM) or installed into permanent memory (e.g. hard disk, CD-ROM or other storage device) of that computer.\n" +
                                            "\n" +
                                            "2. COPYRIGHT. The SOFTWARE is owned by ONVIF and/or its licensor(s), if any, and is protected by copyright laws and international treaty provisions. Therefore you must treat the SOFTWARE like any other copyrighted material (e.g. a book or a musical recording) except that you may either (a) make a copy of the SOFTWARE solely for backup or archival purposes or (b) transfer the SOFTWARE to a single hard disk provided you keep the original solely for backup purposes.\n" +
                                            "\n" +
                                            "3. OTHER RESTRICTIONS. You may not rent, lease or sublicense the SOFTWARE. You may not reverse engineer, decompile, or disassemble the SOFTWARE.\n" +
                                            "\n" +
                                            "4. THIRD PARTY Software. The SOFTWARE may contain third party software, which requires notices and/or additional terms and conditions. Such required third party software notices and/or additional terms and conditions are located in the readme file or other product documentation. By accepting this license agreement, you are also accepting the additional terms and conditions, if any, set forth therein.\n" +
                                            "\n" +
                                            "5. TERMINATION. This License is effective until terminated. Your rights under this License will terminate automatically without notice from ONVIF if you fail to comply with any term(s) of this License. Upon the termination of this License, you shall cease all use of the SOFTWARE and destroy all copies, full or partial, of the SOFTWARE.\n" +
                                            "\n" +
                                            "6. GOVERNING LAW. This agreement shall be deemed performed in and shall be construed by the laws of Switzerland.\n" +
                                            "\n" +
                                            "DISCLAIMER \n" +
                                            "THE SOFTWARE IS DELIVERED AS IS WITHOUT WARRANTY OF ANY KIND. THE ENTIRE RISK AS TO THE RESULTS AND PERFORMANCE OF THE SOFTWARE IS ASSUMED BY THE PURCHASER/THE USER/YOU. ONVIF DISCLAIMS ALL WARRANTIES, WHETHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT, OR ANY WARRANTY ARISING OUT OF ANY PROPOSAL, SPECIFICATION OR SAMPLE WITH RESPECT TO THE SOFTWARE.\n" +
                                            "\n" +
                                            "ONVIF AND/OR ITS LICENSOR(S) SHALL NOT BE LIABLE FOR LOSS OF DATA, LOSS OF PRODUCTION, LOSS OF PROFIT, LOSS OF USE, LOSS OF CONTRACTS OR FOR ANY OTHER CONSEQUENTIAL, ECONOMIC OR INDIRECT LOSS WHATSOEVER IN RESPECT OF SALE, PURCHASE, DELIVERY, USE OR DISPOSITION OF THE SOFTWARE.\n" +
                                            "\n" +
                                            "ONVIF TOTAL LIABILITY FOR ALL CLAIMS IN ACCORDANCE WITH THE SALE, PURCHASE, DELIVERY AND USE OF THE SOFTWARE SHALL NOT EXCEED THE PRICE PAID FOR THE SOFTWARE.\n" +
                                            "\n";
        
        private const int MAX_TEST_INDEX = 20;

        private const string TEST_SELECTION = "Test selection";

        private const string SELECTED_INDEX = "_SelectedIndex";

        private const string BTN_START_DEFAULT = "Start";
        private const string BTN_START_RUNNING = "Running";

        private const int formWidth = 910;
        private const int formHeight = 520;

        

        #endregion

        #region structures

        public struct DiscoveredDevices_Type
        {
            public RemoteDiscovery.ProbeMatchesType MatchesType;
            public string IP;
        }

        #endregion

        #region Variables

        private int testIndex = 0;        
        private ONVIF_TestCases.TestCases_Class.TestGroup_Type Tests;
        private ONVIF_TestCases.TestCases_Class.TestParameters_Type Parameters = new TestCases_Class.TestParameters_Type();
        
        
        // Control flags, used to keep unwanted messages from appearing as the form is setup
        // or changed during startup or runtime.
        private bool testsReady = false;
        private bool initilizationComplete = false;
        private bool TargetAddressTextChange = false;

        // testRunner thread varialbes.  Tese are used for starting and stopping the 
        // test thread allowing the main form thread to respond to user clicks and actions
        Thread testRunner;
        static object locker = new object();
        private bool testRunner_Quit = false;
        private bool runTest = false;
        private bool runSingleStep = false;
        private bool runSingleTest = false;
        private bool reRunTests = false;
        private int thread_Sleep = 200; //  sleep time between tests

        static int TR_GroupCount = 0, TR_TestCount = 0;

        private string currentTestTitle = "";

        // video form thread.  This thread will be responsible for opening and handling
        // the video form.  Having the video in a seperate form allows the users to move
        // and arange the forms as needed and keeps the test form small enough to work
        // on low resolution monitors.
        Thread videoForm;
        
        // local class implementations.
        ONVIF_TestCases.TestCases_Class TestCases = new ONVIF_TestCases.TestCases_Class();
        ONVIF_TestSummary.TestSummary TestSummary = new TestSummary();
        ONVIF_TestCases.TestMessages TestMessage = new TestMessages();
        ONVIF_NetworkInterface.NetworkInterface_Class NetworkInterface = new ONVIF_NetworkInterface.NetworkInterface_Class();

        // The Registry Key tool is used to save and retreive settings, allowing the form
        // to remember what the users enter.
        private RegistryKey settingsKey = null;

        // Call back delegate.  This is the c style function pointer used by the threads
        // for communication between threads and the main form
        delegate void SetTextCallback(string text, bool append);
        delegate void SetTestTimer_CallBack(int time);

        delegate void SetIntegerValue_CallBack(int value);
        delegate void SetBoolValue_CallBack(bool value);

        delegate void TestScrollBarUpdate_CallBack(object sender, EventArgs e);


        delegate Form GetVideoForm_CallBack();

        // the video form
        public Video Vid = null;

        DiscoveredDevices_Type[] DiscoveredDevices = new DiscoveredDevices_Type[1];
        //RemoteDiscovery.ProbeMatchesType[] DiscoveredDevices = new RemoteDiscovery.ProbeMatchesType[1];
        int DiscoveredDevices_Count = 0;

        //private string SystemBusyString = "";
        //private int SystemBusyPulseCount = 0;

        private System.EventHandler LastDeviceTabSendEvent = null;

        private Media.Profile[] VideoProfiles;

        #endregion

        #region Form Initilization Code

        /// <summary>
        /// OSD Form initilization function
        /// </summary>
        public OSD()
        {
            InitializeComponent();

            InitilizeFormLayout();
            InitilizeTestParameters();
            
            InitilizeVideoForm();

            initilizationComplete = true;

           
        }
        
        private void OSD_Shown(object sender, EventArgs e)
        {
            System.DateTime currentTime = System.DateTime.Now;
            // put up the splash screen
            DisplaySplashImage();

            TestMessage.InitSchemaCollection();
            TestCases.InitTester();

            InitilizeTestRunner();

            // take down the splash screen
            while(System.DateTime.Now < (currentTime.AddSeconds(4)))
                Thread.Sleep(100);

            HideSplashImage();

            if ((this.Size.Width < formWidth) || (this.Size.Height < formHeight))
            {
                this.AutoScroll = true;
            }

            this.AutoSize = true;
        }

        private void DisplaySplashImage()
        {
            pnl_SplashPanel.MinimumSize = new Size(formWidth, formHeight);
            pnl_SplashPanel.MaximumSize = new Size(formWidth, formHeight);
            pnl_SplashPanel.Size = new Size(formWidth, formHeight);

            pnl_SplashPanel.Location = new Point(0, 0);
            pnl_SplashPanel.Update();

        }

        private void HideSplashImage()
        {
            pnl_SplashPanel.Location = new Point(0, formHeight + 20);
            pnl_SplashPanel.Visible = false;

        }

        /// <summary>
        /// Initialize Form controls with any saved user settings
        /// </summary>
        private void InitilizeTestParameters()
        {
            string tmpString;

            tb_ONVIF_ToolVersion.Text = ToolVersion;
            tb_ONVIF_TestSpec.Text = TestVersion;
            tb_ONVIF_CoreSpec.Text = CoreVersion;
            
            
            // user settings are stored in the registery.  Try to read them out and fill out any 
            // data they've entered

            // Attempt to open the key
            settingsKey = Registry.CurrentUser.OpenSubKey("Software\\ONVIF\\TestApp", true);

            // If the return value is null, the key doesn't exist
            if (settingsKey == null) // The key doesn't exist; create it / open it
                settingsKey = Registry.CurrentUser.CreateSubKey("Software\\ONVIF\\TestApp");

            settingsKey.Close();
                
            // see if the user has entered any fields that have been remembered
            tb_TestDevice_Brand.Text = GetParameter(tb_TestDevice_Brand.Name);
            tb_TestDevice_Model.Text = GetParameter(tb_TestDevice_Model.Name);
            tb_TestDevice_SerialNumber.Text = GetParameter(tb_TestDevice_SerialNumber.Name);
            tb_TestDevice_FWversion.Text = GetParameter(tb_TestDevice_FWversion.Name);
            tb_TestDevice_Other.Text = GetParameter(tb_TestDevice_Other.Name);
            tb_Test_Operator.Text = GetParameter(tb_Test_Operator.Name);
            tb_Test_OrganizationName.Text = GetParameter(tb_Test_OrganizationName.Name);
            tb_Test_OrganizationAddress.Text = GetParameter(tb_Test_OrganizationAddress.Name);

            tb_MessageTimeout.Text = GetParameter(tb_MessageTimeout.Name);
            if (tb_MessageTimeout.Text == "")
                tb_MessageTimeout.Text = ONVIF_TestCases.TestCases_Class.DEFAUTLT_TIMEOUT.ToString();

            tb_RebootTime.Text = GetParameter(tb_RebootTime.Name);
            if (tb_RebootTime.Text == "")
                tb_RebootTime.Text = ONVIF_TestCases.TestCases_Class.DEFAULT_REBOOT_TIME.ToString();

            if (DISPLAY_USER_PWD)
            {
                tb_UserName.Text = GetParameter(tb_UserName.Name);
                tb_Password.Text = GetParameter(tb_Password.Name);
            }
            else
            {
                lbl_Password.Visible = false;
                lbl_UserName.Visible = false;
                tb_UserName.Visible = false;
                tb_Password.Visible = false;

                tb_UserName.Text = "";
                tb_Password.Text = "";
            }

            // check to see what test suite the user loaded last
            tmpString = GetParameter(TEST_SELECTION);
            if (tmpString.Equals("") || tmpString.Equals(rb_UseEmbedded_Tests.Name))
                rb_UseEmbedded_Tests.Checked = true;    // default
            else if (tmpString.Equals(rb_UseUserXML_Tests.Name))
                rb_UseUserXML_Tests.Checked = true;
                
            // test file
            tb_TestCaseXMLFile.Text = GetParameter(tb_TestCaseXMLFile.Name);

            // test report file
            tb_TestReportFile.Text = GetParameter(tb_TestReportFile.Name);

            tb_SoapOutput.Text = GetParameter(tb_SoapOutput.Name);
            tb_wsdl_file_test.Text = GetParameter(tb_wsdl_file_test.Name);

            // auto save
            //if(!GetParameter(toolStrip_AutoSave.Name).Equals(""))
            //    toolStrip_AutoSave.Checked = bool.Parse(GetParameter(toolStrip_AutoSave.Name));

            // test parameters         
            tb_TargetMDversion.Text = GetParameter(tb_TargetMDversion.Name);
            tb_TargetEndPointAddress.Text = GetParameter(tb_TargetEndPointAddress.Name);
            tb_TargetType.Text = GetParameter(tb_TargetType.Name);
            tb_TargetScopes.Text = GetParameter(tb_TargetScopes.Name);
            tb_TargetIPAddress.Text = GetParameter(tb_TargetIPAddress.Name); // target IP

            string addressValues = GetParameter(cb_TargetAddress.Name);
            if ((addressValues != null) && (addressValues != ""))
            {
                string[] urls = addressValues.Split(new char[] { ' ' });
                foreach (string url in urls)
                {
                    if ((url != "") && (url != " "))
                        cb_TargetAddress.Items.Add(url);
                }

                if (cb_TargetAddress.Items.Count > 0)
                    cb_TargetAddress.SelectedIndex = 0;
            }

            string selectedIndex = GetParameter(cb_TargetAddress.Name + SELECTED_INDEX);
            if (selectedIndex != "")
            {
                try
                {
                    int index = int.Parse(selectedIndex);
                    if (index < cb_TargetAddress.Items.Count)
                        cb_TargetAddress.SelectedIndex = index;
                }
                catch 
                { 
                    // leave the index at zero
                }
            }

            UpdateTestParameters(false);

            // load tests if avalible
            if (rb_UseEmbedded_Tests.Checked)
            {
                Load_ONVIF_ComplianceTest();
            }
            else if (rb_UseUserXML_Tests.Checked)
            {
                Load_USER_SpecifiedTests();
            }


            tb_StreamURI.Text = "rtsp://192.168.1.160/rtsp_tunnel?h26x=0&Profile=0&video_on=0";
        }

        /// <summary>
        /// Performs Form control layout initialization tasks
        /// </summary>
        private void InitilizeFormLayout()
        {
            int xPos;
            Size aSize = new Size(0, lbl_Test_1.Height);


            this.MaximumSize = new Size(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);

            //this.MaximumSize = new Size(this.MaximumSize.Width, formHeight);
            //this.MaximumSize = new Size(formWidth, formHeight);   
            this.MinimumSize = new Size(formWidth, formHeight);
            this.Size = new Size(formWidth, formHeight);

            this.AutoSize = false;
            
            
            // set their location since it is difficult to do in the UI
            lbl_Test_1.Location = new Point(9, 9 + (17 * 0));
            lbl_Test_2.Location = new Point(9, 9 + (17 * 1));
            lbl_Test_3.Location = new Point(9, 9 + (17 * 2));
            lbl_Test_4.Location = new Point(9, 9 + (17 * 3));
            lbl_Test_5.Location = new Point(9, 9 + (17 * 4));
            lbl_Test_6.Location = new Point(9, 9 + (17 * 5));
            lbl_Test_7.Location = new Point(9, 9 + (17 * 6));
            lbl_Test_8.Location = new Point(9, 9 + (17 * 7));
            lbl_Test_9.Location = new Point(9, 9 + (17 * 8));
            lbl_Test_10.Location = new Point(9, 9 + (17 * 9));
            lbl_Test_11.Location = new Point(9, 9 + (17 * 10));
            lbl_Test_12.Location = new Point(9, 9 + (17 * 11));
            lbl_Test_13.Location = new Point(9, 9 + (17 * 12));
            lbl_Test_14.Location = new Point(9, 9 + (17 * 13));
            lbl_Test_15.Location = new Point(9, 9 + (17 * 14));
            lbl_Test_16.Location = new Point(9, 9 + (17 * 15));
            lbl_Test_17.Location = new Point(9, 9 + (17 * 16));
            lbl_Test_18.Location = new Point(9, 9 + (17 * 17));
            lbl_Test_19.Location = new Point(9, 9 + (17 * 18));
            lbl_Test_20.Location = new Point(9, 9 + (17 * 19));

            xPos = pnl_TestCases.Width - (lbl_testResult_1.Width + 2);

            lbl_testResult_1.Location = new Point(xPos, 9 + (17 * 0));
            lbl_testResult_2.Location = new Point(xPos, 9 + (17 * 1));
            lbl_testResult_3.Location = new Point(xPos, 9 + (17 * 2));
            lbl_testResult_4.Location = new Point(xPos, 9 + (17 * 3));
            lbl_testResult_5.Location = new Point(xPos, 9 + (17 * 4));
            lbl_testResult_6.Location = new Point(xPos, 9 + (17 * 5));
            lbl_testResult_7.Location = new Point(xPos, 9 + (17 * 6));
            lbl_testResult_8.Location = new Point(xPos, 9 + (17 * 7));
            lbl_testResult_9.Location = new Point(xPos, 9 + (17 * 8));
            lbl_testResult_10.Location = new Point(xPos, 9 + (17 * 9));
            lbl_testResult_11.Location = new Point(xPos, 9 + (17 * 10));
            lbl_testResult_12.Location = new Point(xPos, 9 + (17 * 11));
            lbl_testResult_13.Location = new Point(xPos, 9 + (17 * 12));
            lbl_testResult_14.Location = new Point(xPos, 9 + (17 * 13));
            lbl_testResult_15.Location = new Point(xPos, 9 + (17 * 14));
            lbl_testResult_16.Location = new Point(xPos, 9 + (17 * 15));
            lbl_testResult_17.Location = new Point(xPos, 9 + (17 * 16));
            lbl_testResult_18.Location = new Point(xPos, 9 + (17 * 17));
            lbl_testResult_19.Location = new Point(xPos, 9 + (17 * 18));
            lbl_testResult_20.Location = new Point(xPos, 9 + (17 * 19));

            aSize.Width = lbl_testResult_1.Location.X - lbl_Test_1.Location.X;

            // Make sure all the test labels have the same maximum size.  This has to be
            // done to keep the labels from overwriting the rest of the form.
            lbl_Test_1.MaximumSize = aSize;
            lbl_Test_2.MaximumSize = aSize;
            lbl_Test_3.MaximumSize = aSize;
            lbl_Test_4.MaximumSize = aSize;
            lbl_Test_5.MaximumSize = aSize;
            lbl_Test_6.MaximumSize = aSize;
            lbl_Test_7.MaximumSize = aSize;
            lbl_Test_8.MaximumSize = aSize;
            lbl_Test_9.MaximumSize = aSize;
            lbl_Test_10.MaximumSize = aSize;
            lbl_Test_11.MaximumSize = aSize;
            lbl_Test_12.MaximumSize = aSize;
            lbl_Test_13.MaximumSize = aSize;
            lbl_Test_14.MaximumSize = aSize;
            lbl_Test_15.MaximumSize = aSize;
            lbl_Test_16.MaximumSize = aSize;
            lbl_Test_17.MaximumSize = aSize;
            lbl_Test_18.MaximumSize = aSize;
            lbl_Test_19.MaximumSize = aSize;
            lbl_Test_20.MaximumSize = aSize;

            // clear any test text
            clearTestLines();

            // since the tests have not been loaded initilize the vertical 
            // scroll bar to keep the user from scrolling
            vsb_TestScroller.Minimum = 0;
            vsb_TestScroller.LargeChange = 1;
            vsb_TestScroller.SmallChange = 1;
            vsb_TestScroller.Maximum = 0;            

            // if this is not in debug mode remove the debug tab
#if !DEBUG
                TabControl.TabPages.Remove(tab_InternalTesting);
                
#endif

            tb_TargetEndPointAddress.ReadOnly = true;
            tb_TargetType.ReadOnly = true;
            tb_TargetScopes.ReadOnly = true;
            tb_TargetMDversion.ReadOnly = true;


            FloatingDeviceParameters_Clear();
            HideRequiredTestInfoFieldInidicators();

            cb_VideoStreams.Visible = false;

            lbl_EmbeddedTestVersion.Text += " " + ONVIF_Spec_MajorVersion + "." + ONVIF_Spec_MinorVersion;
        }

        /// <summary>
        /// Thread TestRunner initialization
        /// </summary>
        private void InitilizeTestRunner()
        {

            testRunner = new Thread(TestRunner);
            testRunner.Name = "Test Runner";               
            testRunner.Start();
            TestTimer.Interval = 100;
            TestTimer.Enabled = true;
        }

        /// <summary>
        /// Thread VideoForm initialization
        /// </summary>
        private void InitilizeVideoForm()
        {
            videoForm = new Thread(VideoForm);            

        }

       

        /// <summary>
        /// Handle OSD form closing events and recall/kill any running threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OSD_FormClosing(object sender, FormClosingEventArgs e)
        {
            // do anything needed to close the form here
            testRunner_Quit = true;
            try
            {
                testRunner.Join(100);
            }
            catch (Exception expt)
            {
                Console.WriteLine(expt.Message);
            }

            // depending on if the video window has been opened, is open or
            // has been closed the thread will be in different states.  If 
            // it hasn't been opened it is no big deal, but if it has we need
            // to clean up
            if (videoForm.ThreadState == ThreadState.Running)
                videoForm.Abort();

            if (videoForm.ThreadState == ThreadState.Stopped)
                videoForm.Join(100);

        }

        private void Video_FormClosed(int value)
        {
            cb_VideoStreams.Visible = false;
        }

        /// <summary>
        /// Clear Test tab tests fields
        /// </summary>
        private void clearTestLines()
        {
            lbl_Test_1.Text = "";
            lbl_Test_2.Text = "";
            lbl_Test_3.Text = "";
            lbl_Test_4.Text = "";
            lbl_Test_5.Text = "";
            lbl_Test_6.Text = "";
            lbl_Test_7.Text = "";
            lbl_Test_8.Text = "";
            lbl_Test_9.Text = "";
            lbl_Test_10.Text = "";
            lbl_Test_11.Text = "";
            lbl_Test_12.Text = "";
            lbl_Test_13.Text = "";
            lbl_Test_14.Text = "";
            lbl_Test_15.Text = "";
            lbl_Test_16.Text = "";
            lbl_Test_17.Text = "";
            lbl_Test_18.Text = "";
            lbl_Test_19.Text = "";
            lbl_Test_20.Text = "";

            lbl_testResult_1.Text = "";
            lbl_testResult_2.Text = "";
            lbl_testResult_3.Text = "";
            lbl_testResult_4.Text = "";
            lbl_testResult_5.Text = "";
            lbl_testResult_6.Text = "";
            lbl_testResult_7.Text = "";
            lbl_testResult_8.Text = "";
            lbl_testResult_9.Text = "";
            lbl_testResult_10.Text = "";
            lbl_testResult_11.Text = "";
            lbl_testResult_12.Text = "";
            lbl_testResult_13.Text = "";
            lbl_testResult_14.Text = "";
            lbl_testResult_15.Text = "";
            lbl_testResult_16.Text = "";
            lbl_testResult_17.Text = "";
            lbl_testResult_18.Text = "";
            lbl_testResult_19.Text = "";
            lbl_testResult_20.Text = "";

            lbl_testResult_1.BorderStyle = BorderStyle.None;
            lbl_testResult_2.BorderStyle = BorderStyle.None;
            lbl_testResult_3.BorderStyle = BorderStyle.None;
            lbl_testResult_4.BorderStyle = BorderStyle.None;
            lbl_testResult_5.BorderStyle = BorderStyle.None;
            lbl_testResult_6.BorderStyle = BorderStyle.None;
            lbl_testResult_7.BorderStyle = BorderStyle.None;
            lbl_testResult_8.BorderStyle = BorderStyle.None;
            lbl_testResult_9.BorderStyle = BorderStyle.None;
            lbl_testResult_10.BorderStyle = BorderStyle.None;
            lbl_testResult_11.BorderStyle = BorderStyle.None;
            lbl_testResult_12.BorderStyle = BorderStyle.None;
            lbl_testResult_13.BorderStyle = BorderStyle.None;
            lbl_testResult_14.BorderStyle = BorderStyle.None;
            lbl_testResult_15.BorderStyle = BorderStyle.None;
            lbl_testResult_16.BorderStyle = BorderStyle.None;
            lbl_testResult_17.BorderStyle = BorderStyle.None;
            lbl_testResult_18.BorderStyle = BorderStyle.None;
            lbl_testResult_19.BorderStyle = BorderStyle.None;
            lbl_testResult_20.BorderStyle = BorderStyle.None;

            toolTip_Tests.RemoveAll();
            toolTip_Results.RemoveAll();
        }

        private void clearTestLine(int line)
        {
            Label testLabel, testResultLabel;

            switch (line)
            {                
                case 0:
                    testLabel = lbl_Test_1;
                    testResultLabel = lbl_testResult_1;
                    break;
                case 1:
                    testLabel = lbl_Test_2;
                    testResultLabel = lbl_testResult_2;
                    break;
                case 2:
                    testLabel = lbl_Test_3;
                    testResultLabel = lbl_testResult_3;
                    break;
                case 3:
                    testLabel = lbl_Test_4;
                    testResultLabel = lbl_testResult_4;
                    break;
                case 4:
                    testLabel = lbl_Test_5;
                    testResultLabel = lbl_testResult_5;
                    break;
                case 5:
                    testLabel = lbl_Test_6;
                    testResultLabel = lbl_testResult_6;
                    break;
                case 6:
                    testLabel = lbl_Test_7;
                    testResultLabel = lbl_testResult_7;
                    break;
                case 7:
                    testLabel = lbl_Test_8;
                    testResultLabel = lbl_testResult_8;
                    break;
                case 8:
                    testLabel = lbl_Test_9;
                    testResultLabel = lbl_testResult_9;
                    break;
                case 9:
                    testLabel = lbl_Test_10;
                    testResultLabel = lbl_testResult_10;
                    break;
                case 10:
                    testLabel = lbl_Test_11;
                    testResultLabel = lbl_testResult_11;
                    break;
                case 11:
                    testLabel = lbl_Test_12;
                    testResultLabel = lbl_testResult_12;
                    break;
                case 12:
                    testLabel = lbl_Test_13;
                    testResultLabel = lbl_testResult_13;
                    break;
                case 13:
                    testLabel = lbl_Test_14;
                    testResultLabel = lbl_testResult_14;
                    break;
                case 14:
                    testLabel = lbl_Test_15;
                    testResultLabel = lbl_testResult_15;
                    break;
                case 15:
                    testLabel = lbl_Test_16;
                    testResultLabel = lbl_testResult_16;
                    break;
                case 16:
                    testLabel = lbl_Test_17;
                    testResultLabel = lbl_testResult_17;
                    break;
                case 17:
                    testLabel = lbl_Test_18;
                    testResultLabel = lbl_testResult_18;
                    break;
                case 18:
                    testLabel = lbl_Test_19;
                    testResultLabel = lbl_testResult_19;
                    break;
                case 19:
                    testLabel = lbl_Test_20;
                    testResultLabel = lbl_testResult_20;
                    break;
                default:
                    return;
            }

            testLabel.Text = "";
            testResultLabel.Text = "";
            testResultLabel.BorderStyle = BorderStyle.None;

            toolTip_Tests.SetToolTip(testLabel, "");
            toolTip_Tests.SetToolTip(testResultLabel, "");

        }
        
        /// <summary>
        /// Save form parameter to the Registry
        /// </summary>
        /// <param name="name">Name of the control</param>
        /// <param name="value">Value to be saved</param>
        private void SaveParameter(string name, string value)
        {
            if (GetParameter(name).Equals(value))
                return;

            try
            {
                settingsKey = Registry.CurrentUser.OpenSubKey("Software\\ONVIF\\TestApp", true);

                if (settingsKey != null)
                {                    
                    settingsKey.SetValue(name, value);
                }

                settingsKey.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
        }

        /// <summary>
        /// Retreive the Form control value saved in the Registry
        /// </summary>
        /// <param name="name">Name of the Form control</param>
        /// <returns>String saved in the registry or an empty string if not found</returns>
        private string GetParameter(string name)
        {
            try
            {
                settingsKey = Registry.CurrentUser.OpenSubKey("Software\\ONVIF\\TestApp", false);

                if (settingsKey.GetValue(name) != null)
                    return (string)settingsKey.GetValue(name);

                settingsKey.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        #endregion

        #region Form Event Handlers


        /// <summary>
        /// Load user selected test suite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_TestSuite(object sender, EventArgs e)
        {
                        
            if (rb_UseEmbedded_Tests.Checked)
            {
                Load_ONVIF_ComplianceTest(); 
            }
            else if (rb_UseUserXML_Tests.Checked)
            {
                Load_USER_SpecifiedTests();
            }
            // check to see if there are tests ready to run, otherwise let the user know
            // there was a problem
            if (testsReady)
            {
                // tests ready to run
                System.Windows.Forms.MessageBox.Show("Tests have been successfully parsed and are ready to run", "Ready", MessageBoxButtons.OK);
                TabControl.SelectedIndex = 2;
            }
            else
            {
                // there was a problem
                System.Windows.Forms.MessageBox.Show("There was an error in resolving the test cases, please verify the test file is correct", "Error", MessageBoxButtons.OK);
            }

        }

        /// <summary>
        /// Tests vertical scroll bar handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vsb_TestScroller_ValueChanged(object sender, EventArgs e)
        {
            // The Test scroll bar on the Tests tab is not tied to a text field.  It is used
            // to trick the user into thinking that they are scrolling through a set of tests
            // when in reality there are labels being updated in such a way as to appear to 
            // scroll.
            if (testsReady)
            {
                // if there are tests scroll
                testIndex = vsb_TestScroller.Value;
                fillTestArray(Tests);
            }
        }

        /// <summary>
        /// Update the test result/action labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testResult_Click(object sender, EventArgs e)
        {
            int x, y;
            string testName = "";

            // if this label has no text don't do anything
            if (((Label)sender).Text.Equals(""))
                return;

            // Remember the test string associated with this label
            if (sender.Equals(lbl_testResult_1))
                testName = lbl_Test_1.Text;

            if (sender.Equals(lbl_testResult_2))
                testName = lbl_Test_2.Text;

            if (sender.Equals(lbl_testResult_3))
                testName = lbl_Test_3.Text;

            if (sender.Equals(lbl_testResult_4))
                testName = lbl_Test_4.Text;

            if (sender.Equals(lbl_testResult_5))
                testName = lbl_Test_5.Text;

            if (sender.Equals(lbl_testResult_6))
                testName = lbl_Test_6.Text;

            if (sender.Equals(lbl_testResult_7))
                testName = lbl_Test_7.Text;

            if (sender.Equals(lbl_testResult_8))
                testName = lbl_Test_8.Text;

            if (sender.Equals(lbl_testResult_9))
                testName = lbl_Test_9.Text;

            if (sender.Equals(lbl_testResult_10))
                testName = lbl_Test_10.Text;

            if (sender.Equals(lbl_testResult_11))
                testName = lbl_Test_11.Text;

            if (sender.Equals(lbl_testResult_12))
                testName = lbl_Test_12.Text;

            if (sender.Equals(lbl_testResult_13))
                testName = lbl_Test_13.Text;

            if (sender.Equals(lbl_testResult_14))
                testName = lbl_Test_14.Text;

            if (sender.Equals(lbl_testResult_15))
                testName = lbl_Test_15.Text;

            if (sender.Equals(lbl_testResult_16))
                testName = lbl_Test_16.Text;

            if (sender.Equals(lbl_testResult_17))
                testName = lbl_Test_17.Text;

            if (sender.Equals(lbl_testResult_18))
                testName = lbl_Test_18.Text;

            if (sender.Equals(lbl_testResult_19))
                testName = lbl_Test_19.Text;

            if (sender.Equals(lbl_testResult_20))
                testName = lbl_Test_20.Text;


            char[] trimChars = new char[] { ' ', '*' };
            testName = testName.TrimStart(trimChars);

            // search through the Tests object and locate the test clicked
            for (x = 0; x < Tests.Group.Length; x++)
            {
                // check to see if this is a test group
                if (Tests.Group[x].Description.Equals(testName))
                {
                    // once found change the test action.  This will change the charictor
                    // in the braces [ ] next to the test, allowing the user to mark a test
                    // as Execute = [x], Skip = [s], Pass = [p], or Fail = [f]
                    Tests.Group[x].NextAction();

                    // update all this tests group's tests to use this action
                    for (y = 0; y < Tests.Group[x].Tests.Length; y++)
                    {
                        Tests.Group[x].Tests[y].Action = Tests.Group[x].Action;
                    }
                    break;
                }

                for (y = 0; y < Tests.Group[x].Tests.Length; y++)
                {
                    if (Tests.Group[x].Tests[y].Name.Equals(testName))
                    {
                        // once found change the test action.  This will change the charictor
                        // in the braces [ ] next to the test, allowing the user to mark a test
                        // as Execute = [x], Skip = [s], Pass = [p], or Fail = [f]
                        Tests.Group[x].Tests[y].NextAction();
                    }
                }

            }

            // refres the test display
            fillTestArray(Tests);
        }

        /// <summary>
        /// Perform test button actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestButtons_Click(object sender, EventArgs e)
        {
            // don't do anything if the tests are not ready
            if (!testsReady)
                return;

            // this function is used by the 4 test buttons, Start, Stop
            // Step and Restart.  Find out which one was clicked and 
            // action accordingly.
            if (sender.Equals(btn_Start))
            {
                
                
                // make sure the form network interface is closed so the test runner's network interface works fine.
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();

                // set the test time
                Tests.SetTestStartTime();

                // Handle start button 
                //tb_TestSummary.Text = "";
                runTest = true;
                
                // restart the test runner
                if (testRunner.ThreadState == ThreadState.Stopped)
                    InitilizeTestRunner();

            }

            if (sender.Equals(btn_Stop))
            {
                // Hhandle stop button
                runTest = false;
                runSingleStep = false;
                runSingleTest = false;

                testRunner.Abort(0);
              
                    
            }

            if (sender.Equals(btn_Step))
            {
                // make sure the form network interface is closed so the test runner's network interface works fine.
                //NetworkInterface.Disconnect();
                NetworkInterface.UDP_Close();

                // set the test time
                Tests.SetTestStartTime();

                // restart the test runner
                if (testRunner.ThreadState == ThreadState.Stopped)
                    InitilizeTestRunner();

                // Handle step button 
                runSingleStep = true;
            }

            if (sender.Equals(btn_Restart))
            {
                Tests.ReRun();
                SetTestIndex(0, 0);
                // Handle restart button
                tb_TestSummary.Text = "";
                tb_Messages.Text = "";                
                reRunTests = true;
                currentTestTitle = "";
                fillTestArray(Tests);

            }



        }

        /// <summary>
        /// Save test summary information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TestSummarySave_Click(object sender, EventArgs e)
        {
            string buttonText = ((System.Windows.Forms.Button)sender).Text;

            ((System.Windows.Forms.Button)sender).Text = "Wait";
            ((System.Windows.Forms.Button)sender).Refresh();

            // build the test report header
            TestSummary.TestSummary_TestInfo_Type TestInfo = new TestSummary.TestSummary_TestInfo_Type();

            TestInfo.ToolVersion = ToolVersion;
            TestInfo.TestVersion = TestVersion;
            TestInfo.CoreVersion = CoreVersion;

            TestInfo.Device_Brand = tb_TestDevice_Brand.Text;
            TestInfo.Device_Model =  tb_TestDevice_Model.Text;
            TestInfo.Device_SerialNumber = tb_TestDevice_SerialNumber.Text;
            TestInfo.Device_FWversion = tb_TestDevice_FWversion.Text;
            TestInfo.Device_Other = tb_TestDevice_Other.Text;

            TestInfo.Operator = tb_Test_Operator.Text;
            TestInfo.OrganizationName = tb_Test_OrganizationName.Text;
            TestInfo.OrganizationAddress = tb_Test_OrganizationAddress.Text;

            TestInfo.TestDateAndTime = Tests.TestTime;

            string testInformation = tb_TestSummary.Text.ToString();

            // Check to see if an output file was specified, if so create a test report
            // otherwise let the user know what they need to do.
            if (tb_TestReportFile.Text.Equals(""))
                System.Windows.Forms.MessageBox.Show("No ouput file specified", "Error", MessageBoxButtons.OK);
            else
            {
                // if this is going to overwrite an existing file tell the user
                if (File.Exists(tb_TestReportFile.Text))
                {
                    DialogResult aDialog = MessageBox.Show("Overwrite existing test report?", "File Exists", MessageBoxButtons.OKCancel);

                    if (aDialog == DialogResult.OK)
                    {
                        // write out the test report
                        TestSummary.WriteTestSummary(tb_TestReportFile.Text, TestInfo, Tests);
                        MessageBox.Show("File saved");
                    }           
                }
                else
                {
                    // write out the test report
                    TestSummary.WriteTestSummary(tb_TestReportFile.Text, TestInfo, Tests);
                    MessageBox.Show("File saved");
                }

            }

            ((System.Windows.Forms.Button)sender).Text = buttonText;

        }

        /// <summary>
        /// Test case XML file box click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_TestCaseXMLFile_Click(object sender, EventArgs e)
        {
            DialogResult aDialog;

            // open the Open File Dialog, Test Case dialog, and allow the user
            // to select the test case file
            aDialog = ofd_TestCase.ShowDialog();

            // if the user cliked "OK" in the dialog box fill the text box with
            // the file URL
            if (aDialog == DialogResult.OK)
            {
                tb_TestCaseXMLFile.Text = ofd_TestCase.FileName;
                SaveParameter(tb_TestCaseXMLFile.Name, tb_TestCaseXMLFile.Text);
            }
        }

        /// <summary>
        /// Tool strip Item Changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStrip_ItemCheckedChanged(object sender, EventArgs e)
        {
            // if we are not done initilizing the page don't do anything
            if (!initilizationComplete)
                return;

            // hande the different tool strip buttons

            // handle the Auto Save checkbox
            //if (sender.Equals(toolStrip_AutoSave))
            //{
            //    if (toolStrip_AutoSave.Checked)
            //    {
            //        System.Windows.Forms.MessageBox.Show("In auto save mode, test results will automatically be saved as <model><date><time>.pdf in the working directory with each test run", "Notice", MessageBoxButtons.OK);
            //    }
            //    // save the parameter
            //    SaveParameter(toolStrip_AutoSave.Name, toolStrip_AutoSave.Checked.ToString());
            //}
        }

        /// <summary>
        /// Test Report File text box click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_TestReportFile_Click(object sender, EventArgs e)
        {
            DialogResult aDialog;
            // Open the Select File Dialog, Test Report, dialog window
            aDialog = sfd_TestReport.ShowDialog();

            // if the user clicked "OK" fill the test report text field with
            // the file URL
            if (aDialog == DialogResult.OK)
            {
                tb_TestReportFile.Text = sfd_TestReport.FileName;
                SaveParameter(tb_TestReportFile.Name, tb_TestReportFile.Text);
                btn_TestSummarySave_Click(sender, e);
            }
        }
   
        /// <summary>
        /// Tool strip item click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // pop up about message
            if (sender.Equals(aboutToolStripMenuItem))
                System.Windows.Forms.MessageBox.Show(About_Text, "About", MessageBoxButtons.OK);

            // pop up the Help window
            if (sender.Equals(howDoIToolStripMenuItem))
                Help.ShowHelp(this, helpProvider1.HelpNamespace);          
            
        }

        /// <summary>
        /// Vidoe button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Video_Click(object sender, EventArgs e)
        {

            string msg, returnString;
            string btnString = btn_Video.Text;

            StartVideoForm();

            // get the streams and fill the combo box

            if ((videoForm.ThreadState == ThreadState.Unstarted) ||
                 (videoForm.ThreadState == ThreadState.Stopped))
                return;

            

            // send the Get Profiles Request
            btn_Video.Text = "Please Wait";
            btn_Video.Refresh();

            cb_VideoStreams.Items.Clear();
            VideoProfiles = null;

            if ((Parameters.Media_ServiceAddress == "") && !(PollMediaConfigurationAddress()))
            {
                System.Windows.Forms.MessageBox.Show("Unable to retrieve Media Service Address.  Unable to open video stream.", "Error", MessageBoxButtons.OK);
                btn_Video.Text = btnString;
                return;
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
                    System.Windows.Forms.MessageBox.Show("No video profiles found on this device.  Unable to open video stream.", "Error", MessageBoxButtons.OK);
                }

            }
            catch (Exception error)
            {
                cb_VideoStreams.Items.Add(error.Message);
            }

            btn_Video.Text = btnString;

        }

        /// <summary>
        /// Hanldes the selected test radio button checked changed events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_Selected_Test_CheckedChanged(object sender, EventArgs e)
        {
            // update the saved profile depending on what the user has selected
            if (rb_UseEmbedded_Tests.Checked)
                SaveParameter(TEST_SELECTION, rb_UseEmbedded_Tests.Name);
            else if(rb_UseUserXML_Tests.Checked)
                SaveParameter(TEST_SELECTION, rb_UseUserXML_Tests.Name);

        }

        /// <summary>
        /// Send multicast Hello Requests and disvovery the ONVIF compatible devices on the network
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Discover_Click(object sender, EventArgs e)
        {
            string messageSent, messageReceived, errorMessages;
            bool msgReceived = true;
            RemoteDiscovery.ProbeMatchesType PMT;
            RemoteDiscovery.ScopesType Scope;
            int msgErrorCount = 0;
            string tmpTxt = btn_Discover.Text;
            IPEndPoint receivedFrom;

            btn_Discover.Text = "Please wait";
            btn_Discover.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            // clear the old results
            ClearDiscoveredDevices();
            
            // use the multicast probe command to dicover the devices on the network
            UpdateTestParameters(false);
            
            try
            {
                // setup the network interface
                NetworkInterface.UDP_ConnectAnyMulticast("239.255.255.250", 3702, 1);
                //NetworkInterface.UDP_ConnectMulticast("239.255.255.250", IPAddress.Any.ToString(), 3702, 1);
                //NetworkInterface.Connect("239.255.255.250", 3702, 1);

                // build the probe request
                Scope = new RemoteDiscovery.ScopesType();
                Scope.Text = new string[] { "" }; //"onvif://www.onvif.org/type/super" , " onvif://www.onvif.org/type/analytics ", " onvif://www.onvif.org/type/video", " onvif://www.onvif.org/name/Bosch", " onvif://www.onvif.org/location/city/Nuernberg", " onvif://www.onvif.org/hardware/Dinion-IP-NWC" };
                //Scope.MatchBy = "http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc2396";

                //messageSent = TestMessage.Build_ProbeRequest(OnvifTests.DEFAULT_DEVICE_TYPE, Scope);
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

                    errorMessages = "";

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

            //NetworkInterface.Disconnect();
            NetworkInterface.UDP_Close();

            btn_Discover.Text = tmpTxt;
            btn_Discover.Refresh();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// The parameters may be filled in via the Device query and selction or manuall
        /// by the user.  If manually modified this function handles updating the neccissary
        /// parameters
        /// </summary>
        /// <param name="sender">Object of interest</param>
        /// <param name="e">EventArgs</param>
        private void Parameter_TextBoxes_Leave(object sender, EventArgs e)
        {
            // save the parameter
            //if (sender.Equals(tb_TargetXaddr))
            //    SaveParameter(tb_TargetXaddr.Name, tb_TargetXaddr.Text);

            if (sender.Equals(tb_TargetMDversion))
                SaveParameter(tb_TargetMDversion.Name, tb_TargetMDversion.Text);

            if (sender.Equals(tb_TargetEndPointAddress))
                SaveParameter(tb_TargetEndPointAddress.Name, tb_TargetEndPointAddress.Text);

            if (sender.Equals(tb_TargetType))
                SaveParameter(tb_TargetType.Name, tb_TargetType.Text);

            if (sender.Equals(tb_TargetScopes))
                SaveParameter(tb_TargetScopes.Name, tb_TargetScopes.Text);

            if (sender.Equals(tb_TargetIPAddress))
                SaveParameter(tb_TargetIPAddress.Name, tb_TargetIPAddress.Text);

            

            UpdateTestParameters(false);

            //if (sender.Equals(tb_TargetXaddr))
            //    VerifyUserDeviceSelection();
        }

        /// <summary>
        /// Open a connection to the video profile the user has selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_VideoStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult aDialog;
            Media.StreamSetup streamSetup;
            string msg, returnString, errorMessage = "";
            string URI, soapFault;

            // get the URI for the selected profile
            if (VideoProfiles == null)
                return;

            // if the video window isn't open return
            if (Vid == null)
                return;

            // if the selected index is greater then the known profiles length return
            if (cb_VideoStreams.SelectedIndex > VideoProfiles.Length)
                return;

            if ((Parameters.Media_ServiceAddress == "") && !(PollMediaConfigurationAddress()))
            {
                System.Windows.Forms.MessageBox.Show("Unable to retrieve Media Service Address.  Unable to view video stream.", "Error", MessageBoxButtons.OK);
                return;
            }


            // Does the user want the profile streamed?
            aDialog = MessageBox.Show("Do you wish to view profile \"" + VideoProfiles[cb_VideoStreams.SelectedIndex].Name + "\"", "Connect", MessageBoxButtons.OKCancel);

            if (aDialog == DialogResult.OK)
            {
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
                        return;
                    }

                    Media.GetStreamUriResponse GSR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetStreamUriResponse));

                    if (GSR.MediaUri == null)
                    {
                        MessageBox.Show("Unable to connect, Get Stream URI Response MediaUri is NULL" + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
                        return;
                    }

                    if (GSR.MediaUri.Uri == null)
                    {
                        MessageBox.Show("Unable to connect, Get Stream URI Response does not contain a Media URI" + Environment.NewLine + Environment.NewLine + soapFault, "Error", MessageBoxButtons.OK);
                        return;
                    }

                    URI = GSR.MediaUri.Uri;

                    if (URI.Contains("&line=0"))
                        URI = URI.Replace("&line=0", "");

                    Vid.Video_SetCredentials(Parameters.UserName, Parameters.Password);
                    Vid.OpenVideoStream(URI);                    

                }
                catch (Exception err)
                {
                    errorMessage = err.Message;

                }

                if (errorMessage != "")
                    MessageBox.Show("Unable to connect - " + errorMessage, "Error", MessageBoxButtons.OK);

            }


        }

        /// <summary>
        /// Update the IP address and save the parameter when the user changes the 
        /// selected Target Address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_TargetAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = "";

            try
            {
                tmpstring = cb_TargetAddress.SelectedItem.ToString();
                // parse the IP address out of the URL
                tb_TargetIPAddress.Text = TestMessage.ParseIpAddress(tmpstring);
                UpdateTestParameters(false);

                if (initilizationComplete)
                    SaveParameter((cb_TargetAddress.Name + SELECTED_INDEX), cb_TargetAddress.SelectedIndex.ToString());
            }
            catch (Exception err)
            {
                Console.WriteLine("error - " + err.Message);
                tb_TargetIPAddress.Text = "";
            }
        }


        /// <summary>
        /// If the user edits the text manually make note of it.  Once the user
        /// is done (left the control) peform the update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_TargetAddress_TextUpdate(object sender, EventArgs e)
        {
            TargetAddressTextChange = true;
        }

        /// <summary>
        /// Handle any changes made to the Target address once the user leaves
        /// the control.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    string addressValues = GetParameter(cb_TargetAddress.Name);
                    addressValues += " " + tmpString;
                    SaveParameter(cb_TargetAddress.Name, addressValues);
                    cb_TargetAddress.Items.Add(tmpString);
                    cb_TargetAddress.SelectedItem = tmpString;

                }

                // update the IP address
                tb_TargetIPAddress.Text = TestMessage.ParseIpAddress(tmpString);

                // update the parameters
                UpdateTestParameters(false);
            }


        }

        /// <summary>
        /// Handle the "Clear" buttons on the setup tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SetupPageClear_Click(object sender, EventArgs e)
        {
            if (sender == btn_ClearDeviceInformation)
            {
                tb_TestDevice_Brand.Text = "";
                tb_TestDevice_Model.Text = "";
                tb_TestDevice_SerialNumber.Text = "";
                tb_TestDevice_FWversion.Text = "";
                tb_TestDevice_Other.Text = "";
            }

            if (sender == btn_ClearTestInformation)
            {
                tb_Test_Operator.Text = "";
                tb_Test_OrganizationName.Text = "";
                tb_Test_OrganizationAddress.Text = "";
            }

        }

        /// <summary>
        /// Handle the text changed event for the timeout text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestOptions_TextChanged(object sender, EventArgs e)
        {
            UInt32 timeout;

            // if the Message timeout text box changed
            if (sender == tb_MessageTimeout)
            {
                try
                {
                    // try parsing it, if this fails use the default
                    timeout = UInt32.Parse(tb_MessageTimeout.Text);

                }
                catch
                {
                    tb_MessageTimeout.Text = (ONVIF_TestCases.TestCases_Class.DEFAUTLT_TIMEOUT).ToString();
                    timeout = ONVIF_TestCases.TestCases_Class.DEFAUTLT_TIMEOUT;
                }
                // update the value,  If the parsing failed it will be the default value
                SaveParameter(tb_MessageTimeout.Name, tb_MessageTimeout.Text);
                UpdateTestParameters(false);
            }

            // if the reboot time text box has changed
            if (sender == tb_RebootTime)
            {
                try
                {
                    // try parsing it, if this fails use the default
                    timeout = UInt32.Parse(tb_RebootTime.Text);

                }
                catch
                {
                    tb_RebootTime.Text = (ONVIF_TestCases.TestCases_Class.DEFAULT_REBOOT_TIME).ToString();
                    timeout = ONVIF_TestCases.TestCases_Class.DEFAULT_REBOOT_TIME;
                }
                // update the value,  If the parsing failed it will be the default value
                SaveParameter(tb_RebootTime.Name, tb_RebootTime.Text);
                UpdateTestParameters(false);
            }

            // if the reboot time text box has changed
            if (sender == tb_UserName)
            {                
                // update the value
                SaveParameter(tb_UserName.Name, tb_UserName.Text);
                UpdateTestParameters(false);
            }

            // if the reboot time text box has changed
            if (sender == tb_Password)
            {
                // update the value
                SaveParameter(tb_Password.Name, tb_Password.Text);
                UpdateTestParameters(false);
            }


        }


        /// <summary>
        /// Reset the test options panel, currently only the test and reboot timeout values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ResetTestOptions_Click(object sender, EventArgs e)
        {
            tb_MessageTimeout.Text = ONVIF_TestCases.TestCases_Class.DEFAUTLT_TIMEOUT.ToString();
            tb_RebootTime.Text = ONVIF_TestCases.TestCases_Class.DEFAULT_REBOOT_TIME.ToString();
            tb_Password.Text = "";
            tb_UserName.Text = "";


            SaveParameter(tb_MessageTimeout.Name, tb_MessageTimeout.Text);
            SaveParameter(tb_RebootTime.Name, tb_RebootTime.Text);
            SaveParameter(tb_Password.Name, tb_Password.Text);
            SaveParameter(tb_UserName.Name, tb_UserName.Text);
        }


        /// <summary>
        /// Discoverd device list box selected index change event handler.
        /// </summary>
        /// <param name="sender">Object that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void lb_DiscoverdDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x = lb_DiscoverdDevices.SelectedIndex;
            string tmpstring;

            if (x < 0)
                return;

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

            if (DiscoveredDevices[x].MatchesType.ProbeMatch[0].MetadataVersion != null)
                tb_TargetMDversion.Text = DiscoveredDevices[x].MatchesType.ProbeMatch[0].MetadataVersion.ToString();

            if (cb_TargetAddress.Items.Count > 0)
            {
                cb_TargetAddress.SelectedIndex = 0;
            }

            // save the parameters
            SaveParameter(cb_TargetAddress.Name, DiscoveredDevices[x].MatchesType.ProbeMatch[0].XAddrs);
            SaveParameter(tb_TargetMDversion.Name, tb_TargetMDversion.Text);
            SaveParameter(tb_TargetEndPointAddress.Name, tb_TargetEndPointAddress.Text);
            SaveParameter(tb_TargetType.Name, tb_TargetType.Text);
            SaveParameter(tb_TargetScopes.Name, tb_TargetScopes.Text);
            SaveParameter(tb_TargetIPAddress.Name, tb_TargetIPAddress.Text);

            // update the test
            UpdateTestParameters(false);

        }


        private void Tests_MouseSingleClick(object sender, MouseEventArgs e)
        {
            int groupNumber, testNumber;
            // don't do anything if the tests are not ready
            if (!testsReady)
                return;

            // if the tests are running don't do anything either
            if (runTest || runSingleStep || runSingleTest)
                return;

            // unhighlight all the controls
            if (!sender.Equals(lbl_Test_1)) lbl_Test_1.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_2)) lbl_Test_2.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_3)) lbl_Test_3.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_4)) lbl_Test_4.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_5)) lbl_Test_5.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_6)) lbl_Test_6.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_7)) lbl_Test_7.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_8)) lbl_Test_8.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_9)) lbl_Test_9.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_10)) lbl_Test_10.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_11)) lbl_Test_11.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_12)) lbl_Test_12.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_13)) lbl_Test_13.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_14)) lbl_Test_14.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_15)) lbl_Test_15.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_16)) lbl_Test_16.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_17)) lbl_Test_17.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_18)) lbl_Test_18.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_19)) lbl_Test_19.BackColor = Color.Transparent;
            if (!sender.Equals(lbl_Test_20)) lbl_Test_20.BackColor = Color.Transparent;


            currentTestTitle = ((Label)sender).Text;
            ((Label)sender).BackColor = Color.Silver;

            ((Label)sender).Refresh();

            // find out which test was slected and run or display results depending

            currentTestTitle = currentTestTitle.TrimStart(new char[] { '*', ' ' });

            // now figure out the index and start that test.
            groupNumber = testNumber = 0;
            foreach (TestCases_Class.TestSuite_Type group in Tests.Group)
            {
                testNumber = group.GetIndex(currentTestTitle);

                 

                if (testNumber != -1)
                {
                    if (Tests.Group[groupNumber].Tests[testNumber].TestComplete)
                    {
                        // the test has already been run, jut display the results
                        SetTestMessageText("");
                        AddTextMessageText(Tests.Group[groupNumber].Tests[testNumber].Number + " - " + Tests.Group[groupNumber].Tests[testNumber].Name + Environment.NewLine);
                        AddTextMessageText(Tests.Group[groupNumber].Tests[testNumber].Results);
                        break;
                    }
                    else
                    {
                        // run the test
                        SetTestIndex(groupNumber, testNumber);
                        Tests.Group[groupNumber].Tests[testNumber].ResetTest();
                        runSingleTest = true;


                        // restart the test runner
                        if (testRunner.ThreadState == ThreadState.Stopped)
                            InitilizeTestRunner();

                        break;
                    }
                }
                groupNumber++;

            }

        }

    

        /// <summary>
        /// Handle double click events of the test list.  This fucntion allows the user to
        /// start tests they double click on instead of having to run through the entire
        /// test suite
        /// </summary>
        /// <param name="sender">Object that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void Tests_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int groupNumber, testNumber;
            // don't do anything if the tests are not ready
            if (!testsReady)
                return;

            // make sure the form network interface is closed so the test runner's network interface works fine.
            //NetworkInterface.Disconnect();
            NetworkInterface.UDP_Close();

            // set the test time
            Tests.SetTestStartTime();

            // stop the current test
            runTest = false;
            testRunner.Abort(0);

            // find out what test the user clicked on and get the test ID
            if (sender.Equals(lbl_Test_1))
            {
                currentTestTitle = lbl_Test_1.Text;
                lbl_Test_1.BackColor = Color.Silver;
            }
            else
                lbl_Test_1.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_2))
            {
                currentTestTitle = lbl_Test_2.Text;
                lbl_Test_2.BackColor = Color.Silver;
            }
            else
                lbl_Test_2.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_3))
            {
                currentTestTitle = lbl_Test_3.Text;
                lbl_Test_3.BackColor = Color.Silver;
            }
            else
                lbl_Test_3.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_4))
            {
                currentTestTitle = lbl_Test_4.Text;
                lbl_Test_4.BackColor = Color.Silver;
            }
            else
                lbl_Test_4.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_5))
            {
                currentTestTitle = lbl_Test_5.Text;
                lbl_Test_5.BackColor = Color.Silver;
            }
            else
                lbl_Test_5.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_6))
            {
                currentTestTitle = lbl_Test_6.Text;
                lbl_Test_6.BackColor = Color.Silver;
            }
            else
                lbl_Test_6.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_7))
            {
                currentTestTitle = lbl_Test_7.Text;
                lbl_Test_7.BackColor = Color.Silver;
            }
            else
                lbl_Test_7.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_8))
            {
                currentTestTitle = lbl_Test_8.Text;
                lbl_Test_8.BackColor = Color.Silver;
            }
            else
                lbl_Test_8.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_9))
            {
                currentTestTitle = lbl_Test_9.Text;
                lbl_Test_9.BackColor = Color.Silver;
            }
            else
                lbl_Test_9.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_10))
            {
                currentTestTitle = lbl_Test_10.Text;
                lbl_Test_10.BackColor = Color.Silver;
            }
            else
                lbl_Test_10.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_11))
            {
                currentTestTitle = lbl_Test_11.Text;
                lbl_Test_11.BackColor = Color.Silver;
            }
            else
                lbl_Test_11.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_12))
            {
                currentTestTitle = lbl_Test_12.Text;
                lbl_Test_12.BackColor = Color.Silver;
            }
            else
                lbl_Test_12.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_13))
            {
                currentTestTitle = lbl_Test_13.Text;
                lbl_Test_13.BackColor = Color.Silver;
            }
            else
                lbl_Test_13.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_14))
            {
                currentTestTitle = lbl_Test_14.Text;
                lbl_Test_14.BackColor = Color.Silver;
            }
            else
                lbl_Test_14.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_15))
            {
                currentTestTitle = lbl_Test_15.Text;
                lbl_Test_15.BackColor = Color.Silver;
            }
            else
                lbl_Test_15.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_16))
            {
                currentTestTitle = lbl_Test_16.Text;
                lbl_Test_16.BackColor = Color.Silver;
            }
            else
                lbl_Test_16.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_17))
            {
                currentTestTitle = lbl_Test_17.Text;
                lbl_Test_17.BackColor = Color.Silver;
            }
            else
                lbl_Test_17.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_18))
            {
                currentTestTitle = lbl_Test_18.Text;
                lbl_Test_18.BackColor = Color.Silver;
            }
            else
                lbl_Test_18.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_19))
            {
                currentTestTitle = lbl_Test_19.Text;
                lbl_Test_19.BackColor = Color.Silver;
            }
            else
                lbl_Test_19.BackColor = Color.Transparent;

            if (sender.Equals(lbl_Test_20))
            {
                currentTestTitle = lbl_Test_20.Text;
                lbl_Test_20.BackColor = Color.Silver;
            }
            else
                lbl_Test_20.BackColor = Color.Transparent;

            currentTestTitle = currentTestTitle.TrimStart(new char[] { '*', ' ' });

            // now figure out the index and start that test.
            groupNumber = testNumber = 0;
            foreach (TestCases_Class.TestSuite_Type group in Tests.Group)
            {
                testNumber = group.GetIndex(currentTestTitle);
                if (testNumber != -1)
                {
                    SetTestIndex(groupNumber, testNumber);
                    Tests.Group[groupNumber].Tests[testNumber].ResetTest();
                    runSingleTest = true;


                    // restart the test runner
                    if (testRunner.ThreadState == ThreadState.Stopped)
                        InitilizeTestRunner();

                    break;
                }
                groupNumber++;

            }

        }

        /// <summary>
        /// The check button triggers a GetDeviceInformationRequest call to the NVT and will
        /// display the returned information in a text box.  This allows the user to validate
        /// that they are communicating with the correct device.
        /// </summary>
        /// <param name="sender">Sender that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void btn_checkConnection_Click(object sender, EventArgs e)
        {

            string buttonText = ((System.Windows.Forms.Button)sender).Text;
            string URL = "";

            //URL = NetworkInterface.GetMac(Parameters.Target_IP, Parameters.UserName, Parameters.Password);

            URL = Parameters.URL;
            // update the button text and cursor so the user knows something is happening
            btn_checkConnection.Text = "Wait";
            btn_checkConnection.Refresh();

            Cursor.Current = Cursors.WaitCursor;
            // send the request
            VerifyUserDeviceSelection();

            ((System.Windows.Forms.Button)sender).Text = buttonText;
            Cursor.Current = Cursors.Default;

        }

        /// <summary>
        /// Clear test parameters
        /// </summary>
        /// <param name="sender">Sender that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void btn_ClearTestParameters_Click(object sender, EventArgs e)
        {
            ClearTestParameters();
        }

        /// <summary>
        /// Tab control selection handler.  Capture all the tab selection events
        /// and make sure the user doesn't need to enter some values or perform 
        /// some other tasks before enter/leaving a tab
        /// </summary>
        /// <param name="sender">Tab that caused the event</param>
        /// <param name="e">Event arguements</param>
        private void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {

            if (TabControl.SelectedIndex != 0)
            {
                // check to make sure the needed information has been added
                if (tb_TestDevice_Brand.Text.Equals("") ||
                    tb_TestDevice_Model.Text.Equals("") ||
                    tb_TestDevice_SerialNumber.Text.Equals("") ||
                    tb_Test_Operator.Text.Equals("") ||
                    tb_Test_OrganizationName.Text.Equals("") ||
                    tb_Test_OrganizationAddress.Text.Equals("") ||
                    tb_TestDevice_FWversion.Text.Equals(""))
                {
                    InidicateRequiredTestInfoFields();
                    TabControl.SelectedIndex = 0;
                    System.Windows.Forms.MessageBox.Show("Please enter all fields", "Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    HideRequiredTestInfoFieldInidicators();

                    // remember the settings
                    if (settingsKey != null)
                    {
                        SaveParameter(tb_TestDevice_Brand.Name, tb_TestDevice_Brand.Text);
                        SaveParameter(tb_TestDevice_Model.Name, tb_TestDevice_Model.Text);
                        SaveParameter(tb_TestDevice_SerialNumber.Name, tb_TestDevice_SerialNumber.Text);
                        SaveParameter(tb_TestDevice_FWversion.Name, tb_TestDevice_FWversion.Text);
                        SaveParameter(tb_TestDevice_Other.Name, tb_TestDevice_Other.Text);


                        SaveParameter(tb_Test_Operator.Name, tb_Test_Operator.Text);
                        SaveParameter(tb_Test_OrganizationName.Name, tb_Test_OrganizationName.Text);
                        SaveParameter(tb_Test_OrganizationAddress.Name, tb_Test_OrganizationAddress.Text);
                    }
                }
            }

            if (TabControl.SelectedTab == tab_device)
            {
                tb_DeviceTab_URL.Text = Parameters.URL;
                tb_DeviceTab_IP.Text = Parameters.Target_IP;
                tb_DeviceTab_MediaURL.Text = Parameters.Media_ServiceAddress;                 
            }

            if (TabControl.SelectedTab == tab_test)
            {
                // make sure the user has loaded the tests
                if (!testsReady)
                {
                    TabControl.SelectedIndex = 1;
                    System.Windows.Forms.MessageBox.Show("Please select which tests you wish to run and click \"Load Tests\"", "Load Tests", MessageBoxButtons.OK);

                }

            }
        }


        #endregion

        #region Form Functions


        private DeviceManagement.GetDeviceInformationResponse GetDeviceInfo(out string errorMessage)
        {
            DeviceManagement.GetDeviceInformationResponse GDIR;
            string message, messageReceived, soapFault, errormsg;

            message = TestMessage.Build_GetDeviceInformationRequest();

            try
            {
                messageReceived = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.URL, message, Parameters.UserName, Parameters.Password);
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

        private void VerifyUserDeviceSelection()
        {
            string errorMessage = "";
            string deviceInfo = "";
            DialogResult aDialog;
            DeviceManagement.GetDeviceInformationResponse GDIR;

            

            // send a discovery message and display the information to the user so they can verify
            try
            {
                GDIR = GetDeviceInfo(out errorMessage);
            }
            catch (Exception e)
            {
                deviceInfo += "Unable to communicate with device" + Environment.NewLine;
                deviceInfo += "Error - " + e.Message;
                System.Windows.Forms.MessageBox.Show(deviceInfo, "Error", MessageBoxButtons.OK);
                return;
            }
            if (GDIR != null)
            {
                if (GDIR.FirmwareVersion != null)
                    deviceInfo += "Firmware version = " + GDIR.FirmwareVersion + Environment.NewLine;
                    

                if (GDIR.HardwareId != null)
                    deviceInfo += "Hardware ID = " + GDIR.HardwareId + Environment.NewLine;

                if (GDIR.Manufacturer != null)
                    deviceInfo += "Manufacturer = " + GDIR.Manufacturer + Environment.NewLine;

                if (GDIR.Model != null)
                    deviceInfo += "Model = " + GDIR.Model + Environment.NewLine;

                if (GDIR.SerialNumber != null)
                    deviceInfo += "Serial Number = " + GDIR.SerialNumber + Environment.NewLine;

                tb_DeviceTab_FW.Text = GDIR.FirmwareVersion;
                tb_DeviceTab_HW.Text = GDIR.HardwareId;
                tb_DeviceTab_MFG.Text = GDIR.Manufacturer;
                tb_DeviceTab_Model.Text = GDIR.Model;
                tb_DeviceTab_SerialNumber.Text = GDIR.SerialNumber;
            }

            // if on the managment tab show the info in a pop up box
            if (TabControl.SelectedTab == tab_management)
            {
                if (!deviceInfo.Equals(""))
                {

                    deviceInfo = deviceInfo.Insert(0, "Here is the Device Information Response received" + Environment.NewLine + Environment.NewLine);
                    aDialog = System.Windows.Forms.MessageBox.Show(deviceInfo, "Device Info Correct", MessageBoxButtons.OK);

                }
                else
                {
                    aDialog = System.Windows.Forms.MessageBox.Show("Error in comunicating with device \n" + errorMessage, "Error", MessageBoxButtons.OK);

                }
            }
                   

        }

        private bool PollMediaConfigurationAddress()
        {
            string MessagesSent = "";
            string MessagesReceived = "";
            string soapFault = "";
            string errorMessages = "";

            Parameters.Media_ServiceAddress = "";
            tb_DeviceTab_MediaURL.Text = "";

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
                return false;
            }


            // check to make sure there wasn't an error
            if (TestMessage.Check_SoapFault(MessagesReceived, out soapFault))
            {
                // tell the user the NVC was unable to communicate with the device so no media service address was found
                ExceptionMessageBox mbox = new ExceptionMessageBox("GetCapabilitiesResponse returned SOAP error = " + soapFault + "." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                mbox.Show(null);
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
                        return false;
                    }
                    else
                    {
                        if (GCR.Capabilities.Media == null)
                        {
                            // tell the user the NVC was unable to communicate with the device so no media service address was found
                            ExceptionMessageBox mbox = new ExceptionMessageBox("Required capabilities not found, GetCapabilitiesResponse Media = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                            mbox.Show(null);
                            return false;
                        }
                        else
                        {
                            if (GCR.Capabilities.Media.XAddr == null)
                            {
                                // tell the user the NVC was unable to communicate with the device so no media service address was found
                                ExceptionMessageBox mbox = new ExceptionMessageBox("Required capabilities not found, GetCapabilitiesResponse Media service address = NULL." + Environment.NewLine + Environment.NewLine + "No Media service address found, the NVC will be unable to execute Media tests.", "ERROR", ExceptionMessageBoxButtons.OK);
                                mbox.Show(null);
                                return false;
                            }
                            else
                            {
                                Parameters.Media_ServiceAddress = GCR.Capabilities.Media.XAddr;
                                tb_DeviceTab_MediaURL.Text = Parameters.Media_ServiceAddress;
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

        /// <summary>
        /// Convert testActions to displayable string
        /// </summary>
        /// <param name="action">Test action to convert</param>
        /// <returns>string of test action</returns>
        private string testAction_toString(TestCases_Class.TestActions action)
        {
           
            switch (action)
            {
                case TestCases_Class.TestActions.Perform:
                    return "x";

                //case TestCases_Class.TestActions.Fail:
                //    return "f";

                //case TestCases_Class.TestActions.Pass:
                //    return "p";

                case TestCases_Class.TestActions.Skip:
                    return "s";

                default:
                case TestCases_Class.TestActions.NULL:
                    return " ";

            }

        }

        /// <summary>
        /// Convert testCompliance to displayable string
        /// </summary>
        /// <param name="compliance">Test compliance to convert</param>
        /// <returns>string of test compliance</returns>
        private string testCompliance_toString(TestCases_Class.TestCompliance compliance)
        {
            switch (compliance)
            {
                case TestCases_Class.TestCompliance.Should:
                case TestCases_Class.TestCompliance.Should_if_Supported:
                case TestCases_Class.TestCompliance.Optional:
                    return "*";

                default:
                case TestCases_Class.TestCompliance.Must:
                case TestCases_Class.TestCompliance.Must_if_Supported:
                case TestCases_Class.TestCompliance.NULL:
                    return "";

            }

        }

        /// <summary>
        /// Update test and test result/action labels
        /// </summary>
        /// <param name="line">Line number to update 0-13</param>
        /// <param name="test">Test string</param>
        /// <param name="action">Test testAction</param>
        /// <param name="compliance">Test testCompliance</param>
        private void updateTestLine(int line, 
                                    string test, 
                                    TestCases_Class.TestActions action, 
                                    TestCases_Class.TestCompliance compliance,
                                    bool aTest,
                                    bool passed)
        {

            Label testLabel, testResultLabel;

            switch (line)
            {
                default:
                case 0:
                    testLabel = lbl_Test_1;
                    testResultLabel = lbl_testResult_1;
                    break;
                case 1:
                    testLabel = lbl_Test_2;
                    testResultLabel = lbl_testResult_2;
                    break;
                case 2:
                    testLabel = lbl_Test_3;
                    testResultLabel = lbl_testResult_3;
                    break;
                case 3:
                    testLabel = lbl_Test_4;
                    testResultLabel = lbl_testResult_4;
                    break;
                case 4:
                    testLabel = lbl_Test_5;
                    testResultLabel = lbl_testResult_5;
                    break;
                case 5:
                    testLabel = lbl_Test_6;
                    testResultLabel = lbl_testResult_6;
                    break;
                case 6:
                    testLabel = lbl_Test_7;
                    testResultLabel = lbl_testResult_7;
                    break;
                case 7:
                    testLabel = lbl_Test_8;
                    testResultLabel = lbl_testResult_8;
                    break;
                case 8:
                    testLabel = lbl_Test_9;
                    testResultLabel = lbl_testResult_9;
                    break;
                case 9:
                    testLabel = lbl_Test_10;
                    testResultLabel = lbl_testResult_10;
                    break;
                case 10:
                    testLabel = lbl_Test_11;
                    testResultLabel = lbl_testResult_11;
                    break;
                case 11:
                    testLabel = lbl_Test_12;
                    testResultLabel = lbl_testResult_12;
                    break;
                case 12:
                    testLabel = lbl_Test_13;
                    testResultLabel = lbl_testResult_13;
                    break;
                case 13:
                    testLabel = lbl_Test_14;
                    testResultLabel = lbl_testResult_14;
                    break;
                case 14:
                    testLabel = lbl_Test_15;
                    testResultLabel = lbl_testResult_15;
                    break;
                case 15:
                    testLabel = lbl_Test_16;
                    testResultLabel = lbl_testResult_16;
                    break;
                case 16:
                    testLabel = lbl_Test_17;
                    testResultLabel = lbl_testResult_17;
                    break;
                case 17:
                    testLabel = lbl_Test_18;
                    testResultLabel = lbl_testResult_18;
                    break;
                case 18:
                    testLabel = lbl_Test_19;
                    testResultLabel = lbl_testResult_19;
                    break;
                case 19:
                    testLabel = lbl_Test_20;
                    testResultLabel = lbl_testResult_20;
                    break;

            }

            
            

            testLabel.Text = testCompliance_toString(compliance) + test;
            if (compliance == TestCases_Class.TestCompliance.Optional)
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' ') + Environment.NewLine + "Requirement Level - OPTIONAL");
            else if (compliance == TestCases_Class.TestCompliance.Should)
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' ') + Environment.NewLine + "Requirement Level - SHOULD");
            else if (compliance == TestCases_Class.TestCompliance.Should_if_Supported)
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' ') + Environment.NewLine + "Requirement Level - SHOULD IF SUPPORTED");
            else if (compliance == TestCases_Class.TestCompliance.Must_if_Supported)
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' ') + Environment.NewLine + "Requirement Level - MUST IF SUPPORTED");
            else if (compliance == TestCases_Class.TestCompliance.Must)
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' ') + Environment.NewLine + "Requirement Level - MUST");
            else
                toolTip_Tests.SetToolTip(testLabel, test.TrimStart(' '));
            
            testResultLabel.Text = testAction_toString(action);

            if ( (testResultLabel.Text == " ") || (testLabel.Text == ""))
                testResultLabel.BorderStyle = BorderStyle.None;
            else
                testResultLabel.BorderStyle = BorderStyle.FixedSingle;

            if (compliance == TestCases_Class.TestCompliance.Optional)
                toolTip_Results.SetToolTip(testResultLabel, "Optional test");

            // set the font
            if (aTest)
            {
                testLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                testResultLabel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                testLabel.Text = testLabel.Text.Insert(0, "   ");
            }
            else
            {
                testLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                testResultLabel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                
            }

            if (aTest)
            {
                // set the colors accordingly
                if (passed)
                {
                    testLabel.ForeColor = Color.Black;
                    testResultLabel.ForeColor = Color.Black;
                }
                else
                {
                    testLabel.ForeColor = Color.Red;
                    testResultLabel.ForeColor = Color.Red;
                }
            }
            else
            {
                testLabel.ForeColor = Color.Black;
                testResultLabel.ForeColor = Color.Black;
            }

            // if this is the current running test highlight it
            if (testLabel.Text.TrimStart(new char[] {'*', ' '}).Equals(currentTestTitle)) // remove the optional indicator
                testLabel.BackColor = Color.Silver;
            else
                testLabel.BackColor = Color.Transparent;
            
        }

        /// <summary>
        /// Fill the test array labels with the loaded tests
        /// </summary>
        /// <param name="Tests">Tests to display</param>
        private void fillTestArray(TestCases_Class.TestGroup_Type Tests)
        {
            int lineCount = 0;
            int x;

            // clear any existing text
            //clearTestLines();

            if (testIndex == 0) // if we are in the first line print out the test suite info
                updateTestLine(lineCount++, Tests.Name, TestCases_Class.TestActions.NULL, TestCases_Class.TestCompliance.NULL, false, true);
            else
                lineCount = 1;

            // print out the test info
            foreach (TestCases_Class.TestSuite_Type aSuite in Tests.Group)
            {
                // if this test group is >= to the test index it is one to be displayed
                if ((lineCount >= testIndex) && (lineCount < (testIndex + MAX_TEST_INDEX)))
                {
                    updateTestLine(lineCount - testIndex, aSuite.Description, aSuite.Action, TestCases_Class.TestCompliance.NULL, false, true);
                    lineCount++;
                }
                else
                    lineCount++;
                
                // go through the test group and display any tests within
                foreach (TestCases_Class.Test_Type aTest in aSuite.Tests)
                {
                    if ((lineCount >= testIndex) && (lineCount < (testIndex + MAX_TEST_INDEX)))
                    {
                        updateTestLine(lineCount - testIndex, aTest.Name, aTest.Action, aTest.Compliance, true, aTest.TestPassed);
                        lineCount++;
                    }
                    else
                        lineCount++;

                }
            }

            x = (lineCount - testIndex);
            for (; x < MAX_TEST_INDEX; x++)
            {
                clearTestLine(x);
            }


            // clear any remaining lines

            // update the scroll bar so it matches the number of tests avalible
            vsb_TestScroller.Maximum = lineCount;
            vsb_TestScroller.LargeChange = 7;
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
            
            Parameters.TestTimeout = int.Parse(tb_MessageTimeout.Text);
            Parameters.RebootTime = int.Parse(tb_RebootTime.Text);

            Parameters.UserName = tb_UserName.Text;
            Parameters.Password = tb_Password.Text;

            Parameters.Media_ServiceAddress = "";

            
        }

        

        /// <summary>
        /// Remove all discovered devices from the the Device Listbox
        /// </summary>
        private void ClearDiscoveredDevices()
        {
            DiscoveredDevices_Count = 0;
            DiscoveredDevices = new DiscoveredDevices_Type[1];

            lb_DiscoverdDevices.Items.Clear();

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

            tb_DeviceTab_SerialNumber.Text = "";
            tb_DeviceTab_Model.Text = "";
            tb_DeviceTab_MFG.Text = "";
            tb_DeviceTab_IP.Text = "";
            tb_DeviceTab_HW.Text = "";
            tb_DeviceTab_FW.Text = "";
            tb_DeviceTab_URL.Text = "";
            tb_DeviceTab_MediaURL.Text = "";
        }

        /// <summary>
        /// Parse the ProbeMatchesType and add any new unique device to the device list box
        /// </summary>
        /// <param name="newDev">Device to add</param>
        /// <param name="receivedFrom">Network endpoint that sent the ProbeMaches response</param>
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

        /// <summary>
        /// Build the test suite using the embedded ONVIF compliance test
        /// </summary>
        private void Load_ONVIF_ComplianceTest()
        {
            // The user wants to load the embedded tests
            Tests = TestCases.SetupDemo();
            testsReady = true;
            tb_Messages.Text = "";
            fillTestArray(Tests);
        }

        /// <summary>
        /// Build the test suite using the user specified test file
        /// </summary>
        private void Load_USER_SpecifiedTests()
        {
            TestCases_Class.TestGroup_Type? tmpTests;

            // Load and verify the XML test file specified by the user
            if (tb_TestCaseXMLFile.Text.Equals(""))
            {
                // No test file specified
                tb_Messages.Text = "";
                testsReady = false;
            }
            else
            {
                // check the test file to make sure it is valid
                if (TestCases.ParseTest(out tmpTests, tb_TestCaseXMLFile.Text.ToString()))
                {
                    // the test file is valid so load the tests specified
                    Tests = (ONVIF_TestCases.TestCases_Class.TestGroup_Type)tmpTests;
                    testsReady = true;
                    tb_Messages.Text = "";
                    fillTestArray(Tests);
                }
            }
        }

        /// <summary>
        /// Turn on the "*" indicating to the user what test fields are required
        /// </summary>
        private void InidicateRequiredTestInfoFields()
        {
            lbl_required.Visible = true;

            lbl_req1.Visible = true;
            lbl_req2.Visible = true;
            lbl_req3.Visible = true;
            lbl_req4.Visible = true;
            lbl_req5.Visible = true;
            lbl_req6.Visible = true;
            lbl_req7.Visible = true;

        }

        /// <summary>
        /// Turn off the "*" indicating what test fields are required
        /// </summary>
        private void HideRequiredTestInfoFieldInidicators()
        {
            lbl_required.Visible = false;

            lbl_req1.Visible = false;
            lbl_req2.Visible = false;
            lbl_req3.Visible = false;
            lbl_req4.Visible = false;
            lbl_req5.Visible = false;
            lbl_req6.Visible = false;
            lbl_req7.Visible = false;

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
                cb_VideoStreams.Refresh();
            }


        }



        #endregion
             
        #region Threads and Thread Functions
               
        private void GetTestIndex(out int group, out int test)
        {
            lock (locker)
            {
                group = TR_GroupCount;
                test = TR_TestCount;
            }

        }

        private void SetTestIndex(int group, int test)
        {
            lock (locker)
            {
                TR_GroupCount = group;
                TR_TestCount = test;
            }

        }
            

        /// <summary>
        /// Test Runner Thread, performs the user specified test sequence
        /// </summary>
        private void TestRunner()
        {
            ExceptionMessageBox mbox;
            //DialogResult aDialog;
            int x, count;
            string testResults = "";
            int testRunner_Group = 0, testRunner_Test = 0;
            ONVIF_NetworkInterface.NetworkInterface_Class TR_NetworkInterface = new ONVIF_NetworkInterface.NetworkInterface_Class();

            Parameters.Video_OpenWindow = new TestCases_Class.Video_OpenVideoWindow_CallBack(StartVideoForm);
            Parameters.Video_SetMode = new TestCases_Class.Video_SetVideoMode_CallBack(SetVideoFormMode);
            Parameters.Video_CloseWindow = new TestCases_Class.Video_CloseVideoWindow_CallBack(StopVideoForm);

            Parameters.UpdateIPaddress = new TestCases_Class.Parameter_UpdateIP(UpdateTargetAddress);

            // as long as we are not told to quite keep running          
            while (!testRunner_Quit)
            {
                //ResetTestTimer(2000);

                // if the tests area ready and the user has decided to run
                // the tests or a single test go ahead
                if (testsReady && (runTest || runSingleStep || runSingleTest))
                {      
                    
                    // what test are we on?
                    GetTestIndex(out testRunner_Group, out testRunner_Test);



                    // if the user wants to re-run the tests reinitilize the
                    // tests
                    if (reRunTests)
                    {
                        // mark all tests as rerun
                        Tests.ReRun();                        
                        reRunTests = false;
                        currentTestTitle = "";
                    }
                                   


                    // as long as there are still tests to be run
                    if ((testRunner_Group < Tests.Group.Length) && (testRunner_Test < Tests.Group[testRunner_Group].Tests.Length))
                    {
                        // If the test is still not run a single test step
                        if (!Tests.Group[testRunner_Group].Tests[testRunner_Test].TestComplete)
                        {
                            // if this is the first step display the message name
                            if (Tests.Group[testRunner_Group].Tests[testRunner_Test].CurrentStep == 0)
                            {
                                SetTestMessageText("");

                                // make sure the network connection from the previous test is closed
                                //TR_NetworkInterface.Disconnect();
                                TR_NetworkInterface.UDP_Close();

                                AddTextMessageText(Tests.Group[testRunner_Group].Tests[testRunner_Test].Number + " - " + Tests.Group[testRunner_Group].Tests[testRunner_Test].Name + Environment.NewLine);

                                currentTestTitle = Tests.Group[testRunner_Group].Tests[testRunner_Test].Name;

                                // move the scroll bar if needed
                                count = 1; // main title
                                for(x = 0; x < testRunner_Group; x++){
                                    count += Tests.Group[x].Tests.Length + 1; 
                                }
                                count += (x + 1); // add one for the suite titles 
                                count += testRunner_Test;
                                // if count isn't visiable in the scroll bar move it down
                                if ((vsb_TestScroller.Value + MAX_TEST_INDEX) < count)
                                    UpdateTestDisplay(count - MAX_TEST_INDEX);
                                else if(count < (vsb_TestScroller.Value))
                                    UpdateTestDisplay(count);

                                UpdateTestLabel_Labels();
                            }

                            // add the results to the test result text box
                            try
                            {
                                testResults = TestCases.RunTest(ref Tests.Group[testRunner_Group].Tests[testRunner_Test], ref Parameters, ref TR_NetworkInterface);
                                Tests.Group[testRunner_Group].Tests[testRunner_Test].Results += testResults;

                                AddTextMessageText(testResults);
                            }
                            catch (ThreadAbortException exc)
                            {
                                AddTextMessageText(ONVIF_TestCases.OnvifTests.STEP_MSG_SPACING + ONVIF_TestCases.OnvifTests.ERROR_MSG_PREFIX + "Test Aborted - " + exc.Message + Environment.NewLine);
                                //TR_NetworkInterface.Disconnect();
                                TR_NetworkInterface.UDP_Close();
                                if ((exc.ExceptionState == null) || ((int)exc.ExceptionState == 0))
                                    Thread.ResetAbort();
                            }
                            catch (System.Net.Sockets.SocketException socketExp)
                            {
                                // there was a socket problem, restart the network interface and retest
                                AddTextMessageText(ONVIF_TestCases.OnvifTests.STEP_MSG_SPACING + ONVIF_TestCases.OnvifTests.ERROR_MSG_PREFIX + "Network Exception stopping tests - " + socketExp.Message + Environment.NewLine);

                                //TR_NetworkInterface.Disconnect();
                                TR_NetworkInterface.UDP_Close();

                                runTest = false;
                                runSingleStep = false;
                                runSingleTest = false;
                            }
                            catch (TestCase_StopTest StopTest_Excp)
                            {
                                // the inner exeption message is used to pass up the last of the test results if the test has to be stopped
                                if (StopTest_Excp.InnerException != null)
                                {
                                    Tests.Group[testRunner_Group].Tests[testRunner_Test].Results += StopTest_Excp.InnerException.Message;
                                    AddTextMessageText(StopTest_Excp.InnerException.Message);
                                }

                                mbox = new ExceptionMessageBox(StopTest_Excp.Message, "Stop Test?", ExceptionMessageBoxButtons.Custom);
                                mbox.SetButtonText("Stop", "Continue");
                                mbox.Show(null);


                                if (mbox.CustomDialogResult == ExceptionMessageBoxDialogResult.Button1)  // stop the test
                                {
                                    runTest = false;
                                    runSingleStep = false;
                                    runSingleTest = true; // finish this test and stop
                                }
                            }
                            catch (Exception e)
                            {
                                
                                //TR_NetworkInterface.Disconnect();
                                TR_NetworkInterface.UDP_Close();
                                Tests.Group[testRunner_Group].Tests[testRunner_Test].StepComplete(false);
                                AddTextMessageText(ONVIF_TestCases.OnvifTests.STEP_MSG_SPACING + ONVIF_TestCases.OnvifTests.ERROR_MSG_PREFIX + "Error running tests - " + e.Message + Environment.NewLine);
                                runTest = false;
                                runSingleStep = false;
                                runSingleTest = false;
                            }

                            
                        }
                        else
                        {
                            // if this test was executed indicate the pass or failed status
                            if (Tests.Group[testRunner_Group].Tests[testRunner_Test].Action == TestCases_Class.TestActions.Perform)
                            {
                                if (Tests.Group[testRunner_Group].Tests[testRunner_Test].TestPassed)
                                {
                                    AddTextMessageText("Test PASSED" + Environment.NewLine);
                                    Tests.Group[testRunner_Group].Tests[testRunner_Test].Results += "Test PASSED" + Environment.NewLine;
                                }
                                else
                                {
                                    AddTextMessageText("Test FAILED" + Environment.NewLine);
                                    Tests.Group[testRunner_Group].Tests[testRunner_Test].Results += "Test FAILED" + Environment.NewLine;
                                }
                            }

                            AddTextMessageText("" + Environment.NewLine);
                            
                            //TR_NetworkInterface.Disconnect();
                            TR_NetworkInterface.UDP_Close();
                            
                            // now incrament the test count   
                            SetTestIndex(testRunner_Group, ++testRunner_Test);
                            //SetTestMessageText("");
                            runSingleTest = false;    
                       
                        }

                        // move to the next test group
                        if (testRunner_Test >= Tests.Group[testRunner_Group].Tests.Length)
                        {
                            testRunner_Test = 0;
                            SetTestIndex(++testRunner_Group, testRunner_Test);
                        }
                    }
                    else
                    {
                        runTest = runSingleStep = runSingleTest = false;
                        // all the tests are complete, compile summary data here
                        System.Windows.Forms.MessageBox.Show("Tests complete", "Done", MessageBoxButtons.OK);

                        //TR_NetworkInterface.Disconnect();
                        TR_NetworkInterface.UDP_Close();
                    }
                    
                    // if the user only wanted to run a single test stop now
                    if (runSingleStep)
                        runSingleStep = false;
                }
               
                // sleep for a bit so the form will update, and take some strain off the 
                // application
                Thread.Sleep(thread_Sleep);
               
            }

        }

        /// <summary>
        /// Video Form Thread, this thread runs to completion each time and must be 
        /// set back up once done
        /// </summary>
        private void VideoForm()
        {
            Vid = new Video();

            Parameters.RTSPInitVidStream = new TestCases_Class.RTSPInitVidStream_CallBack(Vid.InitVideoStream);
            Parameters.RTSPOptions = new TestCases_Class.RTSPOptions_CallBack(Vid.Query_RTSP_Options);
            Parameters.RTSPDescribe = new TestCases_Class.RTSPDescribe_CallBack(Vid.Query_RTSP_Describe);
            Parameters.RTSPSetup = new TestCases_Class.RTSPSetup_CallBack(Vid.Query_RTSP_Setup);
            Parameters.RTSPPlay = new TestCases_Class.RTSPPlay_CallBack(Vid.Query_RTSP_Play);
            Parameters.RTSPSetParameter = new TestCases_Class.RTSPSetParameter_CallBack(Vid.Query_RTSP_SetParameter);
            Parameters.RTSPTeardown = new TestCases_Class.RTSPTeardown_CallBack(Vid.Query_RTSP_Teardown);

            Parameters.HTTP_POST = new TestCases_Class.Video_SendPOST(Vid.HTTP_Send_POST);
            Parameters.HTTP_GET = new TestCases_Class.Video_PerformGET(Vid.HTTP_Request_GET);

            Parameters.Video_SetCredentials = new TestCases_Class.Video_SetCredentials_CallBack(Vid.Video_SetCredentials);

            Vid.SetDeviceURL(Parameters.URL, Parameters.Target_IP);
            //Vid.InitVideoWindow();

            Application.Run(Vid);

            // close video form
            Vid.Close();
            Vid.Dispose();
            Vid = null;
            Parameters.RTSPOptions = null;
            Parameters.RTSPDescribe = null;
            Parameters.RTSPSetup = null;
            Parameters.RTSPPlay = null;
            Parameters.RTSPSetParameter = null;
            Parameters.RTSPTeardown = null;

            Parameters.HTTP_GET = null;
            Parameters.HTTP_POST = null;

            // call the closed function
            SetIntegerValue_CallBack d = new SetIntegerValue_CallBack(Video_FormClosed);
            this.Invoke(d, new object[] { 0 });
        }


        public void ResetTestTimer(int time)
        {
            if(tb_StreamURI.InvokeRequired)
            {
                SetTestTimer_CallBack d = new SetTestTimer_CallBack(ResetTestTimer);
                this.Invoke(d, new object[] { time });
                
            }
            else
            {
                // reset the timer
                //TestTimer.Stop();
                //TestTimer.Interval = time;
                //TestTimer.Start();
            }

        }

        /// <summary>
        /// Asynchronous function call to set text in the test message box.
        /// Handles setting the text box text used to display test messages.  Since 
        /// the thread running the test is not the same one to create the text box 
        /// the data must go through the call back handler.
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="append">True=Append, False=Overwrite</param>
        public void SetMessageText(string text, bool append)
        {
            if (tb_Messages.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetMessageText);
                this.Invoke(d, new object[] { text, append });
            }
            else
            {
                if (append)
                    tb_Messages.Text += text;
                else
                    tb_Messages.Text = text;
            }
        }

        /// <summary>
        /// Asynchronous function call to set text in the test results message box
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="append">True=Append, False=Overwrite</param>
        public void SetResultsText(string text, bool append)
        {
            if (tb_TestSummary.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetResultsText);
                this.Invoke(d, new object[] { text, append });
            }
            else
            {
                if (append)
                    tb_TestSummary.Text += text;
                else
                    tb_TestSummary.Text = text;

            }
        }

        /// <summary>
        /// Display new test data and append results
        /// Handles setting the text box text used to display test messages.  Since 
        /// the thread running the test is not the same one to create the text box 
        /// the data must go through the call back handler.
        /// </summary>
        /// <param name="message">Message to display</param>
        public void SetTestMessageText(string message)
        {
            SetMessageText(message, false);
            SetResultsText(message, true);
        }

        /// <summary>
        /// Append test data and results
        /// </summary>
        /// <param name="message">Message to append</param>
        public void AddTextMessageText(string message)
        {
            SetMessageText(message, true);
            SetResultsText(message, true);
        }

        private void UpdateTestDisplay(int value)
        {
            if (vsb_TestScroller.InvokeRequired)
            {
                SetIntegerValue_CallBack d = new SetIntegerValue_CallBack(UpdateTestDisplay);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                vsb_TestScroller.Value = value;
            }

        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            // There must be an error in the test. stop the test and restart
            //AddTextMessageText("        ** ERROR - Test failed to check in, aborting test and failing it" + Environment.NewLine);
            //testRunner.Abort(0);
            //testRunner.Interrupt();
            //btn_Stop.PerformClick();
            //TestTimer.Enabled = false;

            if ((runTest || runSingleStep || runSingleTest) && (btn_Start.Text != BTN_START_RUNNING))
                btn_Start.Text = BTN_START_RUNNING;

            if (!(runTest || runSingleStep || runSingleTest) && (btn_Start.Text != BTN_START_DEFAULT))
                btn_Start.Text = BTN_START_DEFAULT;
        }

        #endregion

        #region Delegate Calls

        /// <summary>
        /// Start the video form (open it up)
        /// </summary>
        private void StartVideoForm()
        {

            // open the video form if not already done
            if (videoForm.ThreadState == ThreadState.Unstarted)
                videoForm.Start();
            else if (videoForm.ThreadState == ThreadState.Stopped) // the form has been closed
            {
                videoForm.Join();
                // create a new thread since the old one is done
                videoForm = new Thread(VideoForm);

                videoForm.Start();
            }

            while ((Vid == null) || !Vid.VideoFormRunning)
            {
                System.Threading.Thread.Sleep(100);
            }
            System.Threading.Thread.Sleep(200);
        }

        /// <summary>
        /// Stop the video form (close it)
        /// </summary>
        private void StopVideoForm()
        {
            try
            {
                Vid.CloseVideoWindow();
            }
            catch { }
        }

        /// <summary>
        /// Set video mode (test or not)
        /// </summary>
        /// <param name="testMode"></param>
        private void SetVideoFormMode(bool testMode)
        {
            try
            {
                Vid.SetDisplayMode_Test(testMode);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Delegate call so the Test runner can update the test scroll bar and keep the active test 
        /// viewable by the user
        /// </summary>
        private void UpdateTestLabel_Labels()
        {
            TestScrollBarUpdate_CallBack d = new TestScrollBarUpdate_CallBack(vsb_TestScroller_ValueChanged);
            this.Invoke(d, new object[] { null, null });

        }

        #endregion
        
        #region Device Tab functions

        /***********************************************************
         * 
         *  The device tab is slightly unique in that the content 
         *  changes depending on what the user is trying to do.  There
         *  are 4 temporary fields used for user imput that change 
         *  depending on the funciton.
         *  
         *  The user needs to understand the device they are communicating
         *  with as the functions are raw and do not validate the user imput
         *  
         * 
         * ********************************************************/


        /// <summary>
        /// This function was started to format the XML message returned
        /// to make it more user readable.  
        /// 
        /// NOT CURRENLTY WORKING
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string FormatResponseMessage(string message)
        {
            string tmpMessage = "";
            int x, y;
            bool insideMessage = false;
            bool messageEndMessageFound = false;
            bool insideMessageSring = false;
            bool addNewLine = false;
            Stack tags = new Stack();
            int endIndex;
            string tmpTag;

            // add a new line before each new element and after each end element
            for (x = 0; x < message.Length; x++)
            {
                if (!insideMessageSring)
                {
                    if (((message[x] == '<') && (message[x + 1] != '/')) && (!insideMessage))
                    {
                        insideMessage = true;
                        messageEndMessageFound = false;

                        if (message.IndexOf(' ', x) != -1)
                        {
                            if (message.IndexOf('>', x) < message.IndexOf(' ', x))
                                endIndex = message.IndexOf('>', x);
                            else
                                endIndex = message.IndexOf(' ', x);
                        }
                        else
                            endIndex = message.IndexOf('>', x);

                        tags.Push(message.Substring(x + 1, (endIndex - (x + 1))));
                    }

                    if ((message[x] == '>') && (insideMessage))
                    {
                        addNewLine = true;
                    }


                    if (((message[x] == '<') && (message[x + 1] == '/')) && (insideMessage))
                    {
                        messageEndMessageFound = true;
                        insideMessage = false;
                        tmpTag = (string)tags.Peek();

                        if (tmpTag == message.Substring(x + 2, (message.IndexOf('>', x) - (x + 2))))
                            tags.Pop();

                    }

                    if ((message[x] == '>') && (messageEndMessageFound))
                    {
                        addNewLine = true;
                    }
                }

                if (message[x] == '\"')
                    insideMessageSring = !insideMessageSring;

                tmpMessage += message[x];

                if (addNewLine)
                {
                    tmpMessage += Environment.NewLine;
                    for (y = 0; y < tags.Count; y++)
                        tmpMessage += " ";
                    addNewLine = false;
                }
            }



            return tmpMessage;

        }


        /// <summary>
        /// Clear the floating device parameters labels and text 
        /// boxes for the next parameter
        /// </summary>
        private void FloatingDeviceParameters_Clear()
        {

            lbl_DeviceMsgField1.Text = "";
            lbl_DeviceMsgField2.Text = "";
            lbl_DeviceMsgField3.Text = "";
            lbl_DeviceMsgField4.Text = "";

            tb_DeviceTab_Field1.Text = "";
            tb_DeviceTab_Field2.Text = "";
            tb_DeviceTab_Field3.Text = "";
            tb_DeviceTab_Field4.Text = "";

            if (LastDeviceTabSendEvent != null)
            {
                this.btn_DeviceSendMessage.Click -= LastDeviceTabSendEvent;
                LastDeviceTabSendEvent = null;
            }

            // unitll this is needed hide the entry fields

            lbl_DeviceMsgField1.Visible = false;
            lbl_DeviceMsgField2.Visible = false;
            lbl_DeviceMsgField3.Visible = false;
            lbl_DeviceMsgField4.Visible = false;

            tb_DeviceTab_Field1.Visible = false;
            tb_DeviceTab_Field2.Visible = false;
            tb_DeviceTab_Field3.Visible = false;
            tb_DeviceTab_Field4.Visible = false;

            btn_DeviceSendMessage.Visible = false;
        }

        /// <summary>
        /// Enable the floating parameters text boxes.  The boxes are enabled by providing a non empty
        /// label.  If the paramX provided is "" the field is turned off.
        /// </summary>
        /// <param name="clickEvent">Provided event handler that will take care of the "Send" click event</param>
        /// <param name="param1">Floating parameter 1</param>
        /// <param name="param2">Floating parameter 2</param>
        /// <param name="param3">Floating parameter 3</param>
        /// <param name="param4">Floating parameter 4</param>
        private void FloatingDeviceParameters_Enable(System.EventHandler clickEvent, string param1, string param2, string param3, string param4)
        {

            lbl_DeviceMsgField1.Text = param1;
            lbl_DeviceMsgField2.Text = param2;
            lbl_DeviceMsgField3.Text = param3;
            lbl_DeviceMsgField4.Text = param4;

            // if the parameter label provide isn't "" turn it on, otherwise leave it off
            if (param1 != "")
            {
                lbl_DeviceMsgField1.Visible = true;
                tb_DeviceTab_Field1.Visible = true;
            }

            if (param2 != "")
            {
                lbl_DeviceMsgField2.Visible = true;
                tb_DeviceTab_Field2.Visible = true;
            }

            if (param3 != "")
            {
                lbl_DeviceMsgField3.Visible = true;
                tb_DeviceTab_Field3.Visible = true;
            }

            if (param4 != "")
            {
                lbl_DeviceMsgField4.Visible = true;
                tb_DeviceTab_Field4.Visible = true;
            }
            // set the "Send" click event handler to the specified handler
            if (clickEvent != null)
            {
                LastDeviceTabSendEvent = clickEvent;
                this.btn_DeviceSendMessage.Click += LastDeviceTabSendEvent;
            }

            btn_DeviceSendMessage.Visible = true;
        }

        /// <summary>
        /// "Send" click event handler for the "Set IP" function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceSetIP_Click(object sender, EventArgs e)
        {
            string message = "", response = "";
            string IP, Token;
            DeviceManagement.GetNetworkInterfacesResponse GNIR;
            bool networkTokenFound = false;
            string buttonText = ((System.Windows.Forms.Button)sender).Text;

            // check to make sure the required fields have been filled in
            if (tb_DeviceTab_Field1.Text == "")
            {
                MessageBox.Show("Please enter network interface token", "ERROR", MessageBoxButtons.OK);
                return;
            }


            if (tb_DeviceTab_Field2.Text == "")
            {
                MessageBox.Show("Please enter device IP address", "ERROR", MessageBoxButtons.OK);
                return;
            }


            ((System.Windows.Forms.Button)sender).Text = "Wait";
            ((System.Windows.Forms.Button)sender).Refresh();
            Cursor.Current = Cursors.WaitCursor;

            tb_DeviceMessages.Text = "";
            tb_DeviceMessages.Refresh();

            IP = tb_DeviceTab_Field2.Text;
            Token = tb_DeviceTab_Field1.Text;

            // first get a list of the network interfaces on the device
            try
            {
                message = TestMessage.Build_GetNetworkInterfaces();
                response = NetworkInterface.POST_Message(Parameters.TestTimeout, tb_DeviceTab_URL.Text, message, Parameters.UserName, Parameters.Password);

                GNIR = (DeviceManagement.GetNetworkInterfacesResponse)TestMessage.Parse_SoapMessage(response, typeof(DeviceManagement.GetNetworkInterfacesResponse));

                // look for the token specified
                if (GNIR.NetworkInterfaces != null)
                {
                    foreach (DeviceManagement.NetworkInterface NI in GNIR.NetworkInterfaces)
                    {
                        if (NI.token == Token)
                        {
                            // update the IP and send
                            DeviceManagement.PrefixedIPv4Address address = new DeviceManagement.PrefixedIPv4Address();
                            address.Address = IP;



                            // update the set configruration
                            DeviceManagement.NetworkInterfaceSetConfiguration NISC = new DeviceManagement.NetworkInterfaceSetConfiguration();
                            NISC.IPv4 = new DeviceManagement.IPv4NetworkInterfaceSetConfiguration();

                            NISC.IPv4.DHCP = false;
                            NISC.IPv4.DHCPSpecified = false;
                            NISC.IPv4.Enabled = true;
                            NISC.IPv4.EnabledSpecified = true;


                            NISC.IPv4.Manual = new DeviceManagement.PrefixedIPv4Address[1];
                            NISC.IPv4.Manual[0] = address;


                            NISC.MTU = NI.Info.MTU;
                            NISC.MTUSpecified = true;

                            NISC.Link = NI.Link.AdminSettings;

                            NISC.Enabled = true;
                            NISC.EnabledSpecified = true;



                            message = TestMessage.Build_SetNetworkInterfaces(Token, NISC);

                            response = NetworkInterface.POST_Message(Parameters.TestTimeout, tb_DeviceTab_URL.Text, message, Parameters.UserName, Parameters.Password);
                            tb_DeviceMessages.Text = "IP Address set, message response" + Environment.NewLine + response;


                            networkTokenFound = true;
                            break;
                        }

                    }

                    if (!networkTokenFound)
                        MessageBox.Show("Network interface matching Token \"" + Token + "\" not found", "ERROR", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Device contains no network interfaces", "ERROR", MessageBoxButtons.OK);

            }
            catch (Exception err)
            {
                tb_DeviceMessages.Text = "Error - " + err.Message;
            }

            ((System.Windows.Forms.Button)sender).Text = buttonText;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// "Send" click event handler for the "Add Profile" function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceAddProfile_Click(object sender, EventArgs e)
        {
            string message = "", response = "";
            string Name, Token;
            Media.CreateProfileResponse CPR;
            //bool networkTokenFound = false;
            string buttonText = ((System.Windows.Forms.Button)sender).Text;

            // make sure the required fields have been entered
            if (tb_DeviceTab_Field1.Text == "")
            {
                MessageBox.Show("Please enter profile name", "ERROR", MessageBoxButtons.OK);
                return;
            }


            if (tb_DeviceTab_Field2.Text == "")
            {
                MessageBox.Show("Please enter profile token", "ERROR", MessageBoxButtons.OK);
                return;
            }

            if ((Parameters.Media_ServiceAddress == "") && !(PollMediaConfigurationAddress()))
            {
                System.Windows.Forms.MessageBox.Show("Unable to retrieve Media Service Address.  Unable to open video stream.", "Error", MessageBoxButtons.OK);
                return;
            }


            ((System.Windows.Forms.Button)sender).Text = "Wait";
            ((System.Windows.Forms.Button)sender).Refresh();
            Cursor.Current = Cursors.WaitCursor;

            tb_DeviceMessages.Text = "";
            tb_DeviceMessages.Refresh();

            Name = tb_DeviceTab_Field2.Text;
            Token = tb_DeviceTab_Field1.Text;

            // first get a list of the network interfaces on the device
            try
            {
                // build the Create Profiles Request
                message = TestMessage.Build_Media_CreateProfileRequest(Name, Token);
                response = NetworkInterface.POST_Message(Parameters.TestTimeout, Parameters.Media_ServiceAddress, message, Parameters.UserName, Parameters.Password);

                CPR = (Media.CreateProfileResponse)TestMessage.Parse_SoapMessage(response, typeof(Media.CreateProfileResponse));

                if (CPR.Profile == null)
                {
                    tb_DeviceMessages.Text = "Create Profile Response does not contain new profile\n" + Environment.NewLine + response;
                    tb_DeviceMessages.Text += "Message received" + Environment.NewLine + response;
                }
                else
                    tb_DeviceMessages.Text = "Message received" + Environment.NewLine + response;




            }
            catch (Exception err)
            {
                tb_DeviceMessages.Text = "Error - " + err.Message;
            }

            ((System.Windows.Forms.Button)sender).Text = buttonText;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Device tab button handler.  Each of the action buttons in this tab
        /// use this function to setup/perform the required action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceTabButton_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            // clear the floating parameters
            FloatingDeviceParameters_Clear();

            // update the device info
            string message = "", response = "";
            string buttonText = ((System.Windows.Forms.Button)sender).Text;

            string URL = tb_DeviceTab_URL.Text;
            string IP = tb_DeviceTab_IP.Text;

            if (((URL == null) || (URL == "")) ||
                ((IP == null) || (IP == "")))
            {
                tb_DeviceMessages.Text = "Target Device Management URL and IP address must both be set";
                return;
            }


            ((System.Windows.Forms.Button)sender).Text = "Please wait";
            ((System.Windows.Forms.Button)sender).Refresh();
            Cursor.Current = Cursors.WaitCursor;

            tb_DeviceMessages.Text = "";

            // find out what button was pressed and action on it
            try
            {
                if (sender == btn_DeviceDeviceInfo)
                {
                    VerifyUserDeviceSelection();
                    message = TestMessage.Build_GetDeviceInformationRequest();
                }

                if (sender == btn_DeviceGetProfiles)
                    message = TestMessage.Build_Media_GetProfilesRequest();

                if (sender == btn_DeviceProbe)
                    message = TestMessage.Build_GetScopesRequest();

                if (sender == btn_DeviceHardReset)
                    message = TestMessage.Build_SetSystemFactoryDefaultRequest(DeviceManagement.FactoryDefaultType.Hard);

                if (sender == btn_DeviceReboot)
                    message = TestMessage.Build_SystemRebootRequest();

                if (sender == btn_DeviceNetworkInterfaces)
                    message = TestMessage.Build_GetNetworkInterfaces();

                if (sender == btn_DeviceIP)
                    FloatingDeviceParameters_Enable(new EventHandler(DeviceSetIP_Click), "Token", "IP Address", "", "");

                if (sender == btn_DeviceCreateProfile)
                    FloatingDeviceParameters_Enable(new EventHandler(DeviceAddProfile_Click), "Name", "Token", "", "");

                if (sender == btn_DeviceGetMediaUrl)
                {
                    PollMediaConfigurationAddress();
                    message = TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.Media });
                }

                if (sender == btn_DeviceGetHostName)
                    message = TestMessage.Build_GetHostnameRequest();

                if (message != "")
                {
                    response = NetworkInterface.POST_Message(Parameters.TestTimeout, tb_DeviceTab_URL.Text, message, Parameters.UserName, Parameters.Password);
                    tb_DeviceMessages.Text = "Message received" + Environment.NewLine + response;
                }

            }
            catch (Exception error)
            {
                tb_DeviceMessages.Text = "Error - " + error.Message;
            }



            ((System.Windows.Forms.Button)sender).Text = buttonText;
            Cursor.Current = Cursors.Default;
        }



        #endregion


        private void UpdateTargetAddress(string oldIP, string newIP)
        {
            if (this.InvokeRequired)
            {
                TestCases_Class.Parameter_UpdateIP d = new TestCases_Class.Parameter_UpdateIP(UpdateTargetAddress);
                this.Invoke(d, new object[] { oldIP, newIP });
            }
            else
            {
                cb_TargetAddress.Text = cb_TargetAddress.Text.Replace(oldIP, newIP);
                TargetAddressTextChange = true;
                cb_TargetAddress_Leave(null, null);
            }

        }
   


        #region INTERNAL TEST CODE

        /****************************************************************
         *                  Internal Test Code area                     *
         *                                                              *
         * This area contains test code used during Form development.   *
         *                                                              *
         * Code in this area will not be accessable by anyone in        *
         * released versions.  #if DEBUG statements will turn off Form  *
         * controls and hide this functionality.  It is usefull to keep *
         * avalible while in debug mode for development                 *
         *                                                              *
         ****************************************************************/                                     

        /// <summary>
        /// WSDL file text box click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_wsdl_file_test_Click(object sender, EventArgs e)
        {
            // open the Open File Dialog and fill the text field as needed
            DialogResult aDialog;
            aDialog = ofd_WSDL.ShowDialog();

            if (aDialog == DialogResult.OK)
            {
                tb_wsdl_file_test.Text = ofd_WSDL.FileName;
                SaveParameter(tb_wsdl_file_test.Name, tb_wsdl_file_test.Text);
            }
        }



        private string aMessage = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\" xmlns:d=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\" xmlns:dn=\"http://www.onvif.org/ver10/network\">" +
                                    "<SOAP-ENV:Header>" +
                                        "<wsa:Action>http://schemas.xmlsoap.org/ws/2005/04/DiscoveryLookup/Probe</wsa:Action>" +
                                        "<wsa:MessageID>1</wsa:MessageID>" +
                                        "<wsa:To>urn:schemas-xmlsoap-org:ws:2005:04:discovery</wsa:To>" +
                                        "<d:AppSequence d:MessageNumber=\"0\" d:InstanceId=\"1234\"/>" +
                                    "</SOAP-ENV:Header>" +
                                    "<SOAP-ENV:Body>" +
                                        "<d:Probe>" +
                                            "<d:Types>NetworkVideoTransmitter</d:Types>" +
                                            "<d:Scopes>onvif://www.onvif.org/type/basic onvif://www.onvif.org/type/analytics onvif://www.onvif.org/type/video onvif://www.onvif.org/name/Bosch onvif://www.onvif.org/location/city/Nuernberg onvif://www.onvif.org/hardware/Dinion-IP-NWC</d:Scopes>" +
                                        "</d:Probe>" +
                                    "</SOAP-ENV:Body>" +
                                "</SOAP-ENV:Envelope>";
        
               
        /// <summary>
        /// Send Message button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendMessage_Click(object sender, EventArgs e)
        {
            string returnString = "";
            string msg = "";
            string url = "http://192.168.1.160/Web_Service";
            //object XML_Object;
            RemoteDiscovery.ScopesType Scope;
            //Media.StreamSetup streamSetup;
            //DeviceManagement.GetScopesResponse GSR;

            //msg = Messanger.Build_HelloRequest

            //NetworkInterface.UDP_ConnectMulticast("239.255.255.250", "192.168.1.160", 3702, 1);
            //NetworkInterface.UDP_Connect("192.168.1.160", 3702, 1);
            //System.Windows.Forms.MessageBox.Show("Start NVT.", "Test", MessageBoxButtons.OK);
            //returnString = NetworkInterface.UDP_Listen(300000);
            //object hello = Messanger.Parse_SoapMessage(returnString, typeof(RemoteDiscovery.HelloType));

            //NetworkInterface.UDP_ConnectMulticast("239.255.255.250", "192.168.1.160", 3702, 1);
            ////NetworkInterface.Connect("239.255.255.250", "192.168.1.160", 3702);

            //Scope = new RemoteDiscovery.ScopesType();
            //Scope.Text = new string[] { "onvif://www.onvif.org/type/super" }; //, " onvif://www.onvif.org/type/analytics ", " onvif://www.onvif.org/type/video", " onvif://www.onvif.org/name/Bosch", " onvif://www.onvif.org/location/city/Nuernberg", " onvif://www.onvif.org/hardware/Dinion-IP-NWC" };
            //Scope.MatchBy = "http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc2396";

            //msg = TestMessage.Build_ProbeRequest("dn:NetworkVideoTransmitter", Scope);
            //returnString = NetworkInterface.UDP_SendMulticast(msg);
            //NetworkInterface.UPD_Close();
            //NetworkInterface.UDP_Connect("192.168.1.160", 3702, 1);
            //NetworkInterface.UDP_Send(msg);
            //NetworkInterface.SendUDP("239.255.255.250", msg, 3702);
            //returnString = NetworkInterface.UDP_Listen(1000);
            returnString = "";
            //returnString = NetworkInterface.UDP_Listen(1000);

            //returnString = NetworkInterface.SendUDP_Multicast("239.255.255.250", "192.168.1.160", msg, 3702);

            //returnString = NetworkInterface.MC_Send("239.255.255.250", "192.168.1.160", msg, 3702);

            //string token = "";

           // msg = TestMessage.Build_GetScopesRequest();
            //msg = TestMessage.Build_GetProfileRequest(token);

            msg = TestMessage.Build_Media_GetProfilesRequest();

            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);

            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));


            msg = TestMessage.Build_Media_GetVideoEncoderConfigurations();
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);
            Media.GetVideoEncoderConfigurationsResponse GVECR = (Media.GetVideoEncoderConfigurationsResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetVideoEncoderConfigurationsResponse));



            msg = TestMessage.Build_Media_GetVideoEncoderConfigurationOptions("8", "");

            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);

            Media.GetVideoEncoderConfigurationOptions GVECO = (Media.GetVideoEncoderConfigurationOptions)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetVideoEncoderConfigurationOptions));

            //Media.GetProfilesResponse GPR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));

            //if (GPR.Profiles != null)
            //{
            //    foreach (Media.Profile Profile in GPR.Profiles)
            //    {

            //        streamSetup = new Media.StreamSetup();
            //        streamSetup.Stream = new Media.StreamType();
            //        streamSetup.Transport = new Media.Transport();

            //        msg = TestMessage.Build_GetStreamUriRequest(Profile.token, streamSetup);

            //        try
            //        {
            //            returnString = NetworkInterface.POST_Message(url, msg);

            //            GSR = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(DeviceManagement.GetScopesResponse));
            //        }
            //        catch (Exception err)
            //        {
            //            Console.WriteLine(" ERROR - " + err.Message);
            //        }

            //    }
            //}

            //streamSetup = new Media.StreamSetup();
            //streamSetup.Stream = new Media.StreamType();
            //streamSetup.Transport = new Media.Transport();

            //msg = TestMessage.Build_GetStreamUriRequest("0", streamSetup);
            
            //returnString = NetworkInterface.POST_Message(url, msg);

            //GSR = (DeviceManagement.GetScopesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(DeviceManagement.GetScopesResponse));

            

            //returnString = NetworkInterface.SendMessage(url, msg);
            //returnString = NetworkInterface.UDP_Listen(300000);
            


            //returnString = "";
            //returnString = NetworkInterface.Listen("239.255.255.250", 3702, 1, 300000);
            //try
            //{
            //    RemoteDiscovery.HelloType HelloCompare = new RemoteDiscovery.HelloType();
            //    //Messanger.Verifiy_HelloResponse(returnString, null, ref msg);
            //}
            //catch { }


            DeviceManagement.CapabilityCategory[] capibility = new DeviceManagement.CapabilityCategory[1];
            capibility[0] = DeviceManagement.CapabilityCategory.All;

            //DeviceManagement.ProbeType PT;

            //PT = (DeviceManagement.ProbeType) Messanger.Parse_SoapMessage(msg, typeof(DeviceManagement.ProbeType));

            Scope = new RemoteDiscovery.ScopesType();
            Scope.Text = new string[] { " onvif://www.onvif.org/type/basic ", " onvif://www.onvif.org/type/analytics  ", " onvif://www.onvif.org/type/video ", " onvif://www.onvif.org/name/Bosch ", " onvif://www.onvif.org/location/city/Nuernberg ", " onvif://www.onvif.org/hardware/Dinion-IP-NWC  " };
            msg = TestMessage.Build_ProbeRequest("NetworkVideoTransmitter", Scope);
            returnString = "";
            //returnString = NetworkInterface.SendUDP("192.168.1.160", msg, 3702);

            returnString = "";
            //returnString = NetworkInterface.SendUDP("192.168.1.160", aMessage, 3702);

            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", aMessage, Parameters.UserName, Parameters.Password);

            msg = TestMessage.Build_GetDeviceInformationRequest();
            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", msg, Parameters.UserName, Parameters.Password);

            msg = TestMessage.Build_GetCapabilitiesRequest(capibility);
            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", msg, Parameters.UserName, Parameters.Password);

            msg = TestMessage.Build_GetWsdlUrlRequest();
            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", msg, Parameters.UserName, Parameters.Password);

            msg = TestMessage.Build_GetHostnameRequest();
            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", msg, Parameters.UserName, Parameters.Password);

            //string action = "http://schemas.xmlsoap.org/ws/2005/04/DiscoveryLookup/Probe";
            

            


            //returnString = "";
            //returnString = Inteface.SendMessage(url, action, msg);

            //Inteface.send(msg, url);

            //returnString = "";
            //returnString = Inteface.sendUDP("192.168.1.160", msg);

            //TestSoapMessageConstruct();

            //msg = Messanger.Build_ProbeRequest("Any", new RemoteDiscovery.ScopesType());
            //returnString = "";
            //returnString = Inteface.sendUDP("192.168.1.160", aMessage, 3702);
            //returnString = "";
            //returnString = Inteface.send(aMessage2, "http://192.168.1.160/Web_Service");

            //returnString = "";
            //returnString = Inteface.SendMessage("http://192.168.1.160/onvif/device_service", "", aMessage2);
            
            //returnString = "";
            //returnString = Inteface.SendMessage("http://192.168.1.160/Web_Service", "", aMessage2);

            returnString = "";
            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, "http://192.168.1.160/Web_Service", aMessage, Parameters.UserName, Parameters.Password);


        

            //returnString = "";
            //returnString = NetworkInterface.send(aMessage2, "http://192.168.1.160/Web_Service");

            //returnString = "";
            //returnString = NetworkInterface.send(aMessage2, "http://192.168.1.160:143");

            //returnString = "";
            //returnString = NetworkInterface.send(aMessage2, "http://192.168.1.160:443");

            //returnString = "";
            //returnString = NetworkInterface.send(aMessage2, "http://192.168.1.160:554");


            return;

            //if (tb_wsdl_file_test.Text.Equals(""))
            //    return;


            //Messanger.Build_GetScopesRequest("http://192.168.1.102");
            //Messanger.testMessage(tb_wsdl_file_test.Text);

            //Messanger.Build_ScopesRequiest();
            //try
            //{
            //    RemoteDiscovery.ScopesType scope = new RemoteDiscovery.ScopesType();
            //    scope.MatchBy = "Match by this";
            //    scope.Text = new string[] { "first text", "second text", "last text" };
            //    string message = TestMessage.Build_ProbeRequest("sometypes", scope);
            //    //RemoteDiscovery.ProbeType PT = Messanger.Parse_ProbeRequest(message);
            //}
            //catch (Exception exception)
            //{
            //    System.Windows.Forms.MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK);
            //}

        }

        private string TestSoapMessageConstruct()
        {
            string messageStrings = "";

            messageStrings += TestMessage.Build_AddScopesRequest(new string[] {"item 1", "item 2"});
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_AddVideoEncoderConfigurationRequest("Token here", "Profile here");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_AddVideoSourceConfigurationRequest("Config Token", "a Profile");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_ByeRequest(new RemoteDiscovery.EndpointReferenceType(), 1, true, new RemoteDiscovery.ScopesType(), "some types", "www.cool.com");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_CreateProfileRequest("name here", "token here");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_DeleteProfileRequest("delte this profile token");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_DeleteScopesRequest(new string[] { "delete this scope" });
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetCapabilitiesRequest(new DeviceManagement.CapabilityCategory[] { DeviceManagement.CapabilityCategory.All });
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetDeviceInformationRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetDNSRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetHostnameRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetNTPRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_GetProfileRequest("get this profile token");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_GetProfilesRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetScopesRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_GetStreamUriRequest("stream token", new Media.StreamSetup());
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetSystemDateAndTimeRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_GetVideoEncoderConfigurationRequest("config token");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_GetWsdlUrlRequest();
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_HelloRequest(new RemoteDiscovery.EndpointReferenceType(), 2, new RemoteDiscovery.ScopesType(), "get teyps", "x address");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_ProbeRequest("probe types", new RemoteDiscovery.ScopesType());
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_RemoveVideoEncoderConfigurationRequest("remove token");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_RemoveVideoSourceConfigurationRequest("remove source");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetDNSRequest(new DeviceManagement.IPAddress[1], new string[] { "domain 1", "dm=www.google.com" });
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetHostnameRequest("host name");
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetNTPRequest(true, new DeviceManagement.NetworkHost[1]);
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetScopesRequest(new string[] { "some scope" });
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetSystemDateAndTimeRequest(DeviceManagement.SetDateTimeType.Manual, true, new DeviceManagement.TimeZone(), new DeviceManagement.DateTime());
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SetSystemFactoryDefaultRequest(DeviceManagement.FactoryDefaultType.Hard);
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_Media_SetVideoEncoderConfigurationRequest(new Media.VideoEncoderConfiguration(), true);
            messageStrings += Environment.NewLine;

            messageStrings += TestMessage.Build_SystemRebootRequest();
            messageStrings += Environment.NewLine;



            return messageStrings;        

        }

        private void tb_SoapOutput_Click(object sender, EventArgs e)
        {
            // open the Open File Dialog and fill the text field as needed
            DialogResult aDialog;
            aDialog = sfd_TxtOutput.ShowDialog();

            if (aDialog == DialogResult.OK)
            {
                tb_SoapOutput.Text = sfd_TxtOutput.FileName;
                SaveParameter(tb_SoapOutput.Name, tb_SoapOutput.Text);
            }

        }

        private void btn_SoapSave_Click(object sender, EventArgs e)
        {

            string returnString = "";
            string msg = "";
            string url = "http://192.168.1.160/onvif/device_service";
            //object XML_Object;
            //RemoteDiscovery.ScopesType Scope;
            //Media.StreamSetup streamSetup;
            //DeviceManagement.GetScopesResponse GSR;

            msg = TestMessage.Build_Media_GetProfilesRequest();

            returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);

            Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));


        }

        private void saveToFile(string fileName, string data)
        {
            System.IO.FileStream saveFile;
            System.IO.FileInfo fi;
            
            byte[] fileBytes = Encoding.ASCII.GetBytes(data);

            fi = new System.IO.FileInfo(fileName);

            if (fi.Exists)
                fi = new System.IO.FileInfo(fileName);

            saveFile = fi.Create();

            // save file 
            for (int y = 0; y < fileBytes.Length && saveFile.CanRead; y++)
                saveFile.WriteByte(fileBytes[y]);

            saveFile.Close();
        }

        private void btn_streamURI_Click(object sender, EventArgs e)
        {
            Vid = new Video();

            StartVideoForm();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string returnString = "";
            string msg = "";
            string url = Parameters.URL;

            //Vid.SetDisplayMode_Test(true);
            Cursor.Current = Cursors.WaitCursor;

            // get profiles
            try
            {
                msg = TestMessage.Build_Media_GetProfilesRequest();
                returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);
                Media.GetProfilesResponse GPsR = (Media.GetProfilesResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetProfilesResponse));


                // Set Video Encoder Configuration
                Media.SetVideoEncoderConfiguration SVEC = new Media.SetVideoEncoderConfiguration();
                SVEC.ForcePersistence = false;

                SVEC.Configuration = GPsR.Profiles[0].VideoEncoderConfiguration;
                SVEC.Configuration.Encoding = Media.VideoEncoding.JPEG;

                // get the stream URI
                Media.GetStreamUri GSUri = new Media.GetStreamUri();

                GSUri.ProfileToken = GPsR.Profiles[0].token;

                GSUri.StreamSetup = new Media.StreamSetup();
                GSUri.StreamSetup.Transport = new Media.Transport();
                GSUri.StreamSetup.Transport.Protocol = Media.TransportProtocol.HTTP;
                GSUri.StreamSetup.Stream = Media.StreamType.RTPUnicast;

                msg = TestMessage.Build_Media_GetStreamUriRequest(GSUri);

                returnString = NetworkInterface.POST_Message(Parameters.TestTimeout, url, msg, Parameters.UserName, Parameters.Password);

                Media.GetStreamUriResponse GSUriR = (Media.GetStreamUriResponse)TestMessage.Parse_SoapMessage(returnString, typeof(Media.GetStreamUriResponse));



                MessageBox.Show("HTTP Stream URI = \"" + GSUriR.MediaUri.Uri + "\"", "URI", MessageBoxButtons.OK);
            }
            catch (Exception err)
            {
                MessageBox.Show("ERROR - " + err.Message, "ERROR", MessageBoxButtons.OK);
            }

            Cursor.Current = Cursors.Default;

            //if (GSUriR.MediaUri.Uri.Contains("&line=0"))
            //    GSUriR.MediaUri.Uri = GSUriR.MediaUri.Uri.Replace("&line=0", "");

            //Vid.InitVideoStream(GSUriR.MediaUri.Uri);
            //Vid.Query_RTSP_Options(out returnString, out Parameters.RTSP_Response);
            //Vid.Query_RTSP_Describe(out returnString, out Parameters.RTSP_Response);
            //Vid.Query_RTSP_Setup(out returnString, out Parameters.RTSP_Response, "RTP/AVP/UDP", "unicast");

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn_PassAllTests_Click(object sender, EventArgs e)
        {
            int x, y;

            for(x = 0; x < Tests.Group.Length; x++)            
            {
                for(y = 0; y < Tests.Group[x].Tests.Length; y++)                
                {
                    if ((Tests.Group[x].Tests[y].Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Must) ||
                       (Tests.Group[x].Tests[y].Compliance == ONVIF_TestCases.TestCases_Class.TestCompliance.Must_if_Supported))
                        Tests.Group[x].Tests[y].TestComplete = true;
                    else
                    {
                        Tests.Group[x].Tests[y].TestComplete = true;
                        Tests.Group[x].Tests[y].TestSkipped = true;
                    }

                }
            }
        }

        #endregion

        





        

    }

     
    
}
