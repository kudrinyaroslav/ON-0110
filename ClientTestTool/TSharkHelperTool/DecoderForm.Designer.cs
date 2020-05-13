namespace TSharkHelperTool
{
  partial class DecoderForm
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
      this.tBOutput = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // tBOutput
      // 
      this.tBOutput.DetectUrls = false;
      this.tBOutput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBOutput.Location = new System.Drawing.Point(0, 0);
      this.tBOutput.Name = "tBOutput";
      this.tBOutput.ReadOnly = true;
      this.tBOutput.Size = new System.Drawing.Size(532, 492);
      this.tBOutput.TabIndex = 0;
      this.tBOutput.Text = "";
      // 
      // DecoderForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(532, 492);
      this.Controls.Add(this.tBOutput);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "DecoderForm";
      this.Text = "Decoded Text";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox tBOutput;
  }
}