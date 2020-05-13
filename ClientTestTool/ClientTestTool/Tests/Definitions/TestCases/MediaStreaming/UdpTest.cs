///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MediaStreaming {
  [Test(
    Name             = "Streaming Over UDP",
    Category         = Category.ProfileS,
    Id               = "4",
    FeatureUnderTest = Feature.UDP
    )]
  public class UDPTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var getStreamUriList = conversation.GetMessages(ContentType.Http)
                             .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri"))
                             .ToList();
      AffectedPairs.AddRange(getStreamUriList);

      BeginStep("Client request contains <GetStreamUri> tag", getStreamUriList);

      if (0 == getStreamUriList.Count)
        throw new TestNotSupportedException("Conversation does not contain <GetStreamUri> messages");

      StepCompleted();

      var streamSetupList = getStreamUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri", "StreamSetup"))
                                            .ToList();

      BeginStep("<GetStreamUri> includes tag <StreamSetup>", streamSetupList);

      if (0 == streamSetupList.Count)
        StepFailed("Request <GetStreamUri> does not include tag <StreamSetup>");

      StepCompleted();

      var streamList = streamSetupList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("StreamSetup", "Stream"))
                                      .ToList();

      BeginStep("<StreamSetup> includes tag <Stream> with \"RTP-unicast\" or \"RTP-multicast\" parameter", streamList);

      if (!streamList.Any(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Stream");
        return null != value && (value.ToUpper().Contains("RTP-UNICAST") ||
                                 value.ToUpper().Contains("RTP-MULTICAST"));
      }))
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

        var udpList = protocolList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Protocol");
        return null != value && value.ToUpper().Contains("UDP");
      }).ToList();

      BeginStep("<Transport> includes tag <Protocol> with \"UDP\" parameter", udpList);

      if (protocolList.Any() && !udpList.Any())
        throw new TestNotSupportedException("Request <Transport> does not include tag <Protocol> with \"UDP\" parameter");

      if (!udpList.Any())
        StepFailed("Request <Transport> does not include tag <Protocol> with \"UDP\" parameter");

      StepCompleted();

      var profileTokenList = getStreamUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri", "ProfileToken"))
                                   .ToList();

      BeginStep("<GetStreamUri> includes tag <ProfileToken> with a non-empty string value", profileTokenList);

      if (!profileTokenList.Any(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken");
        return !String.IsNullOrEmpty(value);
      }))
        StepFailed("Request <GetStreamUri> does not include tag <ProfileToken> with a non-empty string value");

      StepCompleted();

      var responseList = getStreamUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode)
                                   .ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("GetStreamUriResponse")).ToList();

      BeginStep("Device response contains <GetStreamUriResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetStreamUriResponse> tag is missing");

      StepCompleted();

      var mediaUriList = responseList.Where(item => item.GetResponse<HttpMessage>().ContainsTag("GetStreamUriResponse", "MediaUri"))
                                     .ToList();

      BeginStep("<GetStreamUriResponse> includes tag: <MediaUri>", mediaUriList);

      if (0 == mediaUriList.Count)
        StepFailed("<GetStreamUriResponse> does not include tag: <MediaUri>");

      StepCompleted();

      var uriList = mediaUriList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetResponse<HttpMessage>(), "Uri")))
                                .ToList();

      BeginStep("<MediaUri> includes tag: <Uri> with valid URI address", uriList);

      if (0 == uriList.Count)
        StepFailed("<MediaUri> does not include tag: <Uri> with valid URI address");

      StepCompleted();

      var rtspList = conversation.GetMessages(ContentType.Rtsp);

      var describeList = rtspList.Where(item => RtspMethod.DESCRIBE == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(describeList);

      BeginStep("Client invokes RTSP DESCRIBE request to retrieve media stream description", describeList);

      if (!rtspList.Any())
        throw new TestNotSupportedException("Conversation doest not contain RTSP messages");

      if (!describeList.Any())
        throw new TestNotSupportedException("Conversation does not contain RTSP DESCRIBE messages");

      StepCompleted();

      responseList = describeList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device responses does not include code RTSP 200 OK");

      StepCompleted();

      var setupList = rtspList.Where(item => RtspMethod.SETUP == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(setupList);

      BeginStep("Client invokes RTSP SETUP request with transport parameter element to set media session parameters", setupList);

      if (!setupList.Any())
        StepFailed("Client request does not include: RTSP SETUP");

      StepCompleted();

      responseList = setupList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device responses does not include: RTSP 200 OK");

      StepCompleted();

      var playList = rtspList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(playList);

      BeginStep("Client invokes RTSP PLAY request to start media stream", playList);

      if (!playList.Any())
        StepFailed("Client request does not include: RTSP PLAY");

      StepCompleted();

      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device responses does not include RTSP 200 OK");

      StepCompleted();

      var teardownList = rtspList.Where(item => RtspMethod.TEARDOWN == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(teardownList);
      BeginStep("Client invokes RTSP TEARDOWN request to terminate the RTSP session", teardownList);

      if (!teardownList.Any())
        StepFailed("Client request does not include:  RTSP TEARDOWN");

      StepCompleted();

      responseList = teardownList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device responses does not include RTSP 200 OK");

      StepCompleted();
    }
  }
}
