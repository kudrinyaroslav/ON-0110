using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Diagnostics.TestsTreeView
{
  partial class FeaturesTree
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeaturesTree));
      this.tVFeatures = new BufferedTreeView();
      this.iLIcons = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // tVFeatures
      // 
      this.tVFeatures.CheckBoxes = false;
      this.tVFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tVFeatures.ImageKey = "ClearTestResults.ico";
      this.tVFeatures.ImageList = this.iLIcons;
      this.tVFeatures.Location = new System.Drawing.Point(0, 0);
      this.tVFeatures.Name = "tVFeatures";
      this.tVFeatures.SelectedImageIndex = 0;
      this.tVFeatures.ShowNodeToolTips = true;
      this.tVFeatures.Size = new System.Drawing.Size(372, 340);
      this.tVFeatures.TabIndex = 0;
      this.tVFeatures.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
      this.tVFeatures.HideSelection = false;
      // 
      // iLIcons
      // 
      this.iLIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iLIcons.ImageStream")));
      this.iLIcons.TransparentColor = System.Drawing.Color.Transparent;
      this.iLIcons.Images.SetKeyName(0, "ClearTestResults.ico");
      this.iLIcons.Images.SetKeyName(1, "NotSupported.ico");
      this.iLIcons.Images.SetKeyName(2, "Supported.ico");
      this.iLIcons.Images.SetKeyName(3, "Undefined.ico");
      this.iLIcons.Images.SetKeyName(4, "Group.ico");
      this.iLIcons.Images.SetKeyName(5, "MUST.ico");
      this.iLIcons.Images.SetKeyName(6, "MUSTIF.ico");
      this.iLIcons.Images.SetKeyName(7, "OPTIONAL.ico");
      this.iLIcons.Images.SetKeyName(8, "OPTIONALIF.ico");
      // 
      // FeaturesTree
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tVFeatures);
      this.Name = "FeaturesTree";
      this.Size = new System.Drawing.Size(372, 340);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView tVFeatures;
    private System.Windows.Forms.ImageList iLIcons;
  }
}
