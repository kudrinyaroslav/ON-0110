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
            this.ilStepIcons = new System.Windows.Forms.ImageList(this.components);
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
            // tcTestResults
            // 
            this.tcTestResults.Controls.Add(this.tpTestResult);
            this.tcTestResults.Controls.Add(this.tpStepDetails);
            this.tcTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTestResults.Location = new System.Drawing.Point(0, 0);
            this.tcTestResults.Name = "tcTestResults";
            this.tcTestResults.SelectedIndex = 0;
            this.tcTestResults.Size = new System.Drawing.Size(474, 424);
            this.tcTestResults.TabIndex = 4;
            // 
            // tpTestResult
            // 
            this.tpTestResult.Controls.Add(this.tbTestResult);
            this.tpTestResult.Location = new System.Drawing.Point(4, 22);
            this.tpTestResult.Name = "tpTestResult";
            this.tpTestResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestResult.Size = new System.Drawing.Size(466, 398);
            this.tpTestResult.TabIndex = 0;
            this.tpTestResult.Text = "Output";
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
            this.tbTestResult.Size = new System.Drawing.Size(460, 392);
            this.tbTestResult.TabIndex = 4;
            // 
            // tpStepDetails
            // 
            this.tpStepDetails.Controls.Add(this.scStepDetails);
            this.tpStepDetails.Location = new System.Drawing.Point(4, 22);
            this.tpStepDetails.Name = "tpStepDetails";
            this.tpStepDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpStepDetails.Size = new System.Drawing.Size(466, 398);
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
            this.scStepDetails.Size = new System.Drawing.Size(460, 392);
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
            this.lvStepDetails.Size = new System.Drawing.Size(460, 127);
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
            this.scTraffic.Size = new System.Drawing.Size(460, 261);
            this.scTraffic.SplitterDistance = 236;
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
            this.tbRequest.Size = new System.Drawing.Size(230, 242);
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
            this.tbResponse.Size = new System.Drawing.Size(214, 241);
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
            // ilStepIcons
            // 
            this.ilStepIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilStepIcons.ImageStream")));
            this.ilStepIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilStepIcons.Images.SetKeyName(0, "NetworkOperation");
            // 
            // TestResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcTestResults);
            this.Name = "TestResultsControl";
            this.Size = new System.Drawing.Size(474, 424);
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

        }

        #endregion

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
        private System.Windows.Forms.ImageList ilStepIcons;
    }
}
