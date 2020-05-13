namespace TestTool.GUI.Controls.Device
{
    partial class DeviceManagementPage
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
            this.btnSetIPAddress = new System.Windows.Forms.Button();
            this.btnGetHostname = new System.Windows.Forms.Button();
            this.btnGetInterfaces = new System.Windows.Forms.Button();
            this.btnReboot = new System.Windows.Forms.Button();
            this.btnHardReset = new System.Windows.Forms.Button();
            this.btnProbe = new System.Windows.Forms.Button();
            this.btnDeviceInfo = new System.Windows.Forms.Button();
            this.lblHardware = new System.Windows.Forms.Label();
            this.tbHardware = new System.Windows.Forms.TextBox();
            this.lblFirmware = new System.Windows.Forms.Label();
            this.tbFirmware = new System.Windows.Forms.TextBox();
            this.lblSerial = new System.Windows.Forms.Label();
            this.tbSerial = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.tbManufacturer = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.btnSetGateway = new System.Windows.Forms.Button();
            this.btnSyncTime = new System.Windows.Forms.Button();
            this.deviceManagementPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnGetCapabilities = new System.Windows.Forms.Button();
            this.btnGetServices = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSetIPAddress
            // 
            this.btnSetIPAddress.Location = new System.Drawing.Point(99, 276);
            this.btnSetIPAddress.Name = "btnSetIPAddress";
            this.btnSetIPAddress.Size = new System.Drawing.Size(90, 23);
            this.btnSetIPAddress.TabIndex = 14;
            this.btnSetIPAddress.Text = "Set IP Address";
            this.deviceManagementPageToolTip.SetToolTip(this.btnSetIPAddress, "Set IP address");
            this.btnSetIPAddress.UseVisualStyleBackColor = true;
            this.btnSetIPAddress.Click += new System.EventHandler(this.btnSetIPAddress_Click);
            // 
            // btnGetHostname
            // 
            this.btnGetHostname.Location = new System.Drawing.Point(3, 247);
            this.btnGetHostname.Name = "btnGetHostname";
            this.btnGetHostname.Size = new System.Drawing.Size(90, 23);
            this.btnGetHostname.TabIndex = 13;
            this.btnGetHostname.Text = "Get Hostname";
            this.deviceManagementPageToolTip.SetToolTip(this.btnGetHostname, "Get hostname");
            this.btnGetHostname.UseVisualStyleBackColor = true;
            this.btnGetHostname.Click += new System.EventHandler(this.btnGetHostname_Click);
            // 
            // btnGetInterfaces
            // 
            this.btnGetInterfaces.Location = new System.Drawing.Point(99, 247);
            this.btnGetInterfaces.Name = "btnGetInterfaces";
            this.btnGetInterfaces.Size = new System.Drawing.Size(90, 23);
            this.btnGetInterfaces.TabIndex = 12;
            this.btnGetInterfaces.Text = "Get Interfaces";
            this.deviceManagementPageToolTip.SetToolTip(this.btnGetInterfaces, "Get network interfaces");
            this.btnGetInterfaces.UseVisualStyleBackColor = true;
            this.btnGetInterfaces.Click += new System.EventHandler(this.btnGetInterfaces_Click);
            // 
            // btnReboot
            // 
            this.btnReboot.Location = new System.Drawing.Point(99, 218);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(90, 23);
            this.btnReboot.TabIndex = 11;
            this.btnReboot.Text = "Reboot";
            this.deviceManagementPageToolTip.SetToolTip(this.btnReboot, "Reboot device");
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // btnHardReset
            // 
            this.btnHardReset.Location = new System.Drawing.Point(195, 218);
            this.btnHardReset.Name = "btnHardReset";
            this.btnHardReset.Size = new System.Drawing.Size(90, 23);
            this.btnHardReset.TabIndex = 10;
            this.btnHardReset.Text = "Hard Reset";
            this.deviceManagementPageToolTip.SetToolTip(this.btnHardReset, "Set system factory defaults");
            this.btnHardReset.UseVisualStyleBackColor = true;
            this.btnHardReset.Click += new System.EventHandler(this.btnHardReset_Click);
            // 
            // btnProbe
            // 
            this.btnProbe.Location = new System.Drawing.Point(3, 218);
            this.btnProbe.Name = "btnProbe";
            this.btnProbe.Size = new System.Drawing.Size(90, 23);
            this.btnProbe.TabIndex = 9;
            this.btnProbe.Text = "Get Scopes";
            this.deviceManagementPageToolTip.SetToolTip(this.btnProbe, "Get device scopes");
            this.btnProbe.UseVisualStyleBackColor = true;
            this.btnProbe.Click += new System.EventHandler(this.btnProbe_Click);
            // 
            // btnDeviceInfo
            // 
            this.btnDeviceInfo.Location = new System.Drawing.Point(3, 189);
            this.btnDeviceInfo.Name = "btnDeviceInfo";
            this.btnDeviceInfo.Size = new System.Drawing.Size(90, 23);
            this.btnDeviceInfo.TabIndex = 8;
            this.btnDeviceInfo.Text = "Device Info";
            this.deviceManagementPageToolTip.SetToolTip(this.btnDeviceInfo, "Get device information");
            this.btnDeviceInfo.UseVisualStyleBackColor = true;
            this.btnDeviceInfo.Click += new System.EventHandler(this.btnDeviceInfo_Click);
            // 
            // lblHardware
            // 
            this.lblHardware.AutoSize = true;
            this.lblHardware.Location = new System.Drawing.Point(3, 137);
            this.lblHardware.Name = "lblHardware";
            this.lblHardware.Size = new System.Drawing.Size(56, 13);
            this.lblHardware.TabIndex = 55;
            this.lblHardware.Text = "Hardware:";
            // 
            // tbHardware
            // 
            this.tbHardware.Location = new System.Drawing.Point(139, 134);
            this.tbHardware.Name = "tbHardware";
            this.tbHardware.ReadOnly = true;
            this.tbHardware.Size = new System.Drawing.Size(226, 20);
            this.tbHardware.TabIndex = 6;
            // 
            // lblFirmware
            // 
            this.lblFirmware.AutoSize = true;
            this.lblFirmware.Location = new System.Drawing.Point(3, 163);
            this.lblFirmware.Name = "lblFirmware";
            this.lblFirmware.Size = new System.Drawing.Size(52, 13);
            this.lblFirmware.TabIndex = 53;
            this.lblFirmware.Text = "Firmware:";
            // 
            // tbFirmware
            // 
            this.tbFirmware.Location = new System.Drawing.Point(139, 160);
            this.tbFirmware.Name = "tbFirmware";
            this.tbFirmware.ReadOnly = true;
            this.tbFirmware.Size = new System.Drawing.Size(226, 20);
            this.tbFirmware.TabIndex = 7;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(3, 111);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(76, 13);
            this.lblSerial.TabIndex = 51;
            this.lblSerial.Text = "Serial Number:";
            // 
            // tbSerial
            // 
            this.tbSerial.Location = new System.Drawing.Point(139, 108);
            this.tbSerial.Name = "tbSerial";
            this.tbSerial.ReadOnly = true;
            this.tbSerial.Size = new System.Drawing.Size(226, 20);
            this.tbSerial.TabIndex = 5;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(3, 85);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 49;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Location = new System.Drawing.Point(139, 82);
            this.tbModel.Name = "tbModel";
            this.tbModel.ReadOnly = true;
            this.tbModel.Size = new System.Drawing.Size(226, 20);
            this.tbModel.TabIndex = 4;
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Location = new System.Drawing.Point(3, 59);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(73, 13);
            this.lblManufacturer.TabIndex = 47;
            this.lblManufacturer.Text = "Manufacturer:";
            // 
            // tbManufacturer
            // 
            this.tbManufacturer.Location = new System.Drawing.Point(139, 56);
            this.tbManufacturer.Name = "tbManufacturer";
            this.tbManufacturer.ReadOnly = true;
            this.tbManufacturer.Size = new System.Drawing.Size(226, 20);
            this.tbManufacturer.TabIndex = 3;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(3, 33);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 45;
            this.lblIP.Text = "IP:";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(139, 30);
            this.tbIP.Name = "tbIP";
            this.tbIP.ReadOnly = true;
            this.tbIP.Size = new System.Drawing.Size(226, 20);
            this.tbIP.TabIndex = 2;
            // 
            // tbReport
            // 
            this.tbReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReport.Location = new System.Drawing.Point(371, 4);
            this.tbReport.Multiline = true;
            this.tbReport.Name = "tbReport";
            this.tbReport.ReadOnly = true;
            this.tbReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbReport.Size = new System.Drawing.Size(396, 480);
            this.tbReport.TabIndex = 17;
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(139, 4);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.ReadOnly = true;
            this.tbUrl.Size = new System.Drawing.Size(226, 20);
            this.tbUrl.TabIndex = 1;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(3, 7);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(134, 13);
            this.lblURL.TabIndex = 41;
            this.lblURL.Text = "Device Management URL:";
            // 
            // btnSetGateway
            // 
            this.btnSetGateway.Location = new System.Drawing.Point(3, 276);
            this.btnSetGateway.Name = "btnSetGateway";
            this.btnSetGateway.Size = new System.Drawing.Size(90, 23);
            this.btnSetGateway.TabIndex = 15;
            this.btnSetGateway.Text = "Set Gateway";
            this.deviceManagementPageToolTip.SetToolTip(this.btnSetGateway, "Set default gateway");
            this.btnSetGateway.UseVisualStyleBackColor = true;
            this.btnSetGateway.Click += new System.EventHandler(this.btnSetGateway_Click);
            // 
            // btnSyncTime
            // 
            this.btnSyncTime.Location = new System.Drawing.Point(195, 247);
            this.btnSyncTime.Name = "btnSyncTime";
            this.btnSyncTime.Size = new System.Drawing.Size(90, 23);
            this.btnSyncTime.TabIndex = 16;
            this.btnSyncTime.Text = "Sync Time";
            this.deviceManagementPageToolTip.SetToolTip(this.btnSyncTime, "Set current time");
            this.btnSyncTime.UseVisualStyleBackColor = true;
            this.btnSyncTime.Click += new System.EventHandler(this.btnSyncTime_Click);
            // 
            // btnGetCapabilities
            // 
            this.btnGetCapabilities.Location = new System.Drawing.Point(99, 189);
            this.btnGetCapabilities.Name = "btnGetCapabilities";
            this.btnGetCapabilities.Size = new System.Drawing.Size(90, 23);
            this.btnGetCapabilities.TabIndex = 56;
            this.btnGetCapabilities.Text = "Get Capabilities";
            this.btnGetCapabilities.UseVisualStyleBackColor = true;
            this.btnGetCapabilities.Click += new System.EventHandler(this.btnGetCapabilities_Click);
            // 
            // btnGetServices
            // 
            this.btnGetServices.Location = new System.Drawing.Point(195, 189);
            this.btnGetServices.Name = "btnGetServices";
            this.btnGetServices.Size = new System.Drawing.Size(90, 23);
            this.btnGetServices.TabIndex = 57;
            this.btnGetServices.Text = "Get Services";
            this.btnGetServices.UseVisualStyleBackColor = true;
            this.btnGetServices.Click += new System.EventHandler(this.btnGetServices_Click);
            // 
            // DeviceManagementPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGetServices);
            this.Controls.Add(this.btnGetCapabilities);
            this.Controls.Add(this.btnSyncTime);
            this.Controls.Add(this.btnSetGateway);
            this.Controls.Add(this.btnSetIPAddress);
            this.Controls.Add(this.btnGetHostname);
            this.Controls.Add(this.btnGetInterfaces);
            this.Controls.Add(this.btnReboot);
            this.Controls.Add(this.btnHardReset);
            this.Controls.Add(this.btnProbe);
            this.Controls.Add(this.btnDeviceInfo);
            this.Controls.Add(this.lblHardware);
            this.Controls.Add(this.tbHardware);
            this.Controls.Add(this.lblFirmware);
            this.Controls.Add(this.tbFirmware);
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.tbSerial);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.tbModel);
            this.Controls.Add(this.lblManufacturer);
            this.Controls.Add(this.tbManufacturer);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.tbReport);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.lblURL);
            this.Name = "DeviceManagementPage";
            this.Size = new System.Drawing.Size(775, 496);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetIPAddress;
        private System.Windows.Forms.Button btnGetHostname;
        private System.Windows.Forms.Button btnGetInterfaces;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.Button btnHardReset;
        private System.Windows.Forms.Button btnProbe;
        private System.Windows.Forms.Button btnDeviceInfo;
        private System.Windows.Forms.Label lblHardware;
        private System.Windows.Forms.TextBox tbHardware;
        private System.Windows.Forms.Label lblFirmware;
        private System.Windows.Forms.TextBox tbFirmware;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox tbSerial;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox tbModel;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.TextBox tbManufacturer;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Button btnSetGateway;
        private System.Windows.Forms.Button btnSyncTime;
        private System.Windows.Forms.ToolTip deviceManagementPageToolTip;
        private System.Windows.Forms.Button btnGetCapabilities;
        private System.Windows.Forms.Button btnGetServices;
    }
}
