///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.DoorControl
{
  [Test(
    Name             = "Unlock Door",
    Category         = Category.ProfileC,
    Id               = "3",
    FeatureUnderTest = Feature.UnlockDoor
    )]
  public class UnlockDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var unlockDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("UnlockDoor"))
        .ToList();

      AffectedPairs.AddRange(unlockDoorList);

      BeginStep("Client request contains <UnlockDoor> tag", unlockDoorList);

      if (0 == unlockDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <UnlockDoor> messages");

      StepCompleted();

      var tokenList = unlockDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "UnlockDoor", "Token"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")))
        .ToList();

      BeginStep("<UnlockDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <UnlockDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = unlockDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("UnlockDoorResponse")).ToList();

      BeginStep("Device response contains <UnlockDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<UnlockDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}