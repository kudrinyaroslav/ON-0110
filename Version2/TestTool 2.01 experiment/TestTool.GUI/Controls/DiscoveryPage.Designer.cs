using System.Drawing;

namespace TestTool.GUI.Controls
{
    partial class DiscoveryPage
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
            this.cmbNICs = new System.Windows.Forms.ComboBox();
            this.lblNIC = new System.Windows.Forms.Label();
            this.gbDutInfo = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lblFirmwareVersion = new System.Windows.Forms.Label();
            this.tbFirmwareVersion = new System.Windows.Forms.TextBox();
            this.lblSerial = new System.Windows.Forms.Label();
            this.tbSerial = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.btnCheckService = new System.Windows.Forms.Button();
            this.lblMetadataVersion = new System.Windows.Forms.Label();
            this.tbMetadataVersion = new System.Windows.Forms.TextBox();
            this.cmbServiceAddress = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCheckIP = new System.Windows.Forms.Button();
            this.lblScopes = new System.Windows.Forms.Label();
            this.tbScopes = new System.Windows.Forms.TextBox();
            this.lblDeviceServiceAddress = new System.Windows.Forms.Label();
            this.lblDeviceIP = new System.Windows.Forms.Label();
            this.tbDeviceIP = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.lblEpAddress = new System.Windows.Forms.Label();
            this.tbEpAddress = new System.Windows.Forms.TextBox();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.hdrIP = new System.Windows.Forms.ColumnHeader();
            this.hdrUUID = new System.Windows.Forms.ColumnHeader();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.checkHWStyle = new System.Windows.Forms.CheckBox();
            this.splitContainerDiscovery = new System.Windows.Forms.SplitContainer();
            this.discoveryPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbDutInfo.SuspendLayout();
            this.splitContainerDiscovery.Panel1.SuspendLayout();
            this.splitContainerDiscovery.Panel2.SuspendLayout();
            this.splitContainerDiscovery.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbNICs
            // 
            this.cmbNICs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNICs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNICs.FormattingEnabled = true;
            this.cmbNICs.Location = new System.Drawing.Point(44, 17);
            this.cmbNICs.Name = "cmbNICs";
            this.cmbNICs.Size = new System.Drawing.Size(298, 21);
            this.cmbNICs.TabIndex = 1;
            // 
            // lblNIC
            // 
            this.lblNIC.AutoSize = true;
            this.lblNIC.Location = new System.Drawing.Point(10, 21);
            this.lblNIC.Name = "lblNIC";
            this.lblNIC.Size = new System.Drawing.Size(28, 13);
            this.lblNIC.TabIndex = 9;
            this.lblNIC.Text = "NIC:";
            // 
            // gbDutInfo
            // 
            this.gbDutInfo.Controls.Add(this.lblPassword);
            this.gbDutInfo.Controls.Add(this.tbPassword);
            this.gbDutInfo.Controls.Add(this.lblUsername);
            this.gbDutInfo.Controls.Add(this.tbUsername);
            this.gbDutInfo.Controls.Add(this.lblFirmwareVersion);
            this.gbDutInfo.Controls.Add(this.tbFirmwareVersion);
            this.gbDutInfo.Controls.Add(this.lblSerial);
            this.gbDutInfo.Controls.Add(this.tbSerial);
            this.gbDutInfo.Controls.Add(this.lblModel);
            this.gbDutInfo.Controls.Add(this.tbModel);
            this.gbDutInfo.Controls.Add(this.lblBrand);
            this.gbDutInfo.Controls.Add(this.tbBrand);
            this.gbDutInfo.Controls.Add(this.btnCheckService);
            this.gbDutInfo.Controls.Add(this.lblMetadataVersion);
            this.gbDutInfo.Controls.Add(this.tbMetadataVersion);
            this.gbDutInfo.Controls.Add(this.cmbServiceAddress);
            this.gbDutInfo.Controls.Add(this.btnClear);
            this.gbDutInfo.Controls.Add(this.btnCheckIP);
            this.gbDutInfo.Controls.Add(this.lblScopes);
            this.gbDutInfo.Controls.Add(this.tbScopes);
            this.gbDutInfo.Controls.Add(this.lblDeviceServiceAddress);
            this.gbDutInfo.Controls.Add(this.lblDeviceIP);
            this.gbDutInfo.Controls.Add(this.tbDeviceIP);
            this.gbDutInfo.Controls.Add(this.lblType);
            this.gbDutInfo.Controls.Add(this.tbType);
            this.gbDutInfo.Controls.Add(this.lblEpAddress);
            this.gbDutInfo.Controls.Add(this.tbEpAddress);
            this.gbDutInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDutInfo.Location = new System.Drawing.Point(0, 0);
            this.gbDutInfo.Name = "gbDutInfo";
            this.gbDutInfo.Size = new System.Drawing.Size(467, 475);
            this.gbDutInfo.TabIndex = 8;
            this.gbDutInfo.TabStop = false;
            this.gbDutInfo.Text = "Device Under Test Information";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 152);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 39;
            this.lblPassword.Text = "Password:";
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(143, 147);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(318, 20);
            this.tbPassword.TabIndex = 12;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 126);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(63, 13);
            this.lblUsername.TabIndex = 38;
            this.lblUsername.Text = "User Name:";
            // 
            // tbUsername
            // 
            this.tbUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUsername.Location = new System.Drawing.Point(143, 123);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(318, 20);
            this.tbUsername.TabIndex = 11;
            // 
            // lblFirmwareVersion
            // 
            this.lblFirmwareVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFirmwareVersion.AutoSize = true;
            this.lblFirmwareVersion.Location = new System.Drawing.Point(6, 423);
            this.lblFirmwareVersion.Name = "lblFirmwareVersion";
            this.lblFirmwareVersion.Size = new System.Drawing.Size(90, 13);
            this.lblFirmwareVersion.TabIndex = 35;
            this.lblFirmwareVersion.Text = "Firmware Version:";
            // 
            // tbFirmwareVersion
            // 
            this.tbFirmwareVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFirmwareVersion.Location = new System.Drawing.Point(143, 420);
            this.tbFirmwareVersion.Name = "tbFirmwareVersion";
            this.tbFirmwareVersion.ReadOnly = true;
            this.tbFirmwareVersion.Size = new System.Drawing.Size(318, 20);
            this.tbFirmwareVersion.TabIndex = 18;
            this.tbFirmwareVersion.Text = "<Press Check to get information>";
            // 
            // lblSerial
            // 
            this.lblSerial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(6, 397);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(76, 13);
            this.lblSerial.TabIndex = 33;
            this.lblSerial.Text = "Serial Number:";
            // 
            // tbSerial
            // 
            this.tbSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSerial.Location = new System.Drawing.Point(143, 394);
            this.tbSerial.Name = "tbSerial";
            this.tbSerial.ReadOnly = true;
            this.tbSerial.Size = new System.Drawing.Size(318, 20);
            this.tbSerial.TabIndex = 17;
            this.tbSerial.Text = "<Press Check to get information>";
            // 
            // lblModel
            // 
            this.lblModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(6, 371);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 31;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModel.Location = new System.Drawing.Point(143, 368);
            this.tbModel.Name = "tbModel";
            this.tbModel.ReadOnly = true;
            this.tbModel.Size = new System.Drawing.Size(318, 20);
            this.tbModel.TabIndex = 16;
            this.tbModel.Text = "<Press Check to get information>";
            // 
            // lblBrand
            // 
            this.lblBrand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(6, 345);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 29;
            this.lblBrand.Text = "Brand:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBrand.Location = new System.Drawing.Point(143, 342);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.ReadOnly = true;
            this.tbBrand.Size = new System.Drawing.Size(318, 20);
            this.tbBrand.TabIndex = 15;
            this.tbBrand.Text = "<Press Check to get information>";
            // 
            // btnCheckService
            // 
            this.btnCheckService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckService.Location = new System.Drawing.Point(403, 95);
            this.btnCheckService.Name = "btnCheckService";
            this.btnCheckService.Size = new System.Drawing.Size(58, 23);
            this.btnCheckService.TabIndex = 10;
            this.btnCheckService.Text = "Check";
            this.discoveryPageToolTip.SetToolTip(this.btnCheckService, "Get device information using address specified");
            this.btnCheckService.UseVisualStyleBackColor = true;
            this.btnCheckService.Click += new System.EventHandler(this.btnCheckService_Click);
            // 
            // lblMetadataVersion
            // 
            this.lblMetadataVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMetadataVersion.AutoSize = true;
            this.lblMetadataVersion.Location = new System.Drawing.Point(6, 319);
            this.lblMetadataVersion.Name = "lblMetadataVersion";
            this.lblMetadataVersion.Size = new System.Drawing.Size(93, 13);
            this.lblMetadataVersion.TabIndex = 26;
            this.lblMetadataVersion.Text = "Metadata Version:";
            // 
            // tbMetadataVersion
            // 
            this.tbMetadataVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMetadataVersion.Location = new System.Drawing.Point(143, 316);
            this.tbMetadataVersion.Name = "tbMetadataVersion";
            this.tbMetadataVersion.ReadOnly = true;
            this.tbMetadataVersion.Size = new System.Drawing.Size(318, 20);
            this.tbMetadataVersion.TabIndex = 14;
            // 
            // cmbServiceAddress
            // 
            this.cmbServiceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbServiceAddress.FormattingEnabled = true;
            this.cmbServiceAddress.Location = new System.Drawing.Point(143, 96);
            this.cmbServiceAddress.Name = "cmbServiceAddress";
            this.cmbServiceAddress.Size = new System.Drawing.Size(254, 21);
            this.cmbServiceAddress.TabIndex = 9;
            this.cmbServiceAddress.SelectedIndexChanged += new System.EventHandler(this.cmbServiceAddress_SelectedIndexChanged);
            this.cmbServiceAddress.Leave += new System.EventHandler(this.cmbServiceAddress_Leave);
            this.cmbServiceAddress.Enter += new System.EventHandler(this.cmbServiceAddress_Enter);
            this.cmbServiceAddress.TextUpdate += new System.EventHandler(this.cmbServiceAddress_TextUpdate);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(401, 446);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 23);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "Clear";
            this.discoveryPageToolTip.SetToolTip(this.btnClear, "Clear device information");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCheckIP
            // 
            this.btnCheckIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckIP.Location = new System.Drawing.Point(403, 68);
            this.btnCheckIP.Name = "btnCheckIP";
            this.btnCheckIP.Size = new System.Drawing.Size(58, 23);
            this.btnCheckIP.TabIndex = 8;
            this.btnCheckIP.Text = "Probe";
            this.discoveryPageToolTip.SetToolTip(this.btnCheckIP, "Probe device");
            this.btnCheckIP.UseVisualStyleBackColor = true;
            this.btnCheckIP.Click += new System.EventHandler(this.btnCheckIP_Click);
            // 
            // lblScopes
            // 
            this.lblScopes.AutoSize = true;
            this.lblScopes.Location = new System.Drawing.Point(6, 178);
            this.lblScopes.Name = "lblScopes";
            this.lblScopes.Size = new System.Drawing.Size(46, 13);
            this.lblScopes.TabIndex = 15;
            this.lblScopes.Text = "Scopes:";
            // 
            // tbScopes
            // 
            this.tbScopes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbScopes.Location = new System.Drawing.Point(143, 173);
            this.tbScopes.Multiline = true;
            this.tbScopes.Name = "tbScopes";
            this.tbScopes.ReadOnly = true;
            this.tbScopes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbScopes.Size = new System.Drawing.Size(318, 134);
            this.tbScopes.TabIndex = 13;
            // 
            // lblDeviceServiceAddress
            // 
            this.lblDeviceServiceAddress.AutoSize = true;
            this.lblDeviceServiceAddress.Location = new System.Drawing.Point(6, 100);
            this.lblDeviceServiceAddress.Name = "lblDeviceServiceAddress";
            this.lblDeviceServiceAddress.Size = new System.Drawing.Size(124, 13);
            this.lblDeviceServiceAddress.TabIndex = 13;
            this.lblDeviceServiceAddress.Text = "Device Service Address:";
            // 
            // lblDeviceIP
            // 
            this.lblDeviceIP.AutoSize = true;
            this.lblDeviceIP.Location = new System.Drawing.Point(6, 74);
            this.lblDeviceIP.Name = "lblDeviceIP";
            this.lblDeviceIP.Size = new System.Drawing.Size(57, 13);
            this.lblDeviceIP.TabIndex = 11;
            this.lblDeviceIP.Text = "Device IP:";
            // 
            // tbDeviceIP
            // 
            this.tbDeviceIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceIP.Location = new System.Drawing.Point(143, 71);
            this.tbDeviceIP.Name = "tbDeviceIP";
            this.tbDeviceIP.Size = new System.Drawing.Size(254, 20);
            this.tbDeviceIP.TabIndex = 7;
            this.tbDeviceIP.TextChanged += new System.EventHandler(this.tbDeviceIP_TextChanged);
            this.tbDeviceIP.Leave += new System.EventHandler(this.tbDeviceIP_Leave);
            this.tbDeviceIP.Enter += new System.EventHandler(this.tbDeviceIP_Enter);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(6, 48);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 9;
            this.lblType.Text = "Type:";
            // 
            // tbType
            // 
            this.tbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbType.Location = new System.Drawing.Point(143, 45);
            this.tbType.Name = "tbType";
            this.tbType.ReadOnly = true;
            this.tbType.Size = new System.Drawing.Size(318, 20);
            this.tbType.TabIndex = 6;
            // 
            // lblEpAddress
            // 
            this.lblEpAddress.AutoSize = true;
            this.lblEpAddress.Location = new System.Drawing.Point(6, 22);
            this.lblEpAddress.Name = "lblEpAddress";
            this.lblEpAddress.Size = new System.Drawing.Size(65, 13);
            this.lblEpAddress.TabIndex = 7;
            this.lblEpAddress.Text = "EP Address:";
            // 
            // tbEpAddress
            // 
            this.tbEpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEpAddress.Location = new System.Drawing.Point(143, 19);
            this.tbEpAddress.Name = "tbEpAddress";
            this.tbEpAddress.ReadOnly = true;
            this.tbEpAddress.Size = new System.Drawing.Size(318, 20);
            this.tbEpAddress.TabIndex = 5;
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrIP,
            this.hdrUUID});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.GridLines = true;
            this.lvDevices.Location = new System.Drawing.Point(6, 94);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(336, 375);
            this.lvDevices.TabIndex = 4;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            this.lvDevices.SelectedIndexChanged += new System.EventHandler(this.lvDevices_SelectedIndexChanged);
            // 
            // hdrIP
            // 
            this.hdrIP.Text = "IP";
            this.hdrIP.Width = 107;
            // 
            // hdrUUID
            // 
            this.hdrUUID.Text = "UUID";
            this.hdrUUID.Width = 220;
            // 
            // btnDiscover
            // 
            this.btnDiscover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscover.Location = new System.Drawing.Point(6, 65);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(336, 23);
            this.btnDiscover.TabIndex = 3;
            this.btnDiscover.Text = "Discover Devices";
            this.discoveryPageToolTip.SetToolTip(this.btnDiscover, "Discover devices in the network");
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // checkHWStyle
            // 
            this.checkHWStyle.AutoSize = true;
            this.checkHWStyle.Location = new System.Drawing.Point(44, 43);
            this.checkHWStyle.Name = "checkHWStyle";
            this.checkHWStyle.Size = new System.Drawing.Size(137, 17);
            this.checkHWStyle.TabIndex = 2;
            this.checkHWStyle.Text = "Use Hardware Notation";
            this.checkHWStyle.UseVisualStyleBackColor = true;
            this.checkHWStyle.CheckedChanged += new System.EventHandler(this.checkHWStyle_CheckedChanged);
            // 
            // splitContainerDiscovery
            // 
            this.splitContainerDiscovery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDiscovery.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDiscovery.Size = new System.Drawing.Size(821, 475);
            this.splitContainerDiscovery.Name = "splitContainerDiscovery";
            // 
            // splitContainerDiscovery.Panel1
            // 
            this.splitContainerDiscovery.Panel1.Controls.Add(this.checkHWStyle);
            this.splitContainerDiscovery.Panel1.Controls.Add(this.cmbNICs);
            this.splitContainerDiscovery.Panel1.Controls.Add(this.lblNIC);
            this.splitContainerDiscovery.Panel1.Controls.Add(this.lvDevices);
            this.splitContainerDiscovery.Panel1.Controls.Add(this.btnDiscover);
            this.splitContainerDiscovery.Panel1MinSize = 300;
            // 
            // splitContainerDiscovery.Panel2
            // 
            this.splitContainerDiscovery.Panel2.Controls.Add(this.gbDutInfo);
            this.splitContainerDiscovery.Panel2MinSize = 400;
            this.splitContainerDiscovery.SplitterDistance = 350;
            this.splitContainerDiscovery.TabIndex = 10;
            // 
            // DiscoveryPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerDiscovery);
            this.Name = "DiscoveryPage";
            this.Size = new System.Drawing.Size(821, 475);
            this.gbDutInfo.ResumeLayout(false);
            this.gbDutInfo.PerformLayout();
            this.splitContainerDiscovery.Panel1.ResumeLayout(false);
            this.splitContainerDiscovery.Panel1.PerformLayout();
            this.splitContainerDiscovery.Panel2.ResumeLayout(false);
            this.splitContainerDiscovery.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbNICs;
        private System.Windows.Forms.Label lblNIC;
        private System.Windows.Forms.GroupBox gbDutInfo;
        private System.Windows.Forms.Label lblFirmwareVersion;
        private System.Windows.Forms.TextBox tbFirmwareVersion;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox tbSerial;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox tbModel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox tbBrand;
        private System.Windows.Forms.Button btnCheckService;
        private System.Windows.Forms.Label lblMetadataVersion;
        private System.Windows.Forms.TextBox tbMetadataVersion;
        private System.Windows.Forms.ComboBox cmbServiceAddress;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCheckIP;
        private System.Windows.Forms.Label lblScopes;
        private System.Windows.Forms.TextBox tbScopes;
        private System.Windows.Forms.Label lblDeviceServiceAddress;
        private System.Windows.Forms.Label lblDeviceIP;
        private System.Windows.Forms.TextBox tbDeviceIP;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label lblEpAddress;
        private System.Windows.Forms.TextBox tbEpAddress;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader hdrIP;
        private System.Windows.Forms.ColumnHeader hdrUUID;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.CheckBox checkHWStyle;
        private System.Windows.Forms.SplitContainer splitContainerDiscovery;
        private System.Windows.Forms.ToolTip discoveryPageToolTip;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox tbUsername;
    }
}
