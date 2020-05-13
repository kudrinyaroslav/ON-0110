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
            this.toolStripTestManagement = new System.Windows.Forms.ToolStrip();
            this.tsbRunAll = new System.Windows.Forms.ToolStripButton();
            this.tsbRunSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPause = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.tsbHalt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbInteractiveFirst = new System.Windows.Forms.ToolStripButton();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.gbTestCases = new System.Windows.Forms.GroupBox();
            this.tvTestCases = new System.Windows.Forms.TreeView();
            this.ilTestIcons = new System.Windows.Forms.ImageList(this.components);
            this.tcTestResults = new System.Windows.Forms.TabControl();
            this.tpTestResult = new System.Windows.Forms.TabPage();
            this.tbTestResult = new System.Windows.Forms.TextBox();
            this.tpStepDetails = new System.Windows.Forms.TabPage();
            this.scStepDetails = new System.Windows.Forms.SplitContainer();
            this.lvStepDetails = new System.Windows.Forms.ListView();
            this.hdrStep = new System.Windows.Forms.ColumnHeader();
            this.hdrResult = new System.Windows.Forms.ColumnHeader();
            this.hdrDetails = new System.Windows.Forms.ColumnHeader();
            this.scTraffic = new System.Windows.Forms.SplitContainer();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.toolStripTestManagement.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.gbTestCases.SuspendLayout();
            this.tcTestResults.SuspendLayout();
            this.tpTestResult.SuspendLayout();
            this.tpStepDetails.SuspendLayout();
            this.scStepDetails.Panel1.SuspendLayout();
            this.scStepDetails.Panel2.SuspendLayout();
            this.scStepDetails.SuspendLayout();
            this.scTraffic.Panel1.SuspendLayout();
            this.scTraffic.Panel2.SuspendLayout();
            this.scTraffic.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTestManagement
            // 
            this.toolStripTestManagement.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTestManagement.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRunAll,
            this.tsbRunSelected,
            this.toolStripSeparator1,
            this.tsbPause,
            this.tsbStop,
            this.tsbHalt,
            this.toolStripSeparator2,
            this.tsbClear,
            this.toolStripSeparator3,
            this.tsbInteractiveFirst});
            this.toolStripTestManagement.Location = new System.Drawing.Point(0, 0);
            this.toolStripTestManagement.Name = "toolStripTestManagement";
            this.toolStripTestManagement.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripTestManagement.Size = new System.Drawing.Size(785, 25);
            this.toolStripTestManagement.TabIndex = 1;
            this.toolStripTestManagement.Text = "toolStripTestManagement";
            // 
            // tsbRunAll
            // 
            this.tsbRunAll.Enabled = false;
            this.tsbRunAll.Image = global::TestTool.GUI.Properties.Resources.RunAll;
            this.tsbRunAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunAll.Name = "tsbRunAll";
            this.tsbRunAll.Size = new System.Drawing.Size(46, 22);
            this.tsbRunAll.Text = "Run";
            this.tsbRunAll.ToolTipText = "Run all selected tests";
            this.tsbRunAll.Click += new System.EventHandler(this.tsbRunAll_Click);
            // 
            // tsbRunSelected
            // 
            this.tsbRunSelected.Enabled = false;
            this.tsbRunSelected.Image = global::TestTool.GUI.Properties.Resources.RunSelected;
            this.tsbRunSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunSelected.Name = "tsbRunSelected";
            this.tsbRunSelected.Size = new System.Drawing.Size(86, 22);
            this.tsbRunSelected.Text = "Run Current";
            this.tsbRunSelected.ToolTipText = "Run current test or group of tests";
            this.tsbRunSelected.Click += new System.EventHandler(this.tsbRunSelected_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPause
            // 
            this.tsbPause.Enabled = false;
            this.tsbPause.Image = global::TestTool.GUI.Properties.Resources.Pause;
            this.tsbPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPause.Name = "tsbPause";
            this.tsbPause.Size = new System.Drawing.Size(56, 22);
            this.tsbPause.Text = "Pause";
            this.tsbPause.ToolTipText = "Pause tests execution at IO operation";
            this.tsbPause.Click += new System.EventHandler(this.tsbPause_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.Enabled = false;
            this.tsbStop.Image = global::TestTool.GUI.Properties.Resources.Stop;
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(49, 22);
            this.tsbStop.Text = "Stop";
            this.tsbStop.ToolTipText = "Stop tests execution at the end of test";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // tsbHalt
            // 
            this.tsbHalt.Enabled = false;
            this.tsbHalt.Image = global::TestTool.GUI.Properties.Resources.Halt;
            this.tsbHalt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHalt.Name = "tsbHalt";
            this.tsbHalt.Size = new System.Drawing.Size(46, 22);
            this.tsbHalt.Text = "Halt";
            this.tsbHalt.ToolTipText = "Stop tests execution immediately";
            this.tsbHalt.Click += new System.EventHandler(this.tsbHalt_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = global::TestTool.GUI.Properties.Resources.Clear;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(52, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.ToolTipText = "Clear test log and report";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbInteractiveFirst
            // 
            this.tsbInteractiveFirst.Checked = true;
            this.tsbInteractiveFirst.CheckOnClick = true;
            this.tsbInteractiveFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbInteractiveFirst.Image = global::TestTool.GUI.Properties.Resources.OK;
            this.tsbInteractiveFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbInteractiveFirst.Name = "tsbInteractiveFirst";
            this.tsbInteractiveFirst.Size = new System.Drawing.Size(146, 22);
            this.tsbInteractiveFirst.Text = "Interactive Tests at First";
            this.tsbInteractiveFirst.ToolTipText = "Rearrange test flow to perform interactive tests at the beginning";
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.gbTestCases);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcTestResults);
            this.scMain.Size = new System.Drawing.Size(785, 464);
            this.scMain.SplitterDistance = 306;
            this.scMain.TabIndex = 6;
            // 
            // gbTestCases
            // 
            this.gbTestCases.Controls.Add(this.tvTestCases);
            this.gbTestCases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTestCases.Location = new System.Drawing.Point(0, 0);
            this.gbTestCases.Name = "gbTestCases";
            this.gbTestCases.Size = new System.Drawing.Size(306, 464);
            this.gbTestCases.TabIndex = 2;
            this.gbTestCases.TabStop = false;
            this.gbTestCases.Text = "Test Cases";
            // 
            // tvTestCases
            // 
            this.tvTestCases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTestCases.CheckBoxes = true;
            this.tvTestCases.FullRowSelect = true;
            this.tvTestCases.HideSelection = false;
            this.tvTestCases.ImageIndex = 0;
            this.tvTestCases.ImageList = this.ilTestIcons;
            this.tvTestCases.Indent = 19;
            this.tvTestCases.Location = new System.Drawing.Point(3, 16);
            this.tvTestCases.Name = "tvTestCases";
            this.tvTestCases.SelectedImageIndex = 0;
            this.tvTestCases.ShowNodeToolTips = true;
            this.tvTestCases.Size = new System.Drawing.Size(300, 444);
            this.tvTestCases.TabIndex = 2;
            this.tvTestCases.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterCheck);
            this.tvTestCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterSelect);
            this.tvTestCases.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeCheck);
            this.tvTestCases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            // 
            // ilTestIcons
            // 
            this.ilTestIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTestIcons.ImageStream")));
            this.ilTestIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTestIcons.Images.SetKeyName(0, "None.ico");
            this.ilTestIcons.Images.SetKeyName(1, "MUST.ico");
            this.ilTestIcons.Images.SetKeyName(2, "MUSTIFIML.ico");
            this.ilTestIcons.Images.SetKeyName(3, "MUSTIFSUP.ico");
            this.ilTestIcons.Images.SetKeyName(4, "MUSTIFSUPIMPL.ico");
            this.ilTestIcons.Images.SetKeyName(5, "SHOULD.ico");
            this.ilTestIcons.Images.SetKeyName(6, "SHOULDIFSUP.ico");
            this.ilTestIcons.Images.SetKeyName(7, "OPTIONAL.ico");
            this.ilTestIcons.Images.SetKeyName(8, "SHOULDIFIML.ico");
            this.ilTestIcons.Images.SetKeyName(9, "SHOULDIFSUPIMPL.ico");
            // 
            // tcTestResults
            // 
            this.tcTestResults.Controls.Add(this.tpTestResult);
            this.tcTestResults.Controls.Add(this.tpStepDetails);
            this.tcTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTestResults.Location = new System.Drawing.Point(0, 0);
            this.tcTestResults.Name = "tcTestResults";
            this.tcTestResults.SelectedIndex = 0;
            this.tcTestResults.Size = new System.Drawing.Size(475, 464);
            this.tcTestResults.TabIndex = 3;
            // 
            // tpTestResult
            // 
            this.tpTestResult.Controls.Add(this.tbTestResult);
            this.tpTestResult.Location = new System.Drawing.Point(4, 22);
            this.tpTestResult.Name = "tpTestResult";
            this.tpTestResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestResult.Size = new System.Drawing.Size(467, 438);
            this.tpTestResult.TabIndex = 0;
            this.tpTestResult.Text = "Test Result";
            this.tpTestResult.UseVisualStyleBackColor = true;
            // 
            // tbTestResult
            // 
            this.tbTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTestResult.Location = new System.Drawing.Point(3, 3);
            this.tbTestResult.Multiline = true;
            this.tbTestResult.Name = "tbTestResult";
            this.tbTestResult.ReadOnly = true;
            this.tbTestResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestResult.Size = new System.Drawing.Size(461, 432);
            this.tbTestResult.TabIndex = 4;
            // 
            // tpStepDetails
            // 
            this.tpStepDetails.Controls.Add(this.scStepDetails);
            this.tpStepDetails.Location = new System.Drawing.Point(4, 22);
            this.tpStepDetails.Name = "tpStepDetails";
            this.tpStepDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpStepDetails.Size = new System.Drawing.Size(467, 438);
            this.tpStepDetails.TabIndex = 1;
            this.tpStepDetails.Text = "Step Details";
            this.tpStepDetails.UseVisualStyleBackColor = true;
            // 
            // scStepDetails
            // 
            this.scStepDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scStepDetails.Location = new System.Drawing.Point(3, 3);
            this.scStepDetails.Name = "scStepDetails";
            this.scStepDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scStepDetails.Panel1
            // 
            this.scStepDetails.Panel1.Controls.Add(this.lvStepDetails);
            // 
            // scStepDetails.Panel2
            // 
            this.scStepDetails.Panel2.Controls.Add(this.scTraffic);
            this.scStepDetails.Size = new System.Drawing.Size(461, 432);
            this.scStepDetails.SplitterDistance = 141;
            this.scStepDetails.TabIndex = 6;
            // 
            // lvStepDetails
            // 
            this.lvStepDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrStep,
            this.hdrResult,
            this.hdrDetails});
            this.lvStepDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvStepDetails.FullRowSelect = true;
            this.lvStepDetails.GridLines = true;
            this.lvStepDetails.HideSelection = false;
            this.lvStepDetails.Location = new System.Drawing.Point(0, 0);
            this.lvStepDetails.Name = "lvStepDetails";
            this.lvStepDetails.ShowItemToolTips = true;
            this.lvStepDetails.Size = new System.Drawing.Size(461, 141);
            this.lvStepDetails.TabIndex = 5;
            this.lvStepDetails.UseCompatibleStateImageBehavior = false;
            this.lvStepDetails.View = System.Windows.Forms.View.Details;
            this.lvStepDetails.SelectedIndexChanged += new System.EventHandler(this.lvStepDetails_SelectedIndexChanged);
            // 
            // hdrStep
            // 
            this.hdrStep.Text = "Step";
            // 
            // hdrResult
            // 
            this.hdrResult.Text = "Result";
            // 
            // hdrDetails
            // 
            this.hdrDetails.Text = "Details";
            this.hdrDetails.Width = 374;
            // 
            // scTraffic
            // 
            this.scTraffic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTraffic.Location = new System.Drawing.Point(0, 0);
            this.scTraffic.Name = "scTraffic";
            // 
            // scTraffic.Panel1
            // 
            this.scTraffic.Panel1.Controls.Add(this.tbRequest);
            this.scTraffic.Panel1.Controls.Add(this.lblRequest);
            // 
            // scTraffic.Panel2
            // 
            this.scTraffic.Panel2.Controls.Add(this.tbResponse);
            this.scTraffic.Panel2.Controls.Add(this.lblResponse);
            this.scTraffic.Size = new System.Drawing.Size(461, 287);
            this.scTraffic.SplitterDistance = 237;
            this.scTraffic.TabIndex = 0;
            // 
            // tbRequest
            // 
            this.tbRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRequest.Location = new System.Drawing.Point(3, 16);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.ReadOnly = true;
            this.tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRequest.Size = new System.Drawing.Size(231, 268);
            this.tbRequest.TabIndex = 7;
            // 
            // lblRequest
            // 
            this.lblRequest.AutoSize = true;
            this.lblRequest.Location = new System.Drawing.Point(3, 0);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(50, 13);
            this.lblRequest.TabIndex = 0;
            this.lblRequest.Text = "Request:";
            // 
            // tbResponse
            // 
            this.tbResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResponse.Location = new System.Drawing.Point(3, 17);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResponse.Size = new System.Drawing.Size(214, 267);
            this.tbResponse.TabIndex = 9;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(3, 1);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(58, 13);
            this.lblResponse.TabIndex = 2;
            this.lblResponse.Text = "Response:";
            // 
            // TestPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.toolStripTestManagement);
            this.Name = "TestPage";
            this.Size = new System.Drawing.Size(785, 489);
            this.toolStripTestManagement.ResumeLayout(false);
            this.toolStripTestManagement.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.gbTestCases.ResumeLayout(false);
            this.tcTestResults.ResumeLayout(false);
            this.tpTestResult.ResumeLayout(false);
            this.tpTestResult.PerformLayout();
            this.tpStepDetails.ResumeLayout(false);
            this.scStepDetails.Panel1.ResumeLayout(false);
            this.scStepDetails.Panel2.ResumeLayout(false);
            this.scStepDetails.ResumeLayout(false);
            this.scTraffic.Panel1.ResumeLayout(false);
            this.scTraffic.Panel1.PerformLayout();
            this.scTraffic.Panel2.ResumeLayout(false);
            this.scTraffic.Panel2.PerformLayout();
            this.scTraffic.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripTestManagement;
        private System.Windows.Forms.ToolStripButton tsbRunAll;
        private System.Windows.Forms.ToolStripButton tsbRunSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbPause;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.ToolStripButton tsbHalt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbInteractiveFirst;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.GroupBox gbTestCases;
        private System.Windows.Forms.TreeView tvTestCases;
        private System.Windows.Forms.TabControl tcTestResults;
        private System.Windows.Forms.TabPage tpTestResult;
        private System.Windows.Forms.TextBox tbTestResult;
        private System.Windows.Forms.TabPage tpStepDetails;
        private System.Windows.Forms.SplitContainer scStepDetails;
        private System.Windows.Forms.ListView lvStepDetails;
        private System.Windows.Forms.ColumnHeader hdrStep;
        private System.Windows.Forms.ColumnHeader hdrResult;
        private System.Windows.Forms.ColumnHeader hdrDetails;
        private System.Windows.Forms.SplitContainer scTraffic;
        private System.Windows.Forms.TextBox tbRequest;
        private System.Windows.Forms.Label lblRequest;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.ImageList ilTestIcons;
    }
}
