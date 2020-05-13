///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using ClientTestTool.Data.Conversations.Events;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Controls.Reporting
{
  /// <summary>
  /// Reporting page
  /// </summary>
  public partial class PageReporting : BaseView
  {
    private int mSelectedConversationIndex = -1;

    /// <summary>
    /// ctor
    /// </summary>
    public PageReporting()
    {
      InitializeComponent();
    }

    #region EventHandlers

    private void ReportingPage_Load(object sender, EventArgs e)
    {
      ConversationList.Instance.OnConversationListChanged += Conversations_ListChanged;
      ConversationList.Instance.OnPairAdded               += Conversations_PairAdded;
      ConversationList.Instance.OnPairValidated           += Conversations_PairValidated;
      ConversationList.Instance.OnConversationValidated   += Conversations_ConversationValidated;

      FillListView();
    }

    private void Conversations_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (ConversationList.IsEmpty)
        ClearSelection();

      FillListView();
    }

    private void Conversations_PairAdded(object sender, ConversationElementEventArgs e)
    {
      this.InvokeIfRequired((() =>
      {
        if (0 == lVConversations.SelectedIndices.Count)
          return;

        if (null == requestResponseList.SelectedConversation)
          return;

        var affectedConversation = ConversationList.GetConversations()[e.ConversationIndex];

        if (requestResponseList.SelectedConversation == affectedConversation)
          requestResponseList.Refresh();
      }));
    }

    private void Conversations_PairValidated(object sender, ConversationElementEventArgs e)
    {
      if (mSelectedConversationIndex != e.ConversationIndex)
        return;

      requestResponseList.OnPairValidated(e);
    }

    private void Conversations_ConversationValidated(object sender, ElementEventArgs e)
    {
      FillListView();
    }

    private void pageReporting_VisibleChanged(object sender, EventArgs e)
    {
      ClearSelection();
      FillListView();
    }


    private void lVConversations_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (0 == lVConversations.SelectedIndices.Count)
      {
        ClearSelection();
        return;
      }

      mSelectedConversationIndex = lVConversations.SelectedIndices[0];
      requestResponseList.SelectedConversation = ConversationList.GetConversations()[mSelectedConversationIndex];
    }

    private void btnSaveReportingLog_Click(object sender, EventArgs e)
    {
      if (ConversationList.IsEmpty)
        return;

      sFDReportingReport.FileName = String.Format("ReportingReport_{0}.xml", DateTime.Now.ToShortTimeString().Replace(':', '_'));
      DialogResult dResult = sFDReportingReport.ShowDialog();

      if (DialogResult.OK != dResult)
        return;

      String filename = sFDReportingReport.FileName;

      ConversationList.SaveConversationReport(filename);
    }

    private void sCMainContainer_SplitterMoved(object sender, SplitterEventArgs e)
    {
      HookUI();
    }

    #endregion
    
    #region Helpers

    protected override void HookUI()
    {
      ResizeListViews();
      FillListView();
    }

    private void ClearSelection()
    {
      mSelectedConversationIndex = -1;
      requestResponseList.Clear();
    }

    private void FillListView()
    {
      this.InvokeIfRequired(() =>
      {
        lVConversations.Items.Clear();
        requestResponseList.Clear();

        foreach (var conversation in ConversationList.GetConversations())
        {
          var item = new ListViewItem((lVConversations.Items.Count + 1).ToString(CultureInfo.InvariantCulture));

          item.SubItems.Add(conversation.Name);
          item.SubItems.Add(conversation.GetStatusString());

          lVConversations.Items.Add(item);
        }
      });
    }

    #endregion

    private enum Filter
    {
      All,
      Issues,
    }
  }
}
