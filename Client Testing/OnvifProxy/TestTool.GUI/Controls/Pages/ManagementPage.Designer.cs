namespace TestTool.GUI.Controls
{
    partial class ManagementPage
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
            this.gbAddresses = new System.Windows.Forms.GroupBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.tbDeviceServiceAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.checkHWStyle = new System.Windows.Forms.CheckBox();
            this.cmbNICs = new System.Windows.Forms.ComboBox();
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbDeviceAddress = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lblDeviceServiceAddress = new System.Windows.Forms.Label();
            this.gbAddresses.SuspendLayout();
            this.gbDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAddresses
            // 
            this.gbAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAddresses.Controls.Add(this.tbPort);
            this.gbAddresses.Controls.Add(this.lblPort);
            this.gbAddresses.Controls.Add(this.tbDeviceServiceAddress);
            this.gbAddresses.Controls.Add(this.lblAddress);
            this.gbAddresses.Controls.Add(this.checkHWStyle);
            this.gbAddresses.Controls.Add(this.cmbNICs);
            this.gbAddresses.Location = new System.Drawing.Point(4, 5);
            this.gbAddresses.Name = "gbAddresses";
            this.gbAddresses.Size = new System.Drawing.Size(603, 108);
            this.gbAddresses.TabIndex = 0;
            this.gbAddresses.TabStop = false;
            this.gbAddresses.Text = "Network Interface";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(147, 44);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 12;
            this.tbPort.TextChanged += new System.EventHandler(this.tbServicePort_TextChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(13, 47);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 14;
            this.lblPort.Text = "Port";
            // 
            // tbDeviceServiceAddress
            // 
            this.tbDeviceServiceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceServiceAddress.Location = new System.Drawing.Point(147, 70);
            this.tbDeviceServiceAddress.Name = "tbDeviceServiceAddress";
            this.tbDeviceServiceAddress.ReadOnly = true;
            this.tbDeviceServiceAddress.Size = new System.Drawing.Size(254, 20);
            this.tbDeviceServiceAddress.TabIndex = 13;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(10, 73);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(124, 13);
            this.lblAddress.TabIndex = 12;
            this.lblAddress.Text = "Device Service Address:";
            // 
            // checkHWStyle
            // 
            this.checkHWStyle.AutoSize = true;
            this.checkHWStyle.Location = new System.Drawing.Point(417, 23);
            this.checkHWStyle.Name = "checkHWStyle";
            this.checkHWStyle.Size = new System.Drawing.Size(137, 17);
            this.checkHWStyle.TabIndex = 11;
            this.checkHWStyle.Text = "Use Hardware Notation";
            this.checkHWStyle.UseVisualStyleBackColor = true;
            this.checkHWStyle.CheckedChanged += new System.EventHandler(this.checkHWStyle_CheckedChanged);
            // 
            // cmbNICs
            // 
            this.cmbNICs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNICs.FormattingEnabled = true;
            this.cmbNICs.Location = new System.Drawing.Point(13, 19);
            this.cmbNICs.Name = "cmbNICs";
            this.cmbNICs.Size = new System.Drawing.Size(387, 21);
            this.cmbNICs.TabIndex = 10;
            this.cmbNICs.SelectedIndexChanged += new System.EventHandler(this.cmbNICs_SelectedIndexChanged);
            // 
            // gbDevice
            // 
            this.gbDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDevice.Controls.Add(this.btnFind);
            this.gbDevice.Controls.Add(this.tbDeviceAddress);
            this.gbDevice.Controls.Add(this.lblPassword);
            this.gbDevice.Controls.Add(this.tbPassword);
            this.gbDevice.Controls.Add(this.lblUsername);
            this.gbDevice.Controls.Add(this.tbUsername);
            this.gbDevice.Controls.Add(this.lblDeviceServiceAddress);
            this.gbDevice.Location = new System.Drawing.Point(7, 119);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Size = new System.Drawing.Size(603, 115);
            this.gbDevice.TabIndex = 1;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Onvif Device for Testing";
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(414, 22);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Find...";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbDeviceAddress
            // 
            this.tbDeviceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceAddress.Location = new System.Drawing.Point(144, 24);
            this.tbDeviceAddress.Name = "tbDeviceAddress";
            this.tbDeviceAddress.Size = new System.Drawing.Size(254, 20);
            this.tbDeviceAddress.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(7, 79);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 44;
            this.lblPassword.Text = "Password:";
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(144, 76);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(254, 20);
            this.tbPassword.TabIndex = 4;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(7, 53);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(63, 13);
            this.lblUsername.TabIndex = 43;
            this.lblUsername.Text = "User Name:";
            // 
            // tbUsername
            // 
            this.tbUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUsername.Location = new System.Drawing.Point(144, 50);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(254, 20);
            this.tbUsername.TabIndex = 3;
            // 
            // lblDeviceServiceAddress
            // 
            this.lblDeviceServiceAddress.AutoSize = true;
            this.lblDeviceServiceAddress.Location = new System.Drawing.Point(7, 27);
            this.lblDeviceServiceAddress.Name = "lblDeviceServiceAddress";
            this.lblDeviceServiceAddress.Size = new System.Drawing.Size(124, 13);
            this.lblDeviceServiceAddress.TabIndex = 42;
            this.lblDeviceServiceAddress.Text = "Device Service Address:";
            // 
            // ManagementPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDevice);
            this.Controls.Add(this.gbAddresses);
            this.Name = "ManagementPage";
            this.Size = new System.Drawing.Size(610, 405);
            this.gbAddresses.ResumeLayout(false);
            this.gbAddresses.PerformLayout();
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAddresses;
        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.CheckBox checkHWStyle;
        private System.Windows.Forms.ComboBox cmbNICs;
        private System.Windows.Forms.TextBox tbDeviceServiceAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox tbDeviceAddress;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lblDeviceServiceAddress;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnFind;
    }
}
