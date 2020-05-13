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
    Name             = "Block Door",
    Category         = Category.ProfileC,
    Id               = "5",
    FeatureUnderTest = Feature.BlockDoor
    )]
  public class BlockDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var blockDoorList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("BlockDoor"))
        .ToList();
      AffectedPairs.AddRange(blockDoorList);

      BeginStep("Client request contains <BlockDoor> tag", blockDoorList);

      if (0 == blockDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <BlockDoor> messages");

      StepCompleted();

      var tokenList = blockDoorList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "BlockDoor", "Token"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")))
        .ToList();

      BeginStep("<BlockDoor> includes tag: <Token> with non-empty string value of specific token, tokenList", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <BlockDoor> does not include tag <Token> with valid value");

      StepCompleted();

      var responseList = blockDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("BlockDoorResponse")).ToList();

      BeginStep("Device response contains <BlockDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<BlockDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}