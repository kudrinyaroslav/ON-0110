///
/// @Author Matthew Tuusberg
///

﻿using System.Windows.Forms;

namespace ClientTestTool.GUI.Extensions
{
  internal static class CheckedListBoxExtension
  {
    public static void UncheckAllItems(this CheckedListBox listBox)
    {
      while (listBox.CheckedIndices.Count > 0)
        listBox.SetItemChecked(listBox.CheckedIndices[0], false);
    }
  }
}
