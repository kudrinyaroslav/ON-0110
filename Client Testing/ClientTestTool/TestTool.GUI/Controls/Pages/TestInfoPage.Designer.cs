namespace TestTool.GUI.Controls.Pages
{
    partial class TestInfoPage
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
            this.gbTestExecutionInformation = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbOrganizationAddress = new System.Windows.Forms.TextBox();
            this.lblOrganizationAddress = new System.Windows.Forms.Label();
            this.lblOrganizationName = new System.Windows.Forms.Label();
            this.tbOrganizationName = new System.Windows.Forms.TextBox();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.tbOperatorName = new System.Windows.Forms.TextBox();
            this.gbDutInformation = new System.Windows.Forms.GroupBox();
            this.tbProductName = new System.Windows.Forms.TextBox();
            this.lblproductName = new System.Windows.Forms.Label();
            this.btnClearDeviceInformation = new System.Windows.Forms.Button();
            this.lblOtherInformation = new System.Windows.Forms.Label();
            this.tbOtherInformation = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.gbResponsible = new System.Windows.Forms.GroupBox();
            this.btnClearMemberInfo = new System.Windows.Forms.Button();
            this.tbMemberAddress = new System.Windows.Forms.TextBox();
            this.tbMemberName = new System.Windows.Forms.TextBox();
            this.lblMemberAddress = new System.Windows.Forms.Label();
            this.lblMemberName = new System.Windows.Forms.Label();
            this.gbTestExecutionInformation.SuspendLayout();
            this.gbDutInformation.SuspendLayout();
            this.gbResponsible.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTestExecutionInformation
            // 
            this.gbTestExecutionInformation.Controls.Add(this.btnClear);
            this.gbTestExecutionInformation.Controls.Add(this.tbOrganizationAddress);
            this.gbTestExecutionInformation.Controls.Add(this.lblOrganizationAddress);
            this.gbTestExecutionInformation.Controls.Add(this.lblOrganizationName);
            this.gbTestExecutionInformation.Controls.Add(this.tbOrganizationName);
            this.gbTestExecutionInformation.Controls.Add(this.lblOperatorName);
            this.gbTestExecutionInformation.Controls.Add(this.tbOperatorName);
            this.gbTestExecutionInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTestExecutionInformation.Location = new System.Drawing.Point(0, 203);
            this.gbTestExecutionInformation.Name = "gbTestExecutionInformation";
            this.gbTestExecutionInformation.Size = new System.Drawing.Size(774, 143);
            this.gbTestExecutionInformation.TabIndex = 23;
            this.gbTestExecutionInformation.TabStop = false;
            this.gbTestExecutionInformation.Text = "Test Execution Information";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(693, 114);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbOrganizationAddress
            // 
            this.tbOrganizationAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrganizationAddress.Location = new System.Drawing.Point(166, 71);
            this.tbOrganizationAddress.Multiline = true;
            this.tbOrganizationAddress.Name = "tbOrganizationAddress";
            this.tbOrganizationAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOrganizationAddress.Size = new System.Drawing.Size(521, 66);
            this.tbOrganizationAddress.TabIndex = 23;
            // 
            // lblOrganizationAddress
            // 
            this.lblOrganizationAddress.AutoSize = true;
            this.lblOrganizationAddress.Location = new System.Drawing.Point(6, 74);
            this.lblOrganizationAddress.Name = "lblOrganizationAddress";
            this.lblOrganizationAddress.Size = new System.Drawing.Size(160, 13);
            this.lblOrganizationAddress.TabIndex = 11;
            this.lblOrganizationAddress.Text = "Executing Organization Address:";
            // 
            // lblOrganizationName
            // 
            this.lblOrganizationName.AutoSize = true;
            this.lblOrganizationName.Location = new System.Drawing.Point(6, 48);
            this.lblOrganizationName.Name = "lblOrganizationName";
            this.lblOrganizationName.Size = new System.Drawing.Size(150, 13);
            this.lblOrganizationName.TabIndex = 9;
            this.lblOrganizationName.Text = "Executing Organization Name:";
            // 
            // tbOrganizationName
            // 
            this.tbOrganizationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrganizationName.Location = new System.Drawing.Point(166, 45);
            this.tbOrganizationName.Name = "tbOrganizationName";
            this.tbOrganizationName.Size = new System.Drawing.Size(521, 20);
            this.tbOrganizationName.TabIndex = 22;
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.AutoSize = true;
            this.lblOperatorName.Location = new System.Drawing.Point(6, 22);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(106, 13);
            this.lblOperatorName.TabIndex = 7;
            this.lblOperatorName.Text = "Test Operator Name:";
            // 
            // tbOperatorName
            // 
            this.tbOperatorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOperatorName.Location = new System.Drawing.Point(166, 19);
            this.tbOperatorName.Name = "tbOperatorName";
            this.tbOperatorName.Size = new System.Drawing.Size(521, 20);
            this.tbOperatorName.TabIndex = 21;
            // 
            // gbDutInformation
            // 
            this.gbDutInformation.Controls.Add(this.tbProductName);
            this.gbDutInformation.Controls.Add(this.lblproductName);
            this.gbDutInformation.Controls.Add(this.btnClearDeviceInformation);
            this.gbDutInformation.Controls.Add(this.lblOtherInformation);
            this.gbDutInformation.Controls.Add(this.tbOtherInformation);
            this.gbDutInformation.Controls.Add(this.lblModel);
            this.gbDutInformation.Controls.Add(this.tbModel);
            this.gbDutInformation.Controls.Add(this.lblBrand);
            this.gbDutInformation.Controls.Add(this.tbBrand);
            this.gbDutInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDutInformation.Location = new System.Drawing.Point(0, 77);
            this.gbDutInformation.Name = "gbDutInformation";
            this.gbDutInformation.Size = new System.Drawing.Size(774, 126);
            this.gbDutInformation.TabIndex = 22;
            this.gbDutInformation.TabStop = false;
            this.gbDutInformation.Text = "Product Under Test Information";
            // 
            // tbProductName
            // 
            this.tbProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProductName.Location = new System.Drawing.Point(166, 19);
            this.tbProductName.Name = "tbProductName";
            this.tbProductName.Size = new System.Drawing.Size(521, 20);
            this.tbProductName.TabIndex = 11;
            // 
            // lblproductName
            // 
            this.lblproductName.AutoSize = true;
            this.lblproductName.Location = new System.Drawing.Point(6, 22);
            this.lblproductName.Name = "lblproductName";
            this.lblproductName.Size = new System.Drawing.Size(78, 13);
            this.lblproductName.TabIndex = 18;
            this.lblproductName.Text = "Product Name:";
            // 
            // btnClearDeviceInformation
            // 
            this.btnClearDeviceInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDeviceInformation.Location = new System.Drawing.Point(693, 94);
            this.btnClearDeviceInformation.Name = "btnClearDeviceInformation";
            this.btnClearDeviceInformation.Size = new System.Drawing.Size(75, 23);
            this.btnClearDeviceInformation.TabIndex = 17;
            this.btnClearDeviceInformation.Text = "Clear";
            this.btnClearDeviceInformation.UseVisualStyleBackColor = true;
            this.btnClearDeviceInformation.Click += new System.EventHandler(this.btnClearDeviceInformation_Click);
            // 
            // lblOtherInformation
            // 
            this.lblOtherInformation.AutoSize = true;
            this.lblOtherInformation.Location = new System.Drawing.Point(7, 100);
            this.lblOtherInformation.Name = "lblOtherInformation";
            this.lblOtherInformation.Size = new System.Drawing.Size(91, 13);
            this.lblOtherInformation.TabIndex = 15;
            this.lblOtherInformation.Text = "Other Information:";
            // 
            // tbOtherInformation
            // 
            this.tbOtherInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOtherInformation.Location = new System.Drawing.Point(166, 97);
            this.tbOtherInformation.Name = "tbOtherInformation";
            this.tbOtherInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOtherInformation.Size = new System.Drawing.Size(521, 20);
            this.tbOtherInformation.TabIndex = 14;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(6, 74);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 9;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModel.Location = new System.Drawing.Point(166, 71);
            this.tbModel.Name = "tbModel";
            this.tbModel.ReadOnly = true;
            this.tbModel.Size = new System.Drawing.Size(521, 20);
            this.tbModel.TabIndex = 13;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(7, 48);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 7;
            this.lblBrand.Text = "Brand:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBrand.Location = new System.Drawing.Point(166, 45);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.ReadOnly = true;
            this.tbBrand.Size = new System.Drawing.Size(521, 20);
            this.tbBrand.TabIndex = 12;
            // 
            // gbResponsible
            // 
            this.gbResponsible.Controls.Add(this.btnClearMemberInfo);
            this.gbResponsible.Controls.Add(this.tbMemberAddress);
            this.gbResponsible.Controls.Add(this.tbMemberName);
            this.gbResponsible.Controls.Add(this.lblMemberAddress);
            this.gbResponsible.Controls.Add(this.lblMemberName);
            this.gbResponsible.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbResponsible.Location = new System.Drawing.Point(0, 0);
            this.gbResponsible.Name = "gbResponsible";
            this.gbResponsible.Size = new System.Drawing.Size(774, 77);
            this.gbResponsible.TabIndex = 21;
            this.gbResponsible.TabStop = false;
            this.gbResponsible.Text = "Responsible Member";
            // 
            // btnClearMemberInfo
            // 
            this.btnClearMemberInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearMemberInfo.Location = new System.Drawing.Point(693, 45);
            this.btnClearMemberInfo.Name = "btnClearMemberInfo";
            this.btnClearMemberInfo.Size = new System.Drawing.Size(75, 23);
            this.btnClearMemberInfo.TabIndex = 4;
            this.btnClearMemberInfo.Text = "Clear";
            this.btnClearMemberInfo.UseVisualStyleBackColor = true;
            this.btnClearMemberInfo.Click += new System.EventHandler(this.btnClearMemberInfo_Click);
            // 
            // tbMemberAddress
            // 
            this.tbMemberAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberAddress.Location = new System.Drawing.Point(166, 47);
            this.tbMemberAddress.Name = "tbMemberAddress";
            this.tbMemberAddress.Size = new System.Drawing.Size(521, 20);
            this.tbMemberAddress.TabIndex = 3;
            // 
            // tbMemberName
            // 
            this.tbMemberName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberName.Location = new System.Drawing.Point(166, 20);
            this.tbMemberName.Name = "tbMemberName";
            this.tbMemberName.Size = new System.Drawing.Size(521, 20);
            this.tbMemberName.TabIndex = 2;
            // 
            // lblMemberAddress
            // 
            this.lblMemberAddress.AutoSize = true;
            this.lblMemberAddress.Location = new System.Drawing.Point(9, 48);
            this.lblMemberAddress.Name = "lblMemberAddress";
            this.lblMemberAddress.Size = new System.Drawing.Size(89, 13);
            this.lblMemberAddress.TabIndex = 1;
            this.lblMemberAddress.Text = "Member Address:";
            // 
            // lblMemberName
            // 
            this.lblMemberName.AutoSize = true;
            this.lblMemberName.Location = new System.Drawing.Point(9, 23);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(79, 13);
            this.lblMemberName.TabIndex = 0;
            this.lblMemberName.Text = "Member Name:";
            // 
            // TestInfoPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTestExecutionInformation);
            this.Controls.Add(this.gbDutInformation);
            this.Controls.Add(this.gbResponsible);
            this.Name = "TestInfoPage";
            this.Size = new System.Drawing.Size(774, 486);
            this.gbTestExecutionInformation.ResumeLayout(false);
            this.gbTestExecutionInformation.PerformLayout();
            this.gbDutInformation.ResumeLayout(false);
            this.gbDutInformation.PerformLayout();
            this.gbResponsible.ResumeLayout(false);
            this.gbResponsible.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTestExecutionInformation;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbOrganizationAddress;
        private System.Windows.Forms.Label lblOrganizationAddress;
        private System.Windows.Forms.Label lblOrganizationName;
        private System.Windows.Forms.TextBox tbOrganizationName;
        private System.Windows.Forms.Label lblOperatorName;
        private System.Windows.Forms.TextBox tbOperatorName;
        private System.Windows.Forms.GroupBox gbDutInformation;
        private System.Windows.Forms.TextBox tbProductName;
        private System.Windows.Forms.Label lblproductName;
        private System.Windows.Forms.Button btnClearDeviceInformation;
        private System.Windows.Forms.Label lblOtherInformation;
        private System.Windows.Forms.TextBox tbOtherInformation;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox tbModel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox tbBrand;
        private System.Windows.Forms.GroupBox gbResponsible;
        private System.Windows.Forms.Button btnClearMemberInfo;
        private System.Windows.Forms.TextBox tbMemberAddress;
        private System.Windows.Forms.TextBox tbMemberName;
        private System.Windows.Forms.Label lblMemberAddress;
        private System.Windows.Forms.Label lblMemberName;
    }
}
