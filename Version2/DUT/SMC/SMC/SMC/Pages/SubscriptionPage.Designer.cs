namespace SMC.Pages
{
    partial class SubscriptionPage
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
            this.tbEventsReceiver = new System.Windows.Forms.TextBox();
            this.lblReceiverAddress = new System.Windows.Forms.Label();
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnUnsubscribe = new System.Windows.Forms.Button();
            this.tbSubscriptionTime = new System.Windows.Forms.TextBox();
            this.tbRenewTime = new System.Windows.Forms.TextBox();
            this.tbConsole = new System.Windows.Forms.RichTextBox();
            this.tbSubscriptionReference = new System.Windows.Forms.TextBox();
            this.lblSubscriptionReference = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblTerminationTime = new System.Windows.Forms.Label();
            this.tbTerminationTime = new System.Windows.Forms.TextBox();
            this.lblTimeLeft = new System.Windows.Forms.Label();
            this.tbTimeLeft = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tbTopicsFilter = new System.Windows.Forms.TextBox();
            this.tcSubscription = new System.Windows.Forms.TabControl();
            this.tbSubscribe = new System.Windows.Forms.TabPage();
            this.lblInitialTerminationTime = new System.Windows.Forms.Label();
            this.tbNamespaces = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpManageSubscription = new System.Windows.Forms.TabPage();
            this.btnSetSynchronizationPoint = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.tcSubscription.SuspendLayout();
            this.tbSubscribe.SuspendLayout();
            this.tpManageSubscription.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbEventsReceiver
            // 
            this.tbEventsReceiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEventsReceiver.Location = new System.Drawing.Point(133, 9);
            this.tbEventsReceiver.Name = "tbEventsReceiver";
            this.tbEventsReceiver.Size = new System.Drawing.Size(512, 20);
            this.tbEventsReceiver.TabIndex = 3;
            // 
            // lblReceiverAddress
            // 
            this.lblReceiverAddress.AutoSize = true;
            this.lblReceiverAddress.Location = new System.Drawing.Point(6, 12);
            this.lblReceiverAddress.Name = "lblReceiverAddress";
            this.lblReceiverAddress.Size = new System.Drawing.Size(89, 13);
            this.lblReceiverAddress.TabIndex = 2;
            this.lblReceiverAddress.Text = "Events Receiver:";
            // 
            // btnSubscribe
            // 
            this.btnSubscribe.Location = new System.Drawing.Point(570, 88);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(75, 23);
            this.btnSubscribe.TabIndex = 8;
            this.btnSubscribe.Text = "Subscribe";
            this.btnSubscribe.UseVisualStyleBackColor = true;
            this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
            // 
            // btnRenew
            // 
            this.btnRenew.Enabled = false;
            this.btnRenew.Location = new System.Drawing.Point(9, 64);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(118, 23);
            this.btnRenew.TabIndex = 5;
            this.btnRenew.Text = "Renew";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnUnsubscribe
            // 
            this.btnUnsubscribe.Enabled = false;
            this.btnUnsubscribe.Location = new System.Drawing.Point(9, 93);
            this.btnUnsubscribe.Name = "btnUnsubscribe";
            this.btnUnsubscribe.Size = new System.Drawing.Size(118, 23);
            this.btnUnsubscribe.TabIndex = 6;
            this.btnUnsubscribe.Text = "Unsubscribe";
            this.btnUnsubscribe.UseVisualStyleBackColor = true;
            this.btnUnsubscribe.Click += new System.EventHandler(this.btnUnsubscribe_Click);
            // 
            // tbSubscriptionTime
            // 
            this.tbSubscriptionTime.Location = new System.Drawing.Point(133, 88);
            this.tbSubscriptionTime.Name = "tbSubscriptionTime";
            this.tbSubscriptionTime.Size = new System.Drawing.Size(114, 20);
            this.tbSubscriptionTime.TabIndex = 7;
            this.tbSubscriptionTime.Text = "PT20S";
            // 
            // tbRenewTime
            // 
            this.tbRenewTime.Location = new System.Drawing.Point(133, 64);
            this.tbRenewTime.Name = "tbRenewTime";
            this.tbRenewTime.Size = new System.Drawing.Size(114, 20);
            this.tbRenewTime.TabIndex = 4;
            this.tbRenewTime.Text = "PT10S";
            // 
            // tbConsole
            // 
            this.tbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsole.Location = new System.Drawing.Point(3, 160);
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.Size = new System.Drawing.Size(669, 234);
            this.tbConsole.TabIndex = 10;
            this.tbConsole.Text = "";
            // 
            // tbSubscriptionReference
            // 
            this.tbSubscriptionReference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubscriptionReference.Location = new System.Drawing.Point(133, 9);
            this.tbSubscriptionReference.Name = "tbSubscriptionReference";
            this.tbSubscriptionReference.ReadOnly = true;
            this.tbSubscriptionReference.Size = new System.Drawing.Size(528, 20);
            this.tbSubscriptionReference.TabIndex = 1;
            // 
            // lblSubscriptionReference
            // 
            this.lblSubscriptionReference.AutoSize = true;
            this.lblSubscriptionReference.Location = new System.Drawing.Point(6, 12);
            this.lblSubscriptionReference.Name = "lblSubscriptionReference";
            this.lblSubscriptionReference.Size = new System.Drawing.Size(121, 13);
            this.lblSubscriptionReference.TabIndex = 11;
            this.lblSubscriptionReference.Text = "Subscription Reference:";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblTerminationTime
            // 
            this.lblTerminationTime.AutoSize = true;
            this.lblTerminationTime.Location = new System.Drawing.Point(6, 38);
            this.lblTerminationTime.Name = "lblTerminationTime";
            this.lblTerminationTime.Size = new System.Drawing.Size(91, 13);
            this.lblTerminationTime.TabIndex = 13;
            this.lblTerminationTime.Text = "Termination Time:";
            // 
            // tbTerminationTime
            // 
            this.tbTerminationTime.Location = new System.Drawing.Point(133, 35);
            this.tbTerminationTime.Name = "tbTerminationTime";
            this.tbTerminationTime.ReadOnly = true;
            this.tbTerminationTime.Size = new System.Drawing.Size(114, 20);
            this.tbTerminationTime.TabIndex = 2;
            // 
            // lblTimeLeft
            // 
            this.lblTimeLeft.AutoSize = true;
            this.lblTimeLeft.Location = new System.Drawing.Point(283, 38);
            this.lblTimeLeft.Name = "lblTimeLeft";
            this.lblTimeLeft.Size = new System.Drawing.Size(50, 13);
            this.lblTimeLeft.TabIndex = 15;
            this.lblTimeLeft.Text = "Time left:";
            // 
            // tbTimeLeft
            // 
            this.tbTimeLeft.Location = new System.Drawing.Point(410, 35);
            this.tbTimeLeft.Name = "tbTimeLeft";
            this.tbTimeLeft.ReadOnly = true;
            this.tbTimeLeft.Size = new System.Drawing.Size(114, 20);
            this.tbTimeLeft.TabIndex = 3;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(6, 38);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(64, 13);
            this.lblFilter.TabIndex = 17;
            this.lblFilter.Text = "Topics filter:";
            // 
            // tbTopicsFilter
            // 
            this.tbTopicsFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTopicsFilter.Location = new System.Drawing.Point(133, 35);
            this.tbTopicsFilter.Name = "tbTopicsFilter";
            this.tbTopicsFilter.Size = new System.Drawing.Size(512, 20);
            this.tbTopicsFilter.TabIndex = 4;
            // 
            // tcSubscription
            // 
            this.tcSubscription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSubscription.Controls.Add(this.tbSubscribe);
            this.tcSubscription.Controls.Add(this.tpManageSubscription);
            this.tcSubscription.Location = new System.Drawing.Point(3, 3);
            this.tcSubscription.Name = "tcSubscription";
            this.tcSubscription.SelectedIndex = 0;
            this.tcSubscription.Size = new System.Drawing.Size(673, 151);
            this.tcSubscription.TabIndex = 19;
            // 
            // tbSubscribe
            // 
            this.tbSubscribe.Controls.Add(this.lblInitialTerminationTime);
            this.tbSubscribe.Controls.Add(this.tbNamespaces);
            this.tbSubscribe.Controls.Add(this.label1);
            this.tbSubscribe.Controls.Add(this.lblReceiverAddress);
            this.tbSubscribe.Controls.Add(this.tbTopicsFilter);
            this.tbSubscribe.Controls.Add(this.tbEventsReceiver);
            this.tbSubscribe.Controls.Add(this.lblFilter);
            this.tbSubscribe.Controls.Add(this.btnSubscribe);
            this.tbSubscribe.Controls.Add(this.tbSubscriptionTime);
            this.tbSubscribe.Location = new System.Drawing.Point(4, 22);
            this.tbSubscribe.Name = "tbSubscribe";
            this.tbSubscribe.Padding = new System.Windows.Forms.Padding(3);
            this.tbSubscribe.Size = new System.Drawing.Size(665, 125);
            this.tbSubscribe.TabIndex = 0;
            this.tbSubscribe.Text = "Subscribe";
            this.tbSubscribe.UseVisualStyleBackColor = true;
            // 
            // lblInitialTerminationTime
            // 
            this.lblInitialTerminationTime.AutoSize = true;
            this.lblInitialTerminationTime.Location = new System.Drawing.Point(6, 91);
            this.lblInitialTerminationTime.Name = "lblInitialTerminationTime";
            this.lblInitialTerminationTime.Size = new System.Drawing.Size(91, 13);
            this.lblInitialTerminationTime.TabIndex = 20;
            this.lblInitialTerminationTime.Text = "Termination Time:";
            // 
            // tbNamespaces
            // 
            this.tbNamespaces.Location = new System.Drawing.Point(133, 62);
            this.tbNamespaces.Name = "tbNamespaces";
            this.tbNamespaces.Size = new System.Drawing.Size(512, 20);
            this.tbNamespaces.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Namespaces:";
            // 
            // tpManageSubscription
            // 
            this.tpManageSubscription.Controls.Add(this.btnSetSynchronizationPoint);
            this.tpManageSubscription.Controls.Add(this.btnClearLog);
            this.tpManageSubscription.Controls.Add(this.tbSubscriptionReference);
            this.tpManageSubscription.Controls.Add(this.tbTimeLeft);
            this.tpManageSubscription.Controls.Add(this.tbRenewTime);
            this.tpManageSubscription.Controls.Add(this.lblTimeLeft);
            this.tpManageSubscription.Controls.Add(this.btnUnsubscribe);
            this.tpManageSubscription.Controls.Add(this.lblSubscriptionReference);
            this.tpManageSubscription.Controls.Add(this.btnRenew);
            this.tpManageSubscription.Controls.Add(this.lblTerminationTime);
            this.tpManageSubscription.Controls.Add(this.tbTerminationTime);
            this.tpManageSubscription.Location = new System.Drawing.Point(4, 22);
            this.tpManageSubscription.Name = "tpManageSubscription";
            this.tpManageSubscription.Padding = new System.Windows.Forms.Padding(3);
            this.tpManageSubscription.Size = new System.Drawing.Size(665, 125);
            this.tpManageSubscription.TabIndex = 1;
            this.tpManageSubscription.Text = "Manage Subscription";
            this.tpManageSubscription.UseVisualStyleBackColor = true;
            // 
            // btnSetSynchronizationPoint
            // 
            this.btnSetSynchronizationPoint.Enabled = false;
            this.btnSetSynchronizationPoint.Location = new System.Drawing.Point(133, 93);
            this.btnSetSynchronizationPoint.Name = "btnSetSynchronizationPoint";
            this.btnSetSynchronizationPoint.Size = new System.Drawing.Size(114, 23);
            this.btnSetSynchronizationPoint.TabIndex = 17;
            this.btnSetSynchronizationPoint.Text = "SynchronizationPoint";
            this.btnSetSynchronizationPoint.UseVisualStyleBackColor = true;
            this.btnSetSynchronizationPoint.Click += new System.EventHandler(this.btnSetSynchronizationPoint_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(584, 93);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 16;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // SubscriptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcSubscription);
            this.Controls.Add(this.tbConsole);
            this.Name = "SubscriptionPage";
            this.Size = new System.Drawing.Size(679, 418);
            this.tcSubscription.ResumeLayout(false);
            this.tbSubscribe.ResumeLayout(false);
            this.tbSubscribe.PerformLayout();
            this.tpManageSubscription.ResumeLayout(false);
            this.tpManageSubscription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbEventsReceiver;
        private System.Windows.Forms.Label lblReceiverAddress;
        private System.Windows.Forms.Button btnSubscribe;
        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnUnsubscribe;
        private System.Windows.Forms.TextBox tbSubscriptionTime;
        private System.Windows.Forms.TextBox tbRenewTime;
        private System.Windows.Forms.RichTextBox tbConsole;
        private System.Windows.Forms.TextBox tbSubscriptionReference;
        private System.Windows.Forms.Label lblSubscriptionReference;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblTerminationTime;
        private System.Windows.Forms.TextBox tbTerminationTime;
        private System.Windows.Forms.Label lblTimeLeft;
        private System.Windows.Forms.TextBox tbTimeLeft;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TextBox tbTopicsFilter;
        private System.Windows.Forms.TabControl tcSubscription;
        private System.Windows.Forms.TabPage tbSubscribe;
        private System.Windows.Forms.TabPage tpManageSubscription;
        private System.Windows.Forms.TextBox tbNamespaces;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInitialTerminationTime;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnSetSynchronizationPoint;
    }
}
