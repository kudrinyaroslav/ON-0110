namespace ReportViewer
{
    partial class fMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.lReport = new System.Windows.Forms.Label();
            this.bOpenReport = new System.Windows.Forms.Button();
            this.ofdReport = new System.Windows.Forms.OpenFileDialog();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.bSaveReport = new System.Windows.Forms.Button();
            this.dgvChecks = new System.Windows.Forms.DataGridView();
            this.gvdcMAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvdcTestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvdcTestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvdcExpectedResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvdcCurrentResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcCheckResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSteps = new System.Windows.Forms.DataGridView();
            this.dgvcStepDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcStepResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcStepDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lResults = new System.Windows.Forms.Label();
            this.bLoadResults = new System.Windows.Forms.Button();
            this.tbResults = new System.Windows.Forms.TextBox();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scSecond = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dvgcID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcRelatedItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgccOldResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcRelatedBugs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRelatedTraces = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgcRelatedFeatures = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSecond)).BeginInit();
            this.scSecond.Panel1.SuspendLayout();
            this.scSecond.Panel2.SuspendLayout();
            this.scSecond.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbReport
            // 
            this.tbReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReport.Location = new System.Drawing.Point(60, 12);
            this.tbReport.Name = "tbReport";
            this.tbReport.ReadOnly = true;
            this.tbReport.Size = new System.Drawing.Size(998, 20);
            this.tbReport.TabIndex = 0;
            this.tbReport.TextChanged += new System.EventHandler(this.tbReport_TextChanged);
            // 
            // lReport
            // 
            this.lReport.AutoSize = true;
            this.lReport.Location = new System.Drawing.Point(12, 15);
            this.lReport.Name = "lReport";
            this.lReport.Size = new System.Drawing.Size(42, 13);
            this.lReport.TabIndex = 1;
            this.lReport.Text = "Report:";
            this.lReport.Click += new System.EventHandler(this.lReport_Click);
            // 
            // bOpenReport
            // 
            this.bOpenReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOpenReport.Location = new System.Drawing.Point(1064, 10);
            this.bOpenReport.Name = "bOpenReport";
            this.bOpenReport.Size = new System.Drawing.Size(75, 23);
            this.bOpenReport.TabIndex = 2;
            this.bOpenReport.Text = "Open";
            this.bOpenReport.UseVisualStyleBackColor = true;
            this.bOpenReport.Click += new System.EventHandler(this.bOpenReport_Click);
            // 
            // ofdReport
            // 
            this.ofdReport.Filter = "XML files|*.xml";
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dvgcID,
            this.dgvcName,
            this.dvgcDescription,
            this.dvgcRelatedItems,
            this.dgccOldResult,
            this.dvgcResult,
            this.dvgcRelatedBugs,
            this.dgvcRelatedTraces,
            this.dvgcRelatedFeatures});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvReport.Location = new System.Drawing.Point(0, 0);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvReport.Size = new System.Drawing.Size(1208, 250);
            this.dgvReport.TabIndex = 4;
            this.dgvReport.SelectionChanged += new System.EventHandler(this.dgvReport_SelectionChanged);
            // 
            // bSaveReport
            // 
            this.bSaveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSaveReport.Location = new System.Drawing.Point(1145, 10);
            this.bSaveReport.Name = "bSaveReport";
            this.bSaveReport.Size = new System.Drawing.Size(75, 23);
            this.bSaveReport.TabIndex = 5;
            this.bSaveReport.Text = "Save";
            this.bSaveReport.UseVisualStyleBackColor = true;
            this.bSaveReport.Click += new System.EventHandler(this.bSaveReport_Click);
            // 
            // dgvChecks
            // 
            this.dgvChecks.AllowUserToAddRows = false;
            this.dgvChecks.AllowUserToDeleteRows = false;
            this.dgvChecks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChecks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvChecks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvChecks.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvChecks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChecks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gvdcMAC,
            this.gvdcTestID,
            this.gvdcTestName,
            this.gvdcExpectedResult,
            this.gvdcCurrentResult,
            this.dvgcCheckResult});
            this.dgvChecks.Location = new System.Drawing.Point(3, 25);
            this.dgvChecks.MultiSelect = false;
            this.dgvChecks.Name = "dgvChecks";
            this.dgvChecks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChecks.Size = new System.Drawing.Size(563, 219);
            this.dgvChecks.TabIndex = 6;
            this.dgvChecks.SelectionChanged += new System.EventHandler(this.dgvChecks_SelectionChanged);
            // 
            // gvdcMAC
            // 
            this.gvdcMAC.HeaderText = "MAC";
            this.gvdcMAC.Name = "gvdcMAC";
            this.gvdcMAC.ReadOnly = true;
            this.gvdcMAC.Width = 55;
            // 
            // gvdcTestID
            // 
            this.gvdcTestID.HeaderText = "Test ID";
            this.gvdcTestID.Name = "gvdcTestID";
            this.gvdcTestID.ReadOnly = true;
            this.gvdcTestID.Width = 62;
            // 
            // gvdcTestName
            // 
            this.gvdcTestName.HeaderText = "TestName";
            this.gvdcTestName.Name = "gvdcTestName";
            this.gvdcTestName.ReadOnly = true;
            this.gvdcTestName.Width = 81;
            // 
            // gvdcExpectedResult
            // 
            this.gvdcExpectedResult.HeaderText = "Expected Result";
            this.gvdcExpectedResult.Name = "gvdcExpectedResult";
            this.gvdcExpectedResult.ReadOnly = true;
            this.gvdcExpectedResult.Width = 101;
            // 
            // gvdcCurrentResult
            // 
            this.gvdcCurrentResult.HeaderText = "Current Result";
            this.gvdcCurrentResult.Name = "gvdcCurrentResult";
            this.gvdcCurrentResult.ReadOnly = true;
            this.gvdcCurrentResult.Width = 91;
            // 
            // dvgcCheckResult
            // 
            this.dvgcCheckResult.HeaderText = "Check Result";
            this.dvgcCheckResult.Name = "dvgcCheckResult";
            this.dvgcCheckResult.ReadOnly = true;
            this.dvgcCheckResult.Width = 88;
            // 
            // dgvSteps
            // 
            this.dgvSteps.AllowUserToAddRows = false;
            this.dgvSteps.AllowUserToDeleteRows = false;
            this.dgvSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSteps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSteps.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSteps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSteps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcStepDetails,
            this.dvgcStepResult,
            this.dgvcStepDescription});
            this.dgvSteps.Location = new System.Drawing.Point(3, 25);
            this.dgvSteps.MultiSelect = false;
            this.dgvSteps.Name = "dgvSteps";
            this.dgvSteps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSteps.Size = new System.Drawing.Size(629, 222);
            this.dgvSteps.TabIndex = 9;
            // 
            // dgvcStepDetails
            // 
            this.dgvcStepDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcStepDetails.HeaderText = "Step Details";
            this.dgvcStepDetails.Name = "dgvcStepDetails";
            this.dgvcStepDetails.ReadOnly = true;
            this.dgvcStepDetails.Width = 82;
            // 
            // dvgcStepResult
            // 
            this.dvgcStepResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dvgcStepResult.HeaderText = "Step Result";
            this.dvgcStepResult.Name = "dvgcStepResult";
            this.dvgcStepResult.ReadOnly = true;
            this.dvgcStepResult.Width = 80;
            // 
            // dgvcStepDescription
            // 
            this.dgvcStepDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcStepDescription.HeaderText = "Step Description";
            this.dgvcStepDescription.Name = "dgvcStepDescription";
            this.dgvcStepDescription.ReadOnly = true;
            this.dgvcStepDescription.Width = 101;
            // 
            // lResults
            // 
            this.lResults.AutoSize = true;
            this.lResults.Location = new System.Drawing.Point(12, 41);
            this.lResults.Name = "lResults";
            this.lResults.Size = new System.Drawing.Size(45, 13);
            this.lResults.TabIndex = 10;
            this.lResults.Text = "Results:";
            // 
            // bLoadResults
            // 
            this.bLoadResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLoadResults.Location = new System.Drawing.Point(1064, 36);
            this.bLoadResults.Name = "bLoadResults";
            this.bLoadResults.Size = new System.Drawing.Size(156, 23);
            this.bLoadResults.TabIndex = 12;
            this.bLoadResults.Text = "Load Results";
            this.bLoadResults.UseVisualStyleBackColor = true;
            this.bLoadResults.Click += new System.EventHandler(this.bLoadResults_Click);
            // 
            // tbResults
            // 
            this.tbResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResults.Location = new System.Drawing.Point(60, 38);
            this.tbResults.Name = "tbResults";
            this.tbResults.ReadOnly = true;
            this.tbResults.Size = new System.Drawing.Size(998, 20);
            this.tbResults.TabIndex = 11;
            // 
            // scMain
            // 
            this.scMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scMain.Location = new System.Drawing.Point(12, 65);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.dgvReport);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.scSecond);
            this.scMain.Size = new System.Drawing.Size(1208, 501);
            this.scMain.SplitterDistance = 250;
            this.scMain.TabIndex = 13;
            // 
            // scSecond
            // 
            this.scSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSecond.Location = new System.Drawing.Point(0, 0);
            this.scSecond.Name = "scSecond";
            // 
            // scSecond.Panel1
            // 
            this.scSecond.Panel1.Controls.Add(this.label1);
            this.scSecond.Panel1.Controls.Add(this.dgvChecks);
            // 
            // scSecond.Panel2
            // 
            this.scSecond.Panel2.Controls.Add(this.label2);
            this.scSecond.Panel2.Controls.Add(this.dgvSteps);
            this.scSecond.Size = new System.Drawing.Size(1208, 247);
            this.scSecond.SplitterDistance = 569;
            this.scSecond.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Checks:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Step Details:";
            // 
            // dvgcID
            // 
            this.dvgcID.HeaderText = "ID";
            this.dvgcID.Name = "dvgcID";
            this.dvgcID.ReadOnly = true;
            this.dvgcID.Width = 43;
            // 
            // dgvcName
            // 
            this.dgvcName.HeaderText = "Name";
            this.dgvcName.Name = "dgvcName";
            this.dgvcName.ReadOnly = true;
            this.dgvcName.Width = 60;
            // 
            // dvgcDescription
            // 
            this.dvgcDescription.HeaderText = "Descrition";
            this.dvgcDescription.Name = "dvgcDescription";
            this.dvgcDescription.ReadOnly = true;
            this.dvgcDescription.Width = 79;
            // 
            // dvgcRelatedItems
            // 
            this.dvgcRelatedItems.HeaderText = "RelatedItems";
            this.dvgcRelatedItems.Name = "dvgcRelatedItems";
            this.dvgcRelatedItems.ReadOnly = true;
            this.dvgcRelatedItems.Width = 94;
            // 
            // dgccOldResult
            // 
            this.dgccOldResult.HeaderText = "Old Result";
            this.dgccOldResult.Name = "dgccOldResult";
            this.dgccOldResult.ReadOnly = true;
            this.dgccOldResult.Width = 81;
            // 
            // dvgcResult
            // 
            this.dvgcResult.HeaderText = "Result";
            this.dvgcResult.Name = "dvgcResult";
            this.dvgcResult.ReadOnly = true;
            this.dvgcResult.Width = 62;
            // 
            // dvgcRelatedBugs
            // 
            this.dvgcRelatedBugs.HeaderText = "Related Bugs";
            this.dvgcRelatedBugs.Name = "dvgcRelatedBugs";
            this.dvgcRelatedBugs.Width = 96;
            // 
            // dgvcRelatedTraces
            // 
            this.dgvcRelatedTraces.HeaderText = "Related Traces";
            this.dgvcRelatedTraces.Name = "dgvcRelatedTraces";
            this.dgvcRelatedTraces.ReadOnly = true;
            this.dgvcRelatedTraces.Width = 96;
            // 
            // dvgcRelatedFeatures
            // 
            this.dvgcRelatedFeatures.HeaderText = "Related Features";
            this.dvgcRelatedFeatures.Name = "dvgcRelatedFeatures";
            this.dvgcRelatedFeatures.ReadOnly = true;
            this.dvgcRelatedFeatures.Width = 104;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 578);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.bLoadResults);
            this.Controls.Add(this.tbResults);
            this.Controls.Add(this.lResults);
            this.Controls.Add(this.bSaveReport);
            this.Controls.Add(this.bOpenReport);
            this.Controls.Add(this.lReport);
            this.Controls.Add(this.tbReport);
            this.Name = "fMain";
            this.Text = "Report Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResizeEnd += new System.EventHandler(this.fMain_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).EndInit();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scSecond.Panel1.ResumeLayout(false);
            this.scSecond.Panel1.PerformLayout();
            this.scSecond.Panel2.ResumeLayout(false);
            this.scSecond.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSecond)).EndInit();
            this.scSecond.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.Label lReport;
        private System.Windows.Forms.Button bOpenReport;
        private System.Windows.Forms.OpenFileDialog ofdReport;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button bSaveReport;
        private System.Windows.Forms.DataGridView dgvChecks;
        private System.Windows.Forms.DataGridView dgvSteps;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcStepDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcStepResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcStepDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn gvdcMAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn gvdcTestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn gvdcTestName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gvdcExpectedResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn gvdcCurrentResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcCheckResult;
        private System.Windows.Forms.Label lResults;
        private System.Windows.Forms.Button bLoadResults;
        private System.Windows.Forms.TextBox tbResults;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.SplitContainer scSecond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcRelatedItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgccOldResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcRelatedBugs;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRelatedTraces;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgcRelatedFeatures;
    }
}

