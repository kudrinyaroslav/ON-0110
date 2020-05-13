namespace ONVIFSampleApp
{
    partial class DevicesList
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
            this.lvDevices = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnProbe = new System.Windows.Forms.Button();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.rbCD1 = new System.Windows.Forms.RadioButton();
            this.rbVersionApril2005 = new System.Windows.Forms.RadioButton();
            this.rbVersion11 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn});
            this.lvDevices.Location = new System.Drawing.Point(3, 75);
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(547, 233);
            this.lvDevices.TabIndex = 2;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Response";
            this.NameColumn.Width = 416;
            // 
            // btnProbe
            // 
            this.btnProbe.Location = new System.Drawing.Point(4, 41);
            this.btnProbe.Name = "btnProbe";
            this.btnProbe.Size = new System.Drawing.Size(75, 23);
            this.btnProbe.TabIndex = 4;
            this.btnProbe.Text = "Probe...";
            this.btnProbe.UseVisualStyleBackColor = true;
            this.btnProbe.Click += new System.EventHandler(this.btnProbe_Click);
            // 
            // btnDiscover
            // 
            this.btnDiscover.Location = new System.Drawing.Point(4, 12);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(75, 23);
            this.btnDiscover.TabIndex = 5;
            this.btnDiscover.Text = "Listen";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // rbCD1
            // 
            this.rbCD1.AutoSize = true;
            this.rbCD1.Location = new System.Drawing.Point(247, 15);
            this.rbCD1.Name = "rbCD1";
            this.rbCD1.Size = new System.Drawing.Size(49, 17);
            this.rbCD1.TabIndex = 8;
            this.rbCD1.TabStop = true;
            this.rbCD1.Text = "CD 1";
            this.rbCD1.UseVisualStyleBackColor = true;
            // 
            // rbVersionApril2005
            // 
            this.rbVersionApril2005.AutoSize = true;
            this.rbVersionApril2005.Checked = true;
            this.rbVersionApril2005.Location = new System.Drawing.Point(156, 15);
            this.rbVersionApril2005.Name = "rbVersionApril2005";
            this.rbVersionApril2005.Size = new System.Drawing.Size(72, 17);
            this.rbVersionApril2005.TabIndex = 7;
            this.rbVersionApril2005.TabStop = true;
            this.rbVersionApril2005.Text = "April 2005";
            this.rbVersionApril2005.UseVisualStyleBackColor = true;
            // 
            // rbVersion11
            // 
            this.rbVersion11.AutoSize = true;
            this.rbVersion11.Location = new System.Drawing.Point(97, 15);
            this.rbVersion11.Name = "rbVersion11";
            this.rbVersion11.Size = new System.Drawing.Size(40, 17);
            this.rbVersion11.TabIndex = 6;
            this.rbVersion11.TabStop = true;
            this.rbVersion11.Text = "1.1";
            this.rbVersion11.UseVisualStyleBackColor = true;
            // 
            // DevicesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbCD1);
            this.Controls.Add(this.rbVersionApril2005);
            this.Controls.Add(this.rbVersion11);
            this.Controls.Add(this.btnDiscover);
            this.Controls.Add(this.btnProbe);
            this.Controls.Add(this.lvDevices);
            this.Name = "DevicesList";
            this.Size = new System.Drawing.Size(553, 311);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.Button btnProbe;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.RadioButton rbCD1;
        private System.Windows.Forms.RadioButton rbVersionApril2005;
        private System.Windows.Forms.RadioButton rbVersion11;
    }
}
