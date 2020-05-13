///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Globalization;
using System.Windows.Forms;
using ClientTestTool.Data.Conversations.Events;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Controls.Base;
﻿using ClientTestTool.GUI.Extensions;
﻿using ClientTestTool.GUI.Utils;

namespace ClientTestTool.GUI.Controls.Reporting
{
  public partial class RequestResponseListView : BaseView
  {
    /// <summary>
    /// ctor
    /// </summary>
    public RequestResponseListView()
    {
      InitializeComponent();
    }

    #region Properties

    /// <summary>
    /// ListView items
    /// </summary>
    public ListView.ListViewItemCollection Items
    {
      get
      {
        return lVRequestResponse.Items;
      }
    }

    /// <summary>
    /// Conversation to show
    /// </summary>
    public Conversation SelectedConversation
    {
      get
      {
        return mSelectedConversation;
      }
      set
      {
        mSelectedConversation = value;
        OnConversationSelected();
      }
    }

    #endregion

    #region EventHandlers

    public void OnPairValidated(ElementEventArgs e)
    {
      lVRequestResponse.BeginInvoke(new Action(() =>
      {
        if (null == mSelectedConversation)
          return;

        var pair = mSelectedConversation[e.ElementIndex];

        lVRequestResponse.Items[e.ElementIndex].SubItems[1].Text = pair.GetStatusString();
        lVRequestResponse.Items[e.ElementIndex].SubItems[2].Text = pair.Request.GetDetails();

        if (0 == lVRequestResponse.SelectedIndices.Count)
          return;

        if (e.ElementIndex == lVRequestResponse.SelectedIndices[0])
          FillPairDetails();
      }));
    }

    private void OnConversationSelected()
    {
      if (null == mSelectedConversation)
        return;

      Refresh();
    }

    private void lVRequestResponse_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (0 == lVRequestResponse.SelectedIndices.Count)
        return;

      if (ConversationList.IsEmpty)
        return;

      if (!mSelectedConversation.Validated)
        return;

      FillPairDetails();
    }

    #endregion

    #region Helpers

    protected override void HookUI()
    {
    }

    public override void Refresh()
    {
      this.InvokeIfRequired((() =>
      {
        lVRequestResponse.BeginUpdate();

        lVRequestResponse.Items.Clear();

        if (null == mSelectedConversation)
          return;

        var pairs = mSelectedConversation.GetMessages();
        
        foreach (var pair in pairs)
          AddPair(pair);

        lVRequestResponse.EndUpdate();
      }));
    }

    private void AddPair(RequestResponsePair pair)
    {
      this.InvokeIfRequired(() =>
      {
        var item = new ListViewItem((lVRequestResponse.Items.Count + 1).ToString(CultureInfo.InvariantCulture));

        var status  = pair.GetStatusString();
        var details = pair.Request.GetDetails();
        
        item.SubItems.Add(status);
        item.SubItems.Add(details);

        lVRequestResponse.Items.Add(item);
      });
    }

    public override void Clear()
    {
      mSelectedConversation = null;
      lVRequestResponse.Items.Clear();
      tBDetails.Clear();
      tBRequest.Clear();
      tBResponse.Clear();
    }

    private void FillPairDetails()
    {
      int pairIndex = lVRequestResponse.SelectedIndices[0];

      var pair = mSelectedConversation[pairIndex];

      var request  = pair.Request;
      var response = pair.Response;

      tBRequest.Text  = request.GetContent();
      tBResponse.Text = response.GetContent();

      tBDetails.Clear();
      tBDetails.Text = pair.GetValidationDetails();
    }


    #endregion

    private Conversation mSelectedConversation;
  }
}
