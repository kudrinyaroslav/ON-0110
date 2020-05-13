using System.Drawing;

namespace TestTool.GUI.Controls
{
    partial class DevicePage
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
            this.tcDevice = new System.Windows.Forms.TabControl();
            this.tpDeviceManagement = new System.Windows.Forms.TabPage();
            this.deviceManagementPage = new TestTool.GUI.Controls.Device.DeviceManagementPage();
            this.tpMedia = new System.Windows.Forms.TabPage();
            this.mediaPage = new TestTool.GUI.Controls.Device.MediaPage();
            this.tpPtz = new System.Windows.Forms.TabPage();
            this.ptzPage = new TestTool.GUI.Controls.Device.PtzPage();
            this.tpRequests = new System.Windows.Forms.TabPage();
            this.requestsPage = new TestTool.GUI.Controls.Device.RequestsPage();
            this.gbCapabilitiesExchange = new System.Windows.Forms.GroupBox();
            this.rbGetServices = new System.Windows.Forms.RadioButton();
            this.rbGetCapabilities = new System.Windows.Forms.RadioButton();
            this.gbAuthentication = new System.Windows.Forms.GroupBox();
            this.rbDigest = new System.Windows.Forms.RadioButton();
            this.rbWsUsername = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelSpace = new System.Windows.Forms.Panel();
            this.tcDevice.SuspendLayout();
            this.tpDeviceManagement.SuspendLayout();
            this.tpMedia.SuspendLayout();
            this.tpPtz.SuspendLayout();
            this.tpRequests.SuspendLayout();
            this.gbCapabilitiesExchange.SuspendLayout();
            this.gbAuthentication.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDevice
            // 
            this.tcDevice.Controls.Add(this.tpDeviceManagement);
            this.tcDevice.Controls.Add(this.tpMedia);
            this.tcDevice.Controls.Add(this.tpPtz);
            this.tcDevice.Controls.Add(this.tpRequests);
            this.tcDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDevice.Location = new System.Drawing.Point(0, 51);
            this.tcDevice.Name = "tcDevice";
            this.tcDevice.SelectedIndex = 0;
            this.tcDevice.Size = new System.Drawing.Size(738, 448);
            this.tcDevice.TabIndex = 0;
            // 
            // tpDeviceManagement
            // 
            this.tpDeviceManagement.Controls.Add(this.deviceManagementPage);
            this.tpDeviceManagement.Location = new System.Drawing.Point(4, 22);
            this.tpDeviceManagement.Name = "tpDeviceManagement";
            this.tpDeviceManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpDeviceManagement.Size = new System.Drawing.Size(730, 422);
            this.tpDeviceManagement.TabIndex = 0;
            this.tpDeviceManagement.Text = "Device Management";
            this.tpDeviceManagement.UseVisualStyleBackColor = true;
            // 
            // deviceManagementPage
            // 
            this.deviceManagementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceManagementPage.Location = new System.Drawing.Point(3, 3);
            this.deviceManagementPage.Name = "deviceManagementPage";
            this.deviceManagementPage.Size = new System.Drawing.Size(724, 416);
            this.deviceManagementPage.TabIndex = 0;
            // 
            // tpMedia
            // 
            this.tpMedia.Controls.Add(this.mediaPage);
            this.tpMedia.Location = new System.Drawing.Point(4, 22);
            this.tpMedia.Name = "tpMedia";
            this.tpMedia.Padding = new System.Windows.Forms.Padding(3);
            this.tpMedia.Size = new System.Drawing.Size(730, 422);
            this.tpMedia.TabIndex = 1;
            this.tpMedia.Text = "Media";
            this.tpMedia.UseVisualStyleBackColor = true;
            // 
            // mediaPage
            // 
            this.mediaPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPage.Location = new System.Drawing.Point(3, 3);
            this.mediaPage.Name = "mediaPage";
            this.mediaPage.Size = new System.Drawing.Size(724, 416);
            this.mediaPage.TabIndex = 0;
            // 
            // tpPtz
            // 
            this.tpPtz.Controls.Add(this.ptzPage);
            this.tpPtz.Location = new System.Drawing.Point(4, 22);
            this.tpPtz.Name = "tpPtz";
            this.tpPtz.Padding = new System.Windows.Forms.Padding(3);
            this.tpPtz.Size = new System.Drawing.Size(730, 422);
            this.tpPtz.TabIndex = 2;
            this.tpPtz.Text = "PTZ";
            this.tpPtz.UseVisualStyleBackColor = true;
            // 
            // ptzPage
            // 
            this.ptzPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptzPage.Location = new System.Drawing.Point(3, 3);
            this.ptzPage.Name = "ptzPage";
            this.ptzPage.Size = new System.Drawing.Size(724, 416);
            this.ptzPage.TabIndex = 0;
            // 
            // tpRequests
            // 
            this.tpRequests.Controls.Add(this.requestsPage);
            this.tpRequests.Location = new System.Drawing.Point(4, 22);
            this.tpRequests.Name = "tpRequests";
            this.tpRequests.Size = new System.Drawing.Size(730, 422);
            this.tpRequests.TabIndex = 3;
            this.tpRequests.Text = "Requests";
            this.tpRequests.UseVisualStyleBackColor = true;
            // 
            // requestsPage
            // 
            this.requestsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requestsPage.Location = new System.Drawing.Point(0, 0);
            this.requestsPage.Name = "requestsPage";
            this.requestsPage.Service = TestTool.GUI.Enums.DutService.DeviceManagement;
            this.requestsPage.Size = new System.Drawing.Size(730, 422);
            this.requestsPage.TabIndex = 0;
            // 
            // gbCapabilitiesExchange
            // 
            this.gbCapabilitiesExchange.Controls.Add(this.rbGetServices);
            this.gbCapabilitiesExchange.Controls.Add(this.rbGetCapabilities);
            this.gbCapabilitiesExchange.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbCapabilitiesExchange.Location = new System.Drawing.Point(0, 0);
            this.gbCapabilitiesExchange.Name = "gbCapabilitiesExchange";
            this.gbCapabilitiesExchange.Size = new System.Drawing.Size(332, 46);
            this.gbCapabilitiesExchange.TabIndex = 1;
            this.gbCapabilitiesExchange.TabStop = false;
            this.gbCapabilitiesExchange.Text = "Capabilities Exchange";
            // 
            // rbGetServices
            // 
            this.rbGetServices.AutoSize = true;
            this.rbGetServices.Checked = true;
            this.rbGetServices.Location = new System.Drawing.Point(176, 19);
            this.rbGetServices.Name = "rbGetServices";
            this.rbGetServices.Size = new System.Drawing.Size(131, 17);
            this.rbGetServices.TabIndex = 1;
            this.rbGetServices.TabStop = true;
            this.rbGetServices.Text = "GetServices";
            this.rbGetServices.UseVisualStyleBackColor = true;
            this.rbGetServices.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbGetCapabilities
            // 
            this.rbGetCapabilities.AutoSize = true;
            this.rbGetCapabilities.Location = new System.Drawing.Point(7, 19);
            this.rbGetCapabilities.Name = "rbGetCapabilities";
            this.rbGetCapabilities.Size = new System.Drawing.Size(149, 17);
            this.rbGetCapabilities.TabIndex = 0;
            this.rbGetCapabilities.Text = "GetCapabilities";
            this.rbGetCapabilities.UseVisualStyleBackColor = true;
            this.rbGetCapabilities.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // gbAuthentication
            // 
            this.gbAuthentication.Controls.Add(this.rbDigest);
            this.gbAuthentication.Controls.Add(this.rbWsUsername);
            this.gbAuthentication.Controls.Add(this.rbNone);
            this.gbAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAuthentication.Location = new System.Drawing.Point(332, 0);
            this.gbAuthentication.Name = "gbAuthentication";
            this.gbAuthentication.Size = new System.Drawing.Size(406, 46);
            this.gbAuthentication.TabIndex = 2;
            this.gbAuthentication.TabStop = false;
            this.gbAuthentication.Text = "Authentication";
            // 
            // rbDigest
            // 
            this.rbDigest.AutoSize = true;
            this.rbDigest.Checked = true;
            this.rbDigest.Location = new System.Drawing.Point(233, 19);
            this.rbDigest.Name = "rbDigest";
            this.rbDigest.Size = new System.Drawing.Size(55, 17);
            this.rbDigest.TabIndex = 2;
            this.rbDigest.TabStop = true;
            this.rbDigest.Text = "Digest";
            this.rbDigest.UseVisualStyleBackColor = true;
            this.rbDigest.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbWsUsername
            // 
            this.rbWsUsername.AutoSize = true;
            this.rbWsUsername.Location = new System.Drawing.Point(84, 19);
            this.rbWsUsername.Name = "rbWsUsername";
            this.rbWsUsername.Size = new System.Drawing.Size(124, 17);
            this.rbWsUsername.TabIndex = 1;
            this.rbWsUsername.Text = "WS-Username token";
            this.rbWsUsername.UseVisualStyleBackColor = true;
            this.rbWsUsername.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(6, 19);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 0;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.gbAuthentication);
            this.panelTop.Controls.Add(this.panelSpace);
            this.panelTop.Controls.Add(this.gbCapabilitiesExchange);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panelTop.Size = new System.Drawing.Size(738, 51);
            this.panelTop.TabIndex = 3;
            // 
            // panelSpace
            // 
            this.panelSpace.Location = new System.Drawing.Point(132, 19);
            this.panelSpace.Name = "panelSpace";
            this.panelSpace.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSpace.Size = new Size(7, 40);
            this.panelSpace.TabIndex = 2;
            // 
            // DevicePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcDevice);
            this.Controls.Add(this.panelTop);
            this.Name = "DevicePage";
            this.Size = new System.Drawing.Size(738, 499);
            this.tcDevice.ResumeLayout(false);
            this.tpDeviceManagement.ResumeLayout(false);
            this.tpMedia.ResumeLayout(false);
            this.tpPtz.ResumeLayout(false);
            this.tpRequests.ResumeLayout(false);
            this.gbCapabilitiesExchange.ResumeLayout(false);
            this.gbCapabilitiesExchange.PerformLayout();
            this.gbAuthentication.ResumeLayout(false);
            this.gbAuthentication.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDevice;
        private System.Windows.Forms.TabPage tpDeviceManagement;
        private System.Windows.Forms.TabPage tpMedia;
        private System.Windows.Forms.TabPage tpPtz;
        private TestTool.GUI.Controls.Device.DeviceManagementPage deviceManagementPage;
        private TestTool.GUI.Controls.Device.MediaPage mediaPage;
        private TestTool.GUI.Controls.Device.PtzPage ptzPage;
        private System.Windows.Forms.TabPage tpRequests;
        private TestTool.GUI.Controls.Device.RequestsPage requestsPage;
        private System.Windows.Forms.GroupBox gbCapabilitiesExchange;
        private System.Windows.Forms.GroupBox gbAuthentication;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.RadioButton rbGetServices;
        private System.Windows.Forms.RadioButton rbGetCapabilities;
        private System.Windows.Forms.RadioButton rbDigest;
        private System.Windows.Forms.RadioButton rbWsUsername;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.Panel panelSpace;
    }
}
