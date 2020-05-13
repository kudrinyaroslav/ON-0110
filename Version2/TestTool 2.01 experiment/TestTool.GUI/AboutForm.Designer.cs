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
            this.linkLabelDevelopedBy = new System.Windows.Forms.LinkLabel();
            this.lblDevelopedBy = new System.Windows.Forms.Label();
            this.lblProtected = new System.Windows.Forms.Label();
            this.linkOnvif = new System.Windows.Forms.LinkLabel();
            this.lblCopyRight = new System.Windows.Forms.Label();
            this.lblToolInfo = new System.Windows.Forms.Label();
            this.tbAgreement = new System.Windows.Forms.TextBox();
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
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.7027F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.29729F));
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
            this.panelTop.Controls.Add(this.linkLabelDevelopedBy);
            this.panelTop.Controls.Add(this.lblDevelopedBy);
            this.panelTop.Controls.Add(this.lblProtected);
            this.panelTop.Controls.Add(this.linkOnvif);
            this.panelTop.Controls.Add(this.lblCopyRight);
            this.panelTop.Controls.Add(this.lblToolInfo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(552, 117);
            this.panelTop.TabIndex = 2;
            // 
            // linkLabelDevelopedBy
            // 
            this.linkLabelDevelopedBy.AutoSize = true;
            this.linkLabelDevelopedBy.Location = new System.Drawing.Point(134, 50);
            this.linkLabelDevelopedBy.Name = "linkLabelDevelopedBy";
            this.linkLabelDevelopedBy.Size = new System.Drawing.Size(0, 13);
            this.linkLabelDevelopedBy.TabIndex = 5;
            this.linkLabelDevelopedBy.Visible = false;
            this.linkLabelDevelopedBy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBy_LinkClicked);
            // 
            // lblDevelopedBy
            // 
            this.lblDevelopedBy.AutoSize = true;
            this.lblDevelopedBy.Location = new System.Drawing.Point(9, 50);
            this.lblDevelopedBy.Name = "lblDevelopedBy";
            this.lblDevelopedBy.Size = new System.Drawing.Size(76, 13);
            this.lblDevelopedBy.TabIndex = 4;
            this.lblDevelopedBy.Text = "Developed by ";
            this.lblDevelopedBy.Visible = false;
            // 
            // lblProtected
            // 
            this.lblProtected.Location = new System.Drawing.Point(9, 72);
            this.lblProtected.Name = "lblProtected";
            this.lblProtected.Size = new System.Drawing.Size(534, 34);
            this.lblProtected.TabIndex = 3;
            this.lblProtected.Text = "This computer program is protected by copyright law and international treaties. U" +
                "nauthorized reproduction or distribution of this program is prohibited.";
            // 
            // linkOnvif
            // 
            this.linkOnvif.AutoSize = true;
            this.linkOnvif.Location = new System.Drawing.Point(236, 27);
            this.linkOnvif.Name = "linkOnvif";
            this.linkOnvif.Size = new System.Drawing.Size(75, 13);
            this.linkOnvif.TabIndex = 2;
            this.linkOnvif.TabStop = true;
            this.linkOnvif.Text = "www.onvif.org";
            this.linkOnvif.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBy_LinkClicked);
            // 
            // lblCopyRight
            // 
            this.lblCopyRight.AutoSize = true;
            this.lblCopyRight.Location = new System.Drawing.Point(9, 27);
            this.lblCopyRight.Name = "lblCopyRight";
            this.lblCopyRight.Size = new System.Drawing.Size(225, 13);
            this.lblCopyRight.TabIndex = 1;
            this.lblCopyRight.Text = "(c) 2010 ONVIF";
            // 
            // lblToolInfo
            // 
            this.lblToolInfo.AutoSize = true;
            this.lblToolInfo.Location = new System.Drawing.Point(9, 6);
            this.lblToolInfo.Name = "lblToolInfo";
            this.lblToolInfo.Size = new System.Drawing.Size(56, 13);
            this.lblToolInfo.TabIndex = 0;
            this.lblToolInfo.Text = "<tool info>";
            // 
            // tbAgreement
            // 
            this.tlpMain.SetColumnSpan(this.tbAgreement, 3);
            this.tbAgreement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAgreement.Location = new System.Drawing.Point(3, 126);
            this.tbAgreement.Multiline = true;
            this.tbAgreement.Name = "tbAgreement";
            this.tbAgreement.ReadOnly = true;
            this.tbAgreement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbAgreement.Size = new System.Drawing.Size(552, 315);
            this.tbAgreement.TabIndex = 3;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 474);
            this.Controls.Add(this.tlpMain);
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
        private System.Windows.Forms.LinkLabel linkLabelDevelopedBy;
        private System.Windows.Forms.Label lblDevelopedBy;
    }
}