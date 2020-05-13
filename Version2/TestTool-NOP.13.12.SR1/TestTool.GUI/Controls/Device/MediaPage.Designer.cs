namespace TestTool.GUI.Controls.Device
{
    partial class MediaPage
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
            this.gbAudio = new System.Windows.Forms.GroupBox();
            this.cmbAudioBitrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonGetAudioCodecs = new System.Windows.Forms.Button();
            this.buttonGetAudioEncoders = new System.Windows.Forms.Button();
            this.buttonGetAudioSources = new System.Windows.Forms.Button();
            this.cmbAudioCodec = new System.Windows.Forms.ComboBox();
            this.lblAudioCodec = new System.Windows.Forms.Label();
            this.cmbAudioEncoder = new System.Windows.Forms.ComboBox();
            this.lblAudioEncoder = new System.Windows.Forms.Label();
            this.cmbAudioSource = new System.Windows.Forms.ComboBox();
            this.lblAudioSource = new System.Windows.Forms.Label();
            this.gbVideo = new System.Windows.Forms.GroupBox();
            this.txtVideoFramerate = new System.Windows.Forms.TextBox();
            this.txtVideoBitrate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonGetVideoCodecs = new System.Windows.Forms.Button();
            this.buttonGetVideoEncoders = new System.Windows.Forms.Button();
            this.buttonGetVideoSources = new System.Windows.Forms.Button();
            this.cmbVideoResolution = new System.Windows.Forms.ComboBox();
            this.lblVideoResolution = new System.Windows.Forms.Label();
            this.cmbVideoCodec = new System.Windows.Forms.ComboBox();
            this.lblVideoCodec = new System.Windows.Forms.Label();
            this.cmbVideoEncoder = new System.Windows.Forms.ComboBox();
            this.lblVideoEncoder = new System.Windows.Forms.Label();
            this.cmbVideoSource = new System.Windows.Forms.ComboBox();
            this.lblVideoSource = new System.Windows.Forms.Label();
            this.btnGetStreams = new System.Windows.Forms.Button();
            this.btnGetMediaUrl = new System.Windows.Forms.Button();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.tbMediaUrl = new System.Windows.Forms.TextBox();
            this.lblMediaUrl = new System.Windows.Forms.Label();
            this.groupProfile = new System.Windows.Forms.GroupBox();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnGetProfiles = new System.Windows.Forms.Button();
            this.cmbMediaProfile = new System.Windows.Forms.ComboBox();
            this.cmbTransport = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbAudio.SuspendLayout();
            this.gbVideo.SuspendLayout();
            this.groupProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAudio
            // 
            this.gbAudio.Controls.Add(this.cmbAudioBitrate);
            this.gbAudio.Controls.Add(this.label5);
            this.gbAudio.Controls.Add(this.buttonGetAudioCodecs);
            this.gbAudio.Controls.Add(this.buttonGetAudioEncoders);
            this.gbAudio.Controls.Add(this.buttonGetAudioSources);
            this.gbAudio.Controls.Add(this.cmbAudioCodec);
            this.gbAudio.Controls.Add(this.lblAudioCodec);
            this.gbAudio.Controls.Add(this.cmbAudioEncoder);
            this.gbAudio.Controls.Add(this.lblAudioEncoder);
            this.gbAudio.Controls.Add(this.cmbAudioSource);
            this.gbAudio.Controls.Add(this.lblAudioSource);
            this.gbAudio.Location = new System.Drawing.Point(6, 240);
            this.gbAudio.Name = "gbAudio";
            this.gbAudio.Size = new System.Drawing.Size(361, 131);
            this.gbAudio.TabIndex = 17;
            this.gbAudio.TabStop = false;
            this.gbAudio.Text = "Audio";
            // 
            // cmbAudioBitrate
            // 
            this.cmbAudioBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioBitrate.FormattingEnabled = true;
            this.cmbAudioBitrate.Location = new System.Drawing.Point(120, 100);
            this.cmbAudioBitrate.Name = "cmbAudioBitrate";
            this.cmbAudioBitrate.Size = new System.Drawing.Size(93, 21);
            this.cmbAudioBitrate.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "Bitrate:";
            // 
            // buttonGetAudioCodecs
            // 
            this.buttonGetAudioCodecs.Location = new System.Drawing.Point(277, 71);
            this.buttonGetAudioCodecs.Name = "buttonGetAudioCodecs";
            this.buttonGetAudioCodecs.Size = new System.Drawing.Size(57, 23);
            this.buttonGetAudioCodecs.TabIndex = 23;
            this.buttonGetAudioCodecs.Text = "Get";
            this.buttonGetAudioCodecs.UseVisualStyleBackColor = true;
            this.buttonGetAudioCodecs.Click += new System.EventHandler(this.buttonGetAudioCodecs_Click);
            // 
            // buttonGetAudioEncoders
            // 
            this.buttonGetAudioEncoders.Location = new System.Drawing.Point(277, 44);
            this.buttonGetAudioEncoders.Name = "buttonGetAudioEncoders";
            this.buttonGetAudioEncoders.Size = new System.Drawing.Size(57, 23);
            this.buttonGetAudioEncoders.TabIndex = 21;
            this.buttonGetAudioEncoders.Text = "Get";
            this.buttonGetAudioEncoders.UseVisualStyleBackColor = true;
            this.buttonGetAudioEncoders.Click += new System.EventHandler(this.buttonGetAudioEncoders_Click);
            // 
            // buttonGetAudioSources
            // 
            this.buttonGetAudioSources.Location = new System.Drawing.Point(277, 17);
            this.buttonGetAudioSources.Name = "buttonGetAudioSources";
            this.buttonGetAudioSources.Size = new System.Drawing.Size(57, 23);
            this.buttonGetAudioSources.TabIndex = 19;
            this.buttonGetAudioSources.Text = "Get";
            this.buttonGetAudioSources.UseVisualStyleBackColor = true;
            this.buttonGetAudioSources.Click += new System.EventHandler(this.buttonGetAudioSources_Click);
            // 
            // cmbAudioCodec
            // 
            this.cmbAudioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioCodec.FormattingEnabled = true;
            this.cmbAudioCodec.Location = new System.Drawing.Point(120, 73);
            this.cmbAudioCodec.Name = "cmbAudioCodec";
            this.cmbAudioCodec.Size = new System.Drawing.Size(142, 21);
            this.cmbAudioCodec.TabIndex = 22;
            this.cmbAudioCodec.SelectedIndexChanged += new System.EventHandler(this.cmbAudioCodec_SelectedIndexChanged);
            // 
            // lblAudioCodec
            // 
            this.lblAudioCodec.AutoSize = true;
            this.lblAudioCodec.Location = new System.Drawing.Point(6, 76);
            this.lblAudioCodec.Name = "lblAudioCodec";
            this.lblAudioCodec.Size = new System.Drawing.Size(41, 13);
            this.lblAudioCodec.TabIndex = 52;
            this.lblAudioCodec.Text = "Codec:";
            // 
            // cmbAudioEncoder
            // 
            this.cmbAudioEncoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioEncoder.FormattingEnabled = true;
            this.cmbAudioEncoder.Location = new System.Drawing.Point(120, 46);
            this.cmbAudioEncoder.Name = "cmbAudioEncoder";
            this.cmbAudioEncoder.Size = new System.Drawing.Size(142, 21);
            this.cmbAudioEncoder.TabIndex = 20;
            this.cmbAudioEncoder.SelectedIndexChanged += new System.EventHandler(this.cmbAudioEncoder_SelectedIndexChanged);
            // 
            // lblAudioEncoder
            // 
            this.lblAudioEncoder.AutoSize = true;
            this.lblAudioEncoder.Location = new System.Drawing.Point(6, 49);
            this.lblAudioEncoder.Name = "lblAudioEncoder";
            this.lblAudioEncoder.Size = new System.Drawing.Size(114, 13);
            this.lblAudioEncoder.TabIndex = 50;
            this.lblAudioEncoder.Text = "Encoder configuration:";
            // 
            // cmbAudioSource
            // 
            this.cmbAudioSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAudioSource.FormattingEnabled = true;
            this.cmbAudioSource.Location = new System.Drawing.Point(120, 19);
            this.cmbAudioSource.Name = "cmbAudioSource";
            this.cmbAudioSource.Size = new System.Drawing.Size(142, 21);
            this.cmbAudioSource.TabIndex = 18;
            // 
            // lblAudioSource
            // 
            this.lblAudioSource.AutoSize = true;
            this.lblAudioSource.Location = new System.Drawing.Point(6, 22);
            this.lblAudioSource.Name = "lblAudioSource";
            this.lblAudioSource.Size = new System.Drawing.Size(108, 13);
            this.lblAudioSource.TabIndex = 48;
            this.lblAudioSource.Text = "Source configuration:";
            // 
            // gbVideo
            // 
            this.gbVideo.Controls.Add(this.txtVideoFramerate);
            this.gbVideo.Controls.Add(this.txtVideoBitrate);
            this.gbVideo.Controls.Add(this.label3);
            this.gbVideo.Controls.Add(this.label2);
            this.gbVideo.Controls.Add(this.buttonGetVideoCodecs);
            this.gbVideo.Controls.Add(this.buttonGetVideoEncoders);
            this.gbVideo.Controls.Add(this.buttonGetVideoSources);
            this.gbVideo.Controls.Add(this.cmbVideoResolution);
            this.gbVideo.Controls.Add(this.lblVideoResolution);
            this.gbVideo.Controls.Add(this.cmbVideoCodec);
            this.gbVideo.Controls.Add(this.lblVideoCodec);
            this.gbVideo.Controls.Add(this.cmbVideoEncoder);
            this.gbVideo.Controls.Add(this.lblVideoEncoder);
            this.gbVideo.Controls.Add(this.cmbVideoSource);
            this.gbVideo.Controls.Add(this.lblVideoSource);
            this.gbVideo.Location = new System.Drawing.Point(6, 81);
            this.gbVideo.Name = "gbVideo";
            this.gbVideo.Size = new System.Drawing.Size(361, 153);
            this.gbVideo.TabIndex = 7;
            this.gbVideo.TabStop = false;
            this.gbVideo.Text = "Video";
            // 
            // txtVideoFramerate
            // 
            this.txtVideoFramerate.Location = new System.Drawing.Point(223, 120);
            this.txtVideoFramerate.Name = "txtVideoFramerate";
            this.txtVideoFramerate.Size = new System.Drawing.Size(93, 20);
            this.txtVideoFramerate.TabIndex = 16;
            this.txtVideoFramerate.Validating += new System.ComponentModel.CancelEventHandler(this.txtVideoFramerate_Validating);
            // 
            // txtVideoBitrate
            // 
            this.txtVideoBitrate.Location = new System.Drawing.Point(114, 120);
            this.txtVideoBitrate.Name = "txtVideoBitrate";
            this.txtVideoBitrate.Size = new System.Drawing.Size(93, 20);
            this.txtVideoBitrate.TabIndex = 15;
            this.txtVideoBitrate.Validating += new System.ComponentModel.CancelEventHandler(this.txtVideoBitrate_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Framerate limit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Bitrate limit:";
            // 
            // buttonGetVideoCodecs
            // 
            this.buttonGetVideoCodecs.Location = new System.Drawing.Point(277, 73);
            this.buttonGetVideoCodecs.Name = "buttonGetVideoCodecs";
            this.buttonGetVideoCodecs.Size = new System.Drawing.Size(57, 23);
            this.buttonGetVideoCodecs.TabIndex = 13;
            this.buttonGetVideoCodecs.Text = "Get";
            this.buttonGetVideoCodecs.UseVisualStyleBackColor = true;
            this.buttonGetVideoCodecs.Click += new System.EventHandler(this.buttonGetVideoCodecs_Click);
            // 
            // buttonGetVideoEncoders
            // 
            this.buttonGetVideoEncoders.Location = new System.Drawing.Point(277, 45);
            this.buttonGetVideoEncoders.Name = "buttonGetVideoEncoders";
            this.buttonGetVideoEncoders.Size = new System.Drawing.Size(57, 23);
            this.buttonGetVideoEncoders.TabIndex = 11;
            this.buttonGetVideoEncoders.Text = "Get";
            this.buttonGetVideoEncoders.UseVisualStyleBackColor = true;
            this.buttonGetVideoEncoders.Click += new System.EventHandler(this.buttonGetVideoEncoders_Click);
            // 
            // buttonGetVideoSources
            // 
            this.buttonGetVideoSources.Location = new System.Drawing.Point(277, 19);
            this.buttonGetVideoSources.Name = "buttonGetVideoSources";
            this.buttonGetVideoSources.Size = new System.Drawing.Size(57, 23);
            this.buttonGetVideoSources.TabIndex = 9;
            this.buttonGetVideoSources.Text = "Get";
            this.buttonGetVideoSources.UseVisualStyleBackColor = true;
            this.buttonGetVideoSources.Click += new System.EventHandler(this.buttonGetVideoSources_Click);
            // 
            // cmbVideoResolution
            // 
            this.cmbVideoResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoResolution.FormattingEnabled = true;
            this.cmbVideoResolution.Location = new System.Drawing.Point(6, 120);
            this.cmbVideoResolution.Name = "cmbVideoResolution";
            this.cmbVideoResolution.Size = new System.Drawing.Size(93, 21);
            this.cmbVideoResolution.TabIndex = 14;
            // 
            // lblVideoResolution
            // 
            this.lblVideoResolution.AutoSize = true;
            this.lblVideoResolution.Location = new System.Drawing.Point(6, 105);
            this.lblVideoResolution.Name = "lblVideoResolution";
            this.lblVideoResolution.Size = new System.Drawing.Size(60, 13);
            this.lblVideoResolution.TabIndex = 54;
            this.lblVideoResolution.Text = "Resolution:";
            // 
            // cmbVideoCodec
            // 
            this.cmbVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoCodec.FormattingEnabled = true;
            this.cmbVideoCodec.Location = new System.Drawing.Point(120, 76);
            this.cmbVideoCodec.Name = "cmbVideoCodec";
            this.cmbVideoCodec.Size = new System.Drawing.Size(142, 21);
            this.cmbVideoCodec.TabIndex = 12;
            this.cmbVideoCodec.SelectedIndexChanged += new System.EventHandler(this.cmbVideoCodec_SelectedIndexChanged);
            // 
            // lblVideoCodec
            // 
            this.lblVideoCodec.AutoSize = true;
            this.lblVideoCodec.Location = new System.Drawing.Point(6, 76);
            this.lblVideoCodec.Name = "lblVideoCodec";
            this.lblVideoCodec.Size = new System.Drawing.Size(41, 13);
            this.lblVideoCodec.TabIndex = 52;
            this.lblVideoCodec.Text = "Codec:";
            // 
            // cmbVideoEncoder
            // 
            this.cmbVideoEncoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoEncoder.FormattingEnabled = true;
            this.cmbVideoEncoder.Location = new System.Drawing.Point(120, 49);
            this.cmbVideoEncoder.Name = "cmbVideoEncoder";
            this.cmbVideoEncoder.Size = new System.Drawing.Size(142, 21);
            this.cmbVideoEncoder.TabIndex = 10;
            this.cmbVideoEncoder.SelectedIndexChanged += new System.EventHandler(this.cmbVideoEncoder_SelectedIndexChanged);
            // 
            // lblVideoEncoder
            // 
            this.lblVideoEncoder.AutoSize = true;
            this.lblVideoEncoder.Location = new System.Drawing.Point(6, 50);
            this.lblVideoEncoder.Name = "lblVideoEncoder";
            this.lblVideoEncoder.Size = new System.Drawing.Size(114, 13);
            this.lblVideoEncoder.TabIndex = 50;
            this.lblVideoEncoder.Text = "Encoder configuration:";
            // 
            // cmbVideoSource
            // 
            this.cmbVideoSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoSource.FormattingEnabled = true;
            this.cmbVideoSource.Location = new System.Drawing.Point(120, 21);
            this.cmbVideoSource.Name = "cmbVideoSource";
            this.cmbVideoSource.Size = new System.Drawing.Size(142, 21);
            this.cmbVideoSource.TabIndex = 6;
            // 
            // lblVideoSource
            // 
            this.lblVideoSource.AutoSize = true;
            this.lblVideoSource.Location = new System.Drawing.Point(6, 22);
            this.lblVideoSource.Name = "lblVideoSource";
            this.lblVideoSource.Size = new System.Drawing.Size(108, 13);
            this.lblVideoSource.TabIndex = 48;
            this.lblVideoSource.Text = "Source configuration:";
            // 
            // btnGetStreams
            // 
            this.btnGetStreams.Location = new System.Drawing.Point(263, 376);
            this.btnGetStreams.Name = "btnGetStreams";
            this.btnGetStreams.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnGetStreams.Size = new System.Drawing.Size(77, 23);
            this.btnGetStreams.TabIndex = 26;
            this.btnGetStreams.Text = "Play Video";
            this.btnGetStreams.UseVisualStyleBackColor = true;
            this.btnGetStreams.Click += new System.EventHandler(this.btnGetStreams_Click);
            // 
            // btnGetMediaUrl
            // 
            this.btnGetMediaUrl.Location = new System.Drawing.Point(313, 4);
            this.btnGetMediaUrl.Name = "btnGetMediaUrl";
            this.btnGetMediaUrl.Size = new System.Drawing.Size(52, 23);
            this.btnGetMediaUrl.TabIndex = 2;
            this.btnGetMediaUrl.Text = "Get ";
            this.btnGetMediaUrl.UseVisualStyleBackColor = true;
            this.btnGetMediaUrl.Click += new System.EventHandler(this.btnGetMediaUrl_Click);
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
            this.tbReport.Size = new System.Drawing.Size(396, 486);
            this.tbReport.TabIndex = 27;
            // 
            // tbMediaUrl
            // 
            this.tbMediaUrl.Location = new System.Drawing.Point(69, 4);
            this.tbMediaUrl.Name = "tbMediaUrl";
            this.tbMediaUrl.ReadOnly = true;
            this.tbMediaUrl.Size = new System.Drawing.Size(238, 20);
            this.tbMediaUrl.TabIndex = 1;
            // 
            // lblMediaUrl
            // 
            this.lblMediaUrl.AutoSize = true;
            this.lblMediaUrl.Location = new System.Drawing.Point(4, 7);
            this.lblMediaUrl.Name = "lblMediaUrl";
            this.lblMediaUrl.Size = new System.Drawing.Size(64, 13);
            this.lblMediaUrl.TabIndex = 52;
            this.lblMediaUrl.Text = "Media URL:";
            // 
            // groupProfile
            // 
            this.groupProfile.Controls.Add(this.btnDeleteProfile);
            this.groupProfile.Controls.Add(this.btnGetProfiles);
            this.groupProfile.Controls.Add(this.cmbMediaProfile);
            this.groupProfile.Location = new System.Drawing.Point(6, 33);
            this.groupProfile.Name = "groupProfile";
            this.groupProfile.Size = new System.Drawing.Size(361, 44);
            this.groupProfile.TabIndex = 3;
            this.groupProfile.TabStop = false;
            this.groupProfile.Text = "Media Profile";
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(189, 15);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(57, 23);
            this.btnDeleteProfile.TabIndex = 6;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Visible = false;
            // 
            // btnGetProfiles
            // 
            this.btnGetProfiles.Location = new System.Drawing.Point(222, 14);
            this.btnGetProfiles.Name = "btnGetProfiles";
            this.btnGetProfiles.Size = new System.Drawing.Size(57, 23);
            this.btnGetProfiles.TabIndex = 5;
            this.btnGetProfiles.Text = "Get";
            this.btnGetProfiles.UseVisualStyleBackColor = true;
            this.btnGetProfiles.Click += new System.EventHandler(this.btnGetProfiles_Click);
            // 
            // cmbMediaProfile
            // 
            this.cmbMediaProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMediaProfile.FormattingEnabled = true;
            this.cmbMediaProfile.Location = new System.Drawing.Point(9, 15);
            this.cmbMediaProfile.Name = "cmbMediaProfile";
            this.cmbMediaProfile.Size = new System.Drawing.Size(200, 21);
            this.cmbMediaProfile.TabIndex = 4;
            this.cmbMediaProfile.SelectedIndexChanged += new System.EventHandler(this.cmbMediaProfile_SelectedIndexChanged);
            // 
            // cmbTransport
            // 
            this.cmbTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransport.FormattingEnabled = true;
            this.cmbTransport.Location = new System.Drawing.Point(64, 378);
            this.cmbTransport.Name = "cmbTransport";
            this.cmbTransport.Size = new System.Drawing.Size(184, 21);
            this.cmbTransport.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Transport:";
            // 
            // MediaPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbTransport);
            this.Controls.Add(this.groupProfile);
            this.Controls.Add(this.gbAudio);
            this.Controls.Add(this.btnGetStreams);
            this.Controls.Add(this.gbVideo);
            this.Controls.Add(this.btnGetMediaUrl);
            this.Controls.Add(this.tbReport);
            this.Controls.Add(this.tbMediaUrl);
            this.Controls.Add(this.lblMediaUrl);
            this.Name = "MediaPage";
            this.Size = new System.Drawing.Size(775, 496);
            this.gbAudio.ResumeLayout(false);
            this.gbAudio.PerformLayout();
            this.gbVideo.ResumeLayout(false);
            this.gbVideo.PerformLayout();
            this.groupProfile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAudio;
        private System.Windows.Forms.ComboBox cmbAudioCodec;
        private System.Windows.Forms.Label lblAudioCodec;
        private System.Windows.Forms.ComboBox cmbAudioEncoder;
        private System.Windows.Forms.Label lblAudioEncoder;
        private System.Windows.Forms.ComboBox cmbAudioSource;
        private System.Windows.Forms.Label lblAudioSource;
        private System.Windows.Forms.GroupBox gbVideo;
        private System.Windows.Forms.ComboBox cmbVideoResolution;
        private System.Windows.Forms.Label lblVideoResolution;
        private System.Windows.Forms.ComboBox cmbVideoCodec;
        private System.Windows.Forms.Label lblVideoCodec;
        private System.Windows.Forms.ComboBox cmbVideoEncoder;
        private System.Windows.Forms.Label lblVideoEncoder;
        private System.Windows.Forms.ComboBox cmbVideoSource;
        private System.Windows.Forms.Label lblVideoSource;
        private System.Windows.Forms.Button btnGetStreams;
        private System.Windows.Forms.Button btnGetMediaUrl;
        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.TextBox tbMediaUrl;
        private System.Windows.Forms.Label lblMediaUrl;
        private System.Windows.Forms.Button buttonGetVideoCodecs;
        private System.Windows.Forms.Button buttonGetVideoEncoders;
        private System.Windows.Forms.Button buttonGetVideoSources;
        private System.Windows.Forms.Button buttonGetAudioCodecs;
        private System.Windows.Forms.Button buttonGetAudioEncoders;
        private System.Windows.Forms.Button buttonGetAudioSources;
        private System.Windows.Forms.GroupBox groupProfile;
        private System.Windows.Forms.Button btnGetProfiles;
        private System.Windows.Forms.ComboBox cmbMediaProfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTransport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbAudioBitrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVideoBitrate;
        private System.Windows.Forms.TextBox txtVideoFramerate;
        private System.Windows.Forms.Button btnDeleteProfile;
    }
}
