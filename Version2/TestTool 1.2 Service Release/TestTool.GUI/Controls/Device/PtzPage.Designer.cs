namespace TestTool.GUI.Controls.Device
{
    partial class PtzPage
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
            this.btnVideo = new System.Windows.Forms.Button();
            this.btnStopAll = new System.Windows.Forms.Button();
            this.btnStartAll = new System.Windows.Forms.Button();
            this.btnStopZoom = new System.Windows.Forms.Button();
            this.btnStartZoom = new System.Windows.Forms.Button();
            this.lblVZoom = new System.Windows.Forms.Label();
            this.nudVzoom = new System.Windows.Forms.NumericUpDown();
            this.lblVy = new System.Windows.Forms.Label();
            this.nudVy = new System.Windows.Forms.NumericUpDown();
            this.btnVxStopMove = new System.Windows.Forms.Button();
            this.btnVxStartMove = new System.Windows.Forms.Button();
            this.lblVx = new System.Windows.Forms.Label();
            this.nudVx = new System.Windows.Forms.NumericUpDown();
            this.rbAbsoluteMove = new System.Windows.Forms.RadioButton();
            this.btnZoomFromMinToMax = new System.Windows.Forms.Button();
            this.btnZoomMax = new System.Windows.Forms.Button();
            this.btnZoomMin = new System.Windows.Forms.Button();
            this.lblZoom = new System.Windows.Forms.Label();
            this.nudZoom = new System.Windows.Forms.NumericUpDown();
            this.btnYFromMinToMax = new System.Windows.Forms.Button();
            this.btnYMax = new System.Windows.Forms.Button();
            this.btnYMin = new System.Windows.Forms.Button();
            this.lblY = new System.Windows.Forms.Label();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.btnXFromMinToMax = new System.Windows.Forms.Button();
            this.btnXMax = new System.Windows.Forms.Button();
            this.btnXMin = new System.Windows.Forms.Button();
            this.lblX = new System.Windows.Forms.Label();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.rbContuniousMove = new System.Windows.Forms.RadioButton();
            this.btnGetPtzUrl = new System.Windows.Forms.Button();
            this.tbPtzUrl = new System.Windows.Forms.TextBox();
            this.lblPtzUrl = new System.Windows.Forms.Label();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.panelAbsoluteMove = new System.Windows.Forms.Panel();
            this.panelContiniusMove = new System.Windows.Forms.Panel();
            this.tbTimeout = new System.Windows.Forms.TextBox();
            this.chkUseTimeout = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPTZProfiles = new System.Windows.Forms.ComboBox();
            this.btnGetProfiles = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMediaUrl = new System.Windows.Forms.TextBox();
            this.btnAddPTZConfig = new System.Windows.Forms.Button();
            this.rbRelativeMove = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudVzoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            this.panelAbsoluteMove.SuspendLayout();
            this.panelContiniusMove.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVideo
            // 
            this.btnVideo.Location = new System.Drawing.Point(8, 420);
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(75, 23);
            this.btnVideo.TabIndex = 33;
            this.btnVideo.Text = "Play Video";
            this.btnVideo.UseVisualStyleBackColor = true;
            this.btnVideo.Click += new System.EventHandler(this.btnVideo_Click);
            // 
            // btnStopAll
            // 
            this.btnStopAll.Location = new System.Drawing.Point(200, 91);
            this.btnStopAll.Name = "btnStopAll";
            this.btnStopAll.Size = new System.Drawing.Size(68, 23);
            this.btnStopAll.TabIndex = 30;
            this.btnStopAll.Text = "Stop All";
            this.btnStopAll.UseVisualStyleBackColor = true;
            this.btnStopAll.Click += new System.EventHandler(this.btnStopAll_Click);
            // 
            // btnStartAll
            // 
            this.btnStartAll.Location = new System.Drawing.Point(126, 91);
            this.btnStartAll.Name = "btnStartAll";
            this.btnStartAll.Size = new System.Drawing.Size(68, 23);
            this.btnStartAll.TabIndex = 29;
            this.btnStartAll.Text = "Start All";
            this.btnStartAll.UseVisualStyleBackColor = true;
            this.btnStartAll.Click += new System.EventHandler(this.btnStartAll_Click);
            // 
            // btnStopZoom
            // 
            this.btnStopZoom.Location = new System.Drawing.Point(200, 62);
            this.btnStopZoom.Name = "btnStopZoom";
            this.btnStopZoom.Size = new System.Drawing.Size(68, 23);
            this.btnStopZoom.TabIndex = 28;
            this.btnStopZoom.Text = "Stop Zoom";
            this.btnStopZoom.UseVisualStyleBackColor = true;
            this.btnStopZoom.Click += new System.EventHandler(this.btnStopZoom_Click);
            // 
            // btnStartZoom
            // 
            this.btnStartZoom.Location = new System.Drawing.Point(126, 62);
            this.btnStartZoom.Name = "btnStartZoom";
            this.btnStartZoom.Size = new System.Drawing.Size(68, 23);
            this.btnStartZoom.TabIndex = 27;
            this.btnStartZoom.Text = "Start Zoom";
            this.btnStartZoom.UseVisualStyleBackColor = true;
            this.btnStartZoom.Click += new System.EventHandler(this.btnStartZoom_Click);
            // 
            // lblVZoom
            // 
            this.lblVZoom.AutoSize = true;
            this.lblVZoom.Location = new System.Drawing.Point(11, 67);
            this.lblVZoom.Name = "lblVZoom";
            this.lblVZoom.Size = new System.Drawing.Size(44, 13);
            this.lblVZoom.TabIndex = 75;
            this.lblVZoom.Text = "VZoom:";
            // 
            // nudVzoom
            // 
            this.nudVzoom.DecimalPlaces = 2;
            this.nudVzoom.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVzoom.Location = new System.Drawing.Point(56, 65);
            this.nudVzoom.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudVzoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudVzoom.Name = "nudVzoom";
            this.nudVzoom.Size = new System.Drawing.Size(64, 20);
            this.nudVzoom.TabIndex = 26;
            // 
            // lblVy
            // 
            this.lblVy.AutoSize = true;
            this.lblVy.Location = new System.Drawing.Point(11, 38);
            this.lblVy.Name = "lblVy";
            this.lblVy.Size = new System.Drawing.Size(24, 13);
            this.lblVy.TabIndex = 73;
            this.lblVy.Text = "VY:";
            // 
            // nudVy
            // 
            this.nudVy.DecimalPlaces = 2;
            this.nudVy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVy.Location = new System.Drawing.Point(56, 36);
            this.nudVy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudVy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudVy.Name = "nudVy";
            this.nudVy.Size = new System.Drawing.Size(64, 20);
            this.nudVy.TabIndex = 25;
            // 
            // btnVxStopMove
            // 
            this.btnVxStopMove.Location = new System.Drawing.Point(200, 5);
            this.btnVxStopMove.Name = "btnVxStopMove";
            this.btnVxStopMove.Size = new System.Drawing.Size(68, 23);
            this.btnVxStopMove.TabIndex = 24;
            this.btnVxStopMove.Text = "Stop Move";
            this.btnVxStopMove.UseVisualStyleBackColor = true;
            this.btnVxStopMove.Click += new System.EventHandler(this.btnVxStopMove_Click);
            // 
            // btnVxStartMove
            // 
            this.btnVxStartMove.Location = new System.Drawing.Point(126, 5);
            this.btnVxStartMove.Name = "btnVxStartMove";
            this.btnVxStartMove.Size = new System.Drawing.Size(68, 23);
            this.btnVxStartMove.TabIndex = 23;
            this.btnVxStartMove.Text = "Start Move";
            this.btnVxStartMove.UseVisualStyleBackColor = true;
            this.btnVxStartMove.Click += new System.EventHandler(this.btnVxStartMove_Click);
            // 
            // lblVx
            // 
            this.lblVx.AutoSize = true;
            this.lblVx.Location = new System.Drawing.Point(10, 9);
            this.lblVx.Name = "lblVx";
            this.lblVx.Size = new System.Drawing.Size(24, 13);
            this.lblVx.TabIndex = 69;
            this.lblVx.Text = "VX:";
            // 
            // nudVx
            // 
            this.nudVx.DecimalPlaces = 2;
            this.nudVx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVx.Location = new System.Drawing.Point(56, 7);
            this.nudVx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudVx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudVx.Name = "nudVx";
            this.nudVx.Size = new System.Drawing.Size(64, 20);
            this.nudVx.TabIndex = 22;
            // 
            // rbAbsoluteMove
            // 
            this.rbAbsoluteMove.AutoSize = true;
            this.rbAbsoluteMove.Checked = true;
            this.rbAbsoluteMove.Location = new System.Drawing.Point(3, 131);
            this.rbAbsoluteMove.Name = "rbAbsoluteMove";
            this.rbAbsoluteMove.Size = new System.Drawing.Size(96, 17);
            this.rbAbsoluteMove.TabIndex = 3;
            this.rbAbsoluteMove.TabStop = true;
            this.rbAbsoluteMove.Text = "Absolute Move";
            this.rbAbsoluteMove.UseVisualStyleBackColor = true;
            this.rbAbsoluteMove.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // btnZoomFromMinToMax
            // 
            this.btnZoomFromMinToMax.Location = new System.Drawing.Point(241, 60);
            this.btnZoomFromMinToMax.Name = "btnZoomFromMinToMax";
            this.btnZoomFromMinToMax.Size = new System.Drawing.Size(115, 23);
            this.btnZoomFromMinToMax.TabIndex = 16;
            this.btnZoomFromMinToMax.Text = "From Min To Max";
            this.btnZoomFromMinToMax.UseVisualStyleBackColor = true;
            this.btnZoomFromMinToMax.Click += new System.EventHandler(this.btnZoomFromMinToMax_Click);
            // 
            // btnZoomMax
            // 
            this.btnZoomMax.Location = new System.Drawing.Point(183, 61);
            this.btnZoomMax.Name = "btnZoomMax";
            this.btnZoomMax.Size = new System.Drawing.Size(52, 23);
            this.btnZoomMax.TabIndex = 15;
            this.btnZoomMax.Text = "Max";
            this.btnZoomMax.UseVisualStyleBackColor = true;
            this.btnZoomMax.Click += new System.EventHandler(this.btnZoomMax_Click);
            // 
            // btnZoomMin
            // 
            this.btnZoomMin.Location = new System.Drawing.Point(125, 61);
            this.btnZoomMin.Name = "btnZoomMin";
            this.btnZoomMin.Size = new System.Drawing.Size(52, 23);
            this.btnZoomMin.TabIndex = 14;
            this.btnZoomMin.Text = "Min";
            this.btnZoomMin.UseVisualStyleBackColor = true;
            this.btnZoomMin.Click += new System.EventHandler(this.btnZoomMin_Click);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Location = new System.Drawing.Point(10, 65);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(37, 13);
            this.lblZoom.TabIndex = 63;
            this.lblZoom.Text = "Zoom:";
            // 
            // nudZoom
            // 
            this.nudZoom.DecimalPlaces = 2;
            this.nudZoom.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudZoom.Location = new System.Drawing.Point(55, 63);
            this.nudZoom.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudZoom.Name = "nudZoom";
            this.nudZoom.Size = new System.Drawing.Size(64, 20);
            this.nudZoom.TabIndex = 13;
            this.nudZoom.ValueChanged += new System.EventHandler(this.nudZoom_ValueChanged);
            // 
            // btnYFromMinToMax
            // 
            this.btnYFromMinToMax.Location = new System.Drawing.Point(241, 31);
            this.btnYFromMinToMax.Name = "btnYFromMinToMax";
            this.btnYFromMinToMax.Size = new System.Drawing.Size(115, 23);
            this.btnYFromMinToMax.TabIndex = 12;
            this.btnYFromMinToMax.Text = "From Min To Max";
            this.btnYFromMinToMax.UseVisualStyleBackColor = true;
            this.btnYFromMinToMax.Click += new System.EventHandler(this.btnYFromMinToMax_Click);
            // 
            // btnYMax
            // 
            this.btnYMax.Location = new System.Drawing.Point(183, 31);
            this.btnYMax.Name = "btnYMax";
            this.btnYMax.Size = new System.Drawing.Size(52, 23);
            this.btnYMax.TabIndex = 11;
            this.btnYMax.Text = "Max";
            this.btnYMax.UseVisualStyleBackColor = true;
            this.btnYMax.Click += new System.EventHandler(this.btnYMax_Click);
            // 
            // btnYMin
            // 
            this.btnYMin.Location = new System.Drawing.Point(125, 31);
            this.btnYMin.Name = "btnYMin";
            this.btnYMin.Size = new System.Drawing.Size(52, 23);
            this.btnYMin.TabIndex = 10;
            this.btnYMin.Text = "Min";
            this.btnYMin.UseVisualStyleBackColor = true;
            this.btnYMin.Click += new System.EventHandler(this.btnYMin_Click);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(10, 36);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 58;
            this.lblY.Text = "Y:";
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 2;
            this.nudY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudY.Location = new System.Drawing.Point(55, 34);
            this.nudY.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(64, 20);
            this.nudY.TabIndex = 9;
            this.nudY.ValueChanged += new System.EventHandler(this.nudY_ValueChanged);
            // 
            // btnXFromMinToMax
            // 
            this.btnXFromMinToMax.Location = new System.Drawing.Point(241, 3);
            this.btnXFromMinToMax.Name = "btnXFromMinToMax";
            this.btnXFromMinToMax.Size = new System.Drawing.Size(115, 23);
            this.btnXFromMinToMax.TabIndex = 8;
            this.btnXFromMinToMax.Text = "From Min To Max";
            this.btnXFromMinToMax.UseVisualStyleBackColor = true;
            this.btnXFromMinToMax.Click += new System.EventHandler(this.btnXFromMinToMax_Click);
            // 
            // btnXMax
            // 
            this.btnXMax.Location = new System.Drawing.Point(183, 3);
            this.btnXMax.Name = "btnXMax";
            this.btnXMax.Size = new System.Drawing.Size(52, 23);
            this.btnXMax.TabIndex = 7;
            this.btnXMax.Text = "Max";
            this.btnXMax.UseVisualStyleBackColor = true;
            this.btnXMax.Click += new System.EventHandler(this.btnXMax_Click);
            // 
            // btnXMin
            // 
            this.btnXMin.Location = new System.Drawing.Point(125, 3);
            this.btnXMin.Name = "btnXMin";
            this.btnXMin.Size = new System.Drawing.Size(52, 23);
            this.btnXMin.TabIndex = 6;
            this.btnXMin.Text = "Min";
            this.btnXMin.UseVisualStyleBackColor = true;
            this.btnXMin.Click += new System.EventHandler(this.btnXMin_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(9, 7);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 53;
            this.lblX.Text = "X:";
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 2;
            this.nudX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudX.Location = new System.Drawing.Point(55, 5);
            this.nudX.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(64, 20);
            this.nudX.TabIndex = 5;
            this.nudX.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // rbContuniousMove
            // 
            this.rbContuniousMove.AutoSize = true;
            this.rbContuniousMove.Location = new System.Drawing.Point(3, 241);
            this.rbContuniousMove.Name = "rbContuniousMove";
            this.rbContuniousMove.Size = new System.Drawing.Size(108, 17);
            this.rbContuniousMove.TabIndex = 20;
            this.rbContuniousMove.TabStop = true;
            this.rbContuniousMove.Text = "Continuous Move";
            this.rbContuniousMove.UseVisualStyleBackColor = true;
            this.rbContuniousMove.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // btnGetPtzUrl
            // 
            this.btnGetPtzUrl.Location = new System.Drawing.Point(3, 66);
            this.btnGetPtzUrl.Name = "btnGetPtzUrl";
            this.btnGetPtzUrl.Size = new System.Drawing.Size(90, 23);
            this.btnGetPtzUrl.TabIndex = 2;
            this.btnGetPtzUrl.Text = "Get URLs";
            this.btnGetPtzUrl.UseVisualStyleBackColor = true;
            this.btnGetPtzUrl.Click += new System.EventHandler(this.btnGetPtzUrl_Click);
            // 
            // tbPtzUrl
            // 
            this.tbPtzUrl.Location = new System.Drawing.Point(100, 31);
            this.tbPtzUrl.Name = "tbPtzUrl";
            this.tbPtzUrl.ReadOnly = true;
            this.tbPtzUrl.Size = new System.Drawing.Size(264, 20);
            this.tbPtzUrl.TabIndex = 1;
            // 
            // lblPtzUrl
            // 
            this.lblPtzUrl.AutoSize = true;
            this.lblPtzUrl.Location = new System.Drawing.Point(3, 7);
            this.lblPtzUrl.Name = "lblPtzUrl";
            this.lblPtzUrl.Size = new System.Drawing.Size(64, 13);
            this.lblPtzUrl.TabIndex = 48;
            this.lblPtzUrl.Text = "Media URL:";
            // 
            // tbReport
            // 
            this.tbReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReport.Location = new System.Drawing.Point(371, 4);
            this.tbReport.Multiline = true;
            this.tbReport.Name = "tbReport";
            this.tbReport.ReadOnly = true;
            this.tbReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReport.Size = new System.Drawing.Size(390, 480);
            this.tbReport.TabIndex = 34;
            // 
            // panelAbsoluteMove
            // 
            this.panelAbsoluteMove.Controls.Add(this.lblX);
            this.panelAbsoluteMove.Controls.Add(this.nudX);
            this.panelAbsoluteMove.Controls.Add(this.btnXMin);
            this.panelAbsoluteMove.Controls.Add(this.btnXMax);
            this.panelAbsoluteMove.Controls.Add(this.btnXFromMinToMax);
            this.panelAbsoluteMove.Controls.Add(this.nudY);
            this.panelAbsoluteMove.Controls.Add(this.lblY);
            this.panelAbsoluteMove.Controls.Add(this.btnYMin);
            this.panelAbsoluteMove.Controls.Add(this.btnYMax);
            this.panelAbsoluteMove.Controls.Add(this.btnYFromMinToMax);
            this.panelAbsoluteMove.Controls.Add(this.nudZoom);
            this.panelAbsoluteMove.Controls.Add(this.lblZoom);
            this.panelAbsoluteMove.Controls.Add(this.btnZoomMin);
            this.panelAbsoluteMove.Controls.Add(this.btnZoomMax);
            this.panelAbsoluteMove.Controls.Add(this.btnZoomFromMinToMax);
            this.panelAbsoluteMove.Location = new System.Drawing.Point(8, 148);
            this.panelAbsoluteMove.Name = "panelAbsoluteMove";
            this.panelAbsoluteMove.Size = new System.Drawing.Size(359, 89);
            this.panelAbsoluteMove.TabIndex = 4;
            // 
            // panelContiniusMove
            // 
            this.panelContiniusMove.Controls.Add(this.tbTimeout);
            this.panelContiniusMove.Controls.Add(this.chkUseTimeout);
            this.panelContiniusMove.Controls.Add(this.lblVx);
            this.panelContiniusMove.Controls.Add(this.nudVx);
            this.panelContiniusMove.Controls.Add(this.btnVxStartMove);
            this.panelContiniusMove.Controls.Add(this.btnVxStopMove);
            this.panelContiniusMove.Controls.Add(this.nudVy);
            this.panelContiniusMove.Controls.Add(this.btnStopAll);
            this.panelContiniusMove.Controls.Add(this.lblVy);
            this.panelContiniusMove.Controls.Add(this.btnStartAll);
            this.panelContiniusMove.Controls.Add(this.nudVzoom);
            this.panelContiniusMove.Controls.Add(this.btnStopZoom);
            this.panelContiniusMove.Controls.Add(this.lblVZoom);
            this.panelContiniusMove.Controls.Add(this.btnStartZoom);
            this.panelContiniusMove.Enabled = false;
            this.panelContiniusMove.Location = new System.Drawing.Point(8, 264);
            this.panelContiniusMove.Name = "panelContiniusMove";
            this.panelContiniusMove.Size = new System.Drawing.Size(344, 150);
            this.panelContiniusMove.TabIndex = 21;
            // 
            // tbTimeout
            // 
            this.tbTimeout.Location = new System.Drawing.Point(125, 120);
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(49, 20);
            this.tbTimeout.TabIndex = 32;
            this.tbTimeout.Text = "10";
            // 
            // chkUseTimeout
            // 
            this.chkUseTimeout.AutoSize = true;
            this.chkUseTimeout.Location = new System.Drawing.Point(11, 123);
            this.chkUseTimeout.Name = "chkUseTimeout";
            this.chkUseTimeout.Size = new System.Drawing.Size(108, 17);
            this.chkUseTimeout.TabIndex = 31;
            this.chkUseTimeout.Text = "Use Timeout (ms)";
            this.chkUseTimeout.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Profile:";
            // 
            // cmbPTZProfiles
            // 
            this.cmbPTZProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPTZProfiles.FormattingEnabled = true;
            this.cmbPTZProfiles.Location = new System.Drawing.Point(63, 104);
            this.cmbPTZProfiles.Name = "cmbPTZProfiles";
            this.cmbPTZProfiles.Size = new System.Drawing.Size(122, 21);
            this.cmbPTZProfiles.TabIndex = 50;
            this.cmbPTZProfiles.SelectedIndexChanged += new System.EventHandler(this.cmbPTZProfiles_SelectedIndexChanged);
            // 
            // btnGetProfiles
            // 
            this.btnGetProfiles.Location = new System.Drawing.Point(191, 102);
            this.btnGetProfiles.Name = "btnGetProfiles";
            this.btnGetProfiles.Size = new System.Drawing.Size(52, 23);
            this.btnGetProfiles.TabIndex = 51;
            this.btnGetProfiles.Text = "Get";
            this.btnGetProfiles.UseVisualStyleBackColor = true;
            this.btnGetProfiles.Click += new System.EventHandler(this.btnGetProfiles_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "PTZ URL:";
            // 
            // tbMediaUrl
            // 
            this.tbMediaUrl.Location = new System.Drawing.Point(100, 4);
            this.tbMediaUrl.Name = "tbMediaUrl";
            this.tbMediaUrl.ReadOnly = true;
            this.tbMediaUrl.Size = new System.Drawing.Size(264, 20);
            this.tbMediaUrl.TabIndex = 53;
            // 
            // btnAddPTZConfig
            // 
            this.btnAddPTZConfig.Location = new System.Drawing.Point(249, 102);
            this.btnAddPTZConfig.Name = "btnAddPTZConfig";
            this.btnAddPTZConfig.Size = new System.Drawing.Size(114, 23);
            this.btnAddPTZConfig.TabIndex = 54;
            this.btnAddPTZConfig.Text = "Add PTZ config";
            this.btnAddPTZConfig.UseVisualStyleBackColor = true;
            this.btnAddPTZConfig.Click += new System.EventHandler(this.btnAddPTZConfig_Click);
            // 
            // rbRelativeMove
            // 
            this.rbRelativeMove.AutoSize = true;
            this.rbRelativeMove.Location = new System.Drawing.Point(105, 131);
            this.rbRelativeMove.Name = "rbRelativeMove";
            this.rbRelativeMove.Size = new System.Drawing.Size(94, 17);
            this.rbRelativeMove.TabIndex = 4;
            this.rbRelativeMove.Text = "Relative Move";
            this.rbRelativeMove.UseVisualStyleBackColor = true;
            this.rbRelativeMove.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // PtzPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbRelativeMove);
            this.Controls.Add(this.btnAddPTZConfig);
            this.Controls.Add(this.tbMediaUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGetProfiles);
            this.Controls.Add(this.cmbPTZProfiles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelContiniusMove);
            this.Controls.Add(this.panelAbsoluteMove);
            this.Controls.Add(this.btnVideo);
            this.Controls.Add(this.rbAbsoluteMove);
            this.Controls.Add(this.rbContuniousMove);
            this.Controls.Add(this.btnGetPtzUrl);
            this.Controls.Add(this.tbPtzUrl);
            this.Controls.Add(this.lblPtzUrl);
            this.Controls.Add(this.tbReport);
            this.Name = "PtzPage";
            this.Size = new System.Drawing.Size(775, 496);
            ((System.ComponentModel.ISupportInitialize)(this.nudVzoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            this.panelAbsoluteMove.ResumeLayout(false);
            this.panelAbsoluteMove.PerformLayout();
            this.panelContiniusMove.ResumeLayout(false);
            this.panelContiniusMove.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVideo;
        private System.Windows.Forms.Button btnStopAll;
        private System.Windows.Forms.Button btnStartAll;
        private System.Windows.Forms.Button btnStopZoom;
        private System.Windows.Forms.Button btnStartZoom;
        private System.Windows.Forms.Label lblVZoom;
        private System.Windows.Forms.NumericUpDown nudVzoom;
        private System.Windows.Forms.Label lblVy;
        private System.Windows.Forms.NumericUpDown nudVy;
        private System.Windows.Forms.Button btnVxStopMove;
        private System.Windows.Forms.Button btnVxStartMove;
        private System.Windows.Forms.Label lblVx;
        private System.Windows.Forms.NumericUpDown nudVx;
        private System.Windows.Forms.RadioButton rbAbsoluteMove;
        private System.Windows.Forms.Button btnZoomFromMinToMax;
        private System.Windows.Forms.Button btnZoomMax;
        private System.Windows.Forms.Button btnZoomMin;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.NumericUpDown nudZoom;
        private System.Windows.Forms.Button btnYFromMinToMax;
        private System.Windows.Forms.Button btnYMax;
        private System.Windows.Forms.Button btnYMin;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.Button btnXFromMinToMax;
        private System.Windows.Forms.Button btnXMax;
        private System.Windows.Forms.Button btnXMin;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.RadioButton rbContuniousMove;
        private System.Windows.Forms.Button btnGetPtzUrl;
        private System.Windows.Forms.TextBox tbPtzUrl;
        private System.Windows.Forms.Label lblPtzUrl;
        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.Panel panelAbsoluteMove;
        private System.Windows.Forms.Panel panelContiniusMove;
        private System.Windows.Forms.TextBox tbTimeout;
        private System.Windows.Forms.CheckBox chkUseTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPTZProfiles;
        private System.Windows.Forms.Button btnGetProfiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMediaUrl;
        private System.Windows.Forms.Button btnAddPTZConfig;
        private System.Windows.Forms.RadioButton rbRelativeMove;
    }
}
