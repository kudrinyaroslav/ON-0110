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

namespace ClientTestTool.Tests.Definitions.TestCases.RelayOutputs.Base
{
    public abstract class BaseRelayOutputSettingsTest : BaseTest 
    {
        protected void ProcessConversation(Conversation conversation, String mode)  // mode - Bistable or Monostable
        {
            List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

            List<RequestResponsePair> setRelayOutputSettingsList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputSettings")).ToList();
            AffectedPairs.AddRange(setRelayOutputSettingsList);

            //[S1] Client request contains “<SetRelayOutputSettings>” tag after the “<Body>
            BeginStep("Client request contains <SetRelayOutputSettings> tag", setRelayOutputSettingsList);

            if (0 == setRelayOutputSettingsList.Count)
                throw new TestNotSupportedException("Conversation does not contain messages with <SetRelayOutputSettings> tag");

            StepCompleted();

            //[S2] “<SetRelayOutputSettings>” includes tag: “<RelayOutputToken>” with non-empty string value of specific token
            List<RequestResponsePair> relayOutputTokenList = setRelayOutputSettingsList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputSettings", "RelayOutputToken")).ToList();
            List<string> valuesList = relayOutputTokenList.Select(item => TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "RelayOutputToken")).ToList();

            BeginStep("<SetRelayOutputSettings> includes tag: <RelayOutputToken> with non-empty string value", relayOutputTokenList);

            if (0 == relayOutputTokenList.Count)
                StepFailed("<RelayOutputToken> tag is missing");
            else if (valuesList.All(String.IsNullOrEmpty))
                StepFailed("Value of <RelayOutputToken> tag is empty");

            StepCompleted();

            //[S3] “<SetRelayOutputSettings>” includes tag: “<Properties>” 
            List<RequestResponsePair> propertiesList = setRelayOutputSettingsList.Where(item =>
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "SetRelayOutputSettings", "Properties")).ToList();
            AffectedPairs.AddRange(propertiesList);

            BeginStep("<SetRelayOutputSettings> includes tag: <Properties>", propertiesList);

            if (0 == propertiesList.Count)
                StepFailed("<Properties> tag is missing");

            StepCompleted();

            //[S4]  “<Properties>” includes tag: “<Mode>” with “Bistable” or "Monostable" value
            List<RequestResponsePair> modeList = setRelayOutputSettingsList.Where(item =>
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Properties", "Mode")).ToList();
            AffectedPairs.AddRange(modeList);

            List<RequestResponsePair> xstableList = modeList.Where(item =>
            {
                return TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Mode").ToLower().Contains(mode.ToLower());
            }).ToList();

            BeginStep("“<Properties>” includes tag: “<Mode>” with “" + mode + "” value", modeList);

            if (0 == modeList.Count)
                StepFailed("<Mode> tag is missing");
            if (0 == xstableList.Count)
            {
                if (isXStable(modeList))
                    throw new TestNotSupportedException("<Mode> does not have “" + mode + "” value");
                else
                    StepFailed("<Mode> does not have “" + mode + "” value");
            }

            StepCompleted();

            //[S5]  “<Properties>” includes tag: “<DelayTime>” 
            List<RequestResponsePair> delayTimeList = setRelayOutputSettingsList.Where(item =>
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Properties", "DelayTime")).ToList();

            BeginStep("“<Properties>” includes tag: “<DelayTime>”", propertiesList);

            if (0 == delayTimeList.Count)
                StepFailed("<DelayTime> tag is missing");

            StepCompleted();

            //[S6] “<Properties>” includes tag: “<IdleState>” with “Closed” OR “Open” value 
            List<RequestResponsePair> idleStateList = setRelayOutputSettingsList.Where(item =>
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "Properties", "IdleState")).ToList();

            List<RequestResponsePair> idleStateList2 = idleStateList.Where(item =>
            {
                bool b = false;
                b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "IdleState").ToLower().Contains("closed");
                if (!b)
                    b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "IdleState").ToLower().Contains("open");
                return b;
            }).ToList();


            BeginStep("“<Properties>” includes tag: “<IdleState>” with “Closed” OR “Open” value", propertiesList);

            if (0 == idleStateList.Count)
                StepFailed("<IdleState> tag is missing");
            if (0 == idleStateList2.Count)
                StepFailed(" “<IdleState>” does not have “Closed” OR “Open” value");

            StepCompleted();

            //[S7] Device response contains “HTTP/* 200 OK” 
            List<RequestResponsePair> responseList = setRelayOutputSettingsList.Where(item => "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

            BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("Response does not contain 200 OK");

            StepCompleted();

            //[S8] Device response contains “<SetRelayOutputSettingsResponse>” tag
            responseList = responseList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), "SetRelayOutputSettingsResponse")).ToList();

            BeginStep("Device response contains <SetRelayOutputSettingsResponse> tag", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("<SetRelayOutputSettingsResponse> tag is missing");

            StepCompleted();

        }

        private bool isXStable(List<RequestResponsePair> modeList) // Bistable or monostable
        {
            bool bRet = false;
 
            List<RequestResponsePair> xstableList = modeList.Where(item =>
            {
                return TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Mode").ToLower().Contains("bistable");
            }).ToList();

            if (0 != xstableList.Count)
            {
                bRet = true;
            }
            else {
                List<RequestResponsePair> xstableList2 = modeList.Where(item =>
                {
                    return TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Mode").ToLower().Contains("monostable");
                }).ToList();

                if (0 != xstableList2.Count)
                {
                    bRet = true;
                }
            }

            return bRet;
        }

    }
}
