///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Utils
{
  public static class ControlHelper
  {
    public static IEnumerable<Control> AllControls(this Control control)
    {
      if (null == control)
        throw new ArgumentNullException();

      var controlsInThisLevel = control.Controls.Cast<Control>().ToList();
      return controlsInThisLevel.SelectMany(AllControls).Concat(controlsInThisLevel);
    }

    public static IEnumerable<Control> AllControls<T>(this Control control) where T : Control
    {
      if (null == control)
        throw new ArgumentNullException();

      var controlsInThisLevel = control.Controls.Cast<Control>().ToList();
      return controlsInThisLevel.SelectMany(AllControls<T>).Concat(controlsInThisLevel.OfType<T>());
    }

    public static IEnumerable<T> AllControlsOfType<T>(this Control control) where T : Control
    {
      if (null == control)
        throw new ArgumentNullException();

      return control.AllControls().OfType<T>();
    }

    public static void InvokeIfRequired(this Control control, Action action)
    {
      if (null == control)
        throw new ArgumentNullException();

      if (control.InvokeRequired)
        control.Invoke(action);
      else
        action();
    }

    public static void ResizeListViews(Control container)
    {
      var listViews = container.AllControlsOfType<ListView>();
      foreach (ListView listView in listViews)
        ResizeListView(listView);
    }

    public static void ResizeGridViews(Control container)
    {
      var gridViews = container.AllControlsOfType<DataGridView>();
      foreach (DataGridView gridView in gridViews)
        ResizeGridView(gridView);
    }

    private static void ResizeListView(ListView listView)
    {
      if (mIsListViewResizing)
        return;

      if (null == listView)
        return;

      mIsListViewResizing = true;

      float totalColumnWidth = 0;
      int columnsCount = listView.Columns.Count;

      for (int i = 0; i < columnsCount; i++)
        totalColumnWidth += Convert.ToInt32(listView.Columns[i].Tag);

      for (int i = 0; i < columnsCount; i++)
      {
        float colPercentage = (Convert.ToInt32(listView.Columns[i].Tag) / totalColumnWidth);
        listView.Columns[i].Width = (int)(colPercentage * listView.ClientRectangle.Width);
      }

      if (0 != columnsCount)
        listView.Columns[columnsCount - 1].Width = -2;

      mIsListViewResizing = false;
    }

    private static void ResizeGridView(DataGridView gridView)
    {
      if (mIsListViewResizing)
        return;

      if (null == gridView)
        return;

      mIsListViewResizing = true;

      float totalColumnWidth = 0;
      int columnsCount = gridView.Columns.Count;

      for (int i = 0; i < columnsCount; i++)
        totalColumnWidth += Convert.ToInt32(gridView.Columns[i].Tag);

      for (int i = 0; i < columnsCount; i++)
      {
        float colPercentage = (Convert.ToInt32(gridView.Columns[i].Tag) / totalColumnWidth);
        gridView.Columns[i].Width = (int)(colPercentage * gridView.ClientRectangle.Width);
      }

      mIsListViewResizing = false;
    }

    private static bool mIsListViewResizing;
  }
}
