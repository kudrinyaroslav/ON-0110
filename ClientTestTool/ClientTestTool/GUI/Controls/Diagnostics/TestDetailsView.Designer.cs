namespace ClientTestTool.GUI.Controls.Diagnostics
{
  partial class TestDetailsView
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
      this.sCTestDetails = new System.Windows.Forms.SplitContainer();
      this.lVSteps = new System.Windows.Forms.ListView();
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.cBConversations = new ClientTestTool.GUI.Controls.Diagnostics.ConversationComboBox();
      this.tLPRequestResponse = new System.Windows.Forms.TableLayoutPanel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.tBRequest = new ClientTestTool.GUI.Controls.Diagnostics.RichTextBoxEx();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.tBResponse = new ClientTestTool.GUI.Controls.Diagnostics.RichTextBoxEx();
      ((System.ComponentModel.ISupportInitialize)(this.sCTestDetails)).BeginInit();
      this.sCTestDetails.Panel1.SuspendLayout();
      this.sCTestDetails.Panel2.SuspendLayout();
      this.sCTestDetails.SuspendLayout();
      this.tLPRequestResponse.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      // 
      // sCTestDetails
      // 
      this.sCTestDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCTestDetails.Dock = System.Windows.Forms.DockStyle.Fill;
      this.sCTestDetails.Location = new System.Drawing.Point(0, 0);
      this.sCTestDetails.Name = "sCTestDetails";
      this.sCTestDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // sCTestDetails.Panel1
      // 
      this.sCTestDetails.Panel1.Controls.Add(this.lVSteps);
      this.sCTestDetails.Panel1.Controls.Add(this.cBConversations);
      // 
      // sCTestDetails.Panel2
      // 
      this.sCTestDetails.Panel2.Controls.Add(this.tLPRequestResponse);
      this.sCTestDetails.Size = new System.Drawing.Size(863, 680);
      this.sCTestDetails.SplitterDistance = 180;
      this.sCTestDetails.SplitterWidth = 3;
      this.sCTestDetails.TabIndex = 16;
      // 
      // lVSteps
      // 
      this.lVSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lVSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader1});
      this.lVSteps.FullRowSelect = true;
      this.lVSteps.GridLines = true;
      this.lVSteps.HideSelection = false;
      this.lVSteps.Location = new System.Drawing.Point(6, 31);
      this.lVSteps.Margin = new System.Windows.Forms.Padding(4);
      this.lVSteps.Name = "lVSteps";
      this.lVSteps.Size = new System.Drawing.Size(849, 141);
      this.lVSteps.TabIndex = 10;
      this.lVSteps.UseCompatibleStateImageBehavior = false;
      this.lVSteps.View = System.Windows.Forms.View.Details;
      this.lVSteps.SelectedIndexChanged += new System.EventHandler(this.lVSteps_SelectedIndexChanged);
      // 
      // columnHeader9
      // 
      this.columnHeader9.Tag = "7";
      this.columnHeader9.Text = "Step #";
      // 
      // columnHeader10
      // 
      this.columnHeader10.Tag = "30";
      this.columnHeader10.Text = "Step Details";
      this.columnHeader10.Width = 125;
      // 
      // columnHeader11
      // 
      this.columnHeader11.Tag = "10";
      this.columnHeader11.Text = "Result";
      this.columnHeader11.Width = 294;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Tag = "30";
      this.columnHeader1.Text = "Description";
      this.columnHeader1.Width = 362;
      // 
      // cBConversations
      // 
      this.cBConversations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cBConversations.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cBConversations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cBConversations.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
      this.cBConversations.FormattingEnabled = true;
      this.cBConversations.Location = new System.Drawing.Point(7, 3);
      this.cBConversations.Name = "cBConversations";
      this.cBConversations.SelectedTestLog = null;
      this.cBConversations.Size = new System.Drawing.Size(848, 20);
      this.cBConversations.TabIndex = 14;
      this.cBConversations.SelectedIndexChanged += new System.EventHandler(this.cBConversations_SelectedIndexChanged);
      // 
      // tLPRequestResponse
      // 
      this.tLPRequestResponse.ColumnCount = 2;
      this.tLPRequestResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tLPRequestResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tLPRequestResponse.Controls.Add(this.groupBox3, 0, 0);
      this.tLPRequestResponse.Controls.Add(this.groupBox4, 1, 0);
      this.tLPRequestResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tLPRequestResponse.Location = new System.Drawing.Point(0, 0);
      this.tLPRequestResponse.Margin = new System.Windows.Forms.Padding(4);
      this.tLPRequestResponse.Name = "tLPRequestResponse";
      this.tLPRequestResponse.RowCount = 1;
      this.tLPRequestResponse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tLPRequestResponse.Size = new System.Drawing.Size(859, 493);
      this.tLPRequestResponse.TabIndex = 13;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.tBRequest);
      this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox3.Location = new System.Drawing.Point(4, 4);
      this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
      this.groupBox3.Size = new System.Drawing.Size(421, 485);
      this.groupBox3.TabIndex = 12;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Request";
      // 
      // tBRequest
      // 
      this.tBRequest.DetectUrls = false;
      this.tBRequest.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBRequest.Location = new System.Drawing.Point(4, 17);
      this.tBRequest.Margin = new System.Windows.Forms.Padding(4);
      this.tBRequest.Name = "tBRequest";
      this.tBRequest.ReadOnly = true;
      this.tBRequest.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.tBRequest.Size = new System.Drawing.Size(413, 464);
      this.tBRequest.TabIndex = 0;
      this.tBRequest.Text = "";
      this.tBRequest.ScrolledToBottom += new System.EventHandler(this.OnRequestResponseScrolled);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.tBResponse);
      this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox4.Location = new System.Drawing.Point(433, 4);
      this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
      this.groupBox4.Size = new System.Drawing.Size(422, 485);
      this.groupBox4.TabIndex = 11;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Response";
      // 
      // tBResponse
      // 
      this.tBResponse.DetectUrls = false;
      this.tBResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBResponse.Location = new System.Drawing.Point(4, 17);
      this.tBResponse.Margin = new System.Windows.Forms.Padding(4);
      this.tBResponse.Name = "tBResponse";
      this.tBResponse.ReadOnly = true;
      this.tBResponse.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.tBResponse.Size = new System.Drawing.Size(414, 464);
      this.tBResponse.TabIndex = 0;
      this.tBResponse.Text = "";
      this.tBResponse.ScrolledToBottom += new System.EventHandler(this.OnRequestResponseScrolled);
      // 
      // TestDetailsView
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.sCTestDetails);
      this.Name = "TestDetailsView";
      this.Size = new System.Drawing.Size(863, 680);
      this.sCTestDetails.Panel1.ResumeLayout(false);
      this.sCTestDetails.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCTestDetails)).EndInit();
      this.sCTestDetails.ResumeLayout(false);
      this.tLPRequestResponse.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer sCTestDetails;
    private System.Windows.Forms.ListView lVSteps;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.ColumnHeader columnHeader11;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private ConversationComboBox cBConversations;
    private System.Windows.Forms.TableLayoutPanel tLPRequestResponse;
    private System.Windows.Forms.GroupBox groupBox3;
    private RichTextBoxEx tBRequest;
    private System.Windows.Forms.GroupBox groupBox4;
    private RichTextBoxEx tBResponse;
  }
}
