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
      Name             = "GotoPreset",
      Id               = "2",
      Category         = Category.ProfileS,
      FeatureUnderTest = Feature.PtzGotoPreset
  )]
  public class PtzGotoPresetTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var gotoPresetList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GotoPreset")).ToList();
      AffectedPairs.AddRange(gotoPresetList);

      //[S1] Client request contains “<gotoPreset>” tag after the “<Body>”
      BeginStep("Client request contains <GotoPreset> tag", gotoPresetList);

      if (0 == gotoPresetList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GotoPreset> tag");

      StepCompleted();

      //[S2] “<GotoPreset>” includes tag: “<﻿ProfileToken>” with non-empty string value of specific token
      var profileTokenList = gotoPresetList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GotoPreset", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<GotoPreset> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] “<GotoPreset>” includes tag: “<﻿PresetToken>” with non-empty string value of specific token
      var presetTokenList = gotoPresetList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GotoPreset", "PresetToken")).ToList();
      valuesList = presetTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "PresetToken")).ToList();

      BeginStep("<GotoPreset> includes tag: <PresetToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<PresetToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <PresetToken> tag is empty");

      StepCompleted();

      //[S4] Device response contains “HTTP/* 200 OK”
      var responseList = gotoPresetList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S5] Device response contains <GotoPresetResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GotoPresetResponse")).ToList();

      BeginStep("Device response contains <GotoPresetResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GotoPresetResponse> tag is missing");

      StepCompleted();
    }
  }
}
