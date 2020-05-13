using ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  partial class PageDiagnostics
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
      this.btnSave = new System.Windows.Forms.Button();
      this.sCMain = new System.Windows.Forms.SplitContainer();
      this.tCModes = new System.Windows.Forms.TabControl();
      this.tPTestCases = new System.Windows.Forms.TabPage();
      this.testsTree = new ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.TestsTree();
      this.tBFeatures = new System.Windows.Forms.TabPage();
      this.featuresTree = new ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.FeaturesTree();
      this.tBProfiles = new System.Windows.Forms.TabPage();
      this.profilesTree = new ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView.ProfilesTree();
      this.tCLog = new System.Windows.Forms.TabControl();
      this.tPOutput = new System.Windows.Forms.TabPage();
      this.vTestOutput = new ClientTestTool.GUI.Controls.Diagnostics.TestOutputView();
      this.tPDetails = new System.Windows.Forms.TabPage();
      this.vTestDetails = new ClientTestTool.GUI.Controls.Diagnostics.TestDetailsView();
      this.gBControls = new System.Windows.Forms.GroupBox();
      this.btnClear = new System.Windows.Forms.Button();
      this.btnRun = new System.Windows.Forms.Button();
      this.sFDDiagnosticsReport = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).BeginInit();
      this.sCMain.Panel1.SuspendLayout();
      this.sCMain.Panel2.SuspendLayout();
      this.sCMain.SuspendLayout();
      this.tCModes.SuspendLayout();
      this.tPTestCases.SuspendLayout();
      this.tBFeatures.SuspendLayout();
      this.tBProfiles.SuspendLayout();
      this.tCLog.SuspendLayout();
      this.tPOutput.SuspendLayout();
      this.tPDetails.SuspendLayout();
      this.gBControls.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(1210, 529);
      this.btnSave.Margin = new System.Windows.Forms.Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(166, 28);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save Diagnostics Log";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // sCMain
      // 
      this.sCMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.sCMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.sCMain.Location = new System.Drawing.Point(0, 55);
      this.sCMain.Name = "sCMain";
      // 
      // sCMain.Panel1
      // 
      this.sCMain.Panel1.Controls.Add(this.tCModes);
      this.sCMain.Panel1MinSize = 290;
      // 
      // sCMain.Panel2
      // 
      this.sCMain.Panel2.Controls.Add(this.tCLog);
      this.sCMain.Panel2MinSize = 580;
      this.sCMain.Size = new System.Drawing.Size(1380, 467);
      this.sCMain.SplitterDistance = 500;
      this.sCMain.SplitterWidth = 3;
      this.sCMain.TabIndex = 4;
      this.sCMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.sCMainContainer_SplitterMoved);
      // 
      // tCModes
      // 
      this.tCModes.Alignment = System.Windows.Forms.TabAlignment.Left;
      this.tCModes.CausesValidation = false;
      this.tCModes.Controls.Add(this.tPTestCases);
      this.tCModes.Controls.Add(this.tBFeatures);
      this.tCModes.Controls.Add(this.tBProfiles);
      this.tCModes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tCModes.Location = new System.Drawing.Point(0, 0);
      this.tCModes.Margin = new System.Windows.Forms.Padding(4);
      this.tCModes.Multiline = true;
      this.tCModes.Name = "tCModes";
      this.tCModes.SelectedIndex = 0;
      this.tCModes.Size = new System.Drawing.Size(496, 463);
      this.tCModes.TabIndex = 9;
      this.tCModes.SelectedIndexChanged += new System.EventHandler(this.tCModes_SelectedIndexChanged);
      // 
      // tPTestCases
      // 
      this.tPTestCases.Controls.Add(this.testsTree);
      this.tPTestCases.Location = new System.Drawing.Point(25, 4);
      this.tPTestCases.Margin = new System.Windows.Forms.Padding(4);
      this.tPTestCases.Name = "tPTestCases";
      this.tPTestCases.Size = new System.Drawing.Size(467, 455);
      this.tPTestCases.TabIndex = 2;
      this.tPTestCases.Text = "Test Cases";
      this.tPTestCases.UseVisualStyleBackColor = true;
      // 
      // testsTree
      // 
      this.testsTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.testsTree.Location = new System.Drawing.Point(0, 0);
      this.testsTree.Margin = new System.Windows.Forms.Padding(5);
      this.testsTree.Name = "testsTree";
      this.testsTree.Size = new System.Drawing.Size(467, 455);
      this.testsTree.TabIndex = 1;
      // 
      // tBFeatures
      // 
      this.tBFeatures.Controls.Add(this.featuresTree);
      this.tBFeatures.Location = new System.Drawing.Point(25, 4);
      this.tBFeatures.Margin = new System.Windows.Forms.Padding(4);
      this.tBFeatures.Name = "tBFeatures";
      this.tBFeatures.Padding = new System.Windows.Forms.Padding(4);
      this.tBFeatures.Size = new System.Drawing.Size(467, 455);
      this.tBFeatures.TabIndex = 1;
      this.tBFeatures.Text = "Features";
      this.tBFeatures.UseVisualStyleBackColor = true;
      // 
      // featuresTree
      // 
      this.featuresTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.featuresTree.Location = new System.Drawing.Point(4, 4);
      this.featuresTree.Margin = new System.Windows.Forms.Padding(5);
      this.featuresTree.Name = "featuresTree";
      this.featuresTree.Size = new System.Drawing.Size(459, 447);
      this.featuresTree.TabIndex = 0;
      // 
      // tBProfiles
      // 
      this.tBProfiles.Controls.Add(this.profilesTree);
      this.tBProfiles.Location = new System.Drawing.Point(25, 4);
      this.tBProfiles.Margin = new System.Windows.Forms.Padding(4);
      this.tBProfiles.Name = "tBProfiles";
      this.tBProfiles.Padding = new System.Windows.Forms.Padding(4);
      this.tBProfiles.Size = new System.Drawing.Size(467, 455);
      this.tBProfiles.TabIndex = 0;
      this.tBProfiles.Text = "Profiles";
      this.tBProfiles.UseVisualStyleBackColor = true;
      // 
      // profilesTree
      // 
      this.profilesTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.profilesTree.Location = new System.Drawing.Point(4, 4);
      this.profilesTree.Margin = new System.Windows.Forms.Padding(5);
      this.profilesTree.Name = "profilesTree";
      this.profilesTree.Size = new System.Drawing.Size(459, 447);
      this.profilesTree.TabIndex = 0;
      // 
      // tCLog
      // 
      this.tCLog.Controls.Add(this.tPOutput);
      this.tCLog.Controls.Add(this.tPDetails);
      this.tCLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tCLog.Location = new System.Drawing.Point(0, 0);
      this.tCLog.Margin = new System.Windows.Forms.Padding(4);
      this.tCLog.Name = "tCLog";
      this.tCLog.SelectedIndex = 0;
      this.tCLog.Size = new System.Drawing.Size(873, 463);
      this.tCLog.TabIndex = 2;
      // 
      // tPOutput
      // 
      this.tPOutput.BackColor = System.Drawing.SystemColors.ControlLight;
      this.tPOutput.Controls.Add(this.vTestOutput);
      this.tPOutput.Location = new System.Drawing.Point(4, 25);
      this.tPOutput.Margin = new System.Windows.Forms.Padding(4);
      this.tPOutput.Name = "tPOutput";
      this.tPOutput.Padding = new System.Windows.Forms.Padding(4);
      this.tPOutput.Size = new System.Drawing.Size(865, 434);
      this.tPOutput.TabIndex = 0;
      this.tPOutput.Text = "Output";
      // 
      // vTestOutput
      // 
      this.vTestOutput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.vTestOutput.Location = new System.Drawing.Point(4, 4);
      this.vTestOutput.Name = "vTestOutput";
      this.vTestOutput.Size = new System.Drawing.Size(857, 426);
      this.vTestOutput.TabIndex = 0;
      // 
      // tPDetails
      // 
      this.tPDetails.Controls.Add(this.vTestDetails);
      this.tPDetails.Location = new System.Drawing.Point(4, 25);
      this.tPDetails.Margin = new System.Windows.Forms.Padding(4);
      this.tPDetails.Name = "tPDetails";
      this.tPDetails.Padding = new System.Windows.Forms.Padding(4);
      this.tPDetails.Size = new System.Drawing.Size(865, 434);
      this.tPDetails.TabIndex = 1;
      this.tPDetails.Text = "Test Details";
      this.tPDetails.UseVisualStyleBackColor = true;
      // 
      // vTestDetails
      // 
      this.vTestDetails.Dock = System.Windows.Forms.DockStyle.Fill;
      this.vTestDetails.Location = new System.Drawing.Point(4, 4);
      this.vTestDetails.Name = "vTestDetails";
      this.vTestDetails.SelectedTest = null;
      this.vTestDetails.Size = new System.Drawing.Size(857, 426);
      this.vTestDetails.TabIndex = 0;
      // 
      // gBControls
      // 
      this.gBControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gBControls.Controls.Add(this.btnClear);
      this.gBControls.Controls.Add(this.btnRun);
      this.gBControls.Location = new System.Drawing.Point(4, 0);
      this.gBControls.Margin = new System.Windows.Forms.Padding(4);
      this.gBControls.Name = "gBControls";
      this.gBControls.Padding = new System.Windows.Forms.Padding(4);
      this.gBControls.Size = new System.Drawing.Size(1376, 48);
      this.gBControls.TabIndex = 2;
      this.gBControls.TabStop = false;
      // 
      // btnClear
      // 
      this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClear.FlatAppearance.BorderSize = 0;
      this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnClear.Image = global::ClientTestTool.Properties.Resources.Clear;
      this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnClear.Location = new System.Drawing.Point(1218, 12);
      this.btnClear.Margin = new System.Windows.Forms.Padding(4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(150, 28);
      this.btnClear.TabIndex = 2;
      this.btnClear.Text = "Clear Test Results";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // btnRun
      // 
      this.btnRun.FlatAppearance.BorderSize = 0;
      this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnRun.Image = global::ClientTestTool.Properties.Resources.RunAll;
      this.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnRun.Location = new System.Drawing.Point(8, 12);
      this.btnRun.Margin = new System.Windows.Forms.Padding(4);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(163, 28);
      this.btnRun.TabIndex = 2;
      this.btnRun.Text = "Run Conformance Test";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRunConformance_Click);
      // 
      // sFDDiagnosticsReport
      // 
      this.sFDDiagnosticsReport.DefaultExt = "xml";
      this.sFDDiagnosticsReport.Filter = "xml|*.xml";
      // 
      // PageDiagnostics
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.sCMain);
      this.Controls.Add(this.gBControls);
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "PageDiagnostics";
      this.Size = new System.Drawing.Size(1380, 561);
      this.Load += new System.EventHandler(this.PageDiagnostics_Load);
      this.sCMain.Panel1.ResumeLayout(false);
      this.sCMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.sCMain)).EndInit();
      this.sCMain.ResumeLayout(false);
      this.tCModes.ResumeLayout(false);
      this.tPTestCases.ResumeLayout(false);
      this.tBFeatures.ResumeLayout(false);
      this.tBProfiles.ResumeLayout(false);
      this.tCLog.ResumeLayout(false);
      this.tPOutput.ResumeLayout(false);
      this.tPDetails.ResumeLayout(false);
      this.gBControls.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gBControls;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.TabControl tCModes;
    private System.Windows.Forms.TabPage tPTestCases;
    private System.Windows.Forms.TabPage tBFeatures;
    private System.Windows.Forms.TabPage tBProfiles;
    private System.Windows.Forms.TabControl tCLog;
    private System.Windows.Forms.TabPage tPOutput;
    private System.Windows.Forms.TabPage tPDetails;
    private TestsTree testsTree;
    private FeaturesTree featuresTree;
    private System.Windows.Forms.Button btnClear;
    private ProfilesTree profilesTree;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.SplitContainer sCMain;
    private TestOutputView vTestOutput;
    private TestDetailsView vTestDetails;
    private System.Windows.Forms.SaveFileDialog sFDDiagnosticsReport;
  }
}
