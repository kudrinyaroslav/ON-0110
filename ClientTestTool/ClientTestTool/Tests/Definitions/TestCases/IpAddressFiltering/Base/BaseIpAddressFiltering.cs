///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Net;
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
namespace ClientTestTool.Tests.Definitions.TestCases.IpAddressFiltering.Base
{
    public abstract class BaseIpAddressFiltering : BaseTest
    {
        protected void ProcessConversation(Conversation conversation, String sIpProtocol, String sAction, short nBound)
        {
            //String sBaseIpAddressFilter = sAction + "IPAddressFilter"; 

            List<RequestResponsePair> filteredList = conversation.GetMessages(ContentType.Http);

            List<RequestResponsePair> baseIPAddressFilterList = filteredList.Where(item => TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), sAction + "IPAddressFilter")).ToList();
            AffectedPairs.AddRange(baseIPAddressFilterList);

            //[S1] Client request contains “<BaseIPAddressFilter>” tag after the “<Body>
            BeginStep("Client request contains <" + sAction + "IPAddressFilter" + "> tag", baseIPAddressFilterList);

            if (0 == baseIPAddressFilterList.Count)
                throw new TestNotSupportedException("Conversation does not contain messages with <" + sAction + "IPAddressFilter" + "> tag");

            StepCompleted();

            //[S2] “<BaseIPAddressFilter>” includes tag: “<Type>” with “Allow” OR “Deny” value
            List<RequestResponsePair> typeList = baseIPAddressFilterList.Where(item => 
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), sAction + "IPAddressFilter", "Type")).ToList();
            List<RequestResponsePair> typeAllowDenyList = typeList.Where(item => 
            {
                bool b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Type").Contains("Allow");
                if (!b)
                   b = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Type").Contains("Deny");
                return b;
            }).ToList();
                

            BeginStep(" “<" + sAction + "IPAddressFilter>” includes tag: “<Type>” with “Allow” OR “Deny” value", typeList);

            if (typeList.Count == 0)
                StepFailed("<Type> tag is missing");
            if (typeAllowDenyList.Count == 0 )
                StepFailed("<Type> tag does not have “Allow” OR “Deny” value");

            StepCompleted();

            //[S3] “<RemoveIPAddressFilter>” includes tag: “<IPv6Address>”
            List<RequestResponsePair> ipAddressList = baseIPAddressFilterList.Where(item => 
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), sAction + "IPAddressFilter", "IP" + sIpProtocol + "Address")).ToList();

            BeginStep(" “<" + sAction + "IPAddressFilter>” includes tag: “<IP" + sIpProtocol + "Address>", typeList);

            if (ipAddressList.Count == 0)
                throw new TestNotSupportedException("<IP" + sIpProtocol + "Address>" + "tag is missing");
           // StepFailed("<IP" + sIpProtocol + "Address>" + "tag is missing");

            StepCompleted();

            //[S4] “<IPv6Address>” includes tag: “<Address>” with specific IPv6 address value 
            List<RequestResponsePair> addressList = ipAddressList.Where(item => 
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "IP" + sIpProtocol + "Address", "Address")).ToList();
            List<RequestResponsePair> filteredAddressList = ipAddressList.Where(item => 
            {
                String sIpAddress = TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Address");
                //bool b = String.IsNullOrEmpty ( TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Address"));
                bool b = String.IsNullOrEmpty(sIpAddress);
                if (!b)
                {
                   IPAddress ip;// = IPAddress.Parse("127.0.0.1")
                   b = !(IPAddress.TryParse(sIpAddress.Trim(), out ip));
                }
                return !b;
            }).ToList();

            BeginStep("<IP" + sIpProtocol + "Address>" + " includes tag: <Address> with specific address value", ipAddressList);

            if (addressList.Count == 0)
                StepFailed("IP" + sIpProtocol + "Address" + " does not include <Address> tag");
            if (filteredAddressList.Count == 0)
                StepFailed("<Address> does not have specific value ");

            StepCompleted();

            //[S5]  “<IPv6Address>” includes tag: “<PrefixLength>” with value range from “0” to “128” 
            List<RequestResponsePair> prefixLengthList = ipAddressList.Where(item => 
                TestUtil.ContainsTag(item.GetRequest<HttpMessage>(), "PrefixLength")).ToList();
            filteredAddressList = prefixLengthList.Where(item => 
            {
                short n = 0;
                bool b = Int16.TryParse( TestUtil.ValueOf(item.GetRequest<HttpMessage>(), "Address"), out n) ;
                if (b)
                {
                    b = (n < nBound);
                }
                return !b;
            }).ToList();

            BeginStep("<IP" + sIpProtocol + "Address>" + " includes tag: <PrefixLength> with value range from 0 to " + nBound.ToString() , ipAddressList);

            if (addressList.Count == 0)
                StepFailed("IP" + sIpProtocol + "Address" + " does not include <PrefixLength> tag");
            if (filteredAddressList.Count == 0)
                StepFailed("Invalid value range bound");

            StepCompleted();

            //[S6] Device response contains “HTTP/* 200 OK” 
            List<RequestResponsePair> responseList = baseIPAddressFilterList.Where(item =>
                "200" == item.GetResponse<HttpResponse>().StatusCode).ToList();

            BeginStep("Device response contains 200 OK", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("Response does not contain 200 OK");

            StepCompleted();

            //[S7] Device response contains “<SetIPAddressFilterResponse>” tag
            responseList = baseIPAddressFilterList.Where(item => TestUtil.ContainsTag(item.GetResponse<HttpMessage>(), sAction + "IPAddressFilterResponse")).ToList();

            BeginStep("Device response contains “<" + sAction + "IPAddressFilterResponse>” tag", responseList, MessageType.Response);

            if (0 == responseList.Count)
                StepFailed("<" + sAction + "IPAddressFilterResponse> tag is missing");

            StepCompleted();

        }
    }
}