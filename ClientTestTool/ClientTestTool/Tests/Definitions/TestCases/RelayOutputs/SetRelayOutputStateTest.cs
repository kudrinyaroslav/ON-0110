///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Linq;
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

namespace ClientTestTool.Tests.Definitions.TestCases.RelayOutputs
{
    [Test(
        Name = "Set Relay Output State",
        Category = Category.Core,
        Id = "2",
        FeatureUnderTest = Feature.SetRelayOutputState
    )]
    public class SetRelayOutputStateTest : BaseTest
    {
        protected override void ProcessConversation(Conversation conversation)
        {
            var filteredList = conversation.GetMessages(ContentType.Http);

            var setRelayOutputStateList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputState")).ToList();
            AffectedPairs.AddRange(setRelayOutputStateList);

            //[S1] Client request contains “<SetRelayOutputState>” tag after the “<Body>
            BeginStep("Client request contains <SetRelayOutputState> tag", setRelayOutputStateList);

            if (0 == setRelayOutputStateList.Count)
                throw new TestNotSupportedException("Conversation does not contain messages with <SetRelayOutputState> tag");

            StepCompleted();

            //[S2] “<SetRelayOutputState>” includes tag: “<RelayOutputToken>” with non-empty string value of specific token
            var relayOutputTokenList = setRelayOutputStateList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputState", "RelayOutputToken")).ToList();
            var valuesList = relayOutputTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "RelayOutputToken")).ToList();

            BeginStep("<SetRelayOutputState> includes tag: <RelayOutputToken> with non-empty string value", relayOutputTokenList);

            if (0 == relayOutputTokenList.Count)
                StepFailed("<RelayOutputToken> tag is missing");
            else if (valuesList.All(String.IsNullOrEmpty))
                StepFailed("Value of <RelayOutputToken> tag is empty");

            StepCompleted();

            //[S3] “<SetRelayOutputState>” includes tag: “<LogicalState>” 
            var logicalStateList = setRelayOutputStateList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputState", "LogicalState")).ToList();
            AffectedPairs.AddRange(logicalStateList);

            BeginStep("<SetRelayOutputState> includes tag: <LogicalState>", logicalStateList);

            if (0 == logicalStateList.Count)
                StepFailed("<LogicalState> tag is missing");

            StepCompleted();

            //[S4] Device response contains “HTTP/* 200 OK” 
            var responseList = setRelayOutputStateList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

            BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("Response does not contain 200 OK");

            StepCompleted();

            //[S5] Device response contains “<SetRelayOutputStateResponse>” tag
            responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetRelayOutputStateResponse")).ToList();

            BeginStep("Device response contains <SetRelayOutputStateResponse> tag", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("<SetRelayOutputStateResponse> tag is missing");

            StepCompleted();
        }
    }
}
