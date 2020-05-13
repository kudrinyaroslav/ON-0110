namespace DiscoveryServer
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
            this.btnRemoveType = new System.Windows.Forms.Button();
            this.btnAddType = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddScope = new System.Windows.Forms.Button();
            this.gbVersion = new System.Windows.Forms.GroupBox();
            this.rbCD1 = new System.Windows.Forms.RadioButton();
            this.rbVersionApril2005 = new System.Windows.Forms.RadioButton();
            this.rbVersion11 = new System.Windows.Forms.RadioButton();
            this.lblScopes = new System.Windows.Forms.Label();
            this.lvScopes = new System.Windows.Forms.ListView();
            this.ScopeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lvTypes = new System.Windows.Forms.ListView();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.tbAdresses = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGUID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.btnHello = new System.Windows.Forms.Button();
            this.btnBye = new System.Windows.Forms.Button();
            this.gbVersion.SuspendLayout();
            this.gbParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemoveType
            // 
            this.btnRemoveType.Location = new System.Drawing.Point(355, 98);
            this.btnRemoveType.Name = "btnRemoveType";
            this.btnRemoveType.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveType.TabIndex = 30;
            this.btnRemoveType.Text = "Remove";
            this.btnRemoveType.UseVisualStyleBackColor = true;
            this.btnRemoveType.Click += new System.EventHandler(this.btnRemoveType_Click);
            // 
            // btnAddType
            // 
            this.btnAddType.Location = new System.Drawing.Point(274, 98);
            this.btnAddType.Name = "btnAddType";
            this.btnAddType.Size = new System.Drawing.Size(75, 23);
            this.btnAddType.TabIndex = 29;
            this.btnAddType.Text = "Add";
            this.btnAddType.UseVisualStyleBackColor = true;
            this.btnAddType.Click += new System.EventHandler(this.btnAddType_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(351, 242);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 28;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddScope
            // 
            this.btnAddScope.Location = new System.Drawing.Point(270, 242);
            this.btnAddScope.Name = "btnAddScope";
            this.btnAddScope.Size = new System.Drawing.Size(75, 23);
            this.btnAddScope.TabIndex = 27;
            this.btnAddScope.Text = "Add";
            this.btnAddScope.UseVisualStyleBackColor = true;
            this.btnAddScope.Click += new System.EventHandler(this.btnAddScope_Click);
            // 
            // gbVersion
            // 
            this.gbVersion.Controls.Add(this.rbCD1);
            this.gbVersion.Controls.Add(this.rbVersionApril2005);
            this.gbVersion.Controls.Add(this.rbVersion11);
            this.gbVersion.Location = new System.Drawing.Point(9, 369);
            this.gbVersion.Name = "gbVersion";
            this.gbVersion.Size = new System.Drawing.Size(417, 97);
            this.gbVersion.TabIndex = 26;
            this.gbVersion.TabStop = false;
            this.gbVersion.Text = "Version";
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
            // lblScopes
            // 
            this.lblScopes.AutoSize = true;
            this.lblScopes.Location = new System.Drawing.Point(6, 247);
            this.lblScopes.Name = "lblScopes";
            this.lblScopes.Size = new System.Drawing.Size(43, 13);
            this.lblScopes.TabIndex = 23;
            this.lblScopes.Text = "Scopes";
            // 
            // lvScopes
            // 
            this.lvScopes.CheckBoxes = true;
            this.lvScopes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ScopeName});
            this.lvScopes.Location = new System.Drawing.Point(5, 268);
            this.lvScopes.Name = "lvScopes";
            this.lvScopes.Size = new System.Drawing.Size(421, 97);
            this.lvScopes.TabIndex = 22;
            this.lvScopes.UseCompatibleStateImageBehavior = false;
            this.lvScopes.View = System.Windows.Forms.View.Details;
            // 
            // ScopeName
            // 
            this.ScopeName.Text = "Scope";
            this.ScopeName.Width = 400;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Types";
            // 
            // lvTypes
            // 
            this.lvTypes.CheckBoxes = true;
            this.lvTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnType,
            this.columnNamespace});
            this.lvTypes.Location = new System.Drawing.Point(9, 127);
            this.lvTypes.Name = "lvTypes";
            this.lvTypes.Size = new System.Drawing.Size(421, 109);
            this.lvTypes.TabIndex = 20;
            this.lvTypes.UseCompatibleStateImageBehavior = false;
            this.lvTypes.View = System.Windows.Forms.View.Details;
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
            // gbParameters
            // 
            this.gbParameters.Controls.Add(this.tbAdresses);
            this.gbParameters.Controls.Add(this.label3);
            this.gbParameters.Controls.Add(this.tbGUID);
            this.gbParameters.Controls.Add(this.label2);
            this.gbParameters.Controls.Add(this.label1);
            this.gbParameters.Controls.Add(this.btnRemoveType);
            this.gbParameters.Controls.Add(this.lvTypes);
            this.gbParameters.Controls.Add(this.btnAddType);
            this.gbParameters.Controls.Add(this.lvScopes);
            this.gbParameters.Controls.Add(this.btnRemove);
            this.gbParameters.Controls.Add(this.lblScopes);
            this.gbParameters.Controls.Add(this.btnAddScope);
            this.gbParameters.Controls.Add(this.gbVersion);
            this.gbParameters.Location = new System.Drawing.Point(13, 13);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(444, 478);
            this.gbParameters.TabIndex = 31;
            this.gbParameters.TabStop = false;
            this.gbParameters.Text = "Parameters";
            // 
            // tbAdresses
            // 
            this.tbAdresses.Location = new System.Drawing.Point(74, 55);
            this.tbAdresses.Name = "tbAdresses";
            this.tbAdresses.Size = new System.Drawing.Size(364, 20);
            this.tbAdresses.TabIndex = 34;
            this.tbAdresses.Text = "http://localhost/onvif/device_service http://localhost/onvif/device_service";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Adresses:";
            // 
            // tbGUID
            // 
            this.tbGUID.Location = new System.Drawing.Point(74, 17);
            this.tbGUID.Name = "tbGUID";
            this.tbGUID.Size = new System.Drawing.Size(364, 20);
            this.tbGUID.TabIndex = 32;
            this.tbGUID.Text = "00075f74-9d25-259d-745f-0700075f745f";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "GUID:";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(12, 497);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 32;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnHello
            // 
            this.btnHello.Location = new System.Drawing.Point(93, 497);
            this.btnHello.Name = "btnHello";
            this.btnHello.Size = new System.Drawing.Size(75, 23);
            this.btnHello.TabIndex = 33;
            this.btnHello.Text = "Hello";
            this.btnHello.UseVisualStyleBackColor = true;
            this.btnHello.Click += new System.EventHandler(this.btnHello_Click);
            // 
            // btnBye
            // 
            this.btnBye.Location = new System.Drawing.Point(174, 497);
            this.btnBye.Name = "btnBye";
            this.btnBye.Size = new System.Drawing.Size(75, 23);
            this.btnBye.TabIndex = 34;
            this.btnBye.Text = "Bye";
            this.btnBye.UseVisualStyleBackColor = true;
            this.btnBye.Click += new System.EventHandler(this.btnBye_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 532);
            this.Controls.Add(this.btnBye);
            this.Controls.Add(this.btnHello);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.gbParameters);
            this.Name = "MainForm";
            this.Text = "Discovery server sample";
            this.gbVersion.ResumeLayout(false);
            this.gbVersion.PerformLayout();
            this.gbParameters.ResumeLayout(false);
            this.gbParameters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRemoveType;
        private System.Windows.Forms.Button btnAddType;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAddScope;
        private System.Windows.Forms.GroupBox gbVersion;
        private System.Windows.Forms.RadioButton rbCD1;
        private System.Windows.Forms.RadioButton rbVersionApril2005;
        private System.Windows.Forms.RadioButton rbVersion11;
        private System.Windows.Forms.Label lblScopes;
        private System.Windows.Forms.ListView lvScopes;
        private System.Windows.Forms.ColumnHeader ScopeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvTypes;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnNamespace;
        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnHello;
        private System.Windows.Forms.Button btnBye;
        private System.Windows.Forms.TextBox tbAdresses;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGUID;
        private System.Windows.Forms.Label label2;
    }
}

