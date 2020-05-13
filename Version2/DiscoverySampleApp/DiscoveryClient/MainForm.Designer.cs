namespace ONVIFSampleApp
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
            this.devicesList1 = new ONVIFSampleApp.DevicesList();
            this.SuspendLayout();
            // 
            // devicesList1
            // 
            this.devicesList1.Location = new System.Drawing.Point(-1, 2);
            this.devicesList1.Name = "devicesList1";
            this.devicesList1.Size = new System.Drawing.Size(621, 409);
            this.devicesList1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 423);
            this.Controls.Add(this.devicesList1);
            this.Name = "MainForm";
            this.Text = "Hello .NET 4.0";
            this.ResumeLayout(false);

        }

        #endregion

        private DevicesList devicesList1;

    }
}

