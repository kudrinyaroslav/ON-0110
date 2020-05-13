using System.Windows.Forms;

namespace TestTool.GUI.Controls
{
    partial class ManagementPage
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
            this.gbProfiles = new System.Windows.Forms.GroupBox();
            this.btnApplyProfile = new System.Windows.Forms.Button();
            this.lblProfilesList = new System.Windows.Forms.Label();
            this.lbProfiles = new System.Windows.Forms.ListBox();
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.lblProfileName = new System.Windows.Forms.Label();
            this.btnSaveCurrentOptions = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbTestMode = new System.Windows.Forms.GroupBox();
            this.rbDiagnosticMode = new System.Windows.Forms.RadioButton();
            this.rbConformanceMode = new System.Windows.Forms.RadioButton();
            this.gbSaveSettings = new System.Windows.Forms.GroupBox();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.scSettings = new TestTool.GUI.Controls.SettingsControl();
            this.gbProfiles.SuspendLayout();
            this.gbTestMode.SuspendLayout();
            this.gbSaveSettings.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProfiles
            // 
            this.gbProfiles.Controls.Add(this.btnApplyProfile);
            this.gbProfiles.Controls.Add(this.lblProfilesList);
            this.gbProfiles.Controls.Add(this.lbProfiles);
            this.gbProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProfiles.Location = new System.Drawing.Point(0, 57);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(395, 341);
            this.gbProfiles.TabIndex = 4;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Load Settings";
            // 
            // btnApplyProfile
            // 
            this.btnApplyProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyProfile.Enabled = false;
            this.btnApplyProfile.Location = new System.Drawing.Point(9, 305);
            this.btnApplyProfile.Name = "btnApplyProfile";
            this.btnApplyProfile.Size = new System.Drawing.Size(120, 23);
            this.btnApplyProfile.TabIndex = 5;
            this.btnApplyProfile.Text = "Load Settings";
            this.toolTip.SetToolTip(this.btnApplyProfile, "Apply currently selected settings");
            this.btnApplyProfile.UseVisualStyleBackColor = true;
            this.btnApplyProfile.Click += new System.EventHandler(this.btnApplyProfile_Click);
            // 
            // lblProfilesList
            // 
            this.lblProfilesList.AutoSize = true;
            this.lblProfilesList.Location = new System.Drawing.Point(9, 15);
            this.lblProfilesList.Name = "lblProfilesList";
            this.lblProfilesList.Size = new System.Drawing.Size(67, 13);
            this.lblProfilesList.TabIndex = 3;
            this.lblProfilesList.Text = "Settings List:";
            // 
            // lbProfiles
            // 
            this.lbProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProfiles.DisplayMember = "Name";
            this.lbProfiles.FormattingEnabled = true;
            this.lbProfiles.HorizontalScrollbar = true;
            this.lbProfiles.Location = new System.Drawing.Point(9, 32);
            this.lbProfiles.Name = "lbProfiles";
            this.lbProfiles.Size = new System.Drawing.Size(380, 238);
            this.lbProfiles.TabIndex = 4;
            this.lbProfiles.SelectedIndexChanged += new System.EventHandler(this.lbProfiles_SelectedIndexChanged);
            // 
            // tbProfileName
            // 
            this.tbProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProfileName.Enabled = false;
            this.tbProfileName.Location = new System.Drawing.Point(91, 23);
            this.tbProfileName.Name = "tbProfileName";
            this.tbProfileName.Size = new System.Drawing.Size(221, 20);
            this.tbProfileName.TabIndex = 6;
            this.tbProfileName.TextChanged += new System.EventHandler(this.tbProfileName_TextChanged);
            // 
            // lblProfileName
            // 
            this.lblProfileName.AutoSize = true;
            this.lblProfileName.Enabled = false;
            this.lblProfileName.Location = new System.Drawing.Point(6, 26);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(79, 13);
            this.lblProfileName.TabIndex = 5;
            this.lblProfileName.Text = "Settings Name:";
            // 
            // btnSaveCurrentOptions
            // 
            this.btnSaveCurrentOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveCurrentOptions.Enabled = false;
            this.btnSaveCurrentOptions.Location = new System.Drawing.Point(318, 21);
            this.btnSaveCurrentOptions.Name = "btnSaveCurrentOptions";
            this.btnSaveCurrentOptions.Size = new System.Drawing.Size(71, 23);
            this.btnSaveCurrentOptions.TabIndex = 7;
            this.btnSaveCurrentOptions.Text = "Save";
            this.toolTip.SetToolTip(this.btnSaveCurrentOptions, "Save current options");
            this.btnSaveCurrentOptions.UseVisualStyleBackColor = true;
            this.btnSaveCurrentOptions.Click += new System.EventHandler(this.btnSaveCurrentOptions_Click);
            // 
            // gbTestMode
            // 
            this.gbTestMode.Controls.Add(this.rbDiagnosticMode);
            this.gbTestMode.Controls.Add(this.rbConformanceMode);
            this.gbTestMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTestMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbTestMode.Location = new System.Drawing.Point(3, 0);
            this.gbTestMode.Name = "gbTestMode";
            this.gbTestMode.Size = new System.Drawing.Size(401, 99);
            this.gbTestMode.TabIndex = 2;
            this.gbTestMode.TabStop = false;
            this.gbTestMode.Text = "Test Mode";
            // 
            // rbDiagnosticMode
            // 
            this.rbDiagnosticMode.AutoSize = true;
            this.rbDiagnosticMode.Checked = true;
            this.rbDiagnosticMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbDiagnosticMode.Location = new System.Drawing.Point(9, 53);
            this.rbDiagnosticMode.Name = "rbDiagnosticMode";
            this.rbDiagnosticMode.Size = new System.Drawing.Size(253, 17);
            this.rbDiagnosticMode.TabIndex = 2;
            this.rbDiagnosticMode.TabStop = true;
            this.rbDiagnosticMode.Text = "Diagnostic mode (for verification and debugging)";
            this.rbDiagnosticMode.UseVisualStyleBackColor = true;
            // 
            // rbConformanceMode
            // 
            this.rbConformanceMode.AutoSize = true;
            this.rbConformanceMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbConformanceMode.Location = new System.Drawing.Point(9, 30);
            this.rbConformanceMode.Name = "rbConformanceMode";
            this.rbConformanceMode.Size = new System.Drawing.Size(328, 17);
            this.rbConformanceMode.TabIndex = 1;
            this.rbConformanceMode.TabStop = true;
            this.rbConformanceMode.Text = "Conformance mode (for claiming ONVIF conformance)";
            this.rbConformanceMode.UseVisualStyleBackColor = true;
            this.rbConformanceMode.CheckedChanged += new System.EventHandler(this.rbConformanceMode_CheckedChanged);
            // 
            // gbSaveSettings
            // 
            this.gbSaveSettings.Controls.Add(this.lblProfileName);
            this.gbSaveSettings.Controls.Add(this.btnSaveCurrentOptions);
            this.gbSaveSettings.Controls.Add(this.tbProfileName);
            this.gbSaveSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSaveSettings.Location = new System.Drawing.Point(0, 3);
            this.gbSaveSettings.Name = "gbSaveSettings";
            this.gbSaveSettings.Size = new System.Drawing.Size(395, 54);
            this.gbSaveSettings.TabIndex = 3;
            this.gbSaveSettings.TabStop = false;
            this.gbSaveSettings.Text = "Save Current Settings";
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.gbProfiles);
            this.panelSettings.Controls.Add(this.gbSaveSettings);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Location = new System.Drawing.Point(0, 99);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Padding = new System.Windows.Forms.Padding(0);
            this.panelSettings.Size = new System.Drawing.Size(401, 401);
            this.panelSettings.TabIndex = 3;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelSettings);
            this.panelLeft.Controls.Add(this.gbTestMode);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(4, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(397, 500);
            this.panelLeft.TabIndex = 1;
            this.panelLeft.Padding = new Padding(0,0,4,0);
            // 
            // scSettings
            // 
            this.scSettings.DnsIpv4 = "";
            this.scSettings.DnsIpv6 = "";
            this.scSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSettings.GatewayIpv4 = "";
            this.scSettings.GatewayIpv6 = "";
            this.scSettings.Location = new System.Drawing.Point(401, 0);
            this.scSettings.MessageTimeout = 0;
            this.scSettings.Name = "scSettings";
            this.scSettings.NtpIpv4 = "";
            this.scSettings.NtpIpv6 = "";
            this.scSettings.OperationDelay = 0;
            this.scSettings.Password1 = "OnvifTest123";
            this.scSettings.Password2 = "OnvifTest321";
            this.scSettings.PTZNodeToken = "";
            this.scSettings.RebootTimeout = 0;
            this.scSettings.Size = new System.Drawing.Size(558, 500);
            this.scSettings.TabIndex = 4;
            this.scSettings.TimeBetweenTests = 0;
            this.scSettings.UseEmbeddedPassword = true;
            // 
            // ManagementPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scSettings);
            this.Controls.Add(this.panelLeft);
            this.Name = "ManagementPage";
            this.Size = new System.Drawing.Size(959, 500);
            this.gbProfiles.ResumeLayout(false);
            this.gbProfiles.PerformLayout();
            this.gbTestMode.ResumeLayout(false);
            this.gbTestMode.PerformLayout();
            this.gbSaveSettings.ResumeLayout(false);
            this.gbSaveSettings.PerformLayout();
            this.panelSettings.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbProfiles;
        private System.Windows.Forms.Button btnApplyProfile;
        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.Button btnSaveCurrentOptions;
        private System.Windows.Forms.Label lblProfilesList;
        private System.Windows.Forms.ListBox lbProfiles;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbTestMode;
        private System.Windows.Forms.RadioButton rbDiagnosticMode;
        private System.Windows.Forms.RadioButton rbConformanceMode;
        private System.Windows.Forms.GroupBox gbSaveSettings;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelLeft;
        private SettingsControl scSettings;
    }
}
