namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
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
      this.tVTestCases = new BufferedTreeView();
      this.iLIcons = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // tVTestCases
      // 
      this.tVTestCases.CheckBoxes = false;
      this.tVTestCases.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tVTestCases.ImageKey = "MUST.ico";
      this.tVTestCases.ImageList = this.iLIcons;
      this.tVTestCases.Location = new System.Drawing.Point(0, 0);
      this.tVTestCases.Name = "tVTestCases";
      this.tVTestCases.SelectedImageIndex = 0;
      this.tVTestCases.ShowNodeToolTips = true;
      this.tVTestCases.Size = new System.Drawing.Size(357, 359);
      this.tVTestCases.TabIndex = 0;
      this.tVTestCases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeSelect);
      this.tVTestCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
      this.tVTestCases.HideSelection = false;
      // 
      // iLIcons
      // 
      this.iLIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iLIcons.ImageStream")));
      this.iLIcons.TransparentColor = System.Drawing.Color.Transparent;
      this.iLIcons.Images.SetKeyName(0, "MUST.ico");
      this.iLIcons.Images.SetKeyName(1, "MUSTIF.ico");
      this.iLIcons.Images.SetKeyName(2, "OPTIONAL.ico");
      this.iLIcons.Images.SetKeyName(3, "OPTIONALIF.ico");
      this.iLIcons.Images.SetKeyName(4, "Group.ico");
      // 
      // TestsTree
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tVTestCases);
      this.Name = "TestsTree";
      this.Size = new System.Drawing.Size(357, 359);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView tVTestCases;
    private System.Windows.Forms.ImageList iLIcons;
  }
}
