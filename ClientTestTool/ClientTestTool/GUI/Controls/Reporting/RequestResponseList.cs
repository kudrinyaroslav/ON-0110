///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Globalization;
using System.Windows.Forms;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Events;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Extensions;

namespace ClientTestTool.GUI.Controls.Reporting
{
  public partial class RequestResponseList : BasePage
  {
    /// <summary>
    /// ctor
    /// </summary>
    public RequestResponseList()
    {
      InitializeComponent();
    }

    #region Events
    #endregion

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

        var pair = mSelectedConversation.GetMessages()[e.ElementIndex];

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

    public void Refresh()
    {
      Invoke(new Action(() =>
      {
        lVRequestResponse.Items.Clear();

        if (null == mSelectedConversation)
          return;

        var pairs = mSelectedConversation.GetMessages();

        foreach (var pair in pairs)
          AddPair(pair);
      }));
    }

    private void AddPair(RequestResponsePair pair)
    {
      if (InvokeRequired)
        Invoke(new Action(() => AddPair(pair)));
      else
      {
        var item = new ListViewItem((lVRequestResponse.Items.Count + 1).ToString(CultureInfo.InvariantCulture));

        String status;
        String details;
        if (!pair.Request.Validated)
        {
          status = "PENDING";
          details = String.Empty;
        }
        else
        {
          status = pair.GetStatusString();
          details = pair.Request.GetDetails();
        }

        item.SubItems.Add(status);
        item.SubItems.Add(details);

        lVRequestResponse.Items.Add(item);
      }
    }

    public void Clear()
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

      var request  = mSelectedConversation.GetRequest(pairIndex);
      var response = mSelectedConversation.GetResponse(pairIndex);

      tBRequest.Text  = request.GetContent();
      tBResponse.Text = response.GetContent();

      tBDetails.Clear();

      FillValidationDetails(request as HttpMessage, response as HttpMessage);
    }

    private void FillValidationDetails(HttpMessage request, HttpMessage response)
    {
      if (null == request || !request.Validated)
        return;

      tBDetails.AppendLine("First-level validation:");
      tBDetails.AppendLine(String.Format("\tWSDL validation of SOAP request:{0}", request.GetStatusString()));

      if (!String.IsNullOrEmpty(request.ValidationError))
        tBDetails.AppendLine(String.Format("\tError:{0}", request.ValidationError));
      tBDetails.AppendLine();

      if (null == response || !response.Validated)
        return;

      tBDetails.AppendLine(String.Format("Second-level validation:"));
      tBDetails.AppendLine(String.Format("\tResponse code of unit:{0}", response.GetStatusString()));
    }

    #endregion

    private Conversation mSelectedConversation;
  }
}
