namespace TestTool.GUI.Forms
{
    partial class MessageForm
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
            this.blinkingPanel = new TestTool.GUI.Forms.BlinkingPanel();
            this.SuspendLayout();
            // 
            // blinkingPanel
            // 
            this.blinkingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.blinkingPanel.BlinkColor = System.Drawing.Color.DarkRed;
            this.blinkingPanel.Location = new System.Drawing.Point(12, 23);
            this.blinkingPanel.Message = "";
            this.blinkingPanel.Name = "blinkingPanel";
            this.blinkingPanel.Size = new System.Drawing.Size(569, 263);
            this.blinkingPanel.TabIndex = 0;
            this.blinkingPanel.TimerEnabled = false;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 298);
            this.ControlBox = false;
            this.Controls.Add(this.blinkingPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Interaction Required!";
            this.ResumeLayout(false);

        }

        #endregion

        private BlinkingPanel blinkingPanel;

    }
}