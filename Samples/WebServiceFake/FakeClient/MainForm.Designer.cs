namespace FakeClient
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
            this.btnCallService = new System.Windows.Forms.Button();
            this.btnWCFProxy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCallService
            // 
            this.btnCallService.Location = new System.Drawing.Point(13, 13);
            this.btnCallService.Name = "btnCallService";
            this.btnCallService.Size = new System.Drawing.Size(75, 23);
            this.btnCallService.TabIndex = 0;
            this.btnCallService.Text = "Service";
            this.btnCallService.UseVisualStyleBackColor = true;
            this.btnCallService.Click += new System.EventHandler(this.btnCallService_Click);
            // 
            // btnWCFProxy
            // 
            this.btnWCFProxy.Location = new System.Drawing.Point(13, 42);
            this.btnWCFProxy.Name = "btnWCFProxy";
            this.btnWCFProxy.Size = new System.Drawing.Size(75, 23);
            this.btnWCFProxy.TabIndex = 1;
            this.btnWCFProxy.Text = "WCF proxy";
            this.btnWCFProxy.UseVisualStyleBackColor = true;
            this.btnWCFProxy.Click += new System.EventHandler(this.btnWCFProxy_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.btnWCFProxy);
            this.Controls.Add(this.btnCallService);
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCallService;
        private System.Windows.Forms.Button btnWCFProxy;
    }
}

