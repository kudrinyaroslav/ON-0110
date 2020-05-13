namespace SMC.Pages
{
    partial class AccessRulesManagement
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tvAccessProfiles = new System.Windows.Forms.TreeView();
            this.gbAccessPolicies = new System.Windows.Forms.GroupBox();
            this.lvAccessPolicies = new System.Windows.Forms.ListView();
            this.chScheduleToken = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEntity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEntityType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEntityTypeNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbAccessProfileProperties = new System.Windows.Forms.GroupBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbAccessPolicies.SuspendLayout();
            this.gbAccessProfileProperties.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tvAccessProfiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbAccessPolicies);
            this.splitContainer1.Panel2.Controls.Add(this.gbAccessProfileProperties);
            this.splitContainer1.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 423);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tvAccessProfiles
            // 
            this.tvAccessProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvAccessProfiles.Location = new System.Drawing.Point(0, 0);
            this.tvAccessProfiles.Name = "tvAccessProfiles";
            this.tvAccessProfiles.Size = new System.Drawing.Size(257, 417);
            this.tvAccessProfiles.TabIndex = 0;
            // 
            // gbAccessPolicies
            // 
            this.gbAccessPolicies.Controls.Add(this.lvAccessPolicies);
            this.gbAccessPolicies.Location = new System.Drawing.Point(4, 80);
            this.gbAccessPolicies.Name = "gbAccessPolicies";
            this.gbAccessPolicies.Size = new System.Drawing.Size(505, 366);
            this.gbAccessPolicies.TabIndex = 29;
            this.gbAccessPolicies.TabStop = false;
            this.gbAccessPolicies.Text = "Access Policies";
            // 
            // lvAccessPolicies
            // 
            this.lvAccessPolicies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chScheduleToken,
            this.chEntity,
            this.chEntityType,
            this.chEntityTypeNamespace});
            this.lvAccessPolicies.GridLines = true;
            this.lvAccessPolicies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvAccessPolicies.Location = new System.Drawing.Point(6, 19);
            this.lvAccessPolicies.MultiSelect = false;
            this.lvAccessPolicies.Name = "lvAccessPolicies";
            this.lvAccessPolicies.Size = new System.Drawing.Size(487, 335);
            this.lvAccessPolicies.TabIndex = 0;
            this.lvAccessPolicies.UseCompatibleStateImageBehavior = false;
            this.lvAccessPolicies.View = System.Windows.Forms.View.Details;
            // 
            // chScheduleToken
            // 
            this.chScheduleToken.Text = "Schedule Token";
            this.chScheduleToken.Width = 100;
            // 
            // chEntity
            // 
            this.chEntity.Text = "Entity";
            this.chEntity.Width = 75;
            // 
            // chEntityType
            // 
            this.chEntityType.Text = "Entity Type";
            this.chEntityType.Width = 108;
            // 
            // chEntityTypeNamespace
            // 
            this.chEntityTypeNamespace.Text = "Entity Type Namespace";
            this.chEntityTypeNamespace.Width = 237;
            // 
            // gbAccessProfileProperties
            // 
            this.gbAccessProfileProperties.Controls.Add(this.tbName);
            this.gbAccessProfileProperties.Controls.Add(this.lblToken);
            this.gbAccessProfileProperties.Controls.Add(this.tbDescription);
            this.gbAccessProfileProperties.Controls.Add(this.tbToken);
            this.gbAccessProfileProperties.Controls.Add(this.label2);
            this.gbAccessProfileProperties.Controls.Add(this.label1);
            this.gbAccessProfileProperties.Location = new System.Drawing.Point(3, 3);
            this.gbAccessProfileProperties.Name = "gbAccessProfileProperties";
            this.gbAccessProfileProperties.Size = new System.Drawing.Size(507, 71);
            this.gbAccessProfileProperties.TabIndex = 28;
            this.gbAccessProfileProperties.TabStop = false;
            this.gbAccessProfileProperties.Text = "Access Profile Properties";
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
            this.tbDescription.Size = new System.Drawing.Size(398, 20);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // AccessRulesManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AccessRulesManagement";
            this.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbAccessPolicies.ResumeLayout(false);
            this.gbAccessProfileProperties.ResumeLayout(false);
            this.gbAccessProfileProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvAccessProfiles;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox gbAccessProfileProperties;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbAccessPolicies;
        private System.Windows.Forms.ListView lvAccessPolicies;
        private System.Windows.Forms.ColumnHeader chScheduleToken;
        private System.Windows.Forms.ColumnHeader chEntity;
        private System.Windows.Forms.ColumnHeader chEntityType;
        private System.Windows.Forms.ColumnHeader chEntityTypeNamespace;
    }
}
