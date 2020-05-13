using System.Windows.Forms;
using ClientTestTool.GUI.Controls.Configuration.DataGridViewProgressBar;

namespace ClientTestTool.GUI.Controls.Configuration
{
  partial class PageConfiguration
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageConfiguration));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.label10 = new System.Windows.Forms.Label();
      this.tBPathNetworkTrace = new System.Windows.Forms.TextBox();
      this.btnStartTesting = new System.Windows.Forms.Button();
      this.btnAddNetworkTrace = new System.Windows.Forms.Button();
      this.lVUnits = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.oFDNetworkTrace = new System.Windows.Forms.OpenFileDialog();
      this.gBInputFiles = new System.Windows.Forms.GroupBox();
      this.dGVNetworkTraces = new System.Windows.Forms.DataGridView();
      this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
      this.gBParsingResults = new System.Windows.Forms.GroupBox();
      this.gBIdentifiedUnits = new System.Windows.Forms.GroupBox();
      this.gBUnitInfo = new System.Windows.Forms.GroupBox();
      this.btnBrowseFeatureList = new System.Windows.Forms.Button();
      this.tBSerialNumber = new System.Windows.Forms.TextBox();
      this.tBFirmware = new System.Windows.Forms.TextBox();
      this.tBModel = new System.Windows.Forms.TextBox();
      this.tBManufacturer = new System.Windows.Forms.TextBox();
      this.tBFeatureList = new System.Windows.Forms.TextBox();
      this.tBNetworkTrace = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.oFDFeatureList = new System.Windows.Forms.OpenFileDialog();
      this.sCMain = new System.Windows.Forms.SplitContainer();
      this.gBInputFiles.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dGVNetworkTraces)).BeginInit();
      this.gBParsingResults.SuspendLayout();
      this.gBIdentifiedUnits.SuspendLayout();
      this.gBUnitInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).BeginInit();
      this.sCMain.Panel1.SuspendLayout();
      this.sCMain.Panel2.SuspendLayout();
      this.sCMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(8, 20);
      this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(180, 17);
      this.label10.TabIndex = 18;
      this.label10.Text = "Network Trace Capture file:";
      // 
      // tBPathNetworkTrace
      // 
      this.tBPathNetworkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBPathNetworkTrace.Location = new System.Drawing.Point(221, 16);
      this.tBPathNetworkTrace.Margin = new System.Windows.Forms.Padding(4);
      this.tBPathNetworkTrace.Name = "tBPathNetworkTrace";
      this.tBPathNetworkTrace.Size = new System.Drawing.Size(256, 22);
      this.tBPathNetworkTrace.TabIndex = 0;
      // 
      // btnStartTesting
      // 
      this.btnStartTesting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnStartTesting.Enabled = false;
      this.btnStartTesting.Image = ((System.Drawing.Image)(resources.GetObject("btnStartTesting.Image")));
      this.btnStartTesting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnStartTesting.Location = new System.Drawing.Point(429, 811);
      this.btnStartTesting.Margin = new System.Windows.Forms.Padding(4);
      this.btnStartTesting.Name = "btnStartTesting";
      this.btnStartTesting.Size = new System.Drawing.Size(157, 28);
      this.btnStartTesting.TabIndex = 1;
      this.btnStartTesting.Text = "Start Parsing";
      this.btnStartTesting.UseVisualStyleBackColor = true;
      this.btnStartTesting.Click += new System.EventHandler(this.btnStartTesting_Click);
      // 
      // btnAddNetworkTrace
      // 
      this.btnAddNetworkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddNetworkTrace.AutoEllipsis = true;
      this.btnAddNetworkTrace.Location = new System.Drawing.Point(487, 14);
      this.btnAddNetworkTrace.Margin = new System.Windows.Forms.Padding(4);
      this.btnAddNetworkTrace.Name = "btnAddNetworkTrace";
      this.btnAddNetworkTrace.Size = new System.Drawing.Size(100, 27);
      this.btnAddNetworkTrace.TabIndex = 22;
      this.btnAddNetworkTrace.TabStop = false;
      this.btnAddNetworkTrace.Text = "Add...";
      this.btnAddNetworkTrace.UseVisualStyleBackColor = true;
      this.btnAddNetworkTrace.Click += new System.EventHandler(this.btnAddNetworkTrace_Click);
      // 
      // lVUnits
      // 
      this.lVUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lVUnits.CheckBoxes = true;
      this.lVUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7});
      this.lVUnits.FullRowSelect = true;
      this.lVUnits.GridLines = true;
      this.lVUnits.HideSelection = false;
      this.lVUnits.Location = new System.Drawing.Point(4, 20);
      this.lVUnits.Margin = new System.Windows.Forms.Padding(4);
      this.lVUnits.Name = "lVUnits";
      this.lVUnits.ShowItemToolTips = true;
      this.lVUnits.Size = new System.Drawing.Size(692, 561);
      this.lVUnits.TabIndex = 27;
      this.lVUnits.UseCompatibleStateImageBehavior = false;
      this.lVUnits.View = System.Windows.Forms.View.Details;
      this.lVUnits.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lVUnits_ItemCheck);
      this.lVUnits.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lVUnits_ItemChecked);
      this.lVUnits.SelectedIndexChanged += new System.EventHandler(this.lVUnits_SelectedIndexChanged);
      this.lVUnits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lVUnits_MouseClick);
      this.lVUnits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lVUnits_MouseDown);
      this.lVUnits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lVUnits_MouseUp);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Tag = "10";
      this.columnHeader1.Text = "Enabled";
      this.columnHeader1.Width = 52;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Tag = "5";
      this.columnHeader2.Text = "#";
      this.columnHeader2.Width = 30;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Tag = "15";
      this.columnHeader3.Text = "Name";
      this.columnHeader3.Width = 120;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Tag = "15";
      this.columnHeader6.Text = "MAC";
      // 
      // columnHeader4
      // 
      this.columnHeader4.Tag = "15";
      this.columnHeader4.Text = "IP";
      this.columnHeader4.Width = 100;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Tag = "20";
      this.columnHeader5.Text = "Attached Feature List";
      this.columnHeader5.Width = 114;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Tag = "10";
      this.columnHeader7.Text = "Type";
      this.columnHeader7.Width = 73;
      // 
      // oFDNetworkTrace
      // 
      this.oFDNetworkTrace.Filter = "Network Trace Files|*.pcapng;*.pcap|All files|*.*";
      this.oFDNetworkTrace.Multiselect = true;
      // 
      // gBInputFiles
      // 
      this.gBInputFiles.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
      this.gBInputFiles.Controls.Add(this.dGVNetworkTraces);
      this.gBInputFiles.Controls.Add(this.btnStartTesting);
      this.gBInputFiles.Controls.Add(this.btnAddNetworkTrace);
      this.gBInputFiles.Controls.Add(this.label10);
      this.gBInputFiles.Controls.Add(this.tBPathNetworkTrace);
      this.gBInputFiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBInputFiles.Location = new System.Drawing.Point(3, 2);
      this.gBInputFiles.Margin = new System.Windows.Forms.Padding(4);
      this.gBInputFiles.Name = "gBInputFiles";
      this.gBInputFiles.Padding = new System.Windows.Forms.Padding(4);
      this.gBInputFiles.Size = new System.Drawing.Size(595, 847);
      this.gBInputFiles.TabIndex = 28;
      this.gBInputFiles.TabStop = false;
      this.gBInputFiles.Text = "Input Files";
      // 
      // dGVNetworkTraces
      // 
      this.dGVNetworkTraces.AllowUserToAddRows = false;
      this.dGVNetworkTraces.AllowUserToResizeRows = false;
      this.dGVNetworkTraces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dGVNetworkTraces.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dGVNetworkTraces.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dGVNetworkTraces.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dGVNetworkTraces.DefaultCellStyle = dataGridViewCellStyle3;
      this.dGVNetworkTraces.Location = new System.Drawing.Point(8, 48);
      this.dGVNetworkTraces.Margin = new System.Windows.Forms.Padding(4);
      this.dGVNetworkTraces.Name = "dGVNetworkTraces";
      this.dGVNetworkTraces.RowHeadersVisible = false;
      this.dGVNetworkTraces.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      this.dGVNetworkTraces.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dGVNetworkTraces.Size = new System.Drawing.Size(579, 755);
      this.dGVNetworkTraces.TabIndex = 27;
      this.dGVNetworkTraces.TabStop = false;
      this.dGVNetworkTraces.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellContentClick);
      this.dGVNetworkTraces.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridView_RowsAdded);
      this.dGVNetworkTraces.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridView_RowsRemoved);
      this.dGVNetworkTraces.SelectionChanged += new System.EventHandler(this.dGVNetworkTraces_SelectionChanged);
      // 
      // Column1
      // 
      this.Column1.HeaderText = "#";
      this.Column1.Name = "Column1";
      this.Column1.ReadOnly = true;
      this.Column1.Width = 40;
      // 
      // Column2
      // 
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
      this.Column2.FillWeight = 130F;
      this.Column2.HeaderText = "File Name";
      this.Column2.Name = "Column2";
      this.Column2.ReadOnly = true;
      this.Column2.Width = 155;
      // 
      // Column3
      // 
      this.Column3.HeaderText = "Size";
      this.Column3.Name = "Column3";
      this.Column3.ReadOnly = true;
      this.Column3.Width = 80;
      // 
      // Column4
      // 
      this.Column4.FillWeight = 85F;
      this.Column4.HeaderText = "Status";
      this.Column4.Name = "Column4";
      this.Column4.ReadOnly = true;
      this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.Column4.Width = 85;
      // 
      // Column5
      // 
      this.Column5.HeaderText = "Delete";
      this.Column5.Image = ((System.Drawing.Image)(resources.GetObject("Column5.Image")));
      this.Column5.Name = "Column5";
      this.Column5.ReadOnly = true;
      this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Column5.Width = 45;
      // 
      // gBParsingResults
      // 
      this.gBParsingResults.Controls.Add(this.gBIdentifiedUnits);
      this.gBParsingResults.Controls.Add(this.gBUnitInfo);
      this.gBParsingResults.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gBParsingResults.Location = new System.Drawing.Point(3, 2);
      this.gBParsingResults.Margin = new System.Windows.Forms.Padding(4);
      this.gBParsingResults.Name = "gBParsingResults";
      this.gBParsingResults.Padding = new System.Windows.Forms.Padding(4);
      this.gBParsingResults.Size = new System.Drawing.Size(716, 847);
      this.gBParsingResults.TabIndex = 29;
      this.gBParsingResults.TabStop = false;
      this.gBParsingResults.Text = "Parsing Results";
      // 
      // gBIdentifiedUnits
      // 
      this.gBIdentifiedUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gBIdentifiedUnits.Controls.Add(this.lVUnits);
      this.gBIdentifiedUnits.Location = new System.Drawing.Point(8, 23);
      this.gBIdentifiedUnits.Margin = new System.Windows.Forms.Padding(4);
      this.gBIdentifiedUnits.Name = "gBIdentifiedUnits";
      this.gBIdentifiedUnits.Padding = new System.Windows.Forms.Padding(4);
      this.gBIdentifiedUnits.Size = new System.Drawing.Size(701, 586);
      this.gBIdentifiedUnits.TabIndex = 29;
      this.gBIdentifiedUnits.TabStop = false;
      this.gBIdentifiedUnits.Text = "Identified Units";
      // 
      // gBUnitInfo
      // 
      this.gBUnitInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gBUnitInfo.Controls.Add(this.btnBrowseFeatureList);
      this.gBUnitInfo.Controls.Add(this.tBSerialNumber);
      this.gBUnitInfo.Controls.Add(this.tBFirmware);
      this.gBUnitInfo.Controls.Add(this.tBModel);
      this.gBUnitInfo.Controls.Add(this.tBManufacturer);
      this.gBUnitInfo.Controls.Add(this.tBFeatureList);
      this.gBUnitInfo.Controls.Add(this.tBNetworkTrace);
      this.gBUnitInfo.Controls.Add(this.label7);
      this.gBUnitInfo.Controls.Add(this.label5);
      this.gBUnitInfo.Controls.Add(this.label4);
      this.gBUnitInfo.Controls.Add(this.label3);
      this.gBUnitInfo.Controls.Add(this.label8);
      this.gBUnitInfo.Controls.Add(this.label1);
      this.gBUnitInfo.Location = new System.Drawing.Point(8, 616);
      this.gBUnitInfo.Margin = new System.Windows.Forms.Padding(4);
      this.gBUnitInfo.Name = "gBUnitInfo";
      this.gBUnitInfo.Padding = new System.Windows.Forms.Padding(4);
      this.gBUnitInfo.Size = new System.Drawing.Size(701, 226);
      this.gBUnitInfo.TabIndex = 28;
      this.gBUnitInfo.TabStop = false;
      this.gBUnitInfo.Text = "Unit Information";
      // 
      // btnBrowseFeatureList
      // 
      this.btnBrowseFeatureList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBrowseFeatureList.Enabled = false;
      this.btnBrowseFeatureList.Location = new System.Drawing.Point(593, 21);
      this.btnBrowseFeatureList.Margin = new System.Windows.Forms.Padding(4);
      this.btnBrowseFeatureList.Name = "btnBrowseFeatureList";
      this.btnBrowseFeatureList.Size = new System.Drawing.Size(100, 28);
      this.btnBrowseFeatureList.TabIndex = 2;
      this.btnBrowseFeatureList.TabStop = false;
      this.btnBrowseFeatureList.Text = "Browse";
      this.btnBrowseFeatureList.UseVisualStyleBackColor = true;
      this.btnBrowseFeatureList.Click += new System.EventHandler(this.btnBrowseFeatureList_Click);
      // 
      // tBSerialNumber
      // 
      this.tBSerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBSerialNumber.Location = new System.Drawing.Point(221, 154);
      this.tBSerialNumber.Margin = new System.Windows.Forms.Padding(4);
      this.tBSerialNumber.Name = "tBSerialNumber";
      this.tBSerialNumber.ReadOnly = true;
      this.tBSerialNumber.Size = new System.Drawing.Size(470, 22);
      this.tBSerialNumber.TabIndex = 6;
      // 
      // tBFirmware
      // 
      this.tBFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBFirmware.Location = new System.Drawing.Point(221, 187);
      this.tBFirmware.Margin = new System.Windows.Forms.Padding(4);
      this.tBFirmware.Name = "tBFirmware";
      this.tBFirmware.ReadOnly = true;
      this.tBFirmware.Size = new System.Drawing.Size(470, 22);
      this.tBFirmware.TabIndex = 6;
      // 
      // tBModel
      // 
      this.tBModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBModel.Location = new System.Drawing.Point(221, 122);
      this.tBModel.Margin = new System.Windows.Forms.Padding(4);
      this.tBModel.Name = "tBModel";
      this.tBModel.ReadOnly = true;
      this.tBModel.Size = new System.Drawing.Size(470, 22);
      this.tBModel.TabIndex = 5;
      // 
      // tBManufacturer
      // 
      this.tBManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBManufacturer.Location = new System.Drawing.Point(221, 90);
      this.tBManufacturer.Margin = new System.Windows.Forms.Padding(4);
      this.tBManufacturer.Name = "tBManufacturer";
      this.tBManufacturer.ReadOnly = true;
      this.tBManufacturer.Size = new System.Drawing.Size(470, 22);
      this.tBManufacturer.TabIndex = 4;
      // 
      // tBFeatureList
      // 
      this.tBFeatureList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBFeatureList.Location = new System.Drawing.Point(221, 23);
      this.tBFeatureList.Margin = new System.Windows.Forms.Padding(4);
      this.tBFeatureList.Name = "tBFeatureList";
      this.tBFeatureList.ReadOnly = true;
      this.tBFeatureList.Size = new System.Drawing.Size(364, 22);
      this.tBFeatureList.TabIndex = 2;
      // 
      // tBNetworkTrace
      // 
      this.tBNetworkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tBNetworkTrace.Location = new System.Drawing.Point(221, 55);
      this.tBNetworkTrace.Margin = new System.Windows.Forms.Padding(4);
      this.tBNetworkTrace.Name = "tBNetworkTrace";
      this.tBNetworkTrace.ReadOnly = true;
      this.tBNetworkTrace.Size = new System.Drawing.Size(470, 22);
      this.tBNetworkTrace.TabIndex = 3;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(17, 158);
      this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(102, 17);
      this.label7.TabIndex = 0;
      this.label7.Text = "Serial Number:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(17, 191);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(69, 17);
      this.label5.TabIndex = 0;
      this.label5.Text = "Firmware:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(17, 126);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(50, 17);
      this.label4.TabIndex = 0;
      this.label4.Text = "Model:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(17, 94);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(96, 17);
      this.label3.TabIndex = 0;
      this.label3.Text = "Manufacturer:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(17, 27);
      this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(119, 17);
      this.label8.TabIndex = 0;
      this.label8.Text = "Feature List XML:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(17, 60);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(108, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Found in Trace:";
      // 
      // oFDFeatureList
      // 
      this.oFDFeatureList.Filter = "Feature List XML|*.xml|All files|*.*";
      // 
      // sCMain
      // 
      this.sCMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.sCMain.Location = new System.Drawing.Point(0, 0);
      this.sCMain.Margin = new System.Windows.Forms.Padding(4);
      this.sCMain.Name = "sCMain";
      // 
      // sCMain.Panel1
      // 
      this.sCMain.Panel1.Controls.Add(this.gBInputFiles);
      this.sCMain.Panel1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.sCMain.Panel1MinSize = 400;
      // 
      // sCMain.Panel2
      // 
      this.sCMain.Panel2.Controls.Add(this.gBParsingResults);
      this.sCMain.Panel2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.sCMain.Panel2MinSize = 500;
      this.sCMain.Size = new System.Drawing.Size(1336, 855);
      this.sCMain.SplitterDistance = 605;
      this.sCMain.SplitterWidth = 5;
      this.sCMain.TabIndex = 30;
      this.sCMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.sCMain_SplitterMoved);
      // 
      // PageConfiguration
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.sCMain);
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "PageConfiguration";
      this.Size = new System.Drawing.Size(1336, 855);
      this.Load += new System.EventHandler(this.ConfigurationPage_Load);
      this.gBInputFiles.ResumeLayout(false);
      this.gBInputFiles.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dGVNetworkTraces)).EndInit();
      this.gBParsingResults.ResumeLayout(false);
      this.gBIdentifiedUnits.ResumeLayout(false);
      this.gBUnitInfo.ResumeLayout(false);
      this.gBUnitInfo.PerformLayout();
      this.sCMain.Panel1.ResumeLayout(false);
      this.sCMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).EndInit();
      this.sCMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox tBPathNetworkTrace;
    private System.Windows.Forms.Button btnStartTesting;
    private System.Windows.Forms.Button btnAddNetworkTrace;
    private System.Windows.Forms.ListView lVUnits;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.OpenFileDialog oFDNetworkTrace;
    private System.Windows.Forms.GroupBox gBInputFiles;
    private System.Windows.Forms.GroupBox gBParsingResults;
    private System.Windows.Forms.GroupBox gBUnitInfo;
    private System.Windows.Forms.TextBox tBSerialNumber;
    private System.Windows.Forms.TextBox tBFirmware;
    private System.Windows.Forms.TextBox tBModel;
    private System.Windows.Forms.TextBox tBManufacturer;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.GroupBox gBIdentifiedUnits;
    private System.Windows.Forms.DataGridView dGVNetworkTraces;
    private System.Windows.Forms.OpenFileDialog oFDFeatureList;
    private System.Windows.Forms.Button btnBrowseFeatureList;
    private System.Windows.Forms.TextBox tBFeatureList;
    private System.Windows.Forms.TextBox tBNetworkTrace;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private SplitContainer sCMain;
    private DataGridViewTextBoxColumn Column1;
    private DataGridViewTextBoxColumn Column2;
    private DataGridViewTextBoxColumn Column3;
    private DataGridViewTextBoxColumn Column4;
    private DataGridViewImageColumn Column5;


  }
}
