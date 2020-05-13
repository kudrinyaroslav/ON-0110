using System.ComponentModel;

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
            this.gbTimeouts = new System.Windows.Forms.GroupBox();
            this.tbSafetyDelay = new System.Windows.Forms.TextBox();
            this.lblSafetyDelay = new System.Windows.Forms.Label();
            this.tbOperationDelay = new System.Windows.Forms.TextBox();
            this.lblTimeBetweenTests = new System.Windows.Forms.Label();
            this.lblOperationDelay = new System.Windows.Forms.Label();
            this.tbTimeBetweenTests = new System.Windows.Forms.TextBox();
            this.lblRebootTime = new System.Windows.Forms.Label();
            this.tbRebootTimeout = new System.Windows.Forms.TextBox();
            this.lblMessageTimeout = new System.Windows.Forms.Label();
            this.tbMessageTimeout = new System.Windows.Forms.TextBox();
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
            this.btnGetPTZNodes = new System.Windows.Forms.Button();
            this.lblPtzNode = new System.Windows.Forms.Label();
            this.cmbPTZNodes = new System.Windows.Forms.ComboBox();
            this.lblPasswords = new System.Windows.Forms.Label();
            this.rbOwnPasswords = new System.Windows.Forms.RadioButton();
            this.tbPassword1 = new System.Windows.Forms.TextBox();
            this.rbEmbeddedPasswords = new System.Windows.Forms.RadioButton();
            this.tbPassword2 = new System.Windows.Forms.TextBox();
            this.cmbSecureMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSubscriptionTimeout = new System.Windows.Forms.TextBox();
            this.lblSubscriptionTimeout = new System.Windows.Forms.Label();
            this.btnGetTopics = new System.Windows.Forms.Button();
            this.cmbEventTopic = new System.Windows.Forms.ComboBox();
            this.lblEventTopic = new System.Windows.Forms.Label();
            this.gbMisc = new System.Windows.Forms.GroupBox();
            this.tcMiscSettings = new System.Windows.Forms.TabControl();
            this.tpSecurity = new System.Windows.Forms.TabPage();
            this.tpPTZ = new System.Windows.Forms.TabPage();
            this.btnGetVideoSources = new System.Windows.Forms.Button();
            this.cmbVideoSource = new System.Windows.Forms.ComboBox();
            this.lblVideoSource = new System.Windows.Forms.Label();
            this.tpEvents = new System.Windows.Forms.TabPage();
            this.tbNamespaces = new System.Windows.Forms.TextBox();
            this.lblNamespaces = new System.Windows.Forms.Label();
            this.tpIO = new System.Windows.Forms.TabPage();
            this.tbRelayOutputsDelayMonostable = new System.Windows.Forms.TextBox();
            this.lblRelayOutputsDelay = new System.Windows.Forms.Label();
            this.toolTipSettings = new System.Windows.Forms.ToolTip(this.components);
            this.gbTimeouts.SuspendLayout();
            this.gbEnvironment.SuspendLayout();
            this.gbMisc.SuspendLayout();
            this.tcMiscSettings.SuspendLayout();
            this.tpSecurity.SuspendLayout();
            this.tpPTZ.SuspendLayout();
            this.tpEvents.SuspendLayout();
            this.tpIO.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTimeouts
            // 
            this.gbTimeouts.Controls.Add(this.tbSafetyDelay);
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
            this.gbTimeouts.Location = new System.Drawing.Point(0, 0);
            this.gbTimeouts.Name = "gbTimeouts";
            this.gbTimeouts.Size = new System.Drawing.Size(410, 147);
            this.gbTimeouts.TabIndex = 7;
            this.gbTimeouts.TabStop = false;
            this.gbTimeouts.Text = "Timeouts";
            // 
            // tbSafetyDelay
            // 
            this.tbSafetyDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSafetyDelay.Location = new System.Drawing.Point(161, 119);
            this.tbSafetyDelay.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbSafetyDelay.Name = "tbSafetyDelay";
            this.tbSafetyDelay.Size = new System.Drawing.Size(232, 20);
            this.tbSafetyDelay.TabIndex = 32;
            this.toolTipSettings.SetToolTip(this.tbSafetyDelay, "Amount of time the Tool waits after a response is received from the DUT. Should b" +
                    "e enough for the DUT to get ready to the next request");
            this.tbSafetyDelay.Validating += new System.ComponentModel.CancelEventHandler(this.tbSafetyDelay_Validating);
            // 
            // lblSafetyDelay
            // 
            this.lblSafetyDelay.AutoSize = true;
            this.lblSafetyDelay.Location = new System.Drawing.Point(6, 119);
            this.lblSafetyDelay.Name = "lblSafetyDelay";
            this.lblSafetyDelay.Size = new System.Drawing.Size(147, 13);
            this.lblSafetyDelay.TabIndex = 31;
            this.lblSafetyDelay.Text = "Time between Requests (ms):";
            // 
            // tbOperationDelay
            // 
            this.tbOperationDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOperationDelay.Location = new System.Drawing.Point(162, 92);
            this.tbOperationDelay.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbOperationDelay.Name = "tbOperationDelay";
            this.tbOperationDelay.Size = new System.Drawing.Size(232, 20);
            this.tbOperationDelay.TabIndex = 30;
            this.toolTipSettings.SetToolTip(this.tbOperationDelay, "Delay for potentially time-consuming operation (like PTZ move)");
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
            this.lblOperationDelay.Size = new System.Drawing.Size(106, 13);
            this.lblOperationDelay.TabIndex = 29;
            this.lblOperationDelay.Text = "Operation delay (ms):";
            // 
            // tbTimeBetweenTests
            // 
            this.tbTimeBetweenTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTimeBetweenTests.Location = new System.Drawing.Point(162, 67);
            this.tbTimeBetweenTests.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbTimeBetweenTests.Name = "tbTimeBetweenTests";
            this.tbTimeBetweenTests.Size = new System.Drawing.Size(232, 20);
            this.tbTimeBetweenTests.TabIndex = 11;
            this.toolTipSettings.SetToolTip(this.tbTimeBetweenTests, "Amount of time the Tool waits between test cases");
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
            this.tbRebootTimeout.Location = new System.Drawing.Point(162, 41);
            this.tbRebootTimeout.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbRebootTimeout.Name = "tbRebootTimeout";
            this.tbRebootTimeout.Size = new System.Drawing.Size(232, 20);
            this.tbRebootTimeout.TabIndex = 10;
            this.toolTipSettings.SetToolTip(this.tbRebootTimeout, "Amount of time enough to reboot");
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
            this.tbMessageTimeout.Location = new System.Drawing.Point(162, 15);
            this.tbMessageTimeout.MaximumSize = new System.Drawing.Size(274, 20);
            this.tbMessageTimeout.Name = "tbMessageTimeout";
            this.tbMessageTimeout.Size = new System.Drawing.Size(230, 20);
            this.tbMessageTimeout.TabIndex = 9;
            this.toolTipSettings.SetToolTip(this.tbMessageTimeout, "Time which the Tool waits for the DUT\'s response");
            this.tbMessageTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.tbMessageTimeout_Validating);
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
            this.gbEnvironment.Location = new System.Drawing.Point(0, 147);
            this.gbEnvironment.Name = "gbEnvironment";
            this.gbEnvironment.Size = new System.Drawing.Size(410, 103);
            this.gbEnvironment.TabIndex = 8;
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
            this.tbGatewayIpv6.Size = new System.Drawing.Size(130, 20);
            this.tbGatewayIpv6.TabIndex = 15;
            this.toolTipSettings.SetToolTip(this.tbGatewayIpv6, "Default gateway to be set during the testing (IPv4)");
            this.tbGatewayIpv6.Validating += new System.ComponentModel.CancelEventHandler(this.tbGatewayIpv6_Validating);
            // 
            // tbGatewayIpv4
            // 
            this.tbGatewayIpv4.Location = new System.Drawing.Point(89, 72);
            this.tbGatewayIpv4.Name = "tbGatewayIpv4";
            this.tbGatewayIpv4.Size = new System.Drawing.Size(130, 20);
            this.tbGatewayIpv4.TabIndex = 14;
            this.toolTipSettings.SetToolTip(this.tbGatewayIpv4, "Default gateway to be set during the testing (IPv4)");
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
            this.tbNtpIp6.Size = new System.Drawing.Size(130, 20);
            this.tbNtpIp6.TabIndex = 13;
            this.toolTipSettings.SetToolTip(this.tbNtpIp6, "NTP server address to be set during the testing (IPv6)");
            this.tbNtpIp6.Validating += new System.ComponentModel.CancelEventHandler(this.tbNtpIp6_Validating);
            // 
            // tbDnsIp6
            // 
            this.tbDnsIp6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDnsIp6.Location = new System.Drawing.Point(263, 19);
            this.tbDnsIp6.MaximumSize = new System.Drawing.Size(150, 20);
            this.tbDnsIp6.Name = "tbDnsIp6";
            this.tbDnsIp6.Size = new System.Drawing.Size(130, 20);
            this.tbDnsIp6.TabIndex = 11;
            this.toolTipSettings.SetToolTip(this.tbDnsIp6, "DNS address to be set during the testing (IPv6)");
            this.tbDnsIp6.Validating += new System.ComponentModel.CancelEventHandler(this.tbDnsIp6_Validating);
            // 
            // tbNtpIp4
            // 
            this.tbNtpIp4.Location = new System.Drawing.Point(89, 45);
            this.tbNtpIp4.Name = "tbNtpIp4";
            this.tbNtpIp4.Size = new System.Drawing.Size(130, 20);
            this.tbNtpIp4.TabIndex = 12;
            this.toolTipSettings.SetToolTip(this.tbNtpIp4, "NTP server address to be set during the testing (IPv4)");
            this.tbNtpIp4.Validating += new System.ComponentModel.CancelEventHandler(this.tbNtpIp4_Validating);
            // 
            // tbDnsIp4
            // 
            this.tbDnsIp4.Location = new System.Drawing.Point(89, 19);
            this.tbDnsIp4.Name = "tbDnsIp4";
            this.tbDnsIp4.Size = new System.Drawing.Size(130, 20);
            this.tbDnsIp4.TabIndex = 10;
            this.toolTipSettings.SetToolTip(this.tbDnsIp4, "DNS address to be set during the testing (IPv4)");
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
            // btnGetPTZNodes
            // 
            this.btnGetPTZNodes.Location = new System.Drawing.Point(299, 9);
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
            this.lblPtzNode.Location = new System.Drawing.Point(3, 14);
            this.lblPtzNode.Name = "lblPtzNode";
            this.lblPtzNode.Size = new System.Drawing.Size(98, 13);
            this.lblPtzNode.TabIndex = 30;
            this.lblPtzNode.Text = "PTZ node for tests:";
            // 
            // cmbPTZNodes
            // 
            this.cmbPTZNodes.FormattingEnabled = true;
            this.cmbPTZNodes.Location = new System.Drawing.Point(150, 11);
            this.cmbPTZNodes.Name = "cmbPTZNodes";
            this.cmbPTZNodes.Size = new System.Drawing.Size(115, 21);
            this.cmbPTZNodes.TabIndex = 29;
            this.toolTipSettings.SetToolTip(this.cmbPTZNodes, "PTZ Node for testing. Whole list could ne get by clicking \"Get\" button");
            // 
            // lblPasswords
            // 
            this.lblPasswords.AutoSize = true;
            this.lblPasswords.Location = new System.Drawing.Point(23, 55);
            this.lblPasswords.Name = "lblPasswords";
            this.lblPasswords.Size = new System.Drawing.Size(61, 13);
            this.lblPasswords.TabIndex = 28;
            this.lblPasswords.Text = "Passwords:";
            // 
            // rbOwnPasswords
            // 
            this.rbOwnPasswords.AutoSize = true;
            this.rbOwnPasswords.Location = new System.Drawing.Point(5, 29);
            this.rbOwnPasswords.Name = "rbOwnPasswords";
            this.rbOwnPasswords.Size = new System.Drawing.Size(137, 17);
            this.rbOwnPasswords.TabIndex = 19;
            this.rbOwnPasswords.TabStop = true;
            this.rbOwnPasswords.Text = "Provide own passwords";
            this.rbOwnPasswords.UseVisualStyleBackColor = true;
            // 
            // tbPassword1
            // 
            this.tbPassword1.Location = new System.Drawing.Point(90, 52);
            this.tbPassword1.Name = "tbPassword1";
            this.tbPassword1.Size = new System.Drawing.Size(130, 20);
            this.tbPassword1.TabIndex = 20;
            this.toolTipSettings.SetToolTip(this.tbPassword1, "Password to be used when TestTool creates/updates users");
            // 
            // rbEmbeddedPasswords
            // 
            this.rbEmbeddedPasswords.AutoSize = true;
            this.rbEmbeddedPasswords.Checked = true;
            this.rbEmbeddedPasswords.Location = new System.Drawing.Point(5, 6);
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
            this.tbPassword2.Location = new System.Drawing.Point(256, 52);
            this.tbPassword2.MaximumSize = new System.Drawing.Size(130, 20);
            this.tbPassword2.Name = "tbPassword2";
            this.tbPassword2.Size = new System.Drawing.Size(129, 20);
            this.tbPassword2.TabIndex = 21;
            this.toolTipSettings.SetToolTip(this.tbPassword2, "Second password to be used when TestTool creates/updates users");
            // 
            // cmbSecureMethod
            // 
            this.cmbSecureMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecureMethod.FormattingEnabled = true;
            this.cmbSecureMethod.Location = new System.Drawing.Point(155, 78);
            this.cmbSecureMethod.Name = "cmbSecureMethod";
            this.cmbSecureMethod.Size = new System.Drawing.Size(231, 21);
            this.cmbSecureMethod.TabIndex = 38;
            this.toolTipSettings.SetToolTip(this.cmbSecureMethod, "Method which requires user authentication to be used in security tests.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Secure method:";
            // 
            // tbSubscriptionTimeout
            // 
            this.tbSubscriptionTimeout.Location = new System.Drawing.Point(150, 135);
            this.tbSubscriptionTimeout.Name = "tbSubscriptionTimeout";
            this.tbSubscriptionTimeout.Size = new System.Drawing.Size(125, 20);
            this.tbSubscriptionTimeout.TabIndex = 4;
            this.toolTipSettings.SetToolTip(this.tbSubscriptionTimeout, "Valid timeout for Subscribe operation");
            this.tbSubscriptionTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.tbSubscriptionTimeout_Validating);
            // 
            // lblSubscriptionTimeout
            // 
            this.lblSubscriptionTimeout.AutoSize = true;
            this.lblSubscriptionTimeout.Location = new System.Drawing.Point(3, 138);
            this.lblSubscriptionTimeout.Name = "lblSubscriptionTimeout";
            this.lblSubscriptionTimeout.Size = new System.Drawing.Size(123, 13);
            this.lblSubscriptionTimeout.TabIndex = 35;
            this.lblSubscriptionTimeout.Text = "Subscription Timeout (s):";
            // 
            // btnGetTopics
            // 
            this.btnGetTopics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetTopics.Location = new System.Drawing.Point(298, 38);
            this.btnGetTopics.Name = "btnGetTopics";
            this.btnGetTopics.Size = new System.Drawing.Size(88, 23);
            this.btnGetTopics.TabIndex = 2;
            this.btnGetTopics.Text = "Get";
            this.btnGetTopics.UseVisualStyleBackColor = true;
            this.btnGetTopics.Click += new System.EventHandler(this.btnGetTopics_Click);
            // 
            // cmbEventTopic
            // 
            this.cmbEventTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEventTopic.FormattingEnabled = true;
            this.cmbEventTopic.Location = new System.Drawing.Point(150, 11);
            this.cmbEventTopic.Name = "cmbEventTopic";
            this.cmbEventTopic.Size = new System.Drawing.Size(235, 21);
            this.cmbEventTopic.TabIndex = 1;
            this.toolTipSettings.SetToolTip(this.cmbEventTopic, "Event topic to be used as filter in Event test cases. Can be selected or entered " +
                    "manually");
            this.cmbEventTopic.SelectedIndexChanged += new System.EventHandler(this.cmbEventTopic_SelectedIndexChanged);
            // 
            // lblEventTopic
            // 
            this.lblEventTopic.AutoSize = true;
            this.lblEventTopic.Location = new System.Drawing.Point(3, 14);
            this.lblEventTopic.Name = "lblEventTopic";
            this.lblEventTopic.Size = new System.Drawing.Size(61, 13);
            this.lblEventTopic.TabIndex = 32;
            this.lblEventTopic.Text = "Event topic";
            // 
            // gbMisc
            // 
            this.gbMisc.Controls.Add(this.tcMiscSettings);
            this.gbMisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMisc.Location = new System.Drawing.Point(0, 250);
            this.gbMisc.Name = "gbMisc";
            this.gbMisc.Size = new System.Drawing.Size(410, 290);
            this.gbMisc.TabIndex = 9;
            this.gbMisc.TabStop = false;
            this.gbMisc.Text = "Miscellaneous";
            // 
            // tcMiscSettings
            // 
            this.tcMiscSettings.Controls.Add(this.tpSecurity);
            this.tcMiscSettings.Controls.Add(this.tpPTZ);
            this.tcMiscSettings.Controls.Add(this.tpEvents);
            this.tcMiscSettings.Controls.Add(this.tpIO);
            this.tcMiscSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMiscSettings.Location = new System.Drawing.Point(3, 16);
            this.tcMiscSettings.Name = "tcMiscSettings";
            this.tcMiscSettings.SelectedIndex = 0;
            this.tcMiscSettings.Size = new System.Drawing.Size(404, 271);
            this.tcMiscSettings.TabIndex = 42;
            // 
            // tpSecurity
            // 
            this.tpSecurity.Controls.Add(this.rbEmbeddedPasswords);
            this.tpSecurity.Controls.Add(this.tbPassword1);
            this.tpSecurity.Controls.Add(this.lblPasswords);
            this.tpSecurity.Controls.Add(this.cmbSecureMethod);
            this.tpSecurity.Controls.Add(this.rbOwnPasswords);
            this.tpSecurity.Controls.Add(this.label2);
            this.tpSecurity.Controls.Add(this.tbPassword2);
            this.tpSecurity.Location = new System.Drawing.Point(4, 22);
            this.tpSecurity.Name = "tpSecurity";
            this.tpSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tpSecurity.Size = new System.Drawing.Size(396, 245);
            this.tpSecurity.TabIndex = 0;
            this.tpSecurity.Text = "Security";
            this.tpSecurity.ToolTipText = "Custom values for Security test cases";
            this.tpSecurity.UseVisualStyleBackColor = true;
            // 
            // tpPTZ
            // 
            this.tpPTZ.Controls.Add(this.btnGetVideoSources);
            this.tpPTZ.Controls.Add(this.cmbVideoSource);
            this.tpPTZ.Controls.Add(this.lblVideoSource);
            this.tpPTZ.Controls.Add(this.btnGetPTZNodes);
            this.tpPTZ.Controls.Add(this.cmbPTZNodes);
            this.tpPTZ.Controls.Add(this.lblPtzNode);
            this.tpPTZ.Location = new System.Drawing.Point(4, 22);
            this.tpPTZ.Name = "tpPTZ";
            this.tpPTZ.Padding = new System.Windows.Forms.Padding(3);
            this.tpPTZ.Size = new System.Drawing.Size(396, 245);
            this.tpPTZ.TabIndex = 1;
            this.tpPTZ.Text = "PTZ";
            this.tpPTZ.ToolTipText = "Settings for performing PTZ tests";
            this.tpPTZ.UseVisualStyleBackColor = true;
            // 
            // btnGetVideoSources
            // 
            this.btnGetVideoSources.Location = new System.Drawing.Point(299, 38);
            this.btnGetVideoSources.Name = "btnGetVideoSources";
            this.btnGetVideoSources.Size = new System.Drawing.Size(88, 23);
            this.btnGetVideoSources.TabIndex = 34;
            this.btnGetVideoSources.Text = "Get";
            this.btnGetVideoSources.UseVisualStyleBackColor = true;
            this.btnGetVideoSources.Visible = false;
            this.btnGetVideoSources.Click += new System.EventHandler(this.btnGetVideoSources_Click);
            // 
            // cmbVideoSource
            // 
            this.cmbVideoSource.FormattingEnabled = true;
            this.cmbVideoSource.Location = new System.Drawing.Point(135, 40);
            this.cmbVideoSource.Name = "cmbVideoSource";
            this.cmbVideoSource.Size = new System.Drawing.Size(115, 21);
            this.cmbVideoSource.TabIndex = 33;
            this.toolTipSettings.SetToolTip(this.cmbVideoSource, "Video source usedfor tests execution");
            this.cmbVideoSource.Visible = false;
            // 
            // lblVideoSource
            // 
            this.lblVideoSource.AutoSize = true;
            this.lblVideoSource.Location = new System.Drawing.Point(3, 43);
            this.lblVideoSource.Name = "lblVideoSource";
            this.lblVideoSource.Size = new System.Drawing.Size(112, 13);
            this.lblVideoSource.TabIndex = 32;
            this.lblVideoSource.Text = "Video source for tests:";
            this.lblVideoSource.Visible = false;
            // 
            // tpEvents
            // 
            this.tpEvents.Controls.Add(this.lblEventTopic);
            this.tpEvents.Controls.Add(this.tbNamespaces);
            this.tpEvents.Controls.Add(this.btnGetTopics);
            this.tpEvents.Controls.Add(this.lblNamespaces);
            this.tpEvents.Controls.Add(this.cmbEventTopic);
            this.tpEvents.Controls.Add(this.tbSubscriptionTimeout);
            this.tpEvents.Controls.Add(this.lblSubscriptionTimeout);
            this.tpEvents.Location = new System.Drawing.Point(4, 22);
            this.tpEvents.Name = "tpEvents";
            this.tpEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpEvents.Size = new System.Drawing.Size(396, 245);
            this.tpEvents.TabIndex = 2;
            this.tpEvents.Text = "Events";
            this.tpEvents.ToolTipText = "Custom values for Events test cases";
            this.tpEvents.UseVisualStyleBackColor = true;
            // 
            // tbNamespaces
            // 
            this.tbNamespaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNamespaces.Location = new System.Drawing.Point(150, 73);
            this.tbNamespaces.Multiline = true;
            this.tbNamespaces.Name = "tbNamespaces";
            this.tbNamespaces.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbNamespaces.Size = new System.Drawing.Size(235, 56);
            this.tbNamespaces.TabIndex = 3;
            this.toolTipSettings.SetToolTip(this.tbNamespaces, "Namespaces definition for the Event Topic above");
            this.tbNamespaces.Leave += new System.EventHandler(this.tbNamespaces_Leave);
            // 
            // lblNamespaces
            // 
            this.lblNamespaces.AutoSize = true;
            this.lblNamespaces.Location = new System.Drawing.Point(3, 73);
            this.lblNamespaces.Name = "lblNamespaces";
            this.lblNamespaces.Size = new System.Drawing.Size(97, 13);
            this.lblNamespaces.TabIndex = 39;
            this.lblNamespaces.Text = "Topic namespaces";
            // 
            // tpIO
            // 
            this.tpIO.Controls.Add(this.tbRelayOutputsDelayMonostable);
            this.tpIO.Controls.Add(this.lblRelayOutputsDelay);
            this.tpIO.Location = new System.Drawing.Point(4, 22);
            this.tpIO.Name = "tpIO";
            this.tpIO.Padding = new System.Windows.Forms.Padding(3);
            this.tpIO.Size = new System.Drawing.Size(396, 245);
            this.tpIO.TabIndex = 3;
            this.tpIO.Text = "I/O";
            this.tpIO.UseVisualStyleBackColor = true;
            // 
            // tbRelayOutputsDelayMonostable
            // 
            this.tbRelayOutputsDelayMonostable.Location = new System.Drawing.Point(150, 11);
            this.tbRelayOutputsDelayMonostable.Name = "tbRelayOutputsDelayMonostable";
            this.tbRelayOutputsDelayMonostable.Size = new System.Drawing.Size(113, 20);
            this.tbRelayOutputsDelayMonostable.TabIndex = 45;
            this.tbRelayOutputsDelayMonostable.Validating += new System.ComponentModel.CancelEventHandler(this.tbRelayOutputDelayTimeMonostable_Validating);
            // 
            // lblRelayOutputsDelay
            // 
            this.lblRelayOutputsDelay.AutoSize = true;
            this.lblRelayOutputsDelay.Location = new System.Drawing.Point(3, 14);
            this.lblRelayOutputsDelay.Name = "lblRelayOutputsDelay";
            this.lblRelayOutputsDelay.Size = new System.Drawing.Size(139, 13);
            this.lblRelayOutputsDelay.TabIndex = 44;
            this.lblRelayOutputsDelay.Text = "Relay outputs delay time (s):";
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMisc);
            this.Controls.Add(this.gbEnvironment);
            this.Controls.Add(this.gbTimeouts);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(410, 540);
            this.gbTimeouts.ResumeLayout(false);
            this.gbTimeouts.PerformLayout();
            this.gbEnvironment.ResumeLayout(false);
            this.gbEnvironment.PerformLayout();
            this.gbMisc.ResumeLayout(false);
            this.tcMiscSettings.ResumeLayout(false);
            this.tpSecurity.ResumeLayout(false);
            this.tpSecurity.PerformLayout();
            this.tpPTZ.ResumeLayout(false);
            this.tpPTZ.PerformLayout();
            this.tpEvents.ResumeLayout(false);
            this.tpEvents.PerformLayout();
            this.tpIO.ResumeLayout(false);
            this.tpIO.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTimeouts;
        private System.Windows.Forms.TextBox tbOperationDelay;
        private System.Windows.Forms.Label lblTimeBetweenTests;
        private System.Windows.Forms.Label lblOperationDelay;
        private System.Windows.Forms.TextBox tbTimeBetweenTests;
        private System.Windows.Forms.Label lblRebootTime;
        private System.Windows.Forms.TextBox tbRebootTimeout;
        private System.Windows.Forms.Label lblMessageTimeout;
        private System.Windows.Forms.TextBox tbMessageTimeout;
        private System.Windows.Forms.GroupBox gbEnvironment;
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
        private System.Windows.Forms.Button btnGetPTZNodes;
        private System.Windows.Forms.Label lblPtzNode;
        private System.Windows.Forms.ComboBox cmbPTZNodes;
        private System.Windows.Forms.Label lblPasswords;
        private System.Windows.Forms.RadioButton rbOwnPasswords;
        private System.Windows.Forms.TextBox tbPassword1;
        private System.Windows.Forms.RadioButton rbEmbeddedPasswords;
        private System.Windows.Forms.TextBox tbPassword2;
        private System.Windows.Forms.ComboBox cmbSecureMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSubscriptionTimeout;
        private System.Windows.Forms.Label lblSubscriptionTimeout;
        private System.Windows.Forms.Button btnGetTopics;
        private System.Windows.Forms.ComboBox cmbEventTopic;
        private System.Windows.Forms.Label lblEventTopic;
        private System.Windows.Forms.GroupBox gbMisc;
        private System.Windows.Forms.TextBox tbSafetyDelay;
        private System.Windows.Forms.Label lblSafetyDelay;
        private System.Windows.Forms.TextBox tbNamespaces;
        private System.Windows.Forms.Label lblNamespaces;
        private System.Windows.Forms.TabControl tcMiscSettings;
        private System.Windows.Forms.TabPage tpSecurity;
        private System.Windows.Forms.TabPage tpPTZ;
        private System.Windows.Forms.TabPage tpEvents;
        private System.Windows.Forms.ToolTip toolTipSettings;
        private System.Windows.Forms.ComboBox cmbVideoSource;
        private System.Windows.Forms.Label lblVideoSource;
        private System.Windows.Forms.Button btnGetVideoSources;
        private System.Windows.Forms.TabPage tpIO;
        //private System.Windows.Forms.TextBox tbRelayOutputsDelayBistable;
        //private System.Windows.Forms.Label labelBistable;
        //private System.Windows.Forms.Label labelMonostableMode;
        private System.Windows.Forms.TextBox tbRelayOutputsDelayMonostable;
        private System.Windows.Forms.Label lblRelayOutputsDelay;
    }
}
