using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Conformance
{
  partial class PageConformance
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
      this.components = new System.ComponentModel.Container();
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.sFDConformance = new System.Windows.Forms.SaveFileDialog();
      this.sCMain = new System.Windows.Forms.SplitContainer();
      this.conformanceInfoView = new ClientTestTool.GUI.Controls.Conformance.ConformanceInfoView();
      this.gBLog = new System.Windows.Forms.GroupBox();
      this.conformanceLogView = new ClientTestTool.GUI.Controls.Conformance.ConformanceLogView();
      this.btnGenerateFeatureList = new System.Windows.Forms.Button();
      this.btnGenetateDoC = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).BeginInit();
      this.sCMain.Panel1.SuspendLayout();
      this.sCMain.Panel2.SuspendLayout();
      this.sCMain.SuspendLayout();
      this.gBLog.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.Name = "contextMenuStrip1";
      this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
      // 
      // sFDConformance
      // 
      this.sFDConformance.DefaultExt = "xml";
      this.sFDConformance.Filter = "All files|*.*";
      // 
      // sCMain
      // 
      this.sCMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.sCMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCMain.Location = new System.Drawing.Point(0, 0);
      this.sCMain.Margin = new System.Windows.Forms.Padding(2);
      this.sCMain.Name = "sCMain";
      // 
      // sCMain.Panel1
      // 
      this.sCMain.Panel1.Controls.Add(this.conformanceInfoView);
      this.sCMain.Panel1MinSize = 450;
      // 
      // sCMain.Panel2
      // 
      this.sCMain.Panel2.Controls.Add(this.gBLog);
      this.sCMain.Panel2MinSize = 350;
      this.sCMain.Size = new System.Drawing.Size(1008, 701);
      this.sCMain.SplitterDistance = 467;
      this.sCMain.SplitterWidth = 3;
      this.sCMain.TabIndex = 14;
      // 
      // conformanceInfoView
      // 
      this.conformanceInfoView.ClientUnderTest = null;
      this.conformanceInfoView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conformanceInfoView.Location = new System.Drawing.Point(0, 0);
      this.conformanceInfoView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.conformanceInfoView.Name = "conformanceInfoView";
      this.conformanceInfoView.Size = new System.Drawing.Size(463, 697);
      this.conformanceInfoView.TabIndex = 0;
      // 
      // gBLog
      // 
      this.gBLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gBLog.Controls.Add(this.conformanceLogView);
      this.gBLog.Controls.Add(this.btnGenerateFeatureList);
      this.gBLog.Controls.Add(this.btnGenetateDoC);
      this.gBLog.Location = new System.Drawing.Point(3, 3);
      this.gBLog.Name = "gBLog";
      this.gBLog.Size = new System.Drawing.Size(529, 690);
      this.gBLog.TabIndex = 13;
      this.gBLog.TabStop = false;
      this.gBLog.Text = "Client Conformance Audit Log:";
      // 
      // conformanceLogView
      // 
      this.conformanceLogView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.conformanceLogView.Location = new System.Drawing.Point(6, 19);
      this.conformanceLogView.Name = "conformanceLogView";
      this.conformanceLogView.Size = new System.Drawing.Size(517, 630);
      this.conformanceLogView.TabIndex = 17;
      // 
      // btnGenerateFeatureList
      // 
      this.btnGenerateFeatureList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnGenerateFeatureList.Location = new System.Drawing.Point(162, 661);
      this.btnGenerateFeatureList.Name = "btnGenerateFeatureList";
      this.btnGenerateFeatureList.Size = new System.Drawing.Size(150, 23);
      this.btnGenerateFeatureList.TabIndex = 16;
      this.btnGenerateFeatureList.Text = "Generate Feature List";
      this.btnGenerateFeatureList.UseVisualStyleBackColor = true;
      this.btnGenerateFeatureList.Click += new System.EventHandler(this.btnGenerateFeatureList_Click);
      // 
      // btnGenetateDoC
      // 
      this.btnGenetateDoC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnGenetateDoC.Location = new System.Drawing.Point(6, 661);
      this.btnGenetateDoC.Name = "btnGenetateDoC";
      this.btnGenetateDoC.Size = new System.Drawing.Size(150, 23);
      this.btnGenetateDoC.TabIndex = 15;
      this.btnGenetateDoC.Text = "Generate DoC with Errata";
      this.btnGenetateDoC.UseVisualStyleBackColor = true;
      this.btnGenetateDoC.Click += new System.EventHandler(this.btnGenerateDoC_Click);
      // 
      // PageConformance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.sCMain);
      this.Name = "PageConformance";
      this.Size = new System.Drawing.Size(1008, 701);
      this.Load += new System.EventHandler(this.ConformancePage_Load);
      this.sCMain.Panel1.ResumeLayout(false);
      this.sCMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).EndInit();
      this.sCMain.ResumeLayout(false);
      this.gBLog.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnGenetateDoC;
    private System.Windows.Forms.Button btnGenerateFeatureList;
    private System.Windows.Forms.GroupBox gBLog;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private SplitContainer sCMain;
    private SaveFileDialog sFDConformance;
    private ConformanceInfoView conformanceInfoView;
    private ConformanceLogView conformanceLogView;

  }
}
