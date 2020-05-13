namespace TestTool.GUI.Forms
{
    partial class DoorSelectionMessageForm
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
            this.lblSelectDoor = new System.Windows.Forms.Label();
            this.cmbDoor = new System.Windows.Forms.ComboBox();
            this.blinkingPanel = new TestTool.GUI.Forms.BlinkingPanel();
            this.SuspendLayout();
            // 
            // lblSelectDoor
            // 
            this.lblSelectDoor.AutoSize = true;
            this.lblSelectDoor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectDoor.Location = new System.Drawing.Point(12, 202);
            this.lblSelectDoor.Name = "lblSelectDoor";
            this.lblSelectDoor.Size = new System.Drawing.Size(138, 13);
            this.lblSelectDoor.TabIndex = 5;
            this.lblSelectDoor.Text = "Select another door for test:";
            // 
            // cmbDoor
            // 
            this.cmbDoor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoor.FormattingEnabled = true;
            this.cmbDoor.Location = new System.Drawing.Point(171, 199);
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
            this.blinkingPanel.Size = new System.Drawing.Size(569, 154);
            this.blinkingPanel.TabIndex = 7;
            this.blinkingPanel.TimerEnabled = false;
            // 
            // DoorSelectionMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 229);
            this.Controls.Add(this.blinkingPanel);
            this.Controls.Add(this.cmbDoor);
            this.Controls.Add(this.lblSelectDoor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoorSelectionMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Interaction Required!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectDoor;
        private System.Windows.Forms.ComboBox cmbDoor;
        private BlinkingPanel blinkingPanel;
    }
}