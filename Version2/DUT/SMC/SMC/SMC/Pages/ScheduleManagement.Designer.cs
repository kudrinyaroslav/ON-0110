namespace SMC.Pages
{
    partial class ScheduleManagement
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
            this.lvSpecialDays = new System.Windows.Forms.ListView();
            this.chGroupToken = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tvSchedules = new System.Windows.Forms.TreeView();
            this.gbScheduleSpecialDays = new System.Windows.Forms.GroupBox();
            this.lvTimeRange = new System.Windows.Forms.ListView();
            this.chTimeRangeFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTimeRangeUntil = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbScheduleProperties = new System.Windows.Forms.GroupBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblStandard = new System.Windows.Forms.Label();
            this.tbStandard = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbScheduleSpecialDays.SuspendLayout();
            this.gbScheduleProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvSpecialDays
            // 
            this.lvSpecialDays.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chGroupToken});
            this.lvSpecialDays.GridLines = true;
            this.lvSpecialDays.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSpecialDays.Location = new System.Drawing.Point(6, 19);
            this.lvSpecialDays.MultiSelect = false;
            this.lvSpecialDays.Name = "lvSpecialDays";
            this.lvSpecialDays.Size = new System.Drawing.Size(190, 335);
            this.lvSpecialDays.TabIndex = 0;
            this.lvSpecialDays.UseCompatibleStateImageBehavior = false;
            this.lvSpecialDays.View = System.Windows.Forms.View.Details;
            this.lvSpecialDays.SelectedIndexChanged += new System.EventHandler(this.lvSpecialDays_SelectedIndexChanged);
            // 
            // chGroupToken
            // 
            this.chGroupToken.Text = "Group Token";
            this.chGroupToken.Width = 221;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.tvSchedules);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbScheduleSpecialDays);
            this.splitContainer1.Panel2.Controls.Add(this.gbScheduleProperties);
            this.splitContainer1.Size = new System.Drawing.Size(867, 464);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 438);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tvSchedules
            // 
            this.tvSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSchedules.Location = new System.Drawing.Point(0, 0);
            this.tvSchedules.Name = "tvSchedules";
            this.tvSchedules.Size = new System.Drawing.Size(287, 432);
            this.tvSchedules.TabIndex = 0;
            // 
            // gbScheduleSpecialDays
            // 
            this.gbScheduleSpecialDays.Controls.Add(this.lvTimeRange);
            this.gbScheduleSpecialDays.Controls.Add(this.lvSpecialDays);
            this.gbScheduleSpecialDays.Location = new System.Drawing.Point(4, 80);
            this.gbScheduleSpecialDays.Name = "gbScheduleSpecialDays";
            this.gbScheduleSpecialDays.Size = new System.Drawing.Size(506, 366);
            this.gbScheduleSpecialDays.TabIndex = 29;
            this.gbScheduleSpecialDays.TabStop = false;
            this.gbScheduleSpecialDays.Text = "Schedule Special Days";
            // 
            // lvTimeRange
            // 
            this.lvTimeRange.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTimeRangeFrom,
            this.chTimeRangeUntil});
            this.lvTimeRange.GridLines = true;
            this.lvTimeRange.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTimeRange.Location = new System.Drawing.Point(202, 19);
            this.lvTimeRange.MultiSelect = false;
            this.lvTimeRange.Name = "lvTimeRange";
            this.lvTimeRange.Size = new System.Drawing.Size(291, 335);
            this.lvTimeRange.TabIndex = 1;
            this.lvTimeRange.UseCompatibleStateImageBehavior = false;
            this.lvTimeRange.View = System.Windows.Forms.View.Details;
            // 
            // chTimeRangeFrom
            // 
            this.chTimeRangeFrom.Text = "From";
            this.chTimeRangeFrom.Width = 144;
            // 
            // chTimeRangeUntil
            // 
            this.chTimeRangeUntil.Text = "Until";
            this.chTimeRangeUntil.Width = 305;
            // 
            // gbScheduleProperties
            // 
            this.gbScheduleProperties.Controls.Add(this.tbStandard);
            this.gbScheduleProperties.Controls.Add(this.lblStandard);
            this.gbScheduleProperties.Controls.Add(this.tbName);
            this.gbScheduleProperties.Controls.Add(this.lblToken);
            this.gbScheduleProperties.Controls.Add(this.tbDescription);
            this.gbScheduleProperties.Controls.Add(this.tbToken);
            this.gbScheduleProperties.Controls.Add(this.lblDescription);
            this.gbScheduleProperties.Controls.Add(this.lblName);
            this.gbScheduleProperties.Location = new System.Drawing.Point(3, 3);
            this.gbScheduleProperties.Name = "gbScheduleProperties";
            this.gbScheduleProperties.Size = new System.Drawing.Size(507, 71);
            this.gbScheduleProperties.TabIndex = 28;
            this.gbScheduleProperties.TabStop = false;
            this.gbScheduleProperties.Text = "Schedule Properties";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(351, 16);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(143, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(9, 19);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(41, 13);
            this.lblToken.TabIndex = 0;
            this.lblToken.Text = "Token:";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(96, 43);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.Size = new System.Drawing.Size(143, 20);
            this.tbDescription.TabIndex = 5;
            // 
            // tbToken
            // 
            this.tbToken.Location = new System.Drawing.Point(96, 16);
            this.tbToken.Name = "tbToken";
            this.tbToken.ReadOnly = true;
            this.tbToken.Size = new System.Drawing.Size(143, 20);
            this.tbToken.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(9, 46);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(285, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            // 
            // lblStandard
            // 
            this.lblStandard.AutoSize = true;
            this.lblStandard.Location = new System.Drawing.Point(285, 46);
            this.lblStandard.Name = "lblStandard";
            this.lblStandard.Size = new System.Drawing.Size(53, 13);
            this.lblStandard.TabIndex = 6;
            this.lblStandard.Text = "Standard:";
            // 
            // tbStandard
            // 
            this.tbStandard.Location = new System.Drawing.Point(351, 43);
            this.tbStandard.Name = "tbStandard";
            this.tbStandard.ReadOnly = true;
            this.tbStandard.Size = new System.Drawing.Size(143, 20);
            this.tbStandard.TabIndex = 7;
            // 
            // ScheduleManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScheduleManagement";
            this.Size = new System.Drawing.Size(867, 464);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbScheduleSpecialDays.ResumeLayout(false);
            this.gbScheduleProperties.ResumeLayout(false);
            this.gbScheduleProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvSpecialDays;
        private System.Windows.Forms.ColumnHeader chGroupToken;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TreeView tvSchedules;
        private System.Windows.Forms.GroupBox gbScheduleSpecialDays;
        private System.Windows.Forms.GroupBox gbScheduleProperties;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ListView lvTimeRange;
        private System.Windows.Forms.ColumnHeader chTimeRangeFrom;
        private System.Windows.Forms.ColumnHeader chTimeRangeUntil;
        private System.Windows.Forms.TextBox tbStandard;
        private System.Windows.Forms.Label lblStandard;


    }
}
