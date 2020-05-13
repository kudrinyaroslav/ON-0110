namespace SMC.Pages
{
    partial class LoggingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggingPage));
            this.lblReceiverAddress = new System.Windows.Forms.Label();
            this.tbLogReceiver = new System.Windows.Forms.TextBox();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblMessageType = new System.Windows.Forms.Label();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkMessage = new System.Windows.Forms.CheckBox();
            this.chkDetails = new System.Windows.Forms.CheckBox();
            this.lvLog = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.ilLogIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblReceiverAddress
            // 
            this.lblReceiverAddress.AutoSize = true;
            this.lblReceiverAddress.Location = new System.Drawing.Point(3, 10);
            this.lblReceiverAddress.Name = "lblReceiverAddress";
            this.lblReceiverAddress.Size = new System.Drawing.Size(74, 13);
            this.lblReceiverAddress.TabIndex = 0;
            this.lblReceiverAddress.Text = "Log Receiver:";
            // 
            // tbLogReceiver
            // 
            this.tbLogReceiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLogReceiver.Location = new System.Drawing.Point(98, 7);
            this.tbLogReceiver.Name = "tbLogReceiver";
            this.tbLogReceiver.Size = new System.Drawing.Size(474, 20);
            this.tbLogReceiver.TabIndex = 1;
            // 
            // btnStartListening
            // 
            this.btnStartListening.Location = new System.Drawing.Point(6, 61);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(86, 23);
            this.btnStartListening.TabIndex = 9;
            this.btnStartListening.Text = "Start Logging";
            this.btnStartListening.UseVisualStyleBackColor = true;
            this.btnStartListening.Click += new System.EventHandler(this.btnStartListening_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(98, 61);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(86, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "StopLogging";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblMessageType
            // 
            this.lblMessageType.AutoSize = true;
            this.lblMessageType.Location = new System.Drawing.Point(3, 35);
            this.lblMessageType.Name = "lblMessageType";
            this.lblMessageType.Size = new System.Drawing.Size(85, 13);
            this.lblMessageType.TabIndex = 4;
            this.lblMessageType.Text = "Message Types:";
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.Checked = true;
            this.chkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkError.Location = new System.Drawing.Point(98, 35);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(48, 17);
            this.chkError.TabIndex = 5;
            this.chkError.Text = "Error";
            this.chkError.UseVisualStyleBackColor = true;
            this.chkError.CheckedChanged += new System.EventHandler(this.chkError_CheckedChanged);
            // 
            // chkWarning
            // 
            this.chkWarning.AutoSize = true;
            this.chkWarning.Checked = true;
            this.chkWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarning.Location = new System.Drawing.Point(165, 35);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(66, 17);
            this.chkWarning.TabIndex = 6;
            this.chkWarning.Text = "Warning";
            this.chkWarning.UseVisualStyleBackColor = true;
            this.chkWarning.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkMessage
            // 
            this.chkMessage.AutoSize = true;
            this.chkMessage.Checked = true;
            this.chkMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMessage.Location = new System.Drawing.Point(250, 35);
            this.chkMessage.Name = "chkMessage";
            this.chkMessage.Size = new System.Drawing.Size(69, 17);
            this.chkMessage.TabIndex = 7;
            this.chkMessage.Text = "Message";
            this.chkMessage.UseVisualStyleBackColor = true;
            this.chkMessage.CheckedChanged += new System.EventHandler(this.chkMessage_CheckedChanged);
            // 
            // chkDetails
            // 
            this.chkDetails.AutoSize = true;
            this.chkDetails.Location = new System.Drawing.Point(338, 35);
            this.chkDetails.Name = "chkDetails";
            this.chkDetails.Size = new System.Drawing.Size(58, 17);
            this.chkDetails.TabIndex = 8;
            this.chkDetails.Text = "Details";
            this.chkDetails.UseVisualStyleBackColor = true;
            this.chkDetails.CheckedChanged += new System.EventHandler(this.chkDetails_CheckedChanged);
            // 
            // lvLog
            // 
            this.lvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colTime,
            this.colMessage});
            this.lvLog.Location = new System.Drawing.Point(6, 103);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(566, 261);
            this.lvLog.SmallImageList = this.ilLogIcons;
            this.lvLog.TabIndex = 12;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            // 
            // colId
            // 
            this.colId.Text = "Id";
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 120;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 400;
            // 
            // ilLogIcons
            // 
            this.ilLogIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilLogIcons.ImageStream")));
            this.ilLogIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilLogIcons.Images.SetKeyName(0, "Error");
            this.ilLogIcons.Images.SetKeyName(1, "Message");
            this.ilLogIcons.Images.SetKeyName(2, "Warning");
            this.ilLogIcons.Images.SetKeyName(3, "Details");
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(497, 61);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear Log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // LoggingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lvLog);
            this.Controls.Add(this.chkDetails);
            this.Controls.Add(this.chkMessage);
            this.Controls.Add(this.chkWarning);
            this.Controls.Add(this.chkError);
            this.Controls.Add(this.lblMessageType);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStartListening);
            this.Controls.Add(this.tbLogReceiver);
            this.Controls.Add(this.lblReceiverAddress);
            this.Name = "LoggingPage";
            this.Size = new System.Drawing.Size(586, 367);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReceiverAddress;
        private System.Windows.Forms.TextBox tbLogReceiver;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblMessageType;
        private System.Windows.Forms.CheckBox chkError;
        private System.Windows.Forms.CheckBox chkWarning;
        private System.Windows.Forms.CheckBox chkMessage;
        private System.Windows.Forms.CheckBox chkDetails;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ImageList ilLogIcons;
        private System.Windows.Forms.Button btnClear;
    }
}
