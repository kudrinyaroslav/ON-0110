namespace TestTool.GUI.Controls
{
    partial class RequestsPage
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
            this.scRequests = new System.Windows.Forms.SplitContainer();
            this.lblTemplates = new System.Windows.Forms.Label();
            this.tvTemplates = new System.Windows.Forms.TreeView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.scTraffic = new System.Windows.Forms.SplitContainer();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.cmbService = new System.Windows.Forms.ComboBox();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.lblServiceAddress = new System.Windows.Forms.Label();
            this.tbServiceAddress = new System.Windows.Forms.TextBox();
            this.lblService = new System.Windows.Forms.Label();
            this.btnAddRequestToTemplates = new System.Windows.Forms.Button();
            this.tbRequestFile = new System.Windows.Forms.TextBox();
            this.lblRequestFile = new System.Windows.Forms.Label();
            this.btnRequestFile = new System.Windows.Forms.Button();
            this.scRequests.Panel1.SuspendLayout();
            this.scRequests.Panel2.SuspendLayout();
            this.scRequests.SuspendLayout();
            this.scTraffic.Panel1.SuspendLayout();
            this.scTraffic.Panel2.SuspendLayout();
            this.scTraffic.SuspendLayout();
            this.SuspendLayout();
            // 
            // scRequests
            // 
            this.scRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scRequests.Location = new System.Drawing.Point(0, 0);
            this.scRequests.Name = "scRequests";
            // 
            // scRequests.Panel1
            // 
            this.scRequests.Panel1.Controls.Add(this.lblTemplates);
            this.scRequests.Panel1.Controls.Add(this.tvTemplates);
            // 
            // scRequests.Panel2
            // 
            this.scRequests.Panel2.Controls.Add(this.btnDelete);
            this.scRequests.Panel2.Controls.Add(this.scTraffic);
            this.scRequests.Panel2.Controls.Add(this.cmbService);
            this.scRequests.Panel2.Controls.Add(this.btnSendRequest);
            this.scRequests.Panel2.Controls.Add(this.lblServiceAddress);
            this.scRequests.Panel2.Controls.Add(this.tbServiceAddress);
            this.scRequests.Panel2.Controls.Add(this.lblService);
            this.scRequests.Panel2.Controls.Add(this.btnAddRequestToTemplates);
            this.scRequests.Panel2.Controls.Add(this.tbRequestFile);
            this.scRequests.Panel2.Controls.Add(this.lblRequestFile);
            this.scRequests.Panel2.Controls.Add(this.btnRequestFile);
            this.scRequests.Size = new System.Drawing.Size(821, 487);
            this.scRequests.SplitterDistance = 272;
            this.scRequests.TabIndex = 5;
            // 
            // lblTemplates
            // 
            this.lblTemplates.AutoSize = true;
            this.lblTemplates.Location = new System.Drawing.Point(6, 9);
            this.lblTemplates.Name = "lblTemplates";
            this.lblTemplates.Size = new System.Drawing.Size(59, 13);
            this.lblTemplates.TabIndex = 1;
            this.lblTemplates.Text = "Templates:";
            // 
            // tvTemplates
            // 
            this.tvTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTemplates.HideSelection = false;
            this.tvTemplates.Location = new System.Drawing.Point(3, 33);
            this.tvTemplates.Name = "tvTemplates";
            this.tvTemplates.Size = new System.Drawing.Size(266, 451);
            this.tvTemplates.TabIndex = 4;
            this.tvTemplates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTemplates_AfterSelect);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(6, 85);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(96, 23);
            this.btnDelete.TabIndex = 20;
            this.btnDelete.Text = "Delete Request";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // scTraffic
            // 
            this.scTraffic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scTraffic.Location = new System.Drawing.Point(0, 114);
            this.scTraffic.Name = "scTraffic";
            // 
            // scTraffic.Panel1
            // 
            this.scTraffic.Panel1.Controls.Add(this.tbRequest);
            this.scTraffic.Panel1.Controls.Add(this.lblRequest);
            // 
            // scTraffic.Panel2
            // 
            this.scTraffic.Panel2.Controls.Add(this.tbResponse);
            this.scTraffic.Panel2.Controls.Add(this.lblResponse);
            this.scTraffic.Size = new System.Drawing.Size(533, 373);
            this.scTraffic.SplitterDistance = 264;
            this.scTraffic.TabIndex = 19;
            // 
            // tbRequest
            // 
            this.tbRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRequest.Location = new System.Drawing.Point(6, 17);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRequest.Size = new System.Drawing.Size(255, 353);
            this.tbRequest.TabIndex = 12;
            this.tbRequest.WordWrap = false;
            // 
            // lblRequest
            // 
            this.lblRequest.AutoSize = true;
            this.lblRequest.Location = new System.Drawing.Point(3, 0);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(50, 13);
            this.lblRequest.TabIndex = 2;
            this.lblRequest.Text = "Request:";
            // 
            // tbResponse
            // 
            this.tbResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResponse.Location = new System.Drawing.Point(5, 17);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResponse.Size = new System.Drawing.Size(259, 353);
            this.tbResponse.TabIndex = 13;
            this.tbResponse.WordWrap = false;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(3, 0);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(58, 13);
            this.lblResponse.TabIndex = 4;
            this.lblResponse.Text = "Response:";
            // 
            // cmbService
            // 
            this.cmbService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbService.DisplayMember = "DisplayName";
            this.cmbService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbService.FormattingEnabled = true;
            this.cmbService.Location = new System.Drawing.Point(95, 6);
            this.cmbService.Name = "cmbService";
            this.cmbService.Size = new System.Drawing.Size(443, 21);
            this.cmbService.TabIndex = 6;
            this.cmbService.ValueMember = "Service";
            this.cmbService.SelectedIndexChanged += new System.EventHandler(this.cmbService_SelectedIndexChanged);
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendRequest.Location = new System.Drawing.Point(446, 85);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(93, 23);
            this.btnSendRequest.TabIndex = 11;
            this.btnSendRequest.Text = "Send Request";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // lblServiceAddress
            // 
            this.lblServiceAddress.AutoSize = true;
            this.lblServiceAddress.Location = new System.Drawing.Point(3, 36);
            this.lblServiceAddress.Name = "lblServiceAddress";
            this.lblServiceAddress.Size = new System.Drawing.Size(87, 13);
            this.lblServiceAddress.TabIndex = 17;
            this.lblServiceAddress.Text = "Service Address:";
            // 
            // tbServiceAddress
            // 
            this.tbServiceAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServiceAddress.Location = new System.Drawing.Point(96, 33);
            this.tbServiceAddress.Name = "tbServiceAddress";
            this.tbServiceAddress.Size = new System.Drawing.Size(443, 20);
            this.tbServiceAddress.TabIndex = 7;
            // 
            // lblService
            // 
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(3, 9);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(46, 13);
            this.lblService.TabIndex = 15;
            this.lblService.Text = "Service:";
            // 
            // btnAddRequestToTemplates
            // 
            this.btnAddRequestToTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRequestToTemplates.Location = new System.Drawing.Point(277, 85);
            this.btnAddRequestToTemplates.Name = "btnAddRequestToTemplates";
            this.btnAddRequestToTemplates.Size = new System.Drawing.Size(163, 23);
            this.btnAddRequestToTemplates.TabIndex = 10;
            this.btnAddRequestToTemplates.Text = "Add Request To Templates";
            this.btnAddRequestToTemplates.UseVisualStyleBackColor = true;
            this.btnAddRequestToTemplates.Click += new System.EventHandler(this.btnAddRequestToTemplates_Click);
            // 
            // tbRequestFile
            // 
            this.tbRequestFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRequestFile.Location = new System.Drawing.Point(96, 59);
            this.tbRequestFile.Name = "tbRequestFile";
            this.tbRequestFile.ReadOnly = true;
            this.tbRequestFile.Size = new System.Drawing.Size(409, 20);
            this.tbRequestFile.TabIndex = 8;
            // 
            // lblRequestFile
            // 
            this.lblRequestFile.AutoSize = true;
            this.lblRequestFile.Location = new System.Drawing.Point(3, 63);
            this.lblRequestFile.Name = "lblRequestFile";
            this.lblRequestFile.Size = new System.Drawing.Size(69, 13);
            this.lblRequestFile.TabIndex = 2;
            this.lblRequestFile.Text = "Request File:";
            // 
            // btnRequestFile
            // 
            this.btnRequestFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRequestFile.Location = new System.Drawing.Point(511, 56);
            this.btnRequestFile.Name = "btnRequestFile";
            this.btnRequestFile.Size = new System.Drawing.Size(28, 23);
            this.btnRequestFile.TabIndex = 9;
            this.btnRequestFile.Text = "...";
            this.btnRequestFile.UseVisualStyleBackColor = true;
            this.btnRequestFile.Click += new System.EventHandler(this.btnRequestFile_Click);
            // 
            // RequestsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scRequests);
            this.Name = "RequestsPage";
            this.Size = new System.Drawing.Size(821, 487);
            this.scRequests.Panel1.ResumeLayout(false);
            this.scRequests.Panel1.PerformLayout();
            this.scRequests.Panel2.ResumeLayout(false);
            this.scRequests.Panel2.PerformLayout();
            this.scRequests.ResumeLayout(false);
            this.scTraffic.Panel1.ResumeLayout(false);
            this.scTraffic.Panel1.PerformLayout();
            this.scTraffic.Panel2.ResumeLayout(false);
            this.scTraffic.Panel2.PerformLayout();
            this.scTraffic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scRequests;
        private System.Windows.Forms.Label lblTemplates;
        private System.Windows.Forms.TreeView tvTemplates;
        private System.Windows.Forms.SplitContainer scTraffic;
        private System.Windows.Forms.TextBox tbRequest;
        private System.Windows.Forms.Label lblRequest;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.ComboBox cmbService;
        private System.Windows.Forms.Button btnSendRequest;
        private System.Windows.Forms.Label lblServiceAddress;
        private System.Windows.Forms.TextBox tbServiceAddress;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.Button btnAddRequestToTemplates;
        private System.Windows.Forms.TextBox tbRequestFile;
        private System.Windows.Forms.Label lblRequestFile;
        private System.Windows.Forms.Button btnRequestFile;
        private System.Windows.Forms.Button btnDelete;
    }
}
