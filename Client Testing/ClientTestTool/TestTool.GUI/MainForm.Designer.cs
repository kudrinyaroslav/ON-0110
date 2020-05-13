namespace TestTool.GUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpManagement = new System.Windows.Forms.TabPage();
            this.managementPage = new TestTool.GUI.Controls.Pages.ManagementPage();
            this.tpConformance = new System.Windows.Forms.TabPage();
            this.testInfoPage = new TestTool.GUI.Controls.Pages.TestInfoPage();
            this.tpDiagnostic = new System.Windows.Forms.TabPage();
            this.diagnosticPage = new TestTool.GUI.Controls.Pages.TestPage();
            this.tpDiscovery = new System.Windows.Forms.TabPage();
            this.status = new System.Windows.Forms.StatusStrip();
            this.tssLabelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tpCustomConfiguration = new System.Windows.Forms.TabPage();
            this.customConfigurationPage1 = new TestTool.GUI.Controls.Pages.CustomConfigurationPage();
            this.tcMain.SuspendLayout();
            this.tpManagement.SuspendLayout();
            this.tpConformance.SuspendLayout();
            this.tpDiagnostic.SuspendLayout();
            this.status.SuspendLayout();
            this.tpCustomConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpManagement);
            this.tcMain.Controls.Add(this.tpConformance);
            this.tcMain.Controls.Add(this.tpDiagnostic);
            this.tcMain.Controls.Add(this.tpCustomConfiguration);
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(895, 542);
            this.tcMain.TabIndex = 0;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpManagement
            // 
            this.tpManagement.Controls.Add(this.managementPage);
            this.tpManagement.Location = new System.Drawing.Point(4, 22);
            this.tpManagement.Name = "tpManagement";
            this.tpManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpManagement.Size = new System.Drawing.Size(887, 516);
            this.tpManagement.TabIndex = 1;
            this.tpManagement.Text = "Management";
            this.tpManagement.UseVisualStyleBackColor = true;
            // 
            // managementPage
            // 
            this.managementPage.AuthenticationMode = TestTool.Device.AuthenticationMode.WS;
            this.managementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementPage.Location = new System.Drawing.Point(3, 3);
            this.managementPage.Name = "managementPage";
            this.managementPage.Password = "12345";
            this.managementPage.Size = new System.Drawing.Size(881, 510);
            this.managementPage.TabIndex = 0;
            this.managementPage.Username = "admin";
            // 
            // tpConformance
            // 
            this.tpConformance.Controls.Add(this.testInfoPage);
            this.tpConformance.Location = new System.Drawing.Point(4, 22);
            this.tpConformance.Name = "tpConformance";
            this.tpConformance.Padding = new System.Windows.Forms.Padding(3);
            this.tpConformance.Size = new System.Drawing.Size(887, 516);
            this.tpConformance.TabIndex = 2;
            this.tpConformance.Text = "Conformance Test";
            this.tpConformance.UseVisualStyleBackColor = true;
            // 
            // testInfoPage
            // 
            this.testInfoPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testInfoPage.Location = new System.Drawing.Point(3, 3);
            this.testInfoPage.Name = "testInfoPage";
            this.testInfoPage.Size = new System.Drawing.Size(881, 510);
            this.testInfoPage.TabIndex = 0;
            // 
            // tpDiagnostic
            // 
            this.tpDiagnostic.Controls.Add(this.diagnosticPage);
            this.tpDiagnostic.Location = new System.Drawing.Point(4, 22);
            this.tpDiagnostic.Name = "tpDiagnostic";
            this.tpDiagnostic.Padding = new System.Windows.Forms.Padding(3);
            this.tpDiagnostic.Size = new System.Drawing.Size(887, 516);
            this.tpDiagnostic.TabIndex = 3;
            this.tpDiagnostic.Text = "Diagnostic";
            this.tpDiagnostic.UseVisualStyleBackColor = true;
            // 
            // diagnosticPage
            // 
            this.diagnosticPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticPage.Location = new System.Drawing.Point(3, 3);
            this.diagnosticPage.Name = "diagnosticPage";
            this.diagnosticPage.Size = new System.Drawing.Size(881, 510);
            this.diagnosticPage.TabIndex = 0;
            // 
            // tpDiscovery
            // 
            this.tpDiscovery.Location = new System.Drawing.Point(4, 22);
            this.tpDiscovery.Name = "tpDiscovery";
            this.tpDiscovery.Padding = new System.Windows.Forms.Padding(3);
            this.tpDiscovery.Size = new System.Drawing.Size(753, 471);
            this.tpDiscovery.TabIndex = 0;
            this.tpDiscovery.Text = "Discovery";
            this.tpDiscovery.UseVisualStyleBackColor = true;
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLabelState});
            this.status.Location = new System.Drawing.Point(0, 545);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(895, 22);
            this.status.TabIndex = 1;
            this.status.Text = "statusStrip1";
            // 
            // tssLabelState
            // 
            this.tssLabelState.Name = "tssLabelState";
            this.tssLabelState.Size = new System.Drawing.Size(75, 17);
            this.tssLabelState.Text = "Not Running";
            // 
            // tpCustomConfiguration
            // 
            this.tpCustomConfiguration.Controls.Add(this.customConfigurationPage1);
            this.tpCustomConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tpCustomConfiguration.Name = "tpCustomConfiguration";
            this.tpCustomConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomConfiguration.Size = new System.Drawing.Size(887, 516);
            this.tpCustomConfiguration.TabIndex = 4;
            this.tpCustomConfiguration.Text = "Custom Configuration";
            this.tpCustomConfiguration.UseVisualStyleBackColor = true;
            // 
            // customConfigurationPage1
            // 
            this.customConfigurationPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customConfigurationPage1.Location = new System.Drawing.Point(3, 3);
            this.customConfigurationPage1.Name = "customConfigurationPage1";
            this.customConfigurationPage1.Size = new System.Drawing.Size(881, 510);
            this.customConfigurationPage1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 567);
            this.Controls.Add(this.status);
            this.Controls.Add(this.tcMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ONVIF Client Test Tool";
            this.tcMain.ResumeLayout(false);
            this.tpManagement.ResumeLayout(false);
            this.tpConformance.ResumeLayout(false);
            this.tpDiagnostic.ResumeLayout(false);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.tpCustomConfiguration.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpDiscovery;
        private System.Windows.Forms.TabPage tpManagement;
        private System.Windows.Forms.TabPage tpConformance;
        private System.Windows.Forms.TabPage tpDiagnostic;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelState;
        private TestTool.GUI.Controls.Pages.TestPage diagnosticPage;
        private TestTool.GUI.Controls.Pages.ManagementPage managementPage;
        private TestTool.GUI.Controls.Pages.TestInfoPage testInfoPage;
        private System.Windows.Forms.TabPage tpCustomConfiguration;
        private TestTool.GUI.Controls.Pages.CustomConfigurationPage customConfigurationPage1;
    }
}

