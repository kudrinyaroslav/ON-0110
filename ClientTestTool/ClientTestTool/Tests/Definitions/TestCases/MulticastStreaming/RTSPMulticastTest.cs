///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Parsers.Frames.Enums;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MulticastStreaming
{
  [Test(
    Name             = "Multicast Streaming Using RTSP",
    Category         = Category.ProfileS,
    Id               = "1",
    FeatureUnderTest = Feature.RTSPMulticast
  )]
  public class RTSPMulticastTest : BaseTest 
  {

    //protected override void ProcessConversation(Conversation conversation) {
    //  fS1 = false;
    //  fS2 = false;
    //  fS3 = false;
    //  fS4 = false;
    //  fS5 = false;
    //  fS6 = false;
    //  fS7 = false;
    //  fS8 = false;
    //  fS9 = false;
    //  fS10 = false;
    //  fS11 = false;
    //  fS12 = false;
    //  fS13 = false;
    //  fS14 = false;
    //  fS15 = false;
    //  fS16 = false;
    //  fS17 = false;
    //  fS18 = false;
    //  fS19 = false;
    //  fS20 = false;
    //  fS21 = false;
    //  fS22 = false;
    //  fS23 = false;
    //  fS24 = false;
    //  fS25 = false;
    //  fS26 = false;
    //  fS27 = false;
    //  fS28 = false;
    //  fS29 = false;
    //  fS30 = false;
    //  fS31 = false;
    //  fS32 = false;
    //  fS33 = false;
    //  List<RequestResponsePair> msgs = conversation.GetMessages(); // TODO filter
    //  //AffectedPairs.AddRange(msgs);
    //  int idx = -1;
    //  int idxRTSP = -1;
    //  while (idx + 1 < msgs.Count && (idx = msgs.FindIndex(idx + 1,
    //    item => ContentType.Http == item.ContentType &&
    //      item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri"))) != -1) {
    //    fS1 = true;
    //    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
    //    doc.LoadXml(msgs[idx].GetRequest<HttpMessage>().GetXmlString(
    //      XmlNamespaceOption.IgnoreNamespaces));
    //    System.Xml.XmlElement docElem = doc.DocumentElement;
    //    System.Xml.XmlNode nodeStreamSetup = docElem.SelectSingleNode(
    //      "Body/GetStreamUri/StreamSetup");
    //    if (nodeStreamSetup == null) {
    //      continue;
    //    }
    //    fS2 = true;
    //    System.Xml.XmlNode nodeStream =
    //      docElem.SelectSingleNode("Body/GetStreamUri/StreamSetup/Stream");
    //    if (nodeStream == null ||
    //      !nodeStream.InnerText.ToUpper().Contains("RTP-MULTICAST")) {
    //      continue;
    //    }
    //    fS3 = true;
    //    System.Xml.XmlNode nodeTransport = docElem.SelectSingleNode(
    //      "Body/GetStreamUri/StreamSetup/Transport");
    //    if (nodeTransport == null) {
    //      continue;
    //    }
    //    fS4 = true;
    //    System.Xml.XmlNode nodeProtocol = docElem.SelectSingleNode(
    //      "Body/GetStreamUri/StreamSetup/Transport/Protocol");
    //    if (nodeProtocol == null ||
    //      (!nodeProtocol.InnerText.ToUpper().Contains("UDP") &&
    //       !nodeProtocol.InnerText.ToUpper().Contains("HTTP") &&
    //       !nodeProtocol.InnerText.ToUpper().Contains("RTSP"))) {
    //      continue;
    //    }
    //    fS5 = true;

    //    System.Xml.XmlNode nodeProfileToken =
    //      docElem.SelectSingleNode("Body/GetStreamUri/ProfileToken");
    //    if (nodeProfileToken == null) {
    //        continue;
    //    }
    //    fS6 = true;

    //    if ("200" != msgs[idx].GetResponse<HttpResponse>().StatusCode) {
    //      continue;
    //    }
    //    fS7 = true;
    //    if (!msgs[idx].GetResponse<HttpMessage>().ContainsTag(
    //          "GetStreamUriResponse")) {
    //      continue;
    //    }
    //    fS8 = true;

    //    System.Xml.XmlDocument docResp = new System.Xml.XmlDocument();
    //    docResp.LoadXml(msgs[idx].GetResponse<HttpMessage>().GetXmlString(
    //      XmlNamespaceOption.IgnoreNamespaces));
    //    docElem = docResp.DocumentElement;
    //    System.Xml.XmlNode nodeMediaUri = 
    //      docElem.SelectSingleNode("Body/GetStreamUriResponse/MediaUri");
    //    if (nodeMediaUri == null) {
    //        continue;
    //    }
    //    fS9 = true;

    //    string sURI = "";
    //    System.Xml.XmlNode nodeUri =
    //      docElem.SelectSingleNode("Body/GetStreamUriResponse/MediaUri/Uri");
    //    if (nodeUri == null ||
    //          (sURI = nodeUri.InnerText.ToString().Trim()) == "") {
    //      continue;
    //    }
    //    fS10 = true;

    //    if (msgs.FindIndex(item => ContentType.Rtsp == 
    //                                 item.ContentType) == -1) {
    //      throw new TestNotSupportedException("Conversation " +
    //        "does not contain RTSP protocol messages such as " +
    //        "DESCRIBE, SETUP or PLAY");
    //        // TODO replace hardcoded values
    //    }
    //    idxRTSP = idx;
    //    while (idxRTSP + 1 < msgs.Count &&
    //      (idxRTSP = msgs.FindIndex(idxRTSP + 1,
    //        item => ContentType.Rtsp == item.ContentType &&
    //          RtspMethod.DESCRIBE == 
    //            item.GetRequest<RtspRequest>().Method)) != -1) {
    //      fS11 = true;
    //      string sURIDESCRIBE = msgs[idxRTSP].GetRequest<RtspRequest>().Url;
    //      // in the 
    //      // Latest Network Trace examples from Hugo\Vivotek IP8371E\Genetec –
    //      // Vivotek.pcapng
    //      // file in the DESCRIBE command
    //      // URI has form IP address:port\file.
    //      // It is rtsp://10.2.22.39:554/live4.sdp
    //      // to be precise, whereas in the <GetStreamUriResponse>
    //      // there is no :554 part.
    //      // So here we will try to get rid of port 
    //      int idx30 = sURIDESCRIBE.LastIndexOf(':');
    //      if (idx30 != -1 && idx30 != sURIDESCRIBE.IndexOf(':')) {
    //        int idx40 = idx30;
    //        while (idx40 < sURIDESCRIBE.Length &&
    //                 System.Char.IsDigit(sURIDESCRIBE[++idx40])) {
    //        }
    //        sURIDESCRIBE = sURIDESCRIBE.Remove(idx30, idx40 - idx30);
    //      }
    //      if (System.String.IsNullOrEmpty(
    //            msgs[idxRTSP].GetRequest<RtspRequest>().Url) ||
    //          (!msgs[idxRTSP].GetRequest<RtspRequest>().Url.Contains(sURI) &&
    //           !sURIDESCRIBE.Contains(sURI))) {
    //        continue;
    //      }
    //      fS12 = true;
    //      if (!msgs[idxRTSP].GetRequest<RtspMessage>().GetContent().
    //            Contains("RTSP/")) {
    //        continue;
    //      }
    //      fS13 = true;
    //      if (msgs[idxRTSP].GetRequest<RtspMessage>().CSeq == 0) {
    //        continue;
    //      }
    //      fS14 = true;
    //      if ("200" != msgs[idxRTSP].GetResponse<RtspResponse>().StatusCode) {
    //        continue;
    //      }
    //      fS15 = true;
    //      int idxSETUP = idxRTSP;          
    //      while (idxSETUP + 1 < msgs.Count &&
    //        (idxSETUP = msgs.FindIndex(idxSETUP + 1,
    //          item => ContentType.Rtsp == item.ContentType &&
    //            RtspMethod.SETUP == 
    //              item.GetRequest<RtspRequest>().Method)) != -1) {
    //        fS16 = true;
    //        if ((sURI = msgs[idxSETUP].GetRequest<RtspRequest>().Url) == "") {
    //          continue;
    //        }
    //        fS17 = true;
    //        if (!msgs[idxSETUP].GetRequest<RtspMessage>().GetContent().Contains(
    //              "RTSP/")) {
    //          continue;
    //        }
    //        fS18 = true;
    //        if (msgs[idxSETUP].GetRequest<RtspMessage>().CSeq == 0) {
    //          continue;
    //        }
    //        fS19 = true;
    //        if (!msgs[idxSETUP].GetRequest<RtspRequest>().Transport.ToUpper().
    //              Contains("RTP/AVP;MULTICAST;")) {
    //          continue;
    //        }
    //        fS20 = true;
    //        if ("200" != 
    //              msgs[idxSETUP].GetResponse<RtspResponse>().StatusCode) {
    //          continue;
    //        }
    //        fS21 = true;
    //        int idxPLAY = idxSETUP;          
    //        while (idxPLAY + 1 < msgs.Count &&
    //          (idxPLAY = msgs.FindIndex(idxPLAY + 1,
    //            item => ContentType.Rtsp == item.ContentType &&
    //              RtspMethod.PLAY ==
    //                item.GetRequest<RtspRequest>().Method)) != -1) {
    //          fS22 = true;
    //          string sURIPLAY = "";
    //          if ((sURIPLAY = msgs[idxPLAY].GetRequest<RtspRequest>().Url) == 
    //                "") {
    //            continue;
    //          }
    //          string sURI2 = sURIPLAY;
    //          // in the Hugo\Canon VB-H43\Genetec – Canon.pcapng
    //          // file in the PLAY command
    //          // URI has form IP address:port\directory.
    //          // It is rtsp://10.2.33.21:554/stream/OmniV4I1JPEGS1=m
    //          // to be precise, whereas in the SETUP
    //          // command there is no :554 part.
    //          // So here we will try to get rid of port 
    //          // (as well as '=' and after '=')
    //          int idx3 = sURIPLAY.LastIndexOf(':');
    //          if (idx3 != -1 && idx3 != sURIPLAY.IndexOf(':')) {
    //            int idx4 = idx3;
    //            while (idx4 < sURIPLAY.Length && 
    //                   System.Char.IsDigit(sURIPLAY[++idx4])) {
    //            }
    //            sURI2 = sURIPLAY.Remove(idx3, idx4 - idx3);
    //          }
    //          string sURI3 = sURI2;
    //          int idx5 = sURI2.IndexOf('=');
    //          if (idx5 != -1) {
    //            sURI3 = sURI2.Substring(0, idx5);
    //          }
    //          if (!sURIPLAY.Contains(sURI) &&
    //              !sURI.Contains(sURIPLAY) &&
    //            // in the 
    //            // Latest Network Trace examples from Hugo\Vivotek IP8371E\
    //            // Genetec – Vivotek.pcapng
    //            // file PLAY command does not include the same URI address
    //            // as in SETUP request command
    //            // So here we are trying to figure it out if 
    //            // the URIs are somewhat similar. 
    //            // Maybe vice versa SETUP request command includes the same URI
    //            // address as in the PLAY command.
    //            // It is "rtsp://10.2.22.39:554/live4.sdp/trackID=4" for SETUP 
    //            // and   "rtsp://10.2.22.39:554/live4.sdp" for PLAY
    //              !sURI3.Contains(sURI) && 
    //              !sURI.Contains(sURI3)) {
    //            // in the Hugo\Canon VB-H43\Genetec – Canon.pcapng
    //            // file PLAY command does not include the same URI address
    //            // as in SETUP request command (even without ':port'
    //            // thing)
    //            // So here we are trying to figure it out if the URIs are 
    //            // somewhat similar. 
    //            // Maybe SETUP request command includes the same URI
    //            // address as in the PLAY command.
    //            // It is rtsp://10.2.33.21/stream/OmniV4I1JPEGS1/JPEGEnc
    //            continue;
    //          }
    //          fS23 = true;
    //          if (!msgs[idxPLAY].GetRequest<RtspMessage>().GetContent().
    //                Contains("RTSP/")) {
    //            continue;
    //          }
    //          fS24 = true;
    //          if (msgs[idxPLAY].GetRequest<RtspMessage>().CSeq == 0) {
    //            continue;
    //          }
    //          fS25 = true;
    //          if (System.String.IsNullOrEmpty(
    //            msgs[idxPLAY].GetRequest<RtspRequest>().Session)) {
    //            continue;
    //          }
    //          fS26 = true;
    //          if ("200" !=
    //                msgs[idxPLAY].GetResponse<RtspResponse>().StatusCode) {
    //            continue;
    //          }
    //          fS27 = true;
    //          int idxTEARDOWN = idxPLAY;
    //          while (idxTEARDOWN + 1 < msgs.Count &&
    //            (idxTEARDOWN = msgs.FindIndex(idxTEARDOWN + 1,
    //              item => ContentType.Rtsp == item.ContentType &&
    //                RtspMethod.TEARDOWN == 
    //                  item.GetRequest<RtspRequest>().Method)) != -1) {
    //            fS28 = true;
    //            if (System.String.IsNullOrEmpty(
    //                  msgs[idxTEARDOWN].GetRequest<RtspRequest>().Url) ||
    //                !msgs[idxTEARDOWN].GetRequest<RtspRequest>().Url.Contains(
    //                  sURIPLAY)) {
    //              continue;
    //            }
    //            fS29 = true;
    //            if (!msgs[idxTEARDOWN].GetRequest<RtspMessage>().GetContent().
    //                  Contains("RTSP/")) {
    //              continue;
    //            }
    //            fS30 = true;
    //            if (msgs[idxTEARDOWN].GetRequest<RtspMessage>().CSeq == 0) {
    //              continue;
    //            }
    //            fS31 = true;
    //            if (System.String.IsNullOrEmpty(
    //                  msgs[idxTEARDOWN].GetRequest<RtspRequest>().Session)) {
    //              continue;
    //            }
    //            fS32 = true;
    //            if ("200" != msgs[idxTEARDOWN].GetResponse<RtspResponse>().
    //                  StatusCode) {
    //              continue;
    //            }
    //            fS33 = true;
    //            PrintResults(conversation);
    //            return;
    //          }
    //        }
    //      } 
    //    }
    //  }
    //  PrintResults(conversation);
    //}


    private void PrintResults(Conversation conversation)
    {

    }

    protected override void ProcessConversation(Conversation conversation)
    {
      var getStreamUriList = conversation.GetMessages(ContentType.Http)
                       .Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri"))
                       .ToList();

      AffectedPairs.AddRange(getStreamUriList);

      BeginStep("Client request contains <GetStreamUri> tag", getStreamUriList);

      if (0 == getStreamUriList.Count)
        throw new TestNotSupportedException("Conversation does not contain requests with <GetStreamUri> tag");

      StepCompleted();

      var streamSetupList = getStreamUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri", "StreamSetup"))
                                            .ToList();

      BeginStep("<GetStreamUri> includes tag <StreamSetup>", streamSetupList);
      
      if (0 == streamSetupList.Count)
        StepFailed("<StreamSetup> tag is missing");
      
      StepCompleted();

      var streamList = streamSetupList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("StreamSetup", "Stream")).ToList();

      var validStreamList = streamList.Where(item =>
                                      {
                                        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Stream");
                                        return null != value && value.ToUpper().Contains("RTP-MULTICAST");
                                      }).ToList();

      BeginStep("<StreamSetup> includes tag <Stream> with \"RTP-MULTICAST\" parameter value", validStreamList);

      if (0 == streamList.Count)
        StepFailed("<StreamSetup> does not contain tag <Stream>");
      else if (0 == validStreamList.Count)
        throw new TestNotSupportedException("<StreamSetup> does not contain tag <Stream> with \"RTP-MULTICAST\" parameter value");

      StepCompleted();

      var transportList = streamSetupList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("StreamSetup", "Transport")).ToList();

      BeginStep("<StreamSetup> includes tag <Transport>", transportList);

      if (0 == transportList.Count)
        StepFailed("Request <StreamSetup> does not include tag <Transport>");

      StepCompleted();

      var protocolList = transportList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("Transport", "Protocol"))
                                .ToList();

      var validProtocolList = protocolList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Protocol");
        return null != value && (value.ToUpper().Contains("UDP")  ||
                                 value.ToUpper().Contains("HTTP") ||
                                 value.ToUpper().Contains("RTSP"));
      }).ToList();

      BeginStep("<Transport> includes tag <Protocol> with \"UDP\" OR \"HTTP\" OR \"RTSP\" value", validProtocolList);

      if (0 == protocolList.Count)
        StepFailed("<Protocol> tag is missing");
      else if (0 == validProtocolList.Count)
        StepFailed("<Protocol> tag with valid value is missing");

      StepCompleted();

      var profileTokenList = getStreamUriList.Where(item => item.GetRequest<HttpMessage>().ContainsTag("GetStreamUri", "ProfileToken"))
                                             .ToList();
      var validProfileTokenList = profileTokenList.Where(item => !String.IsNullOrEmpty(TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "ProfileToken")))
                                                  .ToList();
      BeginStep("<GetStreamUri> includes tag <ProfileToken> with a non-empty string value", validProfileTokenList);

      if (0 == profileTokenList.Count)
        StepFailed("<ProfileToken> tag is missing");
      else if (0 == validProfileTokenList.Count)
        StepFailed("<ProfileToken> tag with non-empty value is missing");

      StepCompleted();

      var responseList = getStreamUriList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

      BeginStep("Device response contains \"HTTP/* 200 OK\"", responseList, MessageType.Response);
      
      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");
      
      StepCompleted();

      responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetStreamUriResponse")).ToList();

      BeginStep("Device response contains <GetStreamUriResponse> tag", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("<GetStreamUriResponse> tag missing");
      
      StepCompleted();

      var mediaUriList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetStreamUriResponse", "MediaUri")).ToList();

      BeginStep("<GetStreamUriResponse> includes tag <MediaUri>", mediaUriList, MessageType.Response);

      if (0 == mediaUriList.Count)
        StepFailed("Response <GetStreamUriResponse> does not include tag <MediaUri>");

      StepCompleted();

      var uriList = mediaUriList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "MediaUri", "Uri")).ToList();
      var validUriList = uriList.Where(item =>
      {
        String value = TestUtil.ValueOf(item.GetResponse<HttpMessage>(), "Uri");

        return null != value && Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute);
      }).ToList();

      BeginStep("<MediaUri> includes tag <Uri> with valid URI address", validUriList, MessageType.Response);
      
      if (0 == validUriList.Count)
        StepFailed("Response <MediaUri> does not include tag <Uri> or URI is empty");

      StepCompleted();

      var rtspList = conversation.GetMessages(ContentType.Rtsp);

      var describeList = rtspList.Where(item => RtspMethod.DESCRIBE == item.GetRequest<RtspRequest>().Method)
                                 .ToList();

      AffectedPairs.AddRange(describeList);

      BeginStep("Client request contains DESCRIBE command", describeList);

      if (0 == rtspList.Count)
        throw new TestNotSupportedException("Conversation does not contain RTSP messages");

      if (0 == describeList.Count)
        StepFailed("DESCRIBE command is missing");

      StepCompleted();

      var validDescribeList = describeList.Where(item =>
      {
        Uri describeUri;
        Uri.TryCreate(item.GetRequest<RtspRequest>().Uri, UriKind.RelativeOrAbsolute, out describeUri);

        return null != describeUri &&
               validUriList.Any(pair => pair.GetRequest<RtspRequest>().Uri.Contains(describeUri.Host));
      }).ToList();

      BeginStep("DESCRIBE includes URI obtained from GetStreamUriResponse", validDescribeList);

      if (0 == validDescribeList.Count)
        StepFailed("URI is missing");
      
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

      if (0 == describeList.Count)
        StepFailed("RTSP DESCRIBE does not include CSeq identifier");

      StepCompleted();

      responseList = describeList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device responses with code RTSP 200 OK", responseList, MessageType.Response);
      
      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      var setupList = rtspList.Where(item => RtspMethod.SETUP == item.GetRequest<RtspRequest>().Method)
                              .ToList();
      AffectedPairs.AddRange(setupList);

      BeginStep("Client request contains SETUP command", setupList);
      
      if (0 == setupList.Count)
        StepFailed("SETUP command is missing");
      
      StepCompleted();

      var setupUriList = setupList.Where(item => Uri.IsWellFormedUriString(item.GetRequest<RtspRequest>().Uri, UriKind.RelativeOrAbsolute))
                                  .ToList();

      BeginStep("SETUP includes any URI", setupUriList);
      
      if (0 == setupUriList.Count)
        StepFailed("URI is missing");

      StepCompleted();

      BeginStep("SETUP includes \"RTSP/*\" version identifier", setupList);

      bool setupIncludesRtspIdentifier = setupList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!setupIncludesRtspIdentifier)
        StepFailed("RTSP SETUP does not include RTSP/* identifier");

      StepCompleted();

      BeginStep("SETUP includes \"CSeq\" identifier", setupList);

      if (0 == setupList.Count)
        StepFailed("\"CSeq\" identifier is missing");

      StepCompleted();

      var validTransportList = setupList.Where(item => item.GetRequest<RtspRequest>().Transport.ToUpper().Contains("RTP/AVP;MULTICAST;"))
                                        .ToList();

      BeginStep("SETUP includes Transport: RTP/AVP;multicast;*\" parameter with values", validTransportList);

      if (0 == validTransportList.Count)
        StepFailed("\"Transport\" parameter is missing");

      StepCompleted();

      responseList = setupList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("SETUP: Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();

      var playList = rtspList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method)
                             .ToList();
      AffectedPairs.AddRange(playList);

      BeginStep("Client request contains PLAY command", playList);

      if (0 == playList.Count)
        StepFailed("PLAY command is missing");

      StepCompleted();

      var validPlayList = playList.Where(item =>
      {
        Uri playUri;
        Uri.TryCreate(item.GetRequest<RtspRequest>().Uri, UriKind.RelativeOrAbsolute, out playUri);

        return null != playUri &&
               setupUriList.Any(pair => pair.GetRequest<RtspRequest>().Uri.Contains(playUri.Host));
      }).ToList();

      BeginStep("PLAY includes the same URI address as in RTSP SETUP request command", validPlayList);
      
      if (0 == validPlayList.Count)
        StepFailed("URI address is not alright");

      StepCompleted();

      BeginStep("PLAY includes \"RTSP/*\" version identifier", playList);

      bool playIncludesRtspIdentifier = playList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!playIncludesRtspIdentifier)
        StepFailed("RTSP PLAY does not include RTSP/* identifier");

      StepCompleted();

      BeginStep("PLAY includes \"CSeq\" identifier", playList);

      if (0 == playList.Count)
        StepFailed("\"CSeq\" identifier is missing");

      StepCompleted();

      BeginStep("PLAY includes \"Session\" parameter", playList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("RTSP PLAY does not include Session command");

      StepCompleted();

      responseList = playList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("Device response contains RTSP/* 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Device response does not contains RTSP/* 200 OK");

      StepCompleted();

      var teardownList = rtspList.Where(item => RtspMethod.PLAY == item.GetRequest<RtspRequest>().Method)
                                 .ToList();
      AffectedPairs.AddRange(teardownList);

      BeginStep("Client invokes RTSP TEARDOWN request to terminate the RTSP session", teardownList);

      if (0 == teardownList.Count)
        StepFailed("TEARDOWN command is missing");

      StepCompleted();

      var validTeardownList = teardownList.Where(item =>
      {
        Uri teardownUri;
        Uri.TryCreate(item.GetRequest<RtspRequest>().Uri, UriKind.RelativeOrAbsolute, out teardownUri);

        return null != teardownUri &&
               validPlayList.Any(pair => pair.GetRequest<RtspRequest>().Uri.Contains(teardownUri.Host));
      }).ToList();

      BeginStep("TEARDOWN includes the same URI address as in RTSP PLAY request command", validTeardownList);

      if (0 == validTeardownList.Count)
        StepFailed("URI address is not alright");

      StepCompleted();

      BeginStep("TEARDOWN includes \"RTSP/*\" version identifier", teardownList);

      bool teardownIncludesRtspIdentifier = teardownList.Any(item =>
      {
        String requestString = item.GetRequest<RtspRequest>().Request;

        return !String.IsNullOrEmpty(requestString) && requestString.ToUpper().Contains("RTSP/");
      });

      if (!teardownIncludesRtspIdentifier)
        StepFailed("\"RTSP/*\" version identifier is missing");

      StepCompleted();

      BeginStep("TEARDOWN includes \"CSeq\" identifier", teardownList);

      if (0 == teardownList.Count)
        StepFailed("\"CSeq\" identifier is missing");

      StepCompleted();

      BeginStep("TEARDOWN includes \"Session\" parameter", teardownList);

      if (playList.All(item => String.IsNullOrEmpty(item.GetRequest<RtspRequest>().Session)))
        StepFailed("\"Session\" parameter is missing");

      StepCompleted();

      responseList = teardownList.Where(item => "200" == item.GetResponse<RtspResponse>().StatusCode).ToList();

      BeginStep("TEARDOWN: Device responses with code RTSP 200 OK", responseList, MessageType.Response);

      if (0 == responseList.Count)
        StepFailed("Response does not contain 200 OK");

      StepCompleted();
    }
  }
}