///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Interfaces;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  public sealed partial class ConversationComboBox : ComboBox, IView
  {
    public ConversationComboBox()
    {
      InitializeComponent();
      Font = new Font(Font, FontStyle.Bold);
    }

    #region Properties

    public Conversation SelectedConversation
    {
      get;
      private set;
    }

    private TestLog mSelectedTestLog;

    public TestLog SelectedTestLog
    {
      get
      {
        return mSelectedTestLog;
      }

      set
      {
        mSelectedTestLog = value;

        RefreshItems();
        Refresh();
      }
    }

    #endregion

    public void Clear()
    {
      Items.Clear();

      SelectedConversation = null;
      mSelectedTestLog     = null;
    }

    protected override void RefreshItems()
    {
      Items.Clear();

      if (null == mSelectedTestLog)
        return;

      // ReSharper disable once CoVariantArrayConversion
      Items.AddRange(mSelectedTestLog.ConversationLogs.Select(item => item.Conversation.Name).ToArray());

      if (0 != Items.Count)
        SelectedIndex = 0;
      
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      if (-1 == SelectedIndex)
        SelectedConversation = null;

      SelectedConversation = mSelectedTestLog.ConversationLogs[SelectedIndex].Conversation;

      base.OnSelectedIndexChanged(e);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      if (-1 != e.Index)
      {
        if (null == SelectedTestLog)
          return;

        e.DrawBackground();

        String text = Items[e.Index].ToString();

        ConversationLog logForConversation = mSelectedTestLog.ConversationLogs.ToList()[e.Index];

        if (null == logForConversation)
          return;

        e.Graphics.DrawString(text, Font, new SolidBrush(logForConversation.TestStatus.GetColor()),
          e.Bounds);
      }

      base.OnDrawItem(e);
    }
  }
}
