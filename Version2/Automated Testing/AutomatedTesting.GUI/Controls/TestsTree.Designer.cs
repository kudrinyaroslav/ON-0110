namespace AutomatedTesting.GUI.Controls
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
            this.tvTests = new System.Windows.Forms.TreeView();
            this.cmsTreeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectTimeoutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTreeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvTests
            // 
            this.tvTests.CheckBoxes = true;
            this.tvTests.ContextMenuStrip = this.cmsTreeMenu;
            this.tvTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTests.Location = new System.Drawing.Point(0, 0);
            this.tvTests.Name = "tvTests";
            this.tvTests.ShowNodeToolTips = true;
            this.tvTests.Size = new System.Drawing.Size(299, 428);
            this.tvTests.TabIndex = 0;
            this.tvTests.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTests_AfterSelect);
            this.tvTests.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTests_BeforeCheck);
            this.tvTests.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTests_AfterCheck);
            this.tvTests.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTests_BeforeSelect);
            // 
            // cmsTreeMenu
            // 
            this.cmsTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectAllToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.unselectTimeoutsToolStripMenuItem});
            this.cmsTreeMenu.Name = "cmsTreeMenu";
            this.cmsTreeMenu.Size = new System.Drawing.Size(173, 98);
            this.cmsTreeMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTreeMenu_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeToolStripMenuItem.Text = "Remove File";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(169, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // unselectTimeoutsToolStripMenuItem
            // 
            this.unselectTimeoutsToolStripMenuItem.Name = "unselectTimeoutsToolStripMenuItem";
            this.unselectTimeoutsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.unselectTimeoutsToolStripMenuItem.Text = "Unselect Timeouts";
            this.unselectTimeoutsToolStripMenuItem.Click += new System.EventHandler(this.unselectTimeoutsToolStripMenuItem_Click);
            // 
            // TestsTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvTests);
            this.Name = "TestsTree";
            this.Size = new System.Drawing.Size(299, 428);
            this.cmsTreeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvTests;
        private System.Windows.Forms.ContextMenuStrip cmsTreeMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unselectTimeoutsToolStripMenuItem;
    }
}
