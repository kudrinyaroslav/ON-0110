namespace AutomatedTesting.GUI.Controls
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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpTestToolOutput = new System.Windows.Forms.TabPage();
            this.tbTestToolOutput = new System.Windows.Forms.TextBox();
            this.tbStepResults = new System.Windows.Forms.TabPage();
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
            this.tpSimulatorOutput = new System.Windows.Forms.TabPage();
            this.tbSimulatorOutput = new System.Windows.Forms.TextBox();
            this.tpTestDescription = new System.Windows.Forms.TabPage();
            this.tbTestName = new System.Windows.Forms.TextBox();
            this.lTestName = new System.Windows.Forms.Label();
            this.scTestDescription = new System.Windows.Forms.SplitContainer();
            this.tbTestDescription = new System.Windows.Forms.TextBox();
            this.tbTestExpectedResult = new System.Windows.Forms.TextBox();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.tbResults = new System.Windows.Forms.TextBox();
            this.tcMain.SuspendLayout();
            this.tpTestToolOutput.SuspendLayout();
            this.tbStepResults.SuspendLayout();
            this.scStepDetails.Panel1.SuspendLayout();
            this.scStepDetails.Panel2.SuspendLayout();
            this.scStepDetails.SuspendLayout();
            this.scTraffic.Panel1.SuspendLayout();
            this.scTraffic.Panel2.SuspendLayout();
            this.scTraffic.SuspendLayout();
            this.tpSimulatorOutput.SuspendLayout();
            this.tpTestDescription.SuspendLayout();
            this.scTestDescription.Panel1.SuspendLayout();
            this.scTestDescription.Panel2.SuspendLayout();
            this.scTestDescription.SuspendLayout();
            this.tpResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpTestToolOutput);
            this.tcMain.Controls.Add(this.tbStepResults);
            this.tcMain.Controls.Add(this.tpSimulatorOutput);
            this.tcMain.Controls.Add(this.tpTestDescription);
            this.tcMain.Controls.Add(this.tpResults);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(440, 434);
            this.tcMain.TabIndex = 0;
            // 
            // tpTestToolOutput
            // 
            this.tpTestToolOutput.Controls.Add(this.tbTestToolOutput);
            this.tpTestToolOutput.Location = new System.Drawing.Point(4, 22);
            this.tpTestToolOutput.Name = "tpTestToolOutput";
            this.tpTestToolOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestToolOutput.Size = new System.Drawing.Size(432, 408);
            this.tpTestToolOutput.TabIndex = 0;
            this.tpTestToolOutput.Text = "TestTool Output";
            this.tpTestToolOutput.UseVisualStyleBackColor = true;
            // 
            // tbTestToolOutput
            // 
            this.tbTestToolOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTestToolOutput.Location = new System.Drawing.Point(3, 3);
            this.tbTestToolOutput.Multiline = true;
            this.tbTestToolOutput.Name = "tbTestToolOutput";
            this.tbTestToolOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestToolOutput.Size = new System.Drawing.Size(426, 402);
            this.tbTestToolOutput.TabIndex = 1;
            // 
            // tbStepResults
            // 
            this.tbStepResults.Controls.Add(this.scStepDetails);
            this.tbStepResults.Location = new System.Drawing.Point(4, 22);
            this.tbStepResults.Name = "tbStepResults";
            this.tbStepResults.Padding = new System.Windows.Forms.Padding(3);
            this.tbStepResults.Size = new System.Drawing.Size(432, 408);
            this.tbStepResults.TabIndex = 2;
            this.tbStepResults.Text = "TestTool Step Results";
            this.tbStepResults.UseVisualStyleBackColor = true;
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
            this.scStepDetails.Size = new System.Drawing.Size(426, 402);
            this.scStepDetails.SplitterDistance = 130;
            this.scStepDetails.TabIndex = 7;
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
            this.lvStepDetails.Size = new System.Drawing.Size(426, 130);
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
            this.scTraffic.Size = new System.Drawing.Size(426, 268);
            this.scTraffic.SplitterDistance = 218;
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
            this.tbRequest.Size = new System.Drawing.Size(212, 249);
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
            this.tbResponse.Size = new System.Drawing.Size(198, 248);
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
            // tpSimulatorOutput
            // 
            this.tpSimulatorOutput.Controls.Add(this.tbSimulatorOutput);
            this.tpSimulatorOutput.Location = new System.Drawing.Point(4, 22);
            this.tpSimulatorOutput.Name = "tpSimulatorOutput";
            this.tpSimulatorOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpSimulatorOutput.Size = new System.Drawing.Size(432, 408);
            this.tpSimulatorOutput.TabIndex = 1;
            this.tpSimulatorOutput.Text = "Simulator Output";
            this.tpSimulatorOutput.UseVisualStyleBackColor = true;
            // 
            // tbSimulatorOutput
            // 
            this.tbSimulatorOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSimulatorOutput.Location = new System.Drawing.Point(3, 3);
            this.tbSimulatorOutput.Multiline = true;
            this.tbSimulatorOutput.Name = "tbSimulatorOutput";
            this.tbSimulatorOutput.Size = new System.Drawing.Size(426, 402);
            this.tbSimulatorOutput.TabIndex = 0;
            // 
            // tpTestDescription
            // 
            this.tpTestDescription.Controls.Add(this.tbTestName);
            this.tpTestDescription.Controls.Add(this.lTestName);
            this.tpTestDescription.Controls.Add(this.scTestDescription);
            this.tpTestDescription.Location = new System.Drawing.Point(4, 22);
            this.tpTestDescription.Name = "tpTestDescription";
            this.tpTestDescription.Size = new System.Drawing.Size(432, 408);
            this.tpTestDescription.TabIndex = 3;
            this.tpTestDescription.Text = "Test Description";
            this.tpTestDescription.UseVisualStyleBackColor = true;
            // 
            // tbTestName
            // 
            this.tbTestName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestName.Location = new System.Drawing.Point(71, 3);
            this.tbTestName.Name = "tbTestName";
            this.tbTestName.ReadOnly = true;
            this.tbTestName.Size = new System.Drawing.Size(358, 20);
            this.tbTestName.TabIndex = 2;
            // 
            // lTestName
            // 
            this.lTestName.AutoSize = true;
            this.lTestName.Location = new System.Drawing.Point(3, 6);
            this.lTestName.Name = "lTestName";
            this.lTestName.Size = new System.Drawing.Size(62, 13);
            this.lTestName.TabIndex = 1;
            this.lTestName.Text = "Test Name:";
            // 
            // scTestDescription
            // 
            this.scTestDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTestDescription.Location = new System.Drawing.Point(0, 0);
            this.scTestDescription.Name = "scTestDescription";
            this.scTestDescription.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTestDescription.Panel1
            // 
            this.scTestDescription.Panel1.Controls.Add(this.tbTestDescription);
            // 
            // scTestDescription.Panel2
            // 
            this.scTestDescription.Panel2.Controls.Add(this.tbTestExpectedResult);
            this.scTestDescription.Size = new System.Drawing.Size(432, 408);
            this.scTestDescription.SplitterDistance = 202;
            this.scTestDescription.TabIndex = 0;
            // 
            // tbTestDescription
            // 
            this.tbTestDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTestDescription.Location = new System.Drawing.Point(0, 0);
            this.tbTestDescription.Multiline = true;
            this.tbTestDescription.Name = "tbTestDescription";
            this.tbTestDescription.ReadOnly = true;
            this.tbTestDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestDescription.Size = new System.Drawing.Size(432, 202);
            this.tbTestDescription.TabIndex = 0;
            // 
            // tbTestExpectedResult
            // 
            this.tbTestExpectedResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTestExpectedResult.Location = new System.Drawing.Point(0, 0);
            this.tbTestExpectedResult.Multiline = true;
            this.tbTestExpectedResult.Name = "tbTestExpectedResult";
            this.tbTestExpectedResult.ReadOnly = true;
            this.tbTestExpectedResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestExpectedResult.Size = new System.Drawing.Size(432, 202);
            this.tbTestExpectedResult.TabIndex = 1;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.tbResults);
            this.tpResults.Location = new System.Drawing.Point(4, 22);
            this.tpResults.Name = "tpResults";
            this.tpResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpResults.Size = new System.Drawing.Size(432, 408);
            this.tpResults.TabIndex = 4;
            this.tpResults.Text = "Results";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // tbResults
            // 
            this.tbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResults.Location = new System.Drawing.Point(3, 3);
            this.tbResults.Multiline = true;
            this.tbResults.Name = "tbResults";
            this.tbResults.ReadOnly = true;
            this.tbResults.Size = new System.Drawing.Size(426, 402);
            this.tbResults.TabIndex = 0;
            // 
            // TestResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcMain);
            this.Name = "TestResultsControl";
            this.Size = new System.Drawing.Size(440, 434);
            this.tcMain.ResumeLayout(false);
            this.tpTestToolOutput.ResumeLayout(false);
            this.tpTestToolOutput.PerformLayout();
            this.tbStepResults.ResumeLayout(false);
            this.scStepDetails.Panel1.ResumeLayout(false);
            this.scStepDetails.Panel2.ResumeLayout(false);
            this.scStepDetails.ResumeLayout(false);
            this.scTraffic.Panel1.ResumeLayout(false);
            this.scTraffic.Panel1.PerformLayout();
            this.scTraffic.Panel2.ResumeLayout(false);
            this.scTraffic.Panel2.PerformLayout();
            this.scTraffic.ResumeLayout(false);
            this.tpSimulatorOutput.ResumeLayout(false);
            this.tpSimulatorOutput.PerformLayout();
            this.tpTestDescription.ResumeLayout(false);
            this.tpTestDescription.PerformLayout();
            this.scTestDescription.Panel1.ResumeLayout(false);
            this.scTestDescription.Panel1.PerformLayout();
            this.scTestDescription.Panel2.ResumeLayout(false);
            this.scTestDescription.Panel2.PerformLayout();
            this.scTestDescription.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            this.tpResults.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpTestToolOutput;
        private System.Windows.Forms.TabPage tpSimulatorOutput;
        private System.Windows.Forms.TextBox tbTestToolOutput;
        private System.Windows.Forms.TextBox tbSimulatorOutput;
        private System.Windows.Forms.TabPage tbStepResults;
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
        private System.Windows.Forms.TabPage tpTestDescription;
        private System.Windows.Forms.SplitContainer scTestDescription;
        private System.Windows.Forms.TextBox tbTestDescription;
        private System.Windows.Forms.TextBox tbTestExpectedResult;
        private System.Windows.Forms.TextBox tbTestName;
        private System.Windows.Forms.Label lTestName;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.TextBox tbResults;
    }
}
