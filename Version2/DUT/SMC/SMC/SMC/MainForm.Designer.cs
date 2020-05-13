namespace SMC
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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.tpDoors = new System.Windows.Forms.TabPage();
            this.tpAccessPoints = new System.Windows.Forms.TabPage();
            this.tpAccessRules = new System.Windows.Forms.TabPage();
            this.tpCredentials = new System.Windows.Forms.TabPage();
            this.tbSchedule = new System.Windows.Forms.TabPage();
            this.tpSensors = new System.Windows.Forms.TabPage();
            this.tpEvents = new System.Windows.Forms.TabPage();
            this.tpConfiguration = new System.Windows.Forms.TabPage();
            this.tpLogging = new System.Windows.Forms.TabPage();
            this.tbSubscription = new System.Windows.Forms.TabPage();
            this.generalSettings = new SMC.Pages.GeneralSettings();
            this.doorsManagement = new SMC.Pages.DoorsManagement();
            this.accessPointsManagement1 = new SMC.Pages.AccessPointsManagement();
            this.accessRulesManagement = new SMC.Pages.AccessRulesManagement();
            this.credentialManagement = new SMC.Pages.CredentialManagement();
            this.scheduleManagement = new SMC.Pages.ScheduleManagement();
            this.sensorControlPage = new SMC.Pages.SensorControlPage();
            this.eventsManagement = new SMC.Pages.EventsManagement();
            this.configurationPage = new SMC.Pages.ConfigurationPage();
            this.loggingPage = new SMC.Pages.LoggingPage();
            this.subscriptionPage1 = new SMC.Pages.SubscriptionPage();
            this.tcMain.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpDoors.SuspendLayout();
            this.tpAccessPoints.SuspendLayout();
            this.tpAccessRules.SuspendLayout();
            this.tpCredentials.SuspendLayout();
            this.tbSchedule.SuspendLayout();
            this.tpSensors.SuspendLayout();
            this.tpEvents.SuspendLayout();
            this.tpConfiguration.SuspendLayout();
            this.tpLogging.SuspendLayout();
            this.tbSubscription.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpGeneral);
            this.tcMain.Controls.Add(this.tpDoors);
            this.tcMain.Controls.Add(this.tpAccessPoints);
            this.tcMain.Controls.Add(this.tpAccessRules);
            this.tcMain.Controls.Add(this.tpCredentials);
            this.tcMain.Controls.Add(this.tbSchedule);
            this.tcMain.Controls.Add(this.tpSensors);
            this.tcMain.Controls.Add(this.tpEvents);
            this.tcMain.Controls.Add(this.tpConfiguration);
            this.tcMain.Controls.Add(this.tpLogging);
            this.tcMain.Controls.Add(this.tbSubscription);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(942, 538);
            this.tcMain.TabIndex = 0;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.generalSettings);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(934, 512);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // tpDoors
            // 
            this.tpDoors.Controls.Add(this.doorsManagement);
            this.tpDoors.Location = new System.Drawing.Point(4, 22);
            this.tpDoors.Name = "tpDoors";
            this.tpDoors.Padding = new System.Windows.Forms.Padding(3);
            this.tpDoors.Size = new System.Drawing.Size(934, 535);
            this.tpDoors.TabIndex = 1;
            this.tpDoors.Text = "Doors Management";
            this.tpDoors.UseVisualStyleBackColor = true;
            // 
            // tpAccessPoints
            // 
            this.tpAccessPoints.Controls.Add(this.accessPointsManagement1);
            this.tpAccessPoints.Location = new System.Drawing.Point(4, 22);
            this.tpAccessPoints.Name = "tpAccessPoints";
            this.tpAccessPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tpAccessPoints.Size = new System.Drawing.Size(934, 535);
            this.tpAccessPoints.TabIndex = 8;
            this.tpAccessPoints.Text = "Access Points Management";
            this.tpAccessPoints.UseVisualStyleBackColor = true;
            // 
            // tpAccessRules
            // 
            this.tpAccessRules.Controls.Add(this.accessRulesManagement);
            this.tpAccessRules.Location = new System.Drawing.Point(4, 22);
            this.tpAccessRules.Name = "tpAccessRules";
            this.tpAccessRules.Padding = new System.Windows.Forms.Padding(3);
            this.tpAccessRules.Size = new System.Drawing.Size(934, 535);
            this.tpAccessRules.TabIndex = 9;
            this.tpAccessRules.Text = "Access Profiles Management";
            this.tpAccessRules.UseVisualStyleBackColor = true;
            // 
            // tpCredentials
            // 
            this.tpCredentials.Controls.Add(this.credentialManagement);
            this.tpCredentials.Location = new System.Drawing.Point(4, 22);
            this.tpCredentials.Name = "tpCredentials";
            this.tpCredentials.Padding = new System.Windows.Forms.Padding(3);
            this.tpCredentials.Size = new System.Drawing.Size(934, 535);
            this.tpCredentials.TabIndex = 9;
            this.tpCredentials.Text = "Credentials Management";
            this.tpCredentials.UseVisualStyleBackColor = true;
            // 
            // tbSchedule
            // 
            this.tbSchedule.Controls.Add(this.scheduleManagement);
            this.tbSchedule.Location = new System.Drawing.Point(4, 22);
            this.tbSchedule.Name = "tbSchedule";
            this.tbSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tbSchedule.Size = new System.Drawing.Size(934, 535);
            this.tbSchedule.TabIndex = 10;
            this.tbSchedule.Text = "Schedule Management";
            this.tbSchedule.UseVisualStyleBackColor = true;
            // 
            // tpSensors
            // 
            this.tpSensors.Controls.Add(this.sensorControlPage);
            this.tpSensors.Location = new System.Drawing.Point(4, 22);
            this.tpSensors.Name = "tpSensors";
            this.tpSensors.Padding = new System.Windows.Forms.Padding(3);
            this.tpSensors.Size = new System.Drawing.Size(934, 535);
            this.tpSensors.TabIndex = 7;
            this.tpSensors.Text = "Sensors";
            this.tpSensors.UseVisualStyleBackColor = true;
            // 
            // tpEvents
            // 
            this.tpEvents.Controls.Add(this.eventsManagement);
            this.tpEvents.Location = new System.Drawing.Point(4, 22);
            this.tpEvents.Name = "tpEvents";
            this.tpEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpEvents.Size = new System.Drawing.Size(934, 535);
            this.tpEvents.TabIndex = 2;
            this.tpEvents.Text = "Events Control";
            this.tpEvents.UseVisualStyleBackColor = true;
            // 
            // tpConfiguration
            // 
            this.tpConfiguration.Controls.Add(this.configurationPage);
            this.tpConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tpConfiguration.Name = "tpConfiguration";
            this.tpConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfiguration.Size = new System.Drawing.Size(934, 535);
            this.tpConfiguration.TabIndex = 4;
            this.tpConfiguration.Text = "Configuration";
            this.tpConfiguration.UseVisualStyleBackColor = true;
            // 
            // tpLogging
            // 
            this.tpLogging.Controls.Add(this.loggingPage);
            this.tpLogging.Location = new System.Drawing.Point(4, 22);
            this.tpLogging.Name = "tpLogging";
            this.tpLogging.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogging.Size = new System.Drawing.Size(934, 535);
            this.tpLogging.TabIndex = 3;
            this.tpLogging.Text = "Log";
            this.tpLogging.UseVisualStyleBackColor = true;
            // 
            // tbSubscription
            // 
            this.tbSubscription.Controls.Add(this.subscriptionPage1);
            this.tbSubscription.Location = new System.Drawing.Point(4, 22);
            this.tbSubscription.Name = "tbSubscription";
            this.tbSubscription.Padding = new System.Windows.Forms.Padding(3);
            this.tbSubscription.Size = new System.Drawing.Size(934, 535);
            this.tbSubscription.TabIndex = 6;
            this.tbSubscription.Text = "Subscription";
            this.tbSubscription.UseVisualStyleBackColor = true;
            // 
            // generalSettings
            // 
            this.generalSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalSettings.Location = new System.Drawing.Point(3, 3);
            this.generalSettings.Name = "generalSettings";
            this.generalSettings.Size = new System.Drawing.Size(928, 506);
            this.generalSettings.TabIndex = 0;
            // 
            // doorsManagement
            // 
            this.doorsManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doorsManagement.Location = new System.Drawing.Point(3, 3);
            this.doorsManagement.Name = "doorsManagement";
            this.doorsManagement.Size = new System.Drawing.Size(928, 529);
            this.doorsManagement.TabIndex = 0;
            // 
            // accessPointsManagement1
            // 
            this.accessPointsManagement1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accessPointsManagement1.Location = new System.Drawing.Point(3, 3);
            this.accessPointsManagement1.Name = "accessPointsManagement1";
            this.accessPointsManagement1.Size = new System.Drawing.Size(928, 529);
            this.accessPointsManagement1.TabIndex = 0;
            // 
            // accessRulesManagement
            // 
            this.accessRulesManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accessRulesManagement.Location = new System.Drawing.Point(3, 3);
            this.accessRulesManagement.Name = "accessRulesManagement";
            this.accessRulesManagement.Size = new System.Drawing.Size(928, 529);
            this.accessRulesManagement.TabIndex = 0;
            // 
            // credentialManagement
            // 
            this.credentialManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.credentialManagement.Location = new System.Drawing.Point(3, 3);
            this.credentialManagement.Name = "credentialManagement";
            this.credentialManagement.Size = new System.Drawing.Size(928, 529);
            this.credentialManagement.TabIndex = 0;
            // 
            // scheduleManagement
            // 
            this.scheduleManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scheduleManagement.Location = new System.Drawing.Point(3, 3);
            this.scheduleManagement.Name = "scheduleManagement";
            this.scheduleManagement.Size = new System.Drawing.Size(928, 529);
            this.scheduleManagement.TabIndex = 0;
            // 
            // sensorControlPage
            // 
            this.sensorControlPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sensorControlPage.Location = new System.Drawing.Point(3, 3);
            this.sensorControlPage.Name = "sensorControlPage";
            this.sensorControlPage.Size = new System.Drawing.Size(928, 529);
            this.sensorControlPage.TabIndex = 0;
            // 
            // eventsManagement
            // 
            this.eventsManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsManagement.Location = new System.Drawing.Point(3, 3);
            this.eventsManagement.Name = "eventsManagement";
            this.eventsManagement.Size = new System.Drawing.Size(928, 529);
            this.eventsManagement.TabIndex = 0;
            // 
            // configurationPage
            // 
            this.configurationPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationPage.Location = new System.Drawing.Point(3, 3);
            this.configurationPage.Name = "configurationPage";
            this.configurationPage.Size = new System.Drawing.Size(928, 529);
            this.configurationPage.TabIndex = 0;
            // 
            // loggingPage
            // 
            this.loggingPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggingPage.Location = new System.Drawing.Point(3, 3);
            this.loggingPage.Name = "loggingPage";
            this.loggingPage.Size = new System.Drawing.Size(928, 529);
            this.loggingPage.TabIndex = 0;
            // 
            // subscriptionPage1
            // 
            this.subscriptionPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subscriptionPage1.Location = new System.Drawing.Point(3, 3);
            this.subscriptionPage1.Name = "subscriptionPage1";
            this.subscriptionPage1.Size = new System.Drawing.Size(928, 529);
            this.subscriptionPage1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 538);
            this.Controls.Add(this.tcMain);
            this.MinimumSize = new System.Drawing.Size(958, 577);
            this.Name = "MainForm";
            this.Text = "Simulator Management Console";
            this.tcMain.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpDoors.ResumeLayout(false);
            this.tpAccessPoints.ResumeLayout(false);
            this.tpAccessRules.ResumeLayout(false);
            this.tpCredentials.ResumeLayout(false);
            this.tbSchedule.ResumeLayout(false);
            this.tpSensors.ResumeLayout(false);
            this.tpEvents.ResumeLayout(false);
            this.tpConfiguration.ResumeLayout(false);
            this.tpLogging.ResumeLayout(false);
            this.tbSubscription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpDoors;
        private SMC.Pages.GeneralSettings generalSettings;
        private SMC.Pages.DoorsManagement doorsManagement;
        private System.Windows.Forms.TabPage tpEvents;
        private SMC.Pages.EventsManagement eventsManagement;
        private System.Windows.Forms.TabPage tpLogging;
        private SMC.Pages.LoggingPage loggingPage;
        private System.Windows.Forms.TabPage tpConfiguration;
        private SMC.Pages.ConfigurationPage configurationPage;
        private System.Windows.Forms.TabPage tbSubscription;
        private SMC.Pages.SubscriptionPage subscriptionPage1;
        private System.Windows.Forms.TabPage tpSensors;
        private SMC.Pages.SensorControlPage sensorControlPage;
        private System.Windows.Forms.TabPage tpAccessPoints;
        private SMC.Pages.AccessPointsManagement accessPointsManagement1;
        private System.Windows.Forms.TabPage tpAccessRules;
        private SMC.Pages.AccessRulesManagement accessRulesManagement;
        private System.Windows.Forms.TabPage tpCredentials;
        private SMC.Pages.CredentialManagement credentialManagement;
        private System.Windows.Forms.TabPage tbSchedule;
        private SMC.Pages.ScheduleManagement scheduleManagement;
    }
}

