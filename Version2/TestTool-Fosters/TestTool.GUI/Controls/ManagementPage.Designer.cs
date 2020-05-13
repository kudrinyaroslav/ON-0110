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
            this.gbSaveSettings = new System.Windows.Forms.GroupBox();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.scSettings = new TestTool.GUI.Controls.SettingsControl();
            this.gbProfiles.SuspendLayout();
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
            this.gbProfiles.Location = new System.Drawing.Point(0, 54);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(362, 446);
            this.gbProfiles.TabIndex = 4;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Load Settings";
            // 
            // btnApplyProfile
            // 
            this.btnApplyProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyProfile.Enabled = false;
            this.btnApplyProfile.Location = new System.Drawing.Point(9, 410);
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
            this.lbProfiles.Size = new System.Drawing.Size(347, 368);
            this.lbProfiles.TabIndex = 4;
            this.lbProfiles.SelectedIndexChanged += new System.EventHandler(this.lbProfiles_SelectedIndexChanged);
            // 
            // tbProfileName
            // 
            this.tbProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProfileName.Location = new System.Drawing.Point(91, 23);
            this.tbProfileName.Name = "tbProfileName";
            this.tbProfileName.Size = new System.Drawing.Size(188, 20);
            this.tbProfileName.TabIndex = 6;
            this.tbProfileName.TextChanged += new System.EventHandler(this.tbProfileName_TextChanged);
            // 
            // lblProfileName
            // 
            this.lblProfileName.AutoSize = true;
            this.lblProfileName.Location = new System.Drawing.Point(6, 26);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(79, 13);
            this.lblProfileName.TabIndex = 5;
            this.lblProfileName.Text = "Settings Name:";
            // 
            // btnSaveCurrentOptions
            // 
            this.btnSaveCurrentOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveCurrentOptions.Location = new System.Drawing.Point(285, 21);
            this.btnSaveCurrentOptions.Name = "btnSaveCurrentOptions";
            this.btnSaveCurrentOptions.Size = new System.Drawing.Size(71, 23);
            this.btnSaveCurrentOptions.TabIndex = 7;
            this.btnSaveCurrentOptions.Text = "Save";
            this.toolTip.SetToolTip(this.btnSaveCurrentOptions, "Save current options");
            this.btnSaveCurrentOptions.UseVisualStyleBackColor = true;
            this.btnSaveCurrentOptions.Click += new System.EventHandler(this.btnSaveCurrentOptions_Click);
            // 
            // gbSaveSettings
            // 
            this.gbSaveSettings.Controls.Add(this.lblProfileName);
            this.gbSaveSettings.Controls.Add(this.btnSaveCurrentOptions);
            this.gbSaveSettings.Controls.Add(this.tbProfileName);
            this.gbSaveSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSaveSettings.Location = new System.Drawing.Point(0, 0);
            this.gbSaveSettings.Name = "gbSaveSettings";
            this.gbSaveSettings.Size = new System.Drawing.Size(362, 54);
            this.gbSaveSettings.TabIndex = 3;
            this.gbSaveSettings.TabStop = false;
            this.gbSaveSettings.Text = "Save Current Settings";
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.gbProfiles);
            this.panelSettings.Controls.Add(this.gbSaveSettings);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(362, 500);
            this.panelSettings.TabIndex = 3;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelSettings);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.panelLeft.Size = new System.Drawing.Size(366, 500);
            this.panelLeft.TabIndex = 1;
            // 
            // scSettings
            // 
            this.scSettings.DnsIpv4 = "";
            this.scSettings.DnsIpv6 = "";
            this.scSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSettings.EventTopic = "";
            this.scSettings.GatewayIpv4 = "";
            this.scSettings.GatewayIpv6 = "";
            this.scSettings.Location = new System.Drawing.Point(366, 0);
            this.scSettings.MessageTimeout = 0;
            this.scSettings.Name = "scSettings";
            this.scSettings.NtpIpv4 = "";
            this.scSettings.NtpIpv6 = "";
            this.scSettings.OperationDelay = 0;
            this.scSettings.Password1 = "OnvifTest123";
            this.scSettings.Password2 = "OnvifTest321";
            this.scSettings.PTZNodeToken = "";
            this.scSettings.RebootTimeout = 0;
            this.scSettings.RecoveryDelay = 0;
            this.scSettings.SecureMethod = "";
            this.scSettings.Size = new System.Drawing.Size(431, 500);
            this.scSettings.SubscriptionTimeout = 0;
            this.scSettings.TabIndex = 4;
            this.scSettings.TimeBetweenTests = 0;
            this.scSettings.TopicNamespaces = "";
            this.scSettings.UseEmbeddedPassword = true;
            // 
            // ManagementPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scSettings);
            this.Controls.Add(this.panelLeft);
            this.Name = "ManagementPage";
            this.Size = new System.Drawing.Size(797, 500);
            this.gbProfiles.ResumeLayout(false);
            this.gbProfiles.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbSaveSettings;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelLeft;
        private SettingsControl scSettings;
    }
}
