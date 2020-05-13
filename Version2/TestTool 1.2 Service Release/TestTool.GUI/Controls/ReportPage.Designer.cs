namespace TestTool.GUI.Controls
{
    partial class ReportPage
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFileName = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.tbTestReport = new System.Windows.Forms.TextBox();
            this.lblTestSummary = new System.Windows.Forms.Label();
            this.reportPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(587, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.reportPageToolTip.SetToolTip(this.btnSave, "Save current tests results");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFileName
            // 
            this.btnFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileName.Location = new System.Drawing.Point(554, 17);
            this.btnFileName.Name = "btnFileName";
            this.btnFileName.Size = new System.Drawing.Size(27, 23);
            this.btnFileName.TabIndex = 8;
            this.btnFileName.Text = "...";
            this.reportPageToolTip.SetToolTip(this.btnFileName, "Select file");
            this.btnFileName.UseVisualStyleBackColor = true;
            this.btnFileName.Click += new System.EventHandler(this.btnFileName_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFileName.Location = new System.Drawing.Point(0, 17);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(548, 20);
            this.tbFileName.TabIndex = 7;
            // 
            // tbTestReport
            // 
            this.tbTestReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestReport.Location = new System.Drawing.Point(0, 22);
            this.tbTestReport.Multiline = true;
            this.tbTestReport.Name = "tbTestReport";
            this.tbTestReport.ReadOnly = true;
            this.tbTestReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestReport.Size = new System.Drawing.Size(662, 410);
            this.tbTestReport.TabIndex = 6;
            // 
            // lblTestSummary
            // 
            this.lblTestSummary.AutoSize = true;
            this.lblTestSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTestSummary.Location = new System.Drawing.Point(0, 0);
            this.lblTestSummary.Name = "lblTestSummary";
            this.lblTestSummary.Padding = new System.Windows.Forms.Padding(3);
            this.lblTestSummary.Size = new System.Drawing.Size(83, 19);
            this.lblTestSummary.TabIndex = 5;
            this.lblTestSummary.Text = "Test Summary:";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTestSummary);
            this.panelTop.Controls.Add(this.tbTestReport);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(662, 435);
            this.panelTop.TabIndex = 10;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.tbFileName);
            this.panelBottom.Controls.Add(this.btnFileName);
            this.panelBottom.Controls.Add(this.btnSave);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 435);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(662, 43);
            this.panelBottom.TabIndex = 11;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(662, 478);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFileName;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.TextBox tbTestReport;
        private System.Windows.Forms.Label lblTestSummary;
        private System.Windows.Forms.ToolTip reportPageToolTip;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
    }
}
