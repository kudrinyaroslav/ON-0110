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
            this.tpDiscovery = new System.Windows.Forms.TabPage();
            this.tpManagement = new System.Windows.Forms.TabPage();
            this.tpConformance = new System.Windows.Forms.TabPage();
            this.tpTest = new System.Windows.Forms.TabPage();
            this.tpDevice = new System.Windows.Forms.TabPage();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splashImage = new TestTool.GUI.Controls.SplashImage();
            this.discoveryPage = new TestTool.GUI.Controls.DiscoveryPage();
            this.managementPage = new TestTool.GUI.Controls.ManagementPage();
            this.setupPage = new TestTool.GUI.Controls.ConformanceTestPage();
            this.testPage = new TestTool.GUI.Controls.TestPage();
            this.devicePage = new TestTool.GUI.Controls.DevicePage();
            this.tcPages.SuspendLayout();
            this.tpDiscovery.SuspendLayout();
            this.tpManagement.SuspendLayout();
            this.tpConformance.SuspendLayout();
            this.tpTest.SuspendLayout();
            this.tpDevice.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPages
            // 
            this.tcPages.Controls.Add(this.tpDiscovery);
            this.tcPages.Controls.Add(this.tpManagement);
            this.tcPages.Controls.Add(this.tpConformance);
            this.tcPages.Controls.Add(this.tpTest);
            this.tcPages.Controls.Add(this.tpDevice);
            this.tcPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPages.Location = new System.Drawing.Point(0, 24);
            this.tcPages.Name = "tcPages";
            this.tcPages.SelectedIndex = 0;
            this.tcPages.Size = new System.Drawing.Size(974, 818);
            this.tcPages.TabIndex = 0;
            this.tcPages.SelectedIndexChanged += new System.EventHandler(this.tcPages_SelectedIndexChanged);
            // 
            // tpDiscovery
            // 
            this.tpDiscovery.Controls.Add(this.discoveryPage);
            this.tpDiscovery.Location = new System.Drawing.Point(4, 22);
            this.tpDiscovery.Name = "tpDiscovery";
            this.tpDiscovery.Padding = new System.Windows.Forms.Padding(3);
            this.tpDiscovery.Size = new System.Drawing.Size(966, 792);
            this.tpDiscovery.TabIndex = 1;
            this.tpDiscovery.Text = "Discovery";
            this.tpDiscovery.UseVisualStyleBackColor = true;
            // 
            // tpManagement
            // 
            this.tpManagement.Controls.Add(this.managementPage);
            this.tpManagement.Location = new System.Drawing.Point(4, 22);
            this.tpManagement.Name = "tpManagement";
            this.tpManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tpManagement.Size = new System.Drawing.Size(892, 792);
            this.tpManagement.TabIndex = 2;
            this.tpManagement.Text = "Management";
            this.tpManagement.UseVisualStyleBackColor = true;
            // 
            // tpConformance
            // 
            this.tpConformance.Controls.Add(this.setupPage);
            this.tpConformance.Location = new System.Drawing.Point(4, 22);
            this.tpConformance.Name = "tpConformance";
            this.tpConformance.Padding = new System.Windows.Forms.Padding(3);
            this.tpConformance.Size = new System.Drawing.Size(892, 792);
            this.tpConformance.TabIndex = 0;
            this.tpConformance.Text = "Conformance Test";
            this.tpConformance.UseVisualStyleBackColor = true;
            // 
            // tpTest
            // 
            this.tpTest.Controls.Add(this.testPage);
            this.tpTest.Location = new System.Drawing.Point(4, 22);
            this.tpTest.Name = "tpTest";
            this.tpTest.Padding = new System.Windows.Forms.Padding(3);
            this.tpTest.Size = new System.Drawing.Size(892, 792);
            this.tpTest.TabIndex = 3;
            this.tpTest.Text = "Diagnostics";
            this.tpTest.UseVisualStyleBackColor = true;
            // 
            // tpDevice
            // 
            this.tpDevice.Controls.Add(this.devicePage);
            this.tpDevice.Location = new System.Drawing.Point(4, 22);
            this.tpDevice.Name = "tpDevice";
            this.tpDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevice.Size = new System.Drawing.Size(892, 792);
            this.tpDevice.TabIndex = 5;
            this.tpDevice.Text = "Debug";
            this.tpDevice.UseVisualStyleBackColor = true;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenuStrip.Size = new System.Drawing.Size(974, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howDoIToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howDoIToolStripMenuItem
            // 
            this.howDoIToolStripMenuItem.Name = "howDoIToolStripMenuItem";
            this.howDoIToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.howDoIToolStripMenuItem.Text = "How Do I...";
            this.howDoIToolStripMenuItem.Click += new System.EventHandler(this.howDoIToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splashImage
            // 
            this.splashImage.BackColor = System.Drawing.Color.Black;
            this.splashImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splashImage.Location = new System.Drawing.Point(0, 24);
            this.splashImage.Name = "splashImage";
            this.splashImage.Size = new System.Drawing.Size(974, 818);
            this.splashImage.TabIndex = 2;
            // 
            // discoveryPage
            // 
            this.discoveryPage.DeviceAddress = null;
            this.discoveryPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discoveryPage.Location = new System.Drawing.Point(3, 3);
            this.discoveryPage.Name = "discoveryPage";
            this.discoveryPage.Password = "";
            this.discoveryPage.SearchScopes = "";
            this.discoveryPage.ServiceAddress = "";
            this.discoveryPage.ShowOptions = true;
            this.discoveryPage.Size = new System.Drawing.Size(960, 786);
            this.discoveryPage.TabIndex = 0;
            this.discoveryPage.UserName = "";
            // 
            // managementPage
            // 
            this.managementPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementPage.Location = new System.Drawing.Point(3, 3);
            this.managementPage.Name = "managementPage";
            this.managementPage.OpenFileForEditing = false;
            this.managementPage.Size = new System.Drawing.Size(886, 786);
            this.managementPage.TabIndex = 0;
            // 
            // setupPage
            // 
            this.setupPage.Brand = "";
            this.setupPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupPage.InternationalAddress = "";
            this.setupPage.Location = new System.Drawing.Point(3, 3);
            this.setupPage.MemberAddress = "";
            this.setupPage.MemberName = "";
            this.setupPage.Model = "";
            this.setupPage.Name = "setupPage";
            this.setupPage.OnvifProductName = "";
            this.setupPage.OperatorName = "";
            this.setupPage.OrganizationAddress = "";
            this.setupPage.OrganizationName = "";
            this.setupPage.OtherInformation = "";
            this.setupPage.ProductTypes = "";
            this.setupPage.ProductTypesOther = "";
            this.setupPage.RegionalAddress = "";
            this.setupPage.Size = new System.Drawing.Size(886, 786);
            this.setupPage.SupportEmail = "";
            this.setupPage.SupportPhone = "";
            this.setupPage.SupportUrl = "";
            this.setupPage.TabIndex = 0;
            // 
            // testPage
            // 
            this.testPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPage.Location = new System.Drawing.Point(3, 3);
            this.testPage.Name = "testPage";
            this.testPage.Repeat = false;
            this.testPage.Size = new System.Drawing.Size(886, 786);
            this.testPage.TabIndex = 0;
            // 
            // devicePage
            // 
            this.devicePage.CapabilitiesExchange = TestTool.GUI.Data.CapabilitiesExchangeStyle.GetServices;
            this.devicePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicePage.Location = new System.Drawing.Point(3, 3);
            this.devicePage.Name = "devicePage";
            this.devicePage.Security = TestTool.HttpTransport.Interfaces.Security.Digest;
            this.devicePage.Size = new System.Drawing.Size(886, 786);
            this.devicePage.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 842);
            this.Controls.Add(this.splashImage);
            this.Controls.Add(this.tcPages);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(900, 830);
            this.Name = "MainForm";
            this.Text = "ONVIF Device Test Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.tcPages.ResumeLayout(false);
            this.tpDiscovery.ResumeLayout(false);
            this.tpManagement.ResumeLayout(false);
            this.tpConformance.ResumeLayout(false);
            this.tpTest.ResumeLayout(false);
            this.tpDevice.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcPages;
        private System.Windows.Forms.TabPage tpConformance;
        private System.Windows.Forms.TabPage tpDiscovery;
        private TestTool.GUI.Controls.ConformanceTestPage setupPage;
        private TestTool.GUI.Controls.DiscoveryPage discoveryPage;
        private System.Windows.Forms.TabPage tpManagement;
        private System.Windows.Forms.TabPage tpTest;
        private System.Windows.Forms.TabPage tpDevice;
        private TestTool.GUI.Controls.ManagementPage managementPage;
        private TestTool.GUI.Controls.TestPage testPage;
        private TestTool.GUI.Controls.DevicePage devicePage;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howDoIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TestTool.GUI.Controls.SplashImage splashImage;
    }
}

