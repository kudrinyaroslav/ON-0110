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
      Name = "Dynamic DNS - Get Dynamic DNS",
      Id = "1",
      Category = Category.Core,
      FeatureUnderTest = Feature.GetDynamicDnsSettings
    )]
    public class GetDynamicDnsSettingsTest : BaseTest
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

            List<RequestResponsePair> getDynamicDNSList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "GetDynamicDNS")).ToList();
            AffectedPairs.AddRange(getDynamicDNSList);

            //[S1] Client request contains “<GetDynamicDNS>” tag after the “<Body>
            BeginStep("Client request contains <GetDynamicDNS> tag", getDynamicDNSList);

            if (0 == getDynamicDNSList.Count)
                throw new TestNotSupportedException("Conversation does not contain messages with <GetDynamicDNS> tag");

            StepCompleted();

            //[S3] Device response contains “HTTP/* 200 OK” 
            List<RequestResponsePair> responseList = getDynamicDNSList.Where(item => 
                "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

            BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("Response does not contain 200 OK");

            StepCompleted();

            //[S4] Device response contains “<GetDynamicDNSResponse>” tag
            responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "GetDynamicDNSResponse")).ToList();

            BeginStep("Device response contains <GetDynamicDNSResponse> tag", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("<GetDynamicDNSResponse> tag is missing");

            StepCompleted();

        }
    }
}