namespace TestTool.GUI.Controls
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
            this.ilFeaturesIcons = new System.Windows.Forms.ImageList(this.components);
            this.tvFeatures = new TestTool.GUI.Controls.TreeViewEx();
            this.SuspendLayout();
            // 
            // ilFeaturesIcons
            // 
            this.ilFeaturesIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFeaturesIcons.ImageStream")));
            this.ilFeaturesIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilFeaturesIcons.Images.SetKeyName(0, "Clear");
            this.ilFeaturesIcons.Images.SetKeyName(1, "Group");
            this.ilFeaturesIcons.Images.SetKeyName(2, "Undefined");
            this.ilFeaturesIcons.Images.SetKeyName(3, "Supported");
            this.ilFeaturesIcons.Images.SetKeyName(4, "NotSupported");
            // 
            // tvFeatures
            // 
            this.tvFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvFeatures.ImageIndex = 0;
            this.tvFeatures.ImageList = this.ilFeaturesIcons;
            this.tvFeatures.Location = new System.Drawing.Point(0, 0);
            this.tvFeatures.Name = "tvFeatures";
            this.tvFeatures.SelectedImageIndex = 0;
            this.tvFeatures.ShowNodeToolTips = true;
            this.tvFeatures.Size = new System.Drawing.Size(307, 419);
            this.tvFeatures.TabIndex = 3;
            this.tvFeatures.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFeatures_NodeMouseClick);
            this.tvFeatures.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFeatures_BeforeSelect);
            this.tvFeatures.Click += new System.EventHandler(this.tvFeatures_Click);
            this.tvFeatures.HideSelection = false;
            // 
            // FeaturesTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvFeatures);
            this.Name = "FeaturesTree";
            this.Size = new System.Drawing.Size(307, 419);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList ilFeaturesIcons;
        private TreeViewEx tvFeatures;
    }
}
