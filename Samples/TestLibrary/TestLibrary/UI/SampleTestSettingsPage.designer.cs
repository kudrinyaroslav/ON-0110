namespace ProfilesTestLibrary.UI
{
    partial class SampleTestSettingsPage
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
            this.rbChoice1 = new System.Windows.Forms.RadioButton();
            this.tbString1 = new System.Windows.Forms.TextBox();
            this.lblPasswords = new System.Windows.Forms.Label();
            this.cmbSecureMethod = new System.Windows.Forms.ComboBox();
            this.rbChoice2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tbString2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rbEmbeddedPasswords
            // 
            this.rbChoice1.AutoSize = true;
            this.rbChoice1.Checked = true;
            this.rbChoice1.Location = new System.Drawing.Point(3, 3);
            this.rbChoice1.Name = "rbChoice1";
            this.rbChoice1.Size = new System.Drawing.Size(229, 17);
            this.rbChoice1.TabIndex = 39;
            this.rbChoice1.TabStop = true;
            this.rbChoice1.Text = "Option 1";
            this.rbChoice1.UseVisualStyleBackColor = true;
            // 
            // tbPassword1
            // 
            this.tbString1.Location = new System.Drawing.Point(88, 49);
            this.tbString1.Name = "tbString1";
            this.tbString1.Size = new System.Drawing.Size(130, 20);
            this.tbString1.TabIndex = 41;
            // 
            // lblPasswords
            // 
            this.lblPasswords.AutoSize = true;
            this.lblPasswords.Location = new System.Drawing.Point(21, 52);
            this.lblPasswords.Name = "Strings";
            this.lblPasswords.Size = new System.Drawing.Size(61, 13);
            this.lblPasswords.TabIndex = 43;
            this.lblPasswords.Text = "Strings:";
            // 
            // cmbSecureMethod
            // 
            this.cmbSecureMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecureMethod.FormattingEnabled = true;
            this.cmbSecureMethod.Location = new System.Drawing.Point(153, 75);
            this.cmbSecureMethod.Name = "cmbSecureMethod";
            this.cmbSecureMethod.Size = new System.Drawing.Size(231, 21);
            this.cmbSecureMethod.TabIndex = 45;
            // 
            // rbOwnPasswords
            // 
            this.rbChoice2.AutoSize = true;
            this.rbChoice2.Location = new System.Drawing.Point(3, 26);
            this.rbChoice2.Name = "rbChoice2";
            this.rbChoice2.Size = new System.Drawing.Size(137, 17);
            this.rbChoice2.TabIndex = 40;
            this.rbChoice2.TabStop = true;
            this.rbChoice2.Text = "Option 2";
            this.rbChoice2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "Selection 1:";
            // 
            // tbPassword2
            // 
            this.tbString2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbString2.Location = new System.Drawing.Point(254, 49);
            this.tbString2.MaximumSize = new System.Drawing.Size(130, 20);
            this.tbString2.Name = "tbString2";
            this.tbString2.Size = new System.Drawing.Size(130, 20);
            this.tbString2.TabIndex = 42;
            // 
            // SecurityTestSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbChoice1);
            this.Controls.Add(this.tbString1);
            this.Controls.Add(this.lblPasswords);
            this.Controls.Add(this.cmbSecureMethod);
            this.Controls.Add(this.rbChoice2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbString2);
            this.Name = "SecurityTestSettingsPage";
            this.Size = new System.Drawing.Size(393, 169);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbChoice1;
        private System.Windows.Forms.TextBox tbString1;
        private System.Windows.Forms.Label lblPasswords;
        private System.Windows.Forms.ComboBox cmbSecureMethod;
        private System.Windows.Forms.RadioButton rbChoice2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbString2;
    }
}
