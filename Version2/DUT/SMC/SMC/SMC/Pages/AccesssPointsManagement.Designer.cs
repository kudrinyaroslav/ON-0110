namespace SMC.Pages
{
    partial class AccessPointsManagement
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
            this.tvAccessPoints = new System.Windows.Forms.TreeView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gbAccessPointOperations = new System.Windows.Forms.GroupBox();
            this.gbAccessPointProperties = new System.Windows.Forms.GroupBox();
            this.tbEntity = new System.Windows.Forms.TextBox();
            this.lblEntity = new System.Windows.Forms.Label();
            this.tbEntityType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAreaTo = new System.Windows.Forms.TextBox();
            this.labelAreaFrom = new System.Windows.Forms.Label();
            this.tbAreaFrom = new System.Windows.Forms.TextBox();
            this.labelAreaTo = new System.Windows.Forms.Label();
            this.gbCapabilities = new System.Windows.Forms.GroupBox();
            this.chkDisable = new SMC.Controls.ReadOnlyCheckbox();
            this.chkDuress = new SMC.Controls.ReadOnlyCheckbox();
            this.chkAnonymousAccess = new SMC.Controls.ReadOnlyCheckbox();
            this.chkTamper = new SMC.Controls.ReadOnlyCheckbox();
            this.chkAccessTaken = new SMC.Controls.ReadOnlyCheckbox();
            this.chkExternal = new SMC.Controls.ReadOnlyCheckbox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbAccessPointOperations.SuspendLayout();
            this.gbAccessPointProperties.SuspendLayout();
            this.gbCapabilities.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tvAccessPoints);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbAccessPointOperations);
            this.splitContainer1.Panel2.Controls.Add(this.gbAccessPointProperties);
            this.splitContainer1.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvAccessPoints
            // 
            this.tvAccessPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvAccessPoints.Location = new System.Drawing.Point(0, 0);
            this.tvAccessPoints.Name = "tvAccessPoints";
            this.tvAccessPoints.Size = new System.Drawing.Size(257, 417);
            this.tvAccessPoints.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 423);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gbAccessPointOperations
            // 
            this.gbAccessPointOperations.Controls.Add(this.btnEnable);
            this.gbAccessPointOperations.Controls.Add(this.btnDisable);
            this.gbAccessPointOperations.Location = new System.Drawing.Point(5, 216);
            this.gbAccessPointOperations.Name = "gbAccessPointOperations";
            this.gbAccessPointOperations.Size = new System.Drawing.Size(505, 186);
            this.gbAccessPointOperations.TabIndex = 28;
            this.gbAccessPointOperations.TabStop = false;
            this.gbAccessPointOperations.Text = "Access Point Operations";
            // 
            // gbAccessPointProperties
            // 
            this.gbAccessPointProperties.Controls.Add(this.tbEntity);
            this.gbAccessPointProperties.Controls.Add(this.lblEntity);
            this.gbAccessPointProperties.Controls.Add(this.tbEntityType);
            this.gbAccessPointProperties.Controls.Add(this.label4);
            this.gbAccessPointProperties.Controls.Add(this.tbAreaTo);
            this.gbAccessPointProperties.Controls.Add(this.labelAreaFrom);
            this.gbAccessPointProperties.Controls.Add(this.tbAreaFrom);
            this.gbAccessPointProperties.Controls.Add(this.labelAreaTo);
            this.gbAccessPointProperties.Controls.Add(this.gbCapabilities);
            this.gbAccessPointProperties.Controls.Add(this.tbName);
            this.gbAccessPointProperties.Controls.Add(this.lblToken);
            this.gbAccessPointProperties.Controls.Add(this.tbDescription);
            this.gbAccessPointProperties.Controls.Add(this.tbToken);
            this.gbAccessPointProperties.Controls.Add(this.label2);
            this.gbAccessPointProperties.Controls.Add(this.label1);
            this.gbAccessPointProperties.Location = new System.Drawing.Point(3, 11);
            this.gbAccessPointProperties.Name = "gbAccessPointProperties";
            this.gbAccessPointProperties.Size = new System.Drawing.Size(507, 199);
            this.gbAccessPointProperties.TabIndex = 27;
            this.gbAccessPointProperties.TabStop = false;
            this.gbAccessPointProperties.Text = "Access Point Properties";
            // 
            // tbEntity
            // 
            this.tbEntity.Location = new System.Drawing.Point(351, 95);
            this.tbEntity.Name = "tbEntity";
            this.tbEntity.ReadOnly = true;
            this.tbEntity.Size = new System.Drawing.Size(143, 20);
            this.tbEntity.TabIndex = 23;
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = true;
            this.lblEntity.Location = new System.Drawing.Point(9, 98);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(60, 13);
            this.lblEntity.TabIndex = 20;
            this.lblEntity.Text = "EntityType:";
            // 
            // tbEntityType
            // 
            this.tbEntityType.Location = new System.Drawing.Point(96, 95);
            this.tbEntityType.Name = "tbEntityType";
            this.tbEntityType.ReadOnly = true;
            this.tbEntityType.Size = new System.Drawing.Size(143, 20);
            this.tbEntityType.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(285, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Entity:";
            // 
            // tbAreaTo
            // 
            this.tbAreaTo.Location = new System.Drawing.Point(351, 69);
            this.tbAreaTo.Name = "tbAreaTo";
            this.tbAreaTo.ReadOnly = true;
            this.tbAreaTo.Size = new System.Drawing.Size(143, 20);
            this.tbAreaTo.TabIndex = 19;
            // 
            // labelAreaFrom
            // 
            this.labelAreaFrom.AutoSize = true;
            this.labelAreaFrom.Location = new System.Drawing.Point(9, 72);
            this.labelAreaFrom.Name = "labelAreaFrom";
            this.labelAreaFrom.Size = new System.Drawing.Size(55, 13);
            this.labelAreaFrom.TabIndex = 16;
            this.labelAreaFrom.Text = "AreaFrom:";
            // 
            // tbAreaFrom
            // 
            this.tbAreaFrom.Location = new System.Drawing.Point(96, 69);
            this.tbAreaFrom.Name = "tbAreaFrom";
            this.tbAreaFrom.ReadOnly = true;
            this.tbAreaFrom.Size = new System.Drawing.Size(143, 20);
            this.tbAreaFrom.TabIndex = 17;
            // 
            // labelAreaTo
            // 
            this.labelAreaTo.AutoSize = true;
            this.labelAreaTo.Location = new System.Drawing.Point(285, 72);
            this.labelAreaTo.Name = "labelAreaTo";
            this.labelAreaTo.Size = new System.Drawing.Size(45, 13);
            this.labelAreaTo.TabIndex = 18;
            this.labelAreaTo.Text = "AreaTo:";
            // 
            // gbCapabilities
            // 
            this.gbCapabilities.Controls.Add(this.chkDisable);
            this.gbCapabilities.Controls.Add(this.chkDuress);
            this.gbCapabilities.Controls.Add(this.chkAnonymousAccess);
            this.gbCapabilities.Controls.Add(this.chkTamper);
            this.gbCapabilities.Controls.Add(this.chkAccessTaken);
            this.gbCapabilities.Controls.Add(this.chkExternal);
            this.gbCapabilities.Location = new System.Drawing.Point(12, 120);
            this.gbCapabilities.Name = "gbCapabilities";
            this.gbCapabilities.Size = new System.Drawing.Size(484, 73);
            this.gbCapabilities.TabIndex = 15;
            this.gbCapabilities.TabStop = false;
            this.gbCapabilities.Text = "Capabilities";
            // 
            // chkDisable
            // 
            this.chkDisable.AutoSize = true;
            this.chkDisable.Location = new System.Drawing.Point(6, 19);
            this.chkDisable.Name = "chkDisable";
            this.chkDisable.Size = new System.Drawing.Size(61, 17);
            this.chkDisable.TabIndex = 1;
            this.chkDisable.Text = "Disable";
            this.chkDisable.UseVisualStyleBackColor = true;
            // 
            // chkDuress
            // 
            this.chkDuress.AutoSize = true;
            this.chkDuress.Location = new System.Drawing.Point(103, 19);
            this.chkDuress.Name = "chkDuress";
            this.chkDuress.Size = new System.Drawing.Size(59, 17);
            this.chkDuress.TabIndex = 2;
            this.chkDuress.Text = "Duress";
            this.chkDuress.UseVisualStyleBackColor = true;
            // 
            // chkAnonymousAccess
            // 
            this.chkAnonymousAccess.AutoSize = true;
            this.chkAnonymousAccess.Location = new System.Drawing.Point(210, 42);
            this.chkAnonymousAccess.Name = "chkAnonymousAccess";
            this.chkAnonymousAccess.Size = new System.Drawing.Size(119, 17);
            this.chkAnonymousAccess.TabIndex = 7;
            this.chkAnonymousAccess.Text = "Anonymous Access";
            this.chkAnonymousAccess.UseVisualStyleBackColor = true;
            // 
            // chkTamper
            // 
            this.chkTamper.AutoSize = true;
            this.chkTamper.Location = new System.Drawing.Point(103, 42);
            this.chkTamper.Name = "chkTamper";
            this.chkTamper.Size = new System.Drawing.Size(62, 17);
            this.chkTamper.TabIndex = 6;
            this.chkTamper.Text = "Tamper";
            this.chkTamper.UseVisualStyleBackColor = true;
            // 
            // chkAccessTaken
            // 
            this.chkAccessTaken.AutoSize = true;
            this.chkAccessTaken.Location = new System.Drawing.Point(7, 42);
            this.chkAccessTaken.Name = "chkAccessTaken";
            this.chkAccessTaken.Size = new System.Drawing.Size(95, 17);
            this.chkAccessTaken.TabIndex = 3;
            this.chkAccessTaken.Text = "Access Taken";
            this.chkAccessTaken.UseVisualStyleBackColor = true;
            // 
            // chkExternal
            // 
            this.chkExternal.AutoSize = true;
            this.chkExternal.Location = new System.Drawing.Point(210, 19);
            this.chkExternal.Name = "chkExternal";
            this.chkExternal.Size = new System.Drawing.Size(128, 17);
            this.chkExternal.TabIndex = 5;
            this.chkExternal.Text = "External Authorization";
            this.chkExternal.UseVisualStyleBackColor = true;
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
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(16, 20);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(75, 23);
            this.btnDisable.TabIndex = 0;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(98, 20);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(75, 23);
            this.btnEnable.TabIndex = 1;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // AccessPointsManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AccessPointsManagement";
            this.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gbAccessPointOperations.ResumeLayout(false);
            this.gbAccessPointProperties.ResumeLayout(false);
            this.gbAccessPointProperties.PerformLayout();
            this.gbCapabilities.ResumeLayout(false);
            this.gbCapabilities.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox gbAccessPointOperations;
        private System.Windows.Forms.GroupBox gbAccessPointProperties;
        private System.Windows.Forms.GroupBox gbCapabilities;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tvAccessPoints;
        private SMC.Controls.ReadOnlyCheckbox chkDisable;
        private SMC.Controls.ReadOnlyCheckbox chkDuress;
        private SMC.Controls.ReadOnlyCheckbox chkAnonymousAccess;
        private SMC.Controls.ReadOnlyCheckbox chkTamper;
        private SMC.Controls.ReadOnlyCheckbox chkAccessTaken;
        private SMC.Controls.ReadOnlyCheckbox chkExternal;
        private System.Windows.Forms.TextBox tbEntity;
        private System.Windows.Forms.Label lblEntity;
        private System.Windows.Forms.TextBox tbEntityType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAreaTo;
        private System.Windows.Forms.Label labelAreaFrom;
        private System.Windows.Forms.TextBox tbAreaFrom;
        private System.Windows.Forms.Label labelAreaTo;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnEnable;

    }
}
