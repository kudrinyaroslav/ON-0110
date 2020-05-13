namespace TestTool.GUI.Controls
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
            this.tpRequests = new System.Windows.Forms.TabPage();
            this.lvRequests = new System.Windows.Forms.ListView();
            this.colTimestamp = new System.Windows.Forms.ColumnHeader();
            this.colDirection = new System.Windows.Forms.ColumnHeader();
            this.ilOperations = new System.Windows.Forms.ImageList(this.components);
            this.tpTests = new System.Windows.Forms.TabPage();
            this.tbMessage = new System.Windows.Forms.TextBox();
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
            this.splitContainer1.Panel2.Controls.Add(this.tbMessage);
            this.splitContainer1.Size = new System.Drawing.Size(766, 499);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 4;
            // 
            // tcLists
            // 
            this.tcLists.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcLists.Controls.Add(this.tpRequests);
            this.tcLists.Controls.Add(this.tpTests);
            this.tcLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcLists.Location = new System.Drawing.Point(0, 0);
            this.tcLists.Multiline = true;
            this.tcLists.Name = "tcLists";
            this.tcLists.SelectedIndex = 0;
            this.tcLists.Size = new System.Drawing.Size(255, 499);
            this.tcLists.TabIndex = 0;
            // 
            // tpRequests
            // 
            this.tpRequests.Controls.Add(this.lvRequests);
            this.tpRequests.Location = new System.Drawing.Point(23, 4);
            this.tpRequests.Name = "tpRequests";
            this.tpRequests.Padding = new System.Windows.Forms.Padding(3);
            this.tpRequests.Size = new System.Drawing.Size(228, 491);
            this.tpRequests.TabIndex = 0;
            this.tpRequests.Text = "Requests";
            this.tpRequests.UseVisualStyleBackColor = true;
            // 
            // lvRequests
            // 
            this.lvRequests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTimestamp,
            this.colDirection});
            this.lvRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRequests.FullRowSelect = true;
            this.lvRequests.Location = new System.Drawing.Point(3, 3);
            this.lvRequests.Name = "lvRequests";
            this.lvRequests.Size = new System.Drawing.Size(222, 485);
            this.lvRequests.SmallImageList = this.ilOperations;
            this.lvRequests.TabIndex = 0;
            this.lvRequests.UseCompatibleStateImageBehavior = false;
            this.lvRequests.View = System.Windows.Forms.View.Details;
            this.lvRequests.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvRequests_MouseDoubleClick);
            this.lvRequests.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvRequests_MouseClick);
            this.lvRequests.SelectedIndexChanged += new System.EventHandler(this.lvRequests_SelectedIndexChanged);
            // 
            // colTimestamp
            // 
            this.colTimestamp.Text = "Timestamp";
            this.colTimestamp.Width = 144;
            // 
            // colDirection
            // 
            this.colDirection.Text = "Direction";
            // 
            // ilOperations
            // 
            this.ilOperations.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilOperations.ImageStream")));
            this.ilOperations.TransparentColor = System.Drawing.Color.Transparent;
            this.ilOperations.Images.SetKeyName(0, "FromClient");
            this.ilOperations.Images.SetKeyName(1, "ToDevice");
            this.ilOperations.Images.SetKeyName(2, "FromDevice");
            this.ilOperations.Images.SetKeyName(3, "ToClient");
            // 
            // tpTests
            // 
            this.tpTests.Location = new System.Drawing.Point(23, 4);
            this.tpTests.Name = "tpTests";
            this.tpTests.Padding = new System.Windows.Forms.Padding(3);
            this.tpTests.Size = new System.Drawing.Size(228, 491);
            this.tpTests.TabIndex = 1;
            this.tpTests.Text = "Tests";
            this.tpTests.UseVisualStyleBackColor = true;
            // 
            // tbMessage
            // 
            this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessage.Location = new System.Drawing.Point(0, 0);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMessage.Size = new System.Drawing.Size(507, 499);
            this.tbMessage.TabIndex = 0;
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
            // 
            // tsbClear
            // 
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(54, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.Click += new System.EventHandler(this.toolStripButton4_Click);
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
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tcLists.ResumeLayout(false);
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
        private System.Windows.Forms.ImageList ilOperations;
        private System.Windows.Forms.ListView lvRequests;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.ColumnHeader colTimestamp;
        private System.Windows.Forms.ColumnHeader colDirection;
    }
}
