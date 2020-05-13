namespace TestTool.GUI
{
    partial class CompactProcessingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompactProcessingForm));
            this.lblFeatureDefinitionStatus = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.lblFailed = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblCompletedCount = new System.Windows.Forms.Label();
            this.lblFailedCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFeatureDefinitionStatus
            // 
            this.lblFeatureDefinitionStatus.AutoSize = true;
            this.lblFeatureDefinitionStatus.Location = new System.Drawing.Point(13, 13);
            this.lblFeatureDefinitionStatus.Name = "lblFeatureDefinitionStatus";
            this.lblFeatureDefinitionStatus.Size = new System.Drawing.Size(176, 13);
            this.lblFeatureDefinitionStatus.TabIndex = 0;
            this.lblFeatureDefinitionStatus.Text = "Feature definition process is running";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(12, 36);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(34, 13);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total:";
            // 
            // lblCompleted
            // 
            this.lblCompleted.AutoSize = true;
            this.lblCompleted.Location = new System.Drawing.Point(13, 58);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(60, 13);
            this.lblCompleted.TabIndex = 2;
            this.lblCompleted.Text = "Completed:";
            // 
            // lblFailed
            // 
            this.lblFailed.AutoSize = true;
            this.lblFailed.Location = new System.Drawing.Point(13, 82);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(38, 13);
            this.lblFailed.TabIndex = 3;
            this.lblFailed.Text = "Failed:";
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(71, 36);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(0, 13);
            this.lblTotalCount.TabIndex = 4;
            // 
            // lblCompletedCount
            // 
            this.lblCompletedCount.AutoSize = true;
            this.lblCompletedCount.Location = new System.Drawing.Point(80, 58);
            this.lblCompletedCount.Name = "lblCompletedCount";
            this.lblCompletedCount.Size = new System.Drawing.Size(0, 13);
            this.lblCompletedCount.TabIndex = 5;
            // 
            // lblFailedCount
            // 
            this.lblFailedCount.AutoSize = true;
            this.lblFailedCount.Location = new System.Drawing.Point(71, 82);
            this.lblFailedCount.Name = "lblFailedCount";
            this.lblFailedCount.Size = new System.Drawing.Size(0, 13);
            this.lblFailedCount.TabIndex = 6;
            // 
            // CompactProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 171);
            this.Controls.Add(this.lblFailedCount);
            this.Controls.Add(this.lblCompletedCount);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.lblFailed);
            this.Controls.Add(this.lblCompleted);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblFeatureDefinitionStatus);
            this.MaximizeBox = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompactProcessingForm";
            this.Text = "TestTool - compact version";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFeatureDefinitionStatus;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblCompletedCount;
        private System.Windows.Forms.Label lblFailedCount;
    }
}