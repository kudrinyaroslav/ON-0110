namespace TestTool.GUI.Controls
{
    partial class TestResultsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestResultsControl));
            this.tcTestResults = new System.Windows.Forms.TabControl();
            this.tpTestResult = new System.Windows.Forms.TabPage();
            this.scOutputResultBorder = new System.Windows.Forms.SplitContainer();
            this.rtbTestResult = new System.Windows.Forms.RichTextBox();
            this.cmsTestLogMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bPrevFailed = new System.Windows.Forms.Button();
            this.bNextFailed = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bPrev = new System.Windows.Forms.Button();
            this.bNext = new System.Windows.Forms.Button();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.labFind = new System.Windows.Forms.Label();
            this.tpStepDetails = new System.Windows.Forms.TabPage();
            this.scStepDetails = new System.Windows.Forms.SplitContainer();
            this.lvStepDetails = new System.Windows.Forms.ListView();
            this.hdrStep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrDetails = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ilStepIcons = new System.Windows.Forms.ImageList(this.components);
            this.scTraffic = new System.Windows.Forms.SplitContainer();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.tcTestResults.SuspendLayout();
            this.tpTestResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOutputResultBorder)).BeginInit();
            this.scOutputResultBorder.Panel1.SuspendLayout();
            this.scOutputResultBorder.Panel2.SuspendLayout();
            this.scOutputResultBorder.SuspendLayout();
            this.cmsTestLogMenu.SuspendLayout();
            this.tpStepDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scStepDetails)).BeginInit();
            this.scStepDetails.Panel1.SuspendLayout();
            this.scStepDetails.Panel2.SuspendLayout();
            this.scStepDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTraffic)).BeginInit();
            this.scTraffic.Panel1.SuspendLayout();
            this.scTraffic.Panel2.SuspendLayout();
            this.scTraffic.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcTestResults
            // 
            this.tcTestResults.Controls.Add(this.tpTestResult);
            this.tcTestResults.Controls.Add(this.tpStepDetails);
            this.tcTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTestResults.Location = new System.Drawing.Point(0, 0);
            this.tcTestResults.Name = "tcTestResults";
            this.tcTestResults.SelectedIndex = 0;
            this.tcTestResults.Size = new System.Drawing.Size(591, 424);
            this.tcTestResults.TabIndex = 4;
            // 
            // tpTestResult
            // 
            this.tpTestResult.Controls.Add(this.scOutputResultBorder);
            this.tpTestResult.Location = new System.Drawing.Point(4, 22);
            this.tpTestResult.Name = "tpTestResult";
            this.tpTestResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestResult.Size = new System.Drawing.Size(583, 398);
            this.tpTestResult.TabIndex = 0;
            this.tpTestResult.Text = "Output";
            this.tpTestResult.UseVisualStyleBackColor = true;
            // 
            // scOutputResultBorder
            // 
            this.scOutputResultBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scOutputResultBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOutputResultBorder.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scOutputResultBorder.IsSplitterFixed = true;
            this.scOutputResultBorder.Location = new System.Drawing.Point(3, 3);
            this.scOutputResultBorder.Name = "scOutputResultBorder";
            this.scOutputResultBorder.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scOutputResultBorder.Panel1
            // 
            this.scOutputResultBorder.Panel1.Controls.Add(this.rtbTestResult);
            // 
            // scOutputResultBorder.Panel2
            // 
            this.scOutputResultBorder.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.scOutputResultBorder.Panel2.Controls.Add(this.bPrevFailed);
            this.scOutputResultBorder.Panel2.Controls.Add(this.bNextFailed);
            this.scOutputResultBorder.Panel2.Controls.Add(this.label1);
            this.scOutputResultBorder.Panel2.Controls.Add(this.bPrev);
            this.scOutputResultBorder.Panel2.Controls.Add(this.bNext);
            this.scOutputResultBorder.Panel2.Controls.Add(this.tbFind);
            this.scOutputResultBorder.Panel2.Controls.Add(this.labFind);
            this.scOutputResultBorder.Size = new System.Drawing.Size(577, 392);
            this.scOutputResultBorder.SplitterDistance = 345;
            this.scOutputResultBorder.TabIndex = 5;
            // 
            // rtbTestResult
            // 
            this.rtbTestResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTestResult.ContextMenuStrip = this.cmsTestLogMenu;
            this.rtbTestResult.DetectUrls = false;
            this.rtbTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTestResult.Location = new System.Drawing.Point(0, 0);
            this.rtbTestResult.Name = "rtbTestResult";
            this.rtbTestResult.ReadOnly = true;
            this.rtbTestResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbTestResult.Size = new System.Drawing.Size(575, 343);
            this.rtbTestResult.TabIndex = 4;
            this.rtbTestResult.Text = "";
            this.rtbTestResult.WordWrap = false;
            // 
            // cmsTestLogMenu
            // 
            this.cmsTestLogMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectAllToolStripMenuItem});
            this.cmsTestLogMenu.Name = "cmsTestLogMenu";
            this.cmsTestLogMenu.Size = new System.Drawing.Size(123, 54);
            this.cmsTestLogMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTestLogMenu_Opening);
            this.cmsTestLogMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsTestLogMenu_ItemClicked);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // bPrevFailed
            // 
            this.bPrevFailed.Image = global::TestTool.GUI.Properties.Resources.PreviousArrowFailedStep;
            this.bPrevFailed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bPrevFailed.Location = new System.Drawing.Point(486, 8);
            this.bPrevFailed.Name = "bPrevFailed";
            this.bPrevFailed.Size = new System.Drawing.Size(81, 24);
            this.bPrevFailed.TabIndex = 12;
            this.bPrevFailed.Text = "Previous";
            this.bPrevFailed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bPrevFailed.UseVisualStyleBackColor = true;
            this.bPrevFailed.Click += new System.EventHandler(this.bPrevFailed_Click);
            // 
            // bNextFailed
            // 
            this.bNextFailed.Image = global::TestTool.GUI.Properties.Resources.NextArrowFailedStep;
            this.bNextFailed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bNextFailed.Location = new System.Drawing.Point(420, 8);
            this.bNextFailed.Name = "bNextFailed";
            this.bNextFailed.Size = new System.Drawing.Size(63, 24);
            this.bNextFailed.TabIndex = 11;
            this.bNextFailed.Text = "Next";
            this.bNextFailed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bNextFailed.UseVisualStyleBackColor = true;
            this.bNextFailed.Click += new System.EventHandler(this.bNextFailed_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(358, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Failed Step:";
            // 
            // bPrev
            // 
            this.bPrev.Image = global::TestTool.GUI.Properties.Resources.PreviousArrow;
            this.bPrev.Location = new System.Drawing.Point(257, 9);
            this.bPrev.Name = "bPrev";
            this.bPrev.Size = new System.Drawing.Size(81, 24);
            this.bPrev.TabIndex = 3;
            this.bPrev.Text = "Previous";
            this.bPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bPrev.UseVisualStyleBackColor = true;
            this.bPrev.Click += new System.EventHandler(this.bPrev_Click);
            // 
            // bNext
            // 
            this.bNext.Image = global::TestTool.GUI.Properties.Resources.NextArrow;
            this.bNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bNext.Location = new System.Drawing.Point(191, 9);
            this.bNext.Name = "bNext";
            this.bNext.Size = new System.Drawing.Size(63, 24);
            this.bNext.TabIndex = 2;
            this.bNext.Text = "Next";
            this.bNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bNext.UseVisualStyleBackColor = true;
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // tbFind
            // 
            this.tbFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFind.Location = new System.Drawing.Point(36, 11);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(149, 20);
            this.tbFind.TabIndex = 1;
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            this.tbFind.Enter += new System.EventHandler(this.tbFind_Enter);
            this.tbFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFind_KeyDown);
            // 
            // labFind
            // 
            this.labFind.AutoSize = true;
            this.labFind.Location = new System.Drawing.Point(6, 15);
            this.labFind.Name = "labFind";
            this.labFind.Size = new System.Drawing.Size(30, 13);
            this.labFind.TabIndex = 0;
            this.labFind.Text = "Find:";
            this.labFind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpStepDetails
            // 
            this.tpStepDetails.Controls.Add(this.scStepDetails);
            this.tpStepDetails.Location = new System.Drawing.Point(4, 22);
            this.tpStepDetails.Name = "tpStepDetails";
            this.tpStepDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpStepDetails.Size = new System.Drawing.Size(583, 398);
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
            this.scStepDetails.Size = new System.Drawing.Size(577, 392);
            this.scStepDetails.SplitterDistance = 127;
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
            this.lvStepDetails.Size = new System.Drawing.Size(577, 127);
            this.lvStepDetails.SmallImageList = this.ilStepIcons;
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
            // ilStepIcons
            // 
            this.ilStepIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilStepIcons.ImageStream")));
            this.ilStepIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilStepIcons.Images.SetKeyName(0, "NetworkOperation");
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
            this.scTraffic.Size = new System.Drawing.Size(577, 261);
            this.scTraffic.SplitterDistance = 295;
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
            this.tbRequest.Size = new System.Drawing.Size(289, 242);
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
            this.tbResponse.Size = new System.Drawing.Size(272, 241);
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
            // TestResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcTestResults);
            this.Name = "TestResultsControl";
            this.Size = new System.Drawing.Size(591, 424);
            this.tcTestResults.ResumeLayout(false);
            this.tpTestResult.ResumeLayout(false);
            this.scOutputResultBorder.Panel1.ResumeLayout(false);
            this.scOutputResultBorder.Panel2.ResumeLayout(false);
            this.scOutputResultBorder.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOutputResultBorder)).EndInit();
            this.scOutputResultBorder.ResumeLayout(false);
            this.cmsTestLogMenu.ResumeLayout(false);
            this.tpStepDetails.ResumeLayout(false);
            this.scStepDetails.Panel1.ResumeLayout(false);
            this.scStepDetails.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scStepDetails)).EndInit();
            this.scStepDetails.ResumeLayout(false);
            this.scTraffic.Panel1.ResumeLayout(false);
            this.scTraffic.Panel1.PerformLayout();
            this.scTraffic.Panel2.ResumeLayout(false);
            this.scTraffic.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTraffic)).EndInit();
            this.scTraffic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcTestResults;
        private System.Windows.Forms.TabPage tpTestResult;
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
        private System.Windows.Forms.ImageList ilStepIcons;
        private System.Windows.Forms.RichTextBox rtbTestResult;
        private System.Windows.Forms.SplitContainer scOutputResultBorder;
        private System.Windows.Forms.Label labFind;
        private System.Windows.Forms.TextBox tbFind;
        private System.Windows.Forms.Button bNext;
        private System.Windows.Forms.Button bPrev;
        private System.Windows.Forms.Button bPrevFailed;
        private System.Windows.Forms.Button bNextFailed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip cmsTestLogMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
