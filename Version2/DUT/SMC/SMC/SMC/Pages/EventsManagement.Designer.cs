namespace SMC.Pages
{
    partial class EventsManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventsManagement));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tvTopics = new System.Windows.Forms.TreeView();
            this.cmsTopics = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyTopicStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNamespacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTopics = new System.Windows.Forms.ImageList(this.components);
            this.tcMessageConstricting = new System.Windows.Forms.TabControl();
            this.tpRawMessage = new System.Windows.Forms.TabPage();
            this.tbMessage = new System.Windows.Forms.RichTextBox();
            this.tpMessageBuilder = new System.Windows.Forms.TabPage();
            this.flpParameters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsTopics.SuspendLayout();
            this.tcMessageConstricting.SuspendLayout();
            this.tpRawMessage.SuspendLayout();
            this.tpMessageBuilder.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.tvTopics);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcMessageConstricting);
            this.splitContainer1.Panel2.Controls.Add(this.btnSend);
            this.splitContainer1.Size = new System.Drawing.Size(695, 449);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(0, 423);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tvTopics
            // 
            this.tvTopics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTopics.ContextMenuStrip = this.cmsTopics;
            this.tvTopics.HideSelection = false;
            this.tvTopics.ImageIndex = 0;
            this.tvTopics.ImageList = this.ilTopics;
            this.tvTopics.Location = new System.Drawing.Point(0, 0);
            this.tvTopics.Name = "tvTopics";
            this.tvTopics.SelectedImageIndex = 0;
            this.tvTopics.Size = new System.Drawing.Size(231, 417);
            this.tvTopics.TabIndex = 0;
            this.tvTopics.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTopics_AfterSelect);
            // 
            // cmsTopics
            // 
            this.cmsTopics.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyTopicStringToolStripMenuItem,
            this.copyNamespacesToolStripMenuItem});
            this.cmsTopics.Name = "cmsTopics";
            this.cmsTopics.Size = new System.Drawing.Size(173, 48);
            this.cmsTopics.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTopics_Opening);
            // 
            // copyTopicStringToolStripMenuItem
            // 
            this.copyTopicStringToolStripMenuItem.Name = "copyTopicStringToolStripMenuItem";
            this.copyTopicStringToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.copyTopicStringToolStripMenuItem.Text = "Copy Topic String";
            this.copyTopicStringToolStripMenuItem.Click += new System.EventHandler(this.copyTopicStringToolStripMenuItem_Click);
            // 
            // copyNamespacesToolStripMenuItem
            // 
            this.copyNamespacesToolStripMenuItem.Name = "copyNamespacesToolStripMenuItem";
            this.copyNamespacesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.copyNamespacesToolStripMenuItem.Text = "Copy Namespaces";
            this.copyNamespacesToolStripMenuItem.Click += new System.EventHandler(this.copyNamespacesToolStripMenuItem_Click);
            // 
            // ilTopics
            // 
            this.ilTopics.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTopics.ImageStream")));
            this.ilTopics.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTopics.Images.SetKeyName(0, "TopicsNamespace");
            this.ilTopics.Images.SetKeyName(1, "Topic");
            this.ilTopics.Images.SetKeyName(2, "PropertyEvent");
            // 
            // tcMessageConstricting
            // 
            this.tcMessageConstricting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMessageConstricting.Controls.Add(this.tpRawMessage);
            this.tcMessageConstricting.Controls.Add(this.tpMessageBuilder);
            this.tcMessageConstricting.Location = new System.Drawing.Point(7, 3);
            this.tcMessageConstricting.Name = "tcMessageConstricting";
            this.tcMessageConstricting.SelectedIndex = 0;
            this.tcMessageConstricting.Size = new System.Drawing.Size(443, 405);
            this.tcMessageConstricting.TabIndex = 3;
            this.tcMessageConstricting.SelectedIndexChanged += new System.EventHandler(this.tcMessageConstricting_SelectedIndexChanged);
            // 
            // tpRawMessage
            // 
            this.tpRawMessage.Controls.Add(this.tbMessage);
            this.tpRawMessage.Location = new System.Drawing.Point(4, 22);
            this.tpRawMessage.Name = "tpRawMessage";
            this.tpRawMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tpRawMessage.Size = new System.Drawing.Size(435, 379);
            this.tpRawMessage.TabIndex = 0;
            this.tpRawMessage.Text = "Raw Message";
            this.tpRawMessage.UseVisualStyleBackColor = true;
            // 
            // tbMessage
            // 
            this.tbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessage.Location = new System.Drawing.Point(9, 6);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(418, 367);
            this.tbMessage.TabIndex = 1;
            this.tbMessage.Text = "";
            // 
            // tpMessageBuilder
            // 
            this.tpMessageBuilder.Controls.Add(this.flpParameters);
            this.tpMessageBuilder.Location = new System.Drawing.Point(4, 22);
            this.tpMessageBuilder.Name = "tpMessageBuilder";
            this.tpMessageBuilder.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessageBuilder.Size = new System.Drawing.Size(435, 379);
            this.tpMessageBuilder.TabIndex = 1;
            this.tpMessageBuilder.Text = "Message Builder";
            this.tpMessageBuilder.UseVisualStyleBackColor = true;
            // 
            // flpParameters
            // 
            this.flpParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpParameters.Location = new System.Drawing.Point(3, 3);
            this.flpParameters.Name = "flpParameters";
            this.flpParameters.Size = new System.Drawing.Size(429, 373);
            this.flpParameters.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(7, 414);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // EventsManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "EventsManagement";
            this.Size = new System.Drawing.Size(695, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.cmsTopics.ResumeLayout(false);
            this.tcMessageConstricting.ResumeLayout(false);
            this.tpRawMessage.ResumeLayout(false);
            this.tpMessageBuilder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TreeView tvTopics;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox tbMessage;
        private System.Windows.Forms.ImageList ilTopics;
        private System.Windows.Forms.ContextMenuStrip cmsTopics;
        private System.Windows.Forms.ToolStripMenuItem copyTopicStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyNamespacesToolStripMenuItem;
        private System.Windows.Forms.TabControl tcMessageConstricting;
        private System.Windows.Forms.TabPage tpRawMessage;
        private System.Windows.Forms.TabPage tpMessageBuilder;
        private System.Windows.Forms.FlowLayoutPanel flpParameters;
    }
}
