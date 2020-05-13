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
    Name             = "Lock Door",
    Category         = Category.ProfileC,
    Id               = "2",
    FeatureUnderTest = Feature.LockDoor
    )]
  public class LockDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var lockDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("LockDoor"))
        .ToList();
      AffectedPairs.AddRange(lockDoorList);

      BeginStep("Client request contains <LockDoor> tag", lockDoorList);

      if (0 == lockDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <LockDoor> messages");

      StepCompleted();

      var tokenList = lockDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "LockDoor", "Token"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")))
        .ToList();

      BeginStep("<LockDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <LockDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = lockDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("LockDoorResponse")).ToList();

      BeginStep("Device response contains <LockDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<LockDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}