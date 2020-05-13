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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzHomePositionTestCases
{
  [Test(
      Name             = "GotoHomePosition",
      Id               = "1",
      Category         = Category.ProfileS,
      FeatureUnderTest = Feature.PtzGotoHomePosition
  )]
  public class PtzGotoHomePositionTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var gotoHomePositionList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GotoHomePosition")).ToList();
      AffectedPairs.AddRange(gotoHomePositionList);

      //[S1] Client request contains “<gotoHomePosition>” tag after the “<Body>”
      BeginStep("Client request contains <GotoHomePosition> tag", gotoHomePositionList);

      if (0 == gotoHomePositionList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GotoHomePosition> tag");

      StepCompleted();

      //[S2] “<GotoHomePosition>” includes tag: “<﻿ProfileToken>” with non-empty string value of specific token
      var profileTokenList = gotoHomePositionList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GotoHomePosition", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<GotoHomePosition> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] Device response contains “HTTP/* 200 OK”
      var responseList = gotoHomePositionList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S4] Device response contains <GotoHomePositionResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GotoHomePositionResponse")).ToList();

      BeginStep("Device response contains <GotoHomePositionResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GotoHomePositionResponse> tag is missing");

      StepCompleted();
    }
  }
}
