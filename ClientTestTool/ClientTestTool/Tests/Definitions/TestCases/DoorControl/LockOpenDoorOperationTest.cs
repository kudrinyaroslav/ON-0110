///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
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
    Name             = "Lock Open Door",
    Category         = Category.ProfileC,
    Id               = "7",
    FeatureUnderTest = Feature.LockOpenDoor
    )]
  public class LockOpenDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var lockOpenDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("LockOpenDoor"))
        .ToList();
      AffectedPairs.AddRange(lockOpenDoorList);

      BeginStep("Client request contains <LockOpenDoor> tag", lockOpenDoorList);

      if (0 == lockOpenDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <LockOpenDoor> messages");

      StepCompleted();

      var tokenList = lockOpenDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "LockOpenDoor", "Token"))
        .ToList();

      var tokens = tokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")).ToList();

      BeginStep("<LockOpenDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (tokens.All(String.IsNullOrEmpty))
        StepFailed("Request's <LockOpenDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = lockOpenDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("LockOpenDoorResponse")).ToList();

      BeginStep("Device response contains <LockOpenDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<LockOpenDoorResponse> tag is missing");

      StepCompleted();

      lockOpenDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("LockOpenReleaseDoor"))
        .ToList();
      AffectedPairs.AddRange(lockOpenDoorList);

      BeginStep("Client request contains <LockOpenReleaseDoor> tag", lockOpenDoorList);

      if (0 == lockOpenDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <LockOpenReleaseDoor> messages");

      StepCompleted();

      tokenList = lockOpenDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "LockOpenReleaseDoor", "Token"))
        .Where(item =>
        {
          String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token");
          return !String.IsNullOrEmpty(value) && tokens.Contains(value);
        })
        .ToList();

      BeginStep("<LockOpenReleaseDoor>” includes tag: “<Token>” with token value from LockDownDoor operation", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <LockOpenReleaseDoor> does not include tag <Token> with valid value");

      StepCompleted();

      responseList = lockOpenDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("LockOpenReleaseDoorResponse")).ToList();

      BeginStep("Device response contains <LockOpenReleaseDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<LockOpenReleaseDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}