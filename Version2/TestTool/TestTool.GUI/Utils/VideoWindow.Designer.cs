namespace TestTool.GUI.Utils
{
    partial class VideoWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // VideoPanel
            // 
            this.VideoPanel.BackColor = System.Drawing.Color.Black;
            this.VideoPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VideoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoPanel.Location = new System.Drawing.Point(0, 0);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Size = new System.Drawing.Size(4, 0);
            this.VideoPanel.TabIndex = 1;
            this.VideoPanel.Visible = false;
            // 
            // VideoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(4, 0);
            this.Controls.Add(this.VideoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ONVIF Test Tool Video Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoWindow_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
    }
}