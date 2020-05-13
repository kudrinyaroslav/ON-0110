///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Global;
using ClientTestTool.Data.Utils;
using ClientTestTool.GUI.Controls.Base;
using ClientTestTool.GUI.Extensions;
using ClientTestTool.GUI.Utils;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Log.Steps;
using ClientTestTool.Tests.Definitions.Log.Test;

namespace ClientTestTool.GUI.Controls.Diagnostics
{
  public partial class TestDetailsView : BaseView
  {
    public TestDetailsView()
    {
      InitializeComponent();
      
      mPairs = new List<RequestResponsePair>();
      mCurrentPairIndex = 0;
    }

    #region Properties

    private BaseTest mSelectedTest;

    public BaseTest SelectedTest
    {
      get
      {
        return mSelectedTest;
      }

      set
      {
        mSelectedTest = value;

        Clear();

        if (null != mSelectedTest)
          cBConversations.SelectedTestLog = mSelectedTest.Log;
      }
    }

    #endregion

    #region Event Handlers

    private void lVSteps_SelectedIndexChanged(object sender, EventArgs e)
    {
      var lVSender = sender as ListView;

      if (null == lVSender)
        throw new ArgumentException("", "sender");

      if (null == SelectedTest)
        return;

      if (0 == lVSender.SelectedIndices.Count)
      {
        mSelectedStep = null;
        return;
      }

      ConversationLog currentLog =
        SelectedTest.Log.ConversationLogs.FirstOrDefault(
          item => item.Conversation == cBConversations.SelectedConversation);

      int selectedIndex = lVSender.SelectedIndices[0];

      if (selectedIndex > currentLog.Steps.Count)
        throw new IndexOutOfRangeException();

      mSelectedStep = currentLog.Steps[selectedIndex];

      ClearRequestResponse();
      ShowPairs(currentLog.Conversation, mSelectedStep);
    }

    private void OnRequestResponseScrolled(object sender, EventArgs e)
    {
      if (mIsPairsLoading)
        return;

      mIsPairsLoading = true;
      mCurrentPairIndex += NUM_FETCH;
      if (mCurrentPairIndex > mPairs.Count - 1)
        mCurrentPairIndex = mPairs.Count;

      ShowNextPairs();
      mIsPairsLoading = false;
    }

    private void cBConversations_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (null == SelectedTest)
        return;

      ClearSteps();
      ClearPairs();
      ClearRequestResponse();

      LoadDetails();
    }

    #endregion

    #region Helpers

    protected override void HookUI()
    {
    }

    public override void Clear()
    {
      cBConversations.Clear();
      ClearSteps();
      ClearPairs();
      ClearRequestResponse();
    }

    private void ClearSteps()
    {
      lVSteps.Items.Clear();
      mSelectedStep = null;
    }

    private void ClearPairs()
    {
      mPairs.Clear();
      mCurrentPairIndex = 0;
    }

    private void ClearRequestResponse()
    {
      tBRequest.Clear();
      tBResponse.Clear();
    }

    public void LoadDetails()
    {
      int selectedIndex = cBConversations.SelectedIndex;

      if (-1 == selectedIndex)
        return;

      if (null == SelectedTest)
        return;

      ConversationLog currentLog = SelectedTest.Log.ConversationLogs[selectedIndex];

      ShowLog(currentLog);
    }

    private void ShowLog(ConversationLog currentLog)
    {
      lVSteps.BeginUpdate();

      lVSteps.Items.Clear();
      currentLog.Steps.ForEach(step =>
      {
        var item = new ListViewItem(step.Number.ToString(CultureInfo.InvariantCulture));
        item.SubItems.Add(step.Message);
        item.SubItems.Add(step.Status.GetDescription().ToUpper());
        item.SubItems.Add(step.ErrorMessage);
        lVSteps.Items.Add(item);
      });

      lVSteps.EndUpdate();

      ShowPairs(currentLog.Conversation, null);
    }

    private void ShowPairs(Conversation selectedConversation, StepResult selectedStep)
    {
      if (null != selectedStep)
        mPairs.AddRange(selectedStep.AffectedPairs);
      else
      {
        mPairs.AddRange(SelectedTest.AffectedPairs);
        mPairs = mPairs.Where(item => item.Conversation == selectedConversation)
                       .OrderBy(item => item.Conversation.IndexOf(item))
                       .ToList();
      }

      if (0 == mPairs.Count)
      {
        this.InvokeIfRequired((() =>
        {
          tBRequest.AppendLine("PAIRS NOT FOUND");
          tBResponse.AppendLine("PAIRS NOT FOUND");
        }));

        return;
      }

      ShowNextPairs();

      HighlightTextIfNeeded();
    }

    private void ShowNextPairs()
    {
      int delta = mPairs.Count - mCurrentPairIndex;

      if (0 == delta)
        return;

      int count = delta > NUM_FETCH ? NUM_FETCH : delta;

      ShowPairs(mPairs.GetRange(mCurrentPairIndex, count));

      HighlightTextIfNeeded();

      mCurrentPairIndex += NUM_FETCH;
      if (mCurrentPairIndex > mPairs.Count - 1)
        mCurrentPairIndex = mPairs.Count;
    }

    private void ShowPairs(List<RequestResponsePair> pairs)
    {
      int i = 0;

      foreach (var pair in LoadPairs(pairs))
      {
        var pairInfo = GetPairInfo(pairs[i]);

        AppendPairContent(tBRequest, pairInfo, pair.Item1);
        AppendPairContent(tBResponse, pairInfo, pair.Item2);

        ++i;
      }
    }

    private void AppendPairContent(RichTextBox textBox, String pairInfo, String content)
    {
      content = ProcessPairContent(content);
      Invoke(new Action(() =>
      {
        textBox.AppendLine(pairInfo, Color.Blue, FontStyle.Bold);
        textBox.AppendText(content);
      }));
    }

    private String ProcessPairContent(String content)
    {
      var sb = new StringBuilder();
      sb.AppendLine();
      sb.AppendLine(content);
      sb.AppendLine();
      sb.AppendLine();

      return sb.ToString();
    }

    private IEnumerable<Tuple<String, String>> LoadPairs(IEnumerable<RequestResponsePair> pairs)
    {
      return pairs.Select(pair => Tuple.Create(pair.Request.GetContent(), pair.Response.GetContent()));
    }

    private void HighlightTextIfNeeded()
    {
      if (null == mSelectedStep)
        return;

      if (!mSelectedStep.AffectedPairs.Any())
        return;

      var textBox = MessageType.Request == mSelectedStep.Type ? tBRequest : tBResponse;
      textBox.HighlightText(0, textBox.TextLength);
    }

    private String GetPairInfo(RequestResponsePair pair)
    {
      var conversation = pair.Conversation;
      int indexOfConversation = ConversationList.GetConversations().IndexOf(conversation) + 1;
      int indexOfPair = conversation.IndexOf(pair) + 1;

      return String.Format("Conversation:{0} - {1} \\ Pair:{2}", indexOfConversation, conversation.Name, indexOfPair);
    }

    #endregion

    private bool mIsPairsLoading = false;
    private StepResult mSelectedStep = null;
    private List<RequestResponsePair> mPairs; 
    private int mCurrentPairIndex;

    private const int NUM_FETCH = 5;
  }
}
