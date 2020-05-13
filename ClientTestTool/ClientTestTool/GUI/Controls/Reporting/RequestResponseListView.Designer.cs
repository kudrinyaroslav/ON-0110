namespace ClientTestTool.GUI.Controls.Reporting
{
  partial class RequestResponseListView
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
      this.gBRequestsResponses = new System.Windows.Forms.GroupBox();
      this.cBFilter = new System.Windows.Forms.ComboBox();
      this.lVRequestResponse = new System.Windows.Forms.ListView();
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.tLPRequestResponse = new System.Windows.Forms.TableLayoutPanel();
      this.gBResponse = new System.Windows.Forms.GroupBox();
      this.tBResponse = new System.Windows.Forms.RichTextBox();
      this.gBRequest = new System.Windows.Forms.GroupBox();
      this.tBRequest = new System.Windows.Forms.RichTextBox();
      this.gBDetails = new System.Windows.Forms.GroupBox();
      this.tBDetails = new System.Windows.Forms.RichTextBox();
      this.tLPPairsInfo = new System.Windows.Forms.TableLayoutPanel();
      this.sCMain = new System.Windows.Forms.SplitContainer();
      this.gBRequestsResponses.SuspendLayout();
      this.tLPRequestResponse.SuspendLayout();
      this.gBResponse.SuspendLayout();
      this.gBRequest.SuspendLayout();
      this.gBDetails.SuspendLayout();
      this.tLPPairsInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).BeginInit();
      this.sCMain.Panel1.SuspendLayout();
      this.sCMain.Panel2.SuspendLayout();
      this.sCMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // gBRequestsResponses
      // 
      this.gBRequestsResponses.Controls.Add(this.cBFilter);
      this.gBRequestsResponses.Controls.Add(this.lVRequestResponse);
      this.gBRequestsResponses.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBRequestsResponses.Location = new System.Drawing.Point(3, 3);
      this.gBRequestsResponses.Name = "gBRequestsResponses";
      this.gBRequestsResponses.Size = new System.Drawing.Size(644, 140);
      this.gBRequestsResponses.TabIndex = 17;
      this.gBRequestsResponses.TabStop = false;
      this.gBRequestsResponses.Text = "Requests && Responses";
      // 
      // cBFilter
      // 
      this.cBFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cBFilter.FormattingEnabled = true;
      this.cBFilter.Items.AddRange(new object[] {
            "All",
            "Issues",
            "Covered Features"});
      this.cBFilter.Location = new System.Drawing.Point(517, 16);
      this.cBFilter.Name = "cBFilter";
      this.cBFilter.Size = new System.Drawing.Size(121, 21);
      this.cBFilter.TabIndex = 1;
      this.cBFilter.Visible = false;
      // 
      // lVRequestResponse
      // 
      this.lVRequestResponse.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
      this.lVRequestResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lVRequestResponse.FullRowSelect = true;
      this.lVRequestResponse.GridLines = true;
      this.lVRequestResponse.HideSelection = false;
      this.lVRequestResponse.Location = new System.Drawing.Point(3, 16);
      this.lVRequestResponse.Name = "lVRequestResponse";
      this.lVRequestResponse.Size = new System.Drawing.Size(638, 121);
      this.lVRequestResponse.TabIndex = 1;
      this.lVRequestResponse.UseCompatibleStateImageBehavior = false;
      this.lVRequestResponse.View = System.Windows.Forms.View.Details;
      this.lVRequestResponse.SelectedIndexChanged += new System.EventHandler(this.lVRequestResponse_SelectedIndexChanged);
      // 
      // columnHeader9
      // 
      this.columnHeader9.Tag = "7";
      this.columnHeader9.Text = "Pair #";
      // 
      // columnHeader10
      // 
      this.columnHeader10.Tag = "45";
      this.columnHeader10.Text = "Result";
      this.columnHeader10.Width = 273;
      // 
      // columnHeader11
      // 
      this.columnHeader11.Tag = "45";
      this.columnHeader11.Text = "Identified Command";
      this.columnHeader11.Width = 337;
      // 
      // tLPRequestResponse
      // 
      this.tLPRequestResponse.ColumnCount = 2;
      this.tLPRequestResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tLPRequestResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tLPRequestResponse.Controls.Add(this.gBResponse, 1, 0);
      this.tLPRequestResponse.Controls.Add(this.gBRequest, 0, 0);
      this.tLPRequestResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tLPRequestResponse.Location = new System.Drawing.Point(0, 0);
      this.tLPRequestResponse.Name = "tLPRequestResponse";
      this.tLPRequestResponse.RowCount = 1;
      this.tLPRequestResponse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tLPRequestResponse.Size = new System.Drawing.Size(650, 273);
      this.tLPRequestResponse.TabIndex = 16;
      // 
      // gBResponse
      // 
      this.gBResponse.Controls.Add(this.tBResponse);
      this.gBResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBResponse.Location = new System.Drawing.Point(328, 3);
      this.gBResponse.Name = "gBResponse";
      this.gBResponse.Size = new System.Drawing.Size(319, 267);
      this.gBResponse.TabIndex = 11;
      this.gBResponse.TabStop = false;
      this.gBResponse.Text = "Response";
      // 
      // tBResponse
      // 
      this.tBResponse.DetectUrls = false;
      this.tBResponse.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBResponse.Location = new System.Drawing.Point(3, 16);
      this.tBResponse.Name = "tBResponse";
      this.tBResponse.ReadOnly = true;
      this.tBResponse.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.tBResponse.Size = new System.Drawing.Size(313, 248);
      this.tBResponse.TabIndex = 0;
      this.tBResponse.Text = "";
      // 
      // gBRequest
      // 
      this.gBRequest.Controls.Add(this.tBRequest);
      this.gBRequest.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBRequest.Location = new System.Drawing.Point(3, 3);
      this.gBRequest.Name = "gBRequest";
      this.gBRequest.Size = new System.Drawing.Size(319, 267);
      this.gBRequest.TabIndex = 12;
      this.gBRequest.TabStop = false;
      this.gBRequest.Text = "Request";
      // 
      // tBRequest
      // 
      this.tBRequest.DetectUrls = false;
      this.tBRequest.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBRequest.Location = new System.Drawing.Point(3, 16);
      this.tBRequest.Name = "tBRequest";
      this.tBRequest.ReadOnly = true;
      this.tBRequest.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.tBRequest.Size = new System.Drawing.Size(313, 248);
      this.tBRequest.TabIndex = 0;
      this.tBRequest.Text = "";
      // 
      // gBDetails
      // 
      this.gBDetails.Controls.Add(this.tBDetails);
      this.gBDetails.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBDetails.Location = new System.Drawing.Point(3, 149);
      this.gBDetails.Name = "gBDetails";
      this.gBDetails.Size = new System.Drawing.Size(644, 98);
      this.gBDetails.TabIndex = 15;
      this.gBDetails.TabStop = false;
      this.gBDetails.Text = "Validation Information";
      // 
      // tBDetails
      // 
      this.tBDetails.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tBDetails.Location = new System.Drawing.Point(3, 16);
      this.tBDetails.Name = "tBDetails";
      this.tBDetails.ReadOnly = true;
      this.tBDetails.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.tBDetails.Size = new System.Drawing.Size(638, 79);
      this.tBDetails.TabIndex = 0;
      this.tBDetails.Text = "";
      // 
      // tLPPairsInfo
      // 
      this.tLPPairsInfo.ColumnCount = 1;
      this.tLPPairsInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tLPPairsInfo.Controls.Add(this.gBDetails, 0, 1);
      this.tLPPairsInfo.Controls.Add(this.gBRequestsResponses, 0, 0);
      this.tLPPairsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tLPPairsInfo.Location = new System.Drawing.Point(0, 0);
      this.tLPPairsInfo.Name = "tLPPairsInfo";
      this.tLPPairsInfo.RowCount = 2;
      this.tLPPairsInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tLPPairsInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
      this.tLPPairsInfo.Size = new System.Drawing.Size(650, 250);
      this.tLPPairsInfo.TabIndex = 18;
      // 
      // sCMain
      // 
      this.sCMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.sCMain.Location = new System.Drawing.Point(0, 0);
      this.sCMain.Name = "sCMain";
      this.sCMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // sCMain.Panel1
      // 
      this.sCMain.Panel1.Controls.Add(this.tLPPairsInfo);
      // 
      // sCMain.Panel2
      // 
      this.sCMain.Panel2.Controls.Add(this.tLPRequestResponse);
      this.sCMain.Size = new System.Drawing.Size(654, 534);
      this.sCMain.SplitterDistance = 254;
      this.sCMain.SplitterWidth = 3;
      this.sCMain.TabIndex = 19;
      // 
      // RequestResponseList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.sCMain);
      this.Name = "RequestResponseList";
      this.Size = new System.Drawing.Size(654, 534);
      this.gBRequestsResponses.ResumeLayout(false);
      this.tLPRequestResponse.ResumeLayout(false);
      this.gBResponse.ResumeLayout(false);
      this.gBRequest.ResumeLayout(false);
      this.gBDetails.ResumeLayout(false);
      this.tLPPairsInfo.ResumeLayout(false);
      this.sCMain.Panel1.ResumeLayout(false);
      this.sCMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).EndInit();
      this.sCMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.ColumnHeader columnHeader11;
    private System.Windows.Forms.ListView lVRequestResponse;
    private System.Windows.Forms.TableLayoutPanel tLPRequestResponse;
    private System.Windows.Forms.GroupBox gBResponse;
    private System.Windows.Forms.RichTextBox tBResponse;
    private System.Windows.Forms.GroupBox gBRequest;
    private System.Windows.Forms.RichTextBox tBRequest;
    private System.Windows.Forms.GroupBox gBDetails;
    private System.Windows.Forms.RichTextBox tBDetails;
    private System.Windows.Forms.GroupBox gBRequestsResponses;
    private System.Windows.Forms.ComboBox cBFilter;
    private System.Windows.Forms.TableLayoutPanel tLPPairsInfo;
    private System.Windows.Forms.SplitContainer sCMain;



  }
}
