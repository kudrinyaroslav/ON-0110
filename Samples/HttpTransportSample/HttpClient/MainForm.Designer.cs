namespace HttpClient
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
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tbConsole = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbTimeout = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.btnBackgroundOperation = new System.Windows.Forms.Button();
            this.btnHalt = new System.Windows.Forms.Button();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.btnGetNtpBackground = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnBatch = new System.Windows.Forms.Button();
            this.btnBreakMessage = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAddress
            // 
            this.tbAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddress.Location = new System.Drawing.Point(70, 17);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(574, 20);
            this.tbAddress.TabIndex = 0;
            this.tbAddress.Text = "http://localhost:51078/Dut.asmx";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(6, 20);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 1;
            this.lblAddress.Text = "Address:";
            // 
            // tbConsole
            // 
            this.tbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsole.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbConsole.Location = new System.Drawing.Point(124, 107);
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.Size = new System.Drawing.Size(538, 329);
            this.tbConsole.TabIndex = 2;
            this.tbConsole.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbTimeout);
            this.groupBox1.Controls.Add(this.lblTimeout);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Controls.Add(this.tbAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 89);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // tbTimeout
            // 
            this.tbTimeout.Location = new System.Drawing.Point(70, 47);
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(206, 20);
            this.tbTimeout.TabIndex = 3;
            this.tbTimeout.Text = "60000";
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(9, 50);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(45, 13);
            this.lblTimeout.TabIndex = 2;
            this.lblTimeout.Text = "Timeout";
            // 
            // btnBackgroundOperation
            // 
            this.btnBackgroundOperation.Location = new System.Drawing.Point(12, 136);
            this.btnBackgroundOperation.Name = "btnBackgroundOperation";
            this.btnBackgroundOperation.Size = new System.Drawing.Size(106, 23);
            this.btnBackgroundOperation.TabIndex = 6;
            this.btnBackgroundOperation.Text = "GetHostname Background";
            this.btnBackgroundOperation.UseVisualStyleBackColor = true;
            this.btnBackgroundOperation.Click += new System.EventHandler(this.btnBackgroundOperation_Click);
            // 
            // btnHalt
            // 
            this.btnHalt.Location = new System.Drawing.Point(12, 413);
            this.btnHalt.Name = "btnHalt";
            this.btnHalt.Size = new System.Drawing.Size(106, 23);
            this.btnHalt.TabIndex = 7;
            this.btnHalt.Text = "Halt";
            this.btnHalt.UseVisualStyleBackColor = true;
            this.btnHalt.Click += new System.EventHandler(this.btnHalt_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 445);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(674, 22);
            this.mainStatusStrip.TabIndex = 8;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(300, 16);
            // 
            // btnGetNtpBackground
            // 
            this.btnGetNtpBackground.Location = new System.Drawing.Point(13, 107);
            this.btnGetNtpBackground.Name = "btnGetNtpBackground";
            this.btnGetNtpBackground.Size = new System.Drawing.Size(105, 23);
            this.btnGetNtpBackground.TabIndex = 9;
            this.btnGetNtpBackground.Text = "GetNTP";
            this.btnGetNtpBackground.UseVisualStyleBackColor = true;
            this.btnGetNtpBackground.Click += new System.EventHandler(this.btnGetNtpBackground_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(13, 384);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(106, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnBatch
            // 
            this.btnBatch.Location = new System.Drawing.Point(13, 165);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(105, 23);
            this.btnBatch.TabIndex = 11;
            this.btnBatch.Text = "Batch";
            this.btnBatch.UseVisualStyleBackColor = true;
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            // 
            // btnBreakMessage
            // 
            this.btnBreakMessage.Location = new System.Drawing.Point(13, 195);
            this.btnBreakMessage.Name = "btnBreakMessage";
            this.btnBreakMessage.Size = new System.Drawing.Size(105, 23);
            this.btnBreakMessage.TabIndex = 12;
            this.btnBreakMessage.Text = "Spoiled message";
            this.btnBreakMessage.UseVisualStyleBackColor = true;
            this.btnBreakMessage.Click += new System.EventHandler(this.btnBreakMessage_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(674, 467);
            this.Controls.Add(this.btnBreakMessage);
            this.Controls.Add(this.btnBatch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGetNtpBackground);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.btnHalt);
            this.Controls.Add(this.btnBackgroundOperation);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbConsole);
            this.Name = "MainForm";
            this.Text = "Http Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.RichTextBox tbConsole;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.TextBox tbTimeout;
        private System.Windows.Forms.Button btnBackgroundOperation;
        private System.Windows.Forms.Button btnHalt;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Button btnGetNtpBackground;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnBatch;
        private System.Windows.Forms.Button btnBreakMessage;
    }
}

