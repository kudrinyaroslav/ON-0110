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
            this.tvTestCases = new TestTool.GUI.Controls.TreeViewEx();
            this.ilTestIcons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tvTestCases
            // 
            this.tvTestCases.CheckBoxes = true;
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
            this.tvTestCases.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterCheck);
            this.tvTestCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTestCases_AfterSelect);
            this.tvTestCases.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeCheck);
            this.tvTestCases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTestCases_BeforeSelect);
            this.tvTestCases.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tvTestCases_NodeMouseClick);
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
            // TestsTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvTestCases);
            this.Name = "TestsTree";
            this.Size = new System.Drawing.Size(307, 399);
            this.ResumeLayout(false);

        }


        #endregion

        private TreeViewEx tvTestCases;
        private System.Windows.Forms.ImageList ilTestIcons;
    }
}
