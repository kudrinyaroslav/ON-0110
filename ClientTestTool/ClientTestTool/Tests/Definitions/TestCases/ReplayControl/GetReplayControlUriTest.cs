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

namespace ClientTestTool.Tests.Definitions.TestCases.ReplayControl
{
  [Test(
    Name             = "Get Replay Uri",
    Category         = Category.ProfileG,
    Id               = "1",
    FeatureUnderTest = Feature.GetReplayUri
  )]
  public class GetReplayControlUriTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getReplayUriList = conversation.GetMessages(ContentType.Http)
                             .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetReplayUri"))
                             .ToList();
      AffectedPairs.AddRange(getReplayUriList);

      BeginStep("Client request contains <GetReplayUri> tag", getReplayUriList);

      if (0 == getReplayUriList.Count)
        throw new TestNotSupportedException("Conversation does not contain <GetReplayUri> messages");

      StepCompleted();

      var streamSetupList = getReplayUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetReplayUri", "StreamSetup"))
                                            .ToList();

      BeginStep("<GetReplayUri> includes tag <StreamSetup>", streamSetupList);

      if (0 == streamSetupList.Count)
        StepFailed("Request <GetReplayUri> does not include tag <StreamSetup>");

      StepCompleted();

      var streamList = streamSetupList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("StreamSetup", "Stream"))
                                      .ToList();

      var validStreamList = streamList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Stream");
        return null != value && (value.ToUpper().Contains("RTP-UNICAST") ||
                                 value.ToUpper().Contains("RTP-MULTICAST"));
      }).ToList();

      BeginStep("<StreamSetup> includes tag <Stream> with \"RTP-unicast\" or \"RTP-multicast\" parameter", streamList);

      if (0 == streamList.Count)
        StepFailed("<StreamSetup> does not include tag <Stream>");
      else if (0 == validStreamList.Count)
        StepFailed("Request <StreamSetup> does not include tag <Stream> with \"RTP-unicast\" or \"RTP-multicast\" parameter");

      StepCompleted();

      var transportList = streamSetupList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("StreamSetup", "Transport"))
                                         .ToList();

      BeginStep("<StreamSetup> includes tag <Transport>", transportList);

      if (0 == transportList.Count)
        StepFailed("Request <StreamSetup> does not include tag <Transport>");

      StepCompleted();

      var protocolList = transportList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Transport", "Protocol"))
                                .ToList();

      var validProtocolList = protocolList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Protocol");
        return null != value && (value.ToUpper().Contains("UDP") ||
                                 value.ToUpper().Contains("HTTP") ||
                                 value.ToUpper().Contains("RTSP"));
      }).ToList();

      BeginStep("<Transport> includes tag <Protocol> with (\"UDP\" OR \"HTTP\" OR \"RTSP\") parameter", protocolList);

      if (0 == protocolList.Count)
        StepFailed("<Transport> does not include tag <Protocol>");
      else if (0 == validProtocolList.Count)
        StepFailed("Request <Transport> does not include tag <Protocol> with (\"UDP\" OR \"HTTP\" OR \"RTSP\") parameter");

      StepCompleted();

      var recordingToken = getReplayUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetReplayUri", "RecordingToken"))
                                   .ToList();

      BeginStep("<GetReplayUri> includes tag <RecordingToken> with a non-empty string value", recordingToken);

      if (!recordingToken.Any(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "RecordingToken");
        return !String.IsNullOrEmpty(value);
      }))
        StepFailed("Request <GetReplayUri> does not include tag <RecordingToken> with a non-empty string value");

      StepCompleted();

      var responseList = getReplayUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                                   .ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("GetReplayUriResponse")).ToList();

      BeginStep("Device response contains <GetReplayUriResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetReplayUriResponse> tag is absent");

      StepCompleted();     
    }
  }
}
