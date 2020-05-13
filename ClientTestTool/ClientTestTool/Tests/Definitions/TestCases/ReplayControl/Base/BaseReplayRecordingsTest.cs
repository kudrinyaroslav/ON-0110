///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Utils;

namespace ClientTestTool.Tests.Definitions.TestCases.ReplayControl.Base
{
  public abstract class BaseReplayRecordingsTest : BaseTest
  {
    protected void ProcessConversation(Conversation conversation, String protocol)
    {

      var responseList = new List<RequestResponsePair>();
      //TODO  [S1-10]

      var filteredList = conversation.GetMessages(ContentType.Http);

      var getReplayUriList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetReplayUri")).ToList();
      AffectedPairs.AddRange(getReplayUriList);

      BeginStep("Client request contains <GetReplayUri> tag", getReplayUriList);

      if (0 == getReplayUriList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetReplayUri> tag");

      StepCompleted();

      var streamSetupList = getReplayUriList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetReplayUri", "StreamSetup")).ToList();
      AffectedPairs.AddRange(streamSetupList);

      BeginStep("<GetReplayUri> includes tag: <StreamSetup> ", streamSetupList);

      if (0 == streamSetupList.Count)
        StepFailed("<StreamSetup> tag is missing");

      StepCompleted();

      var recordingTokenList = getReplayUriList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetReplayUri", "RecordingToken")).ToList();
      var valuesList = recordingTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "RecordingToken")).ToList();
      AffectedPairs.AddRange(recordingTokenList);

      BeginStep("<GetReplayUri> includes tag: <RecordingToken> with non-empty string value of specific token", recordingTokenList);

      if (0 == recordingTokenList.Count)
        StepFailed("<RecordingToken> tag is missing");
      else if (valuesList.All(item => String.IsNullOrEmpty(item)))
        StepFailed("Value of <RecordingToken> tag is empty");

      StepCompleted();

      responseList = getReplayUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();
      AffectedPairs.AddRange(responseList);

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      var getReplayUriResponseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetReplayUriResponse")).ToList();
      AffectedPairs.AddRange(getReplayUriResponseList);

      BeginStep("Device response contains <GetReplayUriResponse> tag", getReplayUriResponseList, MessageType.Response);

      if (0 == getReplayUriResponseList.Count)
        StepFailed("<GetReplayUriResponse> tag is missing");

      StepCompleted();

      //[S6]
      var uriList = getReplayUriList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetReplayUriResponse", "Uri")).ToList();
      AffectedPairs.AddRange(uriList);

      BeginStep("<GetReplayUriResponse> includes tag: <Uri> ", getReplayUriList, MessageType.Response);

      if (0 == uriList.Count)
        StepFailed("<Uri> tag is missing");

      StepCompleted();

      //[S7]
      var rtspMessagesList = conversation.GetMessages(ContentType.Rtsp).ToList();

      var describeList = rtspMessagesList.Where(item => RtspMethod.DESCRIBE == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(describeList);

      BeginStep("Client request introduces RTSP DESCRIBE command", describeList);

      if (!rtspMessagesList.Any())
        throw new TestNotSupportedException("Conversation does not contain any RTSP requests");

      if (!describeList.Any())
        StepFailed("Client request does not include: RTSP DESCRIBE command");

      StepCompleted();

      //[S8]
      BeginStep("RTSP DESCRIBE includes URI", describeList);

      if (describeList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP DESCRIBE does not include: RTSP DESCRIBE includes URI");

      StepCompleted();

      //[S9]
      BeginStep("RTSP DESCRIBE includes: RTSP/* identifier", describeList);

      bool describeIncludesRtspIdentifier = describeList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!describeIncludesRtspIdentifier)
        StepFailed("RTSP DESCRIBE does not include: RTSP DESCRIBE includes: RTSP/* identifier");

      StepCompleted();

      //[S10]
      BeginStep("RTSP DESCRIBE includes: CSeq identifier", describeList);

      if (!describeList.Any())
        StepFailed("RTSP DESCRIBE does not include: RTSP DESCRIBE includes: CSeq identifier");

      StepCompleted();

      //[S11]
      responseList = describeList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not include: RTSP/* 200 OK");

      StepCompleted();
      // [S12] Todo "Device response SDP information contains Media Type: “video” and MIME Type: “JPEG”  
      var validMimeTypeList = describeList.Where(item => item.GetResponse<RtspResponse>().MIMETypes.Any(type => type.Contains("JPEG"))).ToList();
      var validMediaTypeList = describeList.Where(item => item.GetResponse<RtspResponse>().MediaTypes.Any(type => type.Contains("video"))).ToList();

      BeginStep("Device response SDP information contains Media Type: video and MIME Type: JPEG", validMimeTypeList, MessageType.Response);

      if ((!validMimeTypeList.Any()) || (!validMediaTypeList.Any()))
        throw new TestNotSupportedException("Conversation does not contain response for DESCRIBE command with valid stream type information");

      StepCompleted();
      // [S13]  
      var setupList = rtspMessagesList.Where(item => RtspMethod.SETUP == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(setupList);

      BeginStep("Client request introduces RTSP SETUP command", setupList);

      if (!setupList.Any())
        StepFailed("Client request does not include: RTSP SETUP command");

      StepCompleted();
      //[S14]
      BeginStep("RTSP SETUP includes URI", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP SETUP does not include: RTSP SETUP includes URI");

      StepCompleted();

      //[S15]
      BeginStep("RTSP SETUP includes: RTSP/* identifier", setupList);

      bool setupIncludesRtspIdentifier = setupList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!setupIncludesRtspIdentifier)
        StepFailed("RTSP SETUP does not include: RTSP/* identifier");

      StepCompleted();
      //[S16]
      BeginStep("RTSP SETUP includes: CSeq identifier", setupList);

      //RTSP message parser by NTParser is always contains CSeq identifier

      StepCompleted();

      //[S17]
      BeginStep("RTSP SETUP includes: Transport parameter", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Transport)))
        StepFailed("RTSP SETUP does not include: Transport parameter");

      StepCompleted();

      // [S18] RTSP SETUP includes: “Require” parameter with “onvif-replay” AND
      var requireList = setupList.Where(item => !String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Require)).ToList();
      var validRequireList = requireList.Where(item =>
      {
        String value = item.GetRequest<RtspRequest>().Require;
        return value.ToLower().Contains("onvif-replay");
      }).ToList();

      BeginStep("RTSP SETUP includes: \"Require\" parameter with \"onvif-replay\" value", requireList);
      if (0 == requireList.Count)
        StepFailed("RTSP SETUP does not include: Require parameter");
      else if (0 == validRequireList.Count)
        StepFailed("Require parameter does not have \"onvif-replay\" value ");

      StepCompleted();

      //[S19]  
      responseList = setupList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device does not contains RTSP/* 200 OK");

      StepCompleted();

      var playList = rtspMessagesList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(playList);

      BeginStep("Client request introduces RTSP PLAY command", playList);

      if (!playList.Any())
        StepFailed("Client request does not include: RTSP PLAY command");

      StepCompleted();

      BeginStep("RTSP PLAY includes URL", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP PLAY does not include: URL");

      StepCompleted();

      BeginStep("RTSP PLAY includes: RTSP/* identifier", playList);

      bool playIncludesRtspIdentifier = playList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!playIncludesRtspIdentifier)
        StepFailed("RTSP PLAY does not include: RTSP/* identifier");

      StepCompleted();

      BeginStep("RTSP PLAY includes: CSeq identifier", playList);

      //RTSP message parser by NTParser is always contains CSeq identifier
      if (!playList.Any())
        StepFailed("RTSP PLAY does not include: CSeq identifier");

      StepCompleted();

      BeginStep("RTSP PLAY includes: Session command", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("RTSP PLAY does not include: Session command");

      StepCompleted();
      // [S25] RTSP SETUP includes: “Require” parameter with “onvif-replay” AND
      requireList = playList.Where(item => !String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Require)).ToList();
      validRequireList = requireList.Where(item =>
      {
        String value = item.GetRequest<RtspRequest>().Require;
        return value.ToLower().Contains("onvif-replay");
      }).ToList();

      BeginStep("RTSP PLAY includes: \"Require\" parameter with \"onvif-replay\" value", requireList);
      if (0 == requireList.Count)
        StepFailed("RTSP PLAY does not include: Require parameter");
      else if (0 == validRequireList.Count)
        StepFailed("Require parameter does not have \"onvif-replay\" value ");

      StepCompleted();

      //[S26] Device response contains RTSP/* 200 OK
      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();

      var tearDownList = rtspMessagesList.Where(item => RtspMethod.TEARDOWN == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(tearDownList);

      //[S27] Client request introduces RTSP TEARDOWN command
      BeginStep("Client request introduces RTSP TEARDOWN command", playList);

      if (!playList.Any())
        StepFailed("Client request does not include: RTSP TEARDOWN command");

      StepCompleted();

      BeginStep("RTSP TEARDOWN includes: URI address obtained from GetReplayUriResponse", tearDownList);

      if (tearDownList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP TEARDOWN does not include: URI");

      StepCompleted();

      BeginStep("RTSP TEARDOWN includes: RTSP/* identifier", playList);

      bool tearDownIncludesRtspIdentifier = tearDownList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!tearDownIncludesRtspIdentifier)
        StepFailed("RTSP TEARDOWN does not include: RTSP/* identifier");

      StepCompleted();

      BeginStep("RTSP TEARDOWN includes: CSeq identifier", playList);

      //RTSP message parser by NTParser is always contains CSeq identifier
      if (!tearDownList.Any())
        StepFailed("RTSP TEARDOWN does not include: CSeq identifier");

      StepCompleted();

      BeginStep("RTSP TEARDOWN includes: Session command", playList);

      if (tearDownList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("RTSP TEARDOWN does not include: Session command");

      StepCompleted();

      //[S32] Device response contains RTSP/* 200 OK
      responseList = tearDownList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();

    }
  }
}
