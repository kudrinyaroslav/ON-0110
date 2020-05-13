using System.Drawing;

namespace TestTool.GUI.Controls
{
    partial class TestPage
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
            this.toolStripTestManagement = new System.Windows.Forms.ToolStrip();
            this.tsbRunAll = new System.Windows.Forms.ToolStripButton();
            this.tsbRunCurrent = new System.Windows.Forms.ToolStripSplitButton();
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbRunSelected = new System.Windows.Forms.ToolStripSplitButton();
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbQueryFeatures = new System.Windows.Forms.ToolStripButton();
            this.tsbRepeatTests = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPause = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.tsbHalt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButtonClear = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemClearTestResults = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonSave = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFeatureDefinitionLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tcTestsAndFeatures = new System.Windows.Forms.TabControl();
            this.tpTestCases = new System.Windows.Forms.TabPage();
            this.tvTestCases = new TestTool.GUI.Controls.TestsTree();
            this.tpFeatures = new System.Windows.Forms.TabPage();
            this.featuresTree = new TestTool.GUI.Controls.FeaturesTree();
            this.tpProfiles = new System.Windows.Forms.TabPage();
            this.tvProfiles = new TestTool.GUI.Controls.ProfilesTree();
            this.tcTestResults = new TestTool.GUI.Controls.TestResultsControl();
            this.toolStripTestManagement.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.tcTestsAndFeatures.SuspendLayout();
            this.tpTestCases.SuspendLayout();
            this.tpFeatures.SuspendLayout();
            this.tpProfiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTestManagement
            // 
            this.toolStripTestManagement.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTestManagement.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRunAll,
            this.tsbRunCurrent,
            this.tsbRunSelected,
            this.tsbQueryFeatures,
            this.tsbRepeatTests,
            this.toolStripSeparator1,
            this.tsbPause,
            this.tsbStop,
            this.tsbHalt,
            this.toolStripSeparator2,
            this.toolStripDropDownButtonClear,
            this.toolStripDropDownButtonSave,
            this.toolStripSeparator3});
            this.toolStripTestManagement.Location = new System.Drawing.Point(0, 0);
            this.toolStripTestManagement.Name = "toolStripTestManagement";
            this.toolStripTestManagement.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripTestManagement.Size = new System.Drawing.Size(785, 25);
            this.toolStripTestManagement.TabIndex = 1;
            this.toolStripTestManagement.Text = "toolStripTestManagement";
            // 
            // tsbRunAll
            // 
            this.tsbRunAll.Image = global::TestTool.GUI.Properties.Resources.RunAll;
            this.tsbRunAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunAll.Name = "tsbRunAll";
            this.tsbRunAll.Size = new System.Drawing.Size(48, 22);
            this.tsbRunAll.Text = "Run";
            this.tsbRunAll.ToolTipText = "Run all tests";
            this.tsbRunAll.Click += new System.EventHandler(this.tsbRunAll_Click);
            // 
            // tsbRunCurrent
            // 
            this.tsbRunCurrent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem,
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem});
            this.tsbRunCurrent.Enabled = false;
            this.tsbRunCurrent.Image = global::TestTool.GUI.Properties.Resources.RunCurrent;
            this.tsbRunCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunCurrent.Name = "tsbRunCurrent";
            this.tsbRunCurrent.Size = new System.Drawing.Size(103, 22);
            this.tsbRunCurrent.Text = "Run Current";
            this.tsbRunCurrent.ToolTipText = "Run current test or group of tests";
            this.tsbRunCurrent.ButtonClick += new System.EventHandler(this.tsbRunCurrent_Click);
            this.tsbRunCurrent.DropDownOpening += new System.EventHandler(this.tsbRunCurrent_DropDownOpening);
            // 
            // runCurrentAssumeAllFeaturesSupportedToolStripMenuItem
            // 
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem.Name = "runCurrentAssumeAllFeaturesSupportedToolStripMenuItem";
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem.Text = "Assume all Features Supported";
            this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem.Click += new System.EventHandler(this.runCurrentAssumeAllFeaturesSupportedToolStripMenuItem_Click);
            // 
            // runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem
            // 
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem.Name = "runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem";
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem.Text = "Assume all Features Not Supported";
            this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem.Click += new System.EventHandler(this.runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem_Click);
            // 
            // tsbRunSelected
            // 
            this.tsbRunSelected.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem,
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem});
            this.tsbRunSelected.Enabled = false;
            this.tsbRunSelected.Image = global::TestTool.GUI.Properties.Resources.RunSelected;
            this.tsbRunSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunSelected.Name = "tsbRunSelected";
            this.tsbRunSelected.Size = new System.Drawing.Size(107, 22);
            this.tsbRunSelected.Text = "Run Selected";
            this.tsbRunSelected.ToolTipText = "Run all selected tests";
            this.tsbRunSelected.ButtonClick += new System.EventHandler(this.tsbRunSelected_Click);
            this.tsbRunSelected.DropDownOpening += new System.EventHandler(this.tsbRunSelected_DropDownOpening);
            // 
            // runSelectedAssumeAllFeaturesSupportedToolStripMenuItem
            // 
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem.Name = "runSelectedAssumeAllFeaturesSupportedToolStripMenuItem";
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem.Text = "Assume all Features Supported";
            this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem.Click += new System.EventHandler(this.runSelectedAssumeAllFeaturesSupportedToolStripMenuItem_Click);
            // 
            // runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem
            // 
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem.Name = "runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem";
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem.Text = "Assume all Features Not Supported";
            this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem.Click += new System.EventHandler(this.runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem_Click);
            // 
            // tsbQueryFeatures
            // 
            this.tsbQueryFeatures.Image = global::TestTool.GUI.Properties.Resources.Refresh;
            this.tsbQueryFeatures.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQueryFeatures.Name = "tsbQueryFeatures";
            this.tsbQueryFeatures.Size = new System.Drawing.Size(113, 22);
            this.tsbQueryFeatures.Text = "Refresh Features";
            this.tsbQueryFeatures.ToolTipText = "Query features supported by the DUT";
            this.tsbQueryFeatures.Click += new System.EventHandler(this.tsbQueryFeatures_Click);
            // 
            // tsbRepeatTests
            // 
            this.tsbRepeatTests.AutoSize = false;
            this.tsbRepeatTests.CheckOnClick = true;
            this.tsbRepeatTests.Image = global::TestTool.GUI.Properties.Resources.RunOnce;
            this.tsbRepeatTests.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbRepeatTests.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRepeatTests.Name = "tsbRepeatTests";
            this.tsbRepeatTests.Size = new System.Drawing.Size(78, 22);
            this.tsbRepeatTests.Text = "No Repeat";
            this.tsbRepeatTests.Click += new System.EventHandler(this.tsbRepeatTests_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPause
            // 
            this.tsbPause.Enabled = false;
            this.tsbPause.Image = global::TestTool.GUI.Properties.Resources.Pause;
            this.tsbPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPause.Name = "tsbPause";
            this.tsbPause.Size = new System.Drawing.Size(58, 22);
            this.tsbPause.Text = "Pause";
            this.tsbPause.ToolTipText = "Pause tests execution at IO operation";
            this.tsbPause.Click += new System.EventHandler(this.tsbPause_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.Enabled = false;
            this.tsbStop.Image = global::TestTool.GUI.Properties.Resources.Stop;
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(51, 22);
            this.tsbStop.Text = "Stop";
            this.tsbStop.ToolTipText = "Stop tests execution at the end of test";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // tsbHalt
            // 
            this.tsbHalt.Enabled = false;
            this.tsbHalt.Image = global::TestTool.GUI.Properties.Resources.Halt;
            this.tsbHalt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHalt.Name = "tsbHalt";
            this.tsbHalt.Size = new System.Drawing.Size(49, 22);
            this.tsbHalt.Text = "Halt";
            this.tsbHalt.ToolTipText = "Stop tests execution immediately";
            this.tsbHalt.Click += new System.EventHandler(this.tsbHalt_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButtonClear
            // 
            this.toolStripDropDownButtonClear.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClearTestResults,
            this.toolStripMenuItemClearAll});
            this.toolStripDropDownButtonClear.Image = global::TestTool.GUI.Properties.Resources.Clear;
            this.toolStripDropDownButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonClear.Name = "toolStripDropDownButtonClear";
            this.toolStripDropDownButtonClear.Size = new System.Drawing.Size(63, 22);
            this.toolStripDropDownButtonClear.Text = "Clear";
            // 
            // toolStripMenuItemClearTestResults
            // 
            this.toolStripMenuItemClearTestResults.Image = global::TestTool.GUI.Properties.Resources.ClearTestResults;
            this.toolStripMenuItemClearTestResults.Name = "toolStripMenuItemClearTestResults";
            this.toolStripMenuItemClearTestResults.Size = new System.Drawing.Size(166, 22);
            this.toolStripMenuItemClearTestResults.Text = "Clear Test Results";
            this.toolStripMenuItemClearTestResults.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // toolStripMenuItemClearAll
            // 
            this.toolStripMenuItemClearAll.Image = global::TestTool.GUI.Properties.Resources.Clear;
            this.toolStripMenuItemClearAll.Name = "toolStripMenuItemClearAll";
            this.toolStripMenuItemClearAll.Size = new System.Drawing.Size(166, 22);
            this.toolStripMenuItemClearAll.Text = "Clear All";
            this.toolStripMenuItemClearAll.Click += new System.EventHandler(this.tsbClearAll_Click);
            // 
            // toolStripDropDownButtonSave
            // 
            this.toolStripDropDownButtonSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveAll,
            this.toolStripMenuItemSaveCurrent,
            this.saveFeatureDefinitionLogToolStripMenuItem});
            this.toolStripDropDownButtonSave.Image = global::TestTool.GUI.Properties.Resources.Save;
            this.toolStripDropDownButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSave.Name = "toolStripDropDownButtonSave";
            this.toolStripDropDownButtonSave.Size = new System.Drawing.Size(60, 22);
            this.toolStripDropDownButtonSave.Text = "Save";
            this.toolStripDropDownButtonSave.ToolTipText = "Save current result";
            this.toolStripDropDownButtonSave.DropDownOpening += new System.EventHandler(this.toolStripSplitButtonSave_DropDownOpening);
            // 
            // toolStripMenuItemSaveAll
            // 
            this.toolStripMenuItemSaveAll.Enabled = false;
            this.toolStripMenuItemSaveAll.Image = global::TestTool.GUI.Properties.Resources.SaveAll;
            this.toolStripMenuItemSaveAll.Name = "toolStripMenuItemSaveAll";
            this.toolStripMenuItemSaveAll.Size = new System.Drawing.Size(218, 22);
            this.toolStripMenuItemSaveAll.Text = "Save All";
            this.toolStripMenuItemSaveAll.ToolTipText = "Save all test results";
            this.toolStripMenuItemSaveAll.Click += new System.EventHandler(this.toolStripMenuItemSaveAll_Click);
            // 
            // toolStripMenuItemSaveCurrent
            // 
            this.toolStripMenuItemSaveCurrent.Enabled = false;
            this.toolStripMenuItemSaveCurrent.Image = global::TestTool.GUI.Properties.Resources.Save;
            this.toolStripMenuItemSaveCurrent.Name = "toolStripMenuItemSaveCurrent";
            this.toolStripMenuItemSaveCurrent.Size = new System.Drawing.Size(218, 22);
            this.toolStripMenuItemSaveCurrent.Text = "Save Current";
            this.toolStripMenuItemSaveCurrent.ToolTipText = "Save results for currently selected test or group";
            this.toolStripMenuItemSaveCurrent.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // saveFeatureDefinitionLogToolStripMenuItem
            // 
            this.saveFeatureDefinitionLogToolStripMenuItem.Enabled = false;
            this.saveFeatureDefinitionLogToolStripMenuItem.Image = global::TestTool.GUI.Properties.Resources.SaveFeatureDefinitionLog;
            this.saveFeatureDefinitionLogToolStripMenuItem.Name = "saveFeatureDefinitionLogToolStripMenuItem";
            this.saveFeatureDefinitionLogToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.saveFeatureDefinitionLogToolStripMenuItem.Text = "Save Feature Definition Log";
            this.saveFeatureDefinitionLogToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemSaveFeatureDefinitionLog_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tcTestsAndFeatures);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcTestResults);
            this.scMain.Size = new System.Drawing.Size(785, 464);
            this.scMain.SplitterDistance = 250;
            this.scMain.TabIndex = 6;
            // 
            // tcTestsAndFeatures
            // 
            this.tcTestsAndFeatures.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcTestsAndFeatures.Controls.Add(this.tpTestCases);
            this.tcTestsAndFeatures.Controls.Add(this.tpFeatures);
            this.tcTestsAndFeatures.Controls.Add(this.tpProfiles);
            this.tcTestsAndFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTestsAndFeatures.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcTestsAndFeatures.Location = new System.Drawing.Point(0, 0);
            this.tcTestsAndFeatures.Multiline = true;
            this.tcTestsAndFeatures.Name = "tcTestsAndFeatures";
            this.tcTestsAndFeatures.SelectedIndex = 0;
            this.tcTestsAndFeatures.Size = new System.Drawing.Size(306, 464);
            this.tcTestsAndFeatures.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcTestsAndFeatures.TabIndex = 3;
            this.tcTestsAndFeatures.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tcTestsAndFeatures_DrawItem);
            // 
            // tpTestCases
            // 
            this.tpTestCases.Controls.Add(this.tvTestCases);
            this.tpTestCases.Location = new System.Drawing.Point(23, 4);
            this.tpTestCases.Name = "tpTestCases";
            this.tpTestCases.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestCases.Size = new System.Drawing.Size(279, 456);
            this.tpTestCases.TabIndex = 0;
            this.tpTestCases.Text = "Test Cases";
            this.tpTestCases.UseVisualStyleBackColor = true;
            // 
            // tvTestCases
            // 
            this.tvTestCases.CertificationMode = false;
            this.tvTestCases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTestCases.Location = new System.Drawing.Point(3, 3);
            this.tvTestCases.Name = "tvTestCases";
            this.tvTestCases.SelectedNode = null;
            this.tvTestCases.Size = new System.Drawing.Size(273, 450);
            this.tvTestCases.TabIndex = 2;
            this.tvTestCases.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeCheck);
            this.tvTestCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterSelect);
            this.tvTestCases.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterCheck);
            this.tvTestCases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            // 
            // tpFeatures
            // 
            this.tpFeatures.Controls.Add(this.featuresTree);
            this.tpFeatures.Location = new System.Drawing.Point(23, 4);
            this.tpFeatures.Name = "tpFeatures";
            this.tpFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tpFeatures.Size = new System.Drawing.Size(279, 456);
            this.tpFeatures.TabIndex = 1;
            this.tpFeatures.Text = "Features";
            this.tpFeatures.UseVisualStyleBackColor = true;
            // 
            // featuresTree
            // 
            this.featuresTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featuresTree.Location = new System.Drawing.Point(3, 3);
            this.featuresTree.Name = "featuresTree";
            this.featuresTree.Size = new System.Drawing.Size(273, 450);
            this.featuresTree.TabIndex = 0;
            this.featuresTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            this.featuresTree.TreeActivated += new System.EventHandler(this.featuresTree_Click);
            // 
            // tpProfiles
            // 
            this.tpProfiles.Controls.Add(this.tvProfiles);
            this.tpProfiles.Location = new System.Drawing.Point(23, 4);
            this.tpProfiles.Name = "tpProfiles";
            this.tpProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpProfiles.Size = new System.Drawing.Size(279, 456);
            this.tpProfiles.TabIndex = 2;
            this.tpProfiles.Text = "Profiles";
            this.tpProfiles.UseVisualStyleBackColor = true;
            // 
            // tvProfiles
            // 
            this.tvProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProfiles.Location = new System.Drawing.Point(3, 3);
            this.tvProfiles.Name = "tvProfiles";
            this.tvProfiles.Size = new System.Drawing.Size(273, 450);
            this.tvProfiles.TabIndex = 0;
            this.tvProfiles.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            // 
            // tcTestResults
            // 
            this.tcTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTestResults.Location = new System.Drawing.Point(0, 0);
            this.tcTestResults.Name = "tcTestResults";
            this.tcTestResults.Size = new System.Drawing.Size(475, 464);
            this.tcTestResults.TabIndex = 3;
            // 
            // TestPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.toolStripTestManagement);
            this.Name = "TestPage";
            this.Size = new System.Drawing.Size(785, 489);
            this.toolStripTestManagement.ResumeLayout(false);
            this.toolStripTestManagement.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.tcTestsAndFeatures.ResumeLayout(false);
            this.tpTestCases.ResumeLayout(false);
            this.tpFeatures.ResumeLayout(false);
            this.tpProfiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.ToolStrip toolStripTestManagement;
        private System.Windows.Forms.ToolStripButton tsbRunAll;
        private System.Windows.Forms.ToolStripSplitButton tsbRunCurrent;
        private System.Windows.Forms.ToolStripSplitButton tsbRunSelected;
        private System.Windows.Forms.ToolStripButton tsbQueryFeatures;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbPause;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.ToolStripButton tsbHalt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.SplitContainer scMain;
        private TestTool.GUI.Controls.TestsTree tvTestCases;
        private TestTool.GUI.Controls.TestResultsControl tcTestResults;
        private System.Windows.Forms.TabControl tcTestsAndFeatures;
        private System.Windows.Forms.TabPage tpTestCases;
        private System.Windows.Forms.TabPage tpFeatures;
        private FeaturesTree featuresTree;
        private System.Windows.Forms.TabPage tpProfiles;
        private ProfilesTree tvProfiles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearAll;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveCurrent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearTestResults;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonClear;
        private System.Windows.Forms.ToolStripMenuItem saveFeatureDefinitionLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbRepeatTests;
        private System.Windows.Forms.ToolStripMenuItem runCurrentAssumeAllFeaturesSupportedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runCurrentAssumeAllFeaturesNotSupportedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedAssumeAllFeaturesSupportedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedAssumeAllFeaturesNotSupportedToolStripMenuItem;
    }
}
