namespace CameraClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxCamera = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tcModes = new System.Windows.Forms.TabControl();
            this.tabPageProxy = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbMethods = new System.Windows.Forms.GroupBox();
            this.listViewMethods = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCall = new System.Windows.Forms.Button();
            this.tbConsole = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tabPageDirectCall = new System.Windows.Forms.TabPage();
            this.SetAccessPolicy = new System.Windows.Forms.Button();
            this.SetZeroConfiguration = new System.Windows.Forms.Button();
            this.btnSetClientCertificateMode = new System.Windows.Forms.Button();
            this.btnGetClientSertificateMode = new System.Windows.Forms.Button();
            this.comboBoxDiscoveryMode = new System.Windows.Forms.ComboBox();
            this.btnSetDiscoveryMode = new System.Windows.Forms.Button();
            this.btnGetDiscoveryMode = new System.Windows.Forms.Button();
            this.tabPageDiscoveryTest = new System.Windows.Forms.TabPage();
            this.btnHalt = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewTrace = new System.Windows.Forms.ListView();
            this.tcStepDetails = new System.Windows.Forms.TabControl();
            this.tabPageRequest = new System.Windows.Forms.TabPage();
            this.richTextBoxStepRequest = new System.Windows.Forms.RichTextBox();
            this.tabPageResponse = new System.Windows.Forms.TabPage();
            this.richTextBoxStepAnswer = new System.Windows.Forms.RichTextBox();
            this.tabPageExceptions = new System.Windows.Forms.TabPage();
            this.richTextBoxException = new System.Windows.Forms.RichTextBox();
            this.treeViewTests = new System.Windows.Forms.TreeView();
            this.btnDiscoveryTest = new System.Windows.Forms.Button();
            this.tabPageTrace = new System.Windows.Forms.TabPage();
            this.btnClearTrace = new System.Windows.Forms.Button();
            this.tbTrace = new System.Windows.Forms.RichTextBox();
            this.tabPageSoap = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbResponse = new System.Windows.Forms.RichTextBox();
            this.buttonRest = new System.Windows.Forms.Button();
            this.cbShowResult = new System.Windows.Forms.CheckBox();
            this.cbTrace = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ourTimer = new System.Windows.Forms.Timer(this.components);
            this.cbLowLevelTrace = new System.Windows.Forms.CheckBox();
            this.cbHttps = new System.Windows.Forms.CheckBox();
            this.groupBoxCamera.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tcModes.SuspendLayout();
            this.tabPageProxy.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbMethods.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageDirectCall.SuspendLayout();
            this.tabPageDiscoveryTest.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tcStepDetails.SuspendLayout();
            this.tabPageRequest.SuspendLayout();
            this.tabPageResponse.SuspendLayout();
            this.tabPageExceptions.SuspendLayout();
            this.tabPageTrace.SuspendLayout();
            this.tabPageSoap.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCamera
            // 
            this.groupBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCamera.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxCamera.Location = new System.Drawing.Point(13, 13);
            this.groupBoxCamera.Name = "groupBoxCamera";
            this.groupBoxCamera.Size = new System.Drawing.Size(801, 87);
            this.groupBoxCamera.TabIndex = 0;
            this.groupBoxCamera.TabStop = false;
            this.groupBoxCamera.Text = "Camera";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tbAddress, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbUser, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbPassword, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(795, 68);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // tbAddress
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tbAddress, 3);
            this.tbAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAddress.Location = new System.Drawing.Point(98, 3);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(694, 20);
            this.tbAddress.TabIndex = 1;
            this.tbAddress.Text = "http://192.168.3.107/onvif/device_service";
            // 
            // tbUser
            // 
            this.tbUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUser.Location = new System.Drawing.Point(98, 37);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(309, 20);
            this.tbUser.TabIndex = 3;
            // 
            // tbPassword
            // 
            this.tbPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPassword.Location = new System.Drawing.Point(483, 37);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(309, 20);
            this.tbPassword.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "User:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(413, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // tcModes
            // 
            this.tcModes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcModes.Controls.Add(this.tabPageProxy);
            this.tcModes.Controls.Add(this.tabPageDirectCall);
            this.tcModes.Controls.Add(this.tabPageDiscoveryTest);
            this.tcModes.Controls.Add(this.tabPageTrace);
            this.tcModes.Controls.Add(this.tabPageSoap);
            this.tcModes.Location = new System.Drawing.Point(13, 130);
            this.tcModes.Margin = new System.Windows.Forms.Padding(13);
            this.tcModes.Name = "tcModes";
            this.tcModes.SelectedIndex = 0;
            this.tcModes.Size = new System.Drawing.Size(801, 408);
            this.tcModes.TabIndex = 1;
            // 
            // tabPageProxy
            // 
            this.tabPageProxy.Controls.Add(this.tableLayoutPanel1);
            this.tabPageProxy.Location = new System.Drawing.Point(4, 22);
            this.tabPageProxy.Name = "tabPageProxy";
            this.tabPageProxy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProxy.Size = new System.Drawing.Size(793, 382);
            this.tabPageProxy.TabIndex = 0;
            this.tabPageProxy.Text = "Use Proxy";
            this.tabPageProxy.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.gbMethods, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(787, 376);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // gbMethods
            // 
            this.gbMethods.Controls.Add(this.listViewMethods);
            this.gbMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMethods.Location = new System.Drawing.Point(3, 3);
            this.gbMethods.Name = "gbMethods";
            this.gbMethods.Size = new System.Drawing.Size(308, 370);
            this.gbMethods.TabIndex = 4;
            this.gbMethods.TabStop = false;
            this.gbMethods.Text = "Methods";
            // 
            // listViewMethods
            // 
            this.listViewMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMethods.FullRowSelect = true;
            this.listViewMethods.Location = new System.Drawing.Point(3, 16);
            this.listViewMethods.MultiSelect = false;
            this.listViewMethods.Name = "listViewMethods";
            this.listViewMethods.Size = new System.Drawing.Size(302, 351);
            this.listViewMethods.TabIndex = 0;
            this.listViewMethods.UseCompatibleStateImageBehavior = false;
            this.listViewMethods.View = System.Windows.Forms.View.Details;
            this.listViewMethods.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewMethods_MouseDoubleClick);
            this.listViewMethods.SelectedIndexChanged += new System.EventHandler(this.listViewMethods_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCall);
            this.panel1.Controls.Add(this.tbConsole);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(317, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 370);
            this.panel1.TabIndex = 5;
            // 
            // btnCall
            // 
            this.btnCall.Enabled = false;
            this.btnCall.Location = new System.Drawing.Point(9, 3);
            this.btnCall.Name = "btnCall";
            this.btnCall.Size = new System.Drawing.Size(75, 23);
            this.btnCall.TabIndex = 1;
            this.btnCall.Text = "Call";
            this.btnCall.UseVisualStyleBackColor = true;
            this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
            // 
            // tbConsole
            // 
            this.tbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsole.Location = new System.Drawing.Point(3, 32);
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.Size = new System.Drawing.Size(456, 338);
            this.tbConsole.TabIndex = 2;
            this.tbConsole.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(384, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabPageDirectCall
            // 
            this.tabPageDirectCall.Controls.Add(this.SetAccessPolicy);
            this.tabPageDirectCall.Controls.Add(this.SetZeroConfiguration);
            this.tabPageDirectCall.Controls.Add(this.btnSetClientCertificateMode);
            this.tabPageDirectCall.Controls.Add(this.btnGetClientSertificateMode);
            this.tabPageDirectCall.Controls.Add(this.comboBoxDiscoveryMode);
            this.tabPageDirectCall.Controls.Add(this.btnSetDiscoveryMode);
            this.tabPageDirectCall.Controls.Add(this.btnGetDiscoveryMode);
            this.tabPageDirectCall.Location = new System.Drawing.Point(4, 22);
            this.tabPageDirectCall.Name = "tabPageDirectCall";
            this.tabPageDirectCall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDirectCall.Size = new System.Drawing.Size(793, 382);
            this.tabPageDirectCall.TabIndex = 2;
            this.tabPageDirectCall.Text = "Direct Call";
            this.tabPageDirectCall.UseVisualStyleBackColor = true;
            // 
            // SetAccessPolicy
            // 
            this.SetAccessPolicy.Location = new System.Drawing.Point(7, 179);
            this.SetAccessPolicy.Name = "SetAccessPolicy";
            this.SetAccessPolicy.Size = new System.Drawing.Size(135, 23);
            this.SetAccessPolicy.TabIndex = 11;
            this.SetAccessPolicy.Text = "SetAccessPolicy";
            this.SetAccessPolicy.UseVisualStyleBackColor = true;
            this.SetAccessPolicy.Click += new System.EventHandler(this.SetAccessPolicy_Click);
            // 
            // SetZeroConfiguration
            // 
            this.SetZeroConfiguration.Location = new System.Drawing.Point(7, 149);
            this.SetZeroConfiguration.Name = "SetZeroConfiguration";
            this.SetZeroConfiguration.Size = new System.Drawing.Size(135, 23);
            this.SetZeroConfiguration.TabIndex = 10;
            this.SetZeroConfiguration.Text = "SetZeroConfiguration";
            this.SetZeroConfiguration.UseVisualStyleBackColor = true;
            this.SetZeroConfiguration.Click += new System.EventHandler(this.SetZeroConfiguration_Click);
            // 
            // btnSetClientCertificateMode
            // 
            this.btnSetClientCertificateMode.Location = new System.Drawing.Point(7, 119);
            this.btnSetClientCertificateMode.Name = "btnSetClientCertificateMode";
            this.btnSetClientCertificateMode.Size = new System.Drawing.Size(135, 23);
            this.btnSetClientCertificateMode.TabIndex = 9;
            this.btnSetClientCertificateMode.Text = "SetClientCertificateMode";
            this.btnSetClientCertificateMode.UseVisualStyleBackColor = true;
            this.btnSetClientCertificateMode.Click += new System.EventHandler(this.btnSetClientCertificateMode_Click);
            // 
            // btnGetClientSertificateMode
            // 
            this.btnGetClientSertificateMode.Location = new System.Drawing.Point(7, 89);
            this.btnGetClientSertificateMode.Name = "btnGetClientSertificateMode";
            this.btnGetClientSertificateMode.Size = new System.Drawing.Size(135, 23);
            this.btnGetClientSertificateMode.TabIndex = 8;
            this.btnGetClientSertificateMode.Text = "GetClientSertificateMode";
            this.btnGetClientSertificateMode.UseVisualStyleBackColor = true;
            this.btnGetClientSertificateMode.Click += new System.EventHandler(this.btnGetClientSertificateMode_Click);
            // 
            // comboBoxDiscoveryMode
            // 
            this.comboBoxDiscoveryMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDiscoveryMode.FormattingEnabled = true;
            this.comboBoxDiscoveryMode.Location = new System.Drawing.Point(151, 37);
            this.comboBoxDiscoveryMode.Name = "comboBoxDiscoveryMode";
            this.comboBoxDiscoveryMode.Size = new System.Drawing.Size(154, 21);
            this.comboBoxDiscoveryMode.TabIndex = 7;
            // 
            // btnSetDiscoveryMode
            // 
            this.btnSetDiscoveryMode.Location = new System.Drawing.Point(6, 35);
            this.btnSetDiscoveryMode.Name = "btnSetDiscoveryMode";
            this.btnSetDiscoveryMode.Size = new System.Drawing.Size(136, 23);
            this.btnSetDiscoveryMode.TabIndex = 6;
            this.btnSetDiscoveryMode.Text = "Set Discovery Mode";
            this.btnSetDiscoveryMode.UseVisualStyleBackColor = true;
            this.btnSetDiscoveryMode.Click += new System.EventHandler(this.btnSetDiscoveryMode_Click);
            // 
            // btnGetDiscoveryMode
            // 
            this.btnGetDiscoveryMode.Location = new System.Drawing.Point(6, 6);
            this.btnGetDiscoveryMode.Name = "btnGetDiscoveryMode";
            this.btnGetDiscoveryMode.Size = new System.Drawing.Size(136, 23);
            this.btnGetDiscoveryMode.TabIndex = 5;
            this.btnGetDiscoveryMode.Text = "Get Discovery Mode";
            this.btnGetDiscoveryMode.UseVisualStyleBackColor = true;
            this.btnGetDiscoveryMode.Click += new System.EventHandler(this.btnGetDiscoveryMode_Click);
            // 
            // tabPageDiscoveryTest
            // 
            this.tabPageDiscoveryTest.Controls.Add(this.btnHalt);
            this.tabPageDiscoveryTest.Controls.Add(this.btnPause);
            this.tabPageDiscoveryTest.Controls.Add(this.btnStop);
            this.tabPageDiscoveryTest.Controls.Add(this.tableLayoutPanel4);
            this.tabPageDiscoveryTest.Controls.Add(this.btnDiscoveryTest);
            this.tabPageDiscoveryTest.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiscoveryTest.Name = "tabPageDiscoveryTest";
            this.tabPageDiscoveryTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiscoveryTest.Size = new System.Drawing.Size(793, 382);
            this.tabPageDiscoveryTest.TabIndex = 4;
            this.tabPageDiscoveryTest.Text = "Run Sample Test";
            this.tabPageDiscoveryTest.UseVisualStyleBackColor = true;
            // 
            // btnHalt
            // 
            this.btnHalt.Enabled = false;
            this.btnHalt.Location = new System.Drawing.Point(250, 6);
            this.btnHalt.Name = "btnHalt";
            this.btnHalt.Size = new System.Drawing.Size(75, 23);
            this.btnHalt.TabIndex = 4;
            this.btnHalt.Text = "Halt";
            this.btnHalt.UseVisualStyleBackColor = true;
            this.btnHalt.Click += new System.EventHandler(this.btnHalt_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(88, 6);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(169, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.listViewTrace, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.tcStepDetails, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.treeViewTests, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(7, 37);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(780, 342);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // listViewTrace
            // 
            this.listViewTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTrace.FullRowSelect = true;
            this.listViewTrace.Location = new System.Drawing.Point(393, 3);
            this.listViewTrace.MultiSelect = false;
            this.listViewTrace.Name = "listViewTrace";
            this.listViewTrace.Size = new System.Drawing.Size(384, 165);
            this.listViewTrace.TabIndex = 0;
            this.listViewTrace.UseCompatibleStateImageBehavior = false;
            this.listViewTrace.View = System.Windows.Forms.View.Details;
            this.listViewTrace.SelectedIndexChanged += new System.EventHandler(this.listViewTrace_SelectedIndexChanged);
            // 
            // tcStepDetails
            // 
            this.tcStepDetails.Controls.Add(this.tabPageRequest);
            this.tcStepDetails.Controls.Add(this.tabPageResponse);
            this.tcStepDetails.Controls.Add(this.tabPageExceptions);
            this.tcStepDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcStepDetails.Location = new System.Drawing.Point(393, 174);
            this.tcStepDetails.Name = "tcStepDetails";
            this.tcStepDetails.SelectedIndex = 0;
            this.tcStepDetails.Size = new System.Drawing.Size(384, 165);
            this.tcStepDetails.TabIndex = 3;
            // 
            // tabPageRequest
            // 
            this.tabPageRequest.Controls.Add(this.richTextBoxStepRequest);
            this.tabPageRequest.Location = new System.Drawing.Point(4, 22);
            this.tabPageRequest.Name = "tabPageRequest";
            this.tabPageRequest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRequest.Size = new System.Drawing.Size(376, 139);
            this.tabPageRequest.TabIndex = 0;
            this.tabPageRequest.Text = "Request";
            this.tabPageRequest.UseVisualStyleBackColor = true;
            // 
            // richTextBoxStepRequest
            // 
            this.richTextBoxStepRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxStepRequest.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxStepRequest.Name = "richTextBoxStepRequest";
            this.richTextBoxStepRequest.Size = new System.Drawing.Size(370, 133);
            this.richTextBoxStepRequest.TabIndex = 2;
            this.richTextBoxStepRequest.Text = "";
            // 
            // tabPageResponse
            // 
            this.tabPageResponse.Controls.Add(this.richTextBoxStepAnswer);
            this.tabPageResponse.Location = new System.Drawing.Point(4, 22);
            this.tabPageResponse.Name = "tabPageResponse";
            this.tabPageResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResponse.Size = new System.Drawing.Size(376, 139);
            this.tabPageResponse.TabIndex = 1;
            this.tabPageResponse.Text = "Response";
            this.tabPageResponse.UseVisualStyleBackColor = true;
            // 
            // richTextBoxStepAnswer
            // 
            this.richTextBoxStepAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxStepAnswer.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxStepAnswer.Name = "richTextBoxStepAnswer";
            this.richTextBoxStepAnswer.Size = new System.Drawing.Size(370, 133);
            this.richTextBoxStepAnswer.TabIndex = 3;
            this.richTextBoxStepAnswer.Text = "";
            // 
            // tabPageExceptions
            // 
            this.tabPageExceptions.Controls.Add(this.richTextBoxException);
            this.tabPageExceptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageExceptions.Name = "tabPageExceptions";
            this.tabPageExceptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExceptions.Size = new System.Drawing.Size(376, 139);
            this.tabPageExceptions.TabIndex = 2;
            this.tabPageExceptions.Text = "Exceptions";
            this.tabPageExceptions.UseVisualStyleBackColor = true;
            // 
            // richTextBoxException
            // 
            this.richTextBoxException.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxException.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxException.Name = "richTextBoxException";
            this.richTextBoxException.Size = new System.Drawing.Size(370, 133);
            this.richTextBoxException.TabIndex = 0;
            this.richTextBoxException.Text = "";
            // 
            // treeViewTests
            // 
            this.treeViewTests.CheckBoxes = true;
            this.treeViewTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTests.Location = new System.Drawing.Point(3, 3);
            this.treeViewTests.Name = "treeViewTests";
            this.tableLayoutPanel4.SetRowSpan(this.treeViewTests, 2);
            this.treeViewTests.Size = new System.Drawing.Size(384, 336);
            this.treeViewTests.TabIndex = 4;
            this.treeViewTests.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTests_AfterCheck);
            // 
            // btnDiscoveryTest
            // 
            this.btnDiscoveryTest.Enabled = false;
            this.btnDiscoveryTest.Location = new System.Drawing.Point(7, 7);
            this.btnDiscoveryTest.Name = "btnDiscoveryTest";
            this.btnDiscoveryTest.Size = new System.Drawing.Size(75, 23);
            this.btnDiscoveryTest.TabIndex = 0;
            this.btnDiscoveryTest.Text = "Run";
            this.btnDiscoveryTest.UseVisualStyleBackColor = true;
            this.btnDiscoveryTest.Click += new System.EventHandler(this.btnDiscoveryTest_Click);
            // 
            // tabPageTrace
            // 
            this.tabPageTrace.Controls.Add(this.btnClearTrace);
            this.tabPageTrace.Controls.Add(this.tbTrace);
            this.tabPageTrace.Location = new System.Drawing.Point(4, 22);
            this.tabPageTrace.Name = "tabPageTrace";
            this.tabPageTrace.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrace.Size = new System.Drawing.Size(793, 382);
            this.tabPageTrace.TabIndex = 3;
            this.tabPageTrace.Text = "Trace";
            this.tabPageTrace.UseVisualStyleBackColor = true;
            // 
            // btnClearTrace
            // 
            this.btnClearTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTrace.Location = new System.Drawing.Point(712, 6);
            this.btnClearTrace.Name = "btnClearTrace";
            this.btnClearTrace.Size = new System.Drawing.Size(75, 23);
            this.btnClearTrace.TabIndex = 1;
            this.btnClearTrace.Text = "Clear";
            this.btnClearTrace.UseVisualStyleBackColor = true;
            this.btnClearTrace.Click += new System.EventHandler(this.btnClearTrace_Click);
            // 
            // tbTrace
            // 
            this.tbTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTrace.Location = new System.Drawing.Point(7, 35);
            this.tbTrace.Name = "tbTrace";
            this.tbTrace.Size = new System.Drawing.Size(780, 345);
            this.tbTrace.TabIndex = 0;
            this.tbTrace.Text = "";
            // 
            // tabPageSoap
            // 
            this.tabPageSoap.Controls.Add(this.tableLayoutPanel3);
            this.tabPageSoap.Location = new System.Drawing.Point(4, 22);
            this.tabPageSoap.Name = "tabPageSoap";
            this.tabPageSoap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSoap.Size = new System.Drawing.Size(793, 382);
            this.tabPageSoap.TabIndex = 1;
            this.tabPageSoap.Text = "SOAP";
            this.tabPageSoap.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.tbRequest, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSend, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbResponse, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonRest, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(787, 376);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tbRequest
            // 
            this.tbRequest.AllowDrop = true;
            this.tbRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRequest.Location = new System.Drawing.Point(3, 3);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tableLayoutPanel3.SetRowSpan(this.tbRequest, 2);
            this.tbRequest.Size = new System.Drawing.Size(361, 370);
            this.tbRequest.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(370, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(46, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbResponse
            // 
            this.tbResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResponse.Location = new System.Drawing.Point(423, 3);
            this.tbResponse.Name = "tbResponse";
            this.tableLayoutPanel3.SetRowSpan(this.tbResponse, 2);
            this.tbResponse.Size = new System.Drawing.Size(361, 370);
            this.tbResponse.TabIndex = 2;
            this.tbResponse.Text = "";
            // 
            // buttonRest
            // 
            this.buttonRest.Location = new System.Drawing.Point(370, 39);
            this.buttonRest.Name = "buttonRest";
            this.buttonRest.Size = new System.Drawing.Size(47, 24);
            this.buttonRest.TabIndex = 3;
            this.buttonRest.Text = "REST";
            this.buttonRest.UseVisualStyleBackColor = true;
            this.buttonRest.Click += new System.EventHandler(this.buttonRest_Click);
            // 
            // cbShowResult
            // 
            this.cbShowResult.AutoSize = true;
            this.cbShowResult.Location = new System.Drawing.Point(13, 106);
            this.cbShowResult.Name = "cbShowResult";
            this.cbShowResult.Size = new System.Drawing.Size(86, 17);
            this.cbShowResult.TabIndex = 3;
            this.cbShowResult.Text = "Show Result";
            this.cbShowResult.UseVisualStyleBackColor = true;
            // 
            // cbTrace
            // 
            this.cbTrace.AutoSize = true;
            this.cbTrace.Checked = true;
            this.cbTrace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrace.Location = new System.Drawing.Point(105, 106);
            this.cbTrace.Name = "cbTrace";
            this.cbTrace.Size = new System.Drawing.Size(54, 17);
            this.cbTrace.TabIndex = 4;
            this.cbTrace.Text = "Trace";
            this.cbTrace.UseVisualStyleBackColor = true;
            this.cbTrace.CheckedChanged += new System.EventHandler(this.cbTrace_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(826, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // ourTimer
            // 
            this.ourTimer.Interval = 10000;
            this.ourTimer.Tick += new System.EventHandler(this.ourTimer_Tick);
            // 
            // cbLowLevelTrace
            // 
            this.cbLowLevelTrace.AutoSize = true;
            this.cbLowLevelTrace.Location = new System.Drawing.Point(165, 106);
            this.cbLowLevelTrace.Name = "cbLowLevelTrace";
            this.cbLowLevelTrace.Size = new System.Drawing.Size(98, 17);
            this.cbLowLevelTrace.TabIndex = 5;
            this.cbLowLevelTrace.Text = "Low level trace";
            this.cbLowLevelTrace.UseVisualStyleBackColor = true;
            // 
            // cbHttps
            // 
            this.cbHttps.AutoSize = true;
            this.cbHttps.Location = new System.Drawing.Point(270, 106);
            this.cbHttps.Name = "cbHttps";
            this.cbHttps.Size = new System.Drawing.Size(109, 17);
            this.cbHttps.TabIndex = 6;
            this.cbHttps.Text = "HTTPS  transport";
            this.cbHttps.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 573);
            this.Controls.Add(this.cbHttps);
            this.Controls.Add(this.cbLowLevelTrace);
            this.Controls.Add(this.cbShowResult);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tcModes);
            this.Controls.Add(this.cbTrace);
            this.Controls.Add(this.groupBoxCamera);
            this.Name = "MainForm";
            this.Text = "Ping";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxCamera.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tcModes.ResumeLayout(false);
            this.tabPageProxy.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbMethods.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPageDirectCall.ResumeLayout(false);
            this.tabPageDiscoveryTest.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tcStepDetails.ResumeLayout(false);
            this.tabPageRequest.ResumeLayout(false);
            this.tabPageResponse.ResumeLayout(false);
            this.tabPageExceptions.ResumeLayout(false);
            this.tabPageTrace.ResumeLayout(false);
            this.tabPageSoap.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCamera;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCall;
        private System.Windows.Forms.TabControl tcModes;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.TabPage tabPageSoap;
        private System.Windows.Forms.RichTextBox tbConsole;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox tbRequest;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.CheckBox cbShowResult;
        private System.Windows.Forms.TabPage tabPageDirectCall;
        private System.Windows.Forms.CheckBox cbTrace;
        private System.Windows.Forms.TabPage tabPageTrace;
        private System.Windows.Forms.Button btnGetDiscoveryMode;
        private System.Windows.Forms.Button btnSetDiscoveryMode;
        private System.Windows.Forms.ComboBox comboBoxDiscoveryMode;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.Timer ourTimer;
        private System.Windows.Forms.RichTextBox tbTrace;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClearTrace;
        private System.Windows.Forms.GroupBox gbMethods;
        private System.Windows.Forms.ListView listViewMethods;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox tbResponse;
        private System.Windows.Forms.TabPage tabPageDiscoveryTest;
        private System.Windows.Forms.Button btnDiscoveryTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListView listViewTrace;
        private System.Windows.Forms.TabControl tcStepDetails;
        private System.Windows.Forms.TabPage tabPageRequest;
        private System.Windows.Forms.TabPage tabPageResponse;
        private System.Windows.Forms.TabPage tabPageExceptions;
        private System.Windows.Forms.RichTextBox richTextBoxStepRequest;
        private System.Windows.Forms.RichTextBox richTextBoxStepAnswer;
        private System.Windows.Forms.RichTextBox richTextBoxException;
        private System.Windows.Forms.Button btnGetClientSertificateMode;
        private System.Windows.Forms.Button SetZeroConfiguration;
        private System.Windows.Forms.Button btnSetClientCertificateMode;
        private System.Windows.Forms.Button SetAccessPolicy;
        private System.Windows.Forms.CheckBox cbLowLevelTrace;
        private System.Windows.Forms.TreeView treeViewTests;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnHalt;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button buttonRest;
        private System.Windows.Forms.CheckBox cbHttps;
    }
}

