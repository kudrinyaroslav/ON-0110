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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzPresetsTestCases
{
  [Test(
    Name             = "GetPresets",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.PtzGetPresets
  )]
  public class PtzGetPresetsTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getPresetsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetPresets")).ToList();
      AffectedPairs.AddRange(getPresetsList);

      //[S1] Client request contains “<GetPresets>” tag after the “<Body>”
      BeginStep("Client request contains <GetPresets> tag", getPresetsList);

      if (0 == getPresetsList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetPresets> tag");

      StepCompleted();

      //[S2] “<GetPresets>” includes tag: “<﻿ProfileToken>” with non-empty string value of specific token
      var profileTokenList = getPresetsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetPresets", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<GetPresets> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();


      //[S3] Device response contains “HTTP/* 200 OK”
      var responseList = getPresetsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S4] Device response contains <GetPresetsResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetPresetsResponse")).ToList();

      BeginStep("Device response contains <GetPresetsResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetPresetsResponse> tag is missing");

      StepCompleted();
    }
  }
}
