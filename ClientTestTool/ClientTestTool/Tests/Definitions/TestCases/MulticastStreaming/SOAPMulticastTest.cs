///
/// @Author Matthew Tuusberg
///

﻿using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Extensions;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Data.Enums;
using ClientTestTool.Parsers.Frames.Enums;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.MulticastStreaming
{
  [Test(
    Name             = "Multicast Streaming Using SOAP",
    Category         = Category.ProfileS,
    Id               = "2",
    FeatureUnderTest = Feature.SOAPMulticast
    )]
  public class SOAPMulticastTest : BaseTest {

    protected override void ProcessConversation(Conversation conversation)
    {
      var startStreamingList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("StartMulticastStreaming"))
          .ToList();

      var stopStreamingList =
        conversation.GetMessages(ContentType.Http)
          .Where(item => item.GetRequest<HttpMessage>().ContainsTag("StopMulticastStreaming"))
          .ToList();
      
      AffectedPairs.AddRange(startStreamingList);
      AffectedPairs.AddRange(stopStreamingList);

      bool fS1Printed = false;
      bool fS1PrintedSOAP = false;
      bool fS2Printed = false;
      bool fS3Printed = false;
      bool fS4Printed = false;
      bool fS6Printed = false;
      bool fS6PrintedSOAP = false;
      bool fS7Printed = false;
      bool fS8Printed = false;
      bool fS9Printed = false;
      int nBestStep = 0;
      int nBestIdx = 0;
      List<RequestResponsePair> msgs = conversation.GetMessages(ContentType.Http);
      int idx = -1;
      while (idx + 1 < msgs.Count && (idx = msgs.FindIndex(idx + 1,
        item => ContentType.Http == item.ContentType &&
          item.GetRequest<HttpMessage>().ContainsTag(
            "StartMulticastStreaming"))) != -1)
      {
          if (!fS1Printed)
          {
            BeginStep("Client request contains <StartMulticastStreaming> tag", startStreamingList);
            StepCompleted();
            fS1Printed = true;
          }
          if (nBestStep == 0)
          {
            nBestStep = 1;
            nBestIdx = idx;
          }
          System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
          doc.LoadXml(msgs[idx].GetRequest<HttpMessage>().GetXmlString(
            XmlNamespaceOption.IgnoreNamespaces));
          System.Xml.XmlElement docElem = doc.DocumentElement;
          System.Xml.XmlNode nodeProfileToken =
            docElem.SelectSingleNode(
              "Body/StartMulticastStreaming/ProfileToken");
          if (nodeProfileToken != null)
          {
            if (!fS2Printed)
            {
              BeginStep("<StartMulticastStreaming> includes tag " +
                        "<ProfileToken>", startStreamingList);
              StepCompleted();
              fS2Printed = true;
            }
            if (nBestStep < 2)
            {
              nBestStep = 2;
              nBestIdx = idx;
            }

            if ("200" == msgs[idx].GetResponse<HttpResponse>().StatusCode)
            {
              if (!fS3Printed)
              {
                BeginStep("Device response contains \"HTTP/* 200 OK\"", startStreamingList, MessageType.Response);
                StepCompleted();
                fS3Printed = true;
              }
              if (nBestStep < 3)
              {
                nBestStep = 3;
                nBestIdx = idx;
              }
              if (msgs[idx].GetResponse<HttpMessage>().ContainsTag(
                "StartMulticastStreamingResponse"))
              {
                if (!fS4Printed)
                {
                  BeginStep(
                    "Device response contains " +
                    "<StartMulticastStreamingResponse> tag", startStreamingList, MessageType.Response);
                  StepCompleted();
                  fS4Printed = true;
                }
                if (nBestStep < 4)
                {
                  nBestStep = 4;
                  nBestIdx = idx;
                }
                if (idx + 1 < msgs.Count &&
                      ValidationStatus.Failed !=
                        msgs[idx + 1].Request.ValidationStatus)
                {

                  if (msgs[idx + 1].GetRequest<HttpMessage>().ContainsTag(
                        "StopMulticastStreaming"))
                  {
                    if (!fS6Printed)
                    {
                      BeginStep("Client request contains " +
                                  "<StopMulticastStreaming> tag", stopStreamingList);
                      StepCompleted();
                      fS6Printed = true;
                    }
                    if (nBestStep < 6)
                    {
                      nBestStep = 6;
                      nBestIdx = idx;
                    }
                    doc.LoadXml(msgs[idx + 1].GetRequest<HttpMessage>().
                      GetXmlString(XmlNamespaceOption.IgnoreNamespaces));
                    docElem = doc.DocumentElement;
                    nodeProfileToken = docElem.SelectSingleNode(
                      "Body/StopMulticastStreaming/ProfileToken");
                    if (nodeProfileToken != null)
                    {
                      if (!fS7Printed)
                      {
                        BeginStep("<StopMulticastStreaming> includes tag " +
                                  "<ProfileToken>", stopStreamingList);
                        StepCompleted();
                        fS7Printed = true;
                      }
                      if (nBestStep < 7)
                      {
                        nBestStep = 7;
                        nBestIdx = idx;
                      }

                      if ("200" == msgs[idx + 1].GetResponse<HttpResponse>().StatusCode)
                      {
                        if (!fS8Printed)
                        {
                          BeginStep("Device response contains \"HTTP/* 200 OK\"", stopStreamingList, MessageType.Response);
                          StepCompleted();
                          fS8Printed = true;
                        }
                        if (nBestStep < 8)
                        {
                          nBestStep = 8;
                          nBestIdx = idx;
                        }
                        if (msgs[idx + 1].GetResponse<HttpMessage>().ContainsTag(
                              "StopMulticastStreamingResponse"))
                        {
                          if (!fS9Printed)
                          {
                            BeginStep(
                              "Device response contains " +
                              "<StopMulticastStreamingResponse> tag", stopStreamingList, MessageType.Response);
                            StepCompleted();
                            fS9Printed = true;
                          }
                          if (nBestStep < 9)
                          {
                            nBestStep = 9;
                            nBestIdx = idx;
                          }
                          break;
                        }
                      }
                    }
                  }
                }
              }
            }
          
        }
      }
      RequestResponsePair pair = null;
      if (msgs.Count != 0)
      {
        pair = msgs[nBestIdx];
      }
      if (!fS1Printed)
      {
        BeginStep("Client request contains <StartMulticastStreaming> tag", startStreamingList);
          throw new TestNotSupportedException("Conversation does not contain requests with <StartMulticastStreaming> tag");
        StepCompleted();
      }
      if (!fS2Printed)
      {
        BeginStep("<StartMulticastStreaming> includes tag <ProfileToken>", startStreamingList);
        StepFailed("<ProfileToken> tag is missing");
        StepCompleted();
      }
      if (!fS3Printed)
      {
        BeginStep("Device response contains \"HTTP/* 200 OK\"", startStreamingList, MessageType.Response);
        StepFailed("Response does not contain 200 OK");
        StepCompleted();
      }
      if (!fS4Printed)
      {
        BeginStep("Device response contains " +
                    "<StartMulticastStreamingResponse> tag", startStreamingList, MessageType.Response);
        StepFailed("<StartMulticastStreamingResponse> tag is missing");
        StepCompleted();
      }

      if (msgs.Count != 0 && nBestIdx + 1 < msgs.Count)
      {
        pair = msgs[nBestIdx + 1];
      }
      if (!fS6Printed)
      {
        BeginStep("Client request contains " +
                    "<StopMulticastStreaming> tag", stopStreamingList);
        StepFailed("<StopMulticastStreaming> tag is missing");
        StepCompleted();
      }
      if (!fS7Printed)
      {
        BeginStep("<StopMulticastStreaming> includes tag " +
                    "<ProfileToken>", stopStreamingList);
        StepFailed("<ProfileToken> tag is missing");
        StepCompleted();
      }
      if (!fS8Printed)
      {
        BeginStep("Device response contains \"HTTP/* 200 OK\"", stopStreamingList, MessageType.Response);
        StepFailed("Response does not contain 200 OK");
        StepCompleted();
      }
      if (!fS9Printed)
      {
        BeginStep("Device response contains " +
                    "<StopMulticastStreamingResponse> tag", stopStreamingList, MessageType.Response);
        StepFailed("<StopMulticastStreamingResponse> tag is missing");
        StepCompleted();
      }
    }
  }
}