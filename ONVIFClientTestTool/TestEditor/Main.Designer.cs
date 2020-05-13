namespace TestEditor
{
    partial class Main
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
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.bOpenReport = new System.Windows.Forms.Button();
            this.lReport = new System.Windows.Forms.Label();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.ofdReport = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // pgMain
            // 
            this.pgMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgMain.HelpVisible = false;
            this.pgMain.Location = new System.Drawing.Point(12, 38);
            this.pgMain.Name = "pgMain";
            this.pgMain.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgMain.Size = new System.Drawing.Size(920, 510);
            this.pgMain.TabIndex = 0;
            // 
            // bOpenReport
            // 
            this.bOpenReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOpenReport.Location = new System.Drawing.Point(865, 10);
            this.bOpenReport.Name = "bOpenReport";
            this.bOpenReport.Size = new System.Drawing.Size(67, 23);
            this.bOpenReport.TabIndex = 5;
            this.bOpenReport.Text = "Open";
            this.bOpenReport.UseVisualStyleBackColor = true;
            this.bOpenReport.Click += new System.EventHandler(this.bOpenReport_Click);
            // 
            // lReport
            // 
            this.lReport.AutoSize = true;
            this.lReport.Location = new System.Drawing.Point(9, 15);
            this.lReport.Name = "lReport";
            this.lReport.Size = new System.Drawing.Size(42, 13);
            this.lReport.TabIndex = 4;
            this.lReport.Text = "Report:";
            // 
            // tbReport
            // 
            this.tbReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReport.Location = new System.Drawing.Point(57, 12);
            this.tbReport.Name = "tbReport";
            this.tbReport.ReadOnly = true;
            this.tbReport.Size = new System.Drawing.Size(802, 20);
            this.tbReport.TabIndex = 3;
            // 
            // ofdReport
            // 
            this.ofdReport.Filter = "XML files|*.xml";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 560);
            this.Controls.Add(this.bOpenReport);
            this.Controls.Add(this.lReport);
            this.Controls.Add(this.tbReport);
            this.Controls.Add(this.pgMain);
            this.Name = "Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgMain;
        private System.Windows.Forms.Button bOpenReport;
        private System.Windows.Forms.Label lReport;
        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.OpenFileDialog ofdReport;
    }
}

