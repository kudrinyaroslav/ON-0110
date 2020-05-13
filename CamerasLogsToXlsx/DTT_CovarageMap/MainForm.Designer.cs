namespace DTT_CovarageMap
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
            this.AllTestResultsXMLFileTextBox = new System.Windows.Forms.TextBox();
            this.AllTestResultsXMLFileLabel = new System.Windows.Forms.Label();
            this.ActualStateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.SelectAllTestResultsXMLFileButton = new System.Windows.Forms.Button();
            this.SelectAllTestResultsXMLFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.CovarageMapFolderTextBox = new System.Windows.Forms.TextBox();
            this.CovarageMapFolderLabel = new System.Windows.Forms.Label();
            this.CovarageMapFolderButton = new System.Windows.Forms.Button();
            this.SelectCovarageMapFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ProfileLabel = new System.Windows.Forms.Label();
            this.ProfileComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // AllTestResultsXMLFileTextBox
            // 
            this.AllTestResultsXMLFileTextBox.Location = new System.Drawing.Point(12, 26);
            this.AllTestResultsXMLFileTextBox.Name = "AllTestResultsXMLFileTextBox";
            this.AllTestResultsXMLFileTextBox.ReadOnly = true;
            this.AllTestResultsXMLFileTextBox.Size = new System.Drawing.Size(507, 20);
            this.AllTestResultsXMLFileTextBox.TabIndex = 25;
            this.AllTestResultsXMLFileTextBox.Text = "D:\\!PROJECTS\\!ONVIF\\onvif-ext\\Source\\Test Specifications\\Main\\allTestsReport.xml";
            // 
            // AllTestResultsXMLFileLabel
            // 
            this.AllTestResultsXMLFileLabel.AutoSize = true;
            this.AllTestResultsXMLFileLabel.Location = new System.Drawing.Point(12, 10);
            this.AllTestResultsXMLFileLabel.Name = "AllTestResultsXMLFileLabel";
            this.AllTestResultsXMLFileLabel.Size = new System.Drawing.Size(127, 13);
            this.AllTestResultsXMLFileLabel.TabIndex = 24;
            this.AllTestResultsXMLFileLabel.Text = "All Test Results XML File:";
            // 
            // ActualStateLabel
            // 
            this.ActualStateLabel.AutoSize = true;
            this.ActualStateLabel.Location = new System.Drawing.Point(53, 335);
            this.ActualStateLabel.Name = "ActualStateLabel";
            this.ActualStateLabel.Size = new System.Drawing.Size(58, 13);
            this.ActualStateLabel.TabIndex = 23;
            this.ActualStateLabel.Text = "Not Ready";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "State:";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(468, 316);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(83, 29);
            this.StartButton.TabIndex = 21;
            this.StartButton.Text = "START!";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SelectAllTestResultsXMLFileButton
            // 
            this.SelectAllTestResultsXMLFileButton.Location = new System.Drawing.Point(525, 26);
            this.SelectAllTestResultsXMLFileButton.Name = "SelectAllTestResultsXMLFileButton";
            this.SelectAllTestResultsXMLFileButton.Size = new System.Drawing.Size(26, 20);
            this.SelectAllTestResultsXMLFileButton.TabIndex = 20;
            this.SelectAllTestResultsXMLFileButton.Text = "...";
            this.SelectAllTestResultsXMLFileButton.UseVisualStyleBackColor = true;
            this.SelectAllTestResultsXMLFileButton.Click += new System.EventHandler(this.SelectTestSpecFolderButton_Click);
            // 
            // CovarageMapFolderTextBox
            // 
            this.CovarageMapFolderTextBox.Location = new System.Drawing.Point(12, 67);
            this.CovarageMapFolderTextBox.Name = "CovarageMapFolderTextBox";
            this.CovarageMapFolderTextBox.ReadOnly = true;
            this.CovarageMapFolderTextBox.Size = new System.Drawing.Size(507, 20);
            this.CovarageMapFolderTextBox.TabIndex = 28;
            this.CovarageMapFolderTextBox.Text = "D:\\!PROJECTS\\!ONVIF\\onvif-ext\\Source\\Test Specifications\\Main\\";
            // 
            // CovarageMapFolderLabel
            // 
            this.CovarageMapFolderLabel.AutoSize = true;
            this.CovarageMapFolderLabel.Location = new System.Drawing.Point(12, 51);
            this.CovarageMapFolderLabel.Name = "CovarageMapFolderLabel";
            this.CovarageMapFolderLabel.Size = new System.Drawing.Size(112, 13);
            this.CovarageMapFolderLabel.TabIndex = 27;
            this.CovarageMapFolderLabel.Text = "Covarage Map Folder:";
            // 
            // CovarageMapFolderButton
            // 
            this.CovarageMapFolderButton.Location = new System.Drawing.Point(525, 67);
            this.CovarageMapFolderButton.Name = "CovarageMapFolderButton";
            this.CovarageMapFolderButton.Size = new System.Drawing.Size(26, 20);
            this.CovarageMapFolderButton.TabIndex = 26;
            this.CovarageMapFolderButton.Text = "...";
            this.CovarageMapFolderButton.UseVisualStyleBackColor = true;
            this.CovarageMapFolderButton.Click += new System.EventHandler(this.CovarageMapFolderButton_Click);
            // 
            // ProfileLabel
            // 
            this.ProfileLabel.AutoSize = true;
            this.ProfileLabel.Location = new System.Drawing.Point(12, 90);
            this.ProfileLabel.Name = "ProfileLabel";
            this.ProfileLabel.Size = new System.Drawing.Size(39, 13);
            this.ProfileLabel.TabIndex = 29;
            this.ProfileLabel.Text = "Profile:";
            // 
            // ProfileComboBox
            // 
            this.ProfileComboBox.FormattingEnabled = true;
            this.ProfileComboBox.Items.AddRange(new object[] {
            "S",
            "T",
            "A",
            "C",
            "G",
            "Q"});
            this.ProfileComboBox.Location = new System.Drawing.Point(12, 106);
            this.ProfileComboBox.Name = "ProfileComboBox";
            this.ProfileComboBox.Size = new System.Drawing.Size(121, 21);
            this.ProfileComboBox.TabIndex = 30;
            this.ProfileComboBox.Text = "S";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 355);
            this.Controls.Add(this.ProfileComboBox);
            this.Controls.Add(this.ProfileLabel);
            this.Controls.Add(this.CovarageMapFolderTextBox);
            this.Controls.Add(this.CovarageMapFolderLabel);
            this.Controls.Add(this.CovarageMapFolderButton);
            this.Controls.Add(this.AllTestResultsXMLFileTextBox);
            this.Controls.Add(this.AllTestResultsXMLFileLabel);
            this.Controls.Add(this.ActualStateLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.SelectAllTestResultsXMLFileButton);
            this.Name = "MainForm";
            this.Text = "DTT Covarage Map";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AllTestResultsXMLFileTextBox;
        private System.Windows.Forms.Label AllTestResultsXMLFileLabel;
        private System.Windows.Forms.Label ActualStateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SelectAllTestResultsXMLFileButton;
        private System.Windows.Forms.OpenFileDialog SelectAllTestResultsXMLFileDialog;
        private System.Windows.Forms.TextBox CovarageMapFolderTextBox;
        private System.Windows.Forms.Label CovarageMapFolderLabel;
        private System.Windows.Forms.Button CovarageMapFolderButton;
        private System.Windows.Forms.FolderBrowserDialog SelectCovarageMapFolderDialog;
        private System.Windows.Forms.Label ProfileLabel;
        private System.Windows.Forms.ComboBox ProfileComboBox;
    }
}

