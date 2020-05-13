namespace ClientTestTool.GUI.Forms
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
      this.lblName = new System.Windows.Forms.Label();
      this.lblVersion = new System.Windows.Forms.Label();
      this.lblCopyright1 = new System.Windows.Forms.Label();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.lblEULA = new System.Windows.Forms.LinkLabel();
      this.lblONVIFLink = new System.Windows.Forms.LinkLabel();
      this.lblCopyright2 = new System.Windows.Forms.Label();
      this.tBVendor = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // lblName
      // 
      this.lblName.AutoSize = true;
      this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblName.Location = new System.Drawing.Point(16, 8);
      this.lblName.Name = "lblName";
      this.lblName.Size = new System.Drawing.Size(160, 18);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "ONVIF Client Test Tool";
      // 
      // lblVersion
      // 
      this.lblVersion.AutoSize = true;
      this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblVersion.Location = new System.Drawing.Point(16, 24);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(154, 18);
      this.lblVersion.TabIndex = 0;
      this.lblVersion.Text = "Version: 15.02 rev.428";
      // 
      // lblCopyright1
      // 
      this.lblCopyright1.AutoSize = true;
      this.lblCopyright1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblCopyright1.Location = new System.Drawing.Point(12, 49);
      this.lblCopyright1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
      this.lblCopyright1.Name = "lblCopyright1";
      this.lblCopyright1.Size = new System.Drawing.Size(92, 18);
      this.lblCopyright1.TabIndex = 1;
      this.lblCopyright1.Text = "© 2014-2015";
      // 
      // linkLabel1
      // 
      this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.linkLabel1.Location = new System.Drawing.Point(86, 79);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(0, 13);
      this.linkLabel1.TabIndex = 2;
      // 
      // lblEULA
      // 
      this.lblEULA.AutoSize = true;
      this.lblEULA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblEULA.Location = new System.Drawing.Point(12, 69);
      this.lblEULA.Name = "lblEULA";
      this.lblEULA.Size = new System.Drawing.Size(241, 18);
      this.lblEULA.TabIndex = 3;
      this.lblEULA.TabStop = true;
      this.lblEULA.Text = "ONVIF Enduser Licence Agreement";
      this.lblEULA.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEULA_LinkClicked);
      // 
      // lblONVIFLink
      // 
      this.lblONVIFLink.AutoSize = true;
      this.lblONVIFLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblONVIFLink.Location = new System.Drawing.Point(106, 49);
      this.lblONVIFLink.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
      this.lblONVIFLink.Name = "lblONVIFLink";
      this.lblONVIFLink.Size = new System.Drawing.Size(52, 18);
      this.lblONVIFLink.TabIndex = 3;
      this.lblONVIFLink.TabStop = true;
      this.lblONVIFLink.Text = "ONVIF";
      this.lblONVIFLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblONVIFLink_LinkClicked);
      // 
      // lblCopyright2
      // 
      this.lblCopyright2.AutoSize = true;
      this.lblCopyright2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblCopyright2.Location = new System.Drawing.Point(156, 49);
      this.lblCopyright2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
      this.lblCopyright2.Name = "lblCopyright2";
      this.lblCopyright2.Size = new System.Drawing.Size(136, 18);
      this.lblCopyright2.TabIndex = 1;
      this.lblCopyright2.Text = "All Rights Reserved";
      // 
      // tBVendor
      // 
      this.tBVendor.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tBVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.tBVendor.Location = new System.Drawing.Point(15, 93);
      this.tBVendor.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
      this.tBVendor.Multiline = true;
      this.tBVendor.Name = "tBVendor";
      this.tBVendor.ReadOnly = true;
      this.tBVendor.Size = new System.Drawing.Size(261, 16);
      this.tBVendor.TabIndex = 4;
      this.tBVendor.Text = " Developed for ONVIF by Humasys Inc.";
      // 
      // AboutForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(302, 113);
      this.Controls.Add(this.tBVendor);
      this.Controls.Add(this.lblONVIFLink);
      this.Controls.Add(this.lblEULA);
      this.Controls.Add(this.linkLabel1);
      this.Controls.Add(this.lblCopyright2);
      this.Controls.Add(this.lblCopyright1);
      this.Controls.Add(this.lblVersion);
      this.Controls.Add(this.lblName);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.Text = "About";
      this.Load += new System.EventHandler(this.AboutForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.Label lblCopyright1;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.LinkLabel lblEULA;
    private System.Windows.Forms.LinkLabel lblONVIFLink;
    private System.Windows.Forms.Label lblCopyright2;
    private System.Windows.Forms.TextBox tBVendor;
  }
}