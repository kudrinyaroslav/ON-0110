///
/// @Author Matthew Tuusberg
///

﻿using System;
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

namespace ClientTestTool.Tests.Definitions.TestCases.AudioStreaming.Base
{
  public abstract class BaseAudioStreamingTest : BaseTest
  {
    protected void ProcessConversation(Conversation conversation, String sdpInfo) // SDP info in [S15]
    {
      var filteredList = conversation.GetMessages(ContentType.Http);

      var getProfileslist = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetProfiles")).ToList();
      AffectedPairs.AddRange(getProfileslist);

      //[S1] Client request contains <GetProfiles> tag after the <Body>
      BeginStep("Client request contains <GetProfiles> tag", getProfileslist);

      if (0 == getProfileslist.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetProfiles> tag");

      StepCompleted();

      //[S2] Device response contains HTTP/* 200 OK
      var responseList = getProfileslist.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S3] Device response contains <getStreamUriResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetProfilesResponse")).ToList();

      BeginStep("Device response contains <GetProfilesResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetProfilesResponse> tag is missing");

      StepCompleted();

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

      var getStreamUriList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetStreamUri")).ToList();
      AffectedPairs.AddRange(getStreamUriList);

      //[S4] Client request contains <GetStreamUri> tag after the <Body>
      BeginStep("Client request contains <GetStreamUri> tag", getStreamUriList);

      if (0 == getStreamUriList.Count)
        throw new TestNotSupportedException("Conversation does not contain messages with <GetStreamUri> tag");

      StepCompleted();

      //[S5] <GetStreamUri> includes tag: <﻿ProfileToken> with non-empty string value of specific token
      var profileTokenList = getStreamUriList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetStreamUri", "ProfileToken")).ToList();
      var valuesList = profileTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")).ToList();

      BeginStep("<GetStreamUri> includes tag: <ProfileToken> with non-empty string value of specific token", profileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (valuesList.All(String.IsNullOrEmpty))
        StepFailed("Value of <ProfileToken> tag is empty");

      StepCompleted();

      //[S6] Device response contains HTTP/* 200 OK
      responseList = getStreamUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      //[S7] Device response contains <getStreamUriResponse> tag.
      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetStreamUriResponse")).ToList();
      AffectedPairs.AddRange(responseList);

      BeginStep("Device response contains <GetStreamUriResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetStreamUriResponse> tag is missing");

      StepCompleted();

      //[S8] <GetStreamUriResponse> includes tag: <MediaUri>
      var mediaUriList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetStreamUriResponse", "MediaUri")).ToList();

      BeginStep("<GetStreamUriResponse> includes tag: <MediaUri>", responseList);

      if (0 == mediaUriList.Count)
        StepFailed("<MediaUri> tag is missing");

      StepCompleted();
      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

      //[S9] <MediaUri> includes tag: <Uri> with valid URI address
      var uriList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "MediaUri", "Uri")).ToList();

      BeginStep(" <MediaUri> includes tag: <Uri> with valid URI address", responseList);

      if (0 == uriList.Count)
        StepFailed("<Uri> tag is missing");

      StepCompleted();
      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

      var rtspMessagesList = conversation.GetMessages(ContentType.Rtsp).ToList();

      //[S10] Client request introduces RTSP DESCRIBE command 
      var describeList = rtspMessagesList.Where(item => RtspMethod.DESCRIBE == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(describeList);

      BeginStep("Client request introduces RTSP DESCRIBE command", describeList);

      if (!describeList.Any())
        StepFailed("Client request does not include: RTSP DESCRIBE command");

      StepCompleted();

      //[S11] RTSP DESCRIBE includes: URI address obtained from GetStreamUriResponse 
      BeginStep("RTSP DESCRIBE includes URI address obtained from GetStreamUriResponse", describeList);

      if (describeList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP DESCRIBE does not include: URI");

      StepCompleted();

      //[S12] RTSP DESCRIBE includes: “RTSP/*” version identifier
      BeginStep("[S12] RTSP DESCRIBE includes: RTSP/* version identifier", describeList);

      bool describeIncludesRtspIdentifier = describeList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!describeIncludesRtspIdentifier)
        StepFailed("RTSP DESCRIBE does not include RTSP/* version identifier");

      StepCompleted();

      //[S13] RTSP DESCRIBE includes: CSeq identifier 
      BeginStep("RTSP DESCRIBE includes: CSeq identifier", describeList);

      //RTSP message parser by NTParser is always contains CSeq identifier
      if (!describeList.Any())
        StepFailed("RTSP DESCRIBE does not include CSeq identifier");

      StepCompleted();

      //[S14] Device response contains “RTSP/* 200 OK”
      responseList = describeList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();

      // [S15] Device response SDP information contains Media Type: “audio” and MIME Type: “PCMU”
      var validMimeTypeList = describeList.Where(item => item.GetResponse<RtspResponse>().MIMETypes.Any(type => type.Contains(sdpInfo))).ToList();
      var validMediaTypeList = describeList.Where(item => item.GetResponse<RtspResponse>().MediaTypes.Any(type => type.Contains("audio"))).ToList();

      BeginStep("Device response SDP information contains Media Type: audio and MIME Type: " + sdpInfo, validMimeTypeList, MessageType.Response);

      if ((!validMimeTypeList.Any()) || (!validMediaTypeList.Any()))
        throw new TestNotSupportedException("Conversation does not contain response for DESCRIBE command with valid stream type information");

      StepCompleted();

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      // [S16]  
      var setupList = rtspMessagesList.Where(item => RtspMethod.SETUP == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(setupList);

      BeginStep("Client request introduces RTSP SETUP command", setupList);

      if (!setupList.Any())
        StepFailed("Client request does not include: RTSP SETUP command");

      StepCompleted();

      //[S17] RTSP SETUP includes: URI address 
      BeginStep("RTSP SETUP includes URI", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP SETUP does not include: RTSP SETUP includes URI");

      StepCompleted();


      //[S18] Client request introduces RTSP SETUP command
      BeginStep("RTSP SETUP includes: RTSP/* identifier", setupList);

      bool setupIncludesRtspIdentifier = setupList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!setupIncludesRtspIdentifier)
        StepFailed("RTSP SETUP does not include: RTSP/* identifier");

      StepCompleted();
      //[S19] RTSP SETUP includes: “CSeq” identifier
      BeginStep("RTSP SETUP includes: CSeq identifier", setupList);

      //RTSP message parser by NTParser is always contains CSeq identifier

      StepCompleted();

      //[S20] RTSP SETUP includes: “Transport” parameter 
      BeginStep("RTSP SETUP includes: Transport parameter", setupList);

      if (setupList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Transport)))
        StepFailed("RTSP SETUP does not include: Transport parameter");

      StepCompleted();

      //[S21] Device response contains “RTSP/* 200 OK”
      responseList = setupList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device does not contains RTSP/* 200 OK");

      StepCompleted();

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      var playList = rtspMessagesList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(playList);

      //[S22] Client request introduces RTSP PLAY command
      BeginStep("Client request introduces RTSP PLAY command", playList);

      if (!playList.Any())
        StepFailed("Client request does not include: RTSP PLAY command");

      StepCompleted();

      //[S23] RTSP PLAY includes: the same URI address as in RTSP DESCRIBE request 
      BeginStep("RTSP PLAY includes the same URI address as in RTSP DESCRIBE request", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP PLAY does not include: URI");

      StepCompleted();

      //[S24] RTSP PLAY includes: “RTSP/*” version identifier
      BeginStep("RTSP PLAY includes: RTSP/* identifier", playList);

      bool playIncludesRtspIdentifier = playList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!playIncludesRtspIdentifier)
        StepFailed("RTSP PLAY does not include: RTSP/* identifier");

      StepCompleted();

      //[S25] RTSP PLAY includes: “CSeq” identifier
      BeginStep("RTSP PLAY includes: CSeq identifier", playList);

      //RTSP message parser by NTParser is always contains CSeq identifier
      if (!playList.Any())
        StepFailed("RTSP PLAY does not include: CSeq identifier");

      StepCompleted();

      //[S26] RTSP PLAY includes: Session parameter 
      BeginStep("RTSP PLAY includes: Session command", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("RTSP PLAY does not include: Session command");

      StepCompleted();

      //[S27] Device response contains “RTSP/* 200 OK”.
      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();

    }
  }
}
