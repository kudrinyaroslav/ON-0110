namespace SMC.Pages
{
    partial class CredentialManagement
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
            this.tvCredentials = new System.Windows.Forms.TreeView();
            this.gbCredentialAccessProfiles = new System.Windows.Forms.GroupBox();
            this.lvCredentialAccessProfiles = new System.Windows.Forms.ListView();
            this.chAccessProfileToken = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValidFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValidTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbCredentialIdentifiers = new System.Windows.Forms.GroupBox();
            this.lvCredentialIdentifiers = new System.Windows.Forms.ListView();
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chExemptedFromAuthentication = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbAccessProfileProperties = new System.Windows.Forms.GroupBox();
            this.lblValidTo = new System.Windows.Forms.Label();
            this.tbValidTo = new System.Windows.Forms.TextBox();
            this.lblValidFrom = new System.Windows.Forms.Label();
            this.tbValidFrom = new System.Windows.Forms.TextBox();
            this.tbCredentialHolderReference = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCredentialHolderReference = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbCredentialAccessProfiles.SuspendLayout();
            this.gbCredentialIdentifiers.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tvCredentials);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbCredentialAccessProfiles);
            this.splitContainer1.Panel2.Controls.Add(this.gbCredentialIdentifiers);
            this.splitContainer1.Panel2.Controls.Add(this.gbAccessProfileProperties);
            this.splitContainer1.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 1;
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
            // tvCredentials
            // 
            this.tvCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvCredentials.Location = new System.Drawing.Point(0, 0);
            this.tvCredentials.Name = "tvCredentials";
            this.tvCredentials.Size = new System.Drawing.Size(257, 417);
            this.tvCredentials.TabIndex = 0;
            // 
            // gbCredentialAccessProfiles
            // 
            this.gbCredentialAccessProfiles.Controls.Add(this.lvCredentialAccessProfiles);
            this.gbCredentialAccessProfiles.Location = new System.Drawing.Point(5, 296);
            this.gbCredentialAccessProfiles.Name = "gbCredentialAccessProfiles";
            this.gbCredentialAccessProfiles.Size = new System.Drawing.Size(505, 141);
            this.gbCredentialAccessProfiles.TabIndex = 30;
            this.gbCredentialAccessProfiles.TabStop = false;
            this.gbCredentialAccessProfiles.Text = "Credential Access Profiles";
            // 
            // lvCredentialAccessProfiles
            // 
            this.lvCredentialAccessProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAccessProfileToken,
            this.chValidFrom,
            this.chValidTo});
            this.lvCredentialAccessProfiles.GridLines = true;
            this.lvCredentialAccessProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCredentialAccessProfiles.Location = new System.Drawing.Point(10, 19);
            this.lvCredentialAccessProfiles.MultiSelect = false;
            this.lvCredentialAccessProfiles.Name = "lvCredentialAccessProfiles";
            this.lvCredentialAccessProfiles.Size = new System.Drawing.Size(487, 116);
            this.lvCredentialAccessProfiles.TabIndex = 0;
            this.lvCredentialAccessProfiles.UseCompatibleStateImageBehavior = false;
            this.lvCredentialAccessProfiles.View = System.Windows.Forms.View.Details;
            // 
            // chAccessProfileToken
            // 
            this.chAccessProfileToken.Text = "Access Profile Token";
            this.chAccessProfileToken.Width = 100;
            // 
            // chValidFrom
            // 
            this.chValidFrom.Text = "Valid From";
            this.chValidFrom.Width = 141;
            // 
            // chValidTo
            // 
            this.chValidTo.Text = "Valid To";
            this.chValidTo.Width = 161;
            // 
            // gbCredentialIdentifiers
            // 
            this.gbCredentialIdentifiers.Controls.Add(this.lvCredentialIdentifiers);
            this.gbCredentialIdentifiers.Location = new System.Drawing.Point(5, 149);
            this.gbCredentialIdentifiers.Name = "gbCredentialIdentifiers";
            this.gbCredentialIdentifiers.Size = new System.Drawing.Size(505, 141);
            this.gbCredentialIdentifiers.TabIndex = 29;
            this.gbCredentialIdentifiers.TabStop = false;
            this.gbCredentialIdentifiers.Text = "Credential Identifiers";
            // 
            // lvCredentialIdentifiers
            // 
            this.lvCredentialIdentifiers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chType,
            this.chExemptedFromAuthentication,
            this.chValue});
            this.lvCredentialIdentifiers.GridLines = true;
            this.lvCredentialIdentifiers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCredentialIdentifiers.Location = new System.Drawing.Point(10, 19);
            this.lvCredentialIdentifiers.MultiSelect = false;
            this.lvCredentialIdentifiers.Name = "lvCredentialIdentifiers";
            this.lvCredentialIdentifiers.Size = new System.Drawing.Size(487, 116);
            this.lvCredentialIdentifiers.TabIndex = 0;
            this.lvCredentialIdentifiers.UseCompatibleStateImageBehavior = false;
            this.lvCredentialIdentifiers.View = System.Windows.Forms.View.Details;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 100;
            // 
            // chExemptedFromAuthentication
            // 
            this.chExemptedFromAuthentication.Text = "Exempted from Authentication";
            this.chExemptedFromAuthentication.Width = 75;
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 296;
            // 
            // gbAccessProfileProperties
            // 
            this.gbAccessProfileProperties.Controls.Add(this.lblValidTo);
            this.gbAccessProfileProperties.Controls.Add(this.tbValidTo);
            this.gbAccessProfileProperties.Controls.Add(this.lblValidFrom);
            this.gbAccessProfileProperties.Controls.Add(this.tbValidFrom);
            this.gbAccessProfileProperties.Controls.Add(this.tbCredentialHolderReference);
            this.gbAccessProfileProperties.Controls.Add(this.lblToken);
            this.gbAccessProfileProperties.Controls.Add(this.tbDescription);
            this.gbAccessProfileProperties.Controls.Add(this.tbToken);
            this.gbAccessProfileProperties.Controls.Add(this.label2);
            this.gbAccessProfileProperties.Controls.Add(this.lblCredentialHolderReference);
            this.gbAccessProfileProperties.Location = new System.Drawing.Point(3, 3);
            this.gbAccessProfileProperties.Name = "gbAccessProfileProperties";
            this.gbAccessProfileProperties.Size = new System.Drawing.Size(507, 140);
            this.gbAccessProfileProperties.TabIndex = 28;
            this.gbAccessProfileProperties.TabStop = false;
            this.gbAccessProfileProperties.Text = "Access Profile Properties";
            // 
            // lblValidTo
            // 
            this.lblValidTo.AutoSize = true;
            this.lblValidTo.Location = new System.Drawing.Point(257, 114);
            this.lblValidTo.Name = "lblValidTo";
            this.lblValidTo.Size = new System.Drawing.Size(46, 13);
            this.lblValidTo.TabIndex = 8;
            this.lblValidTo.Text = "ValidTo:";
            // 
            // tbValidTo
            // 
            this.tbValidTo.Location = new System.Drawing.Point(319, 111);
            this.tbValidTo.Name = "tbValidTo";
            this.tbValidTo.ReadOnly = true;
            this.tbValidTo.Size = new System.Drawing.Size(175, 20);
            this.tbValidTo.TabIndex = 9;
            // 
            // lblValidFrom
            // 
            this.lblValidFrom.AutoSize = true;
            this.lblValidFrom.Location = new System.Drawing.Point(9, 114);
            this.lblValidFrom.Name = "lblValidFrom";
            this.lblValidFrom.Size = new System.Drawing.Size(56, 13);
            this.lblValidFrom.TabIndex = 6;
            this.lblValidFrom.Text = "ValidFrom:";
            // 
            // tbValidFrom
            // 
            this.tbValidFrom.Location = new System.Drawing.Point(71, 111);
            this.tbValidFrom.Name = "tbValidFrom";
            this.tbValidFrom.ReadOnly = true;
            this.tbValidFrom.Size = new System.Drawing.Size(175, 20);
            this.tbValidFrom.TabIndex = 7;
            // 
            // tbCredentialHolderReference
            // 
            this.tbCredentialHolderReference.Location = new System.Drawing.Point(12, 85);
            this.tbCredentialHolderReference.Name = "tbCredentialHolderReference";
            this.tbCredentialHolderReference.ReadOnly = true;
            this.tbCredentialHolderReference.Size = new System.Drawing.Size(482, 20);
            this.tbCredentialHolderReference.TabIndex = 3;
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
            this.tbDescription.Location = new System.Drawing.Point(96, 40);
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
            this.tbToken.Size = new System.Drawing.Size(398, 20);
            this.tbToken.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description:";
            // 
            // lblCredentialHolderReference
            // 
            this.lblCredentialHolderReference.AutoSize = true;
            this.lblCredentialHolderReference.Location = new System.Drawing.Point(9, 69);
            this.lblCredentialHolderReference.Name = "lblCredentialHolderReference";
            this.lblCredentialHolderReference.Size = new System.Drawing.Size(144, 13);
            this.lblCredentialHolderReference.TabIndex = 2;
            this.lblCredentialHolderReference.Text = "Credential Holder Reference:";
            // 
            // CredentialManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CredentialManagement";
            this.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbCredentialAccessProfiles.ResumeLayout(false);
            this.gbCredentialIdentifiers.ResumeLayout(false);
            this.gbAccessProfileProperties.ResumeLayout(false);
            this.gbAccessProfileProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TreeView tvCredentials;
        private System.Windows.Forms.GroupBox gbCredentialIdentifiers;
        private System.Windows.Forms.GroupBox gbAccessProfileProperties;
        private System.Windows.Forms.TextBox tbCredentialHolderReference;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCredentialHolderReference;
        private System.Windows.Forms.ListView lvCredentialIdentifiers;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chExemptedFromAuthentication;
        private System.Windows.Forms.ColumnHeader chValue;
        private System.Windows.Forms.Label lblValidTo;
        private System.Windows.Forms.TextBox tbValidTo;
        private System.Windows.Forms.Label lblValidFrom;
        private System.Windows.Forms.TextBox tbValidFrom;
        private System.Windows.Forms.GroupBox gbCredentialAccessProfiles;
        private System.Windows.Forms.ListView lvCredentialAccessProfiles;
        private System.Windows.Forms.ColumnHeader chAccessProfileToken;
        private System.Windows.Forms.ColumnHeader chValidFrom;
        private System.Windows.Forms.ColumnHeader chValidTo;

    }
}
