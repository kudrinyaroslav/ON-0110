namespace TestTool.GUI.Controls.Pages
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
            this.tbServicePort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblNetworkInterface = new System.Windows.Forms.Label();
            this.tbDeviceServiceAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.checkHWStyle = new System.Windows.Forms.CheckBox();
            this.cmbNICs = new System.Windows.Forms.ComboBox();
            this.gbAuthentication = new System.Windows.Forms.GroupBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.rbDigest = new System.Windows.Forms.RadioButton();
            this.rbWsUsername = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.gbDiscovery = new System.Windows.Forms.GroupBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.gbAddresses.SuspendLayout();
            this.gbAuthentication.SuspendLayout();
            this.gbDiscovery.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAddresses
            // 
            this.gbAddresses.Controls.Add(this.tbServicePort);
            this.gbAddresses.Controls.Add(this.lblPort);
            this.gbAddresses.Controls.Add(this.lblNetworkInterface);
            this.gbAddresses.Controls.Add(this.tbDeviceServiceAddress);
            this.gbAddresses.Controls.Add(this.lblAddress);
            this.gbAddresses.Controls.Add(this.checkHWStyle);
            this.gbAddresses.Controls.Add(this.cmbNICs);
            this.gbAddresses.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAddresses.Location = new System.Drawing.Point(0, 0);
            this.gbAddresses.Name = "gbAddresses";
            this.gbAddresses.Size = new System.Drawing.Size(610, 108);
            this.gbAddresses.TabIndex = 0;
            this.gbAddresses.TabStop = false;
            this.gbAddresses.Text = "Device Service Address";
            // 
            // tbServicePort
            // 
            this.tbServicePort.Location = new System.Drawing.Point(129, 47);
            this.tbServicePort.Name = "tbServicePort";
            this.tbServicePort.Size = new System.Drawing.Size(100, 20);
            this.tbServicePort.TabIndex = 3;
            this.tbServicePort.Text = "8080";
            this.tbServicePort.TextChanged += new System.EventHandler(this.tbServicePort_TextChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 50);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 15;
            this.lblPort.Text = "Port:";
            // 
            // lblNetworkInterface
            // 
            this.lblNetworkInterface.AutoSize = true;
            this.lblNetworkInterface.Location = new System.Drawing.Point(6, 24);
            this.lblNetworkInterface.Name = "lblNetworkInterface";
            this.lblNetworkInterface.Size = new System.Drawing.Size(95, 13);
            this.lblNetworkInterface.TabIndex = 14;
            this.lblNetworkInterface.Text = "Network Interface:";
            // 
            // tbDeviceServiceAddress
            // 
            this.tbDeviceServiceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceServiceAddress.Location = new System.Drawing.Point(143, 73);
            this.tbDeviceServiceAddress.Name = "tbDeviceServiceAddress";
            this.tbDeviceServiceAddress.ReadOnly = true;
            this.tbDeviceServiceAddress.Size = new System.Drawing.Size(437, 20);
            this.tbDeviceServiceAddress.TabIndex = 4;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(6, 76);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(124, 13);
            this.lblAddress.TabIndex = 12;
            this.lblAddress.Text = "Device Service Address:";
            // 
            // checkHWStyle
            // 
            this.checkHWStyle.AutoSize = true;
            this.checkHWStyle.Location = new System.Drawing.Point(443, 23);
            this.checkHWStyle.Name = "checkHWStyle";
            this.checkHWStyle.Size = new System.Drawing.Size(137, 17);
            this.checkHWStyle.TabIndex = 2;
            this.checkHWStyle.Text = "Use Hardware Notation";
            this.checkHWStyle.UseVisualStyleBackColor = true;
            this.checkHWStyle.CheckedChanged += new System.EventHandler(this.checkHWStyle_CheckedChanged);
            // 
            // cmbNICs
            // 
            this.cmbNICs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNICs.FormattingEnabled = true;
            this.cmbNICs.Location = new System.Drawing.Point(129, 19);
            this.cmbNICs.Name = "cmbNICs";
            this.cmbNICs.Size = new System.Drawing.Size(296, 21);
            this.cmbNICs.TabIndex = 1;
            this.cmbNICs.SelectedIndexChanged += new System.EventHandler(this.cmbNICs_SelectedIndexChanged);
            // 
            // gbAuthentication
            // 
            this.gbAuthentication.Controls.Add(this.tbPassword);
            this.gbAuthentication.Controls.Add(this.lblPassword);
            this.gbAuthentication.Controls.Add(this.tbUsername);
            this.gbAuthentication.Controls.Add(this.lblUsername);
            this.gbAuthentication.Controls.Add(this.rbDigest);
            this.gbAuthentication.Controls.Add(this.rbWsUsername);
            this.gbAuthentication.Controls.Add(this.rbNone);
            this.gbAuthentication.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAuthentication.Location = new System.Drawing.Point(0, 108);
            this.gbAuthentication.Name = "gbAuthentication";
            this.gbAuthentication.Size = new System.Drawing.Size(610, 93);
            this.gbAuthentication.TabIndex = 3;
            this.gbAuthentication.TabStop = false;
            this.gbAuthentication.Text = "Authentication";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(320, 43);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(105, 20);
            this.tbPassword.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(230, 46);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(84, 43);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(100, 20);
            this.tbUsername.TabIndex = 5;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 46);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(39, 13);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "Login: ";
            // 
            // rbDigest
            // 
            this.rbDigest.AutoSize = true;
            this.rbDigest.Enabled = false;
            this.rbDigest.Location = new System.Drawing.Point(233, 19);
            this.rbDigest.Name = "rbDigest";
            this.rbDigest.Size = new System.Drawing.Size(55, 17);
            this.rbDigest.TabIndex = 3;
            this.rbDigest.TabStop = true;
            this.rbDigest.Text = "Digest";
            this.rbDigest.UseVisualStyleBackColor = true;
            // 
            // rbWsUsername
            // 
            this.rbWsUsername.AutoSize = true;
            this.rbWsUsername.Checked = true;
            this.rbWsUsername.Location = new System.Drawing.Point(84, 19);
            this.rbWsUsername.Name = "rbWsUsername";
            this.rbWsUsername.Size = new System.Drawing.Size(124, 17);
            this.rbWsUsername.TabIndex = 2;
            this.rbWsUsername.TabStop = true;
            this.rbWsUsername.Text = "WS-Username token";
            this.rbWsUsername.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(6, 19);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 1;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            // 
            // gbDiscovery
            // 
            this.gbDiscovery.Controls.Add(this.lblDescription);
            this.gbDiscovery.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDiscovery.Location = new System.Drawing.Point(0, 201);
            this.gbDiscovery.Name = "gbDiscovery";
            this.gbDiscovery.Size = new System.Drawing.Size(610, 124);
            this.gbDiscovery.TabIndex = 4;
            this.gbDiscovery.TabStop = false;
            this.gbDiscovery.Text = "Discovery";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(125, 34);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(360, 52);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Some controls to setup discovery behavior. May be should be placed at separate pa" +
                "ge, if too many settings needed";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ManagementPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDiscovery);
            this.Controls.Add(this.gbAuthentication);
            this.Controls.Add(this.gbAddresses);
            this.Name = "ManagementPage";
            this.Size = new System.Drawing.Size(610, 405);
            this.SizeChanged += new System.EventHandler(this.ManagementPage_SizeChanged);
            this.gbAddresses.ResumeLayout(false);
            this.gbAddresses.PerformLayout();
            this.gbAuthentication.ResumeLayout(false);
            this.gbAuthentication.PerformLayout();
            this.gbDiscovery.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAddresses;
        private System.Windows.Forms.CheckBox checkHWStyle;
        private System.Windows.Forms.ComboBox cmbNICs;
        private System.Windows.Forms.TextBox tbDeviceServiceAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox tbServicePort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblNetworkInterface;
        private System.Windows.Forms.GroupBox gbAuthentication;
        private System.Windows.Forms.RadioButton rbDigest;
        private System.Windows.Forms.RadioButton rbWsUsername;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.GroupBox gbDiscovery;
        private System.Windows.Forms.Label lblDescription;
    }
}
