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
            this.tbProductName = new System.Windows.Forms.TextBox();
            this.lblproductName = new System.Windows.Forms.Label();
            this.btnClearDeviceInformation = new System.Windows.Forms.Button();
            //this.btnGetDeviceInformation = new System.Windows.Forms.Button();
            this.lblOtherInformation = new System.Windows.Forms.Label();
            this.tbOtherInformation = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.tbModel = new System.Windows.Forms.TextBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.setupPageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbRunTest = new System.Windows.Forms.GroupBox();
            this.btnDatasheetReport = new System.Windows.Forms.Button();
            this.btnDoCReport = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.lblTestExecutionMessage = new System.Windows.Forms.Label();
            this.pbTestsExecution = new System.Windows.Forms.ProgressBar();
            this.btnRun = new System.Windows.Forms.Button();
            this.gbResponsible = new System.Windows.Forms.GroupBox();
            this.btnClearMemberInfo = new System.Windows.Forms.Button();
            this.tbMemberAddress = new System.Windows.Forms.TextBox();
            this.tbMemberName = new System.Windows.Forms.TextBox();
            this.lblMemberAddress = new System.Windows.Forms.Label();
            this.lblMemberName = new System.Windows.Forms.Label();
            this.gbTestExecutionInformation.SuspendLayout();
            this.gbDutInformation.SuspendLayout();
            this.gbRunTest.SuspendLayout();
            this.gbResponsible.SuspendLayout();
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
            this.tbOrganizationAddress.Location = new System.Drawing.Point(166, 71);
            this.tbOrganizationAddress.Multiline = true;
            this.tbOrganizationAddress.Name = "tbOrganizationAddress";
            this.tbOrganizationAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOrganizationAddress.Size = new System.Drawing.Size(589, 66);
            this.tbOrganizationAddress.TabIndex = 23;
            // 
            // lblOrganizationAddress
            // 
            this.lblOrganizationAddress.AutoSize = true;
            this.lblOrganizationAddress.Location = new System.Drawing.Point(6, 74);
            this.lblOrganizationAddress.Name = "lblOrganizationAddress";
            this.lblOrganizationAddress.Size = new System.Drawing.Size(160, 13);
            this.lblOrganizationAddress.TabIndex = 11;
            this.lblOrganizationAddress.Text = "Executing Organization Address:";
            // 
            // lblOrganizationName
            // 
            this.lblOrganizationName.AutoSize = true;
            this.lblOrganizationName.Location = new System.Drawing.Point(6, 48);
            this.lblOrganizationName.Name = "lblOrganizationName";
            this.lblOrganizationName.Size = new System.Drawing.Size(150, 13);
            this.lblOrganizationName.TabIndex = 9;
            this.lblOrganizationName.Text = "Executing Organization Name:";
            // 
            // tbOrganizationName
            // 
            this.tbOrganizationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrganizationName.Location = new System.Drawing.Point(166, 45);
            this.tbOrganizationName.Name = "tbOrganizationName";
            this.tbOrganizationName.Size = new System.Drawing.Size(589, 20);
            this.tbOrganizationName.TabIndex = 22;
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.AutoSize = true;
            this.lblOperatorName.Location = new System.Drawing.Point(6, 22);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(106, 13);
            this.lblOperatorName.TabIndex = 7;
            this.lblOperatorName.Text = "Test Operator Name:";
            // 
            // tbOperatorName
            // 
            this.tbOperatorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOperatorName.Location = new System.Drawing.Point(166, 19);
            this.tbOperatorName.Name = "tbOperatorName";
            this.tbOperatorName.Size = new System.Drawing.Size(589, 20);
            this.tbOperatorName.TabIndex = 21;
            // 
            // gbDutInformation
            // 
            this.gbDutInformation.Controls.Add(this.tbProductName);
            this.gbDutInformation.Controls.Add(this.lblproductName);
            this.gbDutInformation.Controls.Add(this.btnClearDeviceInformation);
            //this.gbDutInformation.Controls.Add(this.btnGetDeviceInformation);
            this.gbDutInformation.Controls.Add(this.lblOtherInformation);
            this.gbDutInformation.Controls.Add(this.tbOtherInformation);
            this.gbDutInformation.Controls.Add(this.lblModel);
            this.gbDutInformation.Controls.Add(this.tbModel);
            this.gbDutInformation.Controls.Add(this.lblBrand);
            this.gbDutInformation.Controls.Add(this.tbBrand);
            this.gbDutInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDutInformation.Location = new System.Drawing.Point(0, 77);
            this.gbDutInformation.Name = "gbDutInformation";
            this.gbDutInformation.Size = new System.Drawing.Size(842, 126);
            this.gbDutInformation.TabIndex = 10;
            this.gbDutInformation.TabStop = false;
            this.gbDutInformation.Text = "Device Under Test Information";
            // 
            // tbProductName
            // 
            this.tbProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProductName.Location = new System.Drawing.Point(166, 19);
            this.tbProductName.Name = "tbProductName";
            this.tbProductName.Size = new System.Drawing.Size(589, 20);
            this.tbProductName.TabIndex = 11;
            // 
            // lblproductName
            // 
            this.lblproductName.AutoSize = true;
            this.lblproductName.Location = new System.Drawing.Point(6, 22);
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
            // btnGetDeviceInformation
            // 
            //this.btnGetDeviceInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.btnGetDeviceInformation.Location = new System.Drawing.Point(761, 19);
            //this.btnGetDeviceInformation.Name = "btnGetDeviceInformation";
            //this.btnGetDeviceInformation.Size = new System.Drawing.Size(75, 46);
            //this.btnGetDeviceInformation.TabIndex = 16;
            //this.btnGetDeviceInformation.Text = "Get From Device";
            //this.setupPageToolTip.SetToolTip(this.btnGetDeviceInformation, "Get device information from the device");
            //this.btnGetDeviceInformation.UseVisualStyleBackColor = true;
            //this.btnGetDeviceInformation.Visible = false;
            //this.btnGetDeviceInformation.Click += new System.EventHandler(this.btnGetDeviceInformation_Click);
            // 
            // lblOtherInformation
            // 
            this.lblOtherInformation.AutoSize = true;
            this.lblOtherInformation.Location = new System.Drawing.Point(7, 100);
            this.lblOtherInformation.Name = "lblOtherInformation";
            this.lblOtherInformation.Size = new System.Drawing.Size(91, 13);
            this.lblOtherInformation.TabIndex = 15;
            this.lblOtherInformation.Text = "Other Information:";
            // 
            // tbOtherInformation
            // 
            this.tbOtherInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOtherInformation.Location = new System.Drawing.Point(166, 97);
            this.tbOtherInformation.Name = "tbOtherInformation";
            this.tbOtherInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOtherInformation.Size = new System.Drawing.Size(589, 20);
            this.tbOtherInformation.TabIndex = 14;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(6, 74);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(39, 13);
            this.lblModel.TabIndex = 9;
            this.lblModel.Text = "Model:";
            // 
            // tbModel
            // 
            this.tbModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModel.Location = new System.Drawing.Point(166, 71);
            this.tbModel.Name = "tbModel";
            this.tbModel.ReadOnly = true;
            this.tbModel.Size = new System.Drawing.Size(589, 20);
            this.tbModel.TabIndex = 13;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(7, 48);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 7;
            this.lblBrand.Text = "Brand:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBrand.Location = new System.Drawing.Point(166, 45);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.ReadOnly = true;
            this.tbBrand.Size = new System.Drawing.Size(589, 20);
            this.tbBrand.TabIndex = 12;
            // 
            // gbRunTest
            // 
            this.gbRunTest.Controls.Add(this.btnDatasheetReport);
            this.gbRunTest.Controls.Add(this.btnDoCReport);
            this.gbRunTest.Controls.Add(this.tbLog);
            this.gbRunTest.Controls.Add(this.lblProgress);
            this.gbRunTest.Controls.Add(this.btnReport);
            this.gbRunTest.Controls.Add(this.lblTestExecutionMessage);
            this.gbRunTest.Controls.Add(this.pbTestsExecution);
            this.gbRunTest.Controls.Add(this.btnRun);
            this.gbRunTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRunTest.Location = new System.Drawing.Point(0, 346);
            this.gbRunTest.Name = "gbRunTest";
            this.gbRunTest.Size = new System.Drawing.Size(842, 222);
            this.gbRunTest.TabIndex = 22;
            this.gbRunTest.TabStop = false;
            this.gbRunTest.Text = "Conformance";
            // 
            // btnDatasheetReport
            // 
            this.btnDatasheetReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDatasheetReport.Enabled = false;
            this.btnDatasheetReport.Location = new System.Drawing.Point(313, 187);
            this.btnDatasheetReport.Name = "btnDatasheetReport";
            this.btnDatasheetReport.Size = new System.Drawing.Size(159, 26);
            this.btnDatasheetReport.TabIndex = 3;
            this.btnDatasheetReport.Text = "Generate Feature List";
            this.btnDatasheetReport.UseVisualStyleBackColor = true;
            this.btnDatasheetReport.Click += new System.EventHandler(this.btnDatasheetReport_Click);
            // 
            // btnDoCReport
            // 
            this.btnDoCReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDoCReport.Enabled = false;
            this.btnDoCReport.Location = new System.Drawing.Point(6, 187);
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
            this.tbLog.Location = new System.Drawing.Point(6, 119);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(830, 65);
            this.tbLog.TabIndex = 5;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Location = new System.Drawing.Point(674, 74);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(162, 13);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReport.Enabled = false;
            this.btnReport.Location = new System.Drawing.Point(166, 187);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(141, 26);
            this.btnReport.TabIndex = 2;
            this.btnReport.Text = "Generate Test Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lblTestExecutionMessage
            // 
            this.lblTestExecutionMessage.AutoSize = true;
            this.lblTestExecutionMessage.Location = new System.Drawing.Point(6, 74);
            this.lblTestExecutionMessage.Name = "lblTestExecutionMessage";
            this.lblTestExecutionMessage.Size = new System.Drawing.Size(0, 13);
            this.lblTestExecutionMessage.TabIndex = 2;
            // 
            // pbTestsExecution
            // 
            this.pbTestsExecution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTestsExecution.Location = new System.Drawing.Point(6, 90);
            this.pbTestsExecution.Name = "pbTestsExecution";
            this.pbTestsExecution.Size = new System.Drawing.Size(830, 23);
            this.pbTestsExecution.TabIndex = 10;
            // 
            // btnRun
            // 
            this.btnRun.Image = global::TestTool.GUI.Properties.Resources.RunAll;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRun.Location = new System.Drawing.Point(6, 29);
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
            // btnClearMemberInfo
            // 
            this.btnClearMemberInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearMemberInfo.Location = new System.Drawing.Point(761, 45);
            this.btnClearMemberInfo.Name = "btnClearMemberInfo";
            this.btnClearMemberInfo.Size = new System.Drawing.Size(75, 23);
            this.btnClearMemberInfo.TabIndex = 4;
            this.btnClearMemberInfo.Text = "Clear";
            this.btnClearMemberInfo.UseVisualStyleBackColor = true;
            this.btnClearMemberInfo.Click += new System.EventHandler(this.btnClearMemberInfo_Click);
            // 
            // tbMemberAddress
            // 
            this.tbMemberAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberAddress.Location = new System.Drawing.Point(166, 47);
            this.tbMemberAddress.Name = "tbMemberAddress";
            this.tbMemberAddress.Size = new System.Drawing.Size(589, 20);
            this.tbMemberAddress.TabIndex = 3;
            // 
            // tbMemberName
            // 
            this.tbMemberName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemberName.Location = new System.Drawing.Point(166, 20);
            this.tbMemberName.Name = "tbMemberName";
            this.tbMemberName.Size = new System.Drawing.Size(589, 20);
            this.tbMemberName.TabIndex = 2;
            // 
            // lblMemberAddress
            // 
            this.lblMemberAddress.AutoSize = true;
            this.lblMemberAddress.Location = new System.Drawing.Point(9, 48);
            this.lblMemberAddress.Name = "lblMemberAddress";
            this.lblMemberAddress.Size = new System.Drawing.Size(89, 13);
            this.lblMemberAddress.TabIndex = 1;
            this.lblMemberAddress.Text = "Member Address:";
            // 
            // lblMemberName
            // 
            this.lblMemberName.AutoSize = true;
            this.lblMemberName.Location = new System.Drawing.Point(9, 23);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(79, 13);
            this.lblMemberName.TabIndex = 0;
            this.lblMemberName.Text = "Member Name:";
            // 
            // ConformanceTestPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbRunTest);
            this.Controls.Add(this.gbTestExecutionInformation);
            this.Controls.Add(this.gbDutInformation);
            this.Controls.Add(this.gbResponsible);
            this.Name = "ConformanceTestPage";
            this.Size = new System.Drawing.Size(842, 568);
            this.gbTestExecutionInformation.ResumeLayout(false);
            this.gbTestExecutionInformation.PerformLayout();
            this.gbDutInformation.ResumeLayout(false);
            this.gbDutInformation.PerformLayout();
            this.gbRunTest.ResumeLayout(false);
            this.gbRunTest.PerformLayout();
            this.gbResponsible.ResumeLayout(false);
            this.gbResponsible.PerformLayout();
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
        private System.Windows.Forms.Button btnRun;
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
    }
}
