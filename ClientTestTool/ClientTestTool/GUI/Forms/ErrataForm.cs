///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Conformance;
using ClientTestTool.Data.Conformance.DoC;
using ClientTestTool.Data.Global.Conformance;
using ClientTestTool.Data.Global.Settings;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Forms
{
  public partial class ErrataForm : Form
  {
    private readonly ConformanceInfo mConformanceInfo;

    public ErrataForm(ConformanceInfo info)
    {
      InitializeComponent();
      mConformanceInfo = info;
    }

    protected bool mIsListViewResizing = false;

    private void ErrataForm_Load(object sender, EventArgs e)
    {
      RefreshLog();

      dGVErrata.Columns[0].Tag = 15;
      dGVErrata.Columns[1].Tag = 6;
      dGVErrata.Columns[2].Tag = 12;

      ControlHelper.ResizeGridViews(this);
    }

    private void ErrataForm_SizeChanged(object sender, EventArgs e)
    {
      ControlHelper.ResizeGridViews(this);
    }

    private void RefreshLog()
    {
      this.InvokeIfRequired(() =>
      {
        dGVErrata.Rows.Clear();

        foreach (var message in ConformanceLog.Instance.Errors.SelectMany(item => item.Value))
          dGVErrata.Rows.Add(message);
      });
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      var emptyCellsList =
        dGVErrata.Rows.Cast<DataGridViewRow>()
          .Where(row => String.IsNullOrEmpty(row.Cells[1].EditedFormattedValue.ToString()))
          .Select(row => row.Cells[1])
          .ToList();
      if (0 != emptyCellsList.Count)
      {
        emptyCellsList.ForEach(item => item.ErrorText = "this field is required");
        return;
      }

      sFDConformance.FileName = mConformanceInfo.GetDoCFilename(errata: true);
      DialogResult dResult    = sFDConformance.ShowDialog();

      if (DialogResult.OK != dResult)
        return;

      String filename = sFDConformance.FileName;

      var generator = new DoCWithErrataGenerator(GetErratumTable(), mConformanceInfo, CTTSettings.GetDoCErrataTemplateFilename());
      generator.Generate(filename);

      //PdfViewer.View(docFilename);

      Close();
    }

    private List<Tuple<String, String, String>> GetErratumTable()
    {
      var result = new List<Tuple<String, String, String>>();

      for (int i = 0; i < dGVErrata.RowCount; ++i)
      {
        object error       = dGVErrata.Rows[i].Cells[0].Value ?? String.Empty;
        object number      = dGVErrata.Rows[i].Cells[1].Value ?? String.Empty;
        object description = dGVErrata.Rows[i].Cells[2].Value ?? String.Empty;

        result.Add(Tuple.Create(error.ToString(), number.ToString(), description.ToString()));
      }

      return result;
    }

    private void dGVErrata_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.ColumnIndex != 1)
        return;

      dGVErrata.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = String.Empty;

      // Don't try to validate the 'new row' until finished  
      // editing since there 
      // is not any point in validating its initial value. 
      if (dGVErrata.Rows[e.RowIndex].IsNewRow)
        return;

      int newInteger;
      if (!int.TryParse(e.FormattedValue.ToString(),
          out newInteger) || newInteger < 0)
      {
        dGVErrata.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "the value must be a non-negative integer";
        e.Cancel = true;
      }
    }

  }
}
