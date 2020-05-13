using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  partial class ProfilesTree
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfilesTree));
      this.tVProfiles = new BufferedTreeView();
      this.iLIcons = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // tVProfiles
      // 
      this.tVProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tVProfiles.Location = new System.Drawing.Point(0, 0);
      this.tVProfiles.Name = "tVProfiles";
      this.tVProfiles.Size = new System.Drawing.Size(383, 391);
      this.tVProfiles.ImageList = this.iLIcons;
      this.tVProfiles.CheckBoxes = false;
      this.tVProfiles.TabIndex = 0;
      this.tVProfiles.ShowNodeToolTips = true;
      this.tVProfiles.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
      this.tVProfiles.HideSelection = false;
      // 
      // iLIcons
      // 
      this.iLIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iLIcons.ImageStream")));
      this.iLIcons.TransparentColor = System.Drawing.Color.Transparent;
      this.iLIcons.Images.SetKeyName(0, "NotSupported.ico");
      this.iLIcons.Images.SetKeyName(1, "Supported.ico");
      this.iLIcons.Images.SetKeyName(2, "ClearTestResults.ico");
      this.iLIcons.Images.SetKeyName(3, "NotSupported.ico");
      this.iLIcons.Images.SetKeyName(4, "Supported.ico");
      this.iLIcons.Images.SetKeyName(5, "Undefined.ico");
      this.iLIcons.Images.SetKeyName(6, "MUST.ico");
      this.iLIcons.Images.SetKeyName(7, "MUSTIF.ico");
      this.iLIcons.Images.SetKeyName(8, "OPTIONAL.ico");
      this.iLIcons.Images.SetKeyName(9, "OPTIONALIF.ico");
      this.iLIcons.Images.SetKeyName(10, "Group.ico");
      // 
      // ProfilesTree
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tVProfiles);
      this.Name = "ProfilesTree";
      this.Size = new System.Drawing.Size(383, 391);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView tVProfiles;
    private System.Windows.Forms.ImageList iLIcons;

  }
}
