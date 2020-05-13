///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
using System.Collections.Generic;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Http;
using ClientTestTool.Tests.Definitions.Attributes;
using ClientTestTool.Tests.Definitions.Base;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Utils;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Tests.Definitions.TestCases.DynamicDNS
{
    [Test(
      Name = "Dynamic DNS - Set Dynamic DNS",
      Id = "2",
      Category = Category.Core,
      FeatureUnderTest = Feature.SetDynamicDnsSettings
    )]
    public class SetDynamicDnsSettingsTest : BaseTest
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

            List<RequestResponsePair> setDynamicDNSList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetDynamicDNS")).ToList();
            AffectedPairs.AddRange(setDynamicDNSList);

            //[S1] Client request contains “<SetDynamicDNS>” tag after the “<Body>
            BeginStep("Client request contains <SetDynamicDNS> tag", setDynamicDNSList);

            if (0 == setDynamicDNSList.Count)
                throw new TestNotSupportedException("Conversation does not contain messages with <SetDynamicDNS> tag");

            StepCompleted();

            //[S2] “<SetDynamicDNS>” includes tag: “<Type>” with value EITHER “NoUpdate” OR “ClientUpdates” OR “ServerUpdates” 
            List<RequestResponsePair> typeList = setDynamicDNSList.Where(item =>
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetDynamicDNS", "Type")).ToList();
            AffectedPairs.AddRange(typeList);

            List<RequestResponsePair> typeValueList = typeList.Where(item =>
            {
                bool b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Type").Contains("ClientUpdates");
                if (!b)
                    b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Type").Contains("ServerUpdates");
                if (!b)
                    b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Type").Contains("NoUpdate");
                return b;
            }).ToList();

            BeginStep("“<SetDynamicDNS>” includes tag: “<Type>” with value EITHER “NoUpdate” OR “ClientUpdates” OR “ServerUpdates”", typeList);

            if (0 == typeList.Count)
                StepFailed("<Type> tag is missing");
            if (0 == typeValueList.Count)
                StepFailed("<Type> does not have “NoUpdate”, “ClientUpdates” or “ServerUpdates” value");

            StepCompleted();

            //[S3] Device response contains “HTTP/* 200 OK” 
            List<RequestResponsePair> responseList = setDynamicDNSList.Where(item =>
                "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

            BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("Response does not contain 200 OK");

            StepCompleted();

            //[S4] Device response contains “<SetDynamicDNSResponse>” tag
            responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetDynamicDNSResponse")).ToList();

            BeginStep("Device response contains <SetDynamicDNSResponse> tag", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("<SetDynamicDNSResponse> tag is missing");

            StepCompleted();
        }
    }
}

