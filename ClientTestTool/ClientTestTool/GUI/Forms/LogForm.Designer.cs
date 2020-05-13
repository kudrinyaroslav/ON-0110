namespace ClientTestTool.GUI.Forms
{
  partial class LogForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
      this.tBLog = new System.Windows.Forms.RichTextBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // tBLog
      // 
      this.tBLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBLog.DetectUrls = false;
      this.tBLog.Location = new System.Drawing.Point(12, 12);
      this.tBLog.Name = "tBLog";
      this.tBLog.ReadOnly = true;
      this.tBLog.Size = new System.Drawing.Size(531, 473);
      this.tBLog.TabIndex = 0;
      this.tBLog.Text = "";
      // 
      // btnOK
      // 
      this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnOK.Location = new System.Drawing.Point(238, 496);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // LogForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(555, 531);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.tBLog);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "LogForm";
      this.Text = "Application Log";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
      this.Load += new System.EventHandler(this.LogForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox tBLog;
    private System.Windows.Forms.Button btnOK;
  }
}