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
    Name             = "Lock Down Door",
    Category         = Category.ProfileC,
    Id               = "6",
    FeatureUnderTest = Feature.LockDownDoor
    )]
  public class LockDownDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var lockDownDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("LockDownDoor"))
        .ToList();
      AffectedPairs.AddRange(lockDownDoorList);

      BeginStep("Client request contains <LockDownDoor> tag", lockDownDoorList);

      if (0 == lockDownDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <LockDownDoor> messages");

      StepCompleted();

      var tokenList = lockDownDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "LockDownDoor", "Token"))
        .ToList();

      var tokens = tokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")).ToList();

      BeginStep("<LockDownDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (tokens.All(String.IsNullOrEmpty))
        StepFailed("Request's <LockDownDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = lockDownDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("LockDownDoorResponse")).ToList();

      BeginStep("Device response contains <LockDownDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<LockDownDoorResponse> tag is missing");

      StepCompleted();

      lockDownDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("LockDownReleaseDoor"))
        .ToList();
      AffectedPairs.AddRange(lockDownDoorList);

      BeginStep("Client request contains <LockDownReleaseDoor> tag", lockDownDoorList);

      if (0 == lockDownDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <LockDownReleaseDoor> messages");

      StepCompleted();

      tokenList = lockDownDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "LockDownReleaseDoor", "Token"))
        .Where(item =>
        {
          String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token");
          return !String.IsNullOrEmpty(value) && tokens.Contains(value);
        })
        .ToList();

      BeginStep("<LockDownReleaseDoor>” includes tag: “<Token>” with token value from LockDownDoor operation", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <LockDownReleaseDoor> does not include tag <Token> with valid value");

      StepCompleted();

      responseList = lockDownDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("LockDownReleaseDoorResponse")).ToList();

      BeginStep("Device response contains <LockDownReleaseDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<LockDownReleaseDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}