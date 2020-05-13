using TestTool.Tests.Common.Enums;

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
            this.tcPages = new System.Windows.Forms.TabControl();
            this.tpSetup = new System.Windows.Forms.TabPage();
            this.setupPage = new TestTool.GUI.Controls.SetupPage();
            this.tpDiscovery = new System.Windows.Forms.TabPage();
            this.discoveryPage = new TestTool.GUI.Controls.DiscoveryPage();
            this.tpManagement = new System.Windows.Forms.TabPage();
            this.managementPage = new TestTool.GUI.Controls.ManagementPage();
            this.tpTest = new System.Windows.Forms.TabPage();
            this.testPage = new TestTool.GUI.Controls.TestPage();
            this.tpReport = new System.Windows.Forms.TabPage();
            this.reportPage = new TestTool.GUI.Controls.ReportPage();
            this.tpDevice = new System.Windows.Forms.TabPage();
            this.devicePage = new TestTool.GUI.Controls.DevicePage();
            this.tpRequests = new System.Windows.Forms.TabPage();
            this.requestsPage = new TestTool.GUI.Controls.RequestsPage();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splashImage = new TestTool.GUI.Controls.SplashImage();
            this.tcPages.SuspendLayout();
            this.tpSetup.SuspendLayout();
            this.tpDiscovery.SuspendLayout();
            this.tpManagement.SuspendLayout();
            this.tpTest.SuspendLayout();
            this.tpReport.SuspendLayout();
            this.tpDevice.SuspendLayout();
            this.tpRequests.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPages
            // 
            this.tcPages.Controls.Add(this.tpSetup);
            this.tcPages.Controls.Add(this.tpDiscovery);
            this.tcPages.Controls.Add(this.tpManagement);
            this.tcPages.Controls.Add(this.tpTest);
            this.tcPages.Controls.Add(this.tpReport);
            this.tcPages.Controls.Add(this.tpDevice);
            this.tcPages.Controls.Add(this.tpRequests);
            this.tcPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPages.Location = new System.Drawing.Point(0, 24);
            this.tcPages.Name = "tcPages";
            this.tcPages.SelectedIndex = 0;
            this.tcPages.Size = new System.Drawing.Size(817, 516);
            this.tcPages.TabIndex = 0;
            this.tcPages.SelectedIndexChanged += new System.EventHandler(this.tcPages_SelectedIndexChanged);
            // 
            // tpSetup
            // 
            this.tpSetup.Controls.Add(this.setupPage);
            this.tpSetup.Location = new System.Drawing.Point(4, 22);
            this.tpSetup.Name = "tpSetup";
            this.tpSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tpSetup.Size = new System.Drawing.Size(809, 490);
            this.tpSetup.TabIndex = 0;
            this.tpSetup.Text = "Setup";
            this.tpSetup.UseVisualStyleBackColor = true;
            // 
            // setupPage
            // 
            this.setupPage.Brand = "";
            this.setupPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupPage.FirmwareVersion = "";
            this.setupPage.Location = new System.Drawing.Point(3, 3);
            this.setupPage.Model = "";
            this.setupPage.Name = "setupPage";
            this.setupPage.OperatorName = "";
            this.setupPage.OrganizationAddress = "";
            this.setupPage.OrganizationName = "";
            this.setupPage.OtherInformation = "";
            this.setupPage.Serial = "";
            this.setupPage.Size = new System.Drawing.Size(803, 484);
            this.setupPage.TabIndex = 0;
            // 
            // tpDiscovery
            // 
            this.tpDiscovery.Controls.Add(this.discoveryPage);
            this.tpDiscovery.Location = new System.Drawing.Point(4, 22);
            this.tpDiscovery.Name = "tpDiscovery";
            this.tpDiscovery.Padding = new System.Windows.Forms.Padding(3);
            this.tpDiscovery.Size = new System.Drawing.Size(809, 490);
            this.tpDiscovery.TabIndex = 1;
            this.tpDiscovery.Text = "Discovery";
            this.tpDiscovery.UseVisualStyleBackColor = true;
            // 
            // discoveryPage
            // 
            this.discoveryPage.DeviceAddress = null;
            this.discoveryPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discoveryPage.Location = new System.Drawing.Point(3, 3);
            this.discoveryPage.Name = "discoveryPage";
            this.discoveryPage.NICAddress = null;
            this.discoveryPage.ServiceAddress = "";
            this.discoveryPage.Size = new System.Drawing.Size(803, 484);
            this.discoveryPage.TabIndex = 0;
            // 
            // tpManagement
            // 
            this.tpManagement.Controls.Add(this.managementPage);
            this.tpManagement.Location = new System.Drawing.Point(4, 22);
            this.tpManagement.Name = "tpManagement";
            this.tpManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpManagement.Size = new System.Drawing.Size(809, 490);
            this.tpManagement.TabIndex = 2;
            this.tpManagement.Text = "Management";
            this.tpManagement.UseVisualStyleBackColor = true;
            // 
            // managementPage
            // 
            this.managementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementPage.Location = new System.Drawing.Point(3, 3);
            this.managementPage.MessageTimeout = 0;
            this.managementPage.Name = "managementPage";
            this.managementPage.RebootTimeout = 0;
            this.managementPage.Size = new System.Drawing.Size(803, 484);
            this.managementPage.TabIndex = 0;
            this.managementPage.TimeBetweenTests = 0;
            // 
            // tpTest
            // 
            this.tpTest.Controls.Add(this.testPage);
            this.tpTest.Location = new System.Drawing.Point(4, 22);
            this.tpTest.Name = "tpTest";
            this.tpTest.Padding = new System.Windows.Forms.Padding(3);
            this.tpTest.Size = new System.Drawing.Size(809, 490);
            this.tpTest.TabIndex = 3;
            this.tpTest.Text = "Test";
            this.tpTest.UseVisualStyleBackColor = true;
            // 
            // testPage
            // 
            this.testPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPage.Location = new System.Drawing.Point(3, 3);
            this.testPage.Name = "testPage";
            this.testPage.Size = new System.Drawing.Size(803, 484);
            this.testPage.TabIndex = 0;
            // 
            // tpReport
            // 
            this.tpReport.Controls.Add(this.reportPage);
            this.tpReport.Location = new System.Drawing.Point(4, 22);
            this.tpReport.Name = "tpReport";
            this.tpReport.Padding = new System.Windows.Forms.Padding(3);
            this.tpReport.Size = new System.Drawing.Size(809, 490);
            this.tpReport.TabIndex = 4;
            this.tpReport.Text = "Report";
            this.tpReport.UseVisualStyleBackColor = true;
            // 
            // reportPage
            // 
            this.reportPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportPage.FileName = "";
            this.reportPage.Location = new System.Drawing.Point(3, 3);
            this.reportPage.Name = "reportPage";
            this.reportPage.Size = new System.Drawing.Size(803, 484);
            this.reportPage.TabIndex = 0;
            // 
            // tpDevice
            // 
            this.tpDevice.Controls.Add(this.devicePage);
            this.tpDevice.Location = new System.Drawing.Point(4, 22);
            this.tpDevice.Name = "tpDevice";
            this.tpDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevice.Size = new System.Drawing.Size(809, 490);
            this.tpDevice.TabIndex = 5;
            this.tpDevice.Text = "Device";
            this.tpDevice.UseVisualStyleBackColor = true;
            // 
            // devicePage
            // 
            this.devicePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicePage.Location = new System.Drawing.Point(3, 3);
            this.devicePage.Name = "devicePage";
            this.devicePage.Size = new System.Drawing.Size(803, 484);
            this.devicePage.TabIndex = 0;
            // 
            // tpRequests
            // 
            this.tpRequests.Controls.Add(this.requestsPage);
            this.tpRequests.Location = new System.Drawing.Point(4, 22);
            this.tpRequests.Name = "tpRequests";
            this.tpRequests.Padding = new System.Windows.Forms.Padding(3);
            this.tpRequests.Size = new System.Drawing.Size(809, 490);
            this.tpRequests.TabIndex = 6;
            this.tpRequests.Text = "Requests";
            this.tpRequests.UseVisualStyleBackColor = true;
            // 
            // requestsPage
            // 
            this.requestsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requestsPage.Location = new System.Drawing.Point(3, 3);
            this.requestsPage.Name = "requestsPage";
            this.requestsPage.Service = Service.Device;
            this.requestsPage.ServiceAddress = "";
            this.requestsPage.Size = new System.Drawing.Size(803, 484);
            this.requestsPage.TabIndex = 0;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenuStrip.Size = new System.Drawing.Size(817, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howDoIToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howDoIToolStripMenuItem
            // 
            this.howDoIToolStripMenuItem.Name = "howDoIToolStripMenuItem";
            this.howDoIToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.howDoIToolStripMenuItem.Text = "How Do I...";
            this.howDoIToolStripMenuItem.Click += new System.EventHandler(this.howDoIToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splashImage
            // 
            this.splashImage.BackColor = System.Drawing.Color.Black;
            this.splashImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splashImage.Location = new System.Drawing.Point(0, 24);
            this.splashImage.Name = "splashImage";
            this.splashImage.Size = new System.Drawing.Size(817, 516);
            this.splashImage.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClientSize = new System.Drawing.Size(817, 540);
            this.Controls.Add(this.splashImage);
            this.Controls.Add(this.tcPages);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(750, 540);
            this.Name = "MainForm";
            this.Text = "ONVIF Conformance Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tcPages.ResumeLayout(false);
            this.tpSetup.ResumeLayout(false);
            this.tpDiscovery.ResumeLayout(false);
            this.tpManagement.ResumeLayout(false);
            this.tpTest.ResumeLayout(false);
            this.tpReport.ResumeLayout(false);
            this.tpDevice.ResumeLayout(false);
            this.tpRequests.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcPages;
        private System.Windows.Forms.TabPage tpSetup;
        private System.Windows.Forms.TabPage tpDiscovery;
        private TestTool.GUI.Controls.SetupPage setupPage;
        private TestTool.GUI.Controls.DiscoveryPage discoveryPage;
        private System.Windows.Forms.TabPage tpManagement;
        private System.Windows.Forms.TabPage tpTest;
        private System.Windows.Forms.TabPage tpReport;
        private System.Windows.Forms.TabPage tpDevice;
        private System.Windows.Forms.TabPage tpRequests;
        private TestTool.GUI.Controls.ManagementPage managementPage;
        private TestTool.GUI.Controls.TestPage testPage;
        private TestTool.GUI.Controls.ReportPage reportPage;
        private TestTool.GUI.Controls.DevicePage devicePage;
        private TestTool.GUI.Controls.RequestsPage requestsPage;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howDoIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TestTool.GUI.Controls.SplashImage splashImage;
    }
}

