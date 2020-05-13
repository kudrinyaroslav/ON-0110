namespace ONVIF_Tester
{
    partial class Video
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Video));
            this.lbl_videoLabel = new System.Windows.Forms.Label();
            this.gb_VidSimulator = new System.Windows.Forms.GroupBox();
            this.btn_GetVidStream = new System.Windows.Forms.Button();
            this.lb_DeviceProfiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbl_videoLabel
            // 
            this.lbl_videoLabel.AutoSize = true;
            this.lbl_videoLabel.Location = new System.Drawing.Point(12, 327);
            this.lbl_videoLabel.Name = "lbl_videoLabel";
            this.lbl_videoLabel.Size = new System.Drawing.Size(60, 13);
            this.lbl_videoLabel.TabIndex = 0;
            this.lbl_videoLabel.Text = "Video Here";
            // 
            // gb_VidSimulator
            // 
            this.gb_VidSimulator.Location = new System.Drawing.Point(10, 10);
            this.gb_VidSimulator.Name = "gb_VidSimulator";
            this.gb_VidSimulator.Size = new System.Drawing.Size(390, 314);
            this.gb_VidSimulator.TabIndex = 1;
            this.gb_VidSimulator.TabStop = false;
            this.gb_VidSimulator.Text = "video here";
            // 
            // btn_GetVidStream
            // 
            this.btn_GetVidStream.Location = new System.Drawing.Point(638, 300);
            this.btn_GetVidStream.Name = "btn_GetVidStream";
            this.btn_GetVidStream.Size = new System.Drawing.Size(145, 23);
            this.btn_GetVidStream.TabIndex = 2;
            this.btn_GetVidStream.Text = "Get Stream URIs";
            this.btn_GetVidStream.UseVisualStyleBackColor = true;
            // 
            // lb_DeviceProfiles
            // 
            this.lb_DeviceProfiles.FormattingEnabled = true;
            this.lb_DeviceProfiles.Location = new System.Drawing.Point(407, 199);
            this.lb_DeviceProfiles.Name = "lb_DeviceProfiles";
            this.lb_DeviceProfiles.Size = new System.Drawing.Size(376, 95);
            this.lb_DeviceProfiles.TabIndex = 3;
            // 
            // Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 356);
            this.Controls.Add(this.lb_DeviceProfiles);
            this.Controls.Add(this.btn_GetVidStream);
            this.Controls.Add(this.gb_VidSimulator);
            this.Controls.Add(this.lbl_videoLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Video";
            this.Text = "ONVIF Conformance Test Video Window";
            this.Load += new System.EventHandler(this.Video_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Video_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_videoLabel;
        private System.Windows.Forms.GroupBox gb_VidSimulator;
        private System.Windows.Forms.Button btn_GetVidStream;
        private System.Windows.Forms.ListBox lb_DeviceProfiles;
        
    }
}