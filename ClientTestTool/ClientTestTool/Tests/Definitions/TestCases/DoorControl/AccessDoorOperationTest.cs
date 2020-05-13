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
    Name             = "Access Door",
    Category         = Category.ProfileC,
    Id               = "1",
    FeatureUnderTest = Feature.AccessDoor
    )]
  public class AccessDoorOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var accessDoorList = conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("AccessDoor"))
          .ToList();

      AffectedPairs.AddRange(accessDoorList);

      BeginStep("Client request contains <AccessDoor> tag", accessDoorList);

      if (0 == accessDoorList.Count)
        throw new TestNotSupportedException("Conversation does not contain <AccessDoor> messages");

      StepCompleted();

      var tokenList = accessDoorList
          .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AccessDoor", "Token"))
          .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token")))
          .ToList();

      BeginStep("<AccessDoor> includes tag: <Token> with non-empty string value of specific token", tokenList);

      if (0 == tokenList.Count)
        StepFailed("Request's <AccessDoor> does not include tag <Token>");

      StepCompleted();

      var responseList = accessDoorList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains HTTP 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("AccessDoorResponse")).ToList();

      BeginStep("Device response contains <AccessDoorResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<AccessDoorResponse> tag is missing");

      StepCompleted();
    }
  }
}
