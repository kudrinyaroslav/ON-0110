namespace TestTool.GUI
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblProtected = new System.Windows.Forms.Label();
            this.linkOnvif = new System.Windows.Forms.LinkLabel();
            this.lblCopyRight = new System.Windows.Forms.Label();
            this.lblToolInfo = new System.Windows.Forms.Label();
            this.lblCopyrightSign = new System.Windows.Forms.Label();
            this.tbAgreement = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.btnOK, 1, 2);
            this.tlpMain.Controls.Add(this.panelTop, 0, 0);
            this.tlpMain.Controls.Add(this.tbAgreement, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.63063F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.36937F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(558, 474);
            this.tlpMain.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(242, 447);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // panelTop
            // 
            this.tlpMain.SetColumnSpan(this.panelTop, 3);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.lblProtected);
            this.panelTop.Controls.Add(this.linkOnvif);
            this.panelTop.Controls.Add(this.lblCopyRight);
            this.panelTop.Controls.Add(this.lblToolInfo);
            this.panelTop.Controls.Add(this.lblCopyrightSign);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(552, 130);
            this.panelTop.TabIndex = 2;
            // 
            // lblProtected
            // 
            this.lblProtected.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProtected.Location = new System.Drawing.Point(12, 69);
            this.lblProtected.Name = "lblProtected";
            this.lblProtected.Size = new System.Drawing.Size(534, 23);
            this.lblProtected.TabIndex = 3;
            this.lblProtected.Text = "Developed for ONVIF by AstroSoft Ltd.";
            // 
            // linkOnvif
            // 
            this.linkOnvif.AutoSize = true;
            this.linkOnvif.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkOnvif.Location = new System.Drawing.Point(236, 42);
            this.linkOnvif.Name = "linkOnvif";
            this.linkOnvif.Size = new System.Drawing.Size(83, 15);
            this.linkOnvif.TabIndex = 2;
            this.linkOnvif.TabStop = true;
            this.linkOnvif.Text = "www.onvif.org";
            this.linkOnvif.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBy_LinkClicked);
            // 
            // lblCopyRight
            // 
            this.lblCopyRight.AutoSize = true;
            this.lblCopyRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyRight.Location = new System.Drawing.Point(24, 42);
            this.lblCopyRight.Margin = new System.Windows.Forms.Padding(0);
            this.lblCopyRight.Name = "lblCopyRight";
            this.lblCopyRight.Size = new System.Drawing.Size(212, 15);
            this.lblCopyRight.TabIndex = 1;
            this.lblCopyRight.Text = "2015 by ONVIF, Inc. All rights reserved";
            // 
            // lblToolInfo
            // 
            this.lblToolInfo.AutoSize = true;
            this.lblToolInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolInfo.Location = new System.Drawing.Point(9, 6);
            this.lblToolInfo.Name = "lblToolInfo";
            this.lblToolInfo.Size = new System.Drawing.Size(64, 15);
            this.lblToolInfo.TabIndex = 0;
            this.lblToolInfo.Text = "<tool info>";
            // 
            // lblCopyrightSign
            // 
            this.lblCopyrightSign.AutoSize = true;
            this.lblCopyrightSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCopyrightSign.Location = new System.Drawing.Point(12, 42);
            this.lblCopyrightSign.Margin = new System.Windows.Forms.Padding(0);
            this.lblCopyrightSign.Name = "lblCopyrightSign";
            this.lblCopyrightSign.Size = new System.Drawing.Size(16, 16);
            this.lblCopyrightSign.TabIndex = 4;
            this.lblCopyrightSign.Text = "©";
            // 
            // tbAgreement
            // 
            this.tlpMain.SetColumnSpan(this.tbAgreement, 3);
            this.tbAgreement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAgreement.Location = new System.Drawing.Point(3, 139);
            this.tbAgreement.Multiline = true;
            this.tbAgreement.Name = "tbAgreement";
            this.tbAgreement.ReadOnly = true;
            this.tbAgreement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbAgreement.Size = new System.Drawing.Size(552, 302);
            this.tbAgreement.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(534, 34);
            this.label1.TabIndex = 5;
            this.label1.Text = "This computer program is protected by copyright law and international treaties. U" +
    "nauthorized reproduction or distribution of this program is prohibited.";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 474);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblCopyRight;
        private System.Windows.Forms.Label lblToolInfo;
        private System.Windows.Forms.Label lblProtected;
        private System.Windows.Forms.LinkLabel linkOnvif;
        private System.Windows.Forms.TextBox tbAgreement;
        private System.Windows.Forms.Label lblCopyrightSign;
        private System.Windows.Forms.Label label1;
    }
}