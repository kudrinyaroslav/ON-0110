///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Enums;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.EventHandling
{
  [Test(
    Name              = "Pullpoint",
    Category          = Category.Core,
    Id                = "1",
    FeatureUnderTest  = Feature.PullPoint
  )]
  public class PullPointTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> pullpointList = conversation.GetMessages(ContentType.Http);
      pullpointList = pullpointList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("CreatePullPointSubscription")).ToList();
      
      AffectedPairs.AddRange(pullpointList);

      BeginStep("Client request contains <CreatePullPointSubscription> tag", pullpointList);

      if (!pullpointList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <CreatePullPointSubscription> tag");

      StepCompleted();

      var responses = pullpointList.Where(pair => "200" == pair.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responses, MessageType.Response);

      if (!responses.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      pullpointList.RemoveAll(
          item => !item.GetResponse<HttpMessage>().ContainsTag("CreatePullPointSubscriptionResponse"));

      BeginStep("Device response contains <CreatePullPointSubscriptionResponse> tag", pullpointList, MessageType.Response);
      
      if (!pullpointList.Any())
        StepFailed("CreatePullPointSubscriptionResponse tag missing");

      StepCompleted();

      var pullMessagesList = conversation.GetMessages(ContentType.Http)
                                         .Where(item => item.GetRequest<HttpMessage>().ContainsTag("PullMessages"))
                                         .ToList();

      AffectedPairs.AddRange(pullMessagesList);

      BeginStep("Client request contains <PullMessages> tag", pullMessagesList);

      if (0 == pullMessagesList.Count)
        StepFailed("Request with <PullMessages> tag is not present");

      StepCompleted();

      BeginStep("<PullMessages> includes tag: <Timeout>", pullMessagesList);

      if (!pullMessagesList.Any(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "Timeout")))
        StepFailed("<Timeout> tag is not present");

      StepCompleted();

      BeginStep("<PullMessages> includes tag: <MessageLimit>", pullMessagesList);

      if (!pullMessagesList.Any(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PullMessages", "MessageLimit")))
        StepFailed("<MessageLimit> tag is not present");

      StepCompleted();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", pullMessagesList, MessageType.Response);

      if (pullMessagesList.All(item => "200" != item.GetResponse<HttpResponse>().StatusCode))
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <PullMessagesResponse> tag", pullMessagesList, MessageType.Response);

      if (pullMessagesList.All(item => !item.GetResponse<HttpResponse>().ContainsTag("PullMessagesResponse")))
        StepFailed("PullMessagesResponse tag missing");

      StepCompleted();
    }
  }
}