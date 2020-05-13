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

namespace ClientTestTool.Tests.Definitions.TestCases.PtzRelativePositioning
{
  [Test(
    Name             = "RelativeMove PanTilt",
    Id               = "1",
    Category         = Category.ProfileS,
    FeatureUnderTest = Feature.PtzRelativeMovePanTilt
  )]
  public class RelativeMovePanTiltTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var relativeMoveList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "RelativeMove")).ToList();
      AffectedPairs.AddRange(relativeMoveList);

      BeginStep("Client request contains <RelativeMove> tag", relativeMoveList);

      if (0 == relativeMoveList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <RelativeMove> tag");

      StepCompleted();

      var profileTokenList = relativeMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "RelativeMove", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<RelativeMove> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S3] “<RelativeMove>” includes tag: “<Translation>” 
      var translationList = relativeMoveList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "RelativeMove", "Translation")).ToList();
      AffectedPairs.AddRange(translationList);

      BeginStep("<RelativeMove> includes tag: <Translation>", translationList);

      if (0 == translationList.Count)
        StepFailed("<Translation> tag is missing");

      StepCompleted();

      var panTiltList = translationList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Translation", "PanTilt")).ToList();
      AffectedPairs.AddRange(panTiltList);

      var panTiltAttributesList = panTiltList.Select(item => TestUtil.AttributesOf(item.GetRequest<HttpMessage>(), "PanTilt")).ToList();
      BeginStep("<PanTilt> tag contains attribute: “x=” with value (example: -1, 0.1, 1, ...)", panTiltList);

      if (!panTiltAttributesList.Any(item =>
      {
          var x = item.FirstOrDefault(attr => "x" == attr.Name);
          return null != x && !String.IsNullOrEmpty(x.Value);
      }))
        StepFailed("<PanTilt> tag does not contain attribute: \"x=\" with value");

      StepCompleted();

      BeginStep("<PanTilt> tag contains attribute: “y=” with value (example: -1, 0.1, 1, ...)", panTiltList);

      if (!panTiltAttributesList.Any(item =>
      {
          var y = item.FirstOrDefault(attr => "y" == attr.Name);
          return null != y && !String.IsNullOrEmpty(y.Value);
      }))
        StepFailed("<PanTilt> tag does not contain attribute: \"y=\" with value");

      StepCompleted();

      var responseList = relativeMoveList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "RelativeMoveResponse")).ToList();

      BeginStep("Device response contains <RelativeMoveResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<RelativeMoveResponse> tag is missing");

      StepCompleted();

    }

  }
}
