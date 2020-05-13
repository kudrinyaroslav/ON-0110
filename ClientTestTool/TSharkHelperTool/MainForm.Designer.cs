namespace TSharkHelperTool
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.tBPath = new System.Windows.Forms.TextBox();
      this.btnBrowse = new System.Windows.Forms.Button();
      this.tBOutput = new System.Windows.Forms.RichTextBox();
      this.btnRun = new System.Windows.Forms.Button();
      this.tBArgs = new System.Windows.Forms.TextBox();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnFeatureList = new System.Windows.Forms.Button();
      this.btnFrames = new System.Windows.Forms.Button();
      this.statusStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tBPath
      // 
      this.tBPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBPath.Location = new System.Drawing.Point(16, 15);
      this.tBPath.Margin = new System.Windows.Forms.Padding(4);
      this.tBPath.Name = "tBPath";
      this.tBPath.ReadOnly = true;
      this.tBPath.Size = new System.Drawing.Size(552, 22);
      this.tBPath.TabIndex = 0;
      // 
      // btnBrowse
      // 
      this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBrowse.Location = new System.Drawing.Point(577, 12);
      this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new System.Drawing.Size(100, 28);
      this.btnBrowse.TabIndex = 1;
      this.btnBrowse.Text = "Browse";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
      // 
      // tBOutput
      // 
      this.tBOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBOutput.DetectUrls = false;
      this.tBOutput.Location = new System.Drawing.Point(16, 139);
      this.tBOutput.Margin = new System.Windows.Forms.Padding(4);
      this.tBOutput.Name = "tBOutput";
      this.tBOutput.ReadOnly = true;
      this.tBOutput.Size = new System.Drawing.Size(660, 432);
      this.tBOutput.TabIndex = 2;
      this.tBOutput.Text = "";
      this.tBOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tBOutput_MouseDown);
      // 
      // btnRun
      // 
      this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRun.Location = new System.Drawing.Point(577, 48);
      this.btnRun.Margin = new System.Windows.Forms.Padding(4);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(100, 28);
      this.btnRun.TabIndex = 1;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // tBArgs
      // 
      this.tBArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBArgs.Location = new System.Drawing.Point(16, 50);
      this.tBArgs.Margin = new System.Windows.Forms.Padding(4);
      this.tBArgs.Name = "tBArgs";
      this.tBArgs.Size = new System.Drawing.Size(552, 22);
      this.tBArgs.TabIndex = 0;
      this.tBArgs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBArgs_KeyDown);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.progressBar});
      this.statusStrip1.Location = new System.Drawing.Point(0, 577);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
      this.statusStrip1.Size = new System.Drawing.Size(693, 26);
      this.statusStrip1.TabIndex = 4;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(538, 21);
      this.toolStripStatusLabel1.Spring = true;
      // 
      // progressBar
      // 
      this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(133, 20);
      this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnFrames);
      this.groupBox1.Controls.Add(this.btnFeatureList);
      this.groupBox1.Location = new System.Drawing.Point(16, 79);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(660, 53);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Test Queries";
      // 
      // btnFeatureList
      // 
      this.btnFeatureList.Location = new System.Drawing.Point(3, 18);
      this.btnFeatureList.Name = "btnFeatureList";
      this.btnFeatureList.Size = new System.Drawing.Size(93, 29);
      this.btnFeatureList.TabIndex = 0;
      this.btnFeatureList.Text = "FeatureListQuery";
      this.btnFeatureList.UseVisualStyleBackColor = true;
      this.btnFeatureList.Click += new System.EventHandler(this.btnFeatureList_Click);
      // 
      // btnFrames
      // 
      this.btnFrames.Location = new System.Drawing.Point(102, 18);
      this.btnFrames.Name = "btnFrames";
      this.btnFrames.Size = new System.Drawing.Size(103, 29);
      this.btnFrames.TabIndex = 0;
      this.btnFrames.Text = "FramesQuery";
      this.btnFrames.UseVisualStyleBackColor = true;
      this.btnFrames.Click += new System.EventHandler(this.btnFrames_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(693, 603);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.tBOutput);
      this.Controls.Add(this.btnRun);
      this.Controls.Add(this.btnBrowse);
      this.Controls.Add(this.tBArgs);
      this.Controls.Add(this.tBPath);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "MainForm";
      this.Text = "TShark Helper";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tBPath;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.RichTextBox tBOutput;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.TextBox tBArgs;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripProgressBar progressBar;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnFeatureList;
    private System.Windows.Forms.Button btnFrames;
  }
}

