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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzContiniousPositioning
{
  [Test(
   Name             = "Continuous Move Zoom",
   Id               = "2",
   Category         = Category.ProfileS,
   FeatureUnderTest = Feature.ContinuousMoveZoom
   )]
  public class ContinuousMoveZoomTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var continuousMoveList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove")).ToList();
      AffectedPairs.AddRange(continuousMoveList);

      BeginStep("Client request contains <ContinuousMove> tag", continuousMoveList);

      if (0 == continuousMoveList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <ContinuousMove> tag");

      StepCompleted();

      var profileTokenList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<ContinuousMove> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      var velocityList = continuousMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "ContinuousMove", "Velocity")).ToList();
      AffectedPairs.AddRange(velocityList);

      BeginStep("<ContinuousMove> includes tag: <Velocity>", velocityList);

      if (0 == velocityList.Count)
        StepFailed("<Velocity> tag is missing");

      StepCompleted();

      var zoomList = velocityList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Velocity", "Zoom")).ToList();
      AffectedPairs.AddRange(zoomList);

      var zoomAttributesList = zoomList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "Zoom")).ToList();
      BeginStep("<Zoom> tag contains attribute: “x=” with value (example: -1, 0.1, 1, ...)", zoomList);

      if (!zoomAttributesList.Any(item =>
      {
        var x = item.FirstOrDefault(attr => "x" == attr.Name);
        return null != x && !String.IsNullOrEmpty(x.Value);
      }))
        StepFailed("<Zoom> tag does not contain attribute: \"x=\" with value");

      StepCompleted();

      var responseList = continuousMoveList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "ContinuousMoveResponse")).ToList();

      BeginStep("Device response contains <ContinuousMoveResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<ContinuousMoveResponse> tag is missing");

      StepCompleted();
    }

  }
}
