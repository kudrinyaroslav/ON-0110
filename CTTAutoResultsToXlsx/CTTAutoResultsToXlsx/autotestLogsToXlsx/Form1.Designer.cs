using System;
namespace CamerasLogsToXlsx
{
    partial class Form1
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
            this.SelectLogsButton = new System.Windows.Forms.Button();
            this.SelectExportFolderButton = new System.Windows.Forms.Button();
            this.UserExcelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExcelName = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ActualState = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.SelectedAutoTest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SelectLogsButton
            // 
            this.SelectLogsButton.Location = new System.Drawing.Point(12, 27);
            this.SelectLogsButton.Name = "SelectLogsButton";
            this.SelectLogsButton.Size = new System.Drawing.Size(260, 44);
            this.SelectLogsButton.TabIndex = 0;
            this.SelectLogsButton.Text = "Select AutoTesting TestReports";
            this.SelectLogsButton.UseVisualStyleBackColor = true;
            this.SelectLogsButton.Click += new System.EventHandler(this.SelectLogsButton_Click);
            // 
            // SelectExportFolderButton
            // 
            this.SelectExportFolderButton.Location = new System.Drawing.Point(12, 90);
            this.SelectExportFolderButton.Name = "SelectExportFolderButton";
            this.SelectExportFolderButton.Size = new System.Drawing.Size(260, 45);
            this.SelectExportFolderButton.TabIndex = 1;
            this.SelectExportFolderButton.Text = "Select Export Folder";
            this.SelectExportFolderButton.UseVisualStyleBackColor = true;
            this.SelectExportFolderButton.Click += new System.EventHandler(this.SelectExportFolderButton_Click);
            // 
            // UserExcelName
            // 
            this.UserExcelName.Location = new System.Drawing.Point(12, 194);
            this.UserExcelName.Name = "UserExcelName";
            this.UserExcelName.Size = new System.Drawing.Size(259, 20);
            this.UserExcelName.TabIndex = 2;
            this.UserExcelName.Text = "TestResults";
            this.UserExcelName.TextChanged += new System.EventHandler(this.UserExcelName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Your Excel Unique Name";
            // 
            // ExcelName
            // 
            this.ExcelName.AutoSize = true;
            this.ExcelName.Location = new System.Drawing.Point(13, 232);
            this.ExcelName.Name = "ExcelName";
            this.ExcelName.Size = new System.Drawing.Size(83, 13);
            this.ExcelName.TabIndex = 4;
            this.ExcelName.Text = "TestResults.xlsx";
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Location = new System.Drawing.Point(16, 259);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(256, 43);
            this.StartButton.TabIndex = 5;
            this.StartButton.Text = "START!";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 324);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "State:";
            // 
            // ActualState
            // 
            this.ActualState.AutoSize = true;
            this.ActualState.Location = new System.Drawing.Point(63, 324);
            this.ActualState.Name = "ActualState";
            this.ActualState.Size = new System.Drawing.Size(58, 13);
            this.ActualState.TabIndex = 7;
            this.ActualState.Text = "Not Ready";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk_1);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(12, 142);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(322, 13);
            this.FolderLabel.TabIndex = 8;
            this.FolderLabel.Text = "C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\\Common7\\IDE";
            // 
            // SelectedAutoTest
            // 
            this.SelectedAutoTest.AutoSize = true;
            this.SelectedAutoTest.Location = new System.Drawing.Point(12, 71);
            this.SelectedAutoTest.Name = "SelectedAutoTest";
            this.SelectedAutoTest.Size = new System.Drawing.Size(137, 13);
            this.SelectedAutoTest.TabIndex = 9;
            this.SelectedAutoTest.Text = "Selected auto testing report";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 353);
            this.Controls.Add(this.SelectedAutoTest);
            this.Controls.Add(this.FolderLabel);
            this.Controls.Add(this.ActualState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ExcelName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UserExcelName);
            this.Controls.Add(this.SelectExportFolderButton);
            this.Controls.Add(this.SelectLogsButton);
            this.Name = "Form1";
            this.Text = "CTT AutoResults To Xlsx";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectLogsButton;
        private System.Windows.Forms.Button SelectExportFolderButton;
        private System.Windows.Forms.TextBox UserExcelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ExcelName;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ActualState;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.Label SelectedAutoTest;
    }
}

