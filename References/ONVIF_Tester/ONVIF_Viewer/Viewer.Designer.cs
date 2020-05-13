namespace ONVIF_Viewer
{
    partial class Viewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));
            this.gb_VideoBox = new System.Windows.Forms.GroupBox();
            this.lb_DiscoverdDevices = new System.Windows.Forms.ListBox();
            this.btn_discover = new System.Windows.Forms.Button();
            this.gb_CameraInfo = new System.Windows.Forms.GroupBox();
            this.btn_LiveVideo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_TargetAddress = new System.Windows.Forms.ComboBox();
            this.cb_VideoStreams = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_TargetIPAddress = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tb_TargetMDversion = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_TargetScopes = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tb_TargetType = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_TargetEndPointAddress = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.gb_DeviceInfo = new System.Windows.Forms.GroupBox();
            this.tb_DeviceTab_Model = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tb_DeviceTab_SerialNumber = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.tb_DeviceTab_MFG = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tb_DeviceTab_HW = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tb_DeviceTab_FW = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.pnl_SplashPanel = new System.Windows.Forms.Panel();
            this.lbl_loading = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.gb_CameraInfo.SuspendLayout();
            this.gb_DeviceInfo.SuspendLayout();
            this.pnl_SplashPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_VideoBox
            // 
            this.gb_VideoBox.Location = new System.Drawing.Point(458, 12);
            this.gb_VideoBox.Name = "gb_VideoBox";
            this.gb_VideoBox.Size = new System.Drawing.Size(390, 314);
            this.gb_VideoBox.TabIndex = 0;
            this.gb_VideoBox.TabStop = false;
            this.gb_VideoBox.Text = "Video Here";
            // 
            // lb_DiscoverdDevices
            // 
            this.lb_DiscoverdDevices.FormattingEnabled = true;
            this.lb_DiscoverdDevices.Location = new System.Drawing.Point(13, 12);
            this.lb_DiscoverdDevices.Name = "lb_DiscoverdDevices";
            this.lb_DiscoverdDevices.Size = new System.Drawing.Size(439, 277);
            this.lb_DiscoverdDevices.TabIndex = 6;
            this.lb_DiscoverdDevices.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_DiscoverdDevices_MouseUp);
            this.lb_DiscoverdDevices.SelectedIndexChanged += new System.EventHandler(this.lb_DiscoverdDevices_SelectedIndexChanged);
            // 
            // btn_discover
            // 
            this.btn_discover.Location = new System.Drawing.Point(13, 293);
            this.btn_discover.Name = "btn_discover";
            this.btn_discover.Size = new System.Drawing.Size(75, 23);
            this.btn_discover.TabIndex = 7;
            this.btn_discover.Text = "Discover";
            this.btn_discover.UseVisualStyleBackColor = true;
            this.btn_discover.Click += new System.EventHandler(this.btn_discover_Click);
            // 
            // gb_CameraInfo
            // 
            this.gb_CameraInfo.Controls.Add(this.btn_LiveVideo);
            this.gb_CameraInfo.Controls.Add(this.label2);
            this.gb_CameraInfo.Controls.Add(this.cb_TargetAddress);
            this.gb_CameraInfo.Controls.Add(this.cb_VideoStreams);
            this.gb_CameraInfo.Controls.Add(this.label1);
            this.gb_CameraInfo.Location = new System.Drawing.Point(12, 332);
            this.gb_CameraInfo.Name = "gb_CameraInfo";
            this.gb_CameraInfo.Size = new System.Drawing.Size(836, 84);
            this.gb_CameraInfo.TabIndex = 8;
            this.gb_CameraInfo.TabStop = false;
            this.gb_CameraInfo.Text = "Target";
            // 
            // btn_LiveVideo
            // 
            this.btn_LiveVideo.Location = new System.Drawing.Point(755, 46);
            this.btn_LiveVideo.Name = "btn_LiveVideo";
            this.btn_LiveVideo.Size = new System.Drawing.Size(75, 23);
            this.btn_LiveVideo.TabIndex = 27;
            this.btn_LiveVideo.Text = "Connect";
            this.btn_LiveVideo.UseVisualStyleBackColor = true;
            this.btn_LiveVideo.Click += new System.EventHandler(this.btn_LiveVideo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(433, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Video Profile";
            // 
            // cb_TargetAddress
            // 
            this.cb_TargetAddress.FormattingEnabled = true;
            this.cb_TargetAddress.Location = new System.Drawing.Point(137, 19);
            this.cb_TargetAddress.Name = "cb_TargetAddress";
            this.cb_TargetAddress.Size = new System.Drawing.Size(268, 21);
            this.cb_TargetAddress.TabIndex = 25;
            this.cb_TargetAddress.SelectedIndexChanged += new System.EventHandler(this.cb_TargetAddress_SelectedIndexChanged);
            this.cb_TargetAddress.Leave += new System.EventHandler(this.cb_TargetAddress_Leave);
            this.cb_TargetAddress.TextUpdate += new System.EventHandler(this.cb_TargetAddress_TextUpdate);
            // 
            // cb_VideoStreams
            // 
            this.cb_VideoStreams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_VideoStreams.Location = new System.Drawing.Point(562, 19);
            this.cb_VideoStreams.Name = "cb_VideoStreams";
            this.cb_VideoStreams.Size = new System.Drawing.Size(268, 21);
            this.cb_VideoStreams.TabIndex = 13;
            this.cb_VideoStreams.SelectedIndexChanged += new System.EventHandler(this.cb_VideoStreams_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Target Service Address";
            // 
            // tb_TargetIPAddress
            // 
            this.tb_TargetIPAddress.Location = new System.Drawing.Point(136, 145);
            this.tb_TargetIPAddress.Name = "tb_TargetIPAddress";
            this.tb_TargetIPAddress.ReadOnly = true;
            this.tb_TargetIPAddress.Size = new System.Drawing.Size(268, 20);
            this.tb_TargetIPAddress.TabIndex = 24;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 148);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(92, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Target IP Address";
            // 
            // tb_TargetMDversion
            // 
            this.tb_TargetMDversion.Location = new System.Drawing.Point(136, 171);
            this.tb_TargetMDversion.Name = "tb_TargetMDversion";
            this.tb_TargetMDversion.ReadOnly = true;
            this.tb_TargetMDversion.Size = new System.Drawing.Size(268, 20);
            this.tb_TargetMDversion.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 174);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(124, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Target Metadata Version";
            // 
            // tb_TargetScopes
            // 
            this.tb_TargetScopes.Location = new System.Drawing.Point(136, 71);
            this.tb_TargetScopes.Multiline = true;
            this.tb_TargetScopes.Name = "tb_TargetScopes";
            this.tb_TargetScopes.ReadOnly = true;
            this.tb_TargetScopes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_TargetScopes.Size = new System.Drawing.Size(268, 68);
            this.tb_TargetScopes.TabIndex = 19;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 74);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 13);
            this.label19.TabIndex = 18;
            this.label19.Text = "Target Scopes";
            // 
            // tb_TargetType
            // 
            this.tb_TargetType.Location = new System.Drawing.Point(136, 45);
            this.tb_TargetType.Name = "tb_TargetType";
            this.tb_TargetType.ReadOnly = true;
            this.tb_TargetType.Size = new System.Drawing.Size(268, 20);
            this.tb_TargetType.TabIndex = 17;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 48);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "Target Type";
            // 
            // tb_TargetEndPointAddress
            // 
            this.tb_TargetEndPointAddress.Location = new System.Drawing.Point(136, 19);
            this.tb_TargetEndPointAddress.Name = "tb_TargetEndPointAddress";
            this.tb_TargetEndPointAddress.ReadOnly = true;
            this.tb_TargetEndPointAddress.Size = new System.Drawing.Size(268, 20);
            this.tb_TargetEndPointAddress.TabIndex = 15;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 22);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 13);
            this.label18.TabIndex = 14;
            this.label18.Text = "Target EP Address";
            // 
            // gb_DeviceInfo
            // 
            this.gb_DeviceInfo.Controls.Add(this.tb_DeviceTab_Model);
            this.gb_DeviceInfo.Controls.Add(this.label26);
            this.gb_DeviceInfo.Controls.Add(this.tb_DeviceTab_SerialNumber);
            this.gb_DeviceInfo.Controls.Add(this.label25);
            this.gb_DeviceInfo.Controls.Add(this.tb_DeviceTab_MFG);
            this.gb_DeviceInfo.Controls.Add(this.label24);
            this.gb_DeviceInfo.Controls.Add(this.tb_DeviceTab_HW);
            this.gb_DeviceInfo.Controls.Add(this.label23);
            this.gb_DeviceInfo.Controls.Add(this.tb_DeviceTab_FW);
            this.gb_DeviceInfo.Controls.Add(this.label22);
            this.gb_DeviceInfo.Controls.Add(this.tb_TargetEndPointAddress);
            this.gb_DeviceInfo.Controls.Add(this.tb_TargetScopes);
            this.gb_DeviceInfo.Controls.Add(this.label19);
            this.gb_DeviceInfo.Controls.Add(this.tb_TargetType);
            this.gb_DeviceInfo.Controls.Add(this.tb_TargetIPAddress);
            this.gb_DeviceInfo.Controls.Add(this.label16);
            this.gb_DeviceInfo.Controls.Add(this.tb_TargetMDversion);
            this.gb_DeviceInfo.Controls.Add(this.label18);
            this.gb_DeviceInfo.Controls.Add(this.label13);
            this.gb_DeviceInfo.Controls.Add(this.label20);
            this.gb_DeviceInfo.Location = new System.Drawing.Point(13, 422);
            this.gb_DeviceInfo.Name = "gb_DeviceInfo";
            this.gb_DeviceInfo.Size = new System.Drawing.Size(835, 205);
            this.gb_DeviceInfo.TabIndex = 25;
            this.gb_DeviceInfo.TabStop = false;
            this.gb_DeviceInfo.Text = "Target Information";
            // 
            // tb_DeviceTab_Model
            // 
            this.tb_DeviceTab_Model.Location = new System.Drawing.Point(495, 71);
            this.tb_DeviceTab_Model.Name = "tb_DeviceTab_Model";
            this.tb_DeviceTab_Model.ReadOnly = true;
            this.tb_DeviceTab_Model.Size = new System.Drawing.Size(268, 20);
            this.tb_DeviceTab_Model.TabIndex = 40;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(439, 74);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(36, 13);
            this.label26.TabIndex = 39;
            this.label26.Text = "Model";
            // 
            // tb_DeviceTab_SerialNumber
            // 
            this.tb_DeviceTab_SerialNumber.Location = new System.Drawing.Point(495, 123);
            this.tb_DeviceTab_SerialNumber.Name = "tb_DeviceTab_SerialNumber";
            this.tb_DeviceTab_SerialNumber.ReadOnly = true;
            this.tb_DeviceTab_SerialNumber.Size = new System.Drawing.Size(268, 20);
            this.tb_DeviceTab_SerialNumber.TabIndex = 38;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(442, 126);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 13);
            this.label25.TabIndex = 37;
            this.label25.Text = "Serial #";
            // 
            // tb_DeviceTab_MFG
            // 
            this.tb_DeviceTab_MFG.Location = new System.Drawing.Point(495, 97);
            this.tb_DeviceTab_MFG.Name = "tb_DeviceTab_MFG";
            this.tb_DeviceTab_MFG.ReadOnly = true;
            this.tb_DeviceTab_MFG.Size = new System.Drawing.Size(268, 20);
            this.tb_DeviceTab_MFG.TabIndex = 36;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(442, 101);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(25, 13);
            this.label24.TabIndex = 35;
            this.label24.Text = "Mfg";
            // 
            // tb_DeviceTab_HW
            // 
            this.tb_DeviceTab_HW.Location = new System.Drawing.Point(495, 45);
            this.tb_DeviceTab_HW.Name = "tb_DeviceTab_HW";
            this.tb_DeviceTab_HW.ReadOnly = true;
            this.tb_DeviceTab_HW.Size = new System.Drawing.Size(268, 20);
            this.tb_DeviceTab_HW.TabIndex = 34;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(439, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(53, 13);
            this.label23.TabIndex = 33;
            this.label23.Text = "Hardware";
            // 
            // tb_DeviceTab_FW
            // 
            this.tb_DeviceTab_FW.Location = new System.Drawing.Point(495, 19);
            this.tb_DeviceTab_FW.Name = "tb_DeviceTab_FW";
            this.tb_DeviceTab_FW.ReadOnly = true;
            this.tb_DeviceTab_FW.Size = new System.Drawing.Size(268, 20);
            this.tb_DeviceTab_FW.TabIndex = 32;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(439, 22);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(49, 13);
            this.label22.TabIndex = 31;
            this.label22.Text = "Firmware";
            // 
            // pnl_SplashPanel
            // 
            this.pnl_SplashPanel.BackColor = System.Drawing.SystemColors.ControlText;
            this.pnl_SplashPanel.Controls.Add(this.lbl_loading);
            this.pnl_SplashPanel.Controls.Add(this.pictureBox1);
            this.pnl_SplashPanel.Controls.Add(this.pictureBox2);
            this.pnl_SplashPanel.Location = new System.Drawing.Point(12, 643);
            this.pnl_SplashPanel.Name = "pnl_SplashPanel";
            this.pnl_SplashPanel.Size = new System.Drawing.Size(873, 500);
            this.pnl_SplashPanel.TabIndex = 26;
            this.pnl_SplashPanel.Visible = false;
            // 
            // lbl_loading
            // 
            this.lbl_loading.AutoSize = true;
            this.lbl_loading.BackColor = System.Drawing.Color.Transparent;
            this.lbl_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_loading.ForeColor = System.Drawing.Color.White;
            this.lbl_loading.Location = new System.Drawing.Point(747, 365);
            this.lbl_loading.Name = "lbl_loading";
            this.lbl_loading.Size = new System.Drawing.Size(113, 25);
            this.lbl_loading.TabIndex = 2;
            this.lbl_loading.Text = "Loading ...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ONVIF_Viewer.Properties.Resources.logo;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(45, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(279, 86);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ONVIF_Viewer.Properties.Resources.header3;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(48, 131);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(770, 200);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(865, 909);
            this.Controls.Add(this.pnl_SplashPanel);
            this.Controls.Add(this.gb_DeviceInfo);
            this.Controls.Add(this.gb_CameraInfo);
            this.Controls.Add(this.btn_discover);
            this.Controls.Add(this.lb_DiscoverdDevices);
            this.Controls.Add(this.gb_VideoBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Viewer";
            this.Text = "ONVIF Viewer";
            this.Load += new System.EventHandler(this.Viewer_Load);
            this.Shown += new System.EventHandler(this.Viewer_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Viewer_FormClosing);
            this.gb_CameraInfo.ResumeLayout(false);
            this.gb_CameraInfo.PerformLayout();
            this.gb_DeviceInfo.ResumeLayout(false);
            this.gb_DeviceInfo.PerformLayout();
            this.pnl_SplashPanel.ResumeLayout(false);
            this.pnl_SplashPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_VideoBox;
        private System.Windows.Forms.ListBox lb_DiscoverdDevices;
        private System.Windows.Forms.Button btn_discover;
        private System.Windows.Forms.GroupBox gb_CameraInfo;
        private System.Windows.Forms.TextBox tb_TargetEndPointAddress;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_TargetType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_TargetScopes;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_TargetAddress;
        private System.Windows.Forms.TextBox tb_TargetIPAddress;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tb_TargetMDversion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_LiveVideo;
        private System.Windows.Forms.GroupBox gb_DeviceInfo;
        private System.Windows.Forms.TextBox tb_DeviceTab_Model;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tb_DeviceTab_SerialNumber;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tb_DeviceTab_MFG;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tb_DeviceTab_HW;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tb_DeviceTab_FW;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel pnl_SplashPanel;
        private System.Windows.Forms.Label lbl_loading;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox cb_VideoStreams;
    }
}

