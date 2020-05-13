namespace ClientTestTool.GUI.Forms
{
  partial class ErrataForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrataForm));
      this.dGVErrata = new System.Windows.Forms.DataGridView();
      this.ErrorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOk = new System.Windows.Forms.Button();
      this.sFDConformance = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.dGVErrata)).BeginInit();
      this.SuspendLayout();
      // 
      // dGVErrata
      // 
      this.dGVErrata.AllowUserToAddRows = false;
      this.dGVErrata.AllowUserToDeleteRows = false;
      this.dGVErrata.AllowUserToResizeRows = false;
      this.dGVErrata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dGVErrata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dGVErrata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ErrorColumn,
            this.NumberColumn,
            this.DescriptionColumn});
      this.dGVErrata.Location = new System.Drawing.Point(0, 0);
      this.dGVErrata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.dGVErrata.Name = "dGVErrata";
      this.dGVErrata.RowHeadersVisible = false;
      this.dGVErrata.RowTemplate.Height = 24;
      this.dGVErrata.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dGVErrata.Size = new System.Drawing.Size(1045, 649);
      this.dGVErrata.TabIndex = 0;
      this.dGVErrata.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dGVErrata_CellValidating);
      // 
      // ErrorColumn
      // 
      this.ErrorColumn.HeaderText = "Error";
      this.ErrorColumn.Name = "ErrorColumn";
      this.ErrorColumn.ReadOnly = true;
      // 
      // NumberColumn
      // 
      this.NumberColumn.HeaderText = "Number";
      this.NumberColumn.Name = "NumberColumn";
      // 
      // DescriptionColumn
      // 
      this.DescriptionColumn.HeaderText = "Description";
      this.DescriptionColumn.Name = "DescriptionColumn";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(959, 658);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOk
      // 
      this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOk.Location = new System.Drawing.Point(877, 658);
      this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // sFDConformance
      // 
      this.sFDConformance.DefaultExt = "pdf";
      this.sFDConformance.Filter = "All files|*.*";
      // 
      // ErrataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
      this.ClientSize = new System.Drawing.Size(1045, 690);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.dGVErrata);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "ErrataForm";
      this.ShowInTaskbar = false;
      this.Text = "Errata Description";
      this.Load += new System.EventHandler(this.ErrataForm_Load);
      this.SizeChanged += new System.EventHandler(this.ErrataForm_SizeChanged);
      ((System.ComponentModel.ISupportInitialize)(this.dGVErrata)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dGVErrata;
    private System.Windows.Forms.DataGridViewTextBoxColumn ErrorColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn NumberColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.SaveFileDialog sFDConformance;


  }
}