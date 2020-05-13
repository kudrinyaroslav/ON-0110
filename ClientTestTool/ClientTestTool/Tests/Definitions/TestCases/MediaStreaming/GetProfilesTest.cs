///
/// @Author Matthew Tuusberg
///

﻿using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.MediaStreaming
{
  [Test(
    Name             = "Get Profiles",
    Category         = Category.ProfileS,
    Id               = "1",
    FeatureUnderTest = Feature.GetProfiles
    )]
  public class GetProfilesTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getProfilesList = conversation.GetMessages(ContentType.Http)
                                         .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetProfiles"))
                                         .ToList();

      AffectedPairs.AddRange(getProfilesList);

      BeginStep("Client request contains <GetProfiles> tag", getProfilesList);

      if (0 == getProfilesList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetProfiles> tag");

      StepCompleted();

      var responseList = getProfilesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == getProfilesList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Device response contains <GetProfilesResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any(item => TestUtil.ContainsTag(item.GetResponse<HttpResponse>(), "GetProfilesResponse")))
        StepFailed("<GetProfilesResponse> tag is missing");

      StepCompleted();
    }
  }
}
