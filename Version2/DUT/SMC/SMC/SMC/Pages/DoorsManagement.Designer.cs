namespace SMC.Pages
{
    partial class DoorsManagement
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnMonitor = new System.Windows.Forms.Button();
            this.tvDoors = new System.Windows.Forms.TreeView();
            this.tbListenerUri = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gbDoorOperations = new System.Windows.Forms.GroupBox();
            this.btnLockOpenRelease = new System.Windows.Forms.Button();
            this.btnLockDown = new System.Windows.Forms.Button();
            this.gbAccessDoor = new System.Windows.Forms.GroupBox();
            this.chkUseExtendedTime = new System.Windows.Forms.CheckBox();
            this.tbPreAlarm = new System.Windows.Forms.TextBox();
            this.lblAccessTime = new System.Windows.Forms.Label();
            this.lblPreAlarm = new System.Windows.Forms.Label();
            this.btnAccess = new System.Windows.Forms.Button();
            this.tbAccessTime = new System.Windows.Forms.TextBox();
            this.tbOpenTooLong = new System.Windows.Forms.TextBox();
            this.lblOpenTooLong = new System.Windows.Forms.Label();
            this.btnLockDownRelease = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.btnDoubleLock = new System.Windows.Forms.Button();
            this.btnLockOpen = new System.Windows.Forms.Button();
            this.btnBlock = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.gbDoorProperties = new System.Windows.Forms.GroupBox();
            this.gbCapabilities = new System.Windows.Forms.GroupBox();
            this.chkFault = new System.Windows.Forms.CheckBox();
            this.chkLockMonitor = new System.Windows.Forms.CheckBox();
            this.chkDoubleLockMonitor = new System.Windows.Forms.CheckBox();
            this.chkDoorMonitor = new System.Windows.Forms.CheckBox();
            this.chkTamper = new System.Windows.Forms.CheckBox();
            this.chkAccessTimingOverride = new System.Windows.Forms.CheckBox();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.chkLock = new SMC.Controls.ReadOnlyCheckbox();
            this.chkUnlock = new SMC.Controls.ReadOnlyCheckbox();
            this.chkMomentaryAccess = new SMC.Controls.ReadOnlyCheckbox();
            this.chkBlock = new SMC.Controls.ReadOnlyCheckbox();
            this.chkLockDown = new SMC.Controls.ReadOnlyCheckbox();
            this.chkDoubleLock = new SMC.Controls.ReadOnlyCheckbox();
            this.chkLockOpen = new SMC.Controls.ReadOnlyCheckbox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbDoorOperations.SuspendLayout();
            this.gbAccessDoor.SuspendLayout();
            this.gbDoorProperties.SuspendLayout();
            this.gbCapabilities.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.btnMonitor);
            this.splitContainer1.Panel1.Controls.Add(this.tvDoors);
            this.splitContainer1.Panel1.Controls.Add(this.tbListenerUri);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbDoorOperations);
            this.splitContainer1.Panel2.Controls.Add(this.gbDoorProperties);
            this.splitContainer1.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnMonitor
            // 
            this.btnMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMonitor.Location = new System.Drawing.Point(4, 386);
            this.btnMonitor.Name = "btnMonitor";
            this.btnMonitor.Size = new System.Drawing.Size(75, 23);
            this.btnMonitor.TabIndex = 5;
            this.btnMonitor.Text = "Monitor";
            this.btnMonitor.UseVisualStyleBackColor = true;
            this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
            // 
            // tvDoors
            // 
            this.tvDoors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvDoors.Location = new System.Drawing.Point(0, 0);
            this.tvDoors.Name = "tvDoors";
            this.tvDoors.Size = new System.Drawing.Size(257, 350);
            this.tvDoors.TabIndex = 1;
            this.tvDoors.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvDoors_NodeMouseClick);
            // 
            // tbListenerUri
            // 
            this.tbListenerUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbListenerUri.Location = new System.Drawing.Point(3, 414);
            this.tbListenerUri.Name = "tbListenerUri";
            this.tbListenerUri.Size = new System.Drawing.Size(251, 20);
            this.tbListenerUri.TabIndex = 4;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 356);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gbDoorOperations
            // 
            this.gbDoorOperations.Controls.Add(this.btnLockOpenRelease);
            this.gbDoorOperations.Controls.Add(this.btnLockDown);
            this.gbDoorOperations.Controls.Add(this.gbAccessDoor);
            this.gbDoorOperations.Controls.Add(this.btnLockDownRelease);
            this.gbDoorOperations.Controls.Add(this.btnLock);
            this.gbDoorOperations.Controls.Add(this.btnDoubleLock);
            this.gbDoorOperations.Controls.Add(this.btnLockOpen);
            this.gbDoorOperations.Controls.Add(this.btnBlock);
            this.gbDoorOperations.Controls.Add(this.btnUnlock);
            this.gbDoorOperations.Location = new System.Drawing.Point(5, 230);
            this.gbDoorOperations.Name = "gbDoorOperations";
            this.gbDoorOperations.Size = new System.Drawing.Size(505, 216);
            this.gbDoorOperations.TabIndex = 28;
            this.gbDoorOperations.TabStop = false;
            this.gbDoorOperations.Text = "Door Operations";
            // 
            // btnLockOpenRelease
            // 
            this.btnLockOpenRelease.Location = new System.Drawing.Point(127, 48);
            this.btnLockOpenRelease.Name = "btnLockOpenRelease";
            this.btnLockOpenRelease.Size = new System.Drawing.Size(114, 23);
            this.btnLockOpenRelease.TabIndex = 6;
            this.btnLockOpenRelease.Text = "Lock Open Release";
            this.btnLockOpenRelease.UseVisualStyleBackColor = true;
            this.btnLockOpenRelease.Click += new System.EventHandler(this.btnLockOpenRelease_Click);
            // 
            // btnLockDown
            // 
            this.btnLockDown.Location = new System.Drawing.Point(247, 48);
            this.btnLockDown.Name = "btnLockDown";
            this.btnLockDown.Size = new System.Drawing.Size(115, 23);
            this.btnLockDown.TabIndex = 7;
            this.btnLockDown.Text = "Lock Down";
            this.btnLockDown.UseVisualStyleBackColor = true;
            this.btnLockDown.Click += new System.EventHandler(this.btnLockDown_Click);
            // 
            // gbAccessDoor
            // 
            this.gbAccessDoor.Controls.Add(this.chkUseExtendedTime);
            this.gbAccessDoor.Controls.Add(this.tbPreAlarm);
            this.gbAccessDoor.Controls.Add(this.lblAccessTime);
            this.gbAccessDoor.Controls.Add(this.lblPreAlarm);
            this.gbAccessDoor.Controls.Add(this.btnAccess);
            this.gbAccessDoor.Controls.Add(this.tbAccessTime);
            this.gbAccessDoor.Controls.Add(this.tbOpenTooLong);
            this.gbAccessDoor.Controls.Add(this.lblOpenTooLong);
            this.gbAccessDoor.Location = new System.Drawing.Point(6, 77);
            this.gbAccessDoor.Name = "gbAccessDoor";
            this.gbAccessDoor.Size = new System.Drawing.Size(488, 122);
            this.gbAccessDoor.TabIndex = 24;
            this.gbAccessDoor.TabStop = false;
            this.gbAccessDoor.Text = "Access Door";
            // 
            // chkUseExtendedTime
            // 
            this.chkUseExtendedTime.AutoSize = true;
            this.chkUseExtendedTime.Location = new System.Drawing.Point(6, 19);
            this.chkUseExtendedTime.Name = "chkUseExtendedTime";
            this.chkUseExtendedTime.Size = new System.Drawing.Size(119, 17);
            this.chkUseExtendedTime.TabIndex = 8;
            this.chkUseExtendedTime.Text = "Use Extended Time";
            this.chkUseExtendedTime.ThreeState = true;
            this.chkUseExtendedTime.UseVisualStyleBackColor = true;
            // 
            // tbPreAlarm
            // 
            this.tbPreAlarm.Location = new System.Drawing.Point(133, 92);
            this.tbPreAlarm.Name = "tbPreAlarm";
            this.tbPreAlarm.Size = new System.Drawing.Size(100, 20);
            this.tbPreAlarm.TabIndex = 11;
            this.tbPreAlarm.Text = "PT30S";
            // 
            // lblAccessTime
            // 
            this.lblAccessTime.AutoSize = true;
            this.lblAccessTime.Location = new System.Drawing.Point(6, 43);
            this.lblAccessTime.Name = "lblAccessTime";
            this.lblAccessTime.Size = new System.Drawing.Size(71, 13);
            this.lblAccessTime.TabIndex = 18;
            this.lblAccessTime.Text = "Access Time:";
            // 
            // lblPreAlarm
            // 
            this.lblPreAlarm.AutoSize = true;
            this.lblPreAlarm.Location = new System.Drawing.Point(6, 95);
            this.lblPreAlarm.Name = "lblPreAlarm";
            this.lblPreAlarm.Size = new System.Drawing.Size(80, 13);
            this.lblPreAlarm.TabIndex = 22;
            this.lblPreAlarm.Text = "Pre-alarm Time:";
            // 
            // btnAccess
            // 
            this.btnAccess.Location = new System.Drawing.Point(270, 85);
            this.btnAccess.Name = "btnAccess";
            this.btnAccess.Size = new System.Drawing.Size(86, 23);
            this.btnAccess.TabIndex = 12;
            this.btnAccess.Text = "Access";
            this.btnAccess.UseVisualStyleBackColor = true;
            this.btnAccess.Click += new System.EventHandler(this.btnAccess_Click);
            // 
            // tbAccessTime
            // 
            this.tbAccessTime.Location = new System.Drawing.Point(133, 40);
            this.tbAccessTime.Name = "tbAccessTime";
            this.tbAccessTime.Size = new System.Drawing.Size(100, 20);
            this.tbAccessTime.TabIndex = 9;
            this.tbAccessTime.Text = "PT10S";
            // 
            // tbOpenTooLong
            // 
            this.tbOpenTooLong.Location = new System.Drawing.Point(133, 66);
            this.tbOpenTooLong.Name = "tbOpenTooLong";
            this.tbOpenTooLong.Size = new System.Drawing.Size(100, 20);
            this.tbOpenTooLong.TabIndex = 10;
            this.tbOpenTooLong.Text = "PT20S";
            // 
            // lblOpenTooLong
            // 
            this.lblOpenTooLong.AutoSize = true;
            this.lblOpenTooLong.Location = new System.Drawing.Point(6, 69);
            this.lblOpenTooLong.Name = "lblOpenTooLong";
            this.lblOpenTooLong.Size = new System.Drawing.Size(111, 13);
            this.lblOpenTooLong.TabIndex = 20;
            this.lblOpenTooLong.Text = "Open Too Long Time:";
            // 
            // btnLockDownRelease
            // 
            this.btnLockDownRelease.Location = new System.Drawing.Point(368, 48);
            this.btnLockDownRelease.Name = "btnLockDownRelease";
            this.btnLockDownRelease.Size = new System.Drawing.Size(115, 23);
            this.btnLockDownRelease.TabIndex = 8;
            this.btnLockDownRelease.Text = "Lock Down Release";
            this.btnLockDownRelease.UseVisualStyleBackColor = true;
            this.btnLockDownRelease.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(5, 19);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(115, 23);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // btnDoubleLock
            // 
            this.btnDoubleLock.Location = new System.Drawing.Point(247, 19);
            this.btnDoubleLock.Name = "btnDoubleLock";
            this.btnDoubleLock.Size = new System.Drawing.Size(115, 23);
            this.btnDoubleLock.TabIndex = 3;
            this.btnDoubleLock.Text = "Double Lock";
            this.btnDoubleLock.UseVisualStyleBackColor = true;
            this.btnDoubleLock.Click += new System.EventHandler(this.btnDoubleLock_Click);
            // 
            // btnLockOpen
            // 
            this.btnLockOpen.Location = new System.Drawing.Point(6, 48);
            this.btnLockOpen.Name = "btnLockOpen";
            this.btnLockOpen.Size = new System.Drawing.Size(115, 23);
            this.btnLockOpen.TabIndex = 5;
            this.btnLockOpen.Text = "Lock Open";
            this.btnLockOpen.UseVisualStyleBackColor = true;
            this.btnLockOpen.Click += new System.EventHandler(this.btnLockOpen_Click);
            // 
            // btnBlock
            // 
            this.btnBlock.Location = new System.Drawing.Point(368, 19);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Size = new System.Drawing.Size(115, 23);
            this.btnBlock.TabIndex = 4;
            this.btnBlock.Text = "Block";
            this.btnBlock.UseVisualStyleBackColor = true;
            this.btnBlock.Click += new System.EventHandler(this.btnBlock_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(126, 19);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(115, 23);
            this.btnUnlock.TabIndex = 2;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // gbDoorProperties
            // 
            this.gbDoorProperties.Controls.Add(this.gbCapabilities);
            this.gbDoorProperties.Controls.Add(this.tbName);
            this.gbDoorProperties.Controls.Add(this.lblToken);
            this.gbDoorProperties.Controls.Add(this.tbDescription);
            this.gbDoorProperties.Controls.Add(this.tbToken);
            this.gbDoorProperties.Controls.Add(this.label2);
            this.gbDoorProperties.Controls.Add(this.label1);
            this.gbDoorProperties.Location = new System.Drawing.Point(3, 11);
            this.gbDoorProperties.Name = "gbDoorProperties";
            this.gbDoorProperties.Size = new System.Drawing.Size(507, 213);
            this.gbDoorProperties.TabIndex = 27;
            this.gbDoorProperties.TabStop = false;
            this.gbDoorProperties.Text = "Door Properties";
            // 
            // gbCapabilities
            // 
            this.gbCapabilities.Controls.Add(this.chkFault);
            this.gbCapabilities.Controls.Add(this.chkLockMonitor);
            this.gbCapabilities.Controls.Add(this.chkDoubleLockMonitor);
            this.gbCapabilities.Controls.Add(this.chkDoorMonitor);
            this.gbCapabilities.Controls.Add(this.chkTamper);
            this.gbCapabilities.Controls.Add(this.chkAccessTimingOverride);
            this.gbCapabilities.Controls.Add(this.chkAlarm);
            this.gbCapabilities.Controls.Add(this.chkLock);
            this.gbCapabilities.Controls.Add(this.chkUnlock);
            this.gbCapabilities.Controls.Add(this.chkMomentaryAccess);
            this.gbCapabilities.Controls.Add(this.chkBlock);
            this.gbCapabilities.Controls.Add(this.chkLockDown);
            this.gbCapabilities.Controls.Add(this.chkDoubleLock);
            this.gbCapabilities.Controls.Add(this.chkLockOpen);
            this.gbCapabilities.Location = new System.Drawing.Point(7, 81);
            this.gbCapabilities.Name = "gbCapabilities";
            this.gbCapabilities.Size = new System.Drawing.Size(484, 118);
            this.gbCapabilities.TabIndex = 15;
            this.gbCapabilities.TabStop = false;
            this.gbCapabilities.Text = "Capabilities";
            // 
            // chkFault
            // 
            this.chkFault.AutoSize = true;
            this.chkFault.Location = new System.Drawing.Point(251, 89);
            this.chkFault.Name = "chkFault";
            this.chkFault.Size = new System.Drawing.Size(49, 17);
            this.chkFault.TabIndex = 14;
            this.chkFault.Text = "Fault";
            this.chkFault.UseVisualStyleBackColor = true;
            // 
            // chkLockMonitor
            // 
            this.chkLockMonitor.AutoSize = true;
            this.chkLockMonitor.Location = new System.Drawing.Point(100, 66);
            this.chkLockMonitor.Name = "chkLockMonitor";
            this.chkLockMonitor.Size = new System.Drawing.Size(88, 17);
            this.chkLockMonitor.TabIndex = 13;
            this.chkLockMonitor.Text = "Lock Monitor";
            this.chkLockMonitor.UseVisualStyleBackColor = true;
            // 
            // chkDoubleLockMonitor
            // 
            this.chkDoubleLockMonitor.AutoSize = true;
            this.chkDoubleLockMonitor.Location = new System.Drawing.Point(251, 65);
            this.chkDoubleLockMonitor.Name = "chkDoubleLockMonitor";
            this.chkDoubleLockMonitor.Size = new System.Drawing.Size(119, 17);
            this.chkDoubleLockMonitor.TabIndex = 12;
            this.chkDoubleLockMonitor.Text = "DoubleLockMonitor";
            this.chkDoubleLockMonitor.UseVisualStyleBackColor = true;
            // 
            // chkDoorMonitor
            // 
            this.chkDoorMonitor.AutoSize = true;
            this.chkDoorMonitor.Location = new System.Drawing.Point(6, 66);
            this.chkDoorMonitor.Name = "chkDoorMonitor";
            this.chkDoorMonitor.Size = new System.Drawing.Size(87, 17);
            this.chkDoorMonitor.TabIndex = 11;
            this.chkDoorMonitor.Text = "Door Monitor";
            this.chkDoorMonitor.UseVisualStyleBackColor = true;
            // 
            // chkTamper
            // 
            this.chkTamper.AutoSize = true;
            this.chkTamper.Location = new System.Drawing.Point(100, 89);
            this.chkTamper.Name = "chkTamper";
            this.chkTamper.Size = new System.Drawing.Size(62, 17);
            this.chkTamper.TabIndex = 10;
            this.chkTamper.Text = "Tamper";
            this.chkTamper.UseVisualStyleBackColor = true;
            // 
            // chkAccessTimingOverride
            // 
            this.chkAccessTimingOverride.AutoSize = true;
            this.chkAccessTimingOverride.Location = new System.Drawing.Point(101, 19);
            this.chkAccessTimingOverride.Name = "chkAccessTimingOverride";
            this.chkAccessTimingOverride.Size = new System.Drawing.Size(138, 17);
            this.chkAccessTimingOverride.TabIndex = 9;
            this.chkAccessTimingOverride.Text = "Access Timing Override";
            this.chkAccessTimingOverride.UseVisualStyleBackColor = true;
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Location = new System.Drawing.Point(6, 89);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(52, 17);
            this.chkAlarm.TabIndex = 8;
            this.chkAlarm.Text = "Alarm";
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // chkLock
            // 
            this.chkLock.AutoSize = true;
            this.chkLock.Location = new System.Drawing.Point(6, 43);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(50, 17);
            this.chkLock.TabIndex = 1;
            this.chkLock.Text = "Lock";
            this.chkLock.UseVisualStyleBackColor = true;
            // 
            // chkUnlock
            // 
            this.chkUnlock.AutoSize = true;
            this.chkUnlock.Location = new System.Drawing.Point(377, 43);
            this.chkUnlock.Name = "chkUnlock";
            this.chkUnlock.Size = new System.Drawing.Size(60, 17);
            this.chkUnlock.TabIndex = 2;
            this.chkUnlock.Text = "Unlock";
            this.chkUnlock.UseVisualStyleBackColor = true;
            // 
            // chkMomentaryAccess
            // 
            this.chkMomentaryAccess.AutoSize = true;
            this.chkMomentaryAccess.Location = new System.Drawing.Point(7, 20);
            this.chkMomentaryAccess.Name = "chkMomentaryAccess";
            this.chkMomentaryAccess.Size = new System.Drawing.Size(61, 17);
            this.chkMomentaryAccess.TabIndex = 7;
            this.chkMomentaryAccess.Text = "Access";
            this.chkMomentaryAccess.UseVisualStyleBackColor = true;
            // 
            // chkBlock
            // 
            this.chkBlock.AutoSize = true;
            this.chkBlock.Location = new System.Drawing.Point(251, 19);
            this.chkBlock.Name = "chkBlock";
            this.chkBlock.Size = new System.Drawing.Size(53, 17);
            this.chkBlock.TabIndex = 4;
            this.chkBlock.Text = "Block";
            this.chkBlock.UseVisualStyleBackColor = true;
            // 
            // chkLockDown
            // 
            this.chkLockDown.AutoSize = true;
            this.chkLockDown.Location = new System.Drawing.Point(100, 42);
            this.chkLockDown.Name = "chkLockDown";
            this.chkLockDown.Size = new System.Drawing.Size(81, 17);
            this.chkLockDown.TabIndex = 6;
            this.chkLockDown.Text = "Lock Down";
            this.chkLockDown.UseVisualStyleBackColor = true;
            // 
            // chkDoubleLock
            // 
            this.chkDoubleLock.AutoSize = true;
            this.chkDoubleLock.Location = new System.Drawing.Point(377, 19);
            this.chkDoubleLock.Name = "chkDoubleLock";
            this.chkDoubleLock.Size = new System.Drawing.Size(87, 17);
            this.chkDoubleLock.TabIndex = 3;
            this.chkDoubleLock.Text = "Double Lock";
            this.chkDoubleLock.UseVisualStyleBackColor = true;
            this.chkDoubleLock.CheckedChanged += new System.EventHandler(this.chkDoubleLock_CheckedChanged);
            // 
            // chkLockOpen
            // 
            this.chkLockOpen.AutoSize = true;
            this.chkLockOpen.Location = new System.Drawing.Point(251, 42);
            this.chkLockOpen.Name = "chkLockOpen";
            this.chkLockOpen.Size = new System.Drawing.Size(79, 17);
            this.chkLockOpen.TabIndex = 5;
            this.chkLockOpen.Text = "Lock Open";
            this.chkLockOpen.UseVisualStyleBackColor = true;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(351, 16);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(143, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(9, 19);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(41, 13);
            this.lblToken.TabIndex = 0;
            this.lblToken.Text = "Token:";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(96, 43);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.Size = new System.Drawing.Size(398, 20);
            this.tbDescription.TabIndex = 5;
            // 
            // tbToken
            // 
            this.tbToken.Location = new System.Drawing.Point(96, 16);
            this.tbToken.Name = "tbToken";
            this.tbToken.ReadOnly = true;
            this.tbToken.Size = new System.Drawing.Size(143, 20);
            this.tbToken.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // DoorsManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DoorsManagement";
            this.Size = new System.Drawing.Size(774, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gbDoorOperations.ResumeLayout(false);
            this.gbAccessDoor.ResumeLayout(false);
            this.gbAccessDoor.PerformLayout();
            this.gbDoorProperties.ResumeLayout(false);
            this.gbDoorProperties.PerformLayout();
            this.gbCapabilities.ResumeLayout(false);
            this.gbCapabilities.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox gbDoorOperations;
        private System.Windows.Forms.Button btnLockDown;
        private System.Windows.Forms.GroupBox gbAccessDoor;
        private System.Windows.Forms.CheckBox chkUseExtendedTime;
        private System.Windows.Forms.TextBox tbPreAlarm;
        private System.Windows.Forms.Label lblAccessTime;
        private System.Windows.Forms.Label lblPreAlarm;
        private System.Windows.Forms.Button btnAccess;
        private System.Windows.Forms.TextBox tbAccessTime;
        private System.Windows.Forms.TextBox tbOpenTooLong;
        private System.Windows.Forms.Label lblOpenTooLong;
        private System.Windows.Forms.Button btnLockDownRelease;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.Button btnDoubleLock;
        private System.Windows.Forms.Button btnLockOpen;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.GroupBox gbDoorProperties;
        private System.Windows.Forms.GroupBox gbCapabilities;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbListenerUri;
        private System.Windows.Forms.TreeView tvDoors;
        private System.Windows.Forms.Button btnLockOpenRelease;
        private System.Windows.Forms.Button btnMonitor;
        private SMC.Controls.ReadOnlyCheckbox chkLock;
        private SMC.Controls.ReadOnlyCheckbox chkUnlock;
        private SMC.Controls.ReadOnlyCheckbox chkMomentaryAccess;
        private SMC.Controls.ReadOnlyCheckbox chkBlock;
        private SMC.Controls.ReadOnlyCheckbox chkLockDown;
        private SMC.Controls.ReadOnlyCheckbox chkDoubleLock;
        private SMC.Controls.ReadOnlyCheckbox chkLockOpen;
        private System.Windows.Forms.CheckBox chkLockMonitor;
        private System.Windows.Forms.CheckBox chkDoubleLockMonitor;
        private System.Windows.Forms.CheckBox chkDoorMonitor;
        private System.Windows.Forms.CheckBox chkTamper;
        private System.Windows.Forms.CheckBox chkAccessTimingOverride;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.CheckBox chkFault;

    }
}
