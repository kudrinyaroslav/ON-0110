///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.SystemComponentState
{
  [Test(
    Name             = "State of Access Points",
    Category         = Category.ProfileC,
    Id               = "1",
    FeatureUnderTest = Feature.StateOfAccessPoints
    )]
  public class StateOfAccessPointsTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var pullPointList = conversation.GetMessages(ContentType.Http)
          .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "CreatePullPointSubscription"))
          .ToList();

      AffectedPairs.AddRange(pullPointList);

      BeginStep("Client request contains <CreatePullPointSubscription> tag", pullPointList);

      if (0 == pullPointList.Count)
        throw new TestNotSupportedException("Conversation does not contain <CreatePullPointSubscription> messages");

      StepCompleted();

      var filterList = pullPointList
          .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Filter"))
          .ToList();

      if (0 == filterList.Count)
      {
        BeginStep("<CreatePullPointSubscription> does not includes tag <Filter>", pullPointList);
        StepCompleted();
      }
      else
      {
        var topicExpressionList = filterList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Filter", "TopicExpression")).ToList();

        var validTopicExpressionList = topicExpressionList.Where(item =>
            {
              String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "TopicExpression");
              return null != value && (value.Contains(":AccessPoint/State") ||
                                       value.Contains(":AccessPoint//"));
            }).ToList();

        BeginStep("<CreatePullPointSubscription> includes tag <Filter> with tag <TopicExpression> and (\":AccessPoint/State\" OR \":AccessPoint//\") topic value", topicExpressionList);


        if (0 == topicExpressionList.Count)
          StepFailed("<TopicExpression> tag is missing");
        else if (0 == validTopicExpressionList.Count)
          throw new TestNotSupportedException("Valid value value not found");

        StepCompleted();
      }

      var responseList = pullPointList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("CreatePullPointSubscriptionResponse")).ToList();

      BeginStep("Device response contains <CreatePullPointSubscriptionResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<CreatePullPointSubscriptionResponse> tag is missing");

      StepCompleted();

      var pullMessagesList = conversation.GetMessages(ContentType.Http)
          .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages"))
          .ToList();

      BeginStep("Client request contains <PullMessages> tag", pullMessagesList);

      if (0 == pullMessagesList.Count)
        throw new TestNotSupportedException("Conversation does not contain <PullMessages> messages");

      StepCompleted();

      var timeoutList = pullMessagesList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "Timeout"))
        .ToList();

      BeginStep("<PullMessages> includes tag: <Timeout>", timeoutList);

      if (0 == timeoutList.Count)
        StepFailed("<Timeout> tag is missing");

      StepCompleted();

      var messageLimitList = timeoutList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "MessageLimit"))
          .ToList();

      BeginStep("<PullMessages> includes tag: <MessageLimit>", messageLimitList);

      if (0 == messageLimitList.Count)
        StepFailed("<MessageLimit> tag is missing");

      StepCompleted();

      responseList = messageLimitList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("PullMessagesResponse")).ToList();

      BeginStep("Device response contains <PullMessagesResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<PullMessagesResponse> tag is missing");

      StepCompleted();
    }
  }
}