namespace CTT_CovarageMap
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
            this.ActualStateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.SelectTestSpecFolderButton = new System.Windows.Forms.Button();
            this.SelectTestSpecFolderLabel = new System.Windows.Forms.Label();
            this.SelectTestSpecFolderTextBox = new System.Windows.Forms.TextBox();
            this.SelectTestSpecFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // ActualStateLabel
            // 
            this.ActualStateLabel.AutoSize = true;
            this.ActualStateLabel.Location = new System.Drawing.Point(53, 343);
            this.ActualStateLabel.Name = "ActualStateLabel";
            this.ActualStateLabel.Size = new System.Drawing.Size(58, 13);
            this.ActualStateLabel.TabIndex = 16;
            this.ActualStateLabel.Text = "Not Ready";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 343);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "State:";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(468, 324);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(83, 29);
            this.StartButton.TabIndex = 14;
            this.StartButton.Text = "START!";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SelectTestSpecFolderButton
            // 
            this.SelectTestSpecFolderButton.Location = new System.Drawing.Point(525, 34);
            this.SelectTestSpecFolderButton.Name = "SelectTestSpecFolderButton";
            this.SelectTestSpecFolderButton.Size = new System.Drawing.Size(26, 20);
            this.SelectTestSpecFolderButton.TabIndex = 9;
            this.SelectTestSpecFolderButton.Text = "...";
            this.SelectTestSpecFolderButton.UseVisualStyleBackColor = true;
            this.SelectTestSpecFolderButton.Click += new System.EventHandler(this.SelectTestSpecFolderButton_Click);
            // 
            // SelectTestSpecFolderLabel
            // 
            this.SelectTestSpecFolderLabel.AutoSize = true;
            this.SelectTestSpecFolderLabel.Location = new System.Drawing.Point(12, 18);
            this.SelectTestSpecFolderLabel.Name = "SelectTestSpecFolderLabel";
            this.SelectTestSpecFolderLabel.Size = new System.Drawing.Size(127, 13);
            this.SelectTestSpecFolderLabel.TabIndex = 18;
            this.SelectTestSpecFolderLabel.Text = "Test Specification Folder:";
            // 
            // SelectTestSpecFolderTextBox
            // 
            this.SelectTestSpecFolderTextBox.Location = new System.Drawing.Point(12, 34);
            this.SelectTestSpecFolderTextBox.Name = "SelectTestSpecFolderTextBox";
            this.SelectTestSpecFolderTextBox.ReadOnly = true;
            this.SelectTestSpecFolderTextBox.Size = new System.Drawing.Size(507, 20);
            this.SelectTestSpecFolderTextBox.TabIndex = 19;
            this.SelectTestSpecFolderTextBox.Text = "c:\\ONVIF\\!SVN_CTT\\Source\\Test Specifications\\Main";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 365);
            this.Controls.Add(this.SelectTestSpecFolderTextBox);
            this.Controls.Add(this.SelectTestSpecFolderLabel);
            this.Controls.Add(this.ActualStateLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.SelectTestSpecFolderButton);
            this.Name = "MainForm";
            this.Text = "CTT Coverage Map Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ActualStateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SelectTestSpecFolderButton;
        private System.Windows.Forms.Label SelectTestSpecFolderLabel;
        private System.Windows.Forms.TextBox SelectTestSpecFolderTextBox;
        private System.Windows.Forms.FolderBrowserDialog SelectTestSpecFolderDialog;
    }
}

