namespace TestTool.GUI.Controls
{
    partial class LogViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewer));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvCommands = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colTimstamp = new System.Windows.Forms.ColumnHeader();
            this.colService = new System.Windows.Forms.ColumnHeader();
            this.colCommand = new System.Windows.Forms.ColumnHeader();
            this.ilCommandslist = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpOutput = new System.Windows.Forms.TabPage();
            this.tbPlainLog = new System.Windows.Forms.TextBox();
            this.tpDetails = new System.Windows.Forms.TabPage();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpOutput.SuspendLayout();
            this.tpDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvCommands);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(472, 425);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvCommands
            // 
            this.lvCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colTimstamp,
            this.colService,
            this.colCommand});
            this.lvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCommands.FullRowSelect = true;
            this.lvCommands.Location = new System.Drawing.Point(0, 0);
            this.lvCommands.Name = "lvCommands";
            this.lvCommands.Size = new System.Drawing.Size(472, 171);
            this.lvCommands.SmallImageList = this.ilCommandslist;
            this.lvCommands.TabIndex = 0;
            this.lvCommands.UseCompatibleStateImageBehavior = false;
            this.lvCommands.View = System.Windows.Forms.View.Details;
            this.lvCommands.SelectedIndexChanged += new System.EventHandler(this.lvCommands_SelectedIndexChanged);
            // 
            // colId
            // 
            this.colId.Text = "Id";
            this.colId.Width = 46;
            // 
            // colTimstamp
            // 
            this.colTimstamp.Text = "Timestamp";
            this.colTimstamp.Width = 88;
            // 
            // colService
            // 
            this.colService.Text = "Service";
            this.colService.Width = 70;
            // 
            // colCommand
            // 
            this.colCommand.Text = "Command";
            this.colCommand.Width = 306;
            // 
            // ilCommandslist
            // 
            this.ilCommandslist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCommandslist.ImageStream")));
            this.ilCommandslist.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCommandslist.Images.SetKeyName(0, "Selected");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tbRequest);
            this.splitContainer2.Panel1.Controls.Add(this.lblRequest);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tbResponse);
            this.splitContainer2.Panel2.Controls.Add(this.lblResponse);
            this.splitContainer2.Size = new System.Drawing.Size(472, 250);
            this.splitContainer2.SplitterDistance = 236;
            this.splitContainer2.TabIndex = 0;
            // 
            // tbRequest
            // 
            this.tbRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRequest.Location = new System.Drawing.Point(3, 20);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.ReadOnly = true;
            this.tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRequest.Size = new System.Drawing.Size(230, 227);
            this.tbRequest.TabIndex = 1;
            // 
            // lblRequest
            // 
            this.lblRequest.AutoSize = true;
            this.lblRequest.Location = new System.Drawing.Point(4, 4);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(47, 13);
            this.lblRequest.TabIndex = 0;
            this.lblRequest.Text = "Request";
            // 
            // tbResponse
            // 
            this.tbResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResponse.Location = new System.Drawing.Point(3, 20);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResponse.Size = new System.Drawing.Size(226, 227);
            this.tbResponse.TabIndex = 1;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(4, 4);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(55, 13);
            this.lblResponse.TabIndex = 0;
            this.lblResponse.Text = "Response";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpOutput);
            this.tcMain.Controls.Add(this.tpDetails);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(486, 457);
            this.tcMain.TabIndex = 1;
            // 
            // tpOutput
            // 
            this.tpOutput.Controls.Add(this.tbPlainLog);
            this.tpOutput.Location = new System.Drawing.Point(4, 22);
            this.tpOutput.Name = "tpOutput";
            this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutput.Size = new System.Drawing.Size(478, 431);
            this.tpOutput.TabIndex = 0;
            this.tpOutput.Text = "Output";
            this.tpOutput.UseVisualStyleBackColor = true;
            // 
            // tbPlainLog
            // 
            this.tbPlainLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPlainLog.Location = new System.Drawing.Point(6, 6);
            this.tbPlainLog.Multiline = true;
            this.tbPlainLog.Name = "tbPlainLog";
            this.tbPlainLog.ReadOnly = true;
            this.tbPlainLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbPlainLog.Size = new System.Drawing.Size(466, 419);
            this.tbPlainLog.TabIndex = 0;
            // 
            // tpDetails
            // 
            this.tpDetails.Controls.Add(this.splitContainer1);
            this.tpDetails.Location = new System.Drawing.Point(4, 22);
            this.tpDetails.Name = "tpDetails";
            this.tpDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpDetails.Size = new System.Drawing.Size(478, 431);
            this.tpDetails.TabIndex = 1;
            this.tpDetails.Text = "Details";
            this.tpDetails.UseVisualStyleBackColor = true;
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcMain);
            this.Name = "LogViewer";
            this.Size = new System.Drawing.Size(486, 457);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpOutput.ResumeLayout(false);
            this.tpOutput.PerformLayout();
            this.tpDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvCommands;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colTimstamp;
        private System.Windows.Forms.ColumnHeader colCommand;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblRequest;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.TextBox tbRequest;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.TabPage tpDetails;
        private System.Windows.Forms.TextBox tbPlainLog;
        private System.Windows.Forms.ImageList ilCommandslist;
        private System.Windows.Forms.ColumnHeader colService;
    }
}
