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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzAbsolutePositioning
{
  [Test(
    Name             = "AbsoluteMove Zoom",
    Id               = "2",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.AbsoluteZoom
  )]
  public class AbsoluteZoomTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var absoluteMoveList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AbsoluteMove")).ToList();
      AffectedPairs.AddRange(absoluteMoveList);

      BeginStep("Client request contains <AbsoluteMove> tag", absoluteMoveList);

      if (0 == absoluteMoveList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <AbsoluteMove> tag");

      StepCompleted();

      var profileTokenList = absoluteMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AbsoluteMove", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<AbsoluteMove> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] “<AbsoluteMove>” includes tag: “<Position>” 
      var positionList = absoluteMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "AbsoluteMove", "Position")).ToList();
      AffectedPairs.AddRange(positionList);

      BeginStep("<AbsoluteMove> includes tag: <Position>", positionList);

      if (0 == positionList.Count)
        StepFailed("<Position> tag is missing");

      StepCompleted();


      //[S5] <Zoom> tag contains attribute: x= with value (example: -1, 0.1, 1, ...)
      var zoomList = positionList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Position", "Zoom")).ToList();
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

      var responseList = absoluteMoveList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "AbsoluteMoveResponse")).ToList();

      BeginStep("Device response contains <AbsoluteMoveResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<AbsoluteMoveResponse> tag is missing");

      StepCompleted();
    }
  }
}
