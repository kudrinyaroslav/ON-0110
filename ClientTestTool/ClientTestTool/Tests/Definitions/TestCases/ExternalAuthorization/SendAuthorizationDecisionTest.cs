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

namespace ClientTestTool.Tests.Definitions.TestCases.ExternalAuthorization
{
  [Test(
    Name             = "Send Authorization Decision",
    Category         = Category.ProfileC,
    Id               = "2",
    FeatureUnderTest = Feature.SendAuthDecision
  )]
  public class SendAuthorizationDecisionTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var externalAuthList = conversation.GetMessages(ContentType.Http)
                                    .Where(item => item.GetRequest<HttpMessage>().ContainsTag("ExternalAuthorization"))
                                    .ToList();

      AffectedPairs.AddRange(externalAuthList);

      BeginStep("Client request contains <ExternalAuthorization> tag", externalAuthList);

      if (0 == externalAuthList.Count)
        throw new TestNotSupportedException("Conversation does not contain <ExternalAuthorization> messages");

      StepCompleted();

      var tokenList = externalAuthList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ExternalAuthorization", "AccessPointToken"))
                                      .ToList();

      var tokenValuesList = tokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpRequest>(), "AccessPointToken"))
                                     .ToList();

      BeginStep("<ExternalAuthorization> includes tag <AccessPointToken> with non-empty string value", tokenList);

      if (tokenValuesList.All(String.IsNullOrEmpty))
        StepFailed("<DisableAccessPoint> does not include tag <Token> with non-empty string value");

      StepCompleted();

      BeginStep("<ExternalAuthorization> includes tag <AccessPointToken> with non-empty string value", tokenList);

      if (tokenValuesList.All(String.IsNullOrEmpty))
        StepFailed("<ExternalAuthorization> does not include tag <AccessPointToken> with non-empty string value");

      StepCompleted();

      var desicionList = tokenList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ExternalAuthorization", "Decision"))
                                  .ToList();

      BeginStep("<ExternalAuthorization> includes tag <Decision>", desicionList);

      if (0 == desicionList.Count)
        StepFailed("<ExternalAuthorization> does not include tag <Decision>");

      StepCompleted();

      var validDesicions = desicionList.Where(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Decision").Contains("Granted") ||
                                                      TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Decision").Contains("Denied"))
                                       .ToList();

      BeginStep("<Decision> contains value EITHER (Granted OR Denied)", validDesicions);

      if (0 == validDesicions.Count)
        StepFailed("");

      StepCompleted();

      var responseList = validDesicions.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "ExternalAuthorizationResponse"))
                                 .ToList();

      BeginStep("Device response contains <ExternalAuthorizationResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<DisableAccessPointResponse> tag is absent");

      StepCompleted();
    }
  }
}
