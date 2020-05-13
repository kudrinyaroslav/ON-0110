namespace TestTool.GUI.Controls.Pages
{
    partial class CustomConfigurationPage
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
            this.gbConfiguration = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.gbConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Controls.Add(this.btnLoad);
            this.gbConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfiguration.Location = new System.Drawing.Point(0, 0);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(707, 67);
            this.gbConfiguration.TabIndex = 2;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Simulator configuration";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(7, 20);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // CustomConfigurationPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConfiguration);
            this.Name = "CustomConfigurationPage";
            this.Size = new System.Drawing.Size(707, 459);
            this.gbConfiguration.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfiguration;
        private System.Windows.Forms.Button btnLoad;
    }
}
