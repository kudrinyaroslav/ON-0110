namespace ArchitectureSample
{
    partial class MainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.firstTabPage = new System.Windows.Forms.TabPage();
            this.firstTab1 = new ArchitectureSample.Controls.FirstTab();
            this.secondTabPage = new System.Windows.Forms.TabPage();
            this.secondTab1 = new ArchitectureSample.Controls.SecondTab();
            this.thirdTabPage = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl.SuspendLayout();
            this.firstTabPage.SuspendLayout();
            this.secondTabPage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.firstTabPage);
            this.tabControl.Controls.Add(this.secondTabPage);
            this.tabControl.Controls.Add(this.thirdTabPage);
            this.tabControl.Location = new System.Drawing.Point(4, 13);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(696, 414);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // firstTabPage
            // 
            this.firstTabPage.Controls.Add(this.firstTab1);
            this.firstTabPage.Location = new System.Drawing.Point(4, 22);
            this.firstTabPage.Name = "firstTabPage";
            this.firstTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.firstTabPage.Size = new System.Drawing.Size(688, 388);
            this.firstTabPage.TabIndex = 0;
            this.firstTabPage.Text = "First Page";
            this.firstTabPage.UseVisualStyleBackColor = true;
            // 
            // firstTab1
            // 
            this.firstTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.firstTab1.Location = new System.Drawing.Point(3, 3);
            this.firstTab1.Name = "firstTab1";
            this.firstTab1.Size = new System.Drawing.Size(682, 382);
            this.firstTab1.TabIndex = 0;
            // 
            // secondTabPage
            // 
            this.secondTabPage.Controls.Add(this.secondTab1);
            this.secondTabPage.Location = new System.Drawing.Point(4, 22);
            this.secondTabPage.Name = "secondTabPage";
            this.secondTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.secondTabPage.Size = new System.Drawing.Size(688, 386);
            this.secondTabPage.TabIndex = 1;
            this.secondTabPage.Text = "Second Page";
            this.secondTabPage.UseVisualStyleBackColor = true;
            // 
            // secondTab1
            // 
            this.secondTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.secondTab1.Location = new System.Drawing.Point(3, 3);
            this.secondTab1.Name = "secondTab1";
            this.secondTab1.Size = new System.Drawing.Size(682, 380);
            this.secondTab1.TabIndex = 0;
            // 
            // thirdTabPage
            // 
            this.thirdTabPage.Location = new System.Drawing.Point(4, 22);
            this.thirdTabPage.Name = "thirdTabPage";
            this.thirdTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.thirdTabPage.Size = new System.Drawing.Size(688, 386);
            this.thirdTabPage.TabIndex = 2;
            this.thirdTabPage.Text = "Third Page";
            this.thirdTabPage.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 440);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(700, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.AutoSize = false;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 462);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Sample";
            this.tabControl.ResumeLayout(false);
            this.firstTabPage.ResumeLayout(false);
            this.secondTabPage.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage firstTabPage;
        private System.Windows.Forms.TabPage secondTabPage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private ArchitectureSample.Controls.FirstTab firstTab1;
        private ArchitectureSample.Controls.SecondTab secondTab1;
        private System.Windows.Forms.TabPage thirdTabPage;
    }
}

