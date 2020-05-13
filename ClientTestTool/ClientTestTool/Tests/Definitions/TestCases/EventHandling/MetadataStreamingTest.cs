///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.EventHandling // TODO
{
  [Test(
    Name             = "Metadata Streaming",
    Category         = Category.Core,
    Id               = "3",
    FeatureUnderTest = Feature.MetadataStreaming
    )]
  public class MetadataStreamingTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      List<RequestResponsePair> getProfilesList = conversation.GetMessages(ContentType.Http);
      getProfilesList = getProfilesList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetProfiles")).ToList();

      AffectedPairs.AddRange(getProfilesList);

      BeginStep("Client request contains <GetProfiles> tag", getProfilesList);

      if (!getProfilesList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <GetProfiles> tag");

      StepCompleted();

      var responseList = getProfilesList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      BeginStep("Client request contains <GetProfilesResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any(item => item.GetResponse<HttpResponse>().ContainsTag("GetProfilesResponse")))
        StepFailed("Client request does not contains tag: <GetProfilesResponse>");

      StepCompleted();


      var getStreamUriList = conversation.GetMessages(ContentType.Http);
      getStreamUriList = getStreamUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri")).ToList();

      AffectedPairs.AddRange(getProfilesList);

      BeginStep("Client request contains <GetStreamUri> tag", getStreamUriList);

      if (!getStreamUriList.Any())
        throw new TestNotSupportedException("Conversation does not contain requests with <GetStreamUri> tag");

      StepCompleted();

      var streamSetupList = getStreamUriList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpRequest>(), "GetStreamUri", "StreamSetup")).ToList();
      AffectedPairs.AddRange(streamSetupList);

      BeginStep("<GetStreamUri> includes <StreamSetup> tag", streamSetupList);

      if (!streamSetupList.Any())
        StepFailed("<GetStreamUri> does not includes tag: <StreamSetup>");

      StepCompleted();

      var streamList = streamSetupList
                       .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpRequest>(), "StreamSetup", "Stream"))
                       .ToList();

      BeginStep("<StreamSetup> includes tag <Stream> with (RTP-UNICAST OR RTP-MULTICAST) value", streamList);

      if (!streamSetupList.Any(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Stream");

        if (null == value)
          return false;

        value = value.ToUpper();
        return value.Contains("RTP-UNICAST") || value.Contains("RTP-MULTICAST");
      }))
        StepFailed("<StreamSetup> does not includes tag: <Stream> with (RTP-UNICAST OR RTP-MULTICAST) value");

      StepCompleted();

      var transportList = streamSetupList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpRequest>(), "StreamSetup", "Transport")).ToList();

      BeginStep("<StreamSetup> includes <Transport> tag", transportList);

      if (!transportList.Any())
        StepFailed("<StreamSetup> does not includes tag: <Transport>");

      StepCompleted();

      var protocolList = transportList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpRequest>(), "Transport", "Protocol"))
                                      .Where(item =>
                                      {
                                        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Protocol");

                                        return null != value &&
                                               (value.ToUpper().Contains("UDP") ||
                                                value.ToUpper().Contains("HTTP") ||
                                                value.ToUpper().Contains("RTSP"));
                                      }).ToList();

      BeginStep("<Transport> includes <Protocol> tag with (UDP OR HTTP OR RTSP) value ", protocolList);

      if (!protocolList.Any())
        StepFailed("<Transport> does not includes tag:  <Protocol> tag with (UDP OR HTTP OR RTSP) value");

      StepCompleted();

      var profileTokenList = getStreamUriList
        .Where(item => TestUtil.ContainsTag(item.GetRequest<HttpRequest>(), "GetStreamUri", "ProfileToken"))
        .Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")))
        .ToList();

      BeginStep("<GetStreamUri> includes <ProfileToken> tag with a string value of specified profile token", profileTokenList);

      if (!profileTokenList.Any())
        StepFailed("<GetStreamUri> does not includes tag: <ProfileToken> tag with a string value of specified profile token");

      StepCompleted();

      responseList = getStreamUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("<GetStreamUri> does not contains tag: \"HTTP/* 200 OK\"");

      StepCompleted();

      responseList = responseList.Where(item => item.GetResponse<HttpResponse>().ContainsTag("GetStreamUriResponse")).ToList();

      BeginStep("Device response contains <GetStreamUriResponse> tag", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains tag: <GetStreamUriResponse>");

      StepCompleted();

      var rtspMessagesList = conversation.GetMessages(ContentType.Rtsp).ToList();

      var describeList = rtspMessagesList.Where(item => RtspMethod.DESCRIBE == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(describeList);

      BeginStep("Client request introduces RTSP DESCRIBE command", describeList);

      if (!rtspMessagesList.Any())
        throw new TestNotSupportedException("Conversation does not contain any RTSP requests");

      if (!describeList.Any())
        StepFailed("Client request does not include RTSP DESCRIBE command");

      StepCompleted();

      BeginStep("RTSP DESCRIBE includes URI", describeList);

      if (describeList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP DESCRIBE does not include  URI");

      StepCompleted();

      BeginStep("RTSP DESCRIBE includes: RTSP/* identifier", describeList);

      bool describeIncludesRtspIdentifier = describeList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!describeIncludesRtspIdentifier)
        StepFailed("RTSP DESCRIBE does not include RTSP/* identifier");

      StepCompleted();

      BeginStep("RTSP DESCRIBE includes: CSeq identifier", describeList);

      if (!describeList.Any())
        StepFailed("RTSP DESCRIBE does not include CSeq identifier");

      StepCompleted();

      responseList = describeList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not include RTSP/* 200 OK");

      StepCompleted();

      var validMimeTypeList  = describeList.Where(item => item.GetResponse<RtspResponse>().MIMETypes.Any (type => type.Contains("vnd.onvif.metadata"))).ToList();
      var validMediaTypeList = describeList.Where(item => item.GetResponse<RtspResponse>().MediaTypes.Any(type => type.Contains("application"))).ToList();

      var validStreamTypeList = validMimeTypeList.Intersect(validMediaTypeList).ToList();

      BeginStep("Device response SDP information contains Media Type: “application” and MIME Type: “vnd.onvif.metadata”", validStreamTypeList, MessageType.Response);

      if (0 == validStreamTypeList.Count)
        throw new TestNotSupportedException("Valid stream type information is not present");

      StepCompleted();

      var setupList = rtspMessagesList.Where(item => RtspMethod.SETUP == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(setupList);

      BeginStep("Client request introduces RTSP SETUP command", setupList);

      if (!setupList.Any())
        StepFailed("Client request does not include RTSP SETUP command");

      StepCompleted();

      BeginStep("RTSP SETUP includes URL", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP SETUP does not include RTSP SETUP includes URL");

      StepCompleted();

      BeginStep("RTSP SETUP includes: RTSP/* identifier", setupList);

      bool setupIncludesRtspIdentifier = setupList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!setupIncludesRtspIdentifier)
        StepFailed("RTSP SETUP does not include RTSP/* identifier");

      StepCompleted();

      BeginStep("RTSP SETUP includes: CSeq identifier", setupList);

      //RTSP message parser by NTParser is always contains CSeq identifier

      StepCompleted();

      BeginStep("RTSP SETUP includes: Transport command", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Transport)))
        StepFailed("RTSP SETUP does not include Transport command");

      StepCompleted();

      responseList = setupList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device does not contains RTSP/* 200 OK");

      StepCompleted();

      var playList = rtspMessagesList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(playList);

      BeginStep("Client request introduces RTSP PLAY command", playList);

      if (!playList.Any())
        StepFailed("Client request does not include RTSP PLAY command");

      StepCompleted();

      BeginStep("RTSP PLAY includes URL", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP PLAY does not include URL");

      StepCompleted();

      BeginStep("RTSP PLAY includes: RTSP/* identifier", playList);

      bool playIncludesRtspIdentifier = playList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!playIncludesRtspIdentifier)
        StepFailed("RTSP PLAY does not include RTSP/* identifier");

      StepCompleted();

      BeginStep("RTSP PLAY includes: CSeq identifier", playList);

      //RTSP message parser by NTParser is always contains CSeq identifier
      if (!playList.Any())
        StepFailed("RTSP PLAY does not include CSeq identifier");

      StepCompleted();

      BeginStep("RTSP PLAY includes: Session command", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("RTSP PLAY does not include Session command");

      StepCompleted();

      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();
    }
  }
}
