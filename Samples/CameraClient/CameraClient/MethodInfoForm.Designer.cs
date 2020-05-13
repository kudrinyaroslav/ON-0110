namespace CameraClient
{
    partial class MethodInfoForm
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
            this.lblReturnType = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.listViewParameters = new System.Windows.Forms.ListView();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.TypeColumn = new System.Windows.Forms.ColumnHeader();
            this.btnViewType = new System.Windows.Forms.Button();
            this.lblParameters = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblReturnType
            // 
            this.lblReturnType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReturnType.Location = new System.Drawing.Point(12, 14);
            this.lblReturnType.Name = "lblReturnType";
            this.lblReturnType.Size = new System.Drawing.Size(187, 13);
            this.lblReturnType.TabIndex = 0;
            this.lblReturnType.Text = "label1";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(205, 238);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listViewParameters
            // 
            this.listViewParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.TypeColumn});
            this.listViewParameters.FullRowSelect = true;
            this.listViewParameters.Location = new System.Drawing.Point(12, 55);
            this.listViewParameters.Name = "listViewParameters";
            this.listViewParameters.Size = new System.Drawing.Size(268, 177);
            this.listViewParameters.TabIndex = 3;
            this.listViewParameters.UseCompatibleStateImageBehavior = false;
            this.listViewParameters.View = System.Windows.Forms.View.Details;
            this.listViewParameters.DoubleClick += new System.EventHandler(this.listViewParameters_DoubleClick);
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 96;
            // 
            // TypeColumn
            // 
            this.TypeColumn.Text = "Type";
            this.TypeColumn.Width = 154;
            // 
            // btnViewType
            // 
            this.btnViewType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewType.Location = new System.Drawing.Point(205, 9);
            this.btnViewType.Name = "btnViewType";
            this.btnViewType.Size = new System.Drawing.Size(75, 23);
            this.btnViewType.TabIndex = 4;
            this.btnViewType.Text = "View";
            this.btnViewType.UseVisualStyleBackColor = true;
            this.btnViewType.Click += new System.EventHandler(this.btnViewType_Click);
            // 
            // lblParameters
            // 
            this.lblParameters.AutoSize = true;
            this.lblParameters.Location = new System.Drawing.Point(12, 39);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(60, 13);
            this.lblParameters.TabIndex = 5;
            this.lblParameters.Text = "Parameters";
            // 
            // MethodInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.lblParameters);
            this.Controls.Add(this.btnViewType);
            this.Controls.Add(this.listViewParameters);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblReturnType);
            this.Name = "MethodInfoForm";
            this.Text = "MethodInfoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReturnType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView listViewParameters;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader TypeColumn;
        private System.Windows.Forms.Button btnViewType;
        private System.Windows.Forms.Label lblParameters;
    }
}