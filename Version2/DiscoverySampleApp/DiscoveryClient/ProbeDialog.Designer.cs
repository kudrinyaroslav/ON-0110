namespace ONVIFSampleApp
{
    partial class ProbeDialog
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
            this.lblScopes = new System.Windows.Forms.Label();
            this.lvScopes = new System.Windows.Forms.ListView();
            this.ScopeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lvTypes = new System.Windows.Forms.ListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbCD1 = new System.Windows.Forms.RadioButton();
            this.rbVersionApril2005 = new System.Windows.Forms.RadioButton();
            this.rbVersion11 = new System.Windows.Forms.RadioButton();
            this.btnAddScope = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveType = new System.Windows.Forms.Button();
            this.btnAddType = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblScopes
            // 
            this.lblScopes.AutoSize = true;
            this.lblScopes.Location = new System.Drawing.Point(14, 161);
            this.lblScopes.Name = "lblScopes";
            this.lblScopes.Size = new System.Drawing.Size(43, 13);
            this.lblScopes.TabIndex = 12;
            this.lblScopes.Text = "Scopes";
            // 
            // lvScopes
            // 
            this.lvScopes.CheckBoxes = true;
            this.lvScopes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ScopeName});
            this.lvScopes.Location = new System.Drawing.Point(13, 182);
            this.lvScopes.Name = "lvScopes";
            this.lvScopes.Size = new System.Drawing.Size(421, 97);
            this.lvScopes.TabIndex = 11;
            this.lvScopes.UseCompatibleStateImageBehavior = false;
            this.lvScopes.View = System.Windows.Forms.View.Details;
            this.lvScopes.SelectedIndexChanged += new System.EventHandler(this.lvScopes_SelectedIndexChanged);
            // 
            // ScopeName
            // 
            this.ScopeName.Text = "Scope";
            this.ScopeName.Width = 400;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Types";
            // 
            // lvTypes
            // 
            this.lvTypes.CheckBoxes = true;
            this.lvTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType,
            this.columnNamespace});
            this.lvTypes.Location = new System.Drawing.Point(17, 41);
            this.lvTypes.Name = "lvTypes";
            this.lvTypes.Size = new System.Drawing.Size(421, 109);
            this.lvTypes.TabIndex = 9;
            this.lvTypes.UseCompatibleStateImageBehavior = false;
            this.lvTypes.View = System.Windows.Forms.View.Details;
            this.lvTypes.SelectedIndexChanged += new System.EventHandler(this.lvTypes_SelectedIndexChanged);
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            this.columnType.Width = 91;
            // 
            // columnNamespace
            // 
            this.columnNamespace.Text = "Namespace";
            this.columnNamespace.Width = 300;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(147, 388);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(228, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbCD1);
            this.groupBox1.Controls.Add(this.rbVersionApril2005);
            this.groupBox1.Controls.Add(this.rbVersion11);
            this.groupBox1.Location = new System.Drawing.Point(17, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(417, 97);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Version";
            // 
            // rbCD1
            // 
            this.rbCD1.AutoSize = true;
            this.rbCD1.Location = new System.Drawing.Point(6, 67);
            this.rbCD1.Name = "rbCD1";
            this.rbCD1.Size = new System.Drawing.Size(49, 17);
            this.rbCD1.TabIndex = 2;
            this.rbCD1.TabStop = true;
            this.rbCD1.Text = "CD 1";
            this.rbCD1.UseVisualStyleBackColor = true;
            // 
            // rbVersionApril2005
            // 
            this.rbVersionApril2005.AutoSize = true;
            this.rbVersionApril2005.Checked = true;
            this.rbVersionApril2005.Location = new System.Drawing.Point(6, 43);
            this.rbVersionApril2005.Name = "rbVersionApril2005";
            this.rbVersionApril2005.Size = new System.Drawing.Size(72, 17);
            this.rbVersionApril2005.TabIndex = 1;
            this.rbVersionApril2005.TabStop = true;
            this.rbVersionApril2005.Text = "April 2005";
            this.rbVersionApril2005.UseVisualStyleBackColor = true;
            // 
            // rbVersion11
            // 
            this.rbVersion11.AutoSize = true;
            this.rbVersion11.Location = new System.Drawing.Point(6, 19);
            this.rbVersion11.Name = "rbVersion11";
            this.rbVersion11.Size = new System.Drawing.Size(40, 17);
            this.rbVersion11.TabIndex = 0;
            this.rbVersion11.TabStop = true;
            this.rbVersion11.Text = "1.1";
            this.rbVersion11.UseVisualStyleBackColor = true;
            // 
            // btnAddScope
            // 
            this.btnAddScope.Location = new System.Drawing.Point(278, 156);
            this.btnAddScope.Name = "btnAddScope";
            this.btnAddScope.Size = new System.Drawing.Size(75, 23);
            this.btnAddScope.TabIndex = 16;
            this.btnAddScope.Text = "Add";
            this.btnAddScope.UseVisualStyleBackColor = true;
            this.btnAddScope.Click += new System.EventHandler(this.btnAddScope_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(359, 156);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveType
            // 
            this.btnRemoveType.Location = new System.Drawing.Point(359, 12);
            this.btnRemoveType.Name = "btnRemoveType";
            this.btnRemoveType.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveType.TabIndex = 19;
            this.btnRemoveType.Text = "Remove";
            this.btnRemoveType.UseVisualStyleBackColor = true;
            this.btnRemoveType.Click += new System.EventHandler(this.btnRemoveType_Click);
            // 
            // btnAddType
            // 
            this.btnAddType.Location = new System.Drawing.Point(278, 12);
            this.btnAddType.Name = "btnAddType";
            this.btnAddType.Size = new System.Drawing.Size(75, 23);
            this.btnAddType.TabIndex = 18;
            this.btnAddType.Text = "Add";
            this.btnAddType.UseVisualStyleBackColor = true;
            this.btnAddType.Click += new System.EventHandler(this.btnAddType_Click);
            // 
            // ProbeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 418);
            this.Controls.Add(this.btnRemoveType);
            this.Controls.Add(this.btnAddType);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddScope);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblScopes);
            this.Controls.Add(this.lvScopes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvTypes);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProbeDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Probe";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScopes;
        private System.Windows.Forms.ListView lvScopes;
        private System.Windows.Forms.ColumnHeader ScopeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvTypes;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnNamespace;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCD1;
        private System.Windows.Forms.RadioButton rbVersionApril2005;
        private System.Windows.Forms.RadioButton rbVersion11;
        private System.Windows.Forms.Button btnAddScope;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveType;
        private System.Windows.Forms.Button btnAddType;
    }
}