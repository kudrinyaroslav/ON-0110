namespace TestTool.GUI.Controls
{
    partial class ConformanceTestPage
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
            this.gbTestExecutionInformation = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbOrganizationAddress = new System.Windows.Forms.TextBox();
            this.lblOrganizationAddress = new System.Windows.Forms.Label();
            this.lblOrganizationName = new System.Windows.Forms.Label();
            this.tbOrganizationName = new System.Windows.Forms.TextBox();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.tbOperatorName = new System.Windows.Forms.TextBox();
            this.gbDutInformation = new System.Windows.Forms.GroupBox();
            this.btnAddProductType = new System.Windows.Forms.Button();
            this.lblProductTypeOther = new System.Windows.Forms.Label();
            this.lblStar4 = new System.Windows.Forms.Label();
            this.lblStar7 = new System.Windows.Forms.Label();
            this.clbProductType = new System.Windows.Forms.CheckedListBox();
            this.cmsProductTypeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblProductType = new System.Windows.Forms.Label();
            this.tbProductName = new System.Windows.Forms.TextBox();
            this.lblproductName = new System.Windows.Forms.Label();
            this.btnClearDeviceInformation = new System.Windows.Forms.Button();
            this.lblOtherInformation = new System.Windows.Forms.Label();
            this.tbOtherInformation = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.tbProductTypeOther = new System.Windows.Forms.TextBox();
            this.setupPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnClearSupportInformation = new System.Windows.Forms.Button();
            this.btnClearMemberInfo = new System.Windows.Forms.Button();
            this.gbRunTest = new System.Windows.Forms.GroupBox();
            this.btnDatasheetReport = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnDoCReport = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblTestExecutionMessage = new System.Windows.Forms.Label();
            this.pbTestsExecution = new System.Windows.Forms.ProgressBar();
            this.btnRun = new System.Windows.Forms.Button();
            this.gbResponsible = new System.Windows.Forms.GroupBox();
            this.lblStar6 = new System.Windows.Forms.Label();
            this.lblStar5 = new System.Windows.Forms.Label();
            this.tbMemberAddress = new System.Windows.Forms.TextBox();
            this.tbMemberName = new System.Windows.Forms.TextBox();
            this.lblMemberAddress = new System.Windows.Forms.Label();
            this.lblMemberName = new System.Windows.Forms.Label();
            this.gbSupportInformation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbInternationalAddress = new System.Windows.Forms.TextBox();
            this.tbRegionalAddress = new System.Windows.Forms.TextBox();
            this.lblRegionalAddress = new System.Windows.Forms.Label();
            this.lblInternationalAddress = new System.Windows.Forms.Label();
            this.lblStar3 = new System.Windows.Forms.Label();
            this.tbSupportPhone = new System.Windows.Forms.TextBox();
            this.tbSupportEmail = new System.Windows.Forms.TextBox();
            this.tbSupportWebsite = new System.Windows.Forms.TextBox();
            this.lblSupportPhone = new System.Windows.Forms.Label();
            this.lblSupportEmail = new System.Windows.Forms.Label();
            this.lblSupportWebsite = new System.Windows.Forms.Label();
            this.lblStar2 = new System.Windows.Forms.Label();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.gbTestExecutionInformation.SuspendLayout();
            this.gbDutInformation.SuspendLayout();
            this.cmsProductTypeContextMenu.SuspendLayout();
            this.gbRunTest.SuspendLayout();
            this.gbResponsible.SuspendLayout();
            this.gbSupportInformation.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTestExecutionInformation
            // 
            this.gbTestExecutionInformation.Controls.Add(this.btnClear);
            this.gbTestExecutionInformation.Controls.Add(this.tbOrganizationAddress);
            this.gbTestExecutionInformation.Controls.Add(this.lblOrganizationAddress);
            this.gbTestExecutionInformation.Controls.Add(this.lblOrganizationName);
            this.gbTestExecutionInformation.Controls.Add(this.tbOrganizationName);
            this.gbTestExecutionInformation.Controls.Add(this.lblOperatorName);
            this.gbTestExecutionInformation.Controls.Add(this.tbOperatorName);
            this.gbTestExecutionInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTestExecutionInformation.Location = new System.Drawing.Point(0, 203);
            this.gbTestExecutionInformation.Name = "gbTestExecutionInformation";
            this.gbTestExecutionInformation.Size = new System.Drawing.Size(842, 143);
            this.gbTestExecutionInformation.TabIndex = 20;
            this.gbTestExecutionInformation.TabStop = false;
            this.gbTestExecutionInformation.Text = "Test Execution Information";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(761, 114);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClear, "Clear tests execution information");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbOrganizationAddress
            // 
            this.tbOrganizationAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrganizationAddress.Location = new System.Drawing.Point(176, 71);
            this.tbOrganizationAddress.Multiline = true;
            this.tbOrganizationAddress.Name = "tbOrganizationAddress";
            this.tbOrganizationAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOrganizationAddress.Size = new System.Drawing.Size(579, 66);
            this.tbOrganizationAddress.TabIndex = 23;
            // 
            // lblOrganizationAddress
            // 
            this.lblOrganizationAddress.AutoSize = true;
            this.lblOrganizationAddress.Location = new System.Drawing.Point(14, 95);
            this.lblOrganizationAddress.Name = "lblOrganizationAddress";
            this.lblOrganizationAddress.Size = new System.Drawing.Size(160, 13);
            this.lblOrganizationAddress.TabIndex = 11;
            this.lblOrganizationAddress.Text = "Executing Organization Address:";
            // 
            // lblOrganizationName
            // 
            this.lblOrganizationName.AutoSize = true;
            this.lblOrganizationName.Location = new System.Drawing.Point(14, 48);
            this.lblOrganizationName.Name = "lblOrganizationName";
            this.lblOrganizationName.Size = new System.Drawing.Size(150, 13);
            this.lblOrganizationName.TabIndex = 9;
            this.lblOrganizationName.Text = "Executing Organization Name:";
            // 
            // tbOrganizationName
            // 
            this.tbOrganizationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrganizationName.Location = new System.Drawing.Point(176, 45);
            this.tbOrganizationName.Name = "tbOrganizationName";
            this.tbOrganizationName.Size = new System.Drawing.Size(579, 20);
            this.tbOrganizationName.TabIndex = 22;
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.AutoSize = true;
            this.lblOperatorName.Location = new System.Drawing.Point(14, 22);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(106, 13);
            this.lblOperatorName.TabIndex = 7;
            this.lblOperatorName.Text = "Test Operator Name:";
            // 
            // tbOperatorName
            // 
            this.tbOperatorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOperatorName.Location = new System.Drawing.Point(176, 19);
            this.tbOperatorName.Name = "tbOperatorName";
            this.tbOperatorName.Size = new System.Drawing.Size(579, 20);
            this.tbOperatorName.TabIndex = 21;
            // 
            // gbDutInformation
            // 
            this.gbDutInformation.Controls.Add(this.btnAddProductType);
            this.gbDutInformation.Controls.Add(this.lblProductTypeOther);
            this.gbDutInformation.Controls.Add(this.lblStar4);
            this.gbDutInformation.Controls.Add(this.lblStar7);
            this.gbDutInformation.Controls.Add(this.clbProductType);
            this.gbDutInformation.Controls.Add(this.lblProductType);
            this.gbDutInformation.Controls.Add(this.tbProductName);
            this.gbDutInformation.Controls.Add(this.lblproductName);
            this.gbDutInformation.Controls.Add(this.btnClearDeviceInformation);
            this.gbDutInformation.Controls.Add(this.lblOtherInformation);
            this.gbDutInformation.Controls.Add(this.tbOtherInformation);
            this.gbDutInformation.Controls.Add(this.lblModel);
            this.gbDutInformation.Controls.Add(this.tbModel);
            this.gbDutInformation.Controls.Add(this.lblBrand);
            this.gbDutInformation.Controls.Add(this.tbBrand);
            this.gbDutInformation.Controls.Add(this.tbProductTypeOther);
            this.gbDutInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDutInformation.Location = new System.Drawing.Point(0, 77);
            this.gbDutInformation.Name = "gbDutInformation";
            this.gbDutInformation.Size = new System.Drawing.Size(842, 126);
            this.gbDutInformation.TabIndex = 10;
            this.gbDutInformation.TabStop = false;
            this.gbDutInformation.Text = "Device Under Test Information";
            // 
            // btnAddProductType
            // 
            this.btnAddProductType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddProductType.Location = new System.Drawing.Point(625, 59);
            this.btnAddProductType.Name = "btnAddProductType";
            this.btnAddProductType.Size = new System.Drawing.Size(50, 23);
            this.btnAddProductType.TabIndex = 25;
            this.btnAddProductType.Text = "<< Add";
            this.setupPageToolTip.SetToolTip(this.btnAddProductType, "Add the other product type to general list");
            this.btnAddProductType.UseVisualStyleBackColor = true;
            this.btnAddProductType.Click += new System.EventHandler(this.btnAddProductType_Click);
            // 
            // lblProductTypeOther
            // 
            this.lblProductTypeOther.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProductTypeOther.AutoSize = true;
            this.lblProductTypeOther.Location = new System.Drawing.Point(622, 18);
            this.lblProductTypeOther.Name = "lblProductTypeOther";
            this.lblProductTypeOther.Size = new System.Drawing.Size(107, 13);
            this.lblProductTypeOther.TabIndex = 23;
            this.lblProductTypeOther.Text = "Product Type (other):";
            // 
            // lblStar4
            // 
            this.lblStar4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStar4.AutoSize = true;
            this.lblStar4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar4.ForeColor = System.Drawing.Color.Red;
            this.lblStar4.Location = new System.Drawing.Point(387, 18);
            this.lblStar4.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar4.Name = "lblStar4";
            this.lblStar4.Size = new System.Drawing.Size(13, 17);
            this.lblStar4.TabIndex = 22;
            this.lblStar4.Text = "*";
            // 
            // lblStar7
            // 
            this.lblStar7.AutoSize = true;
            this.lblStar7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar7.ForeColor = System.Drawing.Color.Red;
            this.lblStar7.Location = new System.Drawing.Point(3, 22);
            this.lblStar7.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar7.Name = "lblStar7";
            this.lblStar7.Size = new System.Drawing.Size(13, 17);
            this.lblStar7.TabIndex = 21;
            this.lblStar7.Text = "*";
            // 
            // clbProductType
            // 
            this.clbProductType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbProductType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbProductType.CheckOnClick = true;
            this.clbProductType.ColumnWidth = 65;
            this.clbProductType.ContextMenuStrip = this.cmsProductTypeContextMenu;
            this.clbProductType.FormattingEnabled = true;
            this.clbProductType.HorizontalScrollbar = true;
            this.clbProductType.Items.AddRange(new object[] {
            "Access Controller",
            "Access Controller Gateway",
            "Access Control Management System",
            "Decoder",
            "Encoder",
            "Fixed Camera",
            "PTZ Camera",
            "Recorder",
            "Video Management System"});
            this.clbProductType.Location = new System.Drawing.Point(401, 37);
            this.clbProductType.Name = "clbProductType";
            this.clbProductType.Size = new System.Drawing.Size(215, 77);
            this.clbProductType.TabIndex = 20;
            // 
            // cmsProductTypeContextMenu
            // 
            this.cmsProductTypeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.cmsProductTypeContextMenu.Name = "cmsProductTypeContextMenu";
            this.cmsProductTypeContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmsProductTypeContextMenu.ShowImageMargin = false;
            this.cmsProductTypeContextMenu.Size = new System.Drawing.Size(93, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // lblProductType
            // 
            this.lblProductType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProductType.AutoSize = true;
            this.lblProductType.Location = new System.Drawing.Point(398, 18);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(74, 13);
            this.lblProductType.TabIndex = 19;
            this.lblProductType.Text = "Product Type:";
            // 
            // tbProductName
            // 
            this.tbProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProductName.Location = new System.Drawing.Point(176, 19);
            this.tbProductName.Name = "tbProductName";
            this.tbProductName.Size = new System.Drawing.Size(205, 20);
            this.tbProductName.TabIndex = 11;
            // 
            // lblproductName
            // 
            this.lblproductName.AutoSize = true;
            this.lblproductName.Location = new System.Drawing.Point(14, 22);
            this.lblproductName.Name = "lblproductName";
            this.lblproductName.Size = new System.Drawing.Size(78, 13);
            this.lblproductName.TabIndex = 18;
            this.lblproductName.Text = "Product Name:";
            // 
            // btnClearDeviceInformation
            // 
            this.btnClearDeviceInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDeviceInformation.Location = new System.Drawing.Point(761, 94);
            this.btnClearDeviceInformation.Name = "btnClearDeviceInformation";
            this.btnClearDeviceInformation.Size = new System.Drawing.Size(75, 23);
            this.btnClearDeviceInformation.TabIndex = 17;
            this.btnClearDeviceInformation.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClearDeviceInformation, "Clear device information");
            this.btnClearDeviceInformation.UseVisualStyleBackColor = true;
            this.btnClearDeviceInformation.Click += new System.EventHandler(this.btnClearDeviceInformation_Click);
            // 
            // lblOtherInformation
            // 
            this.lblOtherInformation.AutoSize = true;
            this.lblOtherInformation.Location = new System.Drawing.Point(14, 100);
            this.lblOtherInformation.Name = "lblOtherInformation";
            this.lblOtherInformation.Size = new System.Drawing.Size(91, 13);
            this.lblOtherInformation.TabIndex = 15;
            this.lblOtherInformation.Text = "Other Information:";
            // 
            // tbOtherInformation
            // 
            this.tbOtherInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOtherInformation.Location = new System.Drawing.Point(176, 97);
            this.tbOtherInformation.Name = "tbOtherInformation";
            this.tbOtherInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOtherInformation.Size = new System.Drawing.Size(205, 20);
            this.tbOtherInformation.TabIndex = 14;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(14, 74);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 9;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModel.Location = new System.Drawing.Point(176, 71);
            this.tbModel.Name = "tbModel";
            this.tbModel.ReadOnly = true;
            this.tbModel.Size = new System.Drawing.Size(205, 20);
            this.tbModel.TabIndex = 13;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(14, 48);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 7;
            this.lblBrand.Text = "Brand:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBrand.Location = new System.Drawing.Point(176, 45);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.ReadOnly = true;
            this.tbBrand.Size = new System.Drawing.Size(205, 20);
            this.tbBrand.TabIndex = 12;
            // 
            // tbProductTypeOther
            // 
            this.tbProductTypeOther.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tbProductTypeOther.Location = new System.Drawing.Point(625, 37);
            this.tbProductTypeOther.Name = "tbProductTypeOther";
            this.tbProductTypeOther.Size = new System.Drawing.Size(130, 20);
            this.tbProductTypeOther.TabIndex = 24;
            this.setupPageToolTip.SetToolTip(this.tbProductTypeOther, "Enter the other product type");
            // 
            // setupPageToolTip
            // 
            this.setupPageToolTip.AutoPopDelay = 5000;
            this.setupPageToolTip.InitialDelay = 200;
            this.setupPageToolTip.ReshowDelay = 100;
            // 
            // btnClearSupportInformation
            // 
            this.btnClearSupportInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSupportInformation.Location = new System.Drawing.Point(761, 165);
            this.btnClearSupportInformation.Name = "btnClearSupportInformation";
            this.btnClearSupportInformation.Size = new System.Drawing.Size(75, 23);
            this.btnClearSupportInformation.TabIndex = 10;
            this.btnClearSupportInformation.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClearSupportInformation, "Clear support information");
            this.btnClearSupportInformation.UseVisualStyleBackColor = true;
            this.btnClearSupportInformation.Click += new System.EventHandler(this.btnClearSupportInformation_Click);
            // 
            // btnClearMemberInfo
            // 
            this.btnClearMemberInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearMemberInfo.Location = new System.Drawing.Point(761, 45);
            this.btnClearMemberInfo.Name = "btnClearMemberInfo";
            this.btnClearMemberInfo.Size = new System.Drawing.Size(75, 23);
            this.btnClearMemberInfo.TabIndex = 4;
            this.btnClearMemberInfo.Text = "Clear";
            this.setupPageToolTip.SetToolTip(this.btnClearMemberInfo, "Clear member information");
            this.btnClearMemberInfo.UseVisualStyleBackColor = true;
            this.btnClearMemberInfo.Click += new System.EventHandler(this.btnClearMemberInfo_Click);
            // 
            // gbRunTest
            // 
            this.gbRunTest.Controls.Add(this.btnDatasheetReport);
            this.gbRunTest.Controls.Add(this.btnReport);
            this.gbRunTest.Controls.Add(this.btnDoCReport);
            this.gbRunTest.Controls.Add(this.tbLog);
            this.gbRunTest.Controls.Add(this.lblProgress);
            this.gbRunTest.Controls.Add(this.lblTestExecutionMessage);
            this.gbRunTest.Controls.Add(this.pbTestsExecution);
            this.gbRunTest.Controls.Add(this.btnRun);
            this.gbRunTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRunTest.Location = new System.Drawing.Point(0, 540);
            this.gbRunTest.Name = "gbRunTest";
            this.gbRunTest.Size = new System.Drawing.Size(842, 360);
            this.gbRunTest.TabIndex = 30;
            this.gbRunTest.TabStop = false;
            this.gbRunTest.Text = "Conformance";
            // 
            // btnDatasheetReport
            // 
            this.btnDatasheetReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDatasheetReport.Enabled = false;
            this.btnDatasheetReport.Location = new System.Drawing.Point(313, 327);
            this.btnDatasheetReport.Name = "btnDatasheetReport";
            this.btnDatasheetReport.Size = new System.Drawing.Size(159, 26);
            this.btnDatasheetReport.TabIndex = 3;
            this.btnDatasheetReport.Text = "Generate Feature List";
            this.btnDatasheetReport.UseVisualStyleBackColor = true;
            this.btnDatasheetReport.Click += new System.EventHandler(this.btnDatasheetReport_Click);
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReport.Enabled = false;
            this.btnReport.Location = new System.Drawing.Point(166, 327);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(141, 26);
            this.btnReport.TabIndex = 2;
            this.btnReport.Text = "Generate Test Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnDoCReport
            // 
            this.btnDoCReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDoCReport.Enabled = false;
            this.btnDoCReport.Location = new System.Drawing.Point(6, 327);
            this.btnDoCReport.Name = "btnDoCReport";
            this.btnDoCReport.Size = new System.Drawing.Size(154, 26);
            this.btnDoCReport.TabIndex = 1;
            this.btnDoCReport.Text = "Generate DoC";
            this.btnDoCReport.UseVisualStyleBackColor = true;
            this.btnDoCReport.Click += new System.EventHandler(this.btnDoCReport_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(6, 108);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(830, 214);
            this.tbLog.TabIndex = 5;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Location = new System.Drawing.Point(674, 63);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(162, 13);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTestExecutionMessage
            // 
            this.lblTestExecutionMessage.AutoSize = true;
            this.lblTestExecutionMessage.Location = new System.Drawing.Point(4, 63);
            this.lblTestExecutionMessage.Name = "lblTestExecutionMessage";
            this.lblTestExecutionMessage.Size = new System.Drawing.Size(0, 13);
            this.lblTestExecutionMessage.TabIndex = 2;
            // 
            // pbTestsExecution
            // 
            this.pbTestsExecution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTestsExecution.Location = new System.Drawing.Point(6, 79);
            this.pbTestsExecution.Name = "pbTestsExecution";
            this.pbTestsExecution.Size = new System.Drawing.Size(830, 23);
            this.pbTestsExecution.TabIndex = 10;
            // 
            // btnRun
            // 
            this.btnRun.Image = global::TestTool.GUI.Properties.Resources.RunAll;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRun.Location = new System.Drawing.Point(6, 18);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(154, 26);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Start Conformance Test";
            this.btnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // gbResponsible
            // 
            this.gbResponsible.Controls.Add(this.lblStar6);
            this.gbResponsible.Controls.Add(this.lblStar5);
            this.gbResponsible.Controls.Add(this.btnClearMemberInfo);
            this.gbResponsible.Controls.Add(this.tbMemberAddress);
            this.gbResponsible.Controls.Add(this.tbMemberName);
            this.gbResponsible.Controls.Add(this.lblMemberAddress);
            this.gbResponsible.Controls.Add(this.lblMemberName);
            this.gbResponsible.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbResponsible.Location = new System.Drawing.Point(0, 0);
            this.gbResponsible.Name = "gbResponsible";
            this.gbResponsible.Size = new System.Drawing.Size(842, 77);
            this.gbResponsible.TabIndex = 1;
            this.gbResponsible.TabStop = false;
            this.gbResponsible.Text = "Responsible Member";
            // 
            // lblStar6
            // 
            this.lblStar6.AutoSize = true;
            this.lblStar6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar6.ForeColor = System.Drawing.Color.Red;
            this.lblStar6.Location = new System.Drawing.Point(3, 47);
            this.lblStar6.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar6.Name = "lblStar6";
            this.lblStar6.Size = new System.Drawing.Size(13, 17);
            this.lblStar6.TabIndex = 15;
            this.lblStar6.Text = "*";
            // 
            // lblStar5
            // 
            this.lblStar5.AutoSize = true;
            this.lblStar5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar5.ForeColor = System.Drawing.Color.Red;
            this.lblStar5.Location = new System.Drawing.Point(3, 23);
            this.lblStar5.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar5.Name = "lblStar5";
            this.lblStar5.Size = new System.Drawing.Size(13, 17);
            this.lblStar5.TabIndex = 14;
            this.lblStar5.Text = "*";
            // 
            // tbMemberAddress
            // 
            this.tbMemberAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberAddress.Location = new System.Drawing.Point(176, 47);
            this.tbMemberAddress.Name = "tbMemberAddress";
            this.tbMemberAddress.Size = new System.Drawing.Size(579, 20);
            this.tbMemberAddress.TabIndex = 3;
            // 
            // tbMemberName
            // 
            this.tbMemberName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberName.Location = new System.Drawing.Point(176, 20);
            this.tbMemberName.Name = "tbMemberName";
            this.tbMemberName.Size = new System.Drawing.Size(579, 20);
            this.tbMemberName.TabIndex = 2;
            // 
            // lblMemberAddress
            // 
            this.lblMemberAddress.AutoSize = true;
            this.lblMemberAddress.Location = new System.Drawing.Point(14, 47);
            this.lblMemberAddress.Name = "lblMemberAddress";
            this.lblMemberAddress.Size = new System.Drawing.Size(89, 13);
            this.lblMemberAddress.TabIndex = 1;
            this.lblMemberAddress.Text = "Member Address:";
            // 
            // lblMemberName
            // 
            this.lblMemberName.AutoSize = true;
            this.lblMemberName.Location = new System.Drawing.Point(14, 23);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(79, 13);
            this.lblMemberName.TabIndex = 0;
            this.lblMemberName.Text = "Member Name:";
            // 
            // gbSupportInformation
            // 
            this.gbSupportInformation.Controls.Add(this.tableLayoutPanel1);
            this.gbSupportInformation.Controls.Add(this.btnClearSupportInformation);
            this.gbSupportInformation.Controls.Add(this.tbSupportPhone);
            this.gbSupportInformation.Controls.Add(this.tbSupportEmail);
            this.gbSupportInformation.Controls.Add(this.tbSupportWebsite);
            this.gbSupportInformation.Controls.Add(this.lblSupportPhone);
            this.gbSupportInformation.Controls.Add(this.lblSupportEmail);
            this.gbSupportInformation.Controls.Add(this.lblSupportWebsite);
            this.gbSupportInformation.Controls.Add(this.lblStar2);
            this.gbSupportInformation.Controls.Add(this.lblStar1);
            this.gbSupportInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSupportInformation.Location = new System.Drawing.Point(0, 346);
            this.gbSupportInformation.Name = "gbSupportInformation";
            this.gbSupportInformation.Size = new System.Drawing.Size(842, 194);
            this.gbSupportInformation.TabIndex = 23;
            this.gbSupportInformation.TabStop = false;
            this.gbSupportInformation.Text = "Technical Support Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tbInternationalAddress, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbRegionalAddress, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRegionalAddress, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblInternationalAddress, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStar3, 0, 0);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 99);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(752, 90);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // tbInternationalAddress
            // 
            this.tbInternationalAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInternationalAddress.Location = new System.Drawing.Point(13, 20);
            this.tbInternationalAddress.Multiline = true;
            this.tbInternationalAddress.Name = "tbInternationalAddress";
            this.tbInternationalAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInternationalAddress.Size = new System.Drawing.Size(365, 67);
            this.tbInternationalAddress.TabIndex = 5;
            // 
            // tbRegionalAddress
            // 
            this.tbRegionalAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRegionalAddress.Location = new System.Drawing.Point(384, 20);
            this.tbRegionalAddress.Multiline = true;
            this.tbRegionalAddress.Name = "tbRegionalAddress";
            this.tbRegionalAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRegionalAddress.Size = new System.Drawing.Size(365, 67);
            this.tbRegionalAddress.TabIndex = 6;
            // 
            // lblRegionalAddress
            // 
            this.lblRegionalAddress.AutoSize = true;
            this.lblRegionalAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRegionalAddress.Location = new System.Drawing.Point(384, 0);
            this.lblRegionalAddress.Name = "lblRegionalAddress";
            this.lblRegionalAddress.Size = new System.Drawing.Size(169, 13);
            this.lblRegionalAddress.TabIndex = 1;
            this.lblRegionalAddress.Text = "Regional support contact address:";
            // 
            // lblInternationalAddress
            // 
            this.lblInternationalAddress.AutoSize = true;
            this.lblInternationalAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblInternationalAddress.Location = new System.Drawing.Point(13, 0);
            this.lblInternationalAddress.Name = "lblInternationalAddress";
            this.lblInternationalAddress.Size = new System.Drawing.Size(224, 13);
            this.lblInternationalAddress.TabIndex = 0;
            this.lblInternationalAddress.Text = "General international support contact address:";
            // 
            // lblStar3
            // 
            this.lblStar3.AutoSize = true;
            this.lblStar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar3.ForeColor = System.Drawing.Color.Red;
            this.lblStar3.Location = new System.Drawing.Point(0, 0);
            this.lblStar3.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar3.Name = "lblStar3";
            this.lblStar3.Size = new System.Drawing.Size(10, 17);
            this.lblStar3.TabIndex = 14;
            this.lblStar3.Text = "*";
            // 
            // tbSupportPhone
            // 
            this.tbSupportPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSupportPhone.Location = new System.Drawing.Point(176, 68);
            this.tbSupportPhone.Name = "tbSupportPhone";
            this.tbSupportPhone.Size = new System.Drawing.Size(579, 20);
            this.tbSupportPhone.TabIndex = 9;
            // 
            // tbSupportEmail
            // 
            this.tbSupportEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSupportEmail.Location = new System.Drawing.Point(176, 44);
            this.tbSupportEmail.Name = "tbSupportEmail";
            this.tbSupportEmail.Size = new System.Drawing.Size(579, 20);
            this.tbSupportEmail.TabIndex = 8;
            // 
            // tbSupportWebsite
            // 
            this.tbSupportWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSupportWebsite.Location = new System.Drawing.Point(176, 20);
            this.tbSupportWebsite.Name = "tbSupportWebsite";
            this.tbSupportWebsite.Size = new System.Drawing.Size(579, 20);
            this.tbSupportWebsite.TabIndex = 7;
            // 
            // lblSupportPhone
            // 
            this.lblSupportPhone.AutoSize = true;
            this.lblSupportPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSupportPhone.Location = new System.Drawing.Point(14, 69);
            this.lblSupportPhone.Name = "lblSupportPhone";
            this.lblSupportPhone.Size = new System.Drawing.Size(128, 13);
            this.lblSupportPhone.TabIndex = 4;
            this.lblSupportPhone.Text = "Technical support phone:";
            // 
            // lblSupportEmail
            // 
            this.lblSupportEmail.AutoSize = true;
            this.lblSupportEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSupportEmail.Location = new System.Drawing.Point(14, 46);
            this.lblSupportEmail.Name = "lblSupportEmail";
            this.lblSupportEmail.Size = new System.Drawing.Size(125, 13);
            this.lblSupportEmail.TabIndex = 3;
            this.lblSupportEmail.Text = "Technical support e-mail:";
            // 
            // lblSupportWebsite
            // 
            this.lblSupportWebsite.AutoSize = true;
            this.lblSupportWebsite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSupportWebsite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSupportWebsite.Location = new System.Drawing.Point(14, 23);
            this.lblSupportWebsite.Margin = new System.Windows.Forms.Padding(0);
            this.lblSupportWebsite.Name = "lblSupportWebsite";
            this.lblSupportWebsite.Size = new System.Drawing.Size(159, 13);
            this.lblSupportWebsite.TabIndex = 2;
            this.lblSupportWebsite.Text = "Technical support website URL:";
            // 
            // lblStar2
            // 
            this.lblStar2.AutoSize = true;
            this.lblStar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar2.ForeColor = System.Drawing.Color.Red;
            this.lblStar2.Location = new System.Drawing.Point(3, 69);
            this.lblStar2.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar2.Name = "lblStar2";
            this.lblStar2.Size = new System.Drawing.Size(13, 17);
            this.lblStar2.TabIndex = 13;
            this.lblStar2.Text = "*";
            // 
            // lblStar1
            // 
            this.lblStar1.AutoSize = true;
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(3, 23);
            this.lblStar1.Margin = new System.Windows.Forms.Padding(0);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(13, 17);
            this.lblStar1.TabIndex = 12;
            this.lblStar1.Text = "*";
            // 
            // ConformanceTestPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbRunTest);
            this.Controls.Add(this.gbSupportInformation);
            this.Controls.Add(this.gbTestExecutionInformation);
            this.Controls.Add(this.gbDutInformation);
            this.Controls.Add(this.gbResponsible);
            this.Name = "ConformanceTestPage";
            this.Size = new System.Drawing.Size(842, 900);
            this.gbTestExecutionInformation.ResumeLayout(false);
            this.gbTestExecutionInformation.PerformLayout();
            this.gbDutInformation.ResumeLayout(false);
            this.gbDutInformation.PerformLayout();
            this.cmsProductTypeContextMenu.ResumeLayout(false);
            this.gbRunTest.ResumeLayout(false);
            this.gbRunTest.PerformLayout();
            this.gbResponsible.ResumeLayout(false);
            this.gbResponsible.PerformLayout();
            this.gbSupportInformation.ResumeLayout(false);
            this.gbSupportInformation.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTestExecutionInformation;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbOrganizationAddress;
        private System.Windows.Forms.Label lblOrganizationAddress;
        private System.Windows.Forms.Label lblOrganizationName;
        private System.Windows.Forms.TextBox tbOrganizationName;
        private System.Windows.Forms.Label lblOperatorName;
        private System.Windows.Forms.TextBox tbOperatorName;
        private System.Windows.Forms.GroupBox gbDutInformation;
        private System.Windows.Forms.Button btnClearDeviceInformation;
        //private System.Windows.Forms.Button btnGetDeviceInformation;
        private System.Windows.Forms.Label lblOtherInformation;
        private System.Windows.Forms.TextBox tbOtherInformation;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox tbModel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.TextBox tbBrand;
        private System.Windows.Forms.ToolTip setupPageToolTip;
        private System.Windows.Forms.GroupBox gbRunTest;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Label lblTestExecutionMessage;
        private System.Windows.Forms.ProgressBar pbTestsExecution;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnDoCReport;
        private System.Windows.Forms.Button btnDatasheetReport;
        private System.Windows.Forms.Label lblproductName;
        private System.Windows.Forms.TextBox tbProductName;
        private System.Windows.Forms.GroupBox gbResponsible;
        private System.Windows.Forms.Label lblMemberAddress;
        private System.Windows.Forms.Label lblMemberName;
        private System.Windows.Forms.TextBox tbMemberAddress;
        private System.Windows.Forms.TextBox tbMemberName;
        private System.Windows.Forms.Button btnClearMemberInfo;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckedListBox clbProductType;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.GroupBox gbSupportInformation;
        private System.Windows.Forms.Label lblSupportPhone;
        private System.Windows.Forms.Label lblSupportEmail;
        private System.Windows.Forms.Label lblSupportWebsite;
        private System.Windows.Forms.Label lblRegionalAddress;
        private System.Windows.Forms.Label lblInternationalAddress;
        private System.Windows.Forms.TextBox tbSupportPhone;
        private System.Windows.Forms.TextBox tbSupportEmail;
        private System.Windows.Forms.TextBox tbSupportWebsite;
        private System.Windows.Forms.TextBox tbRegionalAddress;
        private System.Windows.Forms.TextBox tbInternationalAddress;
        private System.Windows.Forms.Button btnClearSupportInformation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblStar1;
        private System.Windows.Forms.Label lblStar2;
        private System.Windows.Forms.Label lblStar3;
        private System.Windows.Forms.Label lblStar6;
        private System.Windows.Forms.Label lblStar5;
        private System.Windows.Forms.Label lblStar7;
        private System.Windows.Forms.Label lblStar4;
        private System.Windows.Forms.TextBox tbProductTypeOther;
        private System.Windows.Forms.Label lblProductTypeOther;
        private System.Windows.Forms.Button btnAddProductType;
        private System.Windows.Forms.ContextMenuStrip cmsProductTypeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}
