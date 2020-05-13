///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientTestTool.Data.Conversations;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Conversations.Messages.Http;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Soap;
using ClientTestTool.Parsers.HeaderParser;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.DoorsControl {
  [Test(
    Name = "Lock Down Release Door",
    Category = Category.ProfileC,
    Id = "7",
    RequirementLevel = RequirementLevel.Conditional,
    FeatureUnderTest = Feature.LockDownReleaseDoor
    )]
    public class LockDownReleaseDoorOperationTest : BaseTest {
    protected override void ProcessConversation(Conversation conversation) {
      List<RequestResponsePair> filteredList = conversation.GetMessages(
        ContentType.Http).Where(item => item.GetRequest<HttpMessage>().ContainsTag(
          "LockDownReleaseDoor")).ToList();
      AffectedPairs.AddRange(filteredList);

      if (0 == filteredList.Count)
        throw new TestNotSupportedException(
          "Conversation does not contain <LockDownReleaseDoor> messages");
          // TODO replace hardcoded values

      BeginStep("Client request message is a well-formed SOAP request");
      foreach (RequestResponsePair pair in filteredList)
        if (ValidationStatus.Failed == pair.Request.ValidationStatus) {
          // SOAP Validation failed == step failed
          StepFailed(pair, pair.Request.ValidationError);
          break;
        }
      StepCompleted();

      BeginStep("Client request contains \"<LockDownReleaseDoor>\" tag");
      StepCompleted();

      BeginStep("\"<LockDownReleaseDoor>\" includes tag \"<Token>\"");
      foreach (RequestResponsePair pair in filteredList) {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.LoadXml(pair.GetRequest<HttpMessage>().GetXmlString(
          XmlNamespaceOption.IgnoreNamespaces));
        System.Xml.XmlElement docElem = doc.DocumentElement;
        System.Xml.XmlNode nodeToken = docElem.SelectSingleNode(
          "Body/LockDownReleaseDoor/Token");
        if (nodeToken == null) {
          StepFailed(pair, "Request's \"<LockDownReleaseDoor>\" does not" +
                             " include tag \"<Token>\"");
          break;
        }
      }
      StepCompleted();

      BeginStep("Device response contains \"HTTP/* 200 OK\"");
      foreach (RequestResponsePair pair in filteredList) {
        if ("200" != pair.GetResponse<HttpResponse>().StatusCode) {
          StepFailed(pair, "Response does not contain 200 OK");
          break;
        }
      }
      StepCompleted();

      BeginStep("Device response contains \"<LockDownReleaseDoorResponse>\" tag");
      foreach (RequestResponsePair pair in filteredList) {
        if (!pair.GetResponse<HttpMessage>().ContainsTag(
              "LockDownReleaseDoorResponse")) {
          StepFailed(pair, "<LockDownReleaseDoorResponse> tag is absent");
          break;
        }
      }
      StepCompleted();

      BeginStep("Device response does NOT contain \"<SOAP-ENV:Fault*\" tag");
      foreach (RequestResponsePair pair in filteredList)
        if (pair.GetResponse<HttpMessage>().ContainsTag(SoapOptions.FAULT_TAG)) {
          StepFailed(pair, "SOAP fault detected");
          break;
        }
      StepCompleted();
    }
  }
}