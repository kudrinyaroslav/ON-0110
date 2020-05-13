///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.Security
{
  [Test(
    Name              = "HTTP Digest",
    FeatureUnderTest  = Feature.HTTPDigest,
    Id                = "2"
    )]
  public class DigestAuthenticationTest : BaseTest //TODO
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var unauthorizedList = conversation.GetMessages(ContentType.Http).Where(item => "401" == item.GetResponse<HttpResponse>().StatusCode).ToList();
      var authorizedList   = conversation.GetMessages(ContentType.Http).Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      //var mergedList = unauthorizedList.

      AffectedPairs.AddRange(unauthorizedList);

      BeginStep("Client request contains (HTTP GET method OR HTTP POST method) without any authentication", unauthorizedList);

      StepCompleted();

      BeginStep("Device response contains HTTP/* 401 Unauthorized", unauthorizedList, MessageType.Response);

      if (!unauthorizedList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with digest authentication");

      StepCompleted();

      BeginStep("Device response contains “realm=*” element", unauthorizedList, MessageType.Response);

      StepCompleted();

      BeginStep("Device response contains “nonce=*” element", unauthorizedList, MessageType.Response);

      StepCompleted();

      BeginStep("Client request contains (HTTP GET method OR HTTP POST method) with “Authorization: Digest username=*” element", authorizedList);

      StepCompleted();

      BeginStep("Client request contains “realm=*” element with value from Device response", authorizedList);

      StepCompleted();

      BeginStep("Client request contains “nonce=*” element with value from Device response", authorizedList);

      StepCompleted();

      BeginStep("Client request contains “uri=*” element ", authorizedList);

      StepCompleted();

      BeginStep("Device response contains “HTTP/* 200 OK", authorizedList, MessageType.Response);

      if (0 == authorizedList.Count)
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();
    }
  }
}
