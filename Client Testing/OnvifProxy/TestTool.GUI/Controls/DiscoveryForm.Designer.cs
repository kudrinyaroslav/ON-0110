namespace TestTool.GUI.Controls
{
    partial class DiscoveryForm
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
            this.checkHWStyle = new System.Windows.Forms.CheckBox();
            this.cmbNICs = new System.Windows.Forms.ComboBox();
            this.lblNIC = new System.Windows.Forms.Label();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.hdrIP = new System.Windows.Forms.ColumnHeader();
            this.hdrUUID = new System.Windows.Forms.ColumnHeader();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.cmbServiceAddress = new System.Windows.Forms.ComboBox();
            this.lblDeviceServiceAddress = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkHWStyle
            // 
            this.checkHWStyle.AutoSize = true;
            this.checkHWStyle.Location = new System.Drawing.Point(377, 11);
            this.checkHWStyle.Name = "checkHWStyle";
            this.checkHWStyle.Size = new System.Drawing.Size(137, 17);
            this.checkHWStyle.TabIndex = 11;
            this.checkHWStyle.Text = "Use Hardware Notation";
            this.checkHWStyle.UseVisualStyleBackColor = true;
            this.checkHWStyle.CheckedChanged += new System.EventHandler(this.checkHWStyle_CheckedChanged);
            // 
            // cmbNICs
            // 
            this.cmbNICs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNICs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNICs.FormattingEnabled = true;
            this.cmbNICs.Location = new System.Drawing.Point(46, 9);
            this.cmbNICs.Name = "cmbNICs";
            this.cmbNICs.Size = new System.Drawing.Size(325, 21);
            this.cmbNICs.TabIndex = 10;
            // 
            // lblNIC
            // 
            this.lblNIC.AutoSize = true;
            this.lblNIC.Location = new System.Drawing.Point(12, 12);
            this.lblNIC.Name = "lblNIC";
            this.lblNIC.Size = new System.Drawing.Size(28, 13);
            this.lblNIC.TabIndex = 14;
            this.lblNIC.Text = "NIC:";
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrIP,
            this.hdrUUID});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.GridLines = true;
            this.lvDevices.Location = new System.Drawing.Point(12, 65);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(526, 237);
            this.lvDevices.TabIndex = 13;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            // 
            // hdrIP
            // 
            this.hdrIP.Text = "IP";
            this.hdrIP.Width = 107;
            // 
            // hdrUUID
            // 
            this.hdrUUID.Text = "UUID";
            this.hdrUUID.Width = 220;
            // 
            // btnDiscover
            // 
            this.btnDiscover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscover.Location = new System.Drawing.Point(12, 36);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(526, 23);
            this.btnDiscover.TabIndex = 12;
            this.btnDiscover.Text = "Discover Devices";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // cmbServiceAddress
            // 
            this.cmbServiceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbServiceAddress.FormattingEnabled = true;
            this.cmbServiceAddress.Location = new System.Drawing.Point(162, 313);
            this.cmbServiceAddress.Name = "cmbServiceAddress";
            this.cmbServiceAddress.Size = new System.Drawing.Size(376, 21);
            this.cmbServiceAddress.TabIndex = 15;
            // 
            // lblDeviceServiceAddress
            // 
            this.lblDeviceServiceAddress.AutoSize = true;
            this.lblDeviceServiceAddress.Location = new System.Drawing.Point(9, 321);
            this.lblDeviceServiceAddress.Name = "lblDeviceServiceAddress";
            this.lblDeviceServiceAddress.Size = new System.Drawing.Size(124, 13);
            this.lblDeviceServiceAddress.TabIndex = 16;
            this.lblDeviceServiceAddress.Text = "Device Service Address:";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(197, 340);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(278, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 374);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDeviceServiceAddress);
            this.Controls.Add(this.cmbServiceAddress);
            this.Controls.Add(this.checkHWStyle);
            this.Controls.Add(this.cmbNICs);
            this.Controls.Add(this.lblNIC);
            this.Controls.Add(this.lvDevices);
            this.Controls.Add(this.btnDiscover);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiscoveryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Discover Devices";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkHWStyle;
        private System.Windows.Forms.ComboBox cmbNICs;
        private System.Windows.Forms.Label lblNIC;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader hdrIP;
        private System.Windows.Forms.ColumnHeader hdrUUID;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.ComboBox cmbServiceAddress;
        private System.Windows.Forms.Label lblDeviceServiceAddress;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}