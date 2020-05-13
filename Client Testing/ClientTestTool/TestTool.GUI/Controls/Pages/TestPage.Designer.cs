namespace TestTool.GUI.Controls.Pages
{
    partial class TestPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPage));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tcLists = new System.Windows.Forms.TabControl();
            this.tpTests = new System.Windows.Forms.TabPage();
            this.tvConfigurations = new System.Windows.Forms.TreeView();
            this.cmsConfigurationsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilConfigurations = new System.Windows.Forms.ImageList(this.components);
            this.tpRequests = new System.Windows.Forms.TabPage();
            this.tvOperations = new System.Windows.Forms.TreeView();
            this.ilOperations = new System.Windows.Forms.ImageList(this.components);
            this.lvLog = new TestTool.GUI.Controls.LogViewer();
            this.tsCommands = new System.Windows.Forms.ToolStrip();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tcLists.SuspendLayout();
            this.tpTests.SuspendLayout();
            this.cmsConfigurationsTree.SuspendLayout();
            this.tpRequests.SuspendLayout();
            this.tsCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tcLists);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvLog);
            this.splitContainer1.Size = new System.Drawing.Size(766, 499);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 4;
            // 
            // tcLists
            // 
            this.tcLists.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcLists.Controls.Add(this.tpTests);
            this.tcLists.Controls.Add(this.tpRequests);
            this.tcLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcLists.Location = new System.Drawing.Point(0, 0);
            this.tcLists.Multiline = true;
            this.tcLists.Name = "tcLists";
            this.tcLists.SelectedIndex = 0;
            this.tcLists.Size = new System.Drawing.Size(255, 499);
            this.tcLists.TabIndex = 0;
            // 
            // tpTests
            // 
            this.tpTests.Controls.Add(this.tvConfigurations);
            this.tpTests.Location = new System.Drawing.Point(23, 4);
            this.tpTests.Name = "tpTests";
            this.tpTests.Padding = new System.Windows.Forms.Padding(3);
            this.tpTests.Size = new System.Drawing.Size(228, 491);
            this.tpTests.TabIndex = 0;
            this.tpTests.Text = "Configurations";
            this.tpTests.UseVisualStyleBackColor = true;
            // 
            // tvConfigurations
            // 
            this.tvConfigurations.ContextMenuStrip = this.cmsConfigurationsTree;
            this.tvConfigurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvConfigurations.HideSelection = false;
            this.tvConfigurations.ImageIndex = 0;
            this.tvConfigurations.ImageList = this.ilConfigurations;
            this.tvConfigurations.Location = new System.Drawing.Point(3, 3);
            this.tvConfigurations.Name = "tvConfigurations";
            this.tvConfigurations.SelectedImageIndex = 0;
            this.tvConfigurations.Size = new System.Drawing.Size(222, 485);
            this.tvConfigurations.TabIndex = 0;
            this.tvConfigurations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvConfigurations_AfterSelect);
            this.tvConfigurations.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvConfigurations_BeforeSelect);
            // 
            // cmsConfigurationsTree
            // 
            this.cmsConfigurationsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem});
            this.cmsConfigurationsTree.Name = "cmsConfigurationsTree";
            this.cmsConfigurationsTree.Size = new System.Drawing.Size(104, 26);
            this.cmsConfigurationsTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmsConfigurationsTree_Opening);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // ilConfigurations
            // 
            this.ilConfigurations.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilConfigurations.ImageStream")));
            this.ilConfigurations.TransparentColor = System.Drawing.Color.Transparent;
            this.ilConfigurations.Images.SetKeyName(0, "Group");
            this.ilConfigurations.Images.SetKeyName(1, "Configuration");
            // 
            // tpRequests
            // 
            this.tpRequests.Controls.Add(this.tvOperations);
            this.tpRequests.Location = new System.Drawing.Point(23, 4);
            this.tpRequests.Name = "tpRequests";
            this.tpRequests.Padding = new System.Windows.Forms.Padding(3);
            this.tpRequests.Size = new System.Drawing.Size(228, 491);
            this.tpRequests.TabIndex = 1;
            this.tpRequests.Text = "Requests";
            this.tpRequests.UseVisualStyleBackColor = true;
            // 
            // tvOperations
            // 
            this.tvOperations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOperations.ImageIndex = 0;
            this.tvOperations.ImageList = this.ilOperations;
            this.tvOperations.Location = new System.Drawing.Point(3, 3);
            this.tvOperations.Name = "tvOperations";
            this.tvOperations.SelectedImageIndex = 0;
            this.tvOperations.Size = new System.Drawing.Size(222, 485);
            this.tvOperations.TabIndex = 0;
            this.tvOperations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvOperations_AfterSelect);
            this.tvOperations.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvOperations_NodeMouseClick);
            // 
            // ilOperations
            // 
            this.ilOperations.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilOperations.ImageStream")));
            this.ilOperations.TransparentColor = System.Drawing.Color.Transparent;
            this.ilOperations.Images.SetKeyName(0, "Service");
            this.ilOperations.Images.SetKeyName(1, "Undefined");
            this.ilOperations.Images.SetKeyName(2, "Supported");
            this.ilOperations.Images.SetKeyName(3, "NotSupported");
            // 
            // lvLog
            // 
            this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLog.Location = new System.Drawing.Point(0, 0);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(507, 499);
            this.lvLog.TabIndex = 0;
            // 
            // tsCommands
            // 
            this.tsCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStart,
            this.tsbStop,
            this.toolStripSeparator1,
            this.tsbSave,
            this.tsbClear});
            this.tsCommands.Location = new System.Drawing.Point(0, 0);
            this.tsCommands.Name = "tsCommands";
            this.tsCommands.Size = new System.Drawing.Size(766, 25);
            this.tsCommands.TabIndex = 3;
            this.tsCommands.Text = "toolStrip1";
            // 
            // tsbStart
            // 
            this.tsbStart.Enabled = false;
            this.tsbStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbStart.Image")));
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(51, 22);
            this.tsbStart.Text = "Start";
            this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.Enabled = false;
            this.tsbStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbStop.Image")));
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(51, 22);
            this.tsbStop.Text = "Stop";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSave
            // 
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(51, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(54, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // TestPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tsCommands);
            this.Name = "TestPage";
            this.Size = new System.Drawing.Size(766, 524);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tcLists.ResumeLayout(false);
            this.tpTests.ResumeLayout(false);
            this.cmsConfigurationsTree.ResumeLayout(false);
            this.tpRequests.ResumeLayout(false);
            this.tsCommands.ResumeLayout(false);
            this.tsCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tcLists;
        private System.Windows.Forms.TabPage tpRequests;
        private System.Windows.Forms.TabPage tpTests;
        private System.Windows.Forms.ToolStrip tsCommands;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private LogViewer lvLog;
        private System.Windows.Forms.TreeView tvOperations;
        private System.Windows.Forms.ImageList ilOperations;
        private System.Windows.Forms.TreeView tvConfigurations;
        private System.Windows.Forms.ImageList ilConfigurations;
        private System.Windows.Forms.ContextMenuStrip cmsConfigurationsTree;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
    }
}
