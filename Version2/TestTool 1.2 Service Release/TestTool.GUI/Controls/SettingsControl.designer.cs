namespace TestTool.GUI.Controls
{
    partial class SettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControl));
            this.tcDetails = new System.Windows.Forms.TabControl();
            this.tpProduct = new System.Windows.Forms.TabPage();
            this.gbProductFeatures = new System.Windows.Forms.Panel();
            this.btnSelectTests = new System.Windows.Forms.Button();
            this.tvFeatures = new TestTool.GUI.Controls.TreeViewEx();
            this.ilFeaturesIcons = new System.Windows.Forms.ImageList(this.components);
            this.pnlEmpty = new System.Windows.Forms.Panel();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.gbUserDefined = new System.Windows.Forms.GroupBox();
            this.btnGetPTZNodes = new System.Windows.Forms.Button();
            this.lblPtzNode = new System.Windows.Forms.Label();
            this.cmbPTZNodes = new System.Windows.Forms.ComboBox();
            this.lblPasswords = new System.Windows.Forms.Label();
            this.rbOwnPasswords = new System.Windows.Forms.RadioButton();
            this.tbPassword1 = new System.Windows.Forms.TextBox();
            this.rbEmbeddedPasswords = new System.Windows.Forms.RadioButton();
            this.tbPassword2 = new System.Windows.Forms.TextBox();
            this.gbEnvironment = new System.Windows.Forms.GroupBox();
            this.lblGatewayIpv6 = new System.Windows.Forms.Label();
            this.lblGatewayIpv4 = new System.Windows.Forms.Label();
            this.tbGatewayIpv6 = new System.Windows.Forms.TextBox();
            this.tbGatewayIpv4 = new System.Windows.Forms.TextBox();
            this.lblGateWay = new System.Windows.Forms.Label();
            this.lblNtpIpv6 = new System.Windows.Forms.Label();
            this.lblDnsIpv6 = new System.Windows.Forms.Label();
            this.lblNtpIpv4 = new System.Windows.Forms.Label();
            this.lblDnsIpv4 = new System.Windows.Forms.Label();
            this.tbNtpIp6 = new System.Windows.Forms.TextBox();
            this.tbDnsIp6 = new System.Windows.Forms.TextBox();
            this.tbNtpIp4 = new System.Windows.Forms.TextBox();
            this.tbDnsIp4 = new System.Windows.Forms.TextBox();
            this.lblNTP = new System.Windows.Forms.Label();
            this.lblDNS = new System.Windows.Forms.Label();
            this.gbTimeouts = new System.Windows.Forms.GroupBox();
            this.tbRecoveryDelay = new System.Windows.Forms.TextBox();
            this.lblSafetyDelay = new System.Windows.Forms.Label();
            this.tbOperationDelay = new System.Windows.Forms.TextBox();
            this.lblTimeBetweenTests = new System.Windows.Forms.Label();
            this.lblOperationDelay = new System.Windows.Forms.Label();
            this.tbTimeBetweenTests = new System.Windows.Forms.TextBox();
            this.lblRebootTime = new System.Windows.Forms.Label();
            this.tbRebootTimeout = new System.Windows.Forms.TextBox();
            this.lblMessageTimeout = new System.Windows.Forms.Label();
            this.tbMessageTimeout = new System.Windows.Forms.TextBox();
            this.tcDetails.SuspendLayout();
            this.tpProduct.SuspendLayout();
            this.gbProductFeatures.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.gbUserDefined.SuspendLayout();
            this.gbEnvironment.SuspendLayout();
            this.gbTimeouts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDetails
            // 
            this.tcDetails.Controls.Add(this.tpProduct);
            this.tcDetails.Controls.Add(this.tpSettings);
            this.tcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDetails.Location = new System.Drawing.Point(0, 0);
            this.tcDetails.Name = "tcDetails";
            this.tcDetails.SelectedIndex = 0;
            this.tcDetails.Size = new System.Drawing.Size(411, 469);
            this.tcDetails.TabIndex = 0;
            // 
            // tpProduct
            // 
            this.tpProduct.Controls.Add(this.gbProductFeatures);
            this.tpProduct.Controls.Add(this.pnlEmpty);
            this.tpProduct.Location = new System.Drawing.Point(4, 22);
            this.tpProduct.Name = "tpProduct";
            this.tpProduct.Padding = new System.Windows.Forms.Padding(3);
            this.tpProduct.Size = new System.Drawing.Size(403, 443);
            this.tpProduct.TabIndex = 0;
            this.tpProduct.Text = "Product Features";
            this.tpProduct.UseVisualStyleBackColor = true;
            // 
            // gbProductFeatures
            // 
            this.gbProductFeatures.Controls.Add(this.btnSelectTests);
            this.gbProductFeatures.Controls.Add(this.tvFeatures);
            this.gbProductFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProductFeatures.Location = new System.Drawing.Point(3, 6);
            this.gbProductFeatures.Name = "gbProductFeatures";
            this.gbProductFeatures.Size = new System.Drawing.Size(397, 434);
            this.gbProductFeatures.TabIndex = 8;
            this.gbProductFeatures.Text = "Product Features";
            // 
            // btnSelectTests
            // 
            this.btnSelectTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectTests.Location = new System.Drawing.Point(0, 408);
            this.btnSelectTests.Name = "btnSelectTests";
            this.btnSelectTests.Size = new System.Drawing.Size(85, 23);
            this.btnSelectTests.TabIndex = 3;
            this.btnSelectTests.Text = "Select Tests";
            this.btnSelectTests.UseVisualStyleBackColor = true;
            this.btnSelectTests.Click += new System.EventHandler(this.btnSelectTests_Click);
            // 
            // tvFeatures
            // 
            this.tvFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFeatures.CheckBoxes = true;
            this.tvFeatures.ImageIndex = 0;
            this.tvFeatures.ImageList = this.ilFeaturesIcons;
            this.tvFeatures.Location = new System.Drawing.Point(0, 6);
            this.tvFeatures.Name = "tvFeatures";
            this.tvFeatures.SelectedImageIndex = 0;
            this.tvFeatures.ShowNodeToolTips = true;
            this.tvFeatures.Size = new System.Drawing.Size(394, 396);
            this.tvFeatures.TabIndex = 2;
            this.tvFeatures.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvFeatures_AfterCheck);
            this.tvFeatures.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFeatures_BeforeCheck);
            // 
            // ilFeaturesIcons
            // 
            this.ilFeaturesIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFeaturesIcons.ImageStream")));
            this.ilFeaturesIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilFeaturesIcons.Images.SetKeyName(0, "Undefined");
            this.ilFeaturesIcons.Images.SetKeyName(1, "Mandatory");
            this.ilFeaturesIcons.Images.SetKeyName(2, "Optional");
            // 
            // pnlEmpty
            // 
            this.pnlEmpty.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEmpty.Location = new System.Drawing.Point(3, 3);
            this.pnlEmpty.Name = "pnlEmpty";
            this.pnlEmpty.Size = new System.Drawing.Size(397, 3);
            this.pnlEmpty.TabIndex = 2;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.gbUserDefined);
            this.tpSettings.Controls.Add(this.gbEnvironment);
            this.tpSettings.Controls.Add(this.gbTimeouts);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(403, 443);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // gbUserDefined
            // 
            this.gbUserDefined.Controls.Add(this.btnGetPTZNodes);
            this.gbUserDefined.Controls.Add(this.lblPtzNode);
            this.gbUserDefined.Controls.Add(this.cmbPTZNodes);
            this.gbUserDefined.Controls.Add(this.lblPasswords);
            this.gbUserDefined.Controls.Add(this.rbOwnPasswords);
            this.gbUserDefined.Controls.Add(this.tbPassword1);
            this.gbUserDefined.Controls.Add(this.rbEmbeddedPasswords);
            this.gbUserDefined.Controls.Add(this.tbPassword2);
            this.gbUserDefined.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbUserDefined.Location = new System.Drawing.Point(3, 256);
            this.gbUserDefined.Name = "gbUserDefined";
            this.gbUserDefined.Size = new System.Drawing.Size(397, 136);
            this.gbUserDefined.TabIndex = 8;
            this.gbUserDefined.TabStop = false;
            this.gbUserDefined.Text = "Miscellaneous";
            // 
            // btnGetPTZNodes
            // 
            this.btnGetPTZNodes.Location = new System.Drawing.Point(263, 103);
            this.btnGetPTZNodes.Name = "btnGetPTZNodes";
            this.btnGetPTZNodes.Size = new System.Drawing.Size(88, 23);
            this.btnGetPTZNodes.TabIndex = 31;
            this.btnGetPTZNodes.Text = "Get";
            this.btnGetPTZNodes.UseVisualStyleBackColor = true;
            this.btnGetPTZNodes.Click += new System.EventHandler(this.btnGetPTZNodes_Click);
            // 
            // lblPtzNode
            // 
            this.lblPtzNode.AutoSize = true;
            this.lblPtzNode.Location = new System.Drawing.Point(6, 108);
            this.lblPtzNode.Name = "lblPtzNode";
            this.lblPtzNode.Size = new System.Drawing.Size(98, 13);
            this.lblPtzNode.TabIndex = 30;
            this.lblPtzNode.Text = "PTZ node for tests:";
            // 
            // cmbPTZNodes
            // 
            this.cmbPTZNodes.FormattingEnabled = true;
            this.cmbPTZNodes.Location = new System.Drawing.Point(110, 105);
            this.cmbPTZNodes.Name = "cmbPTZNodes";
            this.cmbPTZNodes.Size = new System.Drawing.Size(109, 21);
            this.cmbPTZNodes.TabIndex = 29;
            // 
            // lblPasswords
            // 
            this.lblPasswords.AutoSize = true;
            this.lblPasswords.Location = new System.Drawing.Point(29, 67);
            this.lblPasswords.Name = "lblPasswords";
            this.lblPasswords.Size = new System.Drawing.Size(61, 13);
            this.lblPasswords.TabIndex = 28;
            this.lblPasswords.Text = "Passwords:";
            // 
            // rbOwnPasswords
            // 
            this.rbOwnPasswords.AutoSize = true;
            this.rbOwnPasswords.Location = new System.Drawing.Point(9, 42);
            this.rbOwnPasswords.Name = "rbOwnPasswords";
            this.rbOwnPasswords.Size = new System.Drawing.Size(137, 17);
            this.rbOwnPasswords.TabIndex = 19;
            this.rbOwnPasswords.TabStop = true;
            this.rbOwnPasswords.Text = "Provide own passwords";
            this.rbOwnPasswords.UseVisualStyleBackColor = true;
            // 
            // tbPassword1
            // 
            this.tbPassword1.Location = new System.Drawing.Point(110, 64);
            this.tbPassword1.Name = "tbPassword1";
            this.tbPassword1.Size = new System.Drawing.Size(109, 20);
            this.tbPassword1.TabIndex = 20;
            // 
            // rbEmbeddedPasswords
            // 
            this.rbEmbeddedPasswords.AutoSize = true;
            this.rbEmbeddedPasswords.Checked = true;
            this.rbEmbeddedPasswords.Location = new System.Drawing.Point(9, 19);
            this.rbEmbeddedPasswords.Name = "rbEmbeddedPasswords";
            this.rbEmbeddedPasswords.Size = new System.Drawing.Size(229, 17);
            this.rbEmbeddedPasswords.TabIndex = 18;
            this.rbEmbeddedPasswords.TabStop = true;
            this.rbEmbeddedPasswords.Text = "Use embedded passwords for security tests";
            this.rbEmbeddedPasswords.UseVisualStyleBackColor = true;
            this.rbEmbeddedPasswords.CheckedChanged += new System.EventHandler(this.rbEmbeddedPasswords_CheckedChanged);
            // 
            // tbPassword2
            // 
            this.tbPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword2.Location = new System.Drawing.Point(263, 64);
            this.tbPassword2.MaximumSize = new System.Drawing.Size(150, 20);
            this.tbPassword2.Name = "tbPassword2";
            this.tbPassword2.Size = new System.Drawing.Size(124, 20);
            this.tbPassword2.TabIndex = 21;
            // 
            // gbEnvironment
            // 
            this.gbEnvironment.Controls.Add(this.lblGatewayIpv6);
            this.gbEnvironment.Controls.Add(this.lblGatewayIpv4);
            this.gbEnvironment.Controls.Add(this.tbGatewayIpv6);
            this.gbEnvironment.Controls.Add(this.tbGatewayIpv4);
            this.gbEnvironment.Controls.Add(this.lblGateWay);
            this.gbEnvironment.Controls.Add(this.lblNtpIpv6);
            this.gbEnvironment.Controls.Add(this.lblDnsIpv6);
            this.gbEnvironment.Controls.Add(this.lblNtpIpv4);
            this.gbEnvironment.Controls.Add(this.lblDnsIpv4);
            this.gbEnvironment.Controls.Add(this.tbNtpIp6);
            this.gbEnvironment.Controls.Add(this.tbDnsIp6);
            this.gbEnvironment.Controls.Add(this.tbNtpIp4);
            this.gbEnvironment.Controls.Add(this.tbDnsIp4);
            this.gbEnvironment.Controls.Add(this.lblNTP);
            this.gbEnvironment.Controls.Add(this.lblDNS);
            this.gbEnvironment.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEnvironment.Location = new System.Drawing.Point(3, 153);
            this.gbEnvironment.Name = "gbEnvironment";
            this.gbEnvironment.Size = new System.Drawing.Size(397, 103);
            this.gbEnvironment.TabIndex = 7;
            this.gbEnvironment.TabStop = false;
            this.gbEnvironment.Text = "Environment";
            // 
            // lblGatewayIpv6
            // 
            this.lblGatewayIpv6.AutoSize = true;
            this.lblGatewayIpv6.Location = new System.Drawing.Point(225, 75);
            this.lblGatewayIpv6.Name = "lblGatewayIpv6";
            this.lblGatewayIpv6.Size = new System.Drawing.Size(32, 13);
            this.lblGatewayIpv6.TabIndex = 21;
            this.lblGatewayIpv6.Text = "IPv6:";
            // 
            // lblGatewayIpv4
            // 
            this.lblGatewayIpv4.AutoSize = true;
            this.lblGatewayIpv4.Location = new System.Drawing.Point(42, 75);
            this.lblGatewayIpv4.Name = "lblGatewayIpv4";
            this.lblGatewayIpv4.Size = new System.Drawing.Size(32, 13);
            this.lblGatewayIpv4.TabIndex = 20;
            this.lblGatewayIpv4.Text = "IPv4:";
            // 
            // tbGatewayIpv6
            // 
            this.tbGatewayIpv6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGatewayIpv6.Location = new System.Drawing.Point(263, 72);
            this.tbGatewayIpv6.MaximumSize = new System.Drawing.Size(150, 20);
            this.tbGatewayIpv6.Name = "tbGatewayIpv6";
            this.tbGatewayIpv6.Size = new System.Drawing.Size(123, 20);
            this.tbGatewayIpv6.TabIndex = 15;
            this.tbGatewayIpv6.Validating += new System.ComponentModel.CancelEventHandler(this.tbGatewayIpv6_Validating);
            // 
            // tbGatewayIpv4
            // 
            this.tbGatewayIpv4.Location = new System.Drawing.Point(110, 72);
            this.tbGatewayIpv4.Name = "tbGatewayIpv4";
            this.tbGatewayIpv4.Size = new System.Drawing.Size(109, 20);
            this.tbGatewayIpv4.TabIndex = 14;
            this.tbGatewayIpv4.Validating += new System.ComponentModel.CancelEventHandler(this.tbGatewayIpv4_Validating);
            // 
            // lblGateWay
            // 
            this.lblGateWay.AutoSize = true;
            this.lblGateWay.Location = new System.Drawing.Point(6, 75);
            this.lblGateWay.Name = "lblGateWay";
            this.lblGateWay.Size = new System.Drawing.Size(26, 13);
            this.lblGateWay.TabIndex = 17;
            this.lblGateWay.Text = "GW";
            // 
            // lblNtpIpv6
            // 
            this.lblNtpIpv6.AutoSize = true;
            this.lblNtpIpv6.Location = new System.Drawing.Point(225, 48);
            this.lblNtpIpv6.Name = "lblNtpIpv6";
            this.lblNtpIpv6.Size = new System.Drawing.Size(32, 13);
            this.lblNtpIpv6.TabIndex = 16;
            this.lblNtpIpv6.Text = "IPv6:";
            // 
            // lblDnsIpv6
            // 
            this.lblDnsIpv6.AutoSize = true;
            this.lblDnsIpv6.Location = new System.Drawing.Point(225, 22);
            this.lblDnsIpv6.Name = "lblDnsIpv6";
            this.lblDnsIpv6.Size = new System.Drawing.Size(32, 13);
            this.lblDnsIpv6.TabIndex = 15;
            this.lblDnsIpv6.Text = "IPv6:";
            // 
            // lblNtpIpv4
            // 
            this.lblNtpIpv4.AutoSize = true;
            this.lblNtpIpv4.Location = new System.Drawing.Point(42, 48);
            this.lblNtpIpv4.Name = "lblNtpIpv4";
            this.lblNtpIpv4.Size = new System.Drawing.Size(32, 13);
            this.lblNtpIpv4.TabIndex = 14;
            this.lblNtpIpv4.Text = "IPv4:";
            // 
            // lblDnsIpv4
            // 
            this.lblDnsIpv4.AutoSize = true;
            this.lblDnsIpv4.Location = new System.Drawing.Point(42, 22);
            this.lblDnsIpv4.Name = "lblDnsIpv4";
            this.lblDnsIpv4.Size = new System.Drawing.Size(32, 13);
            this.lblDnsIpv4.TabIndex = 7;
            this.lblDnsIpv4.Text = "IPv4:";
            // 
            // tbNtpIp6
            // 
            this.tbNtpIp6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNtpIp6.Location = new System.Drawing.Point(263, 45);
            this.tbNtpIp6.MaximumSize = new System.Drawing.Size(150, 20);
            this.tbNtpIp6.Name = "tbNtpIp6";
            this.tbNtpIp6.Size = new System.Drawing.Size(123, 20);
            this.tbNtpIp6.TabIndex = 13;
            this.tbNtpIp6.Validating += new System.ComponentModel.CancelEventHandler(this.tbNtpIp6_Validating);
            // 
            // tbDnsIp6
            // 
            this.tbDnsIp6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDnsIp6.Location = new System.Drawing.Point(263, 19);
            this.tbDnsIp6.MaximumSize = new System.Drawing.Size(150, 20);
            this.tbDnsIp6.Name = "tbDnsIp6";
            this.tbDnsIp6.Size = new System.Drawing.Size(123, 20);
            this.tbDnsIp6.TabIndex = 11;
            this.tbDnsIp6.Validating += new System.ComponentModel.CancelEventHandler(this.tbDnsIp6_Validating);
            // 
            // tbNtpIp4
            // 
            this.tbNtpIp4.Location = new System.Drawing.Point(110, 45);
            this.tbNtpIp4.Name = "tbNtpIp4";
            this.tbNtpIp4.Size = new System.Drawing.Size(109, 20);
            this.tbNtpIp4.TabIndex = 12;
            this.tbNtpIp4.Validating += new System.ComponentModel.CancelEventHandler(this.tbNtpIp4_Validating);
            // 
            // tbDnsIp4
            // 
            this.tbDnsIp4.Location = new System.Drawing.Point(110, 19);
            this.tbDnsIp4.Name = "tbDnsIp4";
            this.tbDnsIp4.Size = new System.Drawing.Size(109, 20);
            this.tbDnsIp4.TabIndex = 10;
            this.tbDnsIp4.Validating += new System.ComponentModel.CancelEventHandler(this.tbDnsIp4_Validating);
            // 
            // lblNTP
            // 
            this.lblNTP.AutoSize = true;
            this.lblNTP.Location = new System.Drawing.Point(6, 48);
            this.lblNTP.Name = "lblNTP";
            this.lblNTP.Size = new System.Drawing.Size(29, 13);
            this.lblNTP.TabIndex = 1;
            this.lblNTP.Text = "NTP";
            // 
            // lblDNS
            // 
            this.lblDNS.AutoSize = true;
            this.lblDNS.Location = new System.Drawing.Point(6, 22);
            this.lblDNS.Name = "lblDNS";
            this.lblDNS.Size = new System.Drawing.Size(30, 13);
            this.lblDNS.TabIndex = 0;
            this.lblDNS.Text = "DNS";
            // 
            // gbTimeouts
            // 
            this.gbTimeouts.Controls.Add(this.tbRecoveryDelay);
            this.gbTimeouts.Controls.Add(this.lblSafetyDelay);
            this.gbTimeouts.Controls.Add(this.tbOperationDelay);
            this.gbTimeouts.Controls.Add(this.lblTimeBetweenTests);
            this.gbTimeouts.Controls.Add(this.lblOperationDelay);
            this.gbTimeouts.Controls.Add(this.tbTimeBetweenTests);
            this.gbTimeouts.Controls.Add(this.lblRebootTime);
            this.gbTimeouts.Controls.Add(this.tbRebootTimeout);
            this.gbTimeouts.Controls.Add(this.lblMessageTimeout);
            this.gbTimeouts.Controls.Add(this.tbMessageTimeout);
            this.gbTimeouts.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTimeouts.Location = new System.Drawing.Point(3, 3);
            this.gbTimeouts.Name = "gbTimeouts";
            this.gbTimeouts.Size = new System.Drawing.Size(397, 150);
            this.gbTimeouts.TabIndex = 6;
            this.gbTimeouts.TabStop = false;
            this.gbTimeouts.Text = "Timeouts";
            // 
            // tbRecoveryDelay
            // 
            this.tbRecoveryDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecoveryDelay.Location = new System.Drawing.Point(159, 118);
            this.tbRecoveryDelay.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbRecoveryDelay.Name = "tbRecoveryDelay";
            this.tbRecoveryDelay.Size = new System.Drawing.Size(227, 20);
            this.tbRecoveryDelay.TabIndex = 34;
            this.tbRecoveryDelay.Validating += new System.ComponentModel.CancelEventHandler(this.tbRecoveryDelay_Validating);
            // 
            // lblSafetyDelay
            // 
            this.lblSafetyDelay.AutoSize = true;
            this.lblSafetyDelay.Location = new System.Drawing.Point(3, 121);
            this.lblSafetyDelay.Name = "lblSafetyDelay";
            this.lblSafetyDelay.Size = new System.Drawing.Size(147, 13);
            this.lblSafetyDelay.TabIndex = 33;
            this.lblSafetyDelay.Text = "Time between Requests (ms):";
            // 
            // tbOperationDelay
            // 
            this.tbOperationDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOperationDelay.Location = new System.Drawing.Point(159, 92);
            this.tbOperationDelay.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbOperationDelay.Name = "tbOperationDelay";
            this.tbOperationDelay.Size = new System.Drawing.Size(227, 20);
            this.tbOperationDelay.TabIndex = 30;
            this.tbOperationDelay.Validating += new System.ComponentModel.CancelEventHandler(this.tbOperationDelay_Validating);
            // 
            // lblTimeBetweenTests
            // 
            this.lblTimeBetweenTests.AutoSize = true;
            this.lblTimeBetweenTests.Location = new System.Drawing.Point(6, 70);
            this.lblTimeBetweenTests.Name = "lblTimeBetweenTests";
            this.lblTimeBetweenTests.Size = new System.Drawing.Size(129, 13);
            this.lblTimeBetweenTests.TabIndex = 19;
            this.lblTimeBetweenTests.Text = "Time Between Tests (ms):";
            // 
            // lblOperationDelay
            // 
            this.lblOperationDelay.AutoSize = true;
            this.lblOperationDelay.Location = new System.Drawing.Point(6, 95);
            this.lblOperationDelay.Name = "lblOperationDelay";
            this.lblOperationDelay.Size = new System.Drawing.Size(98, 13);
            this.lblOperationDelay.TabIndex = 29;
            this.lblOperationDelay.Text = "Operation delay (s):";
            // 
            // tbTimeBetweenTests
            // 
            this.tbTimeBetweenTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTimeBetweenTests.Location = new System.Drawing.Point(159, 67);
            this.tbTimeBetweenTests.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbTimeBetweenTests.Name = "tbTimeBetweenTests";
            this.tbTimeBetweenTests.Size = new System.Drawing.Size(227, 20);
            this.tbTimeBetweenTests.TabIndex = 11;
            this.tbTimeBetweenTests.Validating += new System.ComponentModel.CancelEventHandler(this.tbTimeBetweenTests_Validating);
            // 
            // lblRebootTime
            // 
            this.lblRebootTime.AutoSize = true;
            this.lblRebootTime.Location = new System.Drawing.Point(6, 44);
            this.lblRebootTime.Name = "lblRebootTime";
            this.lblRebootTime.Size = new System.Drawing.Size(108, 13);
            this.lblRebootTime.TabIndex = 17;
            this.lblRebootTime.Text = "Reboot Timeout (ms):";
            // 
            // tbRebootTimeout
            // 
            this.tbRebootTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRebootTimeout.Location = new System.Drawing.Point(159, 41);
            this.tbRebootTimeout.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbRebootTimeout.Name = "tbRebootTimeout";
            this.tbRebootTimeout.Size = new System.Drawing.Size(227, 20);
            this.tbRebootTimeout.TabIndex = 10;
            this.tbRebootTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.tbRebootTimeout_Validating);
            // 
            // lblMessageTimeout
            // 
            this.lblMessageTimeout.AutoSize = true;
            this.lblMessageTimeout.Location = new System.Drawing.Point(6, 20);
            this.lblMessageTimeout.Name = "lblMessageTimeout";
            this.lblMessageTimeout.Size = new System.Drawing.Size(116, 13);
            this.lblMessageTimeout.TabIndex = 15;
            this.lblMessageTimeout.Text = "Message Timeout (ms):";
            // 
            // tbMessageTimeout
            // 
            this.tbMessageTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessageTimeout.Location = new System.Drawing.Point(159, 15);
            this.tbMessageTimeout.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbMessageTimeout.Name = "tbMessageTimeout";
            this.tbMessageTimeout.Size = new System.Drawing.Size(227, 20);
            this.tbMessageTimeout.TabIndex = 9;
            this.tbMessageTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.tbMessageTimeout_Validating);
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcDetails);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(411, 469);
            this.tcDetails.ResumeLayout(false);
            this.tpProduct.ResumeLayout(false);
            this.gbProductFeatures.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.gbUserDefined.ResumeLayout(false);
            this.gbUserDefined.PerformLayout();
            this.gbEnvironment.ResumeLayout(false);
            this.gbEnvironment.PerformLayout();
            this.gbTimeouts.ResumeLayout(false);
            this.gbTimeouts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDetails;
        private System.Windows.Forms.TabPage tpProduct;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.Panel pnlEmpty;
        private System.Windows.Forms.Panel gbProductFeatures;
        private System.Windows.Forms.Button btnSelectTests;
        private System.Windows.Forms.GroupBox gbTimeouts;
        private System.Windows.Forms.Label lblTimeBetweenTests;
        private System.Windows.Forms.TextBox tbTimeBetweenTests;
        private System.Windows.Forms.Label lblRebootTime;
        private System.Windows.Forms.TextBox tbRebootTimeout;
        private System.Windows.Forms.Label lblMessageTimeout;
        private System.Windows.Forms.TextBox tbMessageTimeout;
        private System.Windows.Forms.GroupBox gbEnvironment;
        private System.Windows.Forms.Label lblPasswords;
        private System.Windows.Forms.RadioButton rbOwnPasswords;
        private System.Windows.Forms.RadioButton rbEmbeddedPasswords;
        private System.Windows.Forms.TextBox tbPassword2;
        private System.Windows.Forms.TextBox tbPassword1;
        private System.Windows.Forms.Label lblGatewayIpv6;
        private System.Windows.Forms.Label lblGatewayIpv4;
        private System.Windows.Forms.TextBox tbGatewayIpv6;
        private System.Windows.Forms.TextBox tbGatewayIpv4;
        private System.Windows.Forms.Label lblGateWay;
        private System.Windows.Forms.Label lblNtpIpv6;
        private System.Windows.Forms.Label lblDnsIpv6;
        private System.Windows.Forms.Label lblNtpIpv4;
        private System.Windows.Forms.Label lblDnsIpv4;
        private System.Windows.Forms.TextBox tbNtpIp6;
        private System.Windows.Forms.TextBox tbDnsIp6;
        private System.Windows.Forms.TextBox tbNtpIp4;
        private System.Windows.Forms.TextBox tbDnsIp4;
        private System.Windows.Forms.Label lblNTP;
        private System.Windows.Forms.Label lblDNS;
        private System.Windows.Forms.TextBox tbOperationDelay;
        private System.Windows.Forms.Label lblOperationDelay;
        private System.Windows.Forms.GroupBox gbUserDefined;
        private System.Windows.Forms.Button btnGetPTZNodes;
        private System.Windows.Forms.Label lblPtzNode;
        private System.Windows.Forms.ComboBox cmbPTZNodes;
        private System.Windows.Forms.ImageList ilFeaturesIcons;
        private TestTool.GUI.Controls.TreeViewEx tvFeatures;
        private System.Windows.Forms.TextBox tbRecoveryDelay;
        private System.Windows.Forms.Label lblSafetyDelay;
    }
}
