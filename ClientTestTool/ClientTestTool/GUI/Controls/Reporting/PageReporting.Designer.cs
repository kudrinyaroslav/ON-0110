namespace ClientTestTool.GUI.Controls.Reporting
{
  partial class PageReporting
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
      this.sCMainContainer = new System.Windows.Forms.SplitContainer();
      this.gBConversations = new System.Windows.Forms.GroupBox();
      this.lVConversations = new System.Windows.Forms.ListView();
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.requestResponseList = new ClientTestTool.GUI.Controls.Reporting.RequestResponseListView();
      this.btnSaveConversationReport = new System.Windows.Forms.Button();
      this.sFDReportingReport = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.sCMainContainer)).BeginInit();
      this.sCMainContainer.Panel1.SuspendLayout();
      this.sCMainContainer.Panel2.SuspendLayout();
      this.sCMainContainer.SuspendLayout();
      this.gBConversations.SuspendLayout();
      this.SuspendLayout();
      // 
      // sCMainContainer
      // 
      this.sCMainContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.sCMainContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCMainContainer.Location = new System.Drawing.Point(0, 0);
      this.sCMainContainer.Name = "sCMainContainer";
      // 
      // sCMainContainer.Panel1
      // 
      this.sCMainContainer.Panel1.Controls.Add(this.gBConversations);
      this.sCMainContainer.Panel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 2);
      this.sCMainContainer.Panel1MinSize = 290;
      // 
      // sCMainContainer.Panel2
      // 
      this.sCMainContainer.Panel2.Controls.Add(this.requestResponseList);
      this.sCMainContainer.Panel2.ImeMode = System.Windows.Forms.ImeMode.On;
      this.sCMainContainer.Panel2MinSize = 580;
      this.sCMainContainer.Size = new System.Drawing.Size(1344, 810);
      this.sCMainContainer.SplitterDistance = 351;
      this.sCMainContainer.SplitterWidth = 3;
      this.sCMainContainer.TabIndex = 9;
      this.sCMainContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.sCMainContainer_SplitterMoved);
      // 
      // gBConversations
      // 
      this.gBConversations.Controls.Add(this.lVConversations);
      this.gBConversations.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBConversations.Location = new System.Drawing.Point(3, 4);
      this.gBConversations.Margin = new System.Windows.Forms.Padding(4);
      this.gBConversations.Name = "gBConversations";
      this.gBConversations.Padding = new System.Windows.Forms.Padding(4);
      this.gBConversations.Size = new System.Drawing.Size(341, 800);
      this.gBConversations.TabIndex = 7;
      this.gBConversations.TabStop = false;
      this.gBConversations.Text = "List of Conversations";
      // 
      // lVConversations
      // 
      this.lVConversations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
      this.lVConversations.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lVConversations.FullRowSelect = true;
      this.lVConversations.GridLines = true;
      this.lVConversations.HideSelection = false;
      this.lVConversations.Location = new System.Drawing.Point(4, 19);
      this.lVConversations.Margin = new System.Windows.Forms.Padding(4);
      this.lVConversations.Name = "lVConversations";
      this.lVConversations.Size = new System.Drawing.Size(333, 777);
      this.lVConversations.TabIndex = 0;
      this.lVConversations.UseCompatibleStateImageBehavior = false;
      this.lVConversations.View = System.Windows.Forms.View.Details;
      this.lVConversations.SelectedIndexChanged += new System.EventHandler(this.lVConversations_SelectedIndexChanged);
      // 
      // columnHeader5
      // 
      this.columnHeader5.Tag = "3";
      this.columnHeader5.Text = "#";
      this.columnHeader5.Width = 38;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Tag = "30";
      this.columnHeader6.Text = "Name";
      this.columnHeader6.Width = 149;
      // 
      // requestResponseList
      // 
      this.requestResponseList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.requestResponseList.Location = new System.Drawing.Point(0, 0);
      this.requestResponseList.Margin = new System.Windows.Forms.Padding(0);
      this.requestResponseList.Name = "requestResponseList";
      this.requestResponseList.SelectedConversation = null;
      this.requestResponseList.Size = new System.Drawing.Size(986, 806);
      this.requestResponseList.TabIndex = 8;
      // 
      // btnSaveConversationReport
      // 
      this.btnSaveConversationReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSaveConversationReport.AutoSize = true;
      this.btnSaveConversationReport.Location = new System.Drawing.Point(1166, 817);
      this.btnSaveConversationReport.Margin = new System.Windows.Forms.Padding(4);
      this.btnSaveConversationReport.Name = "btnSaveConversationReport";
      this.btnSaveConversationReport.Size = new System.Drawing.Size(174, 28);
      this.btnSaveConversationReport.TabIndex = 4;
      this.btnSaveConversationReport.Text = "Save Reporting Log";
      this.btnSaveConversationReport.UseVisualStyleBackColor = true;
      this.btnSaveConversationReport.Click += new System.EventHandler(this.btnSaveReportingLog_Click);
      // 
      // sFDReportingReport
      // 
      this.sFDReportingReport.DefaultExt = "xml";
      this.sFDReportingReport.Filter = "xml|*.xml";
      // 
      // PageReporting
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.sCMainContainer);
      this.Controls.Add(this.btnSaveConversationReport);
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "PageReporting";
      this.Size = new System.Drawing.Size(1344, 849);
      this.Load += new System.EventHandler(this.ReportingPage_Load);
      this.VisibleChanged += new System.EventHandler(this.pageReporting_VisibleChanged);
      this.sCMainContainer.Panel1.ResumeLayout(false);
      this.sCMainContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCMainContainer)).EndInit();
      this.sCMainContainer.ResumeLayout(false);
      this.gBConversations.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSaveConversationReport;
    private System.Windows.Forms.GroupBox gBConversations;
    private System.Windows.Forms.ListView lVConversations;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private RequestResponseListView requestResponseList;
    private System.Windows.Forms.SplitContainer sCMainContainer;
    private System.Windows.Forms.SaveFileDialog sFDReportingReport;
  }
}
