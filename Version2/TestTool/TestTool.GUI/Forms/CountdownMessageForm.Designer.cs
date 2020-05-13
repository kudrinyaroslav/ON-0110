namespace TestTool.GUI.Forms
{
    partial class CountdownMessageForm
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
            this.components = new System.ComponentModel.Container();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblSelectDoor = new System.Windows.Forms.Label();
            this.cmbDoor = new System.Windows.Forms.ComboBox();
            this.blinkingPanel = new TestTool.GUI.Forms.BlinkingPanel();
            this.timerCountdown = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblCountdown
            // 
            this.lblCountdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Location = new System.Drawing.Point(14, 324);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(195, 13);
            this.lblCountdown.TabIndex = 2;
            this.lblCountdown.Text = "The window will be closed automatically";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.Location = new System.Drawing.Point(506, 319);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblSelectDoor
            // 
            this.lblSelectDoor.AutoSize = true;
            this.lblSelectDoor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectDoor.Location = new System.Drawing.Point(14, 297);
            this.lblSelectDoor.Name = "lblSelectDoor";
            this.lblSelectDoor.Size = new System.Drawing.Size(138, 13);
            this.lblSelectDoor.TabIndex = 5;
            this.lblSelectDoor.Text = "Select another door for test:";
            // 
            // cmbDoor
            // 
            this.cmbDoor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoor.FormattingEnabled = true;
            this.cmbDoor.Location = new System.Drawing.Point(163, 292);
            this.cmbDoor.Name = "cmbDoor";
            this.cmbDoor.Size = new System.Drawing.Size(164, 21);
            this.cmbDoor.TabIndex = 6;
            this.cmbDoor.SelectedIndexChanged += new System.EventHandler(this.cmbDoor_SelectedIndexChanged);
            // 
            // blinkingPanel
            // 
            this.blinkingPanel.BlinkColor = System.Drawing.Color.DarkRed;
            this.blinkingPanel.Location = new System.Drawing.Point(12, 24);
            this.blinkingPanel.Message = "";
            this.blinkingPanel.Name = "blinkingPanel";
            this.blinkingPanel.Size = new System.Drawing.Size(569, 262);
            this.blinkingPanel.TabIndex = 7;
            this.blinkingPanel.TimerEnabled = false;
            // 
            // timerCountdown
            // 
            this.timerCountdown.Enabled = true;
            this.timerCountdown.Interval = 1000;
            this.timerCountdown.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // CountdownMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 376);
            this.ControlBox = false;
            this.Controls.Add(this.blinkingPanel);
            this.Controls.Add(this.cmbDoor);
            this.Controls.Add(this.lblSelectDoor);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblCountdown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CountdownMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Interaction Required!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label lblSelectDoor;
        private System.Windows.Forms.ComboBox cmbDoor;
        private BlinkingPanel blinkingPanel;
        private System.Windows.Forms.Timer timerCountdown;
    }
}