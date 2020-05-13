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

namespace ClientTestTool.Tests.Definitions.TestCases.MediaProfileConfigurations
{
  [Test(
    Name              = "Create a Media Profile",
    Category          = Category.ProfileS,
    Id                = "3",
    FeatureUnderTest = Feature.CreateMediaProfile
    )]
  public class CreateMediaProfileTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var createProfileList = conversation.GetMessages(ContentType.Http).Where(item => item.GetRequest<HttpMessage>().ContainsTag("CreateProfile")).ToList();

      AffectedPairs.AddRange(createProfileList);

      BeginStep("Search for a Client request with <CreateProfile> tag", createProfileList);

      if (0 == createProfileList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <CreateProfile> tag");

      StepCompleted();

      var nameList = createProfileList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "CreateProfile", "Name")).ToList();

      BeginStep("Verify that <CreateProfile> includes tag: <Name> with non-empty string value", nameList);

      if (!nameList.Any())
        StepFailed("<Name> tag is missing");

      if (nameList.All(item => String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Name"))))
        StepFailed("Valid value of <Name> tag is not present");

      StepCompleted();

      var tokenList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Token"))
          .ToList();

      if (0 == tokenList.Count)
      {
        BeginStep("Client request does not contain <Token> tag", null);
        StepCompleted();
      }
      else
      {
        var intersectList = createProfileList.Intersect(tokenList).ToList();

        BeginStep("<CreateProfile> includes tag: <Token> with non-empty string value of specific profile token", intersectList);

        if (0 == intersectList.Count)
          StepFailed("<Token> tag is not specified");
        else if (intersectList.All(item => String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Token"))))
          StepFailed("Valid value of <Token> tag is not present");

        StepCompleted();
      }

      var responseList = createProfileList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Check for Device response code", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      BeginStep("Device response contains <CreateProfileResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "CreateProfileResponse")))
        StepFailed("<CreateProfileResponse> tag is not present");

      StepCompleted();
    }
  }
}