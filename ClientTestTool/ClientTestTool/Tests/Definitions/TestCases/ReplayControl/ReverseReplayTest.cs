///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Engine.Enums;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;

namespace ClientTestTool.Tests.Definitions.TestCases.ReplayControl
{
  [Test(
    Name = "Reverse Replay",
    Category = Category.ProfileG,
    Id = "5",
    FeatureUnderTest = Feature.ReverseReplay
  )]
  public class ReverseReplayTest : BaseTest
  {
    protected override void ProcessConversation(Conversation conversation)
    {
      var responseList = new List<RequestResponsePair>();
      //TODO  [S1-10]

      var rtspMessagesList = conversation.GetMessages(ContentType.Rtsp).ToList();

      var playList = rtspMessagesList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method).ToList();
      AffectedPairs.AddRange(playList);

      BeginStep("Client request introduces RTSP PLAY command", playList);

      if (!playList.Any())
        StepFailed("Client request does not include: RTSP PLAY command");

      StepCompleted();

      BeginStep("RTSP PLAY includes URI", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Uri)))
        StepFailed("RTSP PLAY does not include: URI");

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

      // [S6] RTSP PLAY includes: “Require” parameter with “onvif-replay” value AND
      var requireList = playList.Where(item => !String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Require)).ToList();
      var validRequireList = requireList.Where(item =>
      {
          String value = item.GetRequest<RtspRequest>().Require;
          return value.ToLower().Contains("onvif-replay");
      }).ToList();

      BeginStep("RTSP PLAY includes: \"Require\" parameter with \"onvif-replay\" value", requireList);
     
      if (0 == validRequireList.Count)
        throw new TestNotSupportedException("Require parameter does not have \"onvif-replay\" value");

      StepCompleted();

      //[S7] RTSP PLAY includes: “Scale” parameter with numeric value AND
      var scaleList = playList.Where(item => !String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Scale)).ToList();
      var validScaleList = scaleList.Where(item =>
      {
          bool bRet = false;
          decimal dOut;
          String value = item.GetRequest<RtspRequest>().Scale;
          if (decimal.TryParse(value, out dOut))
          {
              if (dOut < 0.0m)
              {
                  bRet = true;
              }
          }
          return bRet;
      }).ToList();
      BeginStep("RTSP PLAY includes: \"Scale\" parameter with negative numeric value", playList);

      if (0 == scaleList.Count)
          //StepFailed("RTSP PLAY does not include: Scale parameter");
          throw new TestNotSupportedException("RTSP PLAY does not include: Scale parameter");
      else if (0 == validScaleList.Count)
          throw new TestNotSupportedException("Scale parameter does not have negative numeric value");

      StepCompleted();

      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (!responseList.Any())
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();
    }
  }
}
