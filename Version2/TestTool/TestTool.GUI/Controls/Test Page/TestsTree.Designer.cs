namespace TestTool.GUI.Controls
{
    partial class TestsTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestsTree));
            this.cmsTestsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.expandCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTestIcons = new System.Windows.Forms.ImageList(this.components);
            this.tvTestCases = new TestTool.GUI.Controls.TreeViewEx();
            this.cmsTestsTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsTestsTree
            // 
            this.cmsTestsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandCurrentToolStripMenuItem,
            this.collapseCurrentToolStripMenuItem,
            this.toolStripMenuItem1,
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem});
            this.cmsTestsTree.Name = "cmsTestsTree";
            this.cmsTestsTree.Size = new System.Drawing.Size(163, 120);
            this.cmsTestsTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTestsTree_Opening);
            this.cmsTestsTree.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsTestsTree_Closing);
            // 
            // expandCurrentToolStripMenuItem
            // 
            this.expandCurrentToolStripMenuItem.Name = "expandCurrentToolStripMenuItem";
            this.expandCurrentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.expandCurrentToolStripMenuItem.Text = "Expand Current";
            this.expandCurrentToolStripMenuItem.Click += new System.EventHandler(this.expandCurrentToolStripMenuItem_Click);
            // 
            // collapseCurrentToolStripMenuItem
            // 
            this.collapseCurrentToolStripMenuItem.Name = "collapseCurrentToolStripMenuItem";
            this.collapseCurrentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.collapseCurrentToolStripMenuItem.Text = "Collapse Current";
            this.collapseCurrentToolStripMenuItem.Click += new System.EventHandler(this.collapseCurrentToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // ilTestIcons
            // 
            this.ilTestIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTestIcons.ImageStream")));
            this.ilTestIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTestIcons.Images.SetKeyName(0, "None.ico");
            this.ilTestIcons.Images.SetKeyName(1, "Group");
            this.ilTestIcons.Images.SetKeyName(2, "OPTIONALIF.ico");
            this.ilTestIcons.Images.SetKeyName(3, "MUST.ico");
            this.ilTestIcons.Images.SetKeyName(4, "MUSTIF.ico");
            this.ilTestIcons.Images.SetKeyName(5, "OPTIONAL.ico");
            // 
            // tvTestCases
            // 
            this.tvTestCases.CheckBoxes = true;
            this.tvTestCases.ContextMenuStrip = this.cmsTestsTree;
            this.tvTestCases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTestCases.FullRowSelect = true;
            this.tvTestCases.HideSelection = false;
            this.tvTestCases.ImageIndex = 0;
            this.tvTestCases.ImageList = this.ilTestIcons;
            this.tvTestCases.Indent = 19;
            this.tvTestCases.Location = new System.Drawing.Point(0, 0);
            this.tvTestCases.Name = "tvTestCases";
            this.tvTestCases.SelectedImageIndex = 0;
            this.tvTestCases.ShowNodeToolTips = true;
            this.tvTestCases.Size = new System.Drawing.Size(307, 399);
            this.tvTestCases.TabIndex = 2;
            this.tvTestCases.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvTestCases_NodeMouseDoubleClick);
            this.tvTestCases.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterCheck);
            this.tvTestCases.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvTestCases_MouseClick);
            this.tvTestCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterSelect);
            this.tvTestCases.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvTestCases_NodeMouseClick);
            this.tvTestCases.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeCheck);
            this.tvTestCases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            // 
            // TestsTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvTestCases);
            this.Name = "TestsTree";
            this.Size = new System.Drawing.Size(307, 399);
            this.cmsTestsTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private TreeViewEx tvTestCases;
        private System.Windows.Forms.ImageList ilTestIcons;
        private System.Windows.Forms.ContextMenuStrip cmsTestsTree;
        private System.Windows.Forms.ToolStripMenuItem expandCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}
