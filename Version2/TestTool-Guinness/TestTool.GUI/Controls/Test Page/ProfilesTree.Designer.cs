namespace TestTool.GUI.Controls
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
            this.tvProfiles = new System.Windows.Forms.TreeView();
            this.ilFunctionality = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tvProfiles
            // 
            this.tvProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProfiles.ImageIndex = 0;
            this.tvProfiles.ImageList = this.ilFunctionality;
            this.tvProfiles.Location = new System.Drawing.Point(0, 0);
            this.tvProfiles.Name = "tvProfiles";
            this.tvProfiles.SelectedImageIndex = 0;
            this.tvProfiles.ShowNodeToolTips = true;
            this.tvProfiles.Size = new System.Drawing.Size(281, 445);
            this.tvProfiles.TabIndex = 0;
            this.tvProfiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProfiles_AfterSelect);
            this.tvProfiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProfiles_NodeMouseClick);
            this.tvProfiles.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProfiles_BeforeSelect);
            this.tvProfiles.HideSelection = false;
            // 
            // ilFunctionality
            // 
            this.ilFunctionality.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFunctionality.ImageStream")));
            this.ilFunctionality.TransparentColor = System.Drawing.Color.Transparent;
            this.ilFunctionality.Images.SetKeyName(0, "Group");
            this.ilFunctionality.Images.SetKeyName(1, "Undefined");
            this.ilFunctionality.Images.SetKeyName(2, "Supported");
            this.ilFunctionality.Images.SetKeyName(3, "NotSupported");
            this.ilFunctionality.Images.SetKeyName(4, "SkipTest");
            this.ilFunctionality.Images.SetKeyName(5, "SkipFeature");
            this.ilFunctionality.Images.SetKeyName(6, "Selected");
            // 
            // ProfilesTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvProfiles);
            this.Name = "ProfilesTree";
            this.Size = new System.Drawing.Size(281, 445);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvProfiles;
        private System.Windows.Forms.ImageList ilFunctionality;
    }
}
