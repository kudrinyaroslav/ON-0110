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
            this.tpConfiguration = new System.Windows.Forms.TabPage();
            this.managementPage = new TestTool.GUI.Controls.ManagementPage();
            this.tpConformance = new System.Windows.Forms.TabPage();
            this.testInfoPage = new TestTool.GUI.Controls.TestInfoPage();
            this.tpDiagnostic = new System.Windows.Forms.TabPage();
            this.testPage = new TestTool.GUI.Controls.TestPage();
            this.tpDiscovery = new System.Windows.Forms.TabPage();
            this.status = new System.Windows.Forms.StatusStrip();
            this.tssLabelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcMain.SuspendLayout();
            this.tpConfiguration.SuspendLayout();
            this.tpConformance.SuspendLayout();
            this.tpDiagnostic.SuspendLayout();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpConfiguration);
            this.tcMain.Controls.Add(this.tpConformance);
            this.tcMain.Controls.Add(this.tpDiagnostic);
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(895, 542);
            this.tcMain.TabIndex = 0;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpConfiguration
            // 
            this.tpConfiguration.Controls.Add(this.managementPage);
            this.tpConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tpConfiguration.Name = "tpConfiguration";
            this.tpConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfiguration.Size = new System.Drawing.Size(887, 516);
            this.tpConfiguration.TabIndex = 1;
            this.tpConfiguration.Text = "Configuration";
            this.tpConfiguration.UseVisualStyleBackColor = true;
            // 
            // managementPage
            // 
            this.managementPage.BaseAddress = "http://fe80::789b:ffad:4453:9f6d%11:8080/";
            this.managementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementPage.Location = new System.Drawing.Point(3, 3);
            this.managementPage.Name = "managementPage";
            this.managementPage.Size = new System.Drawing.Size(881, 510);
            this.managementPage.TabIndex = 0;
            // 
            // tpConformance
            // 
            this.tpConformance.Controls.Add(this.testInfoPage);
            this.tpConformance.Location = new System.Drawing.Point(4, 22);
            this.tpConformance.Name = "tpConformance";
            this.tpConformance.Padding = new System.Windows.Forms.Padding(3);
            this.tpConformance.Size = new System.Drawing.Size(753, 471);
            this.tpConformance.TabIndex = 2;
            this.tpConformance.Text = "Conformance Test";
            this.tpConformance.UseVisualStyleBackColor = true;
            // 
            // testInfoPage
            // 
            this.testInfoPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testInfoPage.Location = new System.Drawing.Point(3, 3);
            this.testInfoPage.Name = "testInfoPage";
            this.testInfoPage.Size = new System.Drawing.Size(747, 465);
            this.testInfoPage.TabIndex = 0;
            // 
            // tpDiagnostic
            // 
            this.tpDiagnostic.Controls.Add(this.testPage);
            this.tpDiagnostic.Location = new System.Drawing.Point(4, 22);
            this.tpDiagnostic.Name = "tpDiagnostic";
            this.tpDiagnostic.Padding = new System.Windows.Forms.Padding(3);
            this.tpDiagnostic.Size = new System.Drawing.Size(753, 471);
            this.tpDiagnostic.TabIndex = 3;
            this.tpDiagnostic.Text = "Diagnostic";
            this.tpDiagnostic.UseVisualStyleBackColor = true;
            // 
            // diagnosticPage
            // 
            this.testPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPage.Location = new System.Drawing.Point(3, 3);
            this.testPage.Name = "testPage";
            this.testPage.Size = new System.Drawing.Size(747, 465);
            this.testPage.TabIndex = 0;
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
            this.tpConfiguration.ResumeLayout(false);
            this.tpConformance.ResumeLayout(false);
            this.tpDiagnostic.ResumeLayout(false);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpDiscovery;
        private System.Windows.Forms.TabPage tpConfiguration;
        private System.Windows.Forms.TabPage tpConformance;
        private System.Windows.Forms.TabPage tpDiagnostic;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelState;
        private TestTool.GUI.Controls.TestPage testPage;
        private TestTool.GUI.Controls.ManagementPage managementPage;
        private TestTool.GUI.Controls.TestInfoPage testInfoPage;
    }
}

