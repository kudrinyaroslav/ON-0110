///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Controls.Configuration.DataGridViewProgressBar
{
  internal class DataGridViewProgressCell : DataGridViewImageCell
  {
    // Used to make custom cell consistent with a DataGridViewImageCell
    private static readonly Image mEmptyImage;

    static DataGridViewProgressCell()
    {
      mEmptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
    }

    public DataGridViewProgressCell()
    {
      ValueType = typeof(int);
    }
    // Method required to make the Progress Cell consistent with the default Image Cell. 
    // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
    protected override object GetFormattedValue(object value,
                                                int rowIndex,
                                                ref DataGridViewCellStyle cellStyle,
                                                TypeConverter valueTypeConverter,
                                                TypeConverter formattedValueTypeConverter,
                                                DataGridViewDataErrorContexts context)
    {
      return mEmptyImage;
    }

    protected override void Paint(Graphics g,
                                  Rectangle clipBounds,
                                  Rectangle cellBounds,
                                  int rowIndex,
                                  DataGridViewElementStates cellState,
                                  object value,
                                  object formattedValue,
                                  String errorText,
                                  DataGridViewCellStyle cellStyle,
                                  DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                  DataGridViewPaintParts paintParts)
    {
      int   progressVal = (int)value;
      float percentage  = ((float)progressVal / 100.0f);
      Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
      Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
      // Draws the cell grid

      base.Paint(g, clipBounds, cellBounds,
       rowIndex, cellState, value, formattedValue, errorText,
       cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));

      if (percentage > 0.0)
      {
        g.FillRectangle(new SolidBrush(Color.GreenYellow), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
        g.DrawString(progressVal + "%", cellStyle.Font, foreColorBrush, cellBounds.X, cellBounds.Y + 3);
      }
      else
        g.DrawString(progressVal + "%", cellStyle.Font, foreColorBrush, cellBounds.X, cellBounds.Y + 3);
    }
  }
}
