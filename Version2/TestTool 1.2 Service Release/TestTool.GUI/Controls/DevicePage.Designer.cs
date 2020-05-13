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
            this.tpMedia = new System.Windows.Forms.TabPage();
            this.tpPtz = new System.Windows.Forms.TabPage();
            this.deviceManagementPage = new TestTool.GUI.Controls.Device.DeviceManagementPage();
            this.mediaPage = new TestTool.GUI.Controls.Device.MediaPage();
            this.ptzPage = new TestTool.GUI.Controls.Device.PtzPage();
            this.tcDevice.SuspendLayout();
            this.tpDeviceManagement.SuspendLayout();
            this.tpMedia.SuspendLayout();
            this.tpPtz.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDevice
            // 
            this.tcDevice.Controls.Add(this.tpDeviceManagement);
            this.tcDevice.Controls.Add(this.tpMedia);
            this.tcDevice.Controls.Add(this.tpPtz);
            this.tcDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDevice.Location = new System.Drawing.Point(0, 0);
            this.tcDevice.Name = "tcDevice";
            this.tcDevice.SelectedIndex = 0;
            this.tcDevice.Size = new System.Drawing.Size(738, 499);
            this.tcDevice.TabIndex = 0;
            // 
            // tpDeviceManagement
            // 
            this.tpDeviceManagement.Controls.Add(this.deviceManagementPage);
            this.tpDeviceManagement.Location = new System.Drawing.Point(4, 22);
            this.tpDeviceManagement.Name = "tpDeviceManagement";
            this.tpDeviceManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpDeviceManagement.Size = new System.Drawing.Size(730, 473);
            this.tpDeviceManagement.TabIndex = 0;
            this.tpDeviceManagement.Text = "Device Management";
            this.tpDeviceManagement.UseVisualStyleBackColor = true;
            // 
            // tpMedia
            // 
            this.tpMedia.Controls.Add(this.mediaPage);
            this.tpMedia.Location = new System.Drawing.Point(4, 22);
            this.tpMedia.Name = "tpMedia";
            this.tpMedia.Padding = new System.Windows.Forms.Padding(3);
            this.tpMedia.Size = new System.Drawing.Size(730, 473);
            this.tpMedia.TabIndex = 1;
            this.tpMedia.Text = "Media";
            this.tpMedia.UseVisualStyleBackColor = true;
            // 
            // tpPtz
            // 
            this.tpPtz.Controls.Add(this.ptzPage);
            this.tpPtz.Location = new System.Drawing.Point(4, 22);
            this.tpPtz.Name = "tpPtz";
            this.tpPtz.Padding = new System.Windows.Forms.Padding(3);
            this.tpPtz.Size = new System.Drawing.Size(730, 473);
            this.tpPtz.TabIndex = 2;
            this.tpPtz.Text = "PTZ";
            this.tpPtz.UseVisualStyleBackColor = true;
            // 
            // deviceManagementPage
            // 
            this.deviceManagementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceManagementPage.Location = new System.Drawing.Point(3, 3);
            this.deviceManagementPage.Name = "deviceManagementPage";
            this.deviceManagementPage.Size = new System.Drawing.Size(724, 467);
            this.deviceManagementPage.TabIndex = 0;
            // 
            // mediaPage
            // 
            this.mediaPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPage.Location = new System.Drawing.Point(3, 3);
            this.mediaPage.Name = "mediaPage";
            this.mediaPage.Size = new System.Drawing.Size(724, 467);
            this.mediaPage.TabIndex = 0;
            // 
            // ptzPage
            // 
            this.ptzPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptzPage.Location = new System.Drawing.Point(3, 3);
            this.ptzPage.Name = "ptzPage";
            this.ptzPage.Size = new System.Drawing.Size(724, 467);
            this.ptzPage.TabIndex = 0;
            // 
            // DevicePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcDevice);
            this.Name = "DevicePage";
            this.Size = new System.Drawing.Size(738, 499);
            this.tcDevice.ResumeLayout(false);
            this.tpDeviceManagement.ResumeLayout(false);
            this.tpMedia.ResumeLayout(false);
            this.tpPtz.ResumeLayout(false);
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
    }
}
