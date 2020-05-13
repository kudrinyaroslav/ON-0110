///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
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
    Name             = "Double Lock Door",
    Category         = Category.ProfileC,
    Id               = "4",
    FeatureUnderTest = Feature.DoubleLockDoor
    )]
  public class DoubleLockDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var doubleLockDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("DoubleLockDoor"))
        .ToList();
      AffectedPairs.AddRange(doubleLockDoorList);

      BeginStep("Client request contains <DoubleLockDoor> tag", doubleLockDoorList);

      if (0 == doubleLockDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <DoubleLockDoor> messages");

      StepCompleted();

      var tokenList = doubleLockDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "DoubleLockDoor", "Token"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")))
        .ToList();

      BeginStep("<DoubleLockDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <DoubleLockDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = doubleLockDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("DoubleLockDoorResponse")).ToList();

      BeginStep("Device response contains <DoubleLockDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<DoubleLockDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}