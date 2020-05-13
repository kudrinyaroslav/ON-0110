using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  partial class TestOutputView
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
      this.btnSearchPrev = new System.Windows.Forms.Button();
      this.btnSearchNext = new System.Windows.Forms.Button();
      this.tBFind = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tBOutput = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // btnSearchPrev
      // 
      this.btnSearchPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSearchPrev.Location = new System.Drawing.Point(571, 611);
      this.btnSearchPrev.Margin = new System.Windows.Forms.Padding(4);
      this.btnSearchPrev.Name = "btnSearchPrev";
      this.btnSearchPrev.Size = new System.Drawing.Size(100, 28);
      this.btnSearchPrev.TabIndex = 8;
      this.btnSearchPrev.Text = "Previous";
      this.btnSearchPrev.UseVisualStyleBackColor = true;
      this.btnSearchPrev.Click += new System.EventHandler(this.btnSearchPrev_Click);
      // 
      // btnSearchNext
      // 
      this.btnSearchNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSearchNext.Location = new System.Drawing.Point(463, 611);
      this.btnSearchNext.Margin = new System.Windows.Forms.Padding(4);
      this.btnSearchNext.Name = "btnSearchNext";
      this.btnSearchNext.Size = new System.Drawing.Size(100, 28);
      this.btnSearchNext.TabIndex = 9;
      this.btnSearchNext.Text = "Next";
      this.btnSearchNext.UseVisualStyleBackColor = true;
      this.btnSearchNext.Click += new System.EventHandler(this.btnSearchNext_Click);
      // 
      // tBFind
      // 
      this.tBFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.tBFind.Location = new System.Drawing.Point(308, 613);
      this.tBFind.Margin = new System.Windows.Forms.Padding(4);
      this.tBFind.Name = "tBFind";
      this.tBFind.Size = new System.Drawing.Size(145, 22);
      this.tBFind.TabIndex = 7;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(260, 617);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(39, 17);
      this.label1.TabIndex = 6;
      this.label1.Text = "Find:";
      // 
      // tBOutput
      // 
      this.tBOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBOutput.DetectUrls = false;
      this.tBOutput.Location = new System.Drawing.Point(4, 4);
      this.tBOutput.Margin = new System.Windows.Forms.Padding(4);
      this.tBOutput.Name = "tBOutput";
      this.tBOutput.ReadOnly = true;
      this.tBOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
      this.tBOutput.Size = new System.Drawing.Size(1038, 564);
      this.tBOutput.TabIndex = 5;
      this.tBOutput.Text = "";
      this.tBOutput.Leave += new System.EventHandler(this.tBOutput_Leave);
      // 
      // TestOutputView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnSearchPrev);
      this.Controls.Add(this.btnSearchNext);
      this.Controls.Add(this.tBFind);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tBOutput);
      this.Name = "TestOutputView";
      this.Size = new System.Drawing.Size(1046, 682);
      this.Load += new System.EventHandler(this.TestOutputView_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button  btnSearchPrev;
    private System.Windows.Forms.Button  btnSearchNext;
    private System.Windows.Forms.TextBox tBFind;
    private System.Windows.Forms.Label   label1;
    private RichTextBox                tBOutput;
  }
}
