namespace TestTool.GUI.Controls
{
    partial class SetupPage
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
            this.gbTestExecutionInformation = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbOrganizationAddress = new System.Windows.Forms.TextBox();
            this.lblOrganizationAddress = new System.Windows.Forms.Label();
            this.lblOrganizationName = new System.Windows.Forms.Label();
            this.tbOrganizationName = new System.Windows.Forms.TextBox();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.tbOperatorName = new System.Windows.Forms.TextBox();
            this.gbDutInformation = new System.Windows.Forms.GroupBox();
            this.btnClearDeviceInformation = new System.Windows.Forms.Button();
            this.btnGetDeviceInformation = new System.Windows.Forms.Button();
            this.lblOtherInformation = new System.Windows.Forms.Label();
            this.tbOtherInformation = new System.Windows.Forms.TextBox();
            this.lblFirmware = new System.Windows.Forms.Label();
            this.tbFirmwareVersion = new System.Windows.Forms.TextBox();
            this.lblSerial = new System.Windows.Forms.Label();
            this.tbSerial = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.gbApplicationInformation = new System.Windows.Forms.GroupBox();
            this.cmbCoreVersion = new System.Windows.Forms.ComboBox();
            this.tbToolVersion = new System.Windows.Forms.TextBox();
            this.tbTestSpec = new System.Windows.Forms.TextBox();
            this.lblCoreSpec = new System.Windows.Forms.Label();
            this.lblTestSpec = new System.Windows.Forms.Label();
            this.lblTestToolVersion = new System.Windows.Forms.Label();
            this.setupPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbTestExecutionInformation.SuspendLayout();
            this.gbDutInformation.SuspendLayout();
            this.gbApplicationInformation.SuspendLayout();
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
            this.gbTestExecutionInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTestExecutionInformation.Location = new System.Drawing.Point(0, 294);
            this.gbTestExecutionInformation.Name = "gbTestExecutionInformation";
            this.gbTestExecutionInformation.Size = new System.Drawing.Size(758, 169);
            this.gbTestExecutionInformation.TabIndex = 21;
            this.gbTestExecutionInformation.TabStop = false;
            this.gbTestExecutionInformation.Text = "Test Execution Information";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(677, 110);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClear, "Clear tests execution information");
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
            this.tbOrganizationAddress.Size = new System.Drawing.Size(505, 62);
            this.tbOrganizationAddress.TabIndex = 14;
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
            this.tbOrganizationName.Size = new System.Drawing.Size(505, 20);
            this.tbOrganizationName.TabIndex = 8;
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
            this.tbOperatorName.Size = new System.Drawing.Size(505, 20);
            this.tbOperatorName.TabIndex = 6;
            // 
            // gbDutInformation
            // 
            this.gbDutInformation.Controls.Add(this.btnClearDeviceInformation);
            this.gbDutInformation.Controls.Add(this.btnGetDeviceInformation);
            this.gbDutInformation.Controls.Add(this.lblOtherInformation);
            this.gbDutInformation.Controls.Add(this.tbOtherInformation);
            this.gbDutInformation.Controls.Add(this.lblFirmware);
            this.gbDutInformation.Controls.Add(this.tbFirmwareVersion);
            this.gbDutInformation.Controls.Add(this.lblSerial);
            this.gbDutInformation.Controls.Add(this.tbSerial);
            this.gbDutInformation.Controls.Add(this.lblModel);
            this.gbDutInformation.Controls.Add(this.tbModel);
            this.gbDutInformation.Controls.Add(this.lblBrand);
            this.gbDutInformation.Controls.Add(this.tbBrand);
            this.gbDutInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDutInformation.Location = new System.Drawing.Point(0, 103);
            this.gbDutInformation.Name = "gbDutInformation";
            this.gbDutInformation.Size = new System.Drawing.Size(758, 191);
            this.gbDutInformation.TabIndex = 20;
            this.gbDutInformation.TabStop = false;
            this.gbDutInformation.Text = "Device Under Test Information";
            // 
            // btnClearDeviceInformation
            // 
            this.btnClearDeviceInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDeviceInformation.Location = new System.Drawing.Point(677, 162);
            this.btnClearDeviceInformation.Name = "btnClearDeviceInformation";
            this.btnClearDeviceInformation.Size = new System.Drawing.Size(75, 23);
            this.btnClearDeviceInformation.TabIndex = 17;
            this.btnClearDeviceInformation.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClearDeviceInformation, "Clear device information");
            this.btnClearDeviceInformation.UseVisualStyleBackColor = true;
            this.btnClearDeviceInformation.Click += new System.EventHandler(this.btnClearDeviceInformation_Click);
            // 
            // btnGetDeviceInformation
            // 
            this.btnGetDeviceInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetDeviceInformation.Location = new System.Drawing.Point(677, 19);
            this.btnGetDeviceInformation.Name = "btnGetDeviceInformation";
            this.btnGetDeviceInformation.Size = new System.Drawing.Size(75, 98);
            this.btnGetDeviceInformation.TabIndex = 16;
            this.btnGetDeviceInformation.Text = "Get From Device";
            this.setupPageToolTip.SetToolTip(this.btnGetDeviceInformation, "Get device information from the device");
            this.btnGetDeviceInformation.UseVisualStyleBackColor = true;
            this.btnGetDeviceInformation.Click += new System.EventHandler(this.btnGetDeviceInformation_Click);
            // 
            // lblOtherInformation
            // 
            this.lblOtherInformation.AutoSize = true;
            this.lblOtherInformation.Location = new System.Drawing.Point(6, 126);
            this.lblOtherInformation.Name = "lblOtherInformation";
            this.lblOtherInformation.Size = new System.Drawing.Size(91, 13);
            this.lblOtherInformation.TabIndex = 15;
            this.lblOtherInformation.Text = "Other Information:";
            // 
            // tbOtherInformation
            // 
            this.tbOtherInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOtherInformation.Location = new System.Drawing.Point(166, 123);
            this.tbOtherInformation.Multiline = true;
            this.tbOtherInformation.Name = "tbOtherInformation";
            this.tbOtherInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOtherInformation.Size = new System.Drawing.Size(505, 62);
            this.tbOtherInformation.TabIndex = 14;
            // 
            // lblFirmware
            // 
            this.lblFirmware.AutoSize = true;
            this.lblFirmware.Location = new System.Drawing.Point(6, 100);
            this.lblFirmware.Name = "lblFirmware";
            this.lblFirmware.Size = new System.Drawing.Size(90, 13);
            this.lblFirmware.TabIndex = 13;
            this.lblFirmware.Text = "Firmware Version:";
            // 
            // tbFirmwareVersion
            // 
            this.tbFirmwareVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFirmwareVersion.Location = new System.Drawing.Point(166, 97);
            this.tbFirmwareVersion.Name = "tbFirmwareVersion";
            this.tbFirmwareVersion.Size = new System.Drawing.Size(505, 20);
            this.tbFirmwareVersion.TabIndex = 12;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(6, 74);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(76, 13);
            this.lblSerial.TabIndex = 11;
            this.lblSerial.Text = "Serial Number:";
            // 
            // tbSerial
            // 
            this.tbSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSerial.Location = new System.Drawing.Point(166, 71);
            this.tbSerial.Name = "tbSerial";
            this.tbSerial.Size = new System.Drawing.Size(505, 20);
            this.tbSerial.TabIndex = 10;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(6, 48);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 9;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModel.Location = new System.Drawing.Point(166, 45);
            this.tbModel.Name = "tbModel";
            this.tbModel.Size = new System.Drawing.Size(505, 20);
            this.tbModel.TabIndex = 8;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(6, 22);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 7;
            this.lblBrand.Text = "Brand:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBrand.Location = new System.Drawing.Point(166, 19);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.Size = new System.Drawing.Size(505, 20);
            this.tbBrand.TabIndex = 6;
            // 
            // gbApplicationInformation
            // 
            this.gbApplicationInformation.Controls.Add(this.cmbCoreVersion);
            this.gbApplicationInformation.Controls.Add(this.tbToolVersion);
            this.gbApplicationInformation.Controls.Add(this.tbTestSpec);
            this.gbApplicationInformation.Controls.Add(this.lblCoreSpec);
            this.gbApplicationInformation.Controls.Add(this.lblTestSpec);
            this.gbApplicationInformation.Controls.Add(this.lblTestToolVersion);
            this.gbApplicationInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbApplicationInformation.Location = new System.Drawing.Point(0, 0);
            this.gbApplicationInformation.Name = "gbApplicationInformation";
            this.gbApplicationInformation.Size = new System.Drawing.Size(758, 103);
            this.gbApplicationInformation.TabIndex = 19;
            this.gbApplicationInformation.TabStop = false;
            this.gbApplicationInformation.Text = "Application Information";
            // 
            // cmbCoreVersion
            // 
            this.cmbCoreVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCoreVersion.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCoreVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoreVersion.FormattingEnabled = true;
            this.cmbCoreVersion.Location = new System.Drawing.Point(166, 19);
            this.cmbCoreVersion.Name = "cmbCoreVersion";
            this.cmbCoreVersion.Size = new System.Drawing.Size(505, 21);
            this.cmbCoreVersion.TabIndex = 1;
            // 
            // tbToolVersion
            // 
            this.tbToolVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbToolVersion.Location = new System.Drawing.Point(166, 72);
            this.tbToolVersion.Name = "tbToolVersion";
            this.tbToolVersion.ReadOnly = true;
            this.tbToolVersion.Size = new System.Drawing.Size(505, 20);
            this.tbToolVersion.TabIndex = 3;
            this.tbToolVersion.Text = "<ONVIF Test Tool version 1.02>";
            // 
            // tbTestSpec
            // 
            this.tbTestSpec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestSpec.Location = new System.Drawing.Point(166, 46);
            this.tbTestSpec.Name = "tbTestSpec";
            this.tbTestSpec.ReadOnly = true;
            this.tbTestSpec.Size = new System.Drawing.Size(505, 20);
            this.tbTestSpec.TabIndex = 2;
            this.tbTestSpec.Text = "<ONVIF Test Specification version 1.02, March, 2010>";
            // 
            // lblCoreSpec
            // 
            this.lblCoreSpec.AutoSize = true;
            this.lblCoreSpec.Location = new System.Drawing.Point(6, 23);
            this.lblCoreSpec.Name = "lblCoreSpec";
            this.lblCoreSpec.Size = new System.Drawing.Size(131, 13);
            this.lblCoreSpec.TabIndex = 3;
            this.lblCoreSpec.Text = "ONVIF Core Specification:";
            // 
            // lblTestSpec
            // 
            this.lblTestSpec.AutoSize = true;
            this.lblTestSpec.Location = new System.Drawing.Point(6, 49);
            this.lblTestSpec.Name = "lblTestSpec";
            this.lblTestSpec.Size = new System.Drawing.Size(130, 13);
            this.lblTestSpec.TabIndex = 1;
            this.lblTestSpec.Text = "ONVIF Test Specification:";
            // 
            // lblTestToolVersion
            // 
            this.lblTestToolVersion.AutoSize = true;
            this.lblTestToolVersion.Location = new System.Drawing.Point(6, 75);
            this.lblTestToolVersion.Name = "lblTestToolVersion";
            this.lblTestToolVersion.Size = new System.Drawing.Size(128, 13);
            this.lblTestToolVersion.TabIndex = 0;
            this.lblTestToolVersion.Text = "ONVIF Test Tool Version:";
            // 
            // SetupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTestExecutionInformation);
            this.Controls.Add(this.gbDutInformation);
            this.Controls.Add(this.gbApplicationInformation);
            this.Name = "SetupPage";
            this.Size = new System.Drawing.Size(758, 463);
            this.gbTestExecutionInformation.ResumeLayout(false);
            this.gbTestExecutionInformation.PerformLayout();
            this.gbDutInformation.ResumeLayout(false);
            this.gbDutInformation.PerformLayout();
            this.gbApplicationInformation.ResumeLayout(false);
            this.gbApplicationInformation.PerformLayout();
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
        private System.Windows.Forms.Button btnClearDeviceInformation;
        private System.Windows.Forms.Button btnGetDeviceInformation;
        private System.Windows.Forms.Label lblOtherInformation;
        private System.Windows.Forms.TextBox tbOtherInformation;
        private System.Windows.Forms.Label lblFirmware;
        private System.Windows.Forms.TextBox tbFirmwareVersion;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox tbSerial;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox tbModel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox tbBrand;
        private System.Windows.Forms.GroupBox gbApplicationInformation;
        private System.Windows.Forms.TextBox tbToolVersion;
        private System.Windows.Forms.TextBox tbTestSpec;
        private System.Windows.Forms.Label lblCoreSpec;
        private System.Windows.Forms.Label lblTestSpec;
        private System.Windows.Forms.Label lblTestToolVersion;
        private System.Windows.Forms.ToolTip setupPageToolTip;
        private System.Windows.Forms.ComboBox cmbCoreVersion;
    }
}
