namespace RTSPClient
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
            this.textIpAddress = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonBrowseVLC = new System.Windows.Forms.Button();
            this.textVLCFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStartVLC = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.vlcFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textIpAddress
            // 
            this.textIpAddress.Location = new System.Drawing.Point(12, 12);
            this.textIpAddress.Name = "textIpAddress";
            this.textIpAddress.Size = new System.Drawing.Size(129, 20);
            this.textIpAddress.TabIndex = 1;
            this.textIpAddress.Text = "rtsp://192.168.10.42";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonStop);
            this.tabPage2.Controls.Add(this.buttonBrowseVLC);
            this.tabPage2.Controls.Add(this.textVLCFolder);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.buttonStartVLC);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(531, 293);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "VLC";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStop.Location = new System.Drawing.Point(120, 71);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(85, 27);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonBrowseVLC
            // 
            this.buttonBrowseVLC.Location = new System.Drawing.Point(290, 9);
            this.buttonBrowseVLC.Name = "buttonBrowseVLC";
            this.buttonBrowseVLC.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseVLC.TabIndex = 3;
            this.buttonBrowseVLC.Text = "Browse";
            this.buttonBrowseVLC.UseVisualStyleBackColor = true;
            // 
            // textVLCFolder
            // 
            this.textVLCFolder.Location = new System.Drawing.Point(69, 9);
            this.textVLCFolder.Name = "textVLCFolder";
            this.textVLCFolder.Size = new System.Drawing.Size(206, 20);
            this.textVLCFolder.TabIndex = 2;
            this.textVLCFolder.Text = "C:\\Program Files\\VideoLAN\\VLC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "VLC library";
            // 
            // buttonStartVLC
            // 
            this.buttonStartVLC.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartVLC.Location = new System.Drawing.Point(20, 71);
            this.buttonStartVLC.Name = "buttonStartVLC";
            this.buttonStartVLC.Size = new System.Drawing.Size(85, 27);
            this.buttonStartVLC.TabIndex = 0;
            this.buttonStartVLC.Text = "Play";
            this.buttonStartVLC.UseVisualStyleBackColor = true;
            this.buttonStartVLC.Click += new System.EventHandler(this.buttonStartVLC_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(539, 319);
            this.tabControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 384);
            this.Controls.Add(this.textIpAddress);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textIpAddress;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button buttonStartVLC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog vlcFolderBrowserDialog;
        private System.Windows.Forms.Button buttonBrowseVLC;
        private System.Windows.Forms.TextBox textVLCFolder;
        private System.Windows.Forms.Button buttonStop;

    }
}

