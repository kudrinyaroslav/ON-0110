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
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MediaStreaming {
  [Test(
    Name = "Get Stream Uri",
    Category = Category.ProfileS,
    Id = "2",
    FeatureUnderTest = Feature.GetStreamURI
    )]
  public class GetStreamURITest : BaseTest
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

      BeginStep("<Transport> includes tag <Protocol> with (\"UDP\" OR \"HTTP\" OR \"RTSP\") parameter", protocolList);

      if (!protocolList.Any(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Protocol");
        return null != value && (value.ToUpper().Contains("UDP")  ||
                                 value.ToUpper().Contains("HTTP") ||
                                 value.ToUpper().Contains("RTSP"));
      }))
        StepFailed("Request <Transport> does not include tag <Protocol> with (\"UDP\" OR \"HTTP\" OR \"RTSP\") parameter");
      
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
    }
  }
}