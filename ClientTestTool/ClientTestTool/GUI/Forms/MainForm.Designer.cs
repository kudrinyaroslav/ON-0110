using ClientTestTool.GUI.Controls.Configuration;
using ClientTestTool.GUI.Controls.Conformance;
using ClientTestTool.GUI.Controls.Diagnostics;
using ClientTestTool.GUI.Controls.Reporting;

namespace ClientTestTool.GUI.Forms
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
      this.sSMain = new System.Windows.Forms.StatusStrip();
      this.mainToolStripStatusLabelActivity = new System.Windows.Forms.ToolStripStatusLabel();
      this.tSSLStatus = new System.Windows.Forms.ToolStripStatusLabel();
      this.tSSLPercentage = new System.Windows.Forms.ToolStripStatusLabel();
      this.tSPBProgress = new System.Windows.Forms.ToolStripProgressBar();
      this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
      this.tSMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
      this.addNetworkTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tSMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
      this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.userManualStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tCMain = new System.Windows.Forms.TabControl();
      this.tpConfiguration = new System.Windows.Forms.TabPage();
      this.configurationPage = new ClientTestTool.GUI.Controls.Configuration.PageConfiguration();
      this.tpReporting = new System.Windows.Forms.TabPage();
      this.reportingPage = new ClientTestTool.GUI.Controls.Reporting.PageReporting();
      this.tpDiagnostics = new System.Windows.Forms.TabPage();
      this.diagnosticsPage = new ClientTestTool.GUI.Controls.Diagnostics.PageDiagnostics();
      this.tpConformance = new System.Windows.Forms.TabPage();
      this.conformancePage = new ClientTestTool.GUI.Controls.Conformance.PageConformance();
      this.sSMain.SuspendLayout();
      this.mainMenuStrip.SuspendLayout();
      this.tCMain.SuspendLayout();
      this.tpConfiguration.SuspendLayout();
      this.tpReporting.SuspendLayout();
      this.tpDiagnostics.SuspendLayout();
      this.tpConformance.SuspendLayout();
      this.SuspendLayout();
      // 
      // sSMain
      // 
      this.sSMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripStatusLabelActivity,
            this.tSSLStatus,
            this.tSSLPercentage,
            this.tSPBProgress});
      resources.ApplyResources(this.sSMain, "sSMain");
      this.sSMain.Name = "sSMain";
      this.sSMain.SizingGrip = false;
      // 
      // mainToolStripStatusLabelActivity
      // 
      this.mainToolStripStatusLabelActivity.Name = "mainToolStripStatusLabelActivity";
      resources.ApplyResources(this.mainToolStripStatusLabelActivity, "mainToolStripStatusLabelActivity");
      // 
      // tSSLStatus
      // 
      this.tSSLStatus.Name = "tSSLStatus";
      resources.ApplyResources(this.tSSLStatus, "tSSLStatus");
      this.tSSLStatus.Spring = true;
      // 
      // tSSLPercentage
      // 
      this.tSSLPercentage.Name = "tSSLPercentage";
      resources.ApplyResources(this.tSSLPercentage, "tSSLPercentage");
      // 
      // tSPBProgress
      // 
      this.tSPBProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tSPBProgress.Name = "tSPBProgress";
      resources.ApplyResources(this.tSPBProgress, "tSPBProgress");
      // 
      // mainMenuStrip
      // 
      this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMenuItemFile,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
      resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
      this.mainMenuStrip.Name = "mainMenuStrip";
      // 
      // tSMenuItemFile
      // 
      this.tSMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNetworkTraceToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.tSMenuItemExit});
      this.tSMenuItemFile.Name = "tSMenuItemFile";
      resources.ApplyResources(this.tSMenuItemFile, "tSMenuItemFile");
      // 
      // addNetworkTraceToolStripMenuItem
      // 
      this.addNetworkTraceToolStripMenuItem.Name = "addNetworkTraceToolStripMenuItem";
      resources.ApplyResources(this.addNetworkTraceToolStripMenuItem, "addNetworkTraceToolStripMenuItem");
      this.addNetworkTraceToolStripMenuItem.Click += new System.EventHandler(this.addNetworkTraceToolStripMenuItem_Click);
      // 
      // configurationToolStripMenuItem
      // 
      this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
      this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
      resources.ApplyResources(this.configurationToolStripMenuItem, "configurationToolStripMenuItem");
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // loadToolStripMenuItem
      // 
      this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
      resources.ApplyResources(this.loadToolStripMenuItem, "loadToolStripMenuItem");
      this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
      // 
      // tSMenuItemExit
      // 
      this.tSMenuItemExit.Name = "tSMenuItemExit";
      resources.ApplyResources(this.tSMenuItemExit, "tSMenuItemExit");
      this.tSMenuItemExit.Click += new System.EventHandler(this.tSMenuItemExit_Click);
      // 
      // viewToolStripMenuItem
      // 
      this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLogToolStripMenuItem});
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
      // 
      // showLogToolStripMenuItem
      // 
      this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
      resources.ApplyResources(this.showLogToolStripMenuItem, "showLogToolStripMenuItem");
      this.showLogToolStripMenuItem.Click += new System.EventHandler(this.showLogToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManualStripMenuItem,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
      // 
      // userManualStripMenuItem
      // 
      this.userManualStripMenuItem.Name = "userManualStripMenuItem";
      resources.ApplyResources(this.userManualStripMenuItem, "userManualStripMenuItem");
      this.userManualStripMenuItem.Click += new System.EventHandler(this.userManualStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // tCMain
      // 
      this.tCMain.Controls.Add(this.tpConfiguration);
      this.tCMain.Controls.Add(this.tpReporting);
      this.tCMain.Controls.Add(this.tpDiagnostics);
      this.tCMain.Controls.Add(this.tpConformance);
      resources.ApplyResources(this.tCMain, "tCMain");
      this.tCMain.Multiline = true;
      this.tCMain.Name = "tCMain";
      this.tCMain.SelectedIndex = 0;
      this.tCMain.SelectedIndexChanged += new System.EventHandler(this.tCMain_SelectedIndexChanged);
      this.tCMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tCMain_Selecting);
      // 
      // tpConfiguration
      // 
      this.tpConfiguration.Controls.Add(this.configurationPage);
      resources.ApplyResources(this.tpConfiguration, "tpConfiguration");
      this.tpConfiguration.Name = "tpConfiguration";
      this.tpConfiguration.UseVisualStyleBackColor = true;
      // 
      // configurationPage
      // 
      resources.ApplyResources(this.configurationPage, "configurationPage");
      this.configurationPage.Name = "configurationPage";
      // 
      // tpReporting
      // 
      this.tpReporting.Controls.Add(this.reportingPage);
      resources.ApplyResources(this.tpReporting, "tpReporting");
      this.tpReporting.Name = "tpReporting";
      this.tpReporting.UseVisualStyleBackColor = true;
      // 
      // reportingPage
      // 
      resources.ApplyResources(this.reportingPage, "reportingPage");
      this.reportingPage.Name = "reportingPage";
      // 
      // tpDiagnostics
      // 
      this.tpDiagnostics.Controls.Add(this.diagnosticsPage);
      resources.ApplyResources(this.tpDiagnostics, "tpDiagnostics");
      this.tpDiagnostics.Name = "tpDiagnostics";
      this.tpDiagnostics.UseVisualStyleBackColor = true;
      // 
      // diagnosticsPage
      // 
      resources.ApplyResources(this.diagnosticsPage, "diagnosticsPage");
      this.diagnosticsPage.Name = "diagnosticsPage";
      // 
      // tpConformance
      // 
      this.tpConformance.Controls.Add(this.conformancePage);
      resources.ApplyResources(this.tpConformance, "tpConformance");
      this.tpConformance.Name = "tpConformance";
      this.tpConformance.UseVisualStyleBackColor = true;
      // 
      // conformancePage
      // 
      resources.ApplyResources(this.conformancePage, "conformancePage");
      this.conformancePage.Name = "conformancePage";
      // 
      // MainForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.tCMain);
      this.Controls.Add(this.sSMain);
      this.Controls.Add(this.mainMenuStrip);
      this.DoubleBuffered = true;
      this.MainMenuStrip = this.mainMenuStrip;
      this.Name = "MainForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
      this.sSMain.ResumeLayout(false);
      this.sSMain.PerformLayout();
      this.mainMenuStrip.ResumeLayout(false);
      this.mainMenuStrip.PerformLayout();
      this.tCMain.ResumeLayout(false);
      this.tpConfiguration.ResumeLayout(false);
      this.tpReporting.ResumeLayout(false);
      this.tpDiagnostics.ResumeLayout(false);
      this.tpConformance.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private PageConfiguration configurationPage;
        private PageConformance conformancePage;
        private PageDiagnostics diagnosticsPage;
        private PageReporting reportingPage;
        private System.Windows.Forms.StatusStrip sSMain;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tSMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tSMenuItemExit;
        private System.Windows.Forms.TabControl tCMain;
        private System.Windows.Forms.TabPage tpReporting;
        private System.Windows.Forms.TabPage tpConformance;
        private System.Windows.Forms.TabPage tpDiagnostics;
        private System.Windows.Forms.ToolStripStatusLabel mainToolStripStatusLabelActivity;
        private System.Windows.Forms.ToolStripProgressBar tSPBProgress;
        private System.Windows.Forms.TabPage tpConfiguration;
        private System.Windows.Forms.ToolStripStatusLabel tSSLPercentage;
        private System.Windows.Forms.ToolStripStatusLabel tSSLStatus;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNetworkTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
    }
}

