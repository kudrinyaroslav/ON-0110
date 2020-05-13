///
/// @Author Matthew Tuusberg
///

﻿using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Configuration.DataGridViewProgressBar
{
  internal class DataGridViewProgressColumn : DataGridViewImageColumn
  {
    public DataGridViewProgressColumn()
    {
      CellTemplate = new DataGridViewProgressCell();
    }

    public override sealed DataGridViewCell CellTemplate
    {
      get
      {
        return base.CellTemplate;
      }
      set
      {
        base.CellTemplate = value;
      }
    }
  }
}