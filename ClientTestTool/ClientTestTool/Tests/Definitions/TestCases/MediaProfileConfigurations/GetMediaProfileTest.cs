///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
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
    Name             = "Get Specific Media Profile",
    Category         = Category.ProfileS,
    Id               = "2",
    FeatureUnderTest = Feature.GetMediaProfile
    )]
  public class GetMediaProfileTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getProfileList =
        conversation.GetMessages(ContentType.Http)
                    .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetProfile"))
                    .ToList();
      AffectedPairs.AddRange(getProfileList);

      BeginStep("Search for a Client request with <GetProfile> tag", getProfileList);

      if (0 == getProfileList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetProfile> tag");

      StepCompleted();

      var profileTokenList = getProfileList
                             .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetProfile", "ProfileToken"))
                             .ToList();

      BeginStep("Verify that <GetProfile> includes tag: <ProfileToken> with non-empty string value", getProfileList);

      if (!profileTokenList.Any())
        StepFailed("<ProfileToken> tag is not present");
      else if (profileTokenList.All(item => String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken"))))
        StepFailed("Valid value of <ProfileToken> tag is not present");

      StepCompleted();

      var responseList = getProfileList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Check for Device response code", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response HTTP 200 OK is not present");

      StepCompleted();

      BeginStep("Device response contains <GetProfileResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetProfileResponse")))
        StepFailed("<GetProfileResponse> tag is not present");

      StepCompleted();
    }
  }
}