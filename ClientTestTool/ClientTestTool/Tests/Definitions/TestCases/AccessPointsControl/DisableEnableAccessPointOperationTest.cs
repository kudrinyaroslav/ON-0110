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

namespace ClientTestTool.Tests.Definitions.TestCases.AccessPointsControl
{
  [Test(
    Name             = "Disable Enable Access Point",
    Category         = Category.ProfileC,
    Id               = "1",
    FeatureUnderTest = Feature.DisableEnableAccessPoint
  )]
  public class DisableEnableAccessPointOperationTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var disableList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("DisableAccessPoint"))
        .ToList();

      AffectedPairs.AddRange(disableList);

      BeginStep("Client request contains <DisableAccessPoint> tag", disableList);

      if (0 == disableList.Count)
        throw new TestNotSupportedException("Conversation does not contain <DisableAccessPoint> messages");

      StepCompleted();

      var tokenList = disableList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "DisableAccessPoint", "Token"))
        .ToList();

      var disableListTokens = tokenList
        .Select(item => TestUtil.ValueOf(item.GetRequest<HttpRequest>(), "Token"))
        .ToList();

      BeginStep("<DisableAccessPoint> includes tag <Token> with non-empty string value", tokenList);

      if (disableListTokens.All(String.IsNullOrEmpty))
        StepFailed("<DisableAccessPoint> does not include tag <Token> with non-empty string value");

      StepCompleted();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", disableList, MessageType.Response);

      if (disableList.All(item => "200" != item.GetResponse<HttpResponse>().StatusCode))
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <DisableAccessPointResponse> tag", disableList, MessageType.Response);

      if (!disableList.Any(item => item.GetResponse<HttpMessage>().ContainsTag("DisableAccessPointResponse")))
        StepFailed("<DisableAccessPointResponse> tag is missing");

      StepCompleted();

      var enableList = conversation.GetMessages(ContentType.Http)
        .Where(item => item.GetRequest<HttpMessage>().ContainsTag("EnableAccessPoint"))
        .ToList();

      AffectedPairs.AddRange(enableList);

      BeginStep("Client request contains <EnableAccessPoint> tag", enableList);

      if (!enableList.Any())
        StepFailed("Conversation does not contain <EnableAccessPoint> messages");

      StepCompleted();


      var enableListTokens = enableList
        .Select(item => TestUtil.ValueOf(item.GetRequest<HttpRequest>(), "Token"))
        .ToList();

      BeginStep("<EnableAccessPoint> includes tag <Token> with token value from Disable Access Point operation", enableList);

      if (!enableListTokens.Intersect(disableListTokens).Any())
        StepFailed("<EnableAccessPoint> does not include tag <Token> with token value from Disable Access Point operation");

      StepCompleted();


      BeginStep("Device response contains \"HTTP/* 200 OK\"", enableList, MessageType.Response);

      if (enableList.All(item => "200" != item.GetResponse<HttpResponse>().StatusCode))
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <EnableAccessPointResponse> tag", enableList, MessageType.Response);

      if (!enableList.Any(item => item.GetResponse<HttpMessage>().ContainsTag("EnableAccessPointResponse")))
        StepFailed("<EnableAccessPointResponse> tag is missing");

      StepCompleted();
    }
  }
}